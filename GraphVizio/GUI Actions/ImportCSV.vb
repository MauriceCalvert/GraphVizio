Imports Visio
Imports System.IO
Module _ImportCSV
    Private filename As String = ""
    Function ImportCSV(Optional ByVal thefile As String = "", Optional ByVal Asynchronous As Boolean = True) As String
        If thefile = "" Then
            CurrentSetting.Form.OpenFileDialog.CheckFileExists = True
            CurrentSetting.Form.OpenFileDialog.Filter = "CSV files|*.csv|Text files|*.txt|All files|*.*"
            CurrentSetting.Form.OpenFileDialog.InitialDirectory = My.Settings.CSVDirectory
            CurrentSetting.Form.OpenFileDialog.ShowDialog()
            filename = CurrentSetting.Form.OpenFileDialog.FileName
        Else
            filename = thefile
        End If
        If filename <> "" Then
            Dim fileInfo As New FileInfo(filename)
            My.Settings.CSVDirectory = fileInfo.DirectoryName
            DispatchWithProgress(New ProgressTaskToDo(AddressOf ImportCSVandDraw), Asynchronous)
        End If
        Return ""
    End Function
    Private Function ImportCSVandDraw() As String
        Dim sr As StreamReader = New StreamReader(filename)
        Dim line As String
        Dim lineno As Integer = 0
        Dim graph As New Graph(ROOTGRAPHNAME)
        Dim ad As Document = Nothing

        StartProgress("Importing CSV...", 0)
        Do While sr.Peek() >= 0
            line = sr.ReadLine()
            lineno = lineno + 1
            Dim nodes As String() = line.Split(New [Char]() {","c})
            If nodes.GetUpperBound(0) = 1 Then
                Dim fromnode As Node = graph.AddNode(nodes(0))
                Dim tonode As Node = graph.AddNode(nodes(1))
                graph.Connect(fromnode, tonode)
            Else
                Throw New GraphVizioException("Error at line " & lineno & " in " & filename & _
                    " : " & vbCr & line & vbCr & " line must contain a single comma")
            End If
        Loop
        sr.Close()
        Dim temppath As String = System.IO.Path.GetTempPath
        WriteDOT(graph, temppath & INPUTFILE, DOTDetail.Full)
        Dim layedout As String = RunGraphViz(temppath & INPUTFILE)
        Dim newgraph As Graph = ReadDOT(layedout)
        ad = myVisioApp.Documents.Add("")
        CurrentSetting("drawboundingboxes") = "false"
        DrawGraph(ad, newgraph, True)
        EndProgress()
        Return "" ' all's well
    End Function
End Module
