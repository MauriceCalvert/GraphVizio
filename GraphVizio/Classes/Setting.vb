Imports Visio
Imports System.Diagnostics
Imports System.Windows.Forms
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Friend Class Setting
    Private myForm As Settings = Nothing
    Friend Values As New Dictionary(Of String, String)
    Friend name As String = ""
    Friend ID As Integer
    Private Delegate Sub Invoker()
    Sub New(ByVal newid As Integer)
        ID = newid
        myForm = New Settings(ID)
        Change("algorithm", "dot")
        Change("connectorstyle", "existing")
        Change("connectto", "ideal")
        Change("drawboundingboxes", "true")
        Change("aspectratio", "0")
        Change("leafstack", "none")
        Change("overlap", "prism")
        Change("rankbyposition", "false")
        Change("rankdir", "TB")
        Change("shapefillcolours", "false")
        Change("shapeis", "false")
        Change("shapelinecolours", "false")
        Change("shapetextcolours", "false")
        Change("strict", "false")
        Change("type", "graph")
        Change("seed", "1")
        Change("lockseed", "false")
        PageTable.Add(ID, Me)
    End Sub
    ReadOnly Property Form() As Settings
        Get
            Dim doc As Document
            Dim setid As Integer = -1

            If myForm Is Nothing Then
                doc = myVisioApp.ActiveDocument
                If doc Is Nothing Then
                    CurrentSetting = DefaultSettings
                    Return CurrentSetting.Form
                Else
                    setid = doc.ID
                    myForm = New Settings(setid)
                    FromDocument()
                End If
            End If
            Return myForm
        End Get
    End Property
    Sub Close()
        If myForm.InvokeRequired Then
            myForm.Invoke(New Invoker(AddressOf HideForm))
        Else
            ToDocument()
            PageTable.Remove(ID)
            If CurrentSetting Is Me Then
                CurrentSetting = DefaultSettings
            End If
            myForm.Close()
            myForm = Nothing
        End If
    End Sub
    Sub HideForm()
        If myForm.InvokeRequired Then
            myForm.Invoke(New Invoker(AddressOf HideForm))
        Else
            myForm.Hide()
        End If
    End Sub
    Sub ShowForm()
        If myForm.InvokeRequired Then
            myForm.Invoke(New Invoker(AddressOf ShowForm))
        Else
            myForm.Show()
        End If
    End Sub
    Sub BringToFront()
        If myForm.InvokeRequired Then
            myForm.Invoke(New Invoker(AddressOf BringToFront))
        Else
            myForm.BringToFront()
        End If
    End Sub
    Sub Save(ByVal filename As String)
        Dim fullfilename As String = LocalDataDirectory() & SETTINGSDIRECTORY & "\" & filename & ".txt"
        Dim sw As New StreamWriter(fullfilename, False)
        For Each key As String In Values.Keys
            sw.WriteLine(key & "=" & Values(key))
        Next
        sw.Close()
    End Sub
    Sub Load(ByVal filename As String)
        Dim fullfilename As String = LocalDataDirectory() & SETTINGSDIRECTORY & "\" & filename & ".txt"
        Dim sr As New StreamReader(fullfilename)
        Dim line As String
        Dim parse() As String
        Values = New Dictionary(Of String, String)
        Do While Not sr.EndOfStream
            line = sr.ReadLine

            parse = line.Split(New Char() {"="c}, 2, StringSplitOptions.RemoveEmptyEntries)
            If parse.GetUpperBound(0) > 0 Then
                Change(parse(0), parse(1))
            End If
        Loop
        sr.Close()
    End Sub
    Default Friend Property Value(ByVal key As String) As String
        Get
            If Values.ContainsKey(key) Then
                Return Values(key)
            Else
                Return ("")
            End If
        End Get
        Set(ByVal value As String)
            If value Is Nothing OrElse value = SELECTPROMPT Then
                value = ""
            End If
            If Values.ContainsKey(key) Then
                Values(key) = value
            Else
                Values.Add(key, value)
            End If
        End Set
    End Property
    Friend Sub Remove(ByVal key As String)
        If Values.ContainsKey(key) Then
            Values.Remove(key)
        End If
    End Sub
    Friend Sub Change(ByVal key As String, ByVal newvalue As String)
        Try ' Diagram could have rubbish in settings
            Value(key) = newvalue
            Dim ctrl As Control
            ctrl = FindControl(Form.Controls, key)
            If TypeOf ctrl Is GroupBox Then
                Dim rb As RadioButton = CType(Form.Controls.Find(newvalue, True)(0), RadioButton)
                rb.Checked = True
            ElseIf TypeOf ctrl Is CheckBox Then
                Dim cb As CheckBox = CType(ctrl, CheckBox)
                cb.Checked = newvalue = "true"
            ElseIf TypeOf ctrl Is TextBox Then
                ctrl.Text = newvalue
            ElseIf newvalue IsNot Nothing AndAlso newvalue <> "" Then
                If TypeOf ctrl Is ListBox Then
                    Dim lstbx As ListBox = CType(ctrl, ListBox)
                    lstbx.SelectedItem = newvalue
                ElseIf TypeOf ctrl Is ComboBox Then
                    Dim cmbbx As ComboBox = CType(ctrl, ComboBox)
                    cmbbx.SelectedItem = newvalue
                End If
            End If
        Catch
            Remove(key)
        End Try
    End Sub
    Private Function FindControl(ByVal parents As Control.ControlCollection, ByVal name As String) As Control
        For Each ctr As Control In parents
            If ctr.Name = name Then
                Return ctr
            Else
                Dim ans As Control = FindControl(ctr.Controls, name)
                If ans IsNot Nothing Then
                    Return ans
                End If
            End If
        Next
        Return Nothing
    End Function
    Friend Sub FromDocument()
        If myVisioApp.ActivePage Is Nothing Then Exit Sub
        If myVisioApp.ActivePage.PageSheet Is Nothing Then Exit Sub
        FromDocument(myVisioApp.ActivePage.PageSheet)
    End Sub
    Private Sub FromDocument(ByVal pagesheet As Shape)
        Dim setts As String = CustomProperty(pagesheet, "settings")
        Dim sett As String() = setts.Split(New [Char]() {";"c})
        For Each ss As String In sett
            Dim asgn As String() = ss.Split(New [Char]() {"="c})
            If asgn(0) <> "" Then
                Change(asgn(0), asgn(1))
            End If
        Next
    End Sub
    Friend Sub ToDocument()
        If myVisioApp.ActivePage Is Nothing Then Exit Sub
        If myVisioApp.ActivePage.PageSheet Is Nothing Then Exit Sub
        ToDocument(myVisioApp.ActivePage.PageSheet)
    End Sub
    Private Sub ToDocument(ByVal docsheet As Shape)
        Dim ans As String = ""
        For Each kvp As KeyValuePair(Of String, String) In Values
            ans = ans & kvp.Key & "=" & kvp.Value & ";"
        Next kvp
        AddCustomProperty(docsheet, "settings", ans)
    End Sub
    Friend Sub FromGraph(ByVal graph As Graph)
        Change("digraph", graph.digraph)
        Change("strict", graph.Strict)
        For Each kvp As KeyValuePair(Of String, String) In graph.GraphAttrs
            Change(kvp.Key, kvp.Value)
        Next
    End Sub
    Friend Sub ToGraph(ByVal graph As Graph)
        Dim temp As String = Value("digraph")
        If temp = "" Then
            temp = "False"
        End If
        graph.digraph = temp
        temp = Value("strict")
        If temp = "" Then
            temp = "false"
        End If
        graph.Strict = temp
        graph.GraphAttrs = New Dictionary(Of String, String)
        For Each kvp As KeyValuePair(Of String, String) In Values
            graph.SetAttribute(kvp.Key, kvp.Value)
        Next kvp
    End Sub
    Private Sub LoadControl(ByVal ctl As Control)
        If ctl.Controls.Count > 0 Then
            For Each sctl As Control In ctl.Controls
                LoadControl(sctl)
            Next
        Else
            If TypeOf ctl Is RadioButton Then
                Dim rb As RadioButton = CType(ctl, RadioButton)
                Dim parent As Control = rb.Parent
                If rb.Checked Then
                    CurrentSetting(parent.Name) = rb.Name
                End If
            ElseIf TypeOf ctl Is CheckBox Then
                Dim chkbx As CheckBox = CType(ctl, CheckBox)
                If chkbx.Checked Then
                    CurrentSetting(ctl.Name) = "true"
                Else
                    CurrentSetting(ctl.Name) = "false"
                End If
            ElseIf TypeOf ctl Is TextBox Then
                CurrentSetting(ctl.Name) = ctl.Text
            ElseIf TypeOf ctl Is ListBox Then
                Dim lstbx As ListBox = CType(ctl, ListBox)
                CurrentSetting(ctl.Name) = CStr(lstbx.SelectedItem)
            ElseIf TypeOf ctl Is ComboBox Then
                Dim cmbbx As ComboBox = CType(ctl, ComboBox)
                CurrentSetting(ctl.Name) = CStr(cmbbx.SelectedItem)
            End If
        End If
    End Sub
#If DEBUG Then
    Sub Dump()
        debug.writeline("Settings " & CurrentSetting.ID)
        For Each kvp As KeyValuePair(Of String, String) In Values
            debug.writeline(kvp.Key & "=" & kvp.Value)
        Next
    End Sub
#End If
End Class
