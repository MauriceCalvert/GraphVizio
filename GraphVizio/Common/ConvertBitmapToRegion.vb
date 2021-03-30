Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Module ConvertBitmapToRegion_
    Function ConvertBitmapToRegion(ByVal bm As Bitmap) As Region
        Dim pixel As Color
        Dim result As New Region
        Dim rect As New Rectangle
        Dim pin As Integer = 0
        Dim pout As Integer = 0
        For x = 0 To bm.Width - 1
            For y = 0 To bm.Height - 1
                pixel = bm.GetPixel(x, y)
                If pixel.A > 127 Then ' has some transparency, assume completely transparent
                    pin += 1
                    rect.X = x
                    rect.Y = y
                    result.Union(rect)
                Else
                    pout += 1
                End If
            Next
        Next
        Return result
    End Function
End Module
