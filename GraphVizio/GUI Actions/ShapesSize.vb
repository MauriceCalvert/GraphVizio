Imports Visio
Imports Visio.VisSectionIndices
Imports Visio.VisrowIndices
Imports Visio.VisRowTags
Imports System.Globalization
Module _ShapesSize
    Function ShapesSizeWider() As String
        Return ShapesSize("wider")
    End Function
    Function ShapesSizeNarrower() As String
        Return ShapesSize("narrower")
    End Function
    Function ShapesSizeTaller() As String
        Return ShapesSize("taller")
    End Function
    Function ShapesSizeShorter() As String
        Return ShapesSize("shorter")
    End Function
    Function ShapesSize(ByVal dimension As String) As String
        Dim win As Window = myVisioApp.ActiveWindow
        Dim page As IVPage = myVisioApp.ActivePage
        Dim amount As Integer = My.Settings.ResizeAmount
        Dim all As Boolean = False

        If win Is Nothing Or page Is Nothing Then
            Return ("No document is active")
        End If

        Dim selection As Selection = win.Selection
        If selection.Count = 0 Then
            selection.SelectAll()
            all = True
        End If
        StartProgress("Resizing...", selection.Count)
        For Each shape As Shape In selection
            BumpProgress()
            If Is2D(shape) Then ' a 2D shape
                Select Case dimension
                    Case "wider"
                        shape.CellsU("width").FormulaForceU = Convert.ToString(( _
                                Convert.ToDouble(shape.CellsU("width").ResultIU, CultureInfo.InvariantCulture) _
                                * (100 + amount)) / 100, _
                            CultureInfo.InvariantCulture)
                    Case "narrower"
                        shape.CellsU("width").FormulaForceU = Convert.ToString(( _
                                Convert.ToDouble(shape.CellsU("width").ResultIU, CultureInfo.InvariantCulture) _
                                * (100 - amount)) / 100, _
                            CultureInfo.InvariantCulture)
                    Case "taller"
                        shape.CellsU("height").FormulaForceU = Convert.ToString(( _
                                Convert.ToDouble(shape.CellsU("height").ResultIU, CultureInfo.InvariantCulture) _
                                * (100 + amount)) / 100, _
                            CultureInfo.InvariantCulture)
                    Case "shorter"
                        shape.CellsU("height").FormulaForceU = Convert.ToString(( _
                                Convert.ToDouble(shape.CellsU("height").ResultIU, CultureInfo.InvariantCulture) _
                                * (100 - amount)) / 100, _
                            CultureInfo.InvariantCulture)
                End Select
            End If
        Next
        EndProgress()
        If all Then win.DeselectAll()
        Return ""
    End Function
    Function ShapesSizeToFit() As String
        Dim win As Window = myVisioApp.ActiveWindow
        Dim page As IVPage = myVisioApp.ActivePage
        Dim all As Boolean = False
        If win Is Nothing Or page Is Nothing Then
            Return ("No document is active")
        End If
        Dim selection As Selection = win.Selection
        If selection.Count = 0 Then
            selection.SelectAll()
            all = True
        End If
        StartProgress("Resizing...", selection.Count)
        For Each shape As Shape In selection
            BumpProgress()
            If Is2D(shape) Then ' a 2D shape
                If shape.RowExists(CShort(visSectionObject), CShort(visRowTextXForm), 1) = 0 Then
                    shape.AddRow(CShort(visSectionObject), CShort(visRowTextXForm), 0)
                End If
                shape.CellsU("TxtWidth").FormulaForceU = "=TEXTWIDTH(TheText)"
                shape.CellsU("TxtHeight").FormulaForceU = "=TEXTHEIGHT(TheText,TxtWidth)"
                Dim width As Double = shape.CellsU("TxtWidth").ResultIU
                Dim height As Double = shape.CellsU("TxtHeight").ResultIU
                SizeShape(shape, width, height)
            End If
        Next
        EndProgress()
        If all Then win.DeselectAll()
        Return ""
    End Function
    Function ShapesSizeToWidest() As String
        Dim win As Window = myVisioApp.ActiveWindow
        Dim page As IVPage = myVisioApp.ActivePage
        Dim all As Boolean = False
        If win Is Nothing Or page Is Nothing Then
            Return ("No document is active")
        End If
        Dim selection As Selection = win.Selection
        If selection.Count = 0 Then
            selection.SelectAll()
            all = True
        End If
        Dim maxwidth As Double = 0
        Dim maxheight As Double = 0
        StartProgress("Resizing...", selection.Count * 2)
        For Each shape As Shape In selection
            BumpProgress()
            If Is2D(shape) Then ' a 2D shape
                If shape.RowExists(CShort(visSectionObject), CShort(visRowTextXForm), 1) = 0 Then
                    shape.AddRow(CShort(visSectionObject), CShort(visRowTextXForm), 0)
                End If
                shape.CellsU("TxtWidth").FormulaForceU = "=TEXTWIDTH(TheText)"
                shape.CellsU("TxtHeight").FormulaForceU = "=TEXTHEIGHT(TheText,TxtWidth)"
                Dim width As Double = shape.CellsU("TxtWidth").ResultIU
                Dim height As Double = shape.CellsU("TxtHeight").ResultIU
                If width > maxwidth Then maxwidth = width
                If height > maxheight Then maxheight = height
            End If
        Next
        Dim maxwidths As String = CStr(maxwidth)
        Dim maxheights As String = CStr(maxheight)
        For Each shape As Shape In selection
            BumpProgress()
            If Is2D(shape) Then ' a 2D shape
                SizeShape(shape, maxwidth, maxheight)
            End If
        Next
        EndProgress()
        If all Then win.DeselectAll()
        Return ""
    End Function
    Private Sub SizeShape(ByVal shape As Shape, ByVal width As Double, ByVal height As Double)
        Dim oldwidth As Double = shape.CellsU("Width").ResultIU
        Dim oldheight As Double = shape.CellsU("Height").ResultIU
        Dim oldvalue As Double
        Dim cellname As String

        shape.CellsU("Width").FormulaForceU = Convert.ToString(width, CultureInfo.InvariantCulture)
        shape.CellsU("Height").FormulaForceU = Convert.ToString(width, CultureInfo.InvariantCulture)

        If shape.SectionExists(CShort(visSectionConnectionPts), 1) <> 0 Then

            For i As Short = 0 To shape.RowCount(CShort(visSectionConnectionPts)) - 1S

                For j As Integer = 0 To shape.Section(CShort(visSectionConnectionPts)).Row(i).Count - 1

                    cellname = shape.Section(CShort(visSectionConnectionPts)).Row(i).Cell(j).Name

                    If cellname.EndsWith(".X") Then
                        oldvalue = shape.Section(CShort(visSectionConnectionPts)).Row(i).Cell(j).ResultIU

                        shape.Section(CShort(visSectionConnectionPts)).Row(i).Cell(j).FormulaForceU = _
                            Convert.ToString(oldvalue * width / oldwidth, CultureInfo.InvariantCulture)

                    ElseIf cellname.EndsWith(".Y") Then
                        oldvalue = shape.Section(CShort(visSectionConnectionPts)).Row(i).Cell(j).ResultIU

                        shape.Section(CShort(visSectionConnectionPts)).Row(i).Cell(j).FormulaForceU = _
                            Convert.ToString(oldvalue * height / oldheight, CultureInfo.InvariantCulture)

                    End If
                Next
            Next
        End If
    End Sub
End Module
