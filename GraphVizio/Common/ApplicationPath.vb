Module _ApplicationPath
    Function ApplicationPath() As String
        Dim path As String = System.IO.Path.GetDirectoryName( _
            System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)
        If path.Substring(0, 5) = "file:" Then
            path = path.Substring(5)
        End If
        Do While path.StartsWith("\")
            path = path.Substring(1)
        Loop
        If Not path.EndsWith("\") Then
            path = path & "\"
        End If
        Return path
    End Function
End Module
