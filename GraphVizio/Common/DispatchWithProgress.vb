Imports System.Windows.Forms.Application
Imports System.Exception
Imports System.Reflection
Imports System.Threading
Module DispatchWithProgress_
    Friend ProgressTask As Thread
    Public Delegate Function ProgressTaskToDo(ByRef parameter As Object) As String
    Private MyWorker As Worker
    Friend Sub DispatchWithProgress(
        ByVal TaskToDo As ProgressTaskToDo,
        ByVal Asynchronous As Boolean,
        Optional Parameter As Object = Nothing)

        myVisioApp.ScreenUpdating = My.Settings.DrawingUpdates
        myVisioApp.ShowChanges = My.Settings.DrawingUpdates
        myVisioApp.DeferRecalc = Not My.Settings.DrawingUpdates
        myVisioApp.LiveDynamics = Not My.Settings.DrawingUpdates
        myVisioApp.AutoLayout = Not My.Settings.DrawingUpdates
        myVisioApp.UndoEnabled = Not My.Settings.DrawingUpdates

        Dim ans As String = ""
        Try
            If Asynchronous Then
                Using Semaphore As New AutoResetEvent(False)
                    MyWorker = New Worker(TaskToDo, Semaphore, Parameter)
                    ProgressTask = New Thread(AddressOf MyWorker.DoWork)
                    Try ' Don't risk dying if these little tricks fail
                        ProgressTask.Priority = ThreadPriority.AboveNormal
                        ProgressTask.SetApartmentState(ApartmentState.STA)
                    Catch
                    End Try
                    ProgressTask.Start(Parameter)
                    Semaphore.WaitOne()
                End Using
            Else
                ans = ""
                StartProgress("Launching " & TaskToDo.Method.Name, 0)
                StartTXN()
                ans = TaskToDo(Parameter)
                EndTXN(COMMIT)
                EndProgress()
            End If

        Catch ex As Exception
            ans = "Error in " & TaskToDo.Method.Name & ": " & ex.Message
            EndProgress(True)
            EndTXN(ROLLBACK)
        Finally
            myVisioApp.ScreenUpdating = True
            myVisioApp.ShowChanges = True
            myVisioApp.DeferRecalc = False
            myVisioApp.LiveDynamics = True
            myVisioApp.AutoLayout = True
            myVisioApp.UndoEnabled = True
            If ans <> "" Then
                Warning(ans)
            End If
        End Try

    End Sub
    Private Class Worker
        Private MyWork As ProgressTaskToDo
        Private Delegate Sub Reinstater()
        Private myex As Exception = Nothing
        Private HisSemaphore As AutoResetEvent
        Private HisParameter As Object
        Sub New(ByVal TaskToDo As ProgressTaskToDo, semaphore As AutoResetEvent, parameter As Object)
            MyWork = TaskToDo
            HisSemaphore = semaphore
            HisParameter = parameter
        End Sub
        Sub DoWork(parameter As Object)
            Dim ans As String = ""
            Try
                StartProgress("Launching " & MyWork.Method.Name & " task...", 0)
                StartTXN()
                ans = MyWork(HisParameter)
                EndTXN(COMMIT)
            Catch ex As ThreadAbortException
                EndTXN(ROLLBACK)
                ans = "Task cancelled (you pressed ESC)"
                Thread.ResetAbort()
            Catch ex As Exception
                EndTXN(ROLLBACK)
                HandleError(ex)
            Finally
                EndProgress(True)
                If ans <> "" Then
                    Warning("Error in " & MyWork.Method.Name & ": " & ans)
                End If
                Do While MsgWin IsNot Nothing
                    Thread.Sleep(500)
                    DoEvents()
                Loop
                HisSemaphore.Set()
            End Try
        End Sub
    End Class
End Module
