Friend Class FatalError
    Private errormsg As String
    Sub New(ByVal msg As String)
        InitializeComponent()
        errormsg = msg
        txtDetails.Text = msg
    End Sub
    Private Sub btnclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclose.Click
        Me.Close()
    End Sub
    Private Sub sendreport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sendreport.Click
        OpenEmail("graphvizio@calvert.ch", "GraphVizio error report", errormsg)
    End Sub
    Friend Sub OpenEmail(ByVal EmailAddress As String, ByVal Subject As String, ByVal Body As String)
        Try
            Dim sParams As String
            sParams = "mailto:" & EmailAddress
            sParams = sParams & "?subject=" & Subject
            sParams = sParams & "&body=" & Body
            Me.Close()
            System.Diagnostics.Process.Start(sParams)
        Catch ex As Exception
            Warning("Couldn't open your email program: " & ex.Message)
        End Try
    End Sub
End Class