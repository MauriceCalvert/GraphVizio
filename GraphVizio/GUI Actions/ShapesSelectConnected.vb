Imports Visio
Imports Visio.VisSelectArgs
Imports Visio.VisCellIndices
Module _ShapesSelectConnected
    Function ShapesSelectConnected() As String
        Dim win As Visio.Window = myVisioApp.ActiveWindow
        Dim page As IVPage = myVisioApp.ActivePage
        If win Is Nothing Or page Is Nothing Then
            Return ("No document is active")
        End If
        For Each conn As Shape In page.Shapes
            If IsConnector(conn) Then ' a 1D shape, a connector
                If conn.Connects.Count = 2 AndAlso IsVisible(conn) Then ' Visible connector between 2 shapes
                    If IsVisible(conn.Connects(1).ToCell.Shape) Then
                        win.Select(conn.Connects(1).ToCell.Shape, CShort(visSelect))
                    End If
                    If IsVisible(conn.Connects(2).ToCell.Shape) Then
                        win.Select(conn.Connects(2).ToCell.Shape, CShort(visSelect))
                    End If
                End If
            End If
        Next
        Return ""
    End Function
End Module
