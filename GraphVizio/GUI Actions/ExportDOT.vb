Imports Visio
Module _ExportDOT
    Private filename As String = ""
    Private Page As IVPage
    Function ExportDOT() As String
        Page = myVisioApp.ActivePage
        If Page Is Nothing Then
            Return "No Visio page is active"
        End If
        CurrentSetting.Form.OpenFileDialog.CheckFileExists = False
        CurrentSetting.Form.OpenFileDialog.ShowDialog()
        filename = CurrentSetting.Form.OpenFileDialog.FileName
        If filename <> "" Then
            DispatchWithProgress(New ProgressTaskToDo(AddressOf WriteFullDOT), True)
        End If
        Return ""
    End Function
    Private Function WriteFullDOT() As String
        StartProgress("Analysing...", 0)
        Dim VisGraph As Graph = LoadVisio(Page)
        VisGraph.RenameNodes()
        WriteDOT(VisGraph, filename, DOTDetail.Full)
        EndProgress()
        Return "" ' all's well
    End Function
End Module
