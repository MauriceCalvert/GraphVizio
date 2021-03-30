Module _CleanupString
    Function CleanupString(ByVal s As String) As String
        Dim i As Integer
        Dim ans As String = s
        i = ans.IndexOfAny(ControlChars)
        Do While i >= 0
            ans = ans.Remove(i, 1)
            i = ans.IndexOfAny(ControlChars)
        Loop
        Return ans
    End Function
End Module
