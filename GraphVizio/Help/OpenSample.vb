Imports Visio
Imports Visio.VisOpenSaveArgs
Module OpenSample_
    Function OpenSample(ByVal docname As String) As Document
        Dim doc As Document
        Dim sf As String = ""
        sf = LocalDataDirectory() & docname
        If Not My.Computer.FileSystem.FileExists(sf) Then
            Throw New GraphVizioException("Sample file " & sf & " not found")
        End If
        Try
            doc = myVisioApp.Documents.OpenEx(sf, CShort(visOpenRO))
        Catch e As Exception
            Throw New GraphVizioException("Error opening sample " & sf & ": " & e.Message)
        End Try
        SwitchCurrentSettings()
        Return doc
    End Function
End Module
