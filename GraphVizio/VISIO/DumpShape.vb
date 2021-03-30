Imports System.Diagnostics
Imports Visio
Imports System.IO

Module DumpShape_
#If DEBUG Then
    Private sectionname(255) As String
    Private rowname(255) As String
    Private spaces As String = "                                                                          "
    Sub DumpShape(ByVal shp As Shape)


        For i As Integer = 0 To 255
            sectionname(i) = "UnNamedSection " & i
        Next
        sectionname(0) = "visSectionFirst"
        sectionname(1) = "visSectionObject"
        sectionname(3) = "visSectionCharacter"
        sectionname(4) = "visSectionParagraph"
        sectionname(5) = "visSectionTab"
        sectionname(6) = "visSectionScratch"
        sectionname(7) = "visSectionConnectionPts"
        sectionname(8) = "visSectionTextField"
        sectionname(9) = "visSectionControls"
        sectionname(10) = "visSectionFirstComponent"
        sectionname(239) = "visSectionLastComponent"
        sectionname(240) = "visSectionAction"
        sectionname(241) = "visSectionLayer"
        sectionname(242) = "visSectionUser"
        sectionname(243) = "visSectionProp"
        sectionname(244) = "visSectionHyperlink"
        sectionname(245) = "visSectionReviewer"
        sectionname(246) = "visSectionAnnotation"
        sectionname(247) = "visSectionSmartTag"
        sectionname(252) = "visSectionLast"
        sectionname(255) = "visSectionInval"
        sectionname(255) = "visSectionNone"

        For i As Integer = 0 To 255
            rowname(i) = "UnNamedRow " & i
        Next
        rowname(0) = "visTagDefault"
        rowname(5) = "EventGroup"
        rowname(6) = "LayerMembership"
        rowname(12) = "TextTransform"
        rowname(132) = "visTagEvents"
        rowname(133) = "visTagLineFormat"
        rowname(134) = "visTagFillFormat"
        rowname(136) = "visTagTab0"
        rowname(137) = "visTagComponent"
        rowname(138) = "visTagMoveTo"
        rowname(139) = "visTagLineTo"
        rowname(140) = "visTagArcTo"
        rowname(141) = "visTagInfiniteLine"
        rowname(143) = "visTagEllipse"
        rowname(144) = "visTagEllipticalArcTo"
        rowname(148) = "visTagCharacter"
        rowname(149) = "visTagParagraph"
        rowname(150) = "visTagTab2"
        rowname(151) = "visTagTab10"
        rowname(153) = "visTagCnnctPt"
        rowname(155) = "visTagShapeTransform"
        rowname(157) = "visTag1-D endpoints"
        rowname(162) = "visTagCtlPt"
        rowname(165) = "visTagSplineBeg"
        rowname(166) = "visTagSplineSpan"
        rowname(167) = "visTagLayerMembership"
        rowname(170) = "visTagTextPosition"
        rowname(170) = "visTagCtlPtTip"
        rowname(181) = "visTagTab60"
        rowname(185) = "visTagCnnctNamed"
        rowname(186) = "visTagCnnctPtABCD"
        rowname(187) = "visTagCnnctNamedABCD"
        rowname(193) = "visTagPolylineTo"
        rowname(195) = "visTagNURBSTo"

        DumpShape(shp, 0)
    End Sub
    Private Sub DumpShape(ByVal shp As Visio.Shape, ByVal indent As Integer)
        ShowShape(shp, indent)
        If shp.Shapes.Count > 0 Then
            If shp.Parent IsNot Nothing Then
                Debug.WriteLine(Left(spaces, indent) & "Shape " & shp.Name & "'s parent is " & shp.Parent.name)
            End If
            Debug.WriteLine(Left(spaces, indent) & "Shape " & shp.Name & " contains " & shp.Shapes.Count & " shapes")
            For Each child As Visio.Shape In shp.Shapes
                If child.ID <> shp.ID Then
                    DumpShape(child, indent + 2)
                End If
            Next
        End If
    End Sub
    Sub ShowShape(ByVal shp As Visio.Shape, ByVal indent As Integer)
        Dim thisrow As String

        ' referring to vissectionobject row zero fails
        ' referring to NameU fails for rows that don't have names
        On Error Resume Next
        Debug.WriteLine(Left(spaces, indent) & "---------------------------------------------------")
        Debug.WriteLine(Left(spaces, indent) & "Shape " & shp.Name & " ID " & shp.ID & " Type " & shp.Type & " has " & shp.GeometryCount & " geometry sections")
        Debug.WriteLine(Left(spaces, indent) & "  Text " & shp.Text & " Style " & shp.Style & " AreaIU " & shp.AreaIU & " LengthIU " & shp.LengthIU & " Charcount " & shp.CharCount)
        Debug.WriteLine(Left(spaces, indent) & "  NameID " & shp.NameID & " ObjectType " & shp.ObjectType & " OneD " & shp.OneD & " Shapes " & shp.Shapes.Count)

        For sect As Short = 1 To 255
            If shp.SectionExists(sect, 0) <> 0 AndAlso shp.RowCount(sect) > 0 Then
                Debug.WriteLine(Left(spaces, indent) & "  Section " & sect & ":" & sectionname(sect) & " has " & shp.RowCount(sect) & " rows")
                For i As Short = 0 To shp.RowCount(sect) - 1S
                    thisrow = "" ' reset as next stmt may fail
                    thisrow = shp.Section(sect).Row(i).NameU
                    thisrow = thisrow & " " & shp.RowType(sect, i) & ":" & rowname(shp.RowType(sect, i))
                    If shp.Section(sect).Row(i).Count > 0 Then
                        Debug.WriteLine(Left(spaces, indent) & "    Row " & i & ":" & thisrow & " has " & shp.Section(sect).Row(i).Count & " cells")
                        For j As Integer = 0 To shp.Section(sect).Row(i).Count - 1
                            If shp.Section(sect).Row(i).Cell(j).ResultIU <> 0 Then
                                Debug.WriteLine(Left(spaces, indent) & "      " & j & " " & shp.Section(sect).Row(i).Cell(j).Name & "=" & _
                                    shp.Section(sect).Row(i).Cell(j).ResultIU)
                            End If
                        Next
                    End If
                Next
            End If
        Next
    End Sub
#End If
End Module
