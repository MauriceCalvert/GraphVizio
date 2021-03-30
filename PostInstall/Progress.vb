Imports System.Windows.Forms
Imports System.Windows.Forms.Application
Module _StartProgress
    Private depth As Integer = 0
    Sub EndProgress(Optional ByVal force As Boolean = False)
        depth = depth - 1
        If depth = 0 Or force Then
            If ProgressWin IsNot Nothing Then
                ProgressWin.Close()
                ProgressWin = Nothing
            End If
            depth = 0
        End If
    End Sub
    Sub StartProgress(ByVal msg As String, ByVal max As Integer)
        If ProgressWin Is Nothing OrElse depth = 0 Then
            ProgressWin = New ProgressForm
        End If
        depth = depth + 1
        ProgressWin.Status.Text = msg
        ProgressWin.Progress.Value = 0
        If max <= 0 Then
            ProgressWin.Progress.Style = ProgressBarStyle.Marquee
            ProgressWin.Progress.Maximum = 0
        Else
            ProgressWin.Progress.Style = ProgressBarStyle.Continuous
            ProgressWin.Progress.Maximum = max
        End If
        ProgressWin.Visible = True
        DoEvents()
    End Sub
    Sub BumpProgress()
        If ProgressWin.Progress.Value < ProgressWin.Progress.Maximum Then
            ProgressWin.Progress.PerformStep()
        Else
            ProgressWin.Progress.Refresh()
        End If
        DoEvents()
    End Sub
End Module
