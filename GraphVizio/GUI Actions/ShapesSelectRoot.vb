Imports Visio
Module _ShapesSelectRoot
    Function ShapesSelectRoot() As String
        Dim shape As Shape = GetRootShape(myVisioApp.ActivePage)
        If shape Is Nothing Then
            Return ("Root shape not defined, use Set root to define it")
        Else
            myVisioApp.ActiveWindow.DeselectAll()
            myVisioApp.ActiveWindow.Select(shape, CShort(tagVisSelectArgs.visSelect))
        End If
        Return ""
    End Function
End Module
