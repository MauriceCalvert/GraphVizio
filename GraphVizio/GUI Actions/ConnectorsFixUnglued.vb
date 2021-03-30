Imports Visio
Imports Visio.VisSelectArgs
Imports System.Collections.Generic
Imports Visio.VisUnitCodes
Imports Visio.VisCellIndices
Imports Visio.VisRowIndices
Imports Visio.VisSectionIndices
Imports Visio.VisRowTags
Imports Visio.VisCellVals
Module _ConnectorsFixUnglued
    Function ConnectorsFixUnglued() As String
        Dim win As Window = myVisioApp.ActiveWindow
        Dim page As IVPage = myVisioApp.ActivePage
        If win Is Nothing Or page Is Nothing Then
            Return ("No document is active")
        End If
        For Each conn As Shape In page.Shapes
            If IsConnector(conn) Then ' a 1D shape, a connector
                If conn.Connects.Count < 2 Then ' A mis-glued connector
                    Dim XCell As Cell = conn.CellsU("BeginX")
                    Dim YCell As Cell = conn.CellsU("BeginY")
                    Dim conXpos As Double = XCell.ResultIU
                    Dim conYpos As Double = YCell.ResultIU
                    Dim closest As Double = 1.0E+30
                    Dim fromshape As Shape = Nothing
                    Dim toshape As Shape = Nothing

                    ' First the "from" end
                    XCell = conn.CellsU("BeginX")
                    YCell = conn.CellsU("BeginY")
                    conXpos = XCell.ResultIU
                    conYpos = YCell.ResultIU
                    If conn.Connects.Count = 1 Then
                        fromshape = conn.Connects(1).ToCell.Shape
                    End If
                    If fromshape Is Nothing Then
                        closest = 1.0E+30
                        For Each shape As Shape In page.Shapes
                            If shape IsNot conn AndAlso _
                                Is2D(shape) Then
                                Dim distance As Double = shape.DistanceFromPoint(conXpos, conYpos, 0)
                                If distance < closest Then
                                    closest = distance
                                    fromshape = shape
                                End If
                            End If
                        Next
                    End If

                    ' Whatever we finally settled on "from", glue to it
                    If fromshape.SectionExists(CShort(visSectionConnectionPts), 1) = 0 Then
                        fromshape.AddSection(CShort(visSectionConnectionPts))
                    End If
                    If conn.SectionExists(CShort(visSectionConnectionPts), 1) = 0 Then
                        conn.AddSection(CShort(visSectionConnectionPts))
                    End If
                    If fromshape.CellExistsU("connections.centre", -1) = 0 Then
                        Dim newrow As Short = fromshape.AddNamedRow(CShort(visSectionConnectionPts), "centre", CShort(visTagCnnctNamed))
                        fromshape.CellsSRC(CShort(visSectionConnectionPts), newrow, CShort(visX)).Formula = "=width/2"
                        fromshape.CellsSRC(CShort(visSectionConnectionPts), newrow, CShort(visY)).Formula = "=height/2"
                    End If
                    XCell.GlueTo(fromshape.CellsU("connections.centre"))

                    ' Now for the "to" end
                    XCell = conn.CellsU("EndX")
                    YCell = conn.CellsU("EndY")
                    conXpos = XCell.ResultIU
                    conYpos = YCell.ResultIU
                    closest = 1.0E+30
                    For Each shape As Shape In page.Shapes
                        If shape IsNot conn AndAlso _
                            shape IsNot fromshape AndAlso _
                            Is2D(shape) Then
                            Dim distance As Double = shape.DistanceFromPoint(conXpos, conYpos, 0)
                            If distance < closest Then
                                closest = distance
                                toshape = shape
                            End If
                        End If
                    Next
                    If toshape.SectionExists(CShort(visSectionConnectionPts), 1) = 0 Then
                        toshape.AddSection(CShort(visSectionConnectionPts))
                    End If
                    If conn.SectionExists(CShort(visSectionConnectionPts), 1) = 0 Then
                        conn.AddSection(CShort(visSectionConnectionPts))
                    End If
                    If toshape.CellExistsU("connections.centre", -1) = 0 Then
                        Dim newrow As Short = toshape.AddNamedRow(CShort(visSectionConnectionPts), "centre", CShort(visTagCnnctNamed))
                        toshape.CellsSRC(CShort(visSectionConnectionPts), newrow, CShort(visX)).Formula = "=width/2"
                        toshape.CellsSRC(CShort(visSectionConnectionPts), newrow, CShort(visY)).Formula = "=height/2"
                    End If
                    XCell.GlueTo(toshape.CellsU("connections.centre"))
                End If
            End If
        Next
        Return ""
    End Function
End Module
