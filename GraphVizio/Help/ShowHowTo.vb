Imports Visio
Imports Visio.VisSelectArgs
Imports Visio.VisWindowStates
Imports Visio.VisUICmds
Imports System
Imports System.Threading
Imports System.Threading.Thread
Imports System.Drawing
Imports System.Diagnostics
Imports System.Windows.Forms
Imports System.Windows.Forms.Application
Imports System.Collections.Generic
Imports System.Globalization
' Imports System.Windows.Input

Module ShowHowTo__
    Private Declare Ansi Function PostMessage Lib "user32.dll" Alias "PostMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Private Const WM_KEYDOWN As Integer = &H100
    Private Const WM_KEYFIRST As Integer = &H100
    Private Const WM_KEYUP As Integer = &H101
    Private Const WM_CHAR As Integer = &H102
    Private Const WM_DEADCHAR As Integer = &H103
    Private Const WM_SYSKEYDOWN As Integer = &H104
    Private Const WM_SYSKEYUP As Integer = &H105
    Private Const WM_SYSCHAR As Integer = &H106
    Private Const WM_SYSDEADCHAR As Integer = &H107
    Private Const WM_KEYLAST As Integer = &H108
    Private Const AltDown As Integer = &H20000001 ' the alt key is being pressed at the same time
    Private Const IDDEFAULT As Integer = 0
    Private Const IDOK As Integer = 1
    Private Const IDCANCEL As Integer = 2
    Private Const IDABORT As Integer = 3
    Private Const IDRETRY As Integer = 4
    Private Const IDIGNORE As Integer = 5
    Private Const IDYES As Integer = 6
    Private Const IDNO As Integer = 7
    Private slidestep As Integer = My.Settings.SlideStep ' pixels per step when sliding
    Private slidepause As Integer = My.Settings.SlidePause ' mS to pause at each step
    Private aborted As Boolean = False
    Private crashed As Boolean = False
    Private pause As Integer = 1000 ' mS
    Private explain As Explain
    Private bounds As Rectangle
    Private showedsettings As Boolean

    Function ShowHowTo(ByRef topic As Object) As String
        Dim howto As HowTo = HowTos(CStr(topic))
        Dim arg() As String
        Dim visiohwnd As Integer = 0
        Dim screen As System.Windows.Forms.Screen = System.Windows.Forms.Screen.PrimaryScreen
        Dim visiowin As Visio.Window = DirectCast(myVisioApp.Window, Visio.Window)
        Dim blankdoc As Visio.Document = Nothing
        Dim currentdoc As Visio.Document = Nothing
        Dim thingtodo As HowToAction = Nothing
        Dim drawingupdates As Boolean = My.Settings.DrawingUpdates
        My.Settings.DrawingUpdates = True
        myVisioApp.ScreenUpdating = True
        myVisioApp.ShowChanges = True

        bounds = screen.Bounds
        slidestep = My.Settings.SlideStep
        slidepause = My.Settings.SlidePause
        showedsettings = False

        Try ' Find the window handle of the active document. If there isn't one, create a blank document
            visiohwnd = myVisioApp.ActiveWindow.WindowHandle32

        Catch ex As Exception
            Try
                blankdoc = myVisioApp.Documents.Add("")
                visiohwnd = myVisioApp.ActiveWindow.WindowHandle32
            Catch ex2 As Exception
                Warning("An open document is required for the help to function." & vbCrLf &
                    "Attempt to create a blank document failed: " & ex.Message)
                Return ""
            End Try
        End Try

        AllowFading = False
        Select Case My.Settings.HelpSpeed
            Case "Slow"
                pause = 1500
            Case "Fast"
                pause = 500
            Case Else
                pause = 1000
        End Select

        explain = New Explain()
        aborted = False
        AddHandler explain.Abort, AddressOf Abort

        Try
            HelpInProgress = True
            For Each thingtodo In howto.actions

                arg = thingtodo.args.Split(New Char() {" "c})

                Select Case thingtodo.verb

                    Case "alt"
                        SendKey(visiohwnd, WM_SYSCHAR, Asc(arg(0)), AltDown)
                        For ai As Integer = 1 To arg.GetUpperBound(0)
                            Select Case arg(ai)
                                Case "down"
                                    SendKey(visiohwnd, WM_SYSCHAR, Keys.Down, 1)
                                Case Else
                                    SendKey(visiohwnd, WM_SYSCHAR, Asc(arg(ai)), AltDown)
                            End Select
                        Next
                        Idle(pause)

                    Case "check"
                        GotoSettings()
                        Dim ctl As Control = FindControl(CurrentSetting.Form.Controls, arg(0))
                        If ctl Is Nothing Then
                            Throw New ArgumentException("Can't find control " & arg(0))
                        Else
                            PointOut(ctl)
                            If TypeOf (ctl) Is CheckBox Then
                                Dim cb As CheckBox = CType(ctl, CheckBox)
                                cb.Checked = True
                            Else
                                Dim rb As RadioButton = CType(ctl, RadioButton)
                                rb.Checked = True
                            End If
                        End If
                        Idle(pause)

                    Case "close"
                        Idle(pause * 5)
                        If currentdoc IsNot Nothing Then
                            myVisioApp.AlertResponse = IDNO
                            currentdoc.Close()
                            currentdoc = Nothing
                            myVisioApp.AlertResponse = IDDEFAULT
                        End If

                    Case "connectorsselectconnected"
                        Action("connectors_select_connected")
                        Idle(pause * 2)

                    Case "connectorsselectunconnected"
                        Action("connectors_select_unconnected")
                        Idle(pause * 2)

                    Case "connectorsselectpartlyconnected"
                        Action("connectors_select_partlyconnected")
                        Idle(pause * 2)

                    Case "connectorsfixunglued"
                        Action("connectors_fix_unglued")
                        Idle(pause * 2)

                    Case "deselect"
                        visiowin.DeselectAll()

                    Case "down"
                        SendKey(visiohwnd, WM_KEYDOWN, Keys.Down, 1)
                        If IsNumeric(arg(0)) Then
                            For i As Integer = 2 To CInt(arg(0))
                                SendKey(visiohwnd, WM_KEYDOWN, Keys.Down, 1)
                                Idle(pause \ 4)
                            Next
                        End If
                        Idle(pause)

                    Case "enter"
                        SendKey(visiohwnd, WM_CHAR, Keys.Enter, 1)
                        Idle(pause)

                    Case "escape"
                        SendKey(visiohwnd, WM_CHAR, Keys.Escape, 1)
                        Idle(pause)

                    Case "fixunglued"
                        Action("connectors_fix_unglued")
                        Idle(pause)

                    Case "goto"
                        If arg(0) <> "settings" Then
                            If CurrentSetting IsNot Nothing Then
                                CurrentSetting.HideForm()
                            End If
                        End If
                        If Not explain.Visible Then
                            explain.Display("")
                        End If
                        Select Case arg(0)
                            Case "bottom"
                                Slide(explain, "centre", bounds.Width >> 1, bounds.Height - explain.Height - 32)
                            Case "bottomleft"
                                Slide(explain, "right", 5, bounds.Height - explain.Height - 32)
                            Case "bottomright"
                                Slide(explain, "left", bounds.Width - 16, bounds.Height - explain.Height - 32)
                            Case "centre"
                                Slide(explain, "centre", bounds.Width \ 4, (bounds.Height >> 1) - (explain.Height >> 1))
                            Case "left"
                                Slide(explain, "right", 5, (bounds.Height >> 1) - (explain.Height >> 1))
                            Case "menu"
                                Slide(explain, "left", 265, 50)
                            Case "right"
                                Slide(explain, "left", bounds.Width - 16, (bounds.Height >> 1) - (explain.Height >> 1))
                            Case "settings"
                                GotoSettings()
                            Case "top"
                                Slide(explain, "centre", bounds.Width >> 1, 100 + explain.Height >> 1)
                            Case "topleft"
                                Slide(explain, "right", 5, 100 + explain.Height >> 1)
                            Case "topright"
                                Slide(explain, "left", bounds.Width - 16, 100 + explain.Height >> 1)
                            Case Else
                                Warning("Don't know how to goto '" & arg(0) & "'")
                        End Select

                    Case "importcsv"
                        ImportCSV(LocalDataDirectory() & arg(0), False)
                        currentdoc = myVisioApp.ActiveDocument

                    Case "layout"
                        DispatchWithProgress(New ProgressTaskToDo(AddressOf Layout), False)

                    Case "left"
                        SendKey(visiohwnd, WM_KEYDOWN, Keys.Left, 1)
                        Idle(pause)
                        If IsNumeric(arg(0)) Then
                            For i As Integer = 2 To CInt(arg(0))
                                SendKey(visiohwnd, WM_KEYDOWN, Keys.Left, 1)
                                Idle(pause \ 4)
                            Next
                        End If
                        Idle(pause)

                    Case "maketaller"
                        Action("shapes_size_taller")
                        Idle(pause)

                    Case "opensample"
                        currentdoc = OpenSample(arg(0))
                        For Each win As Window In myVisioApp.Windows
                            If win.Document Is currentdoc Then
                                win.Activate()
                                Exit For
                            End If
                        Next

                    Case "pause"
                        Idle(pause * Convert.ToInt32(arg(0), CultureInfo.InvariantCulture))

                    Case "pointout"
                        Dim ctl As Control = FindControl(CurrentSetting.Form.Controls, arg(0))
                        If ctl Is Nothing Then
                            Throw New ArgumentException("Can't find control " & arg(0))
                        Else
                            PointOut(ctl)
                        End If

                    Case "openstencil"
                        Dim stencil As Document
                        stencil = OpenStencil(VisOpenSaveArgs.visOpenRO Or VisOpenSaveArgs.visOpenDocked Or VisOpenSaveArgs.visOpenCopy)

                    Case "right"
                        SendKey(visiohwnd, WM_KEYDOWN, Keys.Right, 1)
                        Idle(pause)
                        If IsNumeric(arg(0)) Then
                            For i As Integer = 2 To CInt(arg(0))
                                SendKey(visiohwnd, WM_KEYDOWN, Keys.Right, 1)
                                Idle(pause \ 4)
                            Next
                        End If
                        Idle(pause)

                    Case "say"
                        explain.Display(thingtodo.args)
                        arg = thingtodo.args.Split(vbCr.ToCharArray)
                        Idle(pause * (3 + arg.GetUpperBound(0) * 2)) ' Idle a bit more if several lines to read

                    Case "select"
                        Dim win As Visio.Window = myVisioApp.ActiveWindow
                        win.DeselectAll()
                        For i As Integer = 0 To arg.GetUpperBound(0)
                            For j = 1 To myVisioApp.ActivePage.Shapes.Count
                                If myVisioApp.ActivePage.Shapes(j).Text = arg(i) Then
                                    win.Select(myVisioApp.ActivePage.Shapes(j), CShort(visSelect))
                                End If
                            Next
                        Next
                        Idle(pause)

                    Case "set"
                        GotoSettings()
                        Dim ctl As Control = FindControl(CurrentSetting.Form.Controls, arg(0))
                        If ctl IsNot Nothing Then
                            PointOut(ctl)
                            If TypeOf (ctl) Is ComboBox Then
                                Dim cb As ComboBox = CType(ctl, ComboBox)
                                cb.SelectedItem = arg(1)
                            ElseIf TypeOf (ctl) Is TextBox Then
                                Dim tb As TextBox = CType(ctl, TextBox)
                                tb.Text = arg(1)
                            End If
                        Else
                            Warning("Can't set " & arg(0) & " = " & arg(1) & ", control not found")
                        End If
                        Idle(pause)

                    Case "shapefillcolour"
                        GotoSettings()
                        PointOut(CurrentSetting.Form.shapefillcolour)
                        Dim cc As New ColorConverter
                        CurrentSetting.Form.shapefillcolour.BackColor =
                            DirectCast(cc.ConvertFromString(arg(0)), System.Drawing.Color)

                    Case "shapelinecolour"
                        GotoSettings()
                        PointOut(CurrentSetting.Form.shapelinecolour)
                        Dim cc As New ColorConverter
                        CurrentSetting.Form.shapelinecolour.BackColor =
                            DirectCast(cc.ConvertFromString(arg(0)), System.Drawing.Color)

                    Case "shapesautocluster"
                        Action("shapes_autocluster")

                    Case "shapescluster"
                        Action("shapes_cluster")

                    Case "shapesselectconnected"
                        Action("shapes_select_connected")
                        Idle(pause * 2)

                    Case "shapesselectunconnected"
                        Action("shapes_select_unconnected")
                        Idle(pause * 2)

                    Case "shapetextcolour"
                        GotoSettings()
                        PointOut(CurrentSetting.Form.shapetextcolour)
                        Dim cc As New ColorConverter
                        CurrentSetting.Form.shapetextcolour.BackColor =
                            DirectCast(cc.ConvertFromString(arg(0)), System.Drawing.Color)

                    Case "showconnectors"
                        Dim win As Window = myVisioApp.ActiveWindow
                        win.DeselectAll()
                        For Each shape As Shape In currentdoc.Pages(1).Shapes
                            If shape.CellExistsU("BeginX", CShort(True)) <> 0 Then ' a 1D shape, a connector
                                shape.BringToFront()
                                win.DeselectAll()
                                win.Select(shape, CShort(visSelect))
                                Idle(pause * 2)
                                win.DeselectAll()
                            End If
                        Next

                    Case "showoptions"
                        Dim opts As New Options
                        opts.Show()
                        Idle(pause * 3)
                        opts.Close()

                    Case "showmenu"
                        SendKey(visiohwnd, WM_KEYDOWN, Keys.Escape, 1)
                        Idle(pause \ 4)
                        SendKey(visiohwnd, WM_SYSCHAR, Keys.G, AltDown)
                        Idle(pause \ 4)
                        Dim i As Integer = 0
                        Do While i <= arg.GetUpperBound(0) AndAlso IsNumeric(arg(i))
                            For j As Integer = 2 To CInt(arg(i))
                                SendKey(visiohwnd, WM_KEYDOWN, Keys.Down, 1)
                                Idle(pause \ 4)
                            Next
                            If i < arg.GetUpperBound(0) Then
                                SendKey(visiohwnd, WM_KEYDOWN, Keys.Right, 1)
                                Idle(pause \ 4)
                            End If
                            i += 1
                        Loop
                        Idle(pause * 2)
                        For j As Integer = 0 To arg.GetUpperBound(0) + 1
                            SendKey(visiohwnd, WM_KEYDOWN, Keys.Escape, 1)
                        Next

                    Case "sizetofit"
                        Action("shapes_size_to_fit")
                        Idle(pause)

                    Case "sizetowidest"
                        Action("shapes_size_to_widest")
                        Idle(pause)

                    Case "stop"
                        Stop

                    Case "uncheck"
                        GotoSettings()
                        Dim ctl As Control = FindControl(CurrentSetting.Form.Controls, arg(0))
                        If ctl Is Nothing Then
                            Throw New ArgumentException("Can't find control " & arg(0))
                        Else
                            If TypeOf (ctl) Is CheckBox Then
                                Dim cb As CheckBox = CType(ctl, CheckBox)
                                cb.Checked = False
                            Else
                                Dim rb As RadioButton = CType(ctl, RadioButton)
                                rb.Checked = False
                            End If
                        End If
                        Idle(pause)

                    Case "up"
                        SendKey(visiohwnd, WM_KEYDOWN, Keys.Up, 1)
                        Idle(pause)
                        If IsNumeric(arg(0)) Then
                            For i As Integer = 2 To CInt(arg(0))
                                SendKey(visiohwnd, WM_KEYDOWN, Keys.Up, 1)
                                Idle(pause \ 4)
                            Next
                        End If
                        Idle(pause)

                    Case "updating"
                        If arg(0) = "off" Then
                            My.Settings.DrawingUpdates = False
                        Else
                            My.Settings.DrawingUpdates = True
                        End If

                    Case "zoom"
                        Dim win As Window = myVisioApp.ActiveWindow
                        If arg(0) = "page" Then
                            win.Zoom = -1 ' zoom to fit
                        Else
                            win.Zoom = Convert.ToDouble(arg(0), CultureInfo.InvariantCulture) / 100
                        End If
                        Idle(pause)

                    Case ""

                    Case Else
                        Warning("Unknown help verb " & thingtodo.verb & " ignored")

                End Select
                If aborted Then
                    Exit For
                End If
            Next

        Catch ex As Exception
            crashed = True
            MsgBox("Problem displaying help : " & ex.Message)

        Finally
            If explain IsNot Nothing Then
                RemoveHandler explain.Abort, AddressOf Abort
                explain.Close()
                explain = Nothing
            End If

            If Not (aborted Or crashed) Then
                If showedsettings Then
                    CurrentSetting.Close()
                End If
                Idle(pause * 4)
            End If

            If blankdoc IsNot Nothing Then
                myVisioApp.AlertResponse = IDNO ' NO
                blankdoc.Close()
                myVisioApp.AlertResponse = IDDEFAULT ' Back to normal
            End If

            If currentdoc IsNot Nothing Then
                myVisioApp.AlertResponse = IDNO ' NO
                currentdoc.Close()
                myVisioApp.AlertResponse = IDDEFAULT ' Back to normal
            End If
            AllowFading = True

            HelpInProgress = False
            My.Settings.DrawingUpdates = drawingupdates
        End Try
        Return ""
    End Function
    Sub GotoSettings()
        showedsettings = True
        CurrentSetting.ShowForm()
        CurrentSetting.BringToFront()
        DoEvents()
        If CurrentSetting.Form.Left < (bounds.Width >> 1) Then ' move to right of settings
            Slide(explain, "right", CurrentSetting.Form.Left, CurrentSetting.Form.Top)
        Else
            Slide(explain, "left", CurrentSetting.Form.Left, CurrentSetting.Form.Top)
        End If
    End Sub
    Function LocationOf(ByVal ctl As Control) As Point
        Dim result As New Point(0, 0)
        Dim p As Control = ctl
        Do While p IsNot Nothing
            result.Offset(p.Left, p.Top)
            p = p.Parent
        Loop
        Return result
    End Function
    Sub PointOut(ByVal ctl As Control)
        Dim p As Control = ctl.Parent
        Dim tc As TabControl
        ctl.Focus()
        Do While p IsNot Nothing
            If TypeOf (p) Is TabPage Then
                If p.Parent IsNot Nothing Then
                    If TypeOf (p.Parent) Is TabControl Then
                        tc = DirectCast(p.Parent, TabControl)
                        Dim tp As TabPage = DirectCast(p, TabPage)
                        If tc.SelectedTab.Name <> tp.Name Then
                            Dim location As Point = LocationOf(p.Parent)
                            Select Case tp.Name
                                Case "tpGraph"
                                    location.Offset(20, 0)
                                Case "tpShapes"
                                    location.Offset(65, 0)
                                Case "tpConnectors"
                                    location.Offset(125, 0)
                                Case "tpAdvanced"
                                    location.Offset(185, 0)
                            End Select
                            Dim activepointer As Pointer = Nothing
                            activepointer = New Pointer(location)
                            activepointer.Show()
                            Idle(pause * 4)
                            activepointer.Close()
                        End If
                        tc.SelectedTab = DirectCast(p, TabPage)
                        tc.Refresh()
                        Exit Do
                    End If
                End If
            End If
            p = p.Parent
        Loop
        If p IsNot Nothing Then
            Dim activepointer As Pointer = Nothing
            activepointer = New Pointer(LocationOf(ctl))
            activepointer.Show()
            If TypeOf (ctl) Is ComboBox Then
                Dim cb As ComboBox = CType(ctl, ComboBox)
                cb.DroppedDown = True
                Idle(pause)
                cb.DroppedDown = False
            Else
                Idle(pause)
            End If
            activepointer.Close()
        End If
    End Sub
    Sub Slide(ByVal explain As Form, ByVal side As String, ByVal x As Integer, ByVal y As Integer)

        Const twopi As Double = Math.PI * 2
        Const degrees As Double = 20
        Const fudge1 As Double = degrees / 360 * twopi
        Dim fudge2 As Double = Math.Sin(fudge1)
        Dim fudge3 As Double = Math.Sin((degrees + 90) / 360 * twopi)
        Dim fudge4 As Double = 1 / (fudge3 - fudge2)

        Dim left As Double = explain.Left
        Dim top As Double = explain.Top
        Dim l As Double
        Dim t As Double
        Dim xdistance As Double
        Dim ydistance As Double
        Dim steps As Integer
        Dim stepsize As Double = slidestep
        Dim stopwatch As New Stopwatch
        Dim i As Integer
        Dim velocity As Integer
        Dim theta As Double
        Dim srqt24 As Double = CSng(Math.Sqrt(2.0F) / 4.0F)
        Dim mS As Integer

        If side = "left" Then
            x = x - explain.Width
        ElseIf side = "centre" Then
            x = x - (explain.Width >> 1) ' wicked division by 2
        End If

        If x < 0 Then x = 0
        If y < 0 Then y = 0
        xdistance = x - left
        ydistance = y - top
        If Math.Abs(xdistance) > Math.Abs(ydistance) Then
            steps = CInt(Math.Abs(xdistance))
        Else
            steps = CInt(Math.Abs(ydistance))
        End If

        If steps = 0 Then
            Exit Sub
        End If

        ' uncomment to recalculate delay
        'slidestep = 0
        'slidepause = 1

        If slidestep = 0 Then
            stepsize = 3
            stopwatch.Start()
        Else
            steps = CInt(steps / slidestep)
        End If

        Try
            i = 0
            Do While i <= steps
                If i < 3 Then
                    explain.Opacity = explain.Opacity - 0.1
                End If
                l = left + xdistance * i / steps
                theta = i / steps * Math.PI / 2
                t = top + ydistance * (Math.Sin(theta + fudge1) - fudge2) * fudge4
                explain.Left = CInt(l)
                explain.Top = CInt(t)
                Sleep(slidepause)
                velocity = 1 + CInt(3 * Math.Sin(i / steps * Math.PI))
                If i + velocity >= steps Then
                    i = i + 1
                Else
                    i = i + velocity
                End If
            Loop
            explain.Left = x
            explain.Top = y
            explain.Opacity = 1
        Catch ex As Exception

        End Try

        If slidestep = 0 Then
            stopwatch.Stop()
            mS = CInt(stopwatch.ElapsedMilliseconds)
            Dim msperstep As Double = mS / (steps / stepsize)
            slidestep = CInt(3 + Math.Floor(msperstep))
            slidepause = CInt(500 / slidestep)
            slidestep = 3
            slidepause = CInt((500 - mS) / (steps / stepsize))
            My.Settings.SlideStep = slidestep
            My.Settings.SlidePause = slidepause
            My.Settings.Save()
        End If
    End Sub
    Private Sub Abort()
        aborted = True
    End Sub
    Private Sub Idle(ByVal mS As Integer)
        Dim sw As New Stopwatch
        sw.Start()
        Do While sw.ElapsedMilliseconds < mS
            DoEvents()
            If aborted Then
                Exit Sub
            End If
            Sleep(100)
        Loop
        sw.Stop()
    End Sub
    Private Sub SendKey(ByVal hwnd As Integer, ByVal keytype As Integer, ByVal key As Integer, ByVal count As Integer)
        PostMessage(hwnd, keytype, key, count)
        Sleep(25)
        If keytype = WM_KEYDOWN Then
            PostMessage(hwnd, WM_KEYUP, key, count)
            Sleep(25)
        End If
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
End Module
