Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports Visio
Imports Visio.VisOpenSaveArgs

Module SortStencil_
    Sub SortStencil()
        Try
            Dim sel As Selection
            sel = myVisioApp.ActiveWindow.Selection
            Dim shp As Visio.Shape
            shp = sel.PrimaryItem
            If shp Is Nothing Then
                MsgBox("Select a shape from the stencil you want to sort")
                Exit Sub
            End If
            Dim masterid As String = shp.Master.BaseID ' 		BaseID	"{382E3C84-5E42-428B-8AFD-A6B14EE93B2E}"	String
            Dim stencildoc As Document = Nothing
            'Dim stencil As document = myVisioApp.Documents.OpenEx(sf, CShort(mode))
            For Each doc As Document In myVisioApp.Documents
                Debug.WriteLine(doc.Name)
                If doc.Type = 2 Then ' VisDocumentTypes.visTypeStencil 
                    For Each mstr As Object In doc.Masters
                        Debug.WriteLine("  " & mstr.Name)
                        If mstr.baseid = masterid Then
                            stencildoc = doc
                            Exit For
                        End If
                    Next
                    If stencildoc IsNot Nothing Then
                        Exit For
                    End If
                End If
            Next
            If shp Is Nothing Then
                MsgBox($"Couldn't find {shp.Master.Name} in any stencil!")
                Exit Sub
            End If
            Dim sorted As Document = myVisioApp.Documents.AddEx("Sorted.vss", , 512) ' visAddStencil
            Dim mstrs As New List(Of Master)
            For Each mstr As Master In stencildoc.Masters
                mstrs.Add(mstr)
            Next
            mstrs = mstrs.OrderBy(Function(q) q.Name).ToList
            For Each mstr As Master In mstrs
                sorted.Masters.Drop(mstr, 0, 0)
            Next
            'Dim stencilname As String = stencildoc.FullName
            'stencildoc.Close()
            'Dim stencil = myVisioApp.Documents.OpenEx(stencilname, visOpenRW)
            'Dim mstrs As New List(Of Master)
            'For Each mstr As Master In stencil.Masters
            '    mstrs.Add(mstr)
            'Next
            'For Each mstr As Master In mstrs
            '    mstr.Delete()
            'Next
            'For Each mstr As Master In mstrs.OrderBy(Function(q) q.Name)
            '    stencil.Masters.Drop(mstr, 0, 0)
            'Next

        Catch e As Exception
            Throw New GraphVizioException("SortStencil failed" & e.Message)
        End Try
    End Sub
End Module

