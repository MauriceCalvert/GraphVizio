Imports Visio
Imports System.Collections.Generic
Module _SwitchCurrentSettings
    Sub SwitchCurrentSettings()
        Dim doc As Document
        Dim setid As Integer = -1

        ' 1 = VisDocumentTypes.visTypeDrawing
        If myVisioApp.ActiveDocument IsNot Nothing AndAlso myVisioApp.ActiveDocument.Type <> 1 Then
            Exit Sub ' Forget about it when stencils are activated
        End If
        doc = myVisioApp.ActiveDocument

        If doc IsNot Nothing Then
            setid = doc.ID
        End If

        If CurrentSetting IsNot Nothing Then
            If CurrentSetting.Form Is Nothing Then ' application shutdown
                Exit Sub
            End If
            If setid = CurrentSetting.ID Then
                CurrentSetting.ToDocument()
            Else
                CurrentSetting.HideForm()
            End If
        End If

        If Not PageTable.TryGetValue(setid, CurrentSetting) Then
            CurrentSetting = New Setting(setid)
            CurrentSetting.FromDocument()
        End If

        If DisplaySettings Then
            CurrentSetting.ShowForm()
        End If

    End Sub
End Module
