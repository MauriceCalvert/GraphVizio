Imports Visio
Imports Visio.VisCellIndices
Module IsVisible_
    Function IsVisible(ByVal shape As Shape) As Boolean
        If shape.LayerCount = 0 Then
            Return True
        End If
        For i As Short = 1 To shape.LayerCount
            If shape.Layer(i).CellsC(CShort(visLayerVisible)).FormulaU <> "0" Then
                Return True
            End If
        Next
        Return False
    End Function
End Module
