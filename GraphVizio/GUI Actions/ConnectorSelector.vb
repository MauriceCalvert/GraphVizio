Imports Visio
Imports Visio.VisSelectArgs
Module _ConnectorSelector
    Function ConnectorSelector0() As String
        Return ConnectorSelector(0)
    End Function
    Function ConnectorSelector1() As String
        Return ConnectorSelector(1)
    End Function
    Function ConnectorSelector2() As String
        Return ConnectorSelector(2)
    End Function
    Function ConnectorSelector(ByVal connections As Integer) As String
        Dim win As Window = myVisioApp.ActiveWindow
        Dim page As IVPage = myVisioApp.ActivePage
        If win Is Nothing Or page Is Nothing Then
            Return ("No document is active")
        End If
        For Each conn As Shape In page.Shapes
            If IsConnector(conn) AndAlso IsVisible(conn) Then ' a visible 1D shape, a connector
                If conn.Connects.Count = connections Then
                    win.Select(conn, CShort(visSelect))
                End If
            End If
        Next
        Return ""
    End Function
End Module
