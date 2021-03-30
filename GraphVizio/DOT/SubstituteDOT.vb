Module _SubstituteDOT
    Function SubstituteDOT(ByVal graph As Graph, ByVal node As Node, ByVal text As String) As String
        Dim ans As String = text
        ans = ans.Replace("\N", node.ID)
        ans = ans.Replace("\G", graph.Name)
        ans = ans.Replace("\n", ChrW(8232))
        Return ans
    End Function
    Function SubstituteDOT(ByVal graph As Graph, ByVal edge As Edge, ByVal text As String) As String
        Dim ans As String = text
        ans = ans.Replace("\E", edge.FromNode.ID & "->" & edge.ToNode.ID)
        ans = ans.Replace("\G", graph.Name)
        ans = ans.Replace("\n", ChrW(8232))
        Return ans
    End Function
End Module
