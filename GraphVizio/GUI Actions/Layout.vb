Imports Visio
Imports System.Diagnostics
Module _Layout
    ' must be called by:
    '   DispatchWithProgress(New TaskToDo(AddressOf Layout), True)
    ' so that the progress window works
    Function Layout() As String
        Dim newshapes As Boolean = False
        Dim doc As Document = myvisioapp.ActiveDocument
        Dim Page As IVPage = myVisioApp.ActivePage
        Dim dotfile As String = ""

        If doc Is Nothing Then
            Return ("No Visio document is open")
        End If
        If Page Is Nothing Then
            Return ("No Visio page is active")
        End If

        Dim Graph As Graph = LoadVisio(Page)
        If Graph.GlobalNodeNames.Count = 0 Then
            Return "There is nothing to layout, there are no connected shapes on the page"
        End If
        CurrentSetting.ToGraph(Graph)

        Dim leaves As String = CurrentSetting.Value("leafstack")
        If leaves <> "none" Then
            Do While Page.Layers.Count > 0 ' delete all layers
                Page.Layers(1).Delete(0)
            Loop
            Graph.VerticalLeaves(leaves)
        End If

        Dim shapename As String = CurrentSetting.Value("shapename")
        If CurrentSetting.Value("shapeis") = "true" AndAlso shapename <> SELECTPROMPT Then
            If shapename.EndsWith("+") Then shapename = "rectangle"
            Graph.DefaultNodeAttrs.Add("shape", CurrentSetting.Value("shapename"))
            RemoveShapeParms(Graph)
            newshapes = True
        End If

        If CurrentSetting.Value("rankbyposition") = "true" Then
            Graph.RankByPosition()
        End If

        newshapes = newshapes Or _
            CurrentSetting.Value("shapefillcolours") = "true" Or _
            CurrentSetting.Value("shapelinecolours") = "true" Or _
            CurrentSetting.Value("shapetextcolours") = "true"

        Dim temppath As String = System.IO.Path.GetTempPath
        WriteDOT(Graph, temppath & INPUTFILE, DOTDetail.Minimal)
        Try
            dotfile = RunGraphViz(temppath & INPUTFILE)
        Catch ex As GraphVizioException
            If MsgBox(ex.Message & vbCrLf & vbCrLf & _
                "Would you like to view the graphviz source file?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Notepad(temppath & INPUTFILE)
            End If
            Return ""
        End Try

        Dim NewGraph As Graph = ReadDOT(dotfile)
        Graph.CopyLayoutFrom(NewGraph)
        Graph.Sort()
        Page = DrawGraph(doc, Graph, newshapes)
        Return "" ' all's well
    End Function
    Private Sub RemoveShapeParms(ByVal g As Graph)
        For Each node As Node In g.Nodes
            node.Attributes.Remove("shape")
            node.Attributes.Remove("sides")
            node.Attributes.Remove("color")
            node.Attributes.Remove("fillcolor")
            node.Attributes.Remove("style")
        Next
        For Each sg As Graph In g.SubGraphs
            RemoveShapeParms(sg) ' recursion !
        Next
    End Sub
End Module

