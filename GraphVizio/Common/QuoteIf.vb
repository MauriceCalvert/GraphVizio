Module _QuoteIf
    Friend Function QuoteIf(ByVal s As String) As String
        If String.IsNullOrEmpty(s) OrElse s.Length = 0 Then
            Return """"""
        End If
        If s.IndexOfAny(Quotable) >= 0 OrElse "0123456789".IndexOf(s.Substring(0, 1)) >= 0 Then
            Return Quote(s)
        Else
            Return s
        End If
    End Function
End Module
