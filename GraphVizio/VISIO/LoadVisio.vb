Imports Visio
Imports Visio.VisCellIndices
Imports Visio.VisRowIndices
Imports Visio.VisSectionIndices
Imports Visio.VisUnitCodes
Imports vb = Microsoft.VisualBasic
Imports System.Globalization
Imports System.Collections.Generic

Module _LoadVisio

    Private Class Connection

        Public FromNode As Integer
        Public ToNode As Integer

        Sub New(f As Integer, t As Integer)
            FromNode = f
            ToNode = t
        End Sub
    End Class

    Private document As Document
    Dim colours As Colors

    Friend Function LoadVisio(ByVal page As IVPage) As Graph

        Dim PageSheet As Shape = page.PageSheet
        Dim PageWidth As Double = PageSheet.Cells("PageWidth").Result(visInches)
        Dim PageHeight As Double = PageSheet.Cells("PageHeight").Result(visInches)
        Dim layers As Integer = page.Layers.Count
        Dim connections As New Dictionary(Of Connection, Boolean)

        document = page.Document
        colours = document.Colors

        Dim Graph As New Graph(ROOTGRAPHNAME)

        StartProgress("Analysing diagram...", page.Shapes.Count)

        For Each shape As Shape In page.Shapes

            BumpProgress()

            If shape.CellExistsU("BeginX", CShort(True)) <> 0 Then ' a 1D shape, a connector

                If shape.Connects.Count = 2 Then ' Connector between 2 shapes

                    Dim conn As New Connection(shape.Connects(1).ToCell.Shape.ID, shape.Connects(2).ToCell.Shape.ID)
                    If connections.ContainsKey(conn) Then
                        Continue For
                    End If
                    connections.Add(conn, True)

                    Try
                        ' If either end of the connector is an arrow, this is a digraph (directed graph)
                        Dim x As Object = shape.CellsSRC(CShort(visSectionObject), CShort(visRowLine), CShort(visLineEndArrow)).ResultIU
                        If shape.CellsSRC(CShort(visSectionObject), CShort(visRowLine), CShort(visLineEndArrow)).ResultIU <> 0 Then
                            Graph.digraph = "true"
                        End If
                        If shape.CellsSRC(CShort(visSectionObject), CShort(visRowLine), CShort(visLineBeginArrow)).ResultIU <> 0 Then
                            Graph.digraph = "true"
                        End If
                    Catch ex As Exception
                    End Try

                    Dim FromShape As Shape
                    Dim ToShape As Shape
                    Dim FromNode As Node
                    Dim ToNode As Node
                    Dim subgraph As Graph = Graph

                    FromShape = shape.Connects(1).ToCell.Shape
                    ToShape = shape.Connects(2).ToCell.Shape

                    FromNode = FindShape(Graph, FromShape)
                    ToNode = FindShape(Graph, ToShape)

                    Dim edge As Edge = subgraph.Connect(FromNode, ToNode)
                    edge.Shape = shape
                    edge.SetAttribute("color", RGBfromPalette(colours, shape.Cells("LineColor").ResultInt(visUnitsColor, -1)))
                    edge.SetAttribute("fillcolor", RGBfromPalette(colours, shape.Cells("FillForegnd").ResultInt(visUnitsColor, -1)))

                    Dim sText As String = shape.Text
                    SetShapeText(edge, sText)

                End If ' connects=2

            Else ' a 2-D shape.

                If CustomProperty(shape, "BoundingBox") = "true" Then

                    Dim thisnode As Node = FindShape(Graph, shape)
                    Dim graphname As String = CustomProperty(shape, "GraphName")
                    Dim subgraph As Graph = Graph.SubGraph(graphname)

                    For Each attr As String In thisnode.Attributes.Keys
                        If Not (attr = "width" Or attr = "height" Or attr = "pos") Then
                            subgraph.GraphAttrs.Add(attr, thisnode.Attributes(attr))
                        End If
                    Next

                    Graph.RemoveNode(thisnode)

                End If

            End If

        Next

        EndProgress()

        Return Graph

    End Function

    Private Sub SetShapeText(ByVal thing As Object, ByRef text As String)
        Dim shapetext As String = text
        Dim ch As Integer
        If shapetext <> "" Then
            shapetext = shapetext.Replace(vbLf, "\n")
            shapetext = shapetext.Replace(ChrW(8232), "\n") ' &#8232; = Line Separator
            For i As Integer = 0 To shapetext.Length - 1
                ch = AscW(shapetext.Substring(i, 1))
                If ch < 32 Or ch > 255 Then
                    shapetext = shapetext.Replace(ChrW(ch), " ")
                End If
            Next
            If shapetext Is Nothing Then shapetext = "" ' replace returns nothing for empty strings
            If shapetext <> "" Then
                thing.SetAttribute("label", shapetext)
            End If
        End If
    End Sub

    Private Function FindShape(ByVal graph As Graph, ByVal Shape As Shape) As Node

        ' Find node in graph or any subgraph

        Dim node As Node = Nothing

        If graph.NodeNames.TryGetValue(CStr(Shape.ID), node) Then
            Return node
        End If

        For Each sg As Graph In graph.SubGraphs
            If sg.NodeNames.TryGetValue(CStr(Shape.ID), node) Then
                Return node
            End If
        Next

        If Shape.LayerCount > 0 AndAlso String.Compare(Shape.Layer(1).Name, ROOTGRAPHNAME, True) <> 0 Then

            Dim SubGraphName As String = Shape.Layer(1).Name
            Dim subgraph As Graph = graph.SubGraph(SubGraphName)

            node = subgraph.AddNode(CStr(Shape.ID))

            If Shape.LayerCount > 1 Then
                Warning("Shape " & Shape.Text & " is in more than 1 layer, it may appear repeatedly after layout")
            End If
            System.Diagnostics.Debug.WriteLine("Shape " & Shape.Text & " added to subgraph " & SubGraphName)

        Else

            System.Diagnostics.Debug.WriteLine("Shape " & Shape.Text & " added to main graph")
            node = graph.AddNode(CStr(Shape.ID))

        End If

        node.Shape = Shape
        node.SetAttribute("width", Convert.ToString(Shape.Cells("Width").Result(visInches), CultureInfo.InvariantCulture))
        node.SetAttribute("height", Convert.ToString(Shape.Cells("Height").Result(visInches), CultureInfo.InvariantCulture))
        node.MoveTo(Shape.Cells("PinX").Result(visInches), Shape.Cells("PinY").Result(visInches))
        node.SetAttribute("color", RGBfromPalette(colours, Shape.Cells("LineColor").ResultInt(visUnitsColor, -1)))
        node.SetAttribute("fillcolor", RGBfromPalette(colours, Shape.Cells("FillForegnd").ResultInt(visUnitsColor, -1)))

        Dim fontcolour As String = RGBfromPalette(colours, Shape.Cells("Char.Color").ResultInt(visUnitsColor, -1))

        If fontcolour <> "#000000" Then
            node.SetAttribute("fontcolor", RGBfromPalette(colours, Shape.Cells("Char.Color").ResultInt(visUnitsColor, -1)))
        End If

        Try
            Dim Fonts As Fonts
            Fonts = myVisioApp.ActiveDocument.Fonts
            Dim fontindex As Integer = Shape.CellsSRC(CShort(visSectionCharacter), CShort(visRowCharacter), CShort(visCharacterFont)).ResultInt(VisUnitCodes.visNumber, 0)
            Dim fontname As String = Fonts(fontindex).Name
            node.SetAttribute("fontname", fontname)
            Dim fontsize As Double = Shape.CellsSRC(CShort(visSectionCharacter), CShort(visRowCharacter), CShort(visCharacterSize)).Result(visPoints)
            node.SetAttribute("fontsize", fontsize.ToString(CultureInfo.InvariantCulture))

        Catch ex As Exception

        End Try
        Dim transparency As Double = Shape.Cells("FillForegndTrans").ResultIU
        If transparency <> 100 Then
            node.SetAttribute("style", "filled")
        End If
        Dim sText As String = Shape.Text
        SetShapeText(node, sText)
        DetermineShapeType(node)
        Return node
    End Function
End Module
