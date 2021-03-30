Imports System
Imports System.Diagnostics
Imports System.Collections
Imports System.ComponentModel
Imports System.Configuration.Install
Imports System.Reflection
Imports System.IO

<RunInstaller(True)> _
Public Class InstallerClass
    Inherits System.Configuration.Install.Installer
    Public Sub New()
        MyBase.New()
        AddHandler Me.AfterInstall, AddressOf InstallComplete
    End Sub
    Private Sub InstallComplete(ByVal sender As Object, ByVal e As InstallEventArgs)
        Try
            ' Microsoft.VisualBasic.MsgBox("Start PostInstall",, "GraphVizio StartPostinstall")
            Dim mydir As String = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            Directory.SetCurrentDirectory(mydir)
            Process.Start(mydir & "\PostInstall.exe")
        Catch ex As Exception
            Microsoft.VisualBasic.MsgBox("InstallComplete failed " & ex.Message,, "GraphVizio StartPostinstall")
        End Try
    End Sub
End Class
