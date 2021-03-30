Imports Visio
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Windows.Forms.Application
Imports System.IO
Imports System.Drawing
Friend Class Settings
    Friend ID As Integer
    Private Showing As String
    Private LockInterface As Boolean = True
    Private ToolTipsSet As Boolean = False
    Private Docking As Boolean = False
    Private WithEvents Fader As New Timer
    Sub New(ByVal newID As Integer)
        InitializeComponent()
        ID = newID
        If ToolTips Is Nothing Then LoadToolTips()
        SetToolTips(CType(Me.Controls, ControlCollection))
    End Sub
    Private Sub SetupControls()
        LockInterface = True
        Dim circular As Boolean = circo.Checked Or twopi.Checked
        setroot.Enabled = circular
        showroot.Enabled = circular
        tb.Enabled = Not circular
        lr.Enabled = Not circular
        If splines.Checked Then
            ideal.Checked = True
            CurrentSetting("connectto") = "ideal"
            CurrentSetting("splines") = "true"
            connectto.Enabled = False
        Else
            connectto.Enabled = True
        End If
        rankdir.Enabled = dot.Checked
        connectorname.Enabled = connectoris.Checked
        shapename.Enabled = shapeis.Checked
        leafstack.Enabled = dot.Checked
        If myVisioApp.ActivePage Is Nothing Then
            setroot.Enabled = False
            showroot.Enabled = False
            btnLayout.Enabled = False
        End If
        LockInterface = False
    End Sub
    Private Sub Main_Activated(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles Me.Activated
        Try
            LockInterface = True
            If StencilConnectors Is Nothing Then GetStencilShapes()
            If connectorname.Items.Count = 0 Then
                connectorname.Items.Add(SELECTPROMPT)
                connectorname.SelectedItem = SELECTPROMPT
                For Each conn As String In StencilConnectors
                    connectorname.Items.Add(conn)
                Next
            End If
            If shapename.Items.Count = 0 Then
                shapename.Items.Add(SELECTPROMPT)
                shapename.SelectedItem = SELECTPROMPT
                For Each conn As String In StencilShapes
                    shapename.Items.Add(conn)
                Next
            End If
            SetupControls()
            ToolTipWidget.AutomaticDelay = My.Settings.ToolTipDelay
            DockMyself()
            LockInterface = False
            ShowCorrectPicture()
            Me.Opacity = 1
            Fader.Start()
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub Main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) _
        Handles Me.FormClosing
        Try
            If e.CloseReason = CloseReason.UserClosing Then
                DisplaySettings = False
            End If
            PageTable.Remove(ID)
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub Main_Load(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles Me.Load
        Try
            CurrentSetting.FromDocument()
            Fader.Interval = My.Settings.FadeDelay * 1000
            Me.Opacity = 1
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub btnLayout_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles btnLayout.Click
        Action("layout_current")
    End Sub
    Private Sub btnImportDOT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btnImportDOT.Click
        Action("diagram_importgraphviz")
    End Sub
    Private Sub RadioChanged(ByVal sender As RadioButton, ByVal e As System.EventArgs) _
        Handles circo.CheckedChanged, dot.CheckedChanged, fdp.CheckedChanged, neato.CheckedChanged, twopi.CheckedChanged, _
            tb.CheckedChanged, lr.CheckedChanged, connectoris.CheckedChanged, _
            straight.CheckedChanged, splines.CheckedChanged, _
            ideal.CheckedChanged, topbottom.CheckedChanged, quadrant.CheckedChanged, _
            existing.CheckedChanged, glue.CheckedChanged, centre.CheckedChanged
        If LockInterface Then Exit Sub
        Try
            If sender.Parent Is Nothing Then Exit Sub ' Happens when we check a radio ourselves (in SetupControls)
            If CBool(sender.Checked) Then
                CurrentSetting(CStr(sender.Parent.Name)) = CStr(sender.Name)
                SetupControls()
                ShowPicture(CStr(sender.Name))
            End If
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub CheckedChanged(ByVal sender As CheckBox, ByVal e As System.EventArgs) _
        Handles shapeis.CheckedChanged, shapefillcolours.CheckedChanged, shapelinecolours.CheckedChanged, _
            shapetextcolours.CheckedChanged, drawboundingboxes.CheckedChanged, _
            rankbyposition.CheckedChanged, digraph.CheckedChanged, lockseed.CheckedChanged
        If LockInterface Then Exit Sub
        Try
            If CBool(sender.Checked) Then
                CurrentSetting(CStr(sender.Name)) = "true"
                ShowPicture(CStr(sender.Name))
            Else
                CurrentSetting(CStr(sender.Name)) = "false"
            End If
            SetupControls()
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub DropDownClosed(ByVal sender As ComboBox, ByVal e As System.EventArgs) _
        Handles connectorname.DropDownClosed, shapename.DropDownClosed, leafstack.DropDownClosed
        If LockInterface Then Exit Sub
        ShowPicture(CStr(sender.Name) & CStr(sender.SelectedItem))
    End Sub
    Private Sub SelectedIndexChanged(ByVal sender As ComboBox, ByVal e As System.EventArgs) _
        Handles _
            shapename.SelectedIndexChanged, connectorname.SelectedIndexChanged, _
            leafstack.SelectedIndexChanged, strict.SelectedIndexChanged, overlap.SelectedIndexChanged
        If LockInterface Then Exit Sub
        Try
            CurrentSetting(CStr(sender.Name)) = CStr(sender.SelectedItem)
            ShowPicture(CStr(sender.SelectedItem))
            SetupControls()
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub colour_changed(ByVal sender As PictureBox, ByVal e As System.EventArgs) _
        Handles shapefillcolour.BackColorChanged, shapelinecolour.BackColorChanged, shapetextcolour.BackColorChanged
        If LockInterface Then Exit Sub
        CurrentSetting(CStr(sender.Name)) = ColorToRGB(sender.BackColor)
    End Sub
    Private Sub colour_Click(ByVal sender As PictureBox, ByVal e As System.EventArgs) _
        Handles shapefillcolour.Click, shapelinecolour.Click, shapetextcolour.Click
        If LockInterface Then Exit Sub
        Try
            ColorDialog.ShowHelp = True
            ColorDialog.Color = CType(sender.BackColor, Drawing.Color)
            If (ColorDialog.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
                sender.BackColor = ColorDialog.Color
            End If
            SetupControls()
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub setroot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles setroot.Click
        Try
            Action("shapes_set_root")
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub showroot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles showroot.Click
        Try
            Action("shapes_select_root")
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub Main_Move(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles Me.Move
        If Docking OrElse LockInterface Then Exit Sub
        Try
            Dim myLeft As Integer = Me.Left
            Dim myTop As Integer = Me.Top
            Dim myWidth As Integer = Me.Width
            Dim myHeight As Integer = Me.Height
            Dim viswin As Visio.Window = DirectCast(myVisioApp.Window, Visio.Window)
            Dim visleft As Integer, vistop As Integer, viswidth As Integer, visheight As Integer
            viswin.GetWindowRect(visleft, vistop, viswidth, visheight)
            Dim xOffset As Integer, yOffset As Integer
            If myLeft + myWidth / 2 > visleft + viswidth / 2 Then ' attach right
                xOffset = myLeft - (visleft + viswidth)
            Else
                xOffset = myLeft - visleft
            End If
            If myTop + myHeight / 2 > vistop + visheight / 2 Then ' attach bottom
                yOffset = myTop - (vistop + visheight)
            Else
                yOffset = myTop - vistop
            End If
            My.Settings.XOffset = xOffset
            My.Settings.YOffset = yOffset
        Catch
        End Try
    End Sub
    Friend Sub DockMyself()
        Docking = True
        Dim xOffset As Integer = My.Settings.XOffset
        Dim yOffset As Integer = My.Settings.YOffset
        If xOffset < 0 Then
            xOffset = -Me.Width - 24
        End If
        If yOffset < 0 Then
            yOffset = -Me.Height - 48
        End If
        Dim viswin As Visio.Window = DirectCast(myVisioApp.Window, Visio.Window)
        Dim visleft As Integer, vistop As Integer, viswidth As Integer, visheight As Integer
        viswin.GetWindowRect(visleft, vistop, viswidth, visheight)
        If xOffset < 0 Then
            Me.Left = visleft + viswidth + xOffset
        Else
            Me.Left = visleft + xOffset
        End If
        If yOffset < 0 Then
            Me.Top = vistop + visheight + yOffset
        Else
            Me.Top = vistop + yOffset
        End If
        Docking = False
    End Sub
    Private Sub SetToolTips(ByVal parents As Object)
        For Each ctl As Control In CType(parents, System.Collections.IEnumerable)
            SetToolTips(ctl.Controls)
            Dim tooltip As String = ""
            ToolTips.TryGetValue(ctl.Name, tooltip)
            If tooltip <> "" Then
                ToolTipWidget.SetToolTip(ctl, tooltip)
            End If
        Next
    End Sub
    Private Sub ShowPicture(ByVal chosen As String)
        Try
            Dim picpath As String = ""
            If chosen.EndsWith("+") Then
                chosen = chosen.Substring(chosen.Length - 1) & "0"
            End If
            Dim stream As System.IO.Stream = GetStreamResource(chosen & ".jpg")
            If stream IsNot Nothing Then
                Dim jpg As New Bitmap(stream)
                Showing = chosen
                PictureBox.Image = jpg
            End If
        Catch
        End Try
    End Sub
    Private Sub commandoptions_TextChanged(ByVal sender As RichTextBox, ByVal e As System.EventArgs) _
        Handles commandoptions.TextChanged
        CurrentSetting(CStr(sender.Name)) = CStr(sender.Text)
    End Sub
    Private Sub ShowCorrectPicture()
        Select Case TabControl.SelectedTab.Name
            Case "tpgraph"
                For Each rb As RadioButton In algorithm.Controls
                    If rb.Checked Then
                        ShowPicture(rb.Name)
                        Exit Select
                    End If
                Next
            Case "tpshapes"
                If shapeis.Checked Then
                    ShowPicture(shapename.Text)
                Else
                    ShowPicture("blank")
                End If
            Case "tpconnectors"
                For Each rb As RadioButton In connectto.Controls
                    If rb.Checked Then
                        ShowPicture(rb.Name)
                        Exit Select
                    End If
                Next
            Case "tpadvanced"
                If digraph.Checked Then
                    ShowPicture("digraph")
                Else
                    ShowPicture("graph")
                End If
        End Select
    End Sub
    Private Sub TabControl_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles TabControl.SelectedIndexChanged
        ShowCorrectPicture()
    End Sub
    Private Sub seed_Validated(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles seed.Validated
        CurrentSetting("seed") = seed.Text.Trim
        If CurrentSetting("seed") = "0" Then
            lockseed.Checked = True
        End If
    End Sub
    Private Sub seed_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) _
        Handles seed.Validating
        Dim value As Integer
        If Integer.TryParse(seed.Text, value) Then
            If value < 0 Then
                MsgBox("Negative seeds no allowed. Enter a non-negative integer")
                e.Cancel = True
            End If
        Else
            MsgBox("Invalid number " & seed.Text & ", enter a positive integer")
            e.Cancel = True
        End If
    End Sub
    Private Sub btnMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btnMenu.Click
        Action("GraphMenu")
    End Sub
    Private Sub Settings_Resize(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            Me.Opacity = 1
        End If
    End Sub
    Private Sub aspectratio_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles aspectratio.Validated
        CurrentSetting("aspectratio") = aspectratio.Text
    End Sub
    Private Sub aspectratio_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles aspectratio.Validating
        Dim value As Double
        If aspectratio.Text.Trim = "" Then
            aspectratio.Text = "0"
        ElseIf Double.TryParse(aspectratio.Text, value) Then
            If value < 0.0F Then
                MsgBox("Aspect ratio cannot be negative")
                e.Cancel = True
            End If
        Else
            MsgBox("Aspect ratio must be a positive number")
            e.Cancel = True
        End If
    End Sub
    '
    ' Stuff for fading
    '
    Private Sub StartFader()
        If My.Settings.FadeDelay = 0 Or My.Settings.FadePct = 100 Then
            Exit Sub
        End If
        Fader.Interval = My.Settings.FadeDelay * 1000
        Fader.Start()
        Me.Opacity = 1
    End Sub
    Private Sub Settings_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles Me.MouseHover
        StartFader()
    End Sub
    Private Sub Settings_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) _
        Handles Me.MouseMove
        StartFader()
    End Sub
    Private Sub FaderTick() Handles Fader.Tick
        If Me.IsDisposed Then
            Exit Sub
        End If
        Dim mouseat As System.Drawing.Point = Me.PointToClient(Cursor.Position)
        Dim myrect As Rectangle

        Fader.Stop()
        If Not AllowFading OrElse Me.WindowState = FormWindowState.Minimized Then
            Me.Opacity = 1
            Exit Sub
        End If

        myrect = Me.Bounds
        ' Normalise my rectangle to the origin (otherwise contains doens't work correctly)
        myrect.X = 0
        myrect.Y = 0
        If myrect.Contains(mouseat.X, mouseat.Y) Then
            Exit Sub
        End If

        For i = 100 To My.Settings.FadePct Step -1
            Me.Opacity = Me.Opacity - 0.01
            DoEvents()
        Next
    End Sub
End Class