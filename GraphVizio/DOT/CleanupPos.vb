Module _CleanupPos
    Function CleanupPos(ByVal s As String) As String
        If s = "" Then Return ""
        s = CleanupString(s)
        s = s.Replace("\", "") ' DOT adds "\" and ine-feeds for long lines on output
        Return s
    End Function
End Module
