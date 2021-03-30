Imports Visio
Imports Visio.VisUnitCodes
Imports Visio.VisCellIndices
Imports Visio.VisRowIndices
Imports Visio.VisSectionIndices
Imports Visio.VisRowTags
Imports Visio.tagVisDrawSplineFlags
Imports Visio.VisCellVals
Module _CustomProperty
    Function CustomProperty(ByVal Shape As Shape, ByVal Name As String) As String
        Dim value As String = ""
        Try
            If Shape.CellExistsU("Prop." & Name & ".Value", -1) <> 0 Then
                value = Shape.Cells("Prop." & Name & ".Value").ResultStr(visUnitsString)
            End If
        Catch
        End Try
        Return value
    End Function
End Module
