Imports Visio
Imports Visio.VisUnitCodes
Imports Visio.VisCellIndices
Imports vb = Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Module _WriteDOT

    Friend Enum DOTDetail
        Full
        Minimal
    End Enum

    Private ofile As System.IO.StreamWriter
    Private Labels As DOTDetail
    Private arrow As String = ""

    Friend Sub WriteDOT(ByVal Graph As Graph, ByVal filename As String, ByVal DOTDetail As DOTDetail)

        Dim temppath As String = System.IO.Path.GetTempPath
        Labels = DOTDetail

        Dim utf8WithoutBom As New System.Text.UTF8Encoding(False)

        ofile = New StreamWriter(filename, False, utf8WithoutBom)

        StartProgress("Storing layout...", Graph.ItemCount)

        If Graph.Strict = "true" Then ofile.WriteLine("strict ")

        If Graph.digraph = "true" Then
            ofile.WriteLine(" digraph " & Graph.Name & " {")
            arrow = "->"
        Else
            ofile.WriteLine(" graph " & Graph.Name & " {")
            arrow = "--"
        End If

        MergeAttributes(Graph)

        WriteGraph(Graph, 0)
        ofile.WriteLine("}")
        ofile.Close()
        EndProgress()
    End Sub
    Private Sub WriteGraph(ByVal Graph As Graph, ByVal indent As Integer)
        Dim comma As String
        If Graph.GraphAttrs.Count > 0 Then
            ofile.Write(String.Empty.PadLeft(indent + 2, " "c) & "graph [")
            comma = ""
            For Each kvp As KeyValuePair(Of String, String) In Graph.GraphAttrs
                WriteAttr(comma, kvp.Key, kvp.Value)
            Next
            ofile.WriteLine("];")
        End If
        If Graph.DefaultNodeAttrs.Count > 0 Then
            ofile.Write(String.Empty.PadLeft(indent + 2, " "c) & "node [")
            comma = ""
            For Each kvp As KeyValuePair(Of String, String) In Graph.DefaultNodeAttrs
                WriteAttr(comma, kvp.Key, kvp.Value)
            Next
            ofile.WriteLine("];")
        End If
        If Graph.DefaultEdgeAttrs.Count > 0 Then
            ofile.Write(String.Empty.PadLeft(indent + 2, " "c) & "edge [")
            comma = ""
            For Each kvp As KeyValuePair(Of String, String) In Graph.DefaultEdgeAttrs
                WriteAttr(comma, kvp.Key, kvp.Value)
            Next
            ofile.WriteLine("];")
        End If
        For Each sg As Graph In Graph.SubGraphs
            ofile.WriteLine(String.Empty.PadLeft(indent + 2, " "c) & "subgraph " & sg.Name & " {")
            WriteGraph(sg, indent + 4)
            ofile.WriteLine(String.Empty.PadLeft(indent + 2, " "c) & "}")
        Next
        For Each node As Node In Graph.Nodes
            WriteNode(Graph, node, indent)
        Next
        For Each Edge As Edge In Graph.Edges
            ofile.Write(String.Empty.PadLeft(indent + 2, " "c) & Quote(Edge.FromNode.ID) & arrow & Quote(Edge.ToNode.ID))
            If Edge.Attributes.Count > 0 Or Labels = DOTDetail.Minimal Then
                ofile.Write(" [")
                comma = ""
                If Labels = DOTDetail.Minimal Then
                    ofile.Write("id=" & Quote(Edge.ID))
                    comma = ", "
                    If Edge.Attributes.ContainsKey("constraint") Then
                        WriteAttr(comma, "constraint", Edge.Attributes("constraint"))
                    End If
                Else
                    For Each kvp As KeyValuePair(Of String, String) In Edge.Attributes
                        WriteAttr(comma, kvp.Key, kvp.Value)
                    Next
                End If
                ofile.Write("]")
            End If
            ofile.WriteLine(";")
        Next
    End Sub
    Private Sub WriteNode(ByVal graph As Graph, ByVal node As Node, ByVal indent As Integer)
        ofile.Write(String.Empty.PadLeft(indent + 2, " "c) & Quote(node.ID) & " [")
        Dim comma As String = ""
        BumpProgress()
        For Each kvp As KeyValuePair(Of String, String) In node.Attributes
            If Labels = DOTDetail.Full OrElse _
                " width height shape sides ".Contains(" " & kvp.Key & " ") Then
                WriteAttr(comma, kvp.Key, kvp.Value)
            End If
        Next
        ofile.WriteLine("];")
    End Sub
    Private Sub WriteAttr(ByRef comma As String, ByVal lhs As String, ByVal rhs As String)
        If GraphvizGraphOption.ContainsKey(lhs) Then
            ofile.Write(comma & lhs & "=" & QuoteIf(rhs))
            comma = ", "
        End If
    End Sub
End Module
