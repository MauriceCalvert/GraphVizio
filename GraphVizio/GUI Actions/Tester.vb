Imports System.Diagnostics
Imports Visio
Imports System.IO
Module _Tester
#If debug Then
    Function Tester() As String
        'Dim s As String = GetWindowsVersion()
        'DumpSelectedShape()
        Dim i As String = ""
        Dim j As String
        j = i.Substring(7)
        Return ""
    End Function
    Function DumpSelectedShape() As String
        Dim Page As IVPage
        Page = myVisioApp.ActivePage
        If Page Is Nothing Then
            Return "No page active"
        End If

        Dim win As Window = myVisioApp.ActiveWindow
        Dim shp As Visio.Shape

        shp = win.Selection.Item(1) ' dump the first item selected on the page
        DumpShape(shp)
        Return ""
    End Function
#End If
End Module
