Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
Module _ExtractSpline
    Function ExtractSpline(ByVal pos As String) As List(Of Coordinate)
        Dim spline As New List(Of Coordinate)
        If pos = "" Then Return spline
        Try
            ' This can be a bit messy. Splines can look like this: 
            ' s,416,197 e,477,168 424,193 436,188 449,182 462,176 462,176 462,176 463,176
            ' S and E mark the start and end knots and can be anywhere. Remove them before parsing the rest
            pos = CleanupPos(pos) & " " ' add a space to ease case ".... e,123,456" (so that indexof always finds a space)
            Dim knot As String = ""
            Dim kp As Integer
            kp = pos.IndexOf("s,") ' start point defined ?
            If kp >= 0 Then
                Dim sp As Integer
                sp = pos.IndexOf(" ", kp)
                knot = pos.Substring(kp + 2, sp - kp - 2) ' extract start knot
                pos = pos.Remove(kp, sp - kp) ' remove it from where it is
                pos = knot & " " & pos ' and add it at the beginning
            End If
            kp = pos.IndexOf("e,") ' end point defined ?
            If kp >= 0 Then
                Dim sp As Integer
                sp = pos.IndexOf(" ", kp)
                knot = pos.Substring(kp + 2, sp - kp - 2) ' extract end knot
                pos = pos.Remove(kp, sp - kp) ' remove it from where it is
                pos = pos & knot  ' and add it at the end (pos already has a trailing space)
            End If
            ' remove unwanted spaces before splitting
            pos = pos.Trim
            kp = pos.IndexOf("  ")
            While kp >= 0
                pos = pos.Replace("  ", " ")
                kp = pos.IndexOf("  ")
            End While
            Dim coords As String() = pos.Split(New [Char]() {" "c})
            For i As Integer = 0 To coords.GetUpperBound(0)
                Dim xy As String() = coords(i).Split(New [Char]() {","c})
                Dim sp As New Coordinate
                sp.X = Convert.ToDouble(xy(0), CultureInfo.InvariantCulture) / 72
                sp.Y = Convert.ToDouble(xy(1), CultureInfo.InvariantCulture) / 72
                If i > 0 Then ' Check for sucessive knots at identical locations
                    If sp.X = spline(spline.Count - 1).X AndAlso sp.Y = spline(spline.Count - 1).Y Then
                        sp = Nothing
                    End If
                End If
                If sp IsNot Nothing Then
                    spline.Add(sp)
                End If
            Next
            If spline.Count = 1 Then
                ' All knots have identical X,Y. This is a bug seen in NEATO when shapes are on top of each-other
                ' Add a fake knot, just next to the first knot, to make the spline valid (nobody will notice).
                Dim sp As New Coordinate
                sp.X = spline(0).X + 1.0 / 72.0
                sp.Y = spline(0).Y + 1.0 / 72.0
                spline.Add(sp)
            End If
        Catch ex As Exception
            Warning("spline '" & pos & "' is invalid, ignored (" & ex.Message & ")")
            spline = New System.Collections.Generic.List(Of Coordinate)
        End Try
        Return spline
    End Function
End Module

