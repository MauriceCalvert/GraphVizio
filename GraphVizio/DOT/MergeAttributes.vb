Imports System.Collections.Generic
Module MergeAttributes_
    Sub MergeAttributes(ByVal graph As Graph)
        MergeAttribute(graph)
        For Each sg As Graph In graph.SubGraphs
            MergeAttribute(sg)
        Next
    End Sub
    Private Sub MergeAttribute(ByVal graph As Graph)
        Dim candidate As Dictionary(Of String, String)
        Dim failed As Dictionary(Of String, String)
        Dim all As Boolean

        ' Merge node attributes up to defaults in graph node    
        candidate = New Dictionary(Of String, String)
        failed = New Dictionary(Of String, String)
        For Each node As Node In graph.Nodes
            For Each pair As KeyValuePair(Of String, String) In node.Attributes
                If candidate.ContainsKey(pair.Key) Then
                    If pair.Value <> candidate(pair.Key) AndAlso _
                        Not failed.ContainsKey(pair.Key) Then
                        failed.Add(pair.Key, pair.Value)
                    End If
                Else
                    candidate.Add(pair.Key, pair.Value)
                End If
            Next
        Next
        For Each pair As KeyValuePair(Of String, String) In failed
            candidate.Remove(pair.Key)
        Next
        For Each pair As KeyValuePair(Of String, String) In candidate
            all = True
            For Each node As Node In graph.Nodes
                If Not node.Attributes.ContainsKey(pair.Key) Then
                    all = False
                    Exit For
                End If
            Next
            If all Then
                graph.DefaultNodeAttrs.Add(pair.Key, pair.Value)
                For Each node As Node In graph.Nodes
                    node.Attributes.Remove(pair.Key)
                Next
            End If
        Next

        ' Merge edge attributes up to defaults in graph edge    
        candidate = New Dictionary(Of String, String)
        failed = New Dictionary(Of String, String)
        For Each edge As Edge In graph.Edges
            For Each pair As KeyValuePair(Of String, String) In edge.Attributes
                If candidate.ContainsKey(pair.Key) Then
                    If pair.Value <> candidate(pair.Key) Then
                        If Not failed.ContainsKey(pair.Key) Then
                            failed.Add(pair.Key, pair.Value)
                        End If
                    End If
                Else
                    candidate.Add(pair.Key, pair.Value)
                End If
            Next
        Next
        For Each pair As KeyValuePair(Of String, String) In failed
            candidate.Remove(pair.Key)
        Next
        For Each pair As KeyValuePair(Of String, String) In candidate
            all = True
            For Each edge As Edge In graph.Edges
                If Not edge.Attributes.ContainsKey(pair.Key) Then
                    all = False
                    Exit For
                End If
            Next
            If all Then
                graph.DefaultEdgeAttrs.Add(pair.Key, pair.Value)
                For Each edge As Edge In graph.Edges
                    edge.Attributes.Remove(pair.Key)
                Next
            End If
        Next
    End Sub
End Module
