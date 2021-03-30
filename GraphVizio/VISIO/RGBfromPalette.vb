Imports Visio
Module _RGBfromPalette
    Function RGBfromPalette(ByVal palette As Colors, ByVal colour As Integer) As String
        Try ' Sometimes Visio returns colours that are outside the current palette
            Dim r As String = palette(colour).Red.ToString("X2")
            Dim g As String = palette(colour).Green.ToString("X2")
            Dim b As String = palette(colour).Blue.ToString("X2")
            Return "#" & r & g & b
        Catch
            Return "#FF0000"
        End Try
    End Function
End Module
