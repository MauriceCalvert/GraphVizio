Imports System
Imports System.Threading
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Windows.Forms.Application
Module _RunGraphViz
    Function RunGraphViz(ByVal ifile As String) As String

        Dim pgmpath As String = GetGraphvizDir()
        Dim ofile As String = Path.GetTempFileName
        Dim pgm As String = CurrentSetting.Value("algorithm") & ".exe"
        Dim startInfo As ProcessStartInfo
        Dim GraphVizioProcess As Diagnostics.Process
        Dim errors As String
        Dim errorreader As StreamReader
        Dim pos As Integer
        Dim cmd As String
        Dim args As String
        Dim withoutbom As String = RemoveUTFBOM(ifile)
        Dim executable As String = Path.Combine(pgmpath, pgm)

        If Not My.Computer.FileSystem.FileExists(executable) Then
            Throw New GraphVizioException("GraphViz executable " & pgm & " not found at " & pgmpath & vbCrLf &
                "Please (re)install Graphviz from www.graphviz.org")
        Else
            cmd = Quote(executable)
            args = GraphvizOptions() & " -q -o" & Quote(ofile) & " " & Quote(withoutbom)
            StartProgress("Calculating layout...", -1)
            startInfo = New ProcessStartInfo(cmd)
            startInfo.Arguments = args
            startInfo.UseShellExecute = False
            startInfo.RedirectStandardError = True
            startInfo.WindowStyle = ProcessWindowStyle.Hidden
            startInfo.CreateNoWindow = True
            GraphVizioProcess = Process.Start(startInfo)
WAITMORE:
            GraphVizioProcess.WaitForExit(60000)
            EndProgress()
            Dim procEC As Integer = -1
            If GraphVizioProcess.HasExited Then
                procEC = GraphVizioProcess.ExitCode
            Else
                If MsgBox("Graphviz didn't finish after 60 seconds. Continue waiting?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton1, "Timeout") = MsgBoxResult.Yes Then
                    GoTo WAITMORE
                End If
                Throw New GraphVizioException(pgm & " didn't terminate." &
                    " commandline: " & cmd & " " & args)
            End If

            If procEC <> 0 Then
                errorreader = GraphVizioProcess.StandardError
                errors = errorreader.ReadToEnd
                errorreader.Close()
                pos = errors.IndexOf("Error:")
                If pos > 0 Then
                    errors = errors.Substring(pos)
                End If
                Throw New GraphVizioException(pgm & " failed, rc=" & CStr(procEC) &
                    vbCrLf & errors & vbCrLf &
                    "Command Line: " & cmd & " " & args)
            End If
        End If
        Return ofile
    End Function
    Public Function GetGraphvizDir() As String

        Dim answer As String = My.Settings.ExecutableDirectory

        If answer <> "" Then
            If File.Exists(Path.Combine(answer, CurrentSetting("algorithm") & ".exe")) Then
                Return answer
            End If
            answer = "" ' invalid folder, start searching again
        End If

        Dim best As String = ""
        For Each pfd As String In {Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                                   Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)}

            Dim di As DirectoryInfo = New DirectoryInfo(pfd)
            Dim dirs As DirectoryInfo() = di.GetDirectories("Graphviz*")

            For Each dir As DirectoryInfo In dirs
                ' > so that we take the highest numbered Graphviz directory
                If Not dir.Name.StartsWith("GraphVizio") AndAlso dir.Name > best Then
                    If File.Exists(Path.Combine(answer, CurrentSetting("algorithm") & ".exe")) Then
                        best = dir.Name
                        answer = pfd & "\" & dir.Name & "\bin\"
                    End If
                End If
            Next
        Next pfd

        If answer = "" Then

            Dim question As String =
                "No directory called 'Graphviz*' found in any Program Files" & vbCrLf &
                "Enter the full path to " & CurrentSetting("algorithm") & ".exe, for example:" & vbCrLf &
                "P:\Program Files (x86)\Graphviz\bin\"
            Do
                answer = InputBox(question, "Where is Graphviz?")
                If File.Exists(Path.Combine(answer, CurrentSetting("algorithm") & ".exe")) Then
                    Exit Do
                End If
                question = CurrentSetting("algorithm") & ".exe not found in " & answer & vbCrLf &
                    "Enter the full path to " & CurrentSetting("algorithm") & ".exe, for example:" & vbCrLf &
                    "P:\Program Files (x86)\Graphviz\bin\"
            Loop Until answer = ""
        End If
        My.Settings.ExecutableDirectory = answer
        My.Settings.Save()
        Return answer
    End Function
    Private Function GraphvizOptions() As String
        Dim ans As String = ""
        Dim seed As String

        ans = "" ' !!  " -Gcharset=latin1"

        If CurrentSetting("aspectratio") <> "0" Then
            ans = ans & " -Gratio=" & CurrentSetting.Value("aspectratio")
        End If

        If CurrentSetting.Value("overlap") <> "" Then
            ans = ans & " -Goverlap=" & CurrentSetting.Value("overlap")
        End If

        If CurrentSetting.Value("splines") <> "" Then
            ans = ans & " -Gsplines=" & CurrentSetting.Value("splines")
        End If

        If CurrentSetting.Value("rankdir") <> "" Then
            ans = ans & " -Grankdir=" & CurrentSetting.Value("rankdir").ToUpper ' Case sensitive
        End If

        seed = CurrentSetting.Value("seed")
        If seed <> "0" Then
            If CurrentSetting.Value("lockseed") <> "true" Then
                If Not IsNumeric(seed) Then
                    seed = "0"
                End If
                seed = CStr(CInt(seed) + 1)
                CurrentSetting("seed") = seed
            End If
            ans = ans & " -Gstart=" & seed
        End If

        If CurrentSetting("commandoptions") <> "" Then
            ans = ans & " " & CurrentSetting.Value("commandoptions")
        End If

        Return ans
    End Function
End Module
