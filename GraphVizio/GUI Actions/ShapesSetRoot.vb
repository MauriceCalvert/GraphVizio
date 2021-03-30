Imports Visio
Module _ShapesSetRoot
    Function ShapesSetRoot() As String
        Dim win As Window = myVisioApp.ActiveWindow
        Dim page As IVPage = myVisioApp.ActivePage
        If win Is Nothing Or page Is Nothing Then
            Return ("No document is active")
        End If
        If win.Selection.Count = 1 Then
            Dim shape As Shape = win.Selection.Item(1)
            Dim already As Boolean = False
            If Is2D(shape) Then ' a 2-D shape
                For Each shp As Shape In page.Shapes
                    If CustomProperty(shp, "root") = "true" Then
                        DeleteCustomProperty(shp, "root")
                    End If
                Next
                AddCustomProperty(shape, "root", "true")
            Else
                Return ("Root must a shape, not a connector")
            End If
        Else
            Return ("Select exactly one shape before setting root")
        End If
        Return ""
    End Function
End Module
