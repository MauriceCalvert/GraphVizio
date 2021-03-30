Imports Visio
Module Is2D_
    Function Is2D(ByVal shape As Shape) As Boolean
        Return shape.CellExistsU("BeginX", CShort(True)) = 0
    End Function
End Module
