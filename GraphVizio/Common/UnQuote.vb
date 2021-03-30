Module _UnQuote
    Friend Function UnQuote(ByVal s As String) As String
        If s.StartsWith("""") Then
            s = s.Trim(""""c)
            s = s.Replace("""""", """")
        End If
        Return s
    End Function
End Module
