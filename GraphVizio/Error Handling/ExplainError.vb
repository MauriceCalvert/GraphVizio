Module _ExplainError
    Sub ExplainError(ByVal msg As String)
        Dim msgwin As New Messages
        msgwin.Display(msg, System.Drawing.Color.Red)
    End Sub
End Module
