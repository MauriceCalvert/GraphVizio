Imports System.Reflection
Imports System.IO
Imports System.Collections.Generic
Imports System.Drawing
Module _GraphVizioStartup
    Sub GraphVizioStartup()

        ' Note characters which require to be quoted in strings
        Const nice As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_$"
        Dim q As Integer = 0
        For ch As Integer = 1 To 255
            If InStr(nice, CStr(Chr(ch))) = 0 Then
                q = q + 1
                ReDim Preserve Quotable(q)
                Quotable(q) = Chr(ch)
            End If
        Next
        Dim cc As Integer = 0
        For ch As Integer = 0 To 31
            cc = cc + 1
            ReDim Preserve ControlChars(cc - 1)
            ControlChars(cc - 1) = Chr(ch)
        Next
        For ch As Integer = 127 To 159
            cc = cc + 1
            ReDim Preserve ControlChars(cc - 1)
            ControlChars(cc - 1) = Chr(ch)
        Next
        ' Setup Dictionary of options recognised by GraphViz
        For Each gvo As String In GraphvizGraphOptions
            GraphvizGraphOption.Add(gvo, True)
        Next
        AddGraphVizioMenu()
        LoadHowTos()
    End Sub
    Private Sub LoadHowTos()
        Dim ass As Assembly
        Dim wordstream As Stream
        Dim wordreader As StreamReader
        Dim line As String
        Dim cmd As String
        Dim arg As String
        Dim ht As HowTo = Nothing
        Dim hta As HowToAction = Nothing

        HowTos = New Dictionary(Of String, HowTo)
        ass = Assembly.GetExecutingAssembly()
        wordstream = GetStreamResource("HowTos.txt")
        wordreader = New StreamReader(wordstream)
        Do While Not wordreader.EndOfStream
            line = wordreader.ReadLine.Trim & " "
            cmd = line.Substring(0, line.IndexOf(" "c))
            arg = line.Substring(line.IndexOf(" "c) + 1).Trim
            If cmd = "title" Then
                ht = New HowTo
                ht.title = arg
                HowTos.Add(arg, ht)
            Else
                If cmd = "say" AndAlso hta IsNot Nothing AndAlso hta.verb = "say" Then ' "say" continuation
                    hta.args = hta.args & vbCr & arg
                Else
                    hta = New HowToAction
                    hta.verb = cmd
                    hta.args = arg
                    ht.actions.Add(hta)
                End If
            End If
        Loop
        wordreader.Close()
    End Sub
End Module
