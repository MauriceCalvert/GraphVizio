Imports System.Windows.Forms
Module _SavePreset
    Sub SavePreset()
        Dim PresetForm As New Preset
        PresetForm.ShowDialog()
        If PresetForm.DialogResult = DialogResult.OK Then
            CurrentSetting.Save(PresetForm.presetname.Text)
            AddGraphVizioMenu()
        End If
        PresetForm.Dispose()
    End Sub
End Module
