Imports System
Module _CreatePolygon
    Sub CreatePolygon(ByRef polygon As Polygon, ByVal sides As Integer, ByVal width As Double, ByVal height As Double)
        Dim theta As Double
        Dim incr As Double = (360 * Math.PI / 180) / sides ' increment in radians
        If sides Mod 2 = 0 Then ' even number of sides = flat top
            theta = (90 * Math.PI / 180) - incr / 2
            polygon.Point(0).X = (width / 2) * Math.Cos(theta)
            polygon.Point(0).Y = (height / 2) * Math.Sin(theta)
        Else ' odd number of sides = vertex at top
            theta = 90 * Math.PI / 180
            polygon.Point(0).X = 0
            polygon.Point(0).Y = (height / 2)
        End If
        ' Set first point vertically upwards
        ' Last point is same as first (polygons are closed)
        polygon.Point(sides).X = polygon.Point(0).X
        polygon.Point(sides).Y = polygon.Point(0).Y
        For side As Integer = 1 To sides - 1
            theta = theta - incr ' minus = draw clockwise
            polygon.Point(side).X = width / 2 * Math.Cos(theta)
            polygon.Point(side).Y = height / 2 * Math.Sin(theta)
        Next
    End Sub
End Module
