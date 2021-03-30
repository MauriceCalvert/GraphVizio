Imports Visio
Module EnsureSection_
    Sub EnsureSection(ByVal shp As Shape, ByVal section As Short)
        If shp.SectionExists(section, 1) = 0 Then
            shp.AddSection(section)
        End If
    End Sub
End Module
