Imports System
Friend Class GraphVizioException
    Inherits Exception

    Friend Sub New()
    End Sub 'New

    Friend Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub 'New

    Friend Sub New(ByVal message As String, ByVal inner As Exception)
        MyBase.New(message, inner)
    End Sub 'New
End Class

