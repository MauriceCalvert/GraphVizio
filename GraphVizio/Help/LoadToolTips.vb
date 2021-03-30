Imports System.Xml
Imports System.Collections.Generic
Module _LoadToolTips
    Sub LoadToolTips()
        Dim tooltipdoc As New XmlDocument()
        tooltipdoc.LoadXml(My.Resources.tooltips)
        ToolTips = New Dictionary(Of String, String)
        Dim root As XmlNode = tooltipdoc.DocumentElement
        Dim nodeList As XmlNodeList = root.SelectNodes("//tooltip[name][text]")
        For Each tooltip As XmlNode In nodeList
            Dim tooltiptext As String = tooltip.ChildNodes(1).InnerText
            tooltiptext = tooltiptext.Replace(vbLf, " ").Trim
            Do While tooltiptext.StartsWith(vbCr)
                tooltiptext = tooltiptext.Substring(1)
            Loop
            Do While tooltiptext.EndsWith(vbCr)
                tooltiptext = tooltiptext.Substring(0, tooltiptext.Length)
            Loop
            Do While tooltiptext.Contains("  ")
                tooltiptext = tooltiptext.Replace("  ", " ")
            Loop
            Do While tooltiptext.Contains(vbCr & " ")
                tooltiptext = tooltiptext.Replace(vbCr & " ", vbCr)
            Loop
            ToolTips.Add(tooltip.ChildNodes(0).InnerText, tooltiptext)
        Next
    End Sub
End Module
