Imports Visio
Imports Visio.VisCellIndices
Imports Visio.VisRowIndices
Imports Visio.VisSectionIndices
Imports System.Drawing
Imports System.Drawing.Text
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
Module _SetDisplay
    Sub SetDisplay(ByVal node As Node, ByVal attrs As Dictionary(Of String, String))

        Dim shape As Shape = node.Shape
        Dim style As String = ""
        Dim fillcolour As String = ""
        Dim fontcolour As String = ""
        Dim colour As String = ""

        If node.Graph.DefaultNodeAttrs.ContainsKey("style") Then
            style = node.Graph.DefaultNodeAttrs("style")
        End If
        If node.Graph.DefaultNodeAttrs.ContainsKey("fillcolor") Then
            fillcolour = node.Graph.DefaultNodeAttrs("fillcolor")
        End If
        If node.Graph.DefaultNodeAttrs.ContainsKey("fontcolor") Then
            fontcolour = node.Graph.DefaultNodeAttrs("fontcolor")
        End If
        If node.Graph.DefaultNodeAttrs.ContainsKey("color") Then
            colour = node.Graph.DefaultNodeAttrs("color")
        End If

        For Each kvp As KeyValuePair(Of String, String) In attrs
            Select Case kvp.Key
                Case "label"
                    shape.Text = SubstituteDOT(node.Graph, node, node.InheritedAttribute("label"))
                Case "style"
                    style = kvp.Value
                Case "color"
                    colour = kvp.Value
                Case "fillcolor"
                    fillcolour = kvp.Value
                Case "fontcolor"
                    fontcolour = kvp.Value
                Case "fontname"
                    Try ' May well fail if requested font is not installed
                        Dim FntObjs As Fonts
                        FntObjs = myVisioApp.ActiveDocument.Fonts
                        Dim fontid As Integer
                        Dim fontindex As Integer
                        If Integer.TryParse(kvp.Value, fontid) Then
                            fontindex = FntObjs.Item(fontid).Index
                        Else
                            fontindex = -1
                            For i = 1 To FntObjs.Count
                                If FntObjs.Item(i).Name = kvp.Value Then
                                    fontindex = i
                                    Exit For
                                End If
                            Next
                            If fontindex = -1 Then
                                Warning("Font " & kvp.Value & " is not available, using " & FntObjs.Item(1).Name)
                                fontindex = 0
                            End If
                        End If
                        ' Could be ID or FONTINDEX, don't know how to find out
                        EnsureSection(shape, CShort(visSectionCharacter))
                        shape.CellsSRC(CShort(visSectionCharacter), CShort(visRowCharacter), CShort(visCharacterFont)).FormulaForceU = CStr(fontindex)
                    Catch ex As Exception
                        Warning("Font " & kvp.Value & " unobtainable: " & ex.ToString)
                    End Try
                Case "fontsize"
                    Dim fontsize As String = kvp.Value
                    Dim fontsized As Double
                    If Double.TryParse(fontsize, fontsized) Then
                        fontsize = Convert.ToString(fontsize, CultureInfo.InvariantCulture) & " pt"
                    End If
                    EnsureSection(shape, CShort(visSectionCharacter))
                    shape.CellsSRC(CShort(visSectionCharacter), CShort(visRowCharacter), CShort(visCharacterSize)).FormulaForceU = fontsize
            End Select
        Next

        If CurrentSetting.Value("shapefillcolours") = "true" Then
            fillcolour = CurrentSetting.Value("shapefillcolour")
        End If
        EnsureSection(shape, CShort(visSectionObject))
        If style = "filled" Or fillcolour <> "" Then
            If fillcolour = "transparent" Then
                shape.CellsSRC(CShort(visSectionObject), CShort(visRowFill), CShort(visFillForegndTrans)).FormulaForceU = "100%"
            Else
                If fillcolour = "" Then
                    fillcolour = colour
                End If
                If fillcolour = "" Then
                    fillcolour = "lightgrey"
                End If
                shape.CellsSRC(CShort(visSectionObject), CShort(visRowFill), CShort(visFillForegnd)).FormulaForceU = ColourToVisioRGB(fillcolour)
            End If
        End If

        If CurrentSetting.Value("shapelinecolours") = "true" Then
            colour = CurrentSetting.Value("shapelinecolour")
        End If

        If colour <> "" Then
            If colour = "transparent" Then
                shape.CellsSRC(CShort(visSectionObject), CShort(visRowLine), CShort(visLineColorTrans)).FormulaForceU = "100%"
            Else
                shape.CellsSRC(CShort(visSectionObject), CShort(visRowLine), CShort(visLineColor)).FormulaForceU = ColourToVisioRGB(colour)
            End If
        End If

        If CurrentSetting.Value("shapetextcolours") = "true" Then
            fontcolour = CurrentSetting.Value("shapetextcolour")
        End If
        If fontcolour <> "" Then
            shape.CellsSRC(CShort(CShort(visSectionCharacter)), CShort(CShort(visRowCharacter)), CShort(visCharacterColor)).FormulaForceU = ColourToVisioRGB(fontcolour)
        End If

        If style = "invis" Then
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowFill), CShort(visFillForegndTrans)).FormulaForceU = "100%"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLine), CShort(visLineColorTrans)).FormulaForceU = "100%"
            shape.CellsSRC(CShort(visSectionObject), CShort(visRowCharacter), CShort(visCharacterColorTrans)).FormulaForceU = "100%"
        End If
    End Sub
    Sub SetDisplay(ByVal graph As Graph, ByVal edge As Edge, ByVal attrs As Dictionary(Of String, String))
        Dim shape As Shape = edge.Shape
        For Each kvp As KeyValuePair(Of String, String) In attrs
            Select Case kvp.Key
                Case "label"
                    shape.Text = SubstituteDOT(graph, edge, kvp.Value)
                Case "color"
                    shape.CellsSRC(CShort(visSectionObject), CShort(visRowLine), CShort(visLineColor)).FormulaForceU = ColourToVisioRGB(kvp.Value)
                Case "fontcolor"
                    shape.CellsSRC(CShort(visSectionCharacter), CShort(visRowCharacter), CShort(visCharacterColor)).FormulaForceU = ColourToVisioRGB(kvp.Value)
                Case "fontname"
                    Dim FntObjs As Fonts
                    FntObjs = myVisioApp.ActiveDocument.Fonts
                    Try
                        Dim fontid As Integer = FntObjs.Item(kvp.Value).ID
                        Dim fontindex As Integer = FntObjs.Item(kvp.Value).Index
                        ' Could be ID or FONTINDEX, don't know how to find out
                        shape.CellsSRC(CShort(visSectionCharacter), CShort(visRowCharacter), CShort(visCharacterFont)).FormulaForceU = CStr(fontindex)
                    Catch
                    End Try
                Case "fontsize"
                    Dim fontsize As String = kvp.Value
                    Dim fontsized As Double
                    If Double.TryParse(fontsize, fontsized) Then
                        fontsize = Convert.ToString(fontsize, CultureInfo.InvariantCulture) & " pt"
                    End If
                    shape.CellsSRC(CShort(visSectionCharacter), CShort(visRowCharacter), CShort(visCharacterSize)).FormulaForceU = fontsize
                Case "penwidth"
                    Dim width As Double
                    If Double.TryParse(kvp.Value, width) Then
                        shape.CellsSRC(CShort(visSectionObject), CShort(visRowLine), CShort(visLineWeight)).FormulaForceU = width.ToString & "pt"
                    End If
                Case "style"
                    Dim style As String = kvp.Value
                    Select Case style
                        Case "bold"
                            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLine), CShort(visLineWeight)).FormulaForceU = "3 pt"
                        Case "dotted"
                            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLine), CShort(visLinePattern)).FormulaForceU = 23
                        Case "dashed"
                            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLine), CShort(visLinePattern)).FormulaForceU = 16
                        Case "invis"
                            shape.CellsSRC(CShort(visSectionObject), CShort(visRowLine), CShort(visLineColorTrans)).FormulaForceU = "100%"
                    End Select
            End Select
        Next
    End Sub
End Module
