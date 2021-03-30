Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Windows.Forms
Imports AddinExpress.MSO

'Add-in Express Add-in Module
<GuidAttribute("57462138-BD10-4D78-A6E8-6C400474FE09"), ProgIdAttribute("GraphVizio.AddinModule")> _
Public Class AddinModule
    Inherits AddinExpress.MSO.ADXAddinModule

#Region " Add-in Express automatic code "
 
    'Required by Add-in Express - do not modify
    'the methods within this region
 
    Public Overrides Function GetContainer() As System.ComponentModel.IContainer
        If components Is Nothing Then
            components = New System.ComponentModel.Container
        End If
        GetContainer = components
    End Function
 
    <ComRegisterFunctionAttribute()> _
    Public Shared Sub AddinRegister(ByVal t As Type)
        AddinExpress.MSO.ADXAddinModule.ADXRegister(t)
    End Sub
 
    <ComUnregisterFunctionAttribute()> _
    Public Shared Sub AddinUnregister(ByVal t As Type)
        AddinExpress.MSO.ADXAddinModule.ADXUnregister(t)
    End Sub
 
    Public Overrides Sub UninstallControls()
        MyBase.UninstallControls()
    End Sub
 
#End Region
 
    Public Shared Shadows ReadOnly Property CurrentInstance() As AddinModule
        Get
            Return CType(AddinExpress.MSO.ADXAddinModule.CurrentInstance, AddinModule)
        End Get
    End Property

    Public ReadOnly Property VisioApp() As Visio.IVApplication
        Get
            Return CType(HostApplication, Visio.IVApplication)
        End Get
    End Property
    Private Sub AddinModule_AddinBeginShutdown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.AddinBeginShutdown
        VisioShutdown()
    End Sub

    Private Sub AddinModule_AddinStartupComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.AddinStartupComplete
        myVisioApp = CType(HostApplication, Visio.Application)
        VisioStartup()
    End Sub

End Class

