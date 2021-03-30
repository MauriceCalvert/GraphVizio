Imports Visio
Imports Visio.VisUnitCodes
Imports Visio.VisCellIndices
Imports Visio.VisRowIndices
Imports Visio.VisSectionIndices
Imports Visio.VisRowTags
Imports Visio.tagVisDrawSplineFlags
Imports Visio.VisCellVals
Module _DeleteCustomProperty
    Friend Sub DeleteCustomProperty(ByVal Shape As Shape, ByVal Name As String)
        Dim intRowIndex As Short
        If Shape.SectionExists(CShort(visSectionProp), 1) = 0 Then
            Exit Sub
        End If
        If Shape.CellExistsU("Prop." & Name & ".label", -1) <> 0 Then
            intRowIndex = Shape.CellsRowIndexU("Prop." & Name & ".label")
            Shape.DeleteRow(CShort(visSectionProp), intRowIndex)
        End If
    End Sub
End Module
