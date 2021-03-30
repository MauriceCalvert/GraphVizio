Imports System.Windows.Forms
Imports System.Windows.Forms.Application
Imports System.Threading
Module _StartProgress
    Private depth As Integer = 0
    Sub EndProgress(Optional ByVal force As Boolean = False)
        Monitor.Enter(ProgressLock)
        depth = depth - 1
        If depth = 0 Or force Then
            If ProgressWin IsNot Nothing Then
                ProgressWin.Close()
            End If
            depth = 0
        End If
        Monitor.Exit(ProgressLock)
    End Sub
    Private Delegate Sub StartProgressInvoker(ByVal msg As String, ByVal max As Integer)
    Sub StartProgress(ByVal msg As String, ByVal max As Integer)
        Monitor.Enter(ProgressLock)
        If ProgressWin IsNot Nothing AndAlso ProgressWin.InvokeRequired Then
            ProgressWin.Invoke(New StartProgressInvoker(AddressOf StartProgress), msg, max)
        Else
            If ProgressWin Is Nothing OrElse depth = 0 Then
                ProgressWin = New ProgressForm
            End If
            depth = depth + 1
            ProgressWin.Status.Text = msg
            ProgressWin.Progress.Value = 0
            ProgressWin.Progress.Step = 1
            If max <= 0 Then
                ProgressWin.Progress.Style = ProgressBarStyle.Marquee
                ProgressWin.Progress.Maximum = 0
            Else
                ProgressWin.Progress.Style = ProgressBarStyle.Continuous
                ProgressWin.Progress.Maximum = max
            End If
            ProgressWin.Visible = True
        End If
        Monitor.Exit(ProgressLock)
        DoEvents()
    End Sub
End Module
