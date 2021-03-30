Imports System.Diagnostics
Module Notepad_
    Sub Notepad(ByVal file As String)
        Dim startInfo As ProcessStartInfo
        startInfo = New ProcessStartInfo("notepad.exe")
        startInfo.Arguments = file
        startInfo.WindowStyle = ProcessWindowStyle.Normal
        Process.Start(startInfo)
    End Sub
End Module
