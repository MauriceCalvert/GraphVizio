Imports Visio
Module VisioEvents_
    Sub VisioStartup()
        Try
            System.Windows.Forms.Application.EnableVisualStyles()
            If Not My.Settings.VersionChecked Then
                ' Make sure that we're running against the correct version of Visio
                Dim visver As Integer = 0
                Dim vv As String = myVisioApp.Version
                If vv <> "" Then
                    Try
                        Dim dp As Integer = vv.IndexOf(".")
                        If dp < 0 Then
                            dp = vv.IndexOf(",") ' maybe strange number formatting
                            If dp < 0 Then
                                dp = vv.Length + 1
                            End If
                        End If
                        vv = vv.Substring(0, dp)
                        visver = CInt(vv)
                    Catch ex As Exception
                        visver = 0
                    End Try
                End If
                If visver < MINVISIOVERSION Then
                    Warning("Visio reports version " & vv & vbCrLf & _
                        "This addin requires at least Visio 2003. I'll try my best but it might not work out.")
                End If
                My.Settings.VersionChecked = True
                My.Settings.Save()
            End If
            GraphVizioStartup()
            AddHandler myVisioApp.AppActivated, AddressOf AppActivated
            AddHandler myVisioApp.BeforeDocumentClose, AddressOf BeforeDocumentClose
            AddHandler myVisioApp.BeforeDocumentSave, AddressOf BeforeDocumentSave
            AddHandler myVisioApp.BeforeDocumentSaveAs, AddressOf BeforeDocumentSave
            AddHandler myVisioApp.DocumentCreated, AddressOf DocumentCreated
            AddHandler myVisioApp.DocumentOpened, AddressOf DocumentOpened
            AddHandler myVisioApp.WindowActivated, AddressOf WindowActivated
            AddHandler myVisioApp.WindowTurnedToPage, AddressOf WindowTurnedToPage
            AddHandler myVisioApp.AppDeactivated, AddressOf AppDeactivated
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Sub VisioShutdown()
        Try
            My.Settings.Save()
            RemoveHandler myVisioApp.AppActivated, AddressOf AppActivated
            RemoveHandler myVisioApp.BeforeDocumentClose, AddressOf BeforeDocumentClose
            RemoveHandler myVisioApp.BeforeDocumentSave, AddressOf BeforeDocumentSave
            RemoveHandler myVisioApp.BeforeDocumentSaveAs, AddressOf BeforeDocumentSave
            RemoveHandler myVisioApp.DocumentCreated, AddressOf DocumentCreated
            RemoveHandler myVisioApp.DocumentOpened, AddressOf DocumentOpened
            RemoveHandler myVisioApp.WindowActivated, AddressOf WindowActivated
            RemoveHandler myVisioApp.WindowTurnedToPage, AddressOf WindowTurnedToPage
            RemoveHandler myVisioApp.AppDeactivated, AddressOf AppDeactivated
        Catch ex As Exception
        End Try
    End Sub
    Private Sub AppActivated(ByVal app As Visio.IVApplication)
        If HelpInProgress Then
            Exit Sub
        End If
        Try
            If DisplaySettings Then
                CurrentSetting.ShowForm()
            End If
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub AppDeactivated(ByVal app As Visio.IVApplication)
        If DisplaySettings AndAlso CurrentSetting IsNot Nothing AndAlso CurrentSetting.Form IsNot Nothing Then
            CurrentSetting.HideForm()
        End If
    End Sub
    Private Sub BeforeDocumentClose(ByVal doc As Visio.Document)
        If HelpInProgress Then
            Exit Sub
        End If
        Try
            If PageTable.ContainsKey(doc.ID) Then
                PageTable(doc.ID).Close()
            End If
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub BeforeDocumentSave(ByVal doc As Visio.Document)
        Try
            ' 1 = VisDocumentTypes.visTypeDrawing
            If doc.Type = 1 Then
                If CurrentSetting IsNot Nothing Then
                    CurrentSetting.ToDocument()
                End If
            End If
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub DocumentCreated(ByVal doc As Visio.Document)
        If HelpInProgress Then
            Exit Sub
        End If
        Try
            ' 1 = VisDocumentTypes.visTypeDrawing
            If doc.Type = 1 Then
                SwitchCurrentSettings()
                CurrentSetting.ToDocument()
            End If
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub DocumentOpened(ByVal doc As Visio.Document)
        If HelpInProgress Then
            Exit Sub
        End If
        Try
            ' 1 = VisDocumentTypes.visTypeDrawing
            If doc.Type = 1 Then
                SwitchCurrentSettings()
                CurrentSetting.FromDocument()
            End If
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub WindowActivated(ByVal Window As Visio.Window)
        If HelpInProgress Then
            Exit Sub
        End If
        If My.Settings.FirstTime Then
            My.Settings.FirstTime = False
            My.Settings.Save()
            ShowHowTo("View the Introduction to GraphVizio")
        End If
        Try
            SwitchCurrentSettings()
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub WindowTurnedToPage(ByVal page As Visio.Window)
        If HelpInProgress Then
            Exit Sub
        End If
        Try
            SwitchCurrentSettings()
            If CurrentSetting IsNot Nothing AndAlso CurrentSetting.Form IsNot Nothing Then
                CurrentSetting.Form.DockMyself()
            End If
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
End Module
