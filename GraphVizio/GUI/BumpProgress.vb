Imports System.Windows.Forms.Application
Module _BumpProgress
    Private Delegate Sub BumpProgressInvoker(ByVal msg As String, ByVal max As Integer)
    Private Count As Integer
    Sub BumpProgress()
        Count += 1
        If ProgressWin.InvokeRequired Then
            ProgressWin.Invoke(New BumpProgressInvoker(AddressOf BumpProgress))
        Else
            ProgressWin.Progress.PerformStep()
            ProgressWin.Progress.Refresh()
            ProgressWin.BringToFront()
            ProgressWin.Refresh()
        End If
        DoEvents()
    End Sub
    Sub SetProgress(txt As String)
        ProgressWin.Status.Text = txt
        ProgressWin.BringToFront()
        ProgressWin.Status.Refresh()
        ProgressWin.Refresh()
    End Sub

End Module
