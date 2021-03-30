Imports Visio
Imports Visio.VisUnitCodes
Imports Visio.VisCellIndices
Imports Visio.VisRowIndices
Imports Visio.VisSectionIndices
Imports Visio.VisRowTags
Imports System.Math
Imports System.Globalization
Module _DetermineShapeType
    Sub DetermineShapeType(ByVal Node As Node)
        Dim curves As Integer = 0
        Dim lines As Integer = 0
        Dim corners As Integer = 0
        Dim topcorners As Integer = 0
        Dim bottomcorners As Integer = 0
        Dim midpoints As Integer = 0
        Dim Shape As Shape = Node.Shape
        Dim shapewidth As Double = Shape.CellsU("width").Result(visInches)
        Dim shapeheight As Double = Shape.CellsU("height").Result(visInches)
        Dim type As String = "box"
        Dim gsects As Integer = Shape.GeometryCount
        Dim LastXMove As Double = -1
        Dim LastYMove As Double = -1

        shapewidth = Shape.CellsU("width").Result(visInches)
        shapeheight = Shape.CellsU("height").Result(visInches)

        For gsect As Integer = 0 To gsects - 1 ' examine geometry
            Dim cgs As Short = CShort(visSectionFirstComponent + gsect)
            Dim Rows As Short = Shape.RowCount(cgs)
            For gRow As Short = 0 To CShort(Rows - 1)
                Dim rowtype As Integer = Shape.RowType(cgs, gRow)
                Select Case rowtype
                    Case visTagEllipse
                        curves = curves + 1
                    Case visTagEllipticalArcTo
                        curves = curves + 1
                    Case visTagLineTo
                        lines = lines + 1
                        Dim X As Double = Shape.CellsSRC(cgs, gRow, CShort(visX)).Result(visInches)
                        Dim Y As Double = Shape.CellsSRC(cgs, gRow, CShort(visY)).Result(visInches)
                        If Close(X, 0) Or Close(X, shapewidth) Then
                            If Close(Y, 0) Then
                                corners = corners + 1
                                bottomcorners = bottomcorners + 1
                            ElseIf Close(Y, shapeheight) Then
                                corners = corners + 1
                                topcorners = topcorners + 1
                            ElseIf Close(Y, shapeheight / 2) Then
                                midpoints = midpoints + 1
                            End If
                        ElseIf Close(X, shapewidth / 2) Then
                            If Close(Y, 0) Or Close(Y, shapeheight) Then
                                midpoints = midpoints + 1
                            End If
                        End If
                    Case visTagInfiniteLine
                    Case visTagNURBSTo
                    Case visTagMoveTo
                        LastXMove = Shape.CellsSRC(cgs, gRow, CShort(visX)).Result(visInches) / shapewidth
                        LastYMove = Shape.CellsSRC(cgs, gRow, CShort(visY)).Result(visInches) / shapeheight
                    Case visTagPolylineTo
                        ' For a diamond : =POLYLINE(0, 0, 1,0.5, 0.5,0, 0,0.5)
                        Dim X As Double
                        Dim Y As Double
                        Dim polyline As String = Shape.CellsSRC(cgs, gRow, CShort(visPolylineData)).FormulaU
                        Dim lp As Integer = polyline.IndexOf("(")
                        Dim rp As Integer = polyline.IndexOf(")", lp)
                        If lp >= 0 And rp >= 0 Then
                            polyline = polyline.Substring(lp + 1, rp - lp - 1)
                        End If
                        Dim points As String() = polyline.Split(New [Char]() {","c})
                        points(0) = CStr(LastXMove)
                        points(1) = CStr(LastYMove)
                        lines = lines + CInt((points.GetUpperBound(0) + 1) / 2)
                        For i As Integer = 0 To points.GetUpperBound(0) - 1 Step 2
                            X = Convert.ToDouble(points(i), CultureInfo.InvariantCulture)
                            Y = Convert.ToDouble(points(i + 1), CultureInfo.InvariantCulture)
                            If Close(X, 0) Or Close(X, 1) Then
                                If Close(Y, 0) Then
                                    corners = corners + 1
                                    bottomcorners = bottomcorners + 1
                                ElseIf Close(Y, 1) Then
                                    corners = corners + 1
                                    topcorners = topcorners + 1
                                ElseIf Close(Y, 0.5) Then
                                    midpoints = midpoints + 1
                                End If
                            ElseIf Close(X, 0.5) Then
                                If Close(Y, 0) Or Close(Y, 1) Then
                                    midpoints = midpoints + 1
                                End If
                            End If
                        Next
                    Case visTagSplineBeg
                    Case visTagSplineSpan
                    Case Else
                End Select
            Next gRow
        Next gsect
        If curves > 0 Then
            If shapewidth = shapeheight Then
                type = "circle"
            Else
                type = "ellipse"
            End If
        Else
            Select Case lines
                Case 3
                    type = "triangle"
                Case 4
                    If corners = 4 Then
                        type = "box"
                    Else
                        If midpoints = 4 Then
                            type = "diamond"
                        ElseIf bottomcorners = 2 Then
                            type = "trapezium"
                        Else
                            type = "parallelogram"
                        End If
                    End If
                Case Else
                    If lines = 0 Then
                        type = "box"
                    Else
                        type = "polygon"
                        Node.SetAttribute("sides", CStr(lines))
                    End If
            End Select
        End If
        Node.SetAttribute("shape", type)
        ' debug.writeline("Shape " & Shape.Text & " has " & curves & " Curves and " & lines & " lines. It is a " & type)
    End Sub
    Private Function Close(ByVal a As Double, ByVal b As Double) As Boolean
        Return Abs(a - b) < 0.01
    End Function
End Module
