Imports Visio
Imports Visio.VisSelectArgs
Module _ShapesSelectUnconnected
    Function ShapesSelectUnconnected() As String
        Dim win As Window = myVisioApp.ActiveWindow
        Dim page As IVPage = myVisioApp.ActivePage
        If win Is Nothing Or page Is Nothing Then
            Return ("No document is active")
        End If
        win.SelectAll()
        For Each conn As Shape In page.Shapes
            If IsConnector(conn) Then ' a 1D shape, a connector
                win.Select(conn, CShort(visDeselect))
                For i As Integer = 1 To conn.Connects.Count
                    win.Select(conn.Connects(i).ToCell.Shape, CShort(visDeselect))
                Next
            End If
        Next
        Return ""
    End Function
End Module
