Imports System.Drawing
Module _ColorToRGB
    Function ColorToRGB(ByVal colour As Color) As String
        Return "#" & _
            colour.R.ToString("X2") & _
            colour.G.ToString("X2") & _
            colour.B.ToString("X2")
    End Function
End Module
