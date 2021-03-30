Imports Visio
Module IsConnector_
    Function IsConnector(ByVal conn As shape) As Boolean
        Return conn.CellExistsU("BeginX", CShort(True)) <> 0
    End Function
End Module
