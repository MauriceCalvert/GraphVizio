Imports System.Collections
Imports System.Diagnostics
Imports System.Resources

Module GetStreamResource_
    Private myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
    Private myAssemblyPath As String = myAssembly.GetName().Name().Replace(" ", "_")
    Function GetStreamResource(ByVal FileName As String) As System.IO.Stream
        ' Useful for debugging
        'Dim names() As String = myAssembly.GetManifestResourceNames
        'Dim rs As New ResourceSet(myAssembly.GetManifestResourceStream(names(0)))
        'For Each s As DictionaryEntry In rs
        '    Debug.WriteLine($"{s.Key}={s.Value}")
        'Next
        Return myAssembly.GetManifestResourceStream(myAssemblyPath & "." & FileName)
    End Function
End Module
