Module _Warning
    Sub Warning(ByVal msg As String)
        Try
            If Warnings.ContainsKey(msg) Then
                Exit Sub
            End If
            Warnings.Add(msg, 0)
            If MsgWin Is Nothing Then
                MsgWin = New Messages
            End If
            MsgWin.Display(msg, Drawing.Color.Black)
        Catch ex As Exception
            MsgBox("Unable to display message window: " & ex.Message, MsgBoxStyle.Critical)
            MsgBox(msg, MsgBoxStyle.Exclamation)
        End Try
    End Sub
End Module
