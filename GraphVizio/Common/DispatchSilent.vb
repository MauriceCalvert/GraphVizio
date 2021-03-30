Imports System
Imports System.Threading
Imports System.Runtime.Remoting.Messaging
Imports System.Windows.Forms.Application
Module DispatchSilent_
    Friend Delegate Sub SilentTaskToDo(ByRef parameter As Object)
    Private Class Worker
        Private MyTask As SilentTaskToDo
        Sub New(ByRef TaskSilent As SilentTaskToDo)
            MyTask = TaskSilent
        End Sub
        Sub DoWork(ByVal parameter As Object)
            Try
                MyTask(parameter)
            Catch ex As ThreadAbortException
                Thread.ResetAbort()
                Warning("Silent " & MyTask.Method.Name & " was cancelled. Strange.")
            Catch ex As Exception
                Warning("Error in " & MyTask.Method.Name & ": " & ex.Message)
            End Try
        End Sub
    End Class
    Friend Sub DispatchSilent(ByVal TaskSilent As SilentTaskToDo, _
                              ByVal parameter As Object)
#If DEBUG Then
        TaskSilent(parameter)
#Else
        Dim answer As Object = Nothing
        Dim MyWorker As Worker
        MyWorker = New Worker(TaskSilent)
        ProgressTask = New Thread(AddressOf MyWorker.DoWork)

        ' Need to raise priority so that wait loop doesn't hog CPU
        Try ' Don't risk dying if these little tricks fail
            ProgressTask.Priority = ThreadPriority.AboveNormal
            ProgressTask.SetApartmentState(ApartmentState.STA)
        Catch
        End Try

        ProgressTask.Start(parameter)
        Do While MsgWin IsNot Nothing
            Thread.Sleep(500)
            DoEvents()
        Loop
#End If
    End Sub
End Module
