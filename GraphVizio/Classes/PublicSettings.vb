Public Class PublicSettings
    Sub ResetFirst()
        My.Settings.FirstTime = True
        My.Settings.Save()
    End Sub
End Class
