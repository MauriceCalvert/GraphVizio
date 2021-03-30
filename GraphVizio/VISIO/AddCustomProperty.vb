Imports Visio
Imports Visio.VisUnitCodes
Imports Visio.VisCellIndices
Imports Visio.VisRowIndices
Imports Visio.VisSectionIndices
Imports Visio.VisRowTags
Imports Visio.tagVisDrawSplineFlags
Imports Visio.VisCellVals
Module _AddCustomProperty
    Friend Sub AddCustomProperty(ByVal Shape As Shape, _
                        ByVal Name As String, _
                        ByVal Value As String)
        Dim intRowIndex As Integer
        If Shape.SectionExists(CShort(CShort(visSectionProp)), 1) = 0 Then
            Shape.AddSection(CShort(CShort(visSectionProp)))
        End If
        If Shape.CellExistsU("Prop." & Name & ".label", -1) <> 0 Then
            intRowIndex = Shape.CellsRowIndexU("Prop." & Name & ".label")
        Else
            intRowIndex = Shape.AddNamedRow(CShort(CShort(visSectionProp)), Name, CShort(VisRowIndices.visRowProp))
            Shape.CellsSRC(CShort(visSectionProp), CShort(visRowProp + intRowIndex), CShort(visCustPropsPrompt)).FormulaForceU = quote(Name)
            Shape.CellsSRC(CShort(visSectionProp), CShort(visRowProp + intRowIndex), CShort(visCustPropsPrompt)).RowNameU = Name
            Shape.CellsSRC(CShort(visSectionProp), CShort(visRowProp + intRowIndex), CShort(visCustPropsLabel)).FormulaForceU = quote(Name)
            Shape.CellsSRC(CShort(visSectionProp), CShort(visRowProp + intRowIndex), CShort(visCustPropsType)).FormulaForceU = CStr(visPropTypeString)
        End If
        Shape.CellsSRC(CShort(visSectionProp), CShort(visRowProp + intRowIndex), CShort(visCustPropsValue)).FormulaForceU = quote(Value)
    End Sub
End Module
