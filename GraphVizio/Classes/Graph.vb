Imports Visio
Imports System.Diagnostics
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
Friend Class Graph
    Friend DefaultEdgeAttrs As New Dictionary(Of String, String)
    Friend DefaultNodeAttrs As New Dictionary(Of String, String)
    Friend Edges As New List(Of Edge) ' Edges in this (sub)graph
    Friend GlobalEdgeNames As New Dictionary(Of String, Edge) ' likewise
    Friend GlobalNodeNames As New Dictionary(Of String, Node) ' Global, all graphs share this
    Friend GraphAttrs As New Dictionary(Of String, String)
    Friend Name As String = ROOTGRAPHNAME
    Friend NodeNames As New Dictionary(Of String, Node) ' Name lookup for NodeNames on this (sub)graph
    Friend Nodes As New List(Of Node) ' Nodes on this (sub)graph
    Friend Parent As Graph = Nothing
    Friend Phantom As Boolean = False
    Friend Strict As String = "false"
    Friend SubGraphs As New List(Of Graph)
    Private myDigraph As String = "false"
    Friend Sub AddEdge(ByVal edge As Edge)
        If edge.FromNode.Graph Is Nothing Then
            edge.FromNode.Graph = Me
            If Not NodeNames.ContainsKey(edge.FromNode.ID) Then
                NodeNames.Add(edge.FromNode.ID, edge.FromNode)
                Nodes.Add(edge.FromNode)
            End If
        End If
        If edge.ToNode.Graph Is Nothing Then
            edge.ToNode.Graph = Me
            If Not NodeNames.ContainsKey(edge.ToNode.ID) Then
                NodeNames.Add(edge.ToNode.ID, edge.ToNode)
                Nodes.Add(edge.ToNode)
            End If
        End If
        edge.Graph = Me
        Edges.Add(edge)
        If Not GlobalEdgeNames.ContainsKey(edge.ID) Then
            GlobalEdgeNames.Add(edge.ID, edge)
        End If
        If Not edge.FromNode.Edges.Contains(edge) Then
            edge.FromNode.Edges.Add(edge)
        End If
        If Not edge.ToNode.Edges.Contains(edge) Then
            edge.ToNode.Edges.Add(edge)
        End If
    End Sub
    Friend Function AddNode(ByVal nodename As String, Optional ByVal phantom As Boolean = False) As Node
        Dim nnode As Node = Nothing
        If GlobalNodeNames.TryGetValue(nodename, nnode) Then
            Return nnode
        End If
        nnode = New Node(nodename)
        nnode.Phantom = phantom
        AddNode(nnode)
        Return nnode
    End Function
    Friend Sub AddNode(ByVal node As Node)
        If Not GlobalNodeNames.ContainsKey(node.ID) Then
            GlobalNodeNames.Add(node.ID, node)
        End If
        If Not NodeNames.ContainsKey(node.ID) Then
            NodeNames.Add(node.ID, node)
            Nodes.Add(node)
        End If
        node.Graph = Me
    End Sub
    Friend Property digraph() As String
        Get
            If Parent Is Nothing Then
                Return myDigraph
            Else
                Return Parent.digraph
            End If
        End Get
        Set(ByVal value As String)
            If Parent Is Nothing Then
                myDigraph = value
            Else
                Parent.digraph = value
            End If
        End Set
    End Property
    Friend Sub RemoveEdge(ByVal Edge As Edge)
        If GlobalEdgeNames.ContainsKey(Edge.ID) Then
            GlobalEdgeNames.Remove(Edge.ID)
        End If
        Edges.Remove(Edge)
    End Sub
    Friend Sub RemoveNode(ByVal node As Node)
        If GlobalNodeNames.ContainsKey(node.ID) Then
            GlobalNodeNames.Remove(node.ID)
        End If
        If NodeNames.ContainsKey(node.ID) Then
            NodeNames.Remove(node.ID)
        End If
        Nodes.Remove(node)
    End Sub
    Friend Sub AddSubGraph(ByVal SubGraph As Graph)
        For Each node As Node In SubGraph.Nodes
            If Not GlobalNodeNames.ContainsKey(node.ID) Then
                GlobalNodeNames.Add(node.ID, node)
            End If
        Next
        For Each edge As Edge In SubGraph.Edges
            If Not GlobalEdgeNames.ContainsKey(edge.ID) Then
                GlobalEdgeNames.Add(edge.ID, edge)
            End If
        Next
        SubGraph.GlobalNodeNames = GlobalNodeNames
        SubGraph.GlobalEdgeNames = GlobalEdgeNames
        SubGraph.Parent = Me
        SubGraphs.Add(SubGraph)
    End Sub
    Function Connect(ByVal FromNode As Node, ByVal ToNode As Node) As Edge
        Dim Edge As New Edge
        Edge.FromNode = FromNode
        Edge.ToNode = ToNode
        AddEdge(Edge)
        Return (Edge)
    End Function
    Friend ReadOnly Property DefaultEdgeAttr(ByVal key As String) As String
        Get
            Dim s As String = ""
            DefaultEdgeAttrs.TryGetValue(key, s)
            Return s
        End Get
    End Property
    Friend ReadOnly Property DefaultNodeAttr(ByVal key As String) As String
        Get
            Dim s As String = ""
            DefaultNodeAttrs.TryGetValue(key, s)
            Return s
        End Get
    End Property
    Private NewEdgeNames As Dictionary(Of String, Edge)
    Friend Sub CopyLayoutFrom(ByVal NewGraph As Graph)
        Dim newnode As Node = Nothing
        Dim newedge As Edge = Nothing
        Me.SetAttribute("bb", NewGraph.GraphAttr("bb"))
        For Each node As Node In Me.GlobalNodeNames.Values
            If NewGraph.GlobalNodeNames.TryGetValue(node.ID, newnode) Then
                node.SetAttribute("pos", newnode.Attribute("pos"))
            End If
        Next
        For Each edge As Edge In Me.GlobalEdgeNames.Values
            If NewGraph.GlobalEdgeNames.TryGetValue(edge.ID, newedge) Then
                edge.SetAttribute("pos", newedge.Attribute("pos"))
            End If
        Next
    End Sub
    Private Sub CopyLayoutFromSubgraph(ByVal g As Graph, ByVal ng As Graph)
        ng.GlobalEdgeNames = NewEdgeNames
        g.SetAttribute("bb", ng.GraphAttr("bb"))
        For Each sg As Graph In g.SubGraphs
            Dim nsg As Graph = ng.SubGraph(sg.Name)
            CopyLayoutFromSubgraph(sg, nsg)
        Next
        For Each node As Node In g.Nodes
            Dim newnode As Node = Nothing
            If ng.GlobalNodeNames.TryGetValue(node.ID, newnode) Then
                node.SetAttribute("pos", newnode.Attribute("pos"))
            End If
        Next
        For Each edge As Edge In g.Edges
            Dim newedge As Edge = Nothing
            If ng.GlobalEdgeNames.TryGetValue(edge.ID, newedge) Then
                edge.SetAttribute("pos", newedge.Attribute("pos"))
            Else
                edge.SetAttribute("pos", "")
            End If
        Next
    End Sub
#If DEBUG Then
    Friend Sub Dump()
        Debug.WriteLine("--------------------------------------")
        DumpGraph(Me, 0)
    End Sub
    Private Sub DumpAttrs(ByVal graphattrs As Dictionary(Of String, String), ByVal space As String)
        If graphattrs IsNot Nothing Then
            If graphattrs.Count > 0 Then
                Debug.Write(space & "Attributes ")
                Dim comma As String = ""
                For Each kvp As KeyValuePair(Of String, String) In graphattrs
                    Debug.Write(comma & kvp.Key & "=" & kvp.Value)
                    comma = ", "
                Next
            End If
        End If
        Debug.WriteLine("")
    End Sub
    Private Direction As String
    Private Sub DumpGraph(ByVal graph As Graph, ByVal spaces As Integer)
        Dim ss As String = "                                      ".Substring(0, spaces)
        If graph.Parent Is Nothing Then ' top level
            If graph.digraph = "true" Then
                Direction = "->"
            Else
                Direction = "--"
            End If
        End If
        Dim stricts As String = ""
        If Strict = "true" Then
            stricts = "strict"
        End If
        Debug.WriteLine(ss & (stricts & " digraph " & digraph & " " & graph.Name).Trim)
        For Each sg As Graph In graph.SubGraphs
            DumpGraph(sg, spaces + 2)
        Next
        If graph.GraphAttrs.Count > 0 Then
            DumpAttrs(graph.GraphAttrs, ss)
        End If
        If graph.DefaultNodeAttrs.Count > 0 Then
            Debug.Write(ss & "Default Node")
            DumpAttrs(graph.DefaultNodeAttrs, ss)
        End If
        If graph.DefaultEdgeAttrs.Count > 0 Then
            Debug.Write(ss & "Default Edge")
            DumpAttrs(graph.DefaultEdgeAttrs, ss)
        End If
        For Each node As Node In graph.Nodes
            Debug.Write(ss & "Node " & node.ID)
            If node.Root Then
                Debug.Write(" root")
            End If
            If node.Leaf Then
                Debug.Write(" leaf")
            End If
            If node.Phantom Then
                Debug.Write(" phantom")
            End If
            Debug.Write(" rank " & node.Rank)
            Debug.Write(" pos " & Convert.ToString(node.Xpos, CultureInfo.InvariantCulture) & "," & Convert.ToString(node.Ypos, CultureInfo.InvariantCulture))
            DumpAttrs(node.Attributes, ss & "  ")
            If node.Shape IsNot Nothing Then
                Debug.WriteLine(ss & "  Shape " & node.Shape.ID & " is " & node.Shape.Text)
            End If
        Next
        For Each cn As Edge In graph.Edges
            Debug.Write(ss & "Edge " & cn.FromNode.ID & Direction & cn.ToNode.ID & " ID=" & cn.ID & " ")
            DumpAttrs(cn.Attributes, ss)
            If cn.Spline.Count > 0 Then
                Debug.Write(ss & "Spline ")
                For Each co As Coordinate In cn.Spline
                    Debug.Write(co.X & "," & co.Y & " ")
                Next
                Debug.WriteLine("")
            End If
        Next
    End Sub
#End If
    ' Find a node from its name, adding it to the graph's global list but to any particular (sub)graph
    ' Used principally by DotParser
    Function FindNode(ByVal ID As String) As Node
        Dim nnode As Node = Nothing
        If Not GlobalNodeNames.TryGetValue(ID, nnode) Then
            nnode = New Node(ID)
            GlobalNodeNames.Add(ID, nnode)
        End If
        Return nnode
    End Function
    Function GraphAttr(ByVal key As String) As String
        Dim s As String = ""
        GraphAttrs.TryGetValue(key, s)
        Return s
    End Function
    Function ItemCount() As Integer
        Return GlobalNodeNames.Count + GlobalEdgeNames.Count
        ' Return ItemsIn(Me)
    End Function
    Private Function ItemsIn(ByVal graph As Graph) As Integer
        Dim ans As Integer = 0
        For Each sg As Graph In graph.SubGraphs
            ans = ans + ItemsIn(sg)
        Next
        Return Nodes.Count + Edges.Count + ans
    End Function
    Sub New(ByVal newname As String)
        Me.Name = newname
    End Sub
    Sub New(ByVal newname As String, ByVal phantom As Boolean)
        Me.Name = newname
        Me.Phantom = phantom
    End Sub
    Function NodeExists(ByVal ID As String) As Boolean
        Return GlobalNodeNames.ContainsKey(ID)
    End Function
    Sub RankByPosition()
        For Each edge As Edge In Me.GlobalEdgeNames.Values
            If edge.FromNode > edge.ToNode Then
                Dim temp As Node
                temp = edge.FromNode
                edge.FromNode = edge.ToNode
                edge.ToNode = temp
            End If
        Next
    End Sub
    Private Sub RankGraph(ByVal graph As Graph)
        For Each node As Node In graph.Nodes
            If node.Root Then RankNode(node, 0)
        Next
        For Each sg As Graph In graph.SubGraphs
            RankGraph(sg)
        Next
    End Sub
    Private Sub RankNode(ByVal node As Node, ByVal rank As Integer)
        If Not node.Marked Then
            node.Rank = rank
            node.Marked = True
            For Each edge As Edge In node.Edges
                RankNode(edge.ToNode, rank + 1)
            Next
        End If
    End Sub
    Private Sub RenameGraphNodes(ByVal graph As Graph)
        For Each sg As Graph In graph.SubGraphs
            RenameGraphNodes(sg)
        Next
        Dim NewNodes As New Dictionary(Of String, Node)
        For Each node As Node In graph.Nodes
            NewNodes.Add(node.NewName, node)
            node.ID = node.NewName
        Next
        graph.NodeNames = NewNodes
    End Sub
    Friend Sub RenameNodes()
        Dim NewNodes As New Dictionary(Of String, Node)
        Dim allnodes As Dictionary(Of String, Node).ValueCollection = GlobalNodeNames.Values
        Dim nodelabel As String
        Dim newlabel As String
        Dim unique As Integer
        For Each node As Node In allnodes
            nodelabel = node.Attribute("label")
            If nodelabel = "" Then
                nodelabel = node.ID
            End If
            unique = 0
            newlabel = nodelabel
            While NewNodes.ContainsKey(newlabel)
                unique = unique + 1
                newlabel = nodelabel & CStr(unique)
            End While
            NewNodes.Add(newlabel, node)
            node.NewName = newlabel
        Next
        RenameGraphNodes(Me)
        GlobalNodeNames = NewNodes
    End Sub
    Friend Sub RemoveAttribute(ByVal key As String)
        If GraphAttrs.ContainsKey(key) Then
            GraphAttrs.Remove(key)
        End If
    End Sub
    Friend Sub SetAttribute(ByVal key As String, ByVal value As String)
        If GraphAttrs.ContainsKey(key) Then
            GraphAttrs.Item(key) = value
        Else
            GraphAttrs.Add(key, value)
        End If
    End Sub
    Friend Sub Sort()
        ' set root and leaf true for all nodes. 
        'Sortgraph will remove as necessary to leave only 
        ' true roots - who nobody points to
        ' true leaves - those who point to nobody
        For Each node As Node In GlobalNodeNames.Values
            node.Root = True
            node.Leaf = True
        Next
        SortGraph(Me)
        RankGraph(Me)
    End Sub
    Private Sub SortGraph(ByVal graph As Graph)
        graph.Nodes.Sort()
        graph.Edges.Sort()
        For Each node As Node In graph.Nodes
            If node.Phantom Then
                node.Root = False
                node.Leaf = False
            Else
                node.Edges.Sort()
            End If
            node.Marked = False
        Next
        For Each edge As Edge In graph.Edges
            If Not edge.Phantom Then
                edge.FromNode.Leaf = False
                edge.ToNode.Root = False
            End If
        Next
        For Each sg As Graph In graph.SubGraphs
            SortGraph(sg)
        Next
    End Sub
    Function SubGraph(ByVal SubGraphName As String) As Graph
        Dim NewSubGraph As Graph = Nothing
        If SubGraphName = Me.Name Then
            Return Me
        End If
        If SubGraphName = "" Then
            SubGraphName = UniqueName("cluster_")
        Else
            For Each sg As Graph In SubGraphs
                If sg.Name = SubGraphName Then
                    Return sg
                End If
            Next
        End If
        If NewSubGraph Is Nothing Then
            NewSubGraph = New Graph(SubGraphName)
            AddSubGraph(NewSubGraph)
        End If
        NewSubGraph.GlobalNodeNames = GlobalNodeNames
        NewSubGraph.Parent = Me
        Return NewSubGraph
    End Function
    Function PhantomEdge(ByVal graph As Graph, ByVal fromnode As Node, ByVal tonode As Node) As Edge
        Dim edge As Edge = New Edge
        edge.Graph = graph
        edge.FromNode = fromnode
        edge.ToNode = tonode
        edge.Phantom = True
        graph.AddEdge(edge)
        Return edge
    End Function
    Private Sub VerticalLeaf(ByVal graph As Graph, ByVal lowest As Integer)
        Dim HiddenGraphs As New List(Of Graph)
        For Each node As Node In graph.Nodes
            Dim onleft As Edge = Nothing
            Dim subgraph1 As Graph = Nothing
            Dim subgraph2 As Graph = Nothing
            Dim invedge As Edge = Nothing
            Dim spacer1 As Node = Nothing
            Dim spacer2 As Node = Nothing
            Dim spacerwidth As Double = 0
            Dim spacealready As Double = 0
            Dim nodeedges As New List(Of Edge)

            ' Get list of edges emanating from this node, that aren't phantom 
            ' and don't connect phantom nodes
            For Each edge As Edge In node.Edges
                If edge.FromNode Is node AndAlso _
                    Not edge.Phantom AndAlso _
                    Not edge.FromNode.Phantom AndAlso _
                    Not edge.ToNode.Phantom Then
                    nodeedges.Add(edge)
                End If
            Next

            For Each edge As Edge In nodeedges
                If edge.ToNode.Leaf AndAlso edge.ToNode.Rank >= lowest Then
                    If onleft Is Nothing Then
                        onleft = edge
                    Else
                        If subgraph1 Is Nothing Then
                            subgraph1 = New Graph(UniqueName("cluster_"), True)
                            subgraph1.SetAttribute("nodesep", "0")
                            HiddenGraphs.Add(subgraph1)
                            subgraph2 = New Graph(UniqueName("cluster_"), True)
                            subgraph2.SetAttribute("nodesep", "0")
                            subgraph1.AddSubGraph(subgraph2)
                            spacer1 = subgraph1.AddNode(UniqueName("spacer"), True)
                            spacer1.SetAttribute("shape", "rectangle")
                            spacer1.SetAttribute("width", "0")
                            spacer1.SetAttribute("label", "")
                            Dim spaceredge1 As Edge
                            spaceredge1 = PhantomEdge(subgraph1, spacer1, onleft.FromNode)
                            spaceredge1.SetAttribute("constraint", "false")
                            spacer2 = subgraph1.AddNode(UniqueName("spacer"), True)
                            spacer2.SetAttribute("width", "0")
                            spacer2.SetAttribute("label", "")
                            spacer2.SetAttribute("shape", "rectangle")
                            Dim spaceredge2 As Edge
                            spaceredge2 = PhantomEdge(subgraph1, onleft.ToNode, spacer2)
                            spaceredge2.SetAttribute("constraint", "false")
                            Dim spaceredge3 As Edge = PhantomEdge(subgraph1, edge.FromNode, spacer2)
                            subgraph1.AddNode(onleft.FromNode)
                            subgraph2.AddNode(onleft.ToNode)
                            Dim spaceredge4 As Edge = PhantomEdge(subgraph1, spacer1, onleft.ToNode)
                            spacealready = onleft.FromNode.Width / 2
                        End If
                        If edge.ToNode.Width > spacerwidth Then
                            spacerwidth = edge.ToNode.Width
                        End If
                        invedge = New Edge
                        invedge.Phantom = True
                        invedge.FromNode = onleft.ToNode
                        invedge.ToNode = edge.ToNode
                        subgraph2.AddEdge(invedge)
                        onleft = edge
                        subgraph2.AddNode(edge.ToNode)
                    End If
                End If
            Next
        Next
        For Each sg As Graph In graph.SubGraphs
            If Not sg.Phantom Then ' Careful not to recurse on stuff we've just done
                VerticalLeaf(sg, lowest)
            End If
        Next
        For Each subgraph As Graph In HiddenGraphs
            For Each node As Node In subgraph.Nodes
                If Not node.Phantom Then
                    graph.Nodes.Remove(node)
                    graph.NodeNames.Remove(node.ID)
                    subgraph.AddNode(node)
                End If
            Next
            graph.AddSubGraph(subgraph)
            For Each ssg As Graph In subgraph.SubGraphs
                For Each node As Node In ssg.Nodes
                    If Not node.Phantom Then
                        graph.Nodes.Remove(node)
                        graph.NodeNames.Remove(node.ID)
                        ssg.AddNode(node)
                    End If
                Next
            Next
        Next
    End Sub

    Friend Sub VerticalLeaves(ByVal style As String)
        Dim lowest As Integer = 0 ' default=leaves at any depth get stacked vertically
        If style = SELECTPROMPT Then Exit Sub
        Me.Sort()
        If style = "lowest" Then
            For Each kvp As KeyValuePair(Of String, Node) In GlobalNodeNames
                If kvp.Value.Rank > lowest Then lowest = kvp.Value.Rank
            Next
        End If
        For Each node As Node In GlobalNodeNames.Values
            node.Marked = False
        Next
        VerticalLeaf(Me, lowest)
    End Sub
End Class
