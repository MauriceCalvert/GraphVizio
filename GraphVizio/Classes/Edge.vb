Imports Visio
Imports System.Collections.Generic
Imports System.Collections

Friend Class Edge
    Implements IComparable
    Friend Attributes As New Dictionary(Of String, String)
    Friend FromNode As Node = Nothing
    Friend Graph As Graph = Nothing
    Friend ID As String = ""
    Friend Phantom As Boolean = False ' Phantom edge to modify GraphViz layout, not drawn
    Friend Shape As Shape = Nothing
    Friend Spline As New System.Collections.Generic.List(Of Coordinate)
    Friend ToNode As Node = Nothing
    Friend Function CompareTo(ByVal y As Object) As Integer Implements System.IComparable.CompareTo
        Dim other As Edge = CType(y, Edge)
        If Me.FromNode Is other.FromNode Then
            Return Me.ToNode.CompareTo(other.ToNode)
        Else
            Return Me.FromNode.CompareTo(other.FromNode)
        End If
    End Function
    Friend ReadOnly Property Attribute(ByVal Key As String) As String
        Get
            Dim s As String = ""
            Attributes.TryGetValue(Key, s)
            Return s
        End Get
    End Property
    Sub New()
        ID = UniqueName("")
    End Sub
    Public Sub SetAttribute(ByVal key As String, ByVal value As String)
        If key = "id" Then
            Me.ID = value
        Else
            If Attributes.ContainsKey(key) Then
                Attributes.Item(key) = value
            Else
                Attributes.Add(key, value)
            End If
        End If
    End Sub
End Class
