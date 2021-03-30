Imports System.Collections
Imports System.Collections.Generic
Imports GOLD
Imports System.Windows.Forms.Application
Imports System.IO

Class DotParser

    Private Parser As Parser
    Private Graph As Graph
    Private WarnedAboutPorts As Boolean = False

    Private Class Attribute
        Friend LHS As String
        Friend RHS As String
        Sub New(ByVal var As String, ByVal val As String)
            LHS = var
            RHS = val
        End Sub
    End Class

    Sub New()

        Dim gf As String = LocalDataDirectory() & GRAMMARFILE

        Parser = New Parser(gf)

    End Sub

    Private Sub AddRange(ByRef base As Dictionary(Of String, String), ByVal adding As List(Of Attribute))
        Dim found As String = ""
        For Each attr As Attribute In adding
            If base.ContainsKey(attr.LHS) Then
                base.Item(attr.LHS) = attr.RHS
            Else
                base.Add(attr.LHS, attr.RHS)
            End If
        Next
    End Sub

    Private Sub AddRange(ByRef base As Dictionary(Of String, String), ByVal adding As Dictionary(Of String, String))
        Dim found As String = ""
        For Each kvp As KeyValuePair(Of String, String) In adding
            If base.ContainsKey(kvp.Key) Then
                base.Item(kvp.Key) = kvp.Value
            Else
                base.Add(kvp.Key, kvp.Value)
            End If
        Next
    End Sub

    Private Function DescribeParserError(ByVal msg As String, ByVal filename As String) As String
        Dim ans As String = ""
        Try
            Dim token As String = "[no current token]"
            If Parser.CurrentToken IsNot Nothing Then
                token = Parser.CurrentToken.ToString
            End If
            Dim symbols As String = "[no valid symbols]"
            If Parser.ExpectedSymbols IsNot Nothing Then
                token = Parser.ExpectedSymbols.Text
            End If
            ans = msg & " in " & filename & _
              " at line " & Parser.CurrentPosition.Line & " column " & Parser.CurrentPosition.Column & _
                " token '" & token & "'." & _
                " Valid tokens : " & symbols
        Catch ex As Exception
            ans = "Unable to describe parse error: " & ex.ToString
        End Try
        Return ans
    End Function

    Friend Sub DoStatement(ByVal graph As Graph, ByVal stmt As Object)
        If TypeOf stmt Is Node Then
            Dim node As Node = CType(stmt, Node)
            If node.ID = " graph" Then
                AddRange(graph.GraphAttrs, node.Attributes)
            ElseIf node.ID = " node" Then
                AddRange(graph.DefaultNodeAttrs, node.Attributes)
            ElseIf node.ID = " edge" Then
                AddRange(graph.DefaultEdgeAttrs, node.Attributes)
            Else
                If node.Graph Is Nothing Then
                    graph.AddNode(node)
                End If
            End If
        ElseIf TypeOf stmt Is Edge Then
            graph.AddEdge(CType(stmt, Edge))
        ElseIf TypeOf stmt Is List(Of Edge) Then
            For Each edge As Edge In CType(stmt, List(Of Edge))
                graph.AddEdge(edge)
            Next
        ElseIf TypeOf stmt Is Graph Then
            graph.AddSubGraph(CType(stmt, Graph))
        ElseIf TypeOf stmt Is List(Of Attribute) Then
            AddRange(graph.GraphAttrs, CType(stmt, List(Of Attribute)))
        End If
    End Sub

    Private Sub DoStatements(ByVal graph As Graph, ByVal stmts As List(Of Object))
        For Each stmt As Object In stmts
            If TypeOf stmt Is List(Of Object) Then
                For Each obj As Object In CType(stmt, IEnumerable(Of Object))
                    DoStatement(graph, stmt)
                Next
            Else
                DoStatement(graph, stmt)
            End If
        Next
    End Sub

    Function LoadDOT(ByVal ifile As String) As Graph

        Graph = New Graph(ROOTGRAPHNAME)

        If ParseFile(ifile) Then
            Return Graph
        Else
            Throw New Exception("Unable to parse " & ifile)
        End If

    End Function

    'Sub DoReductionOld(ByVal r As Reduction)
    '    DoEvents() ' Let progress move
    '    Dim tokens As ArrayList = r.Tokens
    '    If r.Tokens.Count = 1 Then
    '        If TypeOf r.Tokens(0).data Is Reduction Then
    '            r.Tag = r.Tokens(0).data.tag
    '        End If
    '    End If
    '    Select Case r.ParentRule.TableIndex
    '        Case RuleConstants.Rule_Graph_Lbrace_Rbrace ' <graph> ::= <strict> <graph type> <id> '{' <stmt list> '}'
    '        Case RuleConstants.Rule_Strict_Strict ' <strict> ::= strict
    '            r.Tag = "strict"
    '        Case RuleConstants.Rule_Strict ' <strict> ::=
    '            r.Tag = ""
    '        Case RuleConstants.Rule_Graphtype_Digraph ' <graph type> ::= digraph
    '            r.Tag = "digraph"
    '        Case RuleConstants.Rule_Graphtype_Graph ' <graph type> ::= graph
    '            r.Tag = "graph"
    '        Case RuleConstants.Rule_Stmtlist ' <stmt list> ::= <stmt> <stmt list>
    '            Dim stmtlist As List(Of Object) = CType(r.Tokens(1).data.tag, List(Of Object))
    '            stmtlist.Insert(0, r.Tokens(0).data.tag)
    '            r.Tag = stmtlist
    '        Case RuleConstants.Rule_Stmtlist_Semi ' <stmt list> ::= <stmt> ';' <stmt list>
    '            Dim stmtlist As List(Of Object) = CType(r.Tokens(2).data.tag, List(Of Object))
    '            stmtlist.Insert(0, r.Tokens(0).data.tag)
    '            r.Tag = stmtlist
    '        Case RuleConstants.Rule_Stmtlist2 ' <stmt list> ::=
    '            r.Tag = New List(Of Object)
    '        Case RuleConstants.Rule_Attrstmt ' <attr stmt> ::= <attr noun> <attr list>
    '            Dim node As Node = CType(r.Tokens(0).data.tag, Node)
    '            AddRange(node.Attributes, CType(r.Tokens(1).data.tag, List(Of Attribute)))
    '            r.Tag = node
    '        Case RuleConstants.Rule_Attrnoun_Graph ' <attr noun> ::= graph
    '            r.Tag = New Node(" graph")
    '        Case RuleConstants.Rule_Attrnoun_Node ' <attr noun> ::= node
    '            r.Tag = New Node(" node")
    '        Case RuleConstants.Rule_Attrnoun_Edge ' <attr noun> ::= edge
    '            r.Tag = New Node(" edge")
    '        Case RuleConstants.Rule_Attrlist_Lbracket_Rbracket ' <attr list> ::= '[' <a list> ']'
    '            r.Tag = r.Tokens(1).data.tag
    '        Case RuleConstants.Rule_Attrlist_Lbracket_Rbracket2 ' <attr list> ::= '[' ']'
    '            r.Tag = Nothing
    '        Case RuleConstants.Rule_Alist_Comma ' <a list> ::= <attr Attribute> ',' <a list>
    '            Dim attrlist As List(Of Attribute) = CType(r.Tokens(0).data.tag, List(Of Attribute))
    '            Dim adding As List(Of Attribute) = CType(r.Tokens(2).data.tag, List(Of Attribute))
    '            attrlist.AddRange(adding)
    '            r.Tag = attrlist
    '        Case RuleConstants.Rule_Alist2 ' <a list> ::= <attr Attribute> <a list>
    '            Dim attrlist As List(Of Attribute) = CType(r.Tokens(0).data.tag, List(Of Attribute))
    '            Dim adding As List(Of Attribute) = CType(r.Tokens(1).data.tag, List(Of Attribute))
    '            attrlist.AddRange(adding)
    '            r.Tag = attrlist
    '        Case RuleConstants.Rule_Attrattribute_Eq ' <attr Attribute> ::= <id> '=' <id>
    '            Dim attrlist As New List(Of Attribute)
    '            Dim attr As New Attribute(CType(r.Tokens(0).data.tag, String), CType(r.Tokens(2).data.tag, String))
    '            attrlist.Add(attr)
    '            r.Tag = attrlist
    '        Case RuleConstants.Rule_Nodestmt ' <node stmt> ::= <node id>
    '            Dim node As Node = Graph.FindNode(CType(r.Tokens(0).data.tag, String))
    '            r.Tag = node
    '        Case RuleConstants.Rule_Nodestmt2 ' <node stmt> ::= <node id> <attr list>
    '            Dim node As Node = Graph.FindNode(CType(r.Tokens(0).data.tag, String))
    '            For Each attr As Attribute In CType(r.Tokens(1).data.tag, List(Of Attribute))
    '                node.SetAttribute(attr.LHS, attr.RHS)
    '            Next
    '            r.Tag = node
    '        Case RuleConstants.Rule_Nodeid2, _
    '                RuleConstants.Rule_Port, _
    '                RuleConstants.Rule_Port2, _
    '                RuleConstants.Rule_Port3, _
    '                RuleConstants.Rule_Port4, _
    '                RuleConstants.Rule_Portlocation_Colon, _
    '                RuleConstants.Rule_Portlocation_Colon_Lparan_Comma_Rparan, _
    '                RuleConstants.Rule_Portangle_At
    '            If Not WarnedAboutPorts Then
    '                Throw New GraphVizioException("Port specifications not supported, sorry")
    '            End If
    '        Case RuleConstants.Rule_Edgestmt ' <edge stmt> ::= <node id> <edgeRHS>
    '            Dim edges As List(Of Edge)
    '            edges = CType(r.Tokens(1).data.tag, List(Of Edge))
    '            edges(0).FromNode = Graph.FindNode(CType(r.Tokens(0).data.tag, String))
    '            r.Tag = edges
    '        Case RuleConstants.Rule_Edgestmt2 ' <edge stmt> ::= <node id> <edgeRHS> <attr list>
    '            Dim edges As List(Of Edge)
    '            edges = CType(r.Tokens(1).data.tag, List(Of Edge))
    '            edges(0).FromNode = Graph.FindNode(CStr(r.Tokens(0).data.tag))
    '            For Each edge As Edge In edges
    '                For Each attr As Attribute In CType(r.Tokens(2).data.tag, List(Of Attribute))
    '                    edge.SetAttribute(attr.LHS, attr.RHS)
    '                Next
    '            Next
    '            r.Tag = edges
    '            ' <edge stmt> ::= <subgraph> <edgeRHS>
    '            ' <edge stmt> ::= <subgraph> <edgeRHS> <attr list>
    '        Case RuleConstants.Rule_Edgestmt3, _
    '             RuleConstants.Rule_Edgestmt4

    '        Case RuleConstants.Rule_Edgerhs_Edgeop ' <edgeRHS> ::= edgeop <node id>
    '            Dim edges As New List(Of Edge)
    '            Dim edge As New Edge
    '            edge.ToNode = Graph.FindNode(CStr(r.Tokens(1).data.tag))
    '            edges.Add(edge)
    '            r.Tag = edges
    '        Case RuleConstants.Rule_Edgerhs_Edgeop2 ' <edgeRHS> ::= edgeop <node id> <edgeRHS>
    '            Dim edge As New Edge
    '            Dim edges As List(Of Edge)
    '            Dim node As Node = Graph.FindNode(CStr(r.Tokens(1).data.tag))
    '            edge.ToNode = node
    '            edges = CType(r.Tokens(2).data.tag, List(Of Edge))
    '            edges(0).FromNode = node
    '            edges.Insert(0, edge)
    '            r.Tag = edges
    '        Case RuleConstants.Rule_Subgraphstmt_Subgraph_Lbrace_Rbrace ' <subgraph stmt> ::= subgraph <id> '{' <stmt list> '}'
    '            Dim Subgraph As New Graph(CStr(r.Tokens(1).data.tag))
    '            DoStatements(Subgraph, CType(r.Tokens(3).data.tag, List(Of Object)))
    '            r.Tag = Subgraph
    '        Case RuleConstants.Rule_Subgraphstmt_Lbrace_Rbrace ' <subgraph stmt> ::= '{' <stmt list> '}'
    '            Dim Subgraph As New Graph(UniqueName("cluster_"))
    '            DoStatements(Subgraph, CType(r.Tokens(1).data.tag, List(Of Object)))
    '            r.Tag = Subgraph
    '        Case RuleConstants.Rule_Subgraphstmt_Subgraph_Semi ' <subgraph stmt> ::= subgraph <id> ';'
    '            Dim Subgraph As New Graph(CStr(r.Tokens(1).data.tag))
    '            r.Tag = Subgraph
    '        Case RuleConstants.Rule_Subgraph_Subgraph ' <subgraph> ::= subgraph <id>
    '            Dim Subgraph As New Graph(CStr(r.Tokens(1).data.tag))
    '            r.Tag = Subgraph
    '        Case RuleConstants.Rule_Subgraph_Lbrace_Rbrace ' <subgraph> ::= '{' <stmt list> '}'
    '            Dim Subgraph As New Graph(UniqueName("cluster_"))
    '            DoStatements(Subgraph, CType(r.Tokens(1).data.tag, List(Of Object)))
    '            r.Tag = Subgraph
    '        Case RuleConstants.Rule_Subgraph_Subgraph_Lbrace_Rbrace ' <subgraph> ::= subgraph <id> '{' <stmt list> '}'
    '            Dim Subgraph As New Graph(CStr(r.Tokens(1).data.tag))
    '            DoStatements(Subgraph, CType(r.Tokens(3).data.tag, List(Of Object)))
    '            r.Tag = Subgraph
    '        Case RuleConstants.Rule_Id_Variable ' <id> ::= variable
    '            r.Tag = r.Tokens(0).ToString
    '        Case RuleConstants.Rule_Id_Number ' <id> ::= number
    '            r.Tag = r.Tokens(0).ToString
    '        Case RuleConstants.Rule_Id_Stringlit ' <id> ::= stringlit
    '            r.Tag = UnQuote(r.Tokens(0).ToString)

    '            ' simple reductions here just to document
    '        Case RuleConstants.Rule_Stmt ' <stmt> ::= <attr stmt>
    '        Case RuleConstants.Rule_Stmt2 ' <stmt> ::= <node stmt>
    '        Case RuleConstants.Rule_Stmt3 ' <stmt> ::= <edge stmt>
    '        Case RuleConstants.Rule_Stmt4 ' <stmt> ::= <subgraph stmt>
    '        Case RuleConstants.Rule_Stmt5 ' <stmt> ::= <attr Attribute>
    '        Case RuleConstants.Rule_Alist ' <a list> ::= <attr Attribute>
    '        Case RuleConstants.Rule_Nodeid ' <node id> ::= <id>

    '        Case Else
    '            Parser.CloseFile()
    '            Throw New GraphVizioException("ReadDOT reduction error" & _
    '                " at line " & Parser.CurrentLineNumber & _
    '                " token '" & Parser.CurrentToken.ToString & _
    '                "' Reduction " & r.ParentRule.TableIndex & " unknown")
    '    End Select
    'End Sub

End Class