Module _HandleError
    Sub HandleError(ByVal ex As Exception)
        Try
            myVisioApp.ScreenUpdating = -1
            myVisioApp.ShowChanges = True
        Catch
            ' fails = tough
        End Try
        If ProgressWin IsNot Nothing Then
            ProgressWin.Close()
            ProgressWin = Nothing
        End If
        MsgWin = New Messages
        If TypeOf ex Is System.Threading.ThreadAbortException Then
            MsgWin.Display("Operation cancelled", System.Drawing.Color.Red)
        ElseIf TypeOf ex Is GraphVizioException Then
            MsgWin.Display(ex.Message, System.Drawing.Color.Red)
        Else
            Dim msg As String
            msg = ex.Message & vbCr
            Dim e As Exception = ex.InnerException
            Do While e IsNot Nothing
                msg = msg & vbCr & "...caused by " & e.Message
                e = e.InnerException
            Loop
            msg = msg & vbCr & "Stack trace:" & vbCr & ex.StackTrace.ToString
            msg = msg.Replace(vbCr, vbCrLf)
            Dim errorform As FatalError
            errorform = New FatalError(msg)
            errorform.ShowDialog()
        End If
    End Sub
End Module
