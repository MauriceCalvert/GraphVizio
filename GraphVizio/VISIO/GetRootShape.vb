Imports Visio
Module _GetRootShape
    Function GetRootShape(ByVal Page As IVPage) As Shape
        For Each shape As Shape In Page.Shapes
            If CustomProperty(shape, "root") = "true" Then
                Return shape
            End If
        Next
        Return Nothing
    End Function
End Module
