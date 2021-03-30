Imports Visio
Imports System.Collections.Generic
Imports Visio.VisSelectArgs
Module _ShapesCluster
    Private seennode As Dictionary(Of String, String)
    Private seenedge As Dictionary(Of String, String)
    Private alledges As Dictionary(Of String, Edge).ValueCollection
    Private currentlayer As Layer
    Private ans As String
    Private newlayers As Integer
    Private shapesmoved As Integer
    Function ShapesAutoCluster() As String
        Dim win As Window = myVisioApp.ActiveWindow
        Dim currentpage As IVPage = myVisioApp.ActivePage
        If win Is Nothing Or currentpage Is Nothing Then
            Return ("No document is active")
        End If
        ' delete all existing layers
        Do While currentpage.Layers.Count > 0
            currentpage.Layers(1).Delete(0)
        Loop
        Dim graph As Graph = LoadVisio(currentpage)

        seenedge = New Dictionary(Of String, String)
        seennode = New Dictionary(Of String, String)
        alledges = graph.GlobalEdgeNames.Values
        currentlayer = Nothing
        newlayers = 0
        shapesmoved = 0
        ans = ""
        StartProgress("Clustering...", alledges.Count)
        For Each edge As Edge In alledges
            BumpProgress()
            If Not seenedge.ContainsKey(edge.ID) Then
                seenedge.Add(edge.ID, "")
                If currentlayer Is Nothing Then
                    currentlayer = NewLayer(currentpage)
                    If edge.FromNode.Attributes.ContainsKey("label") Then
                        currentlayer.Name = edge.FromNode.Attribute("label")
                    ElseIf edge.ToNode.Attributes.ContainsKey("label") Then
                        currentlayer.Name = edge.ToNode.Attribute("label")
                    End If
                End If
                currentlayer.Add(edge.Shape, 0)
                Cluster(edge.FromNode)
                Cluster(edge.ToNode)
                currentlayer = Nothing
            End If
        Next
        EndProgress()
        Return ""
    End Function
    Private Sub Cluster(ByVal node As Node)
        If Not seennode.ContainsKey(node.ID) Then
            seennode.Add(node.ID, "")
            currentlayer.Add(node.Shape, 0)
            shapesmoved = shapesmoved + 1
            For Each edge As Edge In node.Edges
                If Not seenedge.ContainsKey(edge.ID) Then
                    seenedge.Add(edge.ID, "")
                    currentlayer.Add(edge.Shape, 0)
                    Cluster(edge.FromNode)
                    Cluster(edge.ToNode)
                End If
            Next
        End If
    End Sub
    Function ShapesCluster() As String
        Dim win As Window = myVisioApp.ActiveWindow
        Dim currentpage As IVPage = myVisioApp.ActivePage
        Dim layer As Layer
        If win Is Nothing Or currentpage Is Nothing Then
            Return ("No document is active")
        End If
        Dim sel As Selection = win.Selection
        ' How many shapes selected (excluding connectors) ?
        Dim shapes As Integer = 0
        For Each shape As Shape In sel
            If Is2D(shape) Then ' a 2D shape
                shapes = shapes + 1
            End If
        Next
        If shapes < 2 Then Return ("You must select at least 2 shapes to cluster")
        layer = NewLayer(currentpage)
        StartProgress("Clustering...", sel.Count)
        For Each shape As Shape In sel
            BumpProgress()
            For i As Short = 1 To shape.LayerCount
                shape.Layer(i).Remove(shape, 0) ' 0=remove connector from any current layers
            Next
            layer.Add(shape, 0)
        Next
        RemoveUnusedLayers(currentpage)
        EndProgress()
        Return ""
    End Function
    Private Function NewLayer(ByVal currentpage As IVPage) As Layer
        ' make a uniquely-named new layer
        Dim layername As String = UniqueName("cluster_")
        Dim layer As Layer
        Do
            layer = Nothing
            For i As Integer = 1 To currentpage.Layers.Count
                If currentpage.Layers(i).Name = layername Then
                    layer = currentpage.Layers(i)
                    layername = UniqueName("cluster_")
                    Exit For
                End If
            Next
        Loop Until layer Is Nothing
        layer = currentpage.Layers.Add(layername)
        newlayers = newlayers + 1
        Return layer
    End Function
    Private Sub RemoveUnusedLayers(ByVal currentpage As IVPage)
        Dim layername As New Dictionary(Of String, Layer)
        For Each shape As Shape In currentpage.Shapes
            For i As Short = 1 To shape.LayerCount
                If Not layername.ContainsKey(shape.Layer(i).Name) Then
                    layername.Add(shape.Layer(i).Name, shape.Layer(i))
                End If
            Next
        Next
        For i As Integer = currentpage.Layers.Count To 1 Step -1
            If Not layername.ContainsKey(currentpage.Layers(i).Name) Then
                currentpage.Layers(i).Delete(0)
            End If
        Next
    End Sub
End Module
