Imports System.Reflection
Public Class About

    Private Sub About_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.SlidePause = CInt(txtHelpPause.Text)
        My.Settings.SlideStep = CInt(txtHelpStep.Text)
    End Sub

    Private Sub About_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ass As Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Dim path As String = ass.GetName.CodeBase
        If path.Substring(0, 5) = "file:" Then
            path = path.Substring(5)
        End If
        Do While path.StartsWith("/")
            path = path.Substring(1)
        Loop
        txtHelpStep.Text = CStr(My.Settings.SlideStep)
        txtHelpPause.Text = CStr(My.Settings.SlidePause)
        txtDLL.Text = path
        txtVersion.Text = ass.GetName.Version.ToString()
        txtVisVersion.Text = myVisioApp.Version
    End Sub
End Class