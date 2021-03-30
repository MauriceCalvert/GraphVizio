Imports System.Threading
Imports System.IO
Imports System.Windows.Forms
Imports System.Text
Imports System.Net
Imports Microsoft.Win32
Imports Visio.VisUICmds
Module Main_
    Private WithEvents visioApplication As Visio.Application
    Public ProgressWin As ProgressForm = Nothing
    Sub Main()
        Dim pf64 As String = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
        Dim pf86 As String = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)

        Dim answer As String = ""
        Dim graphvizok As Boolean = False
        Try
            Application.EnableVisualStyles()

            If Not WaitForInstallerServiceToBeFree(New TimeSpan(0, 1, 0)) Then
                Throw New InvalidOperationException("Windows installer never terminated")
            End If

            For Each path As String In {pf64, pf86}
                Dim di As DirectoryInfo = New DirectoryInfo(path)
                Dim dirs As DirectoryInfo() = di.GetDirectories("Graphviz*")
                For Each dir As DirectoryInfo In dirs
                    If Not dir.Name.StartsWith("GraphVizio") AndAlso dir.Name > answer Then
                        answer = dir.Name
                    End If
                Next
            Next
            If answer = "" Then
                If MsgBox("Graphviz does not appear to be installed in " & pf86 & " or " & pf64 & vbCrLf &
                       "Would you like to download and install it from www.graphviz.org ?",
                        MsgBoxStyle.YesNo, "Install Graphviz") = MsgBoxResult.Yes Then
                    DownloadGraphviz()
                Else
                    MsgBox("You will need to install Graphviz at a later time")
                End If
            End If
            MsgBox("Please start Visio to view the introductory tutorial", MsgBoxStyle.MsgBoxHelp)
        Catch e As Exception
            EndProgress(True)
            MsgBox("Postinstall error: " & ExMessage(e))
        End Try
    End Sub
    Sub DownloadGraphviz()
        Const alink As String = "<a href=""windows/graphviz"
        Const downloadlink As String = "https://graphviz.gitlab.io/_pages/Download/Download_windows.html"
        Dim dlp As String
        Dim apos As Integer
        Dim dlurl As String = ""
        Dim msidata(1000) As Byte
        StartProgress("Preparing to install...", 0)
        dlp = HTTPGetString(downloadlink)
        apos = dlp.IndexOf(alink) ' Find <a href="windows/graphviz-2.38.msi">graphviz-2.38.msi</a>
        If apos < 0 Then
            Throw New InvalidOperationException($"Can't download Graphviz, '{alink}' not found in '{downloadlink}'")
        End If
        dlurl = dlp.Substring(apos + alink.IndexOf("""") + 1) ' windows/graphviz-2.38.msi">graphviz-2.38.msi</a>
        apos = dlurl.IndexOf("""")
        dlurl = dlurl.Substring(0, apos) ' windows/graphviz-2.38.msi
        dlurl = "https://graphviz.gitlab.io/_pages/Download/" & dlurl
        Dim msipath As String = Path.GetTempPath & "graphviz.msi"
        Download(dlurl, msipath)
        Install(msipath)
        File.Delete(msipath)
        EndProgress()
    End Sub
    Sub Install(ByVal msipath As String)
        Dim cmd As String = "msiexec"
        Dim args As String = "/i """ & msipath & """"
        Dim startInfo As ProcessStartInfo
        Dim msiprocess As Diagnostics.Process

        StartProgress("Installing... (Switch to install screen and click Next)", -1)
        startInfo = New ProcessStartInfo(cmd)
        startInfo.Arguments = args
        startInfo.UseShellExecute = False
        startInfo.RedirectStandardError = True
        startInfo.WindowStyle = ProcessWindowStyle.Hidden
        startInfo.CreateNoWindow = True
        msiprocess = Process.Start(startInfo)
        Do
            Thread.Sleep(250)
            BumpProgress()
        Loop Until msiprocess.HasExited
        EndProgress()
        Dim procEC As Integer = -1
        If msiprocess.HasExited Then
            procEC = msiprocess.ExitCode
        Else
            Throw New InvalidOperationException("Graphviz install didn't terminate." & _
                " commandline: " & cmd & " " & args)
        End If
        If procEC <> 0 Then
            Dim errorreader As StreamReader
            Dim errors As String
            errorreader = msiprocess.StandardError
            errors = errorreader.ReadToEnd
            errorreader.Close()
            Throw New InvalidOperationException("Graphviz install failed, rc=" & CStr(procEC) & _
                vbCrLf & errors & vbCrLf & _
                "Command Line: " & cmd & " " & args)
        End If
        EndProgress()
    End Sub
    Private Function HTTPGetString(ByVal url As String) As String
        Dim responseFromServer As String = ""
        Try
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            Dim request As WebRequest = WebRequest.Create(url)
            ' Deal with proxies.
            If WebRequest.DefaultWebProxy Is Nothing Then
                Throw New InvalidOperationException("Unable to detect web proxy configuration")
            Else
                request.Proxy = WebRequest.DefaultWebProxy
            End If

            request.Proxy.Credentials = CredentialCache.DefaultCredentials
            request.Credentials = CredentialCache.DefaultCredentials
            Dim response As WebResponse = request.GetResponse()
            Dim dataStream As Stream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            responseFromServer = reader.ReadToEnd()
            reader.Close()
            response.Close()
        Catch ex As Exception
            Throw New InvalidOperationException("Can't download Graphviz, HTTPGet '" & url & "' failed: " & ExMessage(ex))
        End Try
        Return responseFromServer
    End Function
    Private Sub Download(ByVal url As String, ByVal outputfile As String)
        Dim responseFromServer As String = ""
        Try
            Const buffersize As Integer = 1000
            Dim request As WebRequest = WebRequest.Create(url)
            request.Credentials = CredentialCache.DefaultCredentials
            request.Proxy = New WebProxy
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            If response.ContentLength > 0 Then
                StartProgress("Downloading Graphviz (" & CInt(response.ContentLength / 1000000) & "Mb)...", CInt(response.ContentLength / buffersize))
            Else
                StartProgress("Downloading Graphviz...", 0)
            End If
            Dim sr As Stream = response.GetResponseStream()
            Dim buffer(buffersize) As Byte
            Dim bytes As Integer
            Dim total As Integer = 0
            Dim bumped As Integer = buffersize
            Dim sw As New BinaryWriter(File.Open(outputfile, FileMode.Create))
            Do
                bytes = sr.Read(buffer, 0, buffersize)
                sw.Write(buffer, 0, bytes)
                total = total + bytes
                If total > bumped Then
                    bumped = bumped + buffersize
                    BumpProgress()
                End If
            Loop While bytes > 0
            response.Close()
            sr.Close()
            sw.Close()
        Catch ex As Exception
            Throw New InvalidOperationException("Can't download Graphviz, binary download of '" & url & "' failed: " & ExMessage(ex))
        End Try
    End Sub
    Public Function WaitForInstallerServiceToBeFree(ByVal maxWaitTime As TimeSpan) As Boolean
        ' The _MSIExecute mutex is used by the MSI installer service to serialize installations
        ' and prevent multiple MSI based installations happening at the same time.
        ' For more info: http://msdn.microsoft.com/en-us/library/aa372909(VS.85).aspx
        Const installerServiceMutexName As String = "Global\_MSIExecute"

        Try
            Dim MSIExecuteMutex As Mutex = Mutex.OpenExisting(installerServiceMutexName, System.Security.AccessControl.MutexRights.Synchronize)
            Dim waitSuccess As Boolean = MSIExecuteMutex.WaitOne(maxWaitTime, False)
            MSIExecuteMutex.ReleaseMutex()
            Return waitSuccess
        Catch generatedExceptionName As WaitHandleCannotBeOpenedException
            ' Mutex doesn't exist, do nothing
        Catch generatedExceptionName As ObjectDisposedException
            ' Mutex was disposed between opening it and attempting to wait on it, do nothing
        End Try
        Return True
    End Function
    Function ExMessage(ByVal ex As Exception) As String
        Dim answer As String = ex.Message
        If ex.InnerException IsNot Nothing Then
            answer = answer & " (" & ex.InnerException.Message & ")"
        End If
        Return answer
    End Function
End Module
