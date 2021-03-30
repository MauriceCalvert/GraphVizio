Imports System.Globalization
Module _ParseLabelPosition
    Function ParseLabelPosition(ByVal lps As String, ByRef lp() As Double) As Boolean
        Try
            Dim xy As String() = lps.Split(New [Char]() {","c})
            For i As Integer = 0 To 1
                lp(i) = Convert.ToDouble(xy(i), CultureInfo.InvariantCulture) / 72
            Next
            Return True
        Catch
            Warning("Label position '" & lps & "' invalid")
            Return False
        End Try
    End Function
End Module
