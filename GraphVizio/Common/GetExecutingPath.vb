Public Module GetExecutingPath_
    ''' <summary>
    ''' Get the path to the currently executing assembly
    ''' </summary>
    ''' <returns>Path, without a trailing "\"</returns>
    Public Function GetExecutingPath() As String
        ' #37#
        Dim execpath As String

        execpath = System.IO.Path.GetDirectoryName( _
            System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)

        If execpath.StartsWith("file:") Then
            execpath = execpath.Substring("file:".Length)
        End If

        Do While execpath.StartsWith("\")
            execpath = execpath.Substring(1)
        Loop

        Do While execpath.EndsWith("\")
            execpath = execpath.Substring(0, execpath.Length - 1)
        Loop

        Return execpath

    End Function

End Module
