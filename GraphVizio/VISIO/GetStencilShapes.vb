Imports Visio
Imports System.Collections.Generic
Module _GetStencilConnectors
    Sub GetStencilShapes()
        Dim stencil As Document = OpenStencil()
        Dim names() As String = Nothing
        Dim connector As Shape
        Dim master As Master
        StencilConnectors = New List(Of String)
        StencilShapes = New List(Of String)
        stencil.Masters.GetNamesU(names) ' Implicit conversion unavoidable
        For Each cname As String In names
            master = stencil.Masters(cname)
            connector = master.Shapes(1)
            If connector.CellExistsU("BeginX", CShort(True)) = 0 Then ' a 2D (real) shape
                Dim lastch As String = cname.Substring(cname.Length - 1)
                If lastch >= "0" And lastch <= "9" Then ' Finishes with a digit = series of shapes for ranking
                    If lastch = "0" Then
                        cname = cname.Substring(0, cname.Length - 1) & "+" ' remove trailing digit, add "+" sign instead
                    Else
                        cname = "" ' don't add 1..9
                    End If
                End If
                If cname <> "" Then StencilShapes.Add(cname)
            Else
                StencilConnectors.Add(cname) ' a 1D shape, a connector
            End If
        Next
        StencilConnectors.Sort()
        StencilShapes.Sort()
        stencil.Close()
    End Sub
End Module
