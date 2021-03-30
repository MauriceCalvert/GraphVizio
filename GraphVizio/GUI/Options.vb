Imports System.Reflection
Friend Class Options

    Private Sub Options_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            My.Settings.ResizeAmount = CInt(txtresizeamount.Text)
            My.Settings.FadePct = CInt(txtFadePct.Text)
            My.Settings.FadeDelay = CInt(txtFadeDelay.Text)
            My.Settings.HelpSpeed = cmbHelpSpeed.Text
            My.Settings.ToolTipDelay = CInt(txtTooltipDelay.Text) * 1000
            My.Settings.DrawingUpdates = chkDrawingUpdates.Checked
            My.Settings.ExecutableDirectory = txtBinaries.Text
            My.Settings.Save()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub EditOptions_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            txtresizeamount.Text = CStr(My.Settings.ResizeAmount)
            txtFadePct.Text = CStr(My.Settings.FadePct)
            txtFadeDelay.Text = CStr(My.Settings.FadeDelay)
            cmbHelpSpeed.Text = CStr(My.Settings.HelpSpeed)
            txtTooltipDelay.Text = CStr(My.Settings.ToolTipDelay / 1000)
            chkDrawingUpdates.Checked = My.Settings.DrawingUpdates
            txtBinaries.Text = My.Settings.ExecutableDirectory
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub txtFadePct_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        CheckNumber(e, txtFadePct.Text, "Fade percentage", 0, 100)
    End Sub
    Private Sub txtFadeSeconds_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        CheckNumber(e, txtFadeDelay.Text, "Fade seconds", 0, 1000)
    End Sub
    Private Sub txtresizeamount_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        CheckNumber(e, txtresizeamount.Text, "Resize percentage", 0, 100)
    End Sub
    Private Sub CheckNumber(ByVal e As System.ComponentModel.CancelEventArgs, ByVal value As String, ByVal text As String, ByVal low As Integer, ByVal high As Integer)
        Dim hisvalue As Integer
        If Integer.TryParse(value, hisvalue) Then
            If hisvalue < low Or hisvalue > high Then
                MsgBox(text & " must be a number between " & low & " and " & high, MsgBoxStyle.Exclamation)
                e.Cancel = True
            Else
                MsgBox(text & " must be a number between " & low & " and " & high, MsgBoxStyle.Exclamation)
                e.Cancel = True
            End If
        Else
            MsgBox(value & " is not an integer", MsgBoxStyle.Exclamation)
            e.Cancel = True
        End If
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub
End Class