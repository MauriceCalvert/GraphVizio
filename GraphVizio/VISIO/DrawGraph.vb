Imports Visio
Imports System.Collections.Generic
Imports Visio.VisUnitCodes
Imports Visio.VisCellIndices
Imports Visio.VisRowIndices
Imports Visio.VisSectionIndices
Imports Visio.VisRowTags
Imports Visio.VisCellVals
Imports Visio.VisUICmds
Imports Visio.tagVisDrawSplineFlags
Imports Visio.VisSelectArgs
Imports vb = Microsoft.VisualBasic
Imports System
Imports System.Math
Imports System.Diagnostics
Imports System.Globalization
Module _DrawGraph
    Private ActiveWindow As Window
    Private CurrentPage As IVPage
    Private CurrentDoc As Document
    Private PageSheet As Shape
    Private PageWidth As Double
    Private PageHeight As Double
    Private Centrex As Double
    Private Centrey As Double
    Private Angle As Double
    Private AngleStep As Double
    Private LengthStep As Double
    Private Length As Double
    Private Stencil As Document
    Private Biggest As Integer
    Private Cellno As Integer = 0 ' Generate unique cell names
    Private Layer As Layer = Nothing
    Private ConnectTo As String
    Private ConnectorStyle As String
    Private ConnectorName As String
    Private MainGraphName As String
    Private Stopwatch As Stopwatch

    Function DrawGraph(ByVal doc As Document, ByVal graph As Graph, ByVal NewShapes As Boolean) As IVPage
        Stopwatch = New Stopwatch
        Stopwatch.Start()
        StartProgress($"Graph {graph.Name}: {graph.Nodes.Count} nodes, {graph.Edges.Count} edges...", graph.ItemCount)
        ActiveWindow = myVisioApp.ActiveWindow
        MainGraphName = graph.Name
        ConnectTo = CurrentSetting.Value("connectto")
        ConnectorStyle = CurrentSetting.Value("connectorstyle")
        ConnectorName = CurrentSetting.Value("connectorname")

        If doc Is Nothing Then
            CurrentDoc = myVisioApp.Documents.Add(MainGraphName)
        Else
            CurrentDoc = doc
        End If
        CurrentPage = myVisioApp.ActivePage

        If NewShapes Then
            If CurrentPage.Shapes.Count <> 0 Then
                CurrentPage = CurrentDoc.Pages.Add
            End If
            Angle = 0
            AngleStep = (2 * Math.PI) / 4
            Biggest = 1
            LengthStep = Biggest / 8
            Length = Biggest / 2
            ' ==================================================================================
            ' N.B. If we're drawing new shapes, we *MUST* draw new connectors as well, otherwise
            ' GlueTo croaks with "Inappropriate target object for this action"
            ' ==================================================================================
            If ConnectorStyle = "existing" Then
                ConnectorStyle = "" ' and GetConnector will draw a spline or straight line    
            End If
        Else
            ' Have to do this in two passes, delete messes up "for each"
            ' (Actually if shapes was a real .NET collection, the runtime would catch the error)
            Dim shapes As New List(Of Shape)
            For Each shp As Shape In CurrentPage.Shapes
                If CustomProperty(shp, "BoundingBox") = "true" Then
                    shapes.Add(shp)
                End If
            Next
            For Each shp As Shape In shapes
                shp.Delete()
            Next

            ' Delete existing connectors if need be, same remarks
            If ConnectorStyle <> "existing" Then
                Dim cons As New List(Of Shape)
                For Each shp As Shape In CurrentPage.Shapes
                    If IsConnector(shp) Then ' a 1D shape, a connector
                        cons.Add(shp)
                    End If
                Next
                For Each con As Shape In cons
                    con.Delete()
                Next
            End If
        End If
        PageSheet = CurrentPage.PageSheet
        PageWidth = PageSheet.Cells("PageWidth").Result(visInches)
        PageHeight = PageSheet.Cells("PageHeight").Result(visInches)

        ' Connector routing style=organisation chart
        If PageSheet.SectionExists(CShort(CShort(visSectionObject)), 1) = 0 Then
            PageSheet.AddSection(CShort(CShort(visSectionObject)))
        End If
        PageSheet.CellsSRC(CShort(visSectionObject),
                           CShort(visRowPageLayout),
                           CShort(visPLORouteStyle)).FormulaForceU =
                           CStr(visLORouteOrgChartNS)

        Centrex = PageWidth / 2
        Centrey = PageHeight / 2
        Dim bbs As String = ""
        If graph.GraphAttrs.ContainsKey("bb") Then
            bbs = graph.GraphAttrs("bb")
            If bbs <> "" Then
                Dim bb(3) As Double
                If ParseBoundingBox(bbs, bb) Then
                    Centrex = Centrex - ((bb(2) - bb(0))) / 2
                    Centrey = Centrey - ((bb(3) - bb(1))) / 2
                End If
            End If
        End If

        Dim stencilopen As Boolean = False
        For Each win As Window In myVisioApp.Windows
            If win.Document.Name = STENCILNAME Then
                stencilopen = True
                Exit For
            End If
        Next
        If Not stencilopen Then
            Stencil = OpenStencil()
        End If
        DrawOneGraph(graph, NewShapes)
        Progress("Bringing nodes to front...")
        BringNodesToFront(graph)

        If Not stencilopen Then
            Stencil.Close()
        End If

        If CurrentPage.Layers.Count = 1 Then ' if single layer, delete it
            CurrentPage.Layers(1).Delete(0)
        End If

        ' If drawing with Visio connectors, select them all and tell them to do their routing stuff
        Try
            If ConnectorStyle = "connectoris" AndAlso ConnectorName <> SELECTPROMPT Then
                ConnectorSelector(2)
                ActiveWindow.Selection.Layout()
            End If
        Catch
        End Try
        Progress("Resizing, please be patient...")
        Try
            ActiveWindow.DeselectAll()
            CurrentPage.ResizeToFitContents()
            ActiveWindow.Zoom = -1 ' zoom to fit
        Catch
        End Try
        EndProgress()
        Return CurrentPage
    End Function
    Sub BringNodesToFront(ByVal graph As Graph)
        For Each sg As Graph In graph.SubGraphs
            BringNodesToFront(sg)
        Next
        For Each node As Node In graph.Nodes
            If Not node.Phantom Then
                node.Shape.BringToFront()
            End If
        Next
    End Sub
    Sub DrawOneGraph(ByVal graph As Graph, ByVal NewShapes As Boolean)

        Progress($"Drawing graph {graph.Name}: {graph.Nodes.Count} nodes, {graph.Edges.Count} edges...")

        Dim bbox As Node = Nothing

        SetActiveLayer(graph.Name)

        If CurrentSetting("drawboundingboxes") = "true" AndAlso graph.Parent IsNot Nothing Then
            bbox = DrawBoundingBox(graph)
        End If

        For Each sg As Graph In graph.SubGraphs
            DrawOneGraph(sg, NewShapes)
        Next

        SetActiveLayer(graph.Name)
        DrawNodes(graph, NewShapes)

        If graph.Parent Is Nothing Then
            ActiveWindow.Zoom = -1 ' zoom to fit
        End If

        DrawConnectors(graph, NewShapes)

    End Sub
    Function DrawBoundingBox(ByVal graph As Graph) As Node
        Progress($"Drawing {graph.Name} bounding box")
        Dim tn As Node = Nothing
        Dim bbs As String = ""
        If graph.GraphAttrs.ContainsKey("bb") Then
            bbs = graph.GraphAttrs("bb")
            If bbs <> "" Then
                Dim bb(3) As Double
                If ParseBoundingBox(bbs, bb) Then
                    Dim bbox As Shape = CurrentPage.DrawRectangle(Centrex + bb(0), Centrey + bb(1), Centrex + bb(2), Centrey + bb(3))
                    Dim bgcolour As String = graph.GraphAttr("bgcolor")
                    If bgcolour <> "" Then
                        EnsureSection(bbox, CShort(visSectionObject))
                        bbox.CellsSRC(CShort(visSectionObject), CShort(visRowFill), CShort(visFillForegnd)).FormulaForceU = ColourToVisioRGB(bgcolour)
                    End If
                    tn = New Node()
                    tn.SetAttribute("bb", "true")
                    tn.Shape = bbox
                    tn.Graph = graph
                    AddCustomProperty(bbox, "BoundingBox", "true")
                    AddCustomProperty(bbox, "GraphName", tn.Graph.Name)
                    SetDisplay(tn, graph.GraphAttrs)
                    Dim bblabel As String = ""
                    If graph.GraphAttrs.ContainsKey("label") Then
                        bblabel = graph.GraphAttrs("label")
                        bbox.Text = SubstituteDOT(graph, tn, bblabel) ' Boundingbox label doesn't inherit
                        Dim lp(1) As Double
                        If graph.GraphAttrs.ContainsKey("lp") Then
                            Dim lps As String = graph.GraphAttrs.Item("lp")
                            If lps IsNot Nothing AndAlso lps <> "" Then
                                If ParseLabelPosition(lps, lp) Then
                                    Dim PinX As String = Convert.ToString(lp(0) - bb(0), CultureInfo.InvariantCulture) & " IN"
                                    Dim PinY As String = Convert.ToString(lp(1) - bb(1), CultureInfo.InvariantCulture) & " IN"
                                    bbox.CellsU("TxtPinX").FormulaForceU = PinX
                                    bbox.CellsU("TxtPinY").FormulaForceU = PinY
                                End If
                            End If
                        End If
                    End If
                    If Layer IsNot Nothing Then
                        Layer.Add(bbox, 0) ' 0=remove myShape from any current layers
                    End If
                End If
            End If
        End If
        Return tn
    End Function
    Sub DrawNodes(ByVal Graph As Graph, ByVal NewShapes As Boolean)

        Progress($"Drawing {Graph.Name}, {Graph.Nodes.Count} nodes")

        Dim myShape As Shape
        Dim xpos As Double
        Dim ypos As Double
        Dim SmartShapeWarned As Boolean = False
        Dim nodeno As Integer = 0
        Dim nodes As Integer = Graph.Nodes.Count

        For Each node As Node In Graph.Nodes
            Try
                If Not node.Phantom Then
                    BumpProgress()
                    nodeno += 1
                    Progress($"Drawing node {nodeno}/{nodes} {node.Label}")
                    Dim width As Double = -1
                    Dim height As Double = -1
                    Dim widths As String = node.InheritedAttribute("width")
                    Dim heights As String = node.InheritedAttribute("height")
                    Double.TryParse(widths, NumberStyles.Any, CultureInfo.InvariantCulture, width)
                    Double.TryParse(heights, NumberStyles.Any, CultureInfo.InvariantCulture, height)

                    xpos = node.Xpos
                    ypos = node.Ypos

                    If NewShapes Then
                        ' For graphs without position info, place shapes in a spiral from the middle
                        If xpos = 0 And ypos = 0 Then
                            Angle = Angle + AngleStep
                            Length = Length + LengthStep
                            xpos = Length * Math.Cos(Angle)
                            ypos = Length * Math.Sin(Angle)
                            LengthStep = (Biggest / 8) / Math.Sqrt(xpos * xpos + ypos * ypos)
                            AngleStep = ((2 * Math.PI) / 5) * (Biggest / Math.Sqrt(xpos * xpos + ypos * ypos))
                        End If

                        xpos = xpos + Centrex
                        ypos = ypos + Centrey

                        Dim shapename As String = node.InheritedAttribute("shape")
                        If CurrentSetting.Value("shapeis") = "true" Then
                            Dim ts As String = CurrentSetting.Value("shapename")
                            If ts <> SELECTPROMPT Then shapename = ts
                        End If

                        If shapename = "" Then
                            shapename = "ellipse" ' default shape is ellipse
                        Else
                            If shapename.EndsWith("+") Then ' shape varies by rank
                                Dim suffix As String = node.Rank.ToString
                                Dim highest As String = "0"
                                shapename = shapename.Substring(0, shapename.Length - 1)
                                For Each master As Master In Stencil.Masters
                                    If master.Name.Substring(0, master.Name.Length - 1) = shapename Then
                                        highest = master.Name.Substring(master.Name.Length - 1, 1)
                                        If highest = suffix Then
                                            Exit For
                                        End If
                                    End If
                                Next
                                shapename = shapename & highest
                            End If
                        End If

                        If shapename = "polygon" Then
                            Dim sidess As String = node.InheritedAttribute("sides")
                            Dim sides As Integer = 0
                            Integer.TryParse(sidess, sides)
                            If sides < 3 Then sides = 6
                            Dim vertex As New Polygon(sides + 1)
                            If width = 0 Then width = 1
                            If height = 0 Then height = 1
                            CreatePolygon(vertex, sides, width, height)
                            Dim vertices(sides * 2 + 1) As Double
                            For i As Integer = 0 To sides
                                vertices(i * 2) = vertex.Point(i).X
                                vertices(i * 2 + 1) = vertex.Point(i).Y
                            Next
                            myShape = CurrentPage.DrawPolyline((vertices), CShort(visPolyline1D))
                            myShape.CellsU("PinX").FormulaForceU = xpos & " IN"
                            myShape.CellsU("PinY").FormulaForceU = ypos & " IN"
                        Else
                            Dim master As Master = Nothing
                            If MasterExists(shapename) Then
                                master = Stencil.Masters(shapename)
                            Else
                                If shapename.StartsWith("M") Then
                                    If MasterExists(shapename.Substring(1)) Then ' fix Mdiamond, Msquare etc
                                        shapename = shapename.Substring(1)
                                        master = Stencil.Masters(shapename)
                                    End If
                                End If
                                If master Is Nothing Then
                                    Warning("Couldn't find shape '" & shapename &
                                        "' in Visio Stencil " & ApplicationPath() & STENCILNAME & ". Using 'box'")
                                    shapename = "box"
                                    If Not MasterExists(shapename) Then
                                        Throw New GraphVizioException("Couldn't find shape 'box' in Visio Stencil " & ApplicationPath() & STENCILNAME)
                                    End If
                                    master = Stencil.Masters(shapename)
                                End If
                            End If

                            Try
                                myShape = CurrentPage.Drop(master, xpos, ypos)
                            Catch e As Exception
                                Throw New GraphVizioException("Couldn't drop master '" & node.InheritedAttribute("Shape") & "' on page: " & e.Message)
                            End Try

                        End If

                        If Layer IsNot Nothing Then
                            Layer.Add(myShape, 0) ' 0=remove myShape from any current layers
                        End If
                        node.Shape = myShape
                        SetDisplays(Graph, node)

                    Else ' Just move existing myShape, deleting connection point section
                        myShape = CurrentPage.Shapes.ItemFromID(CInt(node.ID))
                        node.Shape = myShape
                        Unprotect(node)
                        Dim txpos As Double = node.Xpos + Centrex
                        Dim typos As Double = node.Ypos + Centrey
                        Dim PinX As String = Convert.ToString(txpos, CultureInfo.InvariantCulture) & " IN"
                        Dim PinY As String = Convert.ToString(typos, CultureInfo.InvariantCulture) & " IN"
                        Try
                            myShape.CellsU("PinX").FormulaForceU = PinX
                            myShape.CellsU("PinY").FormulaForceU = PinY
                        Catch
                        End Try
                        If Not SmartShapeWarned Then
                            If Math.Abs(myShape.CellsU("PinX").Result(visInches) - txpos) > 0.01 OrElse
                               Math.Abs(myShape.CellsU("PinY").Result(visInches) - typos) > 0.01 Then
                                SmartShapeWarned = True
                                Warning("Unable to change position of " & myShape.Text & " from " &
                                        myShape.CellsU("PinX").Result(visInches) & " " & myShape.CellsU("PinY").Result(visInches) & " to " &
                                        CStr(txpos) & " " & CStr(typos))
                                Warning("You diagram contains smart shapes that cannot be repositioned." & vbCrLf &
                                        "To fix this, choose a specific shape in the Shapes tab and re-layout")
                            End If
                        End If
                        If myShape.SectionExists(CShort(CShort(visSectionConnectionPts)), 1) <> 0 Then
                            myShape.DeleteSection(CShort(CShort(visSectionConnectionPts)))
                        End If
                    End If

                    ' Make sure myShape does have a connection-point section 
                    ' (that we just deleted if we're moving, not for new shapes)
                    If myShape.SectionExists(CShort(CShort(visSectionConnectionPts)), 1) = 0 Then
                        myShape.AddSection(CShort(CShort(visSectionConnectionPts)))
                    End If

                    Try
                        ' Adjust width and height, respecting aspect ratio of new shape
                        If width = 0 Or height = 0 Then
                            myShape.CellsU("width").FormulaForceU = "0"
                            myShape.CellsU("height").FormulaForceU = "0"
                        ElseIf width >= 0 AndAlso height >= 0 Then
                            Dim shpheight As Double = Convert.ToDouble(myShape.CellsU("height").ResultIU, CultureInfo.InvariantCulture) ' inches
                            Dim shpwidth As Double = Convert.ToDouble(myShape.CellsU("width").ResultIU, CultureInfo.InvariantCulture) ' inches
                            If Math.Abs(shpheight / shpwidth - height / width) > 0.01 Then ' aspect ratio changed, adjust to new ratio
                                height = width * shpheight / shpwidth
                            End If
                            myShape.CellsU("width").FormulaForceU = Convert.ToString(width, CultureInfo.InvariantCulture) & " IN"
                            myShape.CellsU("height").FormulaForceU = Convert.ToString(height, CultureInfo.InvariantCulture) & " IN"
                        End If
                    Catch ex As Exception
                        Warning("Unable to set size of " & myShape.Text & ": " & ex.Message)
                    End Try

                End If
            Catch ex As Exception
                Warning("Unexpected error drawing shape " & node.Attribute("label") & ": " & ex.Message)
            End Try
        Next
    End Sub
    Private Function MasterExists(ByVal name As String) As Boolean
        Dim masters() As String = Nothing
        Stencil.Masters.GetNamesU(masters)
        For Each m As String In masters
            If m = name Then
                Return True
            End If
        Next
        Return False
    End Function
    Sub DrawConnectors(ByVal Graph As Graph, ByVal NewShapes As Boolean)

        Progress($"Drawing {Graph.Name}, {Graph.Edges.Count} edges")

        Dim LayerName As String = Graph.Name
        For Each sg As Graph In Graph.SubGraphs
            DrawConnectors(sg, NewShapes)
        Next
        SetActiveLayer(LayerName)
        Dim edgeno As Integer = 0
        Dim edges As Integer = Graph.Edges.Count
        For Each edge As Edge In Graph.Edges
            If Not edge.Phantom Then
                edgeno += 1
                BumpProgress()
                Progress($"Drawing edge {edgeno}/{edges} {edge.FromNode.Label}-{edge.ToNode.Label}")
                DrawConnector(Graph, edge)
            End If
        Next
    End Sub
    Function DrawConnector(ByVal graph As Graph, ByVal edge As Edge) As Shape
        Dim FromNode As Node = edge.FromNode
        Dim ToNode As Node = edge.ToNode
        Dim FromShape As Shape = FromNode.Shape
        Dim ToShape As Shape = ToNode.Shape
        Dim FromX As Double = FromNode.Xpos
        Dim FromY As Double = FromNode.Ypos
        Dim ToX As Double = ToNode.Xpos
        Dim ToY As Double = ToNode.Ypos

        Dim connector As Shape = GetConnector(graph, edge, FromNode, ToNode)
        edge.Shape = connector
        Unprotect(edge)

        Try ' Visio can be a bit touchy when connecting things, catch unpleasantness up front

            EnsureSection(connector, CShort(visSectionObject)) ' section always necessary for connectors

            If ConnectTo = "glue" Then
                ' make dynamic glue
                connector.CellsSRC(CShort(visSectionObject), CShort(visRowMisc), CShort(visGlueType)).Formula = _
                    CStr(visGlueTypeWalking)
                Dim pin As String
                If FromNode.Xpos - ToNode.Xpos > FromNode.Ypos - ToNode.Ypos Then ' further left/right than above/below
                    pin = "PinX"
                Else
                    pin = "PinY"
                End If
                Dim bcell As Cell = connector.CellsU("BeginX")
                Dim fromcell As Cell = FromShape.CellsU(pin)
                bcell.GlueTo(fromcell)

                Dim ecell As Cell = connector.CellsU("EndX")
                Dim tocell As Cell = ToShape.CellsU(pin)
                ecell.GlueTo(tocell)

            Else ' Not glue, create connection points and glue to them

                Dim fromcellname As String
                Dim tocellname As String
                Dim newrow As Short

                ' Default from/to connection points to centre of from and to nodes (used for label positioning fraction below)
                Dim FromVisX As String = "=width/2"
                Dim ToVisX As String = "=width/2"
                Dim FromVisY As String = "=height/2"
                Dim ToVisY As String = "=height/2"

                ' Relative position of from and to nodes will determine above/below or quadrants for connections
                Dim xoffset As Double = ToNode.Xpos - FromNode.Xpos
                Dim yoffset As Double = ToNode.Ypos - FromNode.Ypos

                Cellno = Cellno + 1
                fromcellname = "c" & CStr(Cellno)
                Cellno = Cellno + 1 ' careful, if fromshape=toshape we need a different cell name
                tocellname = "c" & CStr(Cellno)
                EnsureSection(connector, CShort(visSectionConnectionPts))

                Select Case ConnectTo

                    Case "centre" ' the default

                    Case "topbottom"
                        If yoffset < 0 Then ' tonode is in bottom quadrant
                            FromVisX = "=Width/2"
                            FromVisY = "0"
                            ToVisX = "=Width/2"
                            ToVisY = "=Height"
                        Else ' tonode is in top quadrant
                            FromVisX = "=Width/2"
                            FromVisY = "=Height"
                            ToVisX = "=Width/2"
                            ToVisY = "0"
                        End If

                    Case "quadrant"
                        If Abs(yoffset) < Abs(xoffset) Then ' tonode in left or right quadrant
                            If xoffset >= 0 Then ' tonode on right, in right quadrant
                                FromVisX = "=Width"
                                FromVisY = "Height/2"
                                ToVisX = "0"
                                ToVisY = "=Height/2"
                            Else ' tonode on left, in left quadrant
                                FromVisX = "0"
                                FromVisY = "Height/2"
                                ToVisX = "=Width"
                                ToVisY = "=Height/2"
                            End If
                        ElseIf yoffset < 0 Then ' tonode in bottom quadrant
                            FromVisX = "=Width/2"
                            FromVisY = "0"
                            ToVisX = "=Width/2"
                            ToVisY = "=Height"
                        Else ' tonode in top quadrant
                            FromVisX = "=Width/2"
                            FromVisY = "=Height"
                            ToVisX = "=Width/2"
                            ToVisY = "0"
                        End If

                    Case "ideal"
                        If edge.Spline.Count > 1 Then ' Avoid problems in case of single-knotted spline (GraphViz shouldn't do that, but never know)
                            FromX = edge.Spline(0).X
                            FromY = edge.Spline(0).Y
                            ToX = edge.Spline(edge.Spline.Count - 1).X
                            ToY = edge.Spline(edge.Spline.Count - 1).Y
                            FromVisX = "=" & FromX - edge.FromNode.Xpos + edge.FromNode.Width / 2
                            FromVisY = "=" & FromY - edge.FromNode.Ypos + edge.FromNode.Height / 2
                            ToVisX = "=" & ToX - edge.ToNode.Xpos + edge.ToNode.Width / 2
                            ToVisY = "=" & ToY - edge.ToNode.Ypos + edge.ToNode.Height / 2
                        End If

                End Select

                ' Create connection points in from/to nodes
                newrow = FromShape.AddNamedRow(CShort(visSectionConnectionPts), fromcellname, CShort(visTagCnnctNamed))
                FromShape.CellsSRC(CShort(visSectionConnectionPts), newrow, CShort(visX)).Formula = FromVisX
                FromShape.CellsSRC(CShort(visSectionConnectionPts), newrow, CShort(visY)).Formula = FromVisY

                newrow = ToShape.AddNamedRow(CShort(visSectionConnectionPts), tocellname, CShort(visTagCnnctNamed))
                ToShape.CellsSRC(CShort(visSectionConnectionPts), newrow, CShort(visX)).Formula = ToVisX
                ToShape.CellsSRC(CShort(visSectionConnectionPts), newrow, CShort(visY)).Formula = ToVisY

                ' Finally, glue our connector to them
                Dim bcell As Cell = connector.CellsU("BeginX")
                Dim fromcell As Cell = FromShape.CellsU("connections." & fromcellname)
                bcell.GlueTo(fromcell)

                Dim ecell As Cell = connector.CellsU("EndX")
                Dim tocell As Cell = ToShape.CellsU("connections." & tocellname)
                ecell.GlueTo(tocell)
            End If

            ' Set connector's text and make it stay horizontal
            connector.Text = edge.Attribute("label")
            connector.CellsSRC(CShort(visSectionObject), CShort(visRowTextXForm), CShort(visXFormAngle)).FormulaForceU = "=-Angle"
            connector.CellsSRC(CShort(visSectionObject), CShort(visRowTextXForm), CShort(visXFormLocPinX)).FormulaForceU = "=TEXTWIDTH(TheText)*0.5"
            connector.CellsSRC(CShort(visSectionObject), CShort(visRowTextXForm), CShort(visXFormLocPinY)).FormulaForceU = "0 IN"
            connector.CellsSRC(CShort(visSectionObject), CShort(visRowTextXForm), CShort(visXFormWidth)).FormulaForceU = "=TEXTWIDTH(TheText)"
            connector.CellsSRC(CShort(visSectionObject), CShort(visRowTextXForm), CShort(visXFormHeight)).FormulaForceU = "=TEXTHEIGHT(TheText,Width)"

            ' Deal with label's position
            Dim lp(1) As Double
            Dim ea As String = edge.Attribute("lp")
            If ea <> "" Then
                If ParseLabelPosition(ea, lp) Then
                    Dim lx As Double = lp(0) / (ToX + FromX)
                    connector.CellsSRC(CShort(visSectionObject), CShort(visRowTextXForm), CShort(visXFormPinX)).FormulaForceU = _
                        "=Width*" & Convert.ToString(lx, CultureInfo.InvariantCulture)
                    Dim ly As Double = lp(1) / (ToY + FromY)
                    connector.CellsSRC(CShort(visSectionObject), CShort(visRowTextXForm), CShort(visXFormPinY)).FormulaForceU = _
                        "=Height*" & Convert.ToString(ly, CultureInfo.InvariantCulture)
                End If
            End If
            ' Apply default attributes
            SetDisplays(graph, edge)

            ' Put connector in proper layer
            If Layer IsNot Nothing Then
                Layer.Add(connector, 0) ' 0=remove connector from any current layers
            End If

            edge.Shape = connector

        Catch ex As Exception
            Try
                Warning("Problem drawing connector from '" & _
                    FromNode.Attribute("label") & "' (at " & Math.Round(FromX, 1) & "," & Math.Round(FromY, 1) & ") to '" & _
                    ToNode.Attribute("label") & "' (at " & Math.Round(ToX, 1) & "," & Math.Round(ToY, 1) & "): " & ex.Message & _
                    ". Check for missing connectors.")
            Catch
            End Try
        End Try
        Return connector
    End Function
    Function GetConnector(ByVal graph As Graph, ByVal edge As Edge, ByVal FromNode As Node, ByVal ToNode As Node) As Shape
        Dim Connector As Shape = Nothing
        Dim PrevConnector As Shape = Nothing
        Dim FromShape As Shape = FromNode.Shape
        Dim ToShape As Shape = ToNode.Shape

        edge.Spline = ExtractSpline(edge.Attribute("pos"))

        If ConnectorStyle = "existing" Then
            If edge.Shape IsNot Nothing Then
                Return edge.Shape
            End If
        End If

        If ConnectorStyle = "connectoris" AndAlso ConnectorName <> SELECTPROMPT Then
            Dim Master As Master
            Try
                Master = Stencil.Masters(ConnectorName)
                Connector = CurrentPage.Drop(Master, Centrex, Centrey)
            Catch e As Exception
                Warning("Connector '" & ConnectorName & "' not found in Visio Stencil " & _
                    ApplicationPath() & STENCILNAME & ": " & e.Message & ". Using straight line.")
                Connector = CurrentPage.DrawLine( _
                    FromShape.CellsU("PinX").Result(visInches), _
                    FromShape.CellsU("PinY").Result(visInches), _
                    ToShape.CellsU("PinX").Result(visInches), _
                    ToShape.CellsU("PinY").Result(visInches))
            End Try

        Else
            If edge.Spline.Count < 2 Then ' Strange. Draw between centres
                Connector = CurrentPage.DrawLine( _
                    FromShape.CellsU("PinX").Result(visInches), _
                    FromShape.CellsU("PinY").Result(visInches), _
                    ToShape.CellsU("PinX").Result(visInches), _
                    ToShape.CellsU("PinY").Result(visInches))
            ElseIf ConnectorStyle = "straight" Or edge.Spline.Count = 2 Then ' Straight line, express or implied
                Connector = CurrentPage.DrawLine( _
                    Centrex + edge.Spline(0).X, _
                    Centrey + edge.Spline(0).Y, _
                    Centrex + edge.Spline(edge.Spline.Count - 1).X, _
                    Centrey + edge.Spline(edge.Spline.Count - 1).Y)
            Else ' AH, a real spline
                Dim controlpoints As Integer = edge.Spline.Count
                Dim degree As Integer = 6
                If degree >= controlpoints Then
                    degree = controlpoints - 1
                End If
                Dim knots As Integer = controlpoints + 1
                Dim knot(controlpoints) As Double
                Dim j As Integer = 0
                For i As Integer = 0 To knots - 1
                    If i <= degree Then
                        knot(i) = 0
                    Else
                        j = j + 1
                        knot(i) = j
                    End If
                Next
                Dim xy(controlpoints * 2 - 1) As Double
                For i As Integer = 0 To edge.Spline.Count - 1
                    xy(i * 2) = edge.Spline(i).X
                    xy(i * 2 + 1) = edge.Spline(i).Y
                Next
                Connector = CurrentPage.DrawNURBS(CShort(degree), CShort(visSpline1D), (xy), (knot))
            End If
            If graph.digraph = "true" Then ' Add arrowhead
                Connector.CellsU("EndArrow").FormulaForceU = "13"
            Else
                Connector.CellsU("EndArrow").FormulaForceU = "0"
            End If

        End If
        Return Connector
    End Function
    Sub SetActiveLayer(ByVal LayerName As String)
        Layer = Nothing
        If CurrentSetting.Value("leafstack") = "all" OrElse _
            CurrentSetting.Value("leafstack") = "lowest" Then
            Exit Sub
        End If
        If LayerName = "Connector" Then
            LayerName = MainGraphName
        End If
        For i As Integer = 1 To CurrentPage.Layers.Count
            If CurrentPage.Layers(i).Name = LayerName Then
                Layer = CurrentPage.Layers(i)
                Exit For
            End If
        Next
        If Layer Is Nothing Then
            Layer = CurrentPage.Layers.Add(LayerName)
        End If
    End Sub
    Sub Unprotect(ByVal node As Node)
        Try
            Dim shape As Shape = node.Shape
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockAspect)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockBegin)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockCalcWH)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockCrop)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockDelete)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockEnd)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockFormat)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockGroup)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockHeight)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockMoveX)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockMoveY)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockRotate)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockSelect)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockTextEdit)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockVtxEdit)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockWidth)).FormulaForceU = "0"
        Catch ex As Exception
            Warning("Error unprotecting shape " & node.Attribute("label") & ":" & vbCrLf & ex.Message)
        End Try
    End Sub
    Sub Unprotect(ByVal edge As Edge)
        Try
            Dim shape As Shape = edge.Shape
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockAspect)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockBegin)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockCalcWH)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockCrop)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockDelete)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockEnd)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockFormat)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockGroup)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockHeight)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockMoveX)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockMoveY)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockRotate)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockSelect)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockTextEdit)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockVtxEdit)).FormulaForceU = "0"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLock), CShort(visLockWidth)).FormulaForceU = "0"
        Catch ex As Exception
            Dim desc As String
            desc = "connector"

            Try
                desc = desc & " from " & edge.FromNode.Attribute("label")
            Catch ex1 As Exception
                desc = desc & " from [" & ex1.Message & "?]"
            End Try

            Try
                desc = desc & " to " & edge.ToNode.Attribute("label")
            Catch ex2 As Exception
                desc = desc & "to [" & ex2.Message & "?]"
            End Try

            Warning("Error unprotecting " & desc & ":" & vbCrLf & ex.Message)
        End Try
    End Sub
    Private Sub Progress(text As String)
        If Stopwatch.ElapsedMilliseconds > 1000 Then
            SetProgress(text)
            Stopwatch.Restart()
        End If
    End Sub

End Module

