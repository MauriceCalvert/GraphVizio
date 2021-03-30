Imports System.IO
Imports GOLD
Module _ReadDOT
    Function ReadDOT(ByVal dotfile As String) As Graph
        Dim graph As Graph
        Dim fi As FileInfo

        StartProgress("Interpreting layout..", -1)

        ' Remove trailing "\" at line-ends 
        fi = New FileInfo(dotfile)
        If Not fi.Exists Then
            Throw New GraphVizioException(dotfile & " not found (ReadDOT)")
        End If
        Dim sr As StreamReader = New StreamReader(dotfile)
        Dim cleanfile As String = dotfile & ".txt"
        Dim sw As StreamWriter = New StreamWriter(cleanfile)
        Dim line As String
        Dim fullline As String
        Do
            fullline = ""
            Do
                line = sr.ReadLine()
                If line Is Nothing Then Exit Do
                line = line.TrimEnd(" ")
                If line.EndsWith("\") Then
                    fullline = fullline & line.Substring(0, line.Length - 1)
                Else
                    fullline = fullline & line
                End If
            Loop Until Not line.EndsWith("\")
            sw.WriteLine(fullline)
        Loop Until line Is Nothing
        sr.Close()
        sw.Close()

        Dim dp As New DotParser

        graph = dp.LoadDOT(cleanfile)

        EndProgress()

        Return graph

    End Function
End Module

