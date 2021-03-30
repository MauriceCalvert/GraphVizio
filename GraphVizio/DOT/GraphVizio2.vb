﻿'Generated by the GOLD Parser Builder

Option Explicit On
Option Strict Off

Imports System.IO
Imports System.Windows.Forms;


Module MyParser
    Private Parser As New GOLD.Parser

    Private Enum SymbolIndex
        [Eof] = 0                                 ' (EOF)
        [Error] = 1                               ' (Error)
        [Comment] = 2                             ' Comment
        [Newline] = 3                             ' NewLine
        [Whitespace] = 4                          ' Whitespace
        [Timesdiv] = 5                            ' '*/'
        [Divtimes] = 6                            ' '/*'
        [Divdiv] = 7                              ' '//'
        [Minus] = 8                               ' '-'
        [Lparen] = 9                              ' '('
        [Rparen] = 10                             ' ')'
        [Comma] = 11                              ' ','
        [Colon] = 12                              ' ':'
        [Semi] = 13                               ' ';'
        [At] = 14                                 ' '@'
        [Lbracket] = 15                           ' '['
        [Rbracket] = 16                           ' ']'
        [Lbrace] = 17                             ' '{'
        [Rbrace] = 18                             ' '}'
        [Plus] = 19                               ' '+'
        [Eq] = 20                                 ' '='
        [Digraph] = 21                            ' digraph
        [Edge] = 22                               ' edge
        [Edgeop] = 23                             ' edgeop
        [Float] = 24                              ' float
        [Graph] = 25                              ' graph
        [Integer] = 26                            ' integer
        [Node] = 27                               ' node
        [Strict] = 28                             ' strict
        [Stringlit] = 29                          ' stringlit
        [Subgraph] = 30                           ' subgraph
        [Variable] = 31                           ' variable
        [Alist] = 32                              ' <a list>
        [Attrattribute] = 33                      ' <attr Attribute>
        [Attrlist] = 34                           ' <attr list>
        [Attrnoun] = 35                           ' <attr noun>
        [Attrstmt] = 36                           ' <attr stmt>
        [Edgestmt] = 37                           ' <edge stmt>
        [Edgerhs] = 38                            ' <edgeRHS>
        [Graph2] = 39                             ' <graph>
        [Graphtype] = 40                          ' <graph type>
        [Id] = 41                                 ' <id>
        [Nodeid] = 42                             ' <node id>
        [Nodestmt] = 43                           ' <node stmt>
        [Number] = 44                             ' <number>
        [Optionalid] = 45                         ' <optionalid>
        [Port] = 46                               ' <port>
        [Portangle] = 47                          ' <port angle>
        [Portlocation] = 48                       ' <port location>
        [Stmt] = 49                               ' <stmt>
        [Stmtlist] = 50                           ' <stmt list>
        [Strict2] = 51                            ' <strict>
        [Subgraph2] = 52                          ' <subgraph>
        [Subgraphstmt] = 53                       ' <subgraph stmt>
        [Unsigned] = 54                           ' <unsigned>
    End Enum

    Private Enum ProductionIndex
        [Unsigned_Integer] = 0                    ' <unsigned> ::= integer
        [Unsigned_Float] = 1                      ' <unsigned> ::= float
        [Number] = 2                              ' <number> ::= <unsigned>
        [Number_Plus] = 3                         ' <number> ::= '+' <unsigned>
        [Number_Minus] = 4                        ' <number> ::= '-' <unsigned>
        [Id_Variable] = 5                         ' <id> ::= variable
        [Id] = 6                                  ' <id> ::= <number>
        [Id_Stringlit] = 7                        ' <id> ::= stringlit
        [Optionalid] = 8                          ' <optionalid> ::= <id>
        [Optionalid2] = 9                         ' <optionalid> ::= 
        [Graph_Lbrace_Rbrace] = 10                ' <graph> ::= <strict> <graph type> <optionalid> '{' <stmt list> '}'
        [Strict_Strict] = 11                      ' <strict> ::= strict
        [Strict] = 12                             ' <strict> ::= 
        [Graphtype_Digraph] = 13                  ' <graph type> ::= digraph
        [Graphtype_Graph] = 14                    ' <graph type> ::= graph
        [Stmtlist] = 15                           ' <stmt list> ::= <stmt> <stmt list>
        [Stmtlist_Semi] = 16                      ' <stmt list> ::= <stmt> ';' <stmt list>
        [Stmtlist2] = 17                          ' <stmt list> ::= 
        [Stmt] = 18                               ' <stmt> ::= <attr stmt>
        [Stmt2] = 19                              ' <stmt> ::= <node stmt>
        [Stmt3] = 20                              ' <stmt> ::= <edge stmt>
        [Stmt4] = 21                              ' <stmt> ::= <subgraph stmt>
        [Stmt5] = 22                              ' <stmt> ::= <attr Attribute>
        [Attrstmt] = 23                           ' <attr stmt> ::= <attr noun> <attr list>
        [Attrnoun_Graph] = 24                     ' <attr noun> ::= graph
        [Attrnoun_Node] = 25                      ' <attr noun> ::= node
        [Attrnoun_Edge] = 26                      ' <attr noun> ::= edge
        [Attrlist_Lbracket_Rbracket] = 27         ' <attr list> ::= '[' <a list> ']'
        [Attrlist_Lbracket_Rbracket2] = 28        ' <attr list> ::= '[' ']'
        [Alist] = 29                              ' <a list> ::= <attr Attribute>
        [Alist_Comma] = 30                        ' <a list> ::= <attr Attribute> ',' <a list>
        [Alist2] = 31                             ' <a list> ::= <attr Attribute> <a list>
        [Attrattribute_Eq] = 32                   ' <attr Attribute> ::= <id> '=' <id>
        [Nodestmt] = 33                           ' <node stmt> ::= <node id>
        [Nodestmt2] = 34                          ' <node stmt> ::= <node id> <attr list>
        [Nodeid] = 35                             ' <node id> ::= <id>
        [Nodeid2] = 36                            ' <node id> ::= <id> <port>
        [Port] = 37                               ' <port> ::= <port location>
        [Port2] = 38                              ' <port> ::= <port angle>
        [Port3] = 39                              ' <port> ::= <port location> <port angle>
        [Port4] = 40                              ' <port> ::= <port angle> <port location>
        [Portlocation_Colon] = 41                 ' <port location> ::= ':' <id>
        [Portlocation_Colon_Lparen_Comma_Rparen] = 42 ' <port location> ::= ':' <id> '(' <id> ',' <id> ')'
        [Portangle_At] = 43                       ' <port angle> ::= '@' <id>
        [Edgestmt] = 44                           ' <edge stmt> ::= <node id> <edgeRHS>
        [Edgestmt2] = 45                          ' <edge stmt> ::= <node id> <edgeRHS> <attr list>
        [Edgestmt3] = 46                          ' <edge stmt> ::= <subgraph> <edgeRHS>
        [Edgestmt4] = 47                          ' <edge stmt> ::= <subgraph> <edgeRHS> <attr list>
        [Edgerhs_Edgeop] = 48                     ' <edgeRHS> ::= edgeop <node id>
        [Edgerhs_Edgeop2] = 49                    ' <edgeRHS> ::= edgeop <node id> <edgeRHS>
        [Subgraphstmt_Subgraph_Lbrace_Rbrace] = 50 ' <subgraph stmt> ::= subgraph <id> '{' <stmt list> '}'
        [Subgraphstmt_Lbrace_Rbrace] = 51         ' <subgraph stmt> ::= '{' <stmt list> '}'
        [Subgraphstmt_Subgraph_Semi] = 52         ' <subgraph stmt> ::= subgraph <id> ';'
        [Subgraph_Subgraph] = 53                  ' <subgraph> ::= subgraph <id>
        [Subgraph_Lbrace_Rbrace] = 54             ' <subgraph> ::= '{' <stmt list> '}'
        [Subgraph_Subgraph_Lbrace_Rbrace] = 55    ' <subgraph> ::= subgraph <id> '{' <stmt list> '}'
    End Enum

    Public Program As Object     'You might derive a specific object

    Public Sub Setup()
        'This procedure can be called to load the parse tables. The class can
        'read tables using a BinaryReader.
        
        Parser.LoadTables(Path.Combine(Application.StartupPath, "grammar.egt"))
    End Sub
    
    Public Function Parse(ByVal Reader As TextReader) As Boolean
        'This procedure starts the GOLD Parser Engine and handles each of the
        'messages it returns. Each time a reduction is made, you can create new
        'custom object and reassign the .CurrentReduction property. Otherwise, 
        'the system will use the Reduction object that was returned.
        '
        'The resulting tree will be a pure representation of the language 
        'and will be ready to implement.

        Dim Response As GOLD.ParseMessage
        Dim Done as Boolean                  'Controls when we leave the loop
        Dim Accepted As Boolean = False      'Was the parse successful?

        Accepted = False    'Unless the program is accepted by the parser

        Parser.Open(Reader)
        Parser.TrimReductions = False  'Please read about this feature before enabling  

        Done = False
        Do Until Done
            Response = Parser.Parse()

            Select Case Response              
                Case GOLD.ParseMessage.LexicalError
                    'Cannot recognize token
                    Done = True

                Case GOLD.ParseMessage.SyntaxError
                    'Expecting a different token
                    Done = True

                Case GOLD.ParseMessage.Reduction
                    'Create a customized object to store the reduction
                    .CurrentReduction = CreateNewObject(Parser.CurrentReduction)

                Case GOLD.ParseMessage.Accept
                    'Accepted!
                    'Program = Parser.CurrentReduction  'The root node!                 
                    Done = True
                    Accepted = True

                Case GOLD.ParseMessage.TokenRead
                    'You don't have to do anything here.

                Case GOLD.ParseMessage.InternalError
                    'INTERNAL ERROR! Something is horribly wrong.
                    Done = True

                Case GOLD.ParseMessage.NotLoadedError
                    'This error occurs if the CGT was not loaded.                   
                    Done = True

                Case GOLD.ParseMessage.GroupError 
                    'COMMENT ERROR! Unexpected end of file
                    Done = True
            End Select
        Loop

        Return Accepted
    End Function

    Private Function CreateNewObject(Reduction as GOLD.Reduction) As Object
        Dim Result As Object = Nothing

        With Reduction
            Select Case .Parent.TableIndex                        
                Case ProductionIndex.Unsigned_Integer                 
                    ' <unsigned> ::= integer 

                Case ProductionIndex.Unsigned_Float                 
                    ' <unsigned> ::= float 

                Case ProductionIndex.Number                 
                    ' <number> ::= <unsigned> 

                Case ProductionIndex.Number_Plus                 
                    ' <number> ::= '+' <unsigned> 

                Case ProductionIndex.Number_Minus                 
                    ' <number> ::= '-' <unsigned> 

                Case ProductionIndex.Id_Variable                 
                    ' <id> ::= variable 

                Case ProductionIndex.Id                 
                    ' <id> ::= <number> 

                Case ProductionIndex.Id_Stringlit                 
                    ' <id> ::= stringlit 

                Case ProductionIndex.Optionalid                 
                    ' <optionalid> ::= <id> 

                Case ProductionIndex.Optionalid2                 
                    ' <optionalid> ::=  

                Case ProductionIndex.Graph_Lbrace_Rbrace                 
                    ' <graph> ::= <strict> <graph type> <optionalid> '{' <stmt list> '}' 

                Case ProductionIndex.Strict_Strict                 
                    ' <strict> ::= strict 

                Case ProductionIndex.Strict                 
                    ' <strict> ::=  

                Case ProductionIndex.Graphtype_Digraph                 
                    ' <graph type> ::= digraph 

                Case ProductionIndex.Graphtype_Graph                 
                    ' <graph type> ::= graph 

                Case ProductionIndex.Stmtlist                 
                    ' <stmt list> ::= <stmt> <stmt list> 

                Case ProductionIndex.Stmtlist_Semi                 
                    ' <stmt list> ::= <stmt> ';' <stmt list> 

                Case ProductionIndex.Stmtlist2                 
                    ' <stmt list> ::=  

                Case ProductionIndex.Stmt                 
                    ' <stmt> ::= <attr stmt> 

                Case ProductionIndex.Stmt2                 
                    ' <stmt> ::= <node stmt> 

                Case ProductionIndex.Stmt3                 
                    ' <stmt> ::= <edge stmt> 

                Case ProductionIndex.Stmt4                 
                    ' <stmt> ::= <subgraph stmt> 

                Case ProductionIndex.Stmt5                 
                    ' <stmt> ::= <attr Attribute> 

                Case ProductionIndex.Attrstmt                 
                    ' <attr stmt> ::= <attr noun> <attr list> 

                Case ProductionIndex.Attrnoun_Graph                 
                    ' <attr noun> ::= graph 

                Case ProductionIndex.Attrnoun_Node                 
                    ' <attr noun> ::= node 

                Case ProductionIndex.Attrnoun_Edge                 
                    ' <attr noun> ::= edge 

                Case ProductionIndex.Attrlist_Lbracket_Rbracket                 
                    ' <attr list> ::= '[' <a list> ']' 

                Case ProductionIndex.Attrlist_Lbracket_Rbracket2                 
                    ' <attr list> ::= '[' ']' 

                Case ProductionIndex.Alist                 
                    ' <a list> ::= <attr Attribute> 

                Case ProductionIndex.Alist_Comma                 
                    ' <a list> ::= <attr Attribute> ',' <a list> 

                Case ProductionIndex.Alist2                 
                    ' <a list> ::= <attr Attribute> <a list> 

                Case ProductionIndex.Attrattribute_Eq                 
                    ' <attr Attribute> ::= <id> '=' <id> 

                Case ProductionIndex.Nodestmt                 
                    ' <node stmt> ::= <node id> 

                Case ProductionIndex.Nodestmt2                 
                    ' <node stmt> ::= <node id> <attr list> 

                Case ProductionIndex.Nodeid                 
                    ' <node id> ::= <id> 

                Case ProductionIndex.Nodeid2                 
                    ' <node id> ::= <id> <port> 

                Case ProductionIndex.Port                 
                    ' <port> ::= <port location> 

                Case ProductionIndex.Port2                 
                    ' <port> ::= <port angle> 

                Case ProductionIndex.Port3                 
                    ' <port> ::= <port location> <port angle> 

                Case ProductionIndex.Port4                 
                    ' <port> ::= <port angle> <port location> 

                Case ProductionIndex.Portlocation_Colon                 
                    ' <port location> ::= ':' <id> 

                Case ProductionIndex.Portlocation_Colon_Lparen_Comma_Rparen                 
                    ' <port location> ::= ':' <id> '(' <id> ',' <id> ')' 

                Case ProductionIndex.Portangle_At                 
                    ' <port angle> ::= '@' <id> 

                Case ProductionIndex.Edgestmt                 
                    ' <edge stmt> ::= <node id> <edgeRHS> 

                Case ProductionIndex.Edgestmt2                 
                    ' <edge stmt> ::= <node id> <edgeRHS> <attr list> 

                Case ProductionIndex.Edgestmt3                 
                    ' <edge stmt> ::= <subgraph> <edgeRHS> 

                Case ProductionIndex.Edgestmt4                 
                    ' <edge stmt> ::= <subgraph> <edgeRHS> <attr list> 

                Case ProductionIndex.Edgerhs_Edgeop                 
                    ' <edgeRHS> ::= edgeop <node id> 

                Case ProductionIndex.Edgerhs_Edgeop2                 
                    ' <edgeRHS> ::= edgeop <node id> <edgeRHS> 

                Case ProductionIndex.Subgraphstmt_Subgraph_Lbrace_Rbrace                 
                    ' <subgraph stmt> ::= subgraph <id> '{' <stmt list> '}' 

                Case ProductionIndex.Subgraphstmt_Lbrace_Rbrace                 
                    ' <subgraph stmt> ::= '{' <stmt list> '}' 

                Case ProductionIndex.Subgraphstmt_Subgraph_Semi                 
                    ' <subgraph stmt> ::= subgraph <id> ';' 

                Case ProductionIndex.Subgraph_Subgraph                 
                    ' <subgraph> ::= subgraph <id> 

                Case ProductionIndex.Subgraph_Lbrace_Rbrace                 
                    ' <subgraph> ::= '{' <stmt list> '}' 

                Case ProductionIndex.Subgraph_Subgraph_Lbrace_Rbrace                 
                    ' <subgraph> ::= subgraph <id> '{' <stmt list> '}' 

            End Select
        End With     

        Return Result
    End Function
End Module