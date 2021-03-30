Module _UniqueName
    Private unique As Integer = 0
    Function UniqueName(ByVal prefix As String) As String
        unique = unique + 1
        Return prefix & CStr(unique)
    End Function
End Module
