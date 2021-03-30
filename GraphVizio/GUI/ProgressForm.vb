Imports System.Threading
Imports System.Drawing
Imports System.ComponentModel
Imports System.Windows.Forms.Application
Friend Class ProgressForm
    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        Try
            CancelButton.Enabled = False
            TextBox.Text = "Cancelling..."
            Me.Refresh()
            If ProgressTask Is Nothing Then
                MsgBox("Can't abort in debug mode. Click Break!")
            Else
                ProgressTask.Abort()
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ProgressForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ProgressWin = Nothing
    End Sub
    Private Sub ProgressForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim screen As System.Windows.Forms.Screen = System.Windows.Forms.Screen.PrimaryScreen
        Me.Top = CInt(screen.Bounds.Height / 3)
    End Sub
End Class