Imports Visio
Imports System.IO
Module _ImportDOT
    Private filename As String = ""
    Function ImportDOT() As String
        CurrentSetting.Form.OpenFileDialog.CheckFileExists = True
        CurrentSetting.Form.OpenFileDialog.Filter = "Graphviz files|*.gv|All files|*.*"
        CurrentSetting.Form.OpenFileDialog.InitialDirectory = My.Settings.DOTDirectory
        CurrentSetting.Form.OpenFileDialog.ShowDialog()
        filename = CurrentSetting.Form.OpenFileDialog.FileName
        If filename <> "" Then
            Dim fileInfo As New FileInfo(filename)
            My.Settings.DOTDirectory = fileInfo.DirectoryName
            DispatchWithProgress(New ProgressTaskToDo(AddressOf ImportDOTandDraw), True)
        End If
        Return ""
    End Function
    Private Function ImportDOTandDraw() As String
        Dim layedout As String = RunGraphViz(filename)
        Dim ad As Document = Nothing
        StartProgress("Importing...", 0)
        If layedout <> "" Then
            Dim graph As Graph = ReadDOT(layedout)
            If graph Is Nothing Then
                Throw New GraphVizioException("GraphViz couldn't process " & layedout & ". Check file " & layedout & " for syntax errors")
            End If
            ad = myVisioApp.Documents.Add("")
            CurrentSetting("drawboundingboxes") = "true"
            DrawGraph(ad, graph, True)
        End If
        EndProgress()
        Return "" ' all's well
    End Function
End Module
