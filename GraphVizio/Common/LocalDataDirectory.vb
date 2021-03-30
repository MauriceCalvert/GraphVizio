Imports System.Reflection
Imports System.Windows.Forms
Module _LocalDataDirectory
    Function LocalDataDirectory() As String
        Dim assem As [Assembly] = [Assembly].GetExecutingAssembly()
        Return GetExecutingPath() & "\" & assem.GetName.Name & "\"
    End Function
End Module
