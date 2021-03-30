Imports System.Collections
Imports System.Collections.Generic
Imports GOLD
Imports System.Windows.Forms.Application
Imports System.IO

Partial Class DotParser

    Sub DoReduction(r As GOLD.Reduction)

        DoEvents() ' Let progress move
        Dim tokens As ArrayList = r.Tokens

        If r.Tokens.Count = 1 Then ' LHS ::= RHS
            If TypeOf r.Tokens(0).data Is Reduction Then
                r.Tag = r.Tokens(0).data.tag
            End If
        End If

        With r

            Select Case .Parent.TableIndex

                Case ProductionIndex.Unsigned_Integer
                    ' <unsigned> ::= integer 
                    r.Tag = r.Tokens(0).Data

                Case ProductionIndex.Unsigned_Float
                    ' <unsigned> ::= float 
                    r.Tag = r.Tokens(0).Data

                Case ProductionIndex.Number
                    ' <number> ::= <unsigned> 
                    r.Tag = r.Tokens(0).Data.tag

                Case ProductionIndex.Number_Plus
                    ' <number> ::= '+' <unsigned> 
                    r.Tag = r.Tokens(1).Data

                Case ProductionIndex.Number_Minus
                    ' <number> ::= '-' <unsigned> 
                    r.Tag = CStr(-CDbl(r.Tokens(1).Data.tag))

                Case ProductionIndex.Id_Variable
                    ' <id> ::= variable 
                    r.Tag = r.Tokens(0).Data

                Case ProductionIndex.Id
                    ' <id> ::= <number> 
                    r.Tag = r.Tokens(0).Data.tag

                Case ProductionIndex.Id_Stringlit
                    ' <id> ::= stringlit 
                    r.Tag = UnQuote(r.Tokens(0).Data)

                Case ProductionIndex.Graph_Lbrace_Rbrace
                    ' <graph> ::= <strict> <graph type> <id> '{' <stmt list> '}' 
                    If CStr(r.Tokens(0).data.tag) = "strict" Then
                        Graph.Strict = "true"
                    Else
                        Graph.Strict = "false"
                    End If
                    Graph.digraph = (CStr(r.Tokens(1).data.tag) = "digraph").ToString
                    Graph.Name = ROOTGRAPHNAME ' Really should be "CStr(r.Tokens(2).data.tag)" but we have to have a standard root name
                    DoStatements(Graph, CType(r.Tokens(4).data.tag, List(Of Object)))
                    r.Tag = Graph

                Case ProductionIndex.Strict_Strict
                    ' <strict> ::= strict 
                    r.Tag = "strict"

                Case ProductionIndex.Strict
                    ' <strict> ::=  
                    r.Tag = ""

                Case ProductionIndex.Graphtype_Digraph
                    ' <graph type> ::= digraph
                    r.Tag = "digraph"

                Case ProductionIndex.Graphtype_Graph
                    ' <graph type> ::= graph 
                    r.Tag = "graph"

                Case ProductionIndex.Stmtlist
                    ' <stmt list> ::= <stmt> <stmt list> 
                    Dim stmtlist As List(Of Object) = CType(r.Tokens(1).data.tag, List(Of Object))
                    stmtlist.Insert(0, r.Tokens(0).data.tag)
                    r.Tag = stmtlist

                Case ProductionIndex.Stmtlist_Semi
                    ' <stmt list> ::= <stmt> ';' <stmt list> 
                    Dim stmtlist As List(Of Object) = CType(r.Tokens(2).data.tag, List(Of Object))
                    stmtlist.Insert(0, r.Tokens(0).data.tag)
                    r.Tag = stmtlist

                Case ProductionIndex.Stmtlist2
                    ' <stmt list> ::=  
                    r.Tag = New List(Of Object)

                Case ProductionIndex.Stmt
                    ' <stmt> ::= <attr stmt> 
                    ' LHS ::= RHS

                Case ProductionIndex.Stmt2
                    ' <stmt> ::= <node stmt> 
                    ' LHS ::= RHS

                Case ProductionIndex.Stmt3
                    ' <stmt> ::= <edge stmt> 
                    ' LHS ::= RHS

                Case ProductionIndex.Stmt4
                    ' <stmt> ::= <subgraph stmt> 
                    ' LHS ::= RHS

                Case ProductionIndex.Stmt5
                    ' <stmt> ::= <attr Attribute> 
                    ' LHS ::= RHS

                Case ProductionIndex.Attrstmt
                    ' <attr stmt> ::= <attr noun> <attr list> 
                    Dim node As Node = CType(r.Tokens(0).data.tag, Node)
                    AddRange(node.Attributes, CType(r.Tokens(1).data.tag, List(Of Attribute)))
                    r.Tag = node

                Case ProductionIndex.Attrnoun_Graph
                    ' <attr noun> ::= graph 
                    r.Tag = New Node(" graph")

                Case ProductionIndex.Attrnoun_Node
                    ' <attr noun> ::= node 
                    r.Tag = New Node(" node")

                Case ProductionIndex.Attrnoun_Edge
                    ' <attr noun> ::= edge 
                    r.Tag = New Node(" edge")

                Case ProductionIndex.Attrlist_Lbracket_Rbracket
                    ' <attr list> ::= '[' <a list> ']' 
                    r.Tag = r.Tokens(1).data.tag

                Case ProductionIndex.Attrlist_Lbracket_Rbracket2
                    ' <attr list> ::= '[' ']' 
                    r.Tag = Nothing

                Case ProductionIndex.Alist
                    ' <a list> ::= <attr Attribute> 
                    ' LHS ::= RHS

                Case ProductionIndex.Alist_Comma
                    ' <a list> ::= <attr Attribute> ',' <a list> 
                    Dim attrlist As List(Of Attribute) = CType(r.Tokens(0).data.tag, List(Of Attribute))
                    Dim adding As List(Of Attribute) = CType(r.Tokens(2).data.tag, List(Of Attribute))
                    attrlist.AddRange(adding)
                    r.Tag = attrlist

                Case ProductionIndex.Alist2
                    ' <a list> ::= <attr Attribute> <a list> 
                    Dim attrlist As List(Of Attribute) = CType(r.Tokens(0).data.tag, List(Of Attribute))
                    Dim adding As List(Of Attribute) = CType(r.Tokens(1).data.tag, List(Of Attribute))
                    attrlist.AddRange(adding)
                    r.Tag = attrlist

                Case ProductionIndex.Attrattribute_Eq
                    ' <attr Attribute> ::= <id> '=' <id>
                    Dim attrlist As New List(Of Attribute)
                    Dim attr As New Attribute(CType(r.Tokens(0).data.tag, String), CType(r.Tokens(2).data.tag, String))
                    attrlist.Add(attr)
                    r.Tag = attrlist

                Case ProductionIndex.Nodestmt
                    ' <node stmt> ::= <node id> 
                    Dim node As Node = Graph.FindNode(CType(r.Tokens(0).data.tag, String))
                    r.Tag = node

                Case ProductionIndex.Nodestmt2
                    ' <node stmt> ::= <node id> <attr list> 
                    Dim node As Node = Graph.FindNode(CType(r.Tokens(0).data.tag, String))
                    For Each attr As Attribute In CType(r.Tokens(1).data.tag, List(Of Attribute))
                        node.SetAttribute(attr.LHS, attr.RHS)
                    Next
                    r.Tag = node

                Case ProductionIndex.Nodeid
                    ' <node id> ::= <id> 
                    ' LHS ::= RHS


                Case ProductionIndex.Nodeid2,
                     ProductionIndex.Port,
                     ProductionIndex.Port2,
                     ProductionIndex.Port3,
                     ProductionIndex.Port4,
                     ProductionIndex.Portlocation_Colon,
                     ProductionIndex.Portlocation_Colon_Lparen_Comma_Rparen,
                     ProductionIndex.Portangle_At
                    If Not WarnedAboutPorts Then
                        Throw New GraphVizioException("Port specifications not supported, sorry")
                    End If

                Case ProductionIndex.Edgestmt
                    ' <edge stmt> ::= <node id> <edgeRHS> 
                    Dim edges As List(Of Edge)
                    edges = CType(r.Tokens(1).data.tag, List(Of Edge))
                    edges(0).FromNode = Graph.FindNode(CType(r.Tokens(0).data.tag, String))
                    r.Tag = edges

                Case ProductionIndex.Edgestmt2
                    ' <edge stmt> ::= <node id> <edgeRHS> <attr list> 
                    Dim edges As List(Of Edge)
                    edges = CType(r.Tokens(1).data.tag, List(Of Edge))
                    edges(0).FromNode = Graph.FindNode(CStr(r.Tokens(0).data.tag))
                    For Each edge As Edge In edges
                        For Each attr As Attribute In CType(r.Tokens(2).data.tag, List(Of Attribute))
                            edge.SetAttribute(attr.LHS, attr.RHS)
                        Next
                    Next
                    r.Tag = edges

                Case ProductionIndex.Edgestmt3
                    ' <edge stmt> ::= <subgraph> <edgeRHS> 
                    Dim edges As List(Of Edge)
                    Dim subgraph As Graph = CType(r.Tokens(0).data.tag, Graph)
                    edges = CType(r.Tokens(1).data.tag, List(Of Edge))
                    Dim first As Boolean = True
                    For Each node As Node In subgraph.Nodes
                        For Each edge As Edge In edges
                            If first Then
                                Dim newedgefrom As New Edge
                                newedgefrom.FromNode = node
                                newedgefrom.ToNode = edge.FromNode
                                first = False
                            End If
                            Dim newedge As New Edge
                            newedge.FromNode = node
                            newedge.ToNode = edge.ToNode
                        Next
                    Next

                    Dim edgestmt As New List(Of Object)
                    edgestmt.Add(subgraph)
                    edgestmt.Add(edges)
                    r.Tag = edgestmt

                Case ProductionIndex.Edgestmt4
                    ' <edge stmt> ::= <subgraph> <edgeRHS> <attr list> 
                    Dim edges As List(Of Edge)
                    Dim subgraph As Graph = CType(r.Tokens(0).data.tag, Graph)
                    edges = CType(r.Tokens(1).data.tag, List(Of Edge))
                    Dim first As Boolean = True
                    For Each node As Node In subgraph.Nodes
                        For Each edge As Edge In edges
                            If first Then
                                Dim newedgefrom As New Edge
                                newedgefrom.FromNode = node
                                newedgefrom.ToNode = edge.FromNode
                                first = False
                            End If
                            Dim newedge As New Edge
                            newedge.FromNode = node
                            newedge.ToNode = edge.ToNode
                        Next
                    Next

                    For Each edge As Edge In edges
                        For Each attr As Attribute In CType(r.Tokens(2).data.tag, List(Of Attribute))
                            edge.SetAttribute(attr.LHS, attr.RHS)
                        Next
                    Next
                    Dim edgestmt As New List(Of Object)
                    edgestmt.Add(subgraph)
                    edgestmt.Add(edges)
                    r.Tag = edgestmt

                Case ProductionIndex.Edgerhs_Edgeop
                    ' <edgeRHS> ::= edgeop <node id> 
                    Dim edges As New List(Of Edge)
                    Dim edge As New Edge
                    edge.ToNode = Graph.FindNode(CStr(r.Tokens(1).data.tag))
                    edges.Add(edge)
                    r.Tag = edges

                Case ProductionIndex.Edgerhs_Edgeop2
                    ' <edgeRHS> ::= edgeop <node id> <edgeRHS> 
                    Dim edge As New Edge
                    Dim edges As List(Of Edge)
                    Dim node As Node = Graph.FindNode(CStr(r.Tokens(1).data.tag))
                    edge.ToNode = node
                    edges = CType(r.Tokens(2).data.tag, List(Of Edge))
                    edges(0).FromNode = node
                    edges.Insert(0, edge)
                    r.Tag = edges

                Case ProductionIndex.Subgraphstmt_Subgraph_Lbrace_Rbrace
                    ' <subgraph stmt> ::= subgraph <id> '{' <stmt list> '}' 
                    Dim Subgraph As New Graph(CStr(r.Tokens(1).data.tag))
                    DoStatements(Subgraph, CType(r.Tokens(3).data.tag, List(Of Object)))
                    r.Tag = Subgraph

                Case ProductionIndex.Subgraphstmt_Lbrace_Rbrace
                    ' <subgraph stmt> ::= '{' <stmt list> '}' 
                    Dim Subgraph As New Graph(UniqueName("cluster_"))
                    DoStatements(Subgraph, CType(r.Tokens(1).data.tag, List(Of Object)))
                    r.Tag = Subgraph

                Case ProductionIndex.Subgraphstmt_Subgraph_Semi
                    ' <subgraph stmt> ::= subgraph <id> ';' 
                    Dim Subgraph As New Graph(CStr(r.Tokens(1).data.tag))
                    r.Tag = Subgraph

                Case ProductionIndex.Subgraph_Subgraph
                    ' <subgraph> ::= subgraph <id> 
                    Dim Subgraph As New Graph(CStr(r.Tokens(1).data.tag))
                    r.Tag = Subgraph

                Case ProductionIndex.Subgraph_Lbrace_Rbrace
                    ' <subgraph> ::= '{' <stmt list> '}' 
                    Dim Subgraph As New Graph(UniqueName("cluster_"))
                    DoStatements(Subgraph, CType(r.Tokens(1).data.tag, List(Of Object)))
                    r.Tag = Subgraph

                Case ProductionIndex.Subgraph_Subgraph_Lbrace_Rbrace
                    ' <subgraph> ::= subgraph <id> '{' <stmt list> '}' 
                    Dim Subgraph As New Graph(CStr(r.Tokens(1).data.tag))
                    DoStatements(Subgraph, CType(r.Tokens(3).data.tag, List(Of Object)))
                    r.Tag = Subgraph

                Case ProductionIndex.Optionalid
                    ' <optionalid> ::= <id>
                    r.Tag = r.Tokens(0).data.tag

                Case ProductionIndex.Optionalid2
                    ' <optionalid> ::= 
                    r.Tag = ""

                Case Else

                    Parser.Close()

                    Throw (New GraphVizioException("ReadDOT reduction error" & _
                        " at line " & Parser.CurrentPosition.Line & " column " & Parser.CurrentPosition.Column & _
                        " token '" & Parser.CurrentToken.ToString & _
                        "' Reduction " & String.Join("", r.Tokens.ToArray()) & " is unknown"))

            End Select

        End With

    End Sub

End Class
