Imports System.Windows.Forms
Imports System.Windows.Forms.Application
Friend Class Messages
    Private Sub Messages_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then ' User trying to close, just hide
            e.Cancel = True
            Me.Visible = False
        End If
        MsgWin = Nothing
    End Sub
    Sub ClearMessages()
        TextBox.Text = ""
    End Sub
    Friend Sub Display(ByVal msg As String, ByVal colour As System.Drawing.Color)
        If msg <> "" Then
            TextBox.SelectionColor = colour
            TextBox.SelectedText = msg & vbCr
        End If
        Me.Visible = True
        DoEvents()
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub
End Class