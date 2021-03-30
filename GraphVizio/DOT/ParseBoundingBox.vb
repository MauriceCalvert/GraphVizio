Imports System.Globalization
Module _ParseBoundingBox
    Function ParseBoundingBox(ByVal bbs As String, ByRef bb() As Double) As Boolean
        Try
            Dim xy As String() = bbs.Split(New [Char]() {","c})
            For i As Integer = 0 To 3
                bb(i) = Convert.ToDouble(xy(i), CultureInfo.InstalledUICulture) / 72
            Next
            Return True
        Catch
            Warning("Page's bounding box '" & bbs & "' invalid")
            Return False
        End Try
    End Function
End Module
