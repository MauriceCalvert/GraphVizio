Module _EndTXN
    Sub EndTXN(ByVal Commit As Boolean)
        If My.Settings.DrawingUpdates Then
            myVisioApp.EndUndoScope(ScopeStack.Pop(), Commit)
        End If
    End Sub
End Module
