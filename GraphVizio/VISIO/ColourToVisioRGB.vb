Imports System.Drawing
Imports System.Globalization
Module _ColourToVisioRGB
    Function ColourToVisioRGB(ByVal Colour As String) As String
        Dim r As Integer = 127
        Dim g As Integer = 127
        Dim b As Integer = 127
        Dim mycolour As String = Colour
        Try
            mycolour = mycolour.Replace(",", " ") ' Commas are allowed in HSV, skip them
            If mycolour.StartsWith("#") Then
                r = Convert.ToInt32(mycolour.Substring(1, 2), 16)
                g = Convert.ToInt32(mycolour.Substring(3, 2), 16)
                b = Convert.ToInt32(mycolour.Substring(5, 2), 16)
            ElseIf mycolour.Contains(" ") Then ' HSV
                Dim hsv() As String = mycolour.Split(" "c)
                Dim hsl As New RGBHSL.HSL
                hsl.H = Convert.ToDouble(hsv(0), CultureInfo.InvariantCulture)
                hsl.S = Convert.ToDouble(hsv(1), CultureInfo.InvariantCulture)
                hsl.L = Convert.ToDouble(hsv(2), CultureInfo.InvariantCulture)
                Dim answer As Color = RGBHSL.HSL_to_RGB(hsl)
                r = answer.R
                g = answer.G
                b = answer.B
            Else ' named mycolour using the X11 standard
                mycolour = mycolour.Replace("grey", "gray") ' Bloody Americans...
                Dim modifier As String = mycolour.Substring(mycolour.Length - 1, 1)
                If modifier >= "1" And modifier <= "4" Then
                    mycolour = mycolour.Substring(0, mycolour.Length - 1)
                Else
                    modifier = ""
                End If
                r = Color.FromName(mycolour).R
                g = Color.FromName(mycolour).G
                b = Color.FromName(mycolour).B
                Dim multiplier As Double = 1
                Select Case modifier
                    Case "1"
                    Case "2"
                        multiplier = 0.932
                    Case "3"
                        multiplier = 0.804
                    Case "4"
                        multiplier = 0.548
                End Select
                If multiplier < 1 Then
                    r = CInt(r * multiplier)
                    g = CInt(g * multiplier)
                    b = CInt(b * multiplier)
                End If
            End If
        Catch
            Warning("Colour " & Colour & " invalid, replaced with grey")
        End Try
        Return "RGB(" & r & "," & g & "," & b & ")"
    End Function

End Module
