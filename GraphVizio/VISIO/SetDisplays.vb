Imports Visio
Module _SetDisplays
    Sub SetDisplays(ByVal graph As Graph, ByVal node As Node)
        If graph.Parent IsNot Nothing Then
            SetDisplays(graph.Parent, node) ' Apply defaults from containing graph first. Note recursion !
        End If
        Dim shape As shape = node.Shape
        SetDisplay(node, graph.DefaultNodeAttrs) ' Apply this graph's default attributes
        SetDisplay(node, node.Attributes) ' and finally the attributes for this node
    End Sub
    Sub SetDisplays(ByVal graph As Graph, ByVal edge As Edge)
        If graph.Parent IsNot Nothing Then
            SetDisplays(graph.Parent, edge) ' Apply defaults from containung graph first
        End If
        Dim shape As Shape = edge.Shape
        SetDisplay(graph, edge, graph.DefaultEdgeAttrs) ' Apply this graph's default attributes
        SetDisplay(graph, edge, edge.Attributes) ' and finally the attributes for this edge
    End Sub
End Module
