Module _Quote
    Friend Function Quote(ByVal s As String) As String
        Quote = """" & Replace(s, """", """""") & """"
    End Function
End Module
