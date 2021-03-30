Module _StartTXN
    Sub StartTXN()
        If My.Settings.DrawingUpdates Then
            ScopeStack.Push(myVisioApp.BeginUndoScope(CStr(CurrentSetting.ID)))
        End If
    End Sub
End Module
