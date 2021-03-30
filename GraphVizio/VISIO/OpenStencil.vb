Imports Visio
Imports Visio.VisOpenSaveArgs
Module _OpenStencil
    Function OpenStencil(Optional ByVal mode As VisOpenSaveArgs = CType(visOpenRO + visOpenHidden, VisOpenSaveArgs)) As Document
        Dim stencil As Document
        Dim sf As String = ""
        sf = LocalDataDirectory() & STENCILNAME
        Try
            stencil = myVisioApp.Documents.OpenEx(sf, CShort(mode))
            ' Make sure master shapes have no layers
            ' (They get added when dropping, which creates spurious layers)
            For Each master As Master In stencil.Masters
                Do While master.Layers.Count > 0
                    master.Layers(1).Delete(0)
                Loop
            Next
        Catch e As Exception
            Throw New GraphVizioException("Couldn't open Visio Stencil " & sf & ": " & e.Message)
        End Try
        Return stencil
    End Function
End Module

