Friend Class Polygon
    Friend Sides As Integer
    Friend Point() As Coordinate
    Sub New(ByVal nsides As Integer)
        Sides = nsides
        ReDim Point(Sides - 1)
        For i As Integer = 0 To Sides - 1
            Point(i) = New Coordinate
        Next
    End Sub
End Class
