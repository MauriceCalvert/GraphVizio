Imports System.Collections.Generic
Imports Office
Imports Office.MsoControlType

Module _AddGraphVizioMenu
    Friend Sub AddGraphVizioMenu()
        Try
            Dim VisioCommandBars As CommandBars = CType(myVisioApp.CommandBars, CommandBars)
            Dim VisioMenuBar As CommandBar = VisioCommandBars("Menu bar")
            If VisioMenuBar Is Nothing Then
                VisioMenuBar = VisioCommandBars(1)
                Warning("GraphVizio cannot initialise correctly, the standard Visio menu 'Menu bar' is not present." & vbCrLf &
                    "'Graph' option added to menu '" & VisioMenuBar.Name & "'. Good luck finding it >;-)")
            End If
            If (VisioMenuBar.Protection And MsoBarProtection.msoBarNoCustomize) <> 0 Then
                ' Ha-ha, trying to stop us customising eh? Well screw that !
                VisioMenuBar.Protection = MsoBarProtection.msoBarNoProtection
                If (VisioMenuBar.Protection And MsoBarProtection.msoBarNoCustomize) <> 0 Then
                    Warning("Can't add GraphVizio menu, 'Menu Bar' customisation is locked and refuses to unlock. " & vbCrLf &
                        "Looks like it's time to re-install Visio?")
                    Exit Sub
                Else
                    Warning("'Menu Bar' customisation was locked. A useless ploy. " & vbCrLf &
                        "Customisation unlocked to allow GraphVizio to install 'Graph' menu")
                End If
            End If
            Dim myCommandBar As CommandBarControl = VisioMenuBar.FindControl(, , TITLE)
            If myCommandBar IsNot Nothing Then
                myCommandBar.Delete()
            End If

            Dim myMenu As CommandBarPopup = Nothing
            Dim subMenu As CommandBarPopup
            Dim shapemenupos As Integer = 7 ' position before "Shape"
            Try
                myMenu = CType(VisioMenuBar.Controls.Add(Type:=msoControlPopup, Before:=shapemenupos, Temporary:=True), CommandBarPopup)
            Catch ex As Exception
                Warning("Unable to install 'Graph' menu in standard 'Menu Bar' : " & ex.Message & vbCrLf &
                    "Looks like it's time to re-install Visio?")
                Exit Sub
            End Try
            myMenu.Caption = "&Graph"
            myMenu.Tag = TITLE

            '-------------------------------------
            Dim cbrCmdBar As CommandBar = Nothing
            Dim cbbutton As CommandBarButton = Nothing
            Dim mybarname As String = ""

            For Each cb As CommandBar In myVisioApp.CommandBars
                If cb.Name = "GraphVizio" Then
                    cbrCmdBar = cb
                    Exit For
                End If
            Next

            If cbrCmdBar Is Nothing Then
                cbrCmdBar = CType(myVisioApp.CommandBars.Add(Name:=TITLE, Temporary:=False), CommandBar)
                cbrCmdBar.Context = "2*"
                'Add a button to MyDrawingCommandBar
                cbbutton = CType(cbrCmdBar.Controls.Add(Type:=msoControlButton), CommandBarButton)
                With cbbutton
                    .Caption = "GraphVizio Menu"
                    .TooltipText = "Display the Graph Menu in the main menu bar"
                    .FaceId = 7075
                    .Tag = "GraphMenu"
                    AddHandler cbbutton.Click, AddressOf menuItem_Click
                End With
            End If

            AddMenuItem(myMenu, "S&ettings", "settings", False)
            AddMenuItem(myMenu, "&Layout!", "layout_current", False)

            subMenu = AddSubMenu(myMenu, "&Preset")
            AddMenuItem(subMenu, "&Save...", "preset_save", False)

            If Not My.Computer.FileSystem.DirectoryExists(LocalDataDirectory() & SETTINGSDIRECTORY) Then
                Try
                    My.Computer.FileSystem.CreateDirectory(LocalDataDirectory() & SETTINGSDIRECTORY)
                Catch ex As Exception
                    Warning("Unable to create settings in " & LocalDataDirectory() & SETTINGSDIRECTORY & " : " & ex.Message & vbCrLf &
                        "Presets will not work")
                End Try
            End If

            Try ' Ignore errors due to silly filenames etc, uninteresting
                For Each settingFile As String In
                    My.Computer.FileSystem.GetFiles(
                        LocalDataDirectory() & SETTINGSDIRECTORY,
                        FileIO.SearchOption.SearchTopLevelOnly,
                        "*.txt")
                    settingFile = Mid(settingFile, settingFile.LastIndexOf("\") + 2) ' Remove directories
                    settingFile = Left(settingFile, InStr(settingFile, ".") - 1) ' remove ".txt"
                    If settingFile <> "" Then
                        AddMenuItem(subMenu, settingFile, "preset " & settingFile, False)
                    End If
                Next
            Catch ex As Exception
                Warning("Error setting up presets : " & ex.Message)
            End Try

            subMenu = AddSubMenu(myMenu, "&Diagram")
            AddMenuItem(subMenu, "&Import Graphviz", "diagram_importgraphviz", False)
            AddMenuItem(subMenu, "Import &CSV", "diagram_importcsv", False)
            AddMenuItem(subMenu, "&Export Graphviz", "diagram_exportgraphviz", False)

            subMenu = AddSubMenu(myMenu, "&Shapes")
            AddMenuItem(subMenu, "&Auto-cluster", "shapes_autocluster", False)
            AddMenuItem(subMenu, "&Cluster", "shapes_cluster", False)
            AddMenuItem(subMenu, "Make &Narrower", "shapes_size_narrower", True)
            AddMenuItem(subMenu, "Make &Wider", "shapes_size_wider", False)
            AddMenuItem(subMenu, "Make &Taller", "shapes_size_taller", False)
            AddMenuItem(subMenu, "Make &Shorter", "shapes_size_shorter", False)
            AddMenuItem(subMenu, "Size to &Fit", "shapes_size_to_fit", True)
            AddMenuItem(subMenu, "Size to W&idest", "shapes_size_to_widest", False)
            AddMenuItem(subMenu, "Select &Connected", "shapes_select_connected", True)
            AddMenuItem(subMenu, "Select &UnConnected", "shapes_select_unconnected", False)
            AddMenuItem(subMenu, "Set &Root", "shapes_set_root", True)
            AddMenuItem(subMenu, "Select R&oot", "shapes_select_root", False)

            subMenu = AddSubMenu(myMenu, "&Connectors")
            AddMenuItem(subMenu, "Select &Connected", "connectors_select_connected", False)
            AddMenuItem(subMenu, "Select &UnConnected", "connectors_select_unconnected", False)
            AddMenuItem(subMenu, "Select &PartlyConnected", "connectors_select_partlyconnected", False)
            AddMenuItem(subMenu, "&Fix unglued", "connectors_fix_unglued", True)

            subMenu = AddSubMenu(myMenu, "&Tools")
            AddMenuItem(subMenu, "&Options...", "options", False)
            AddMenuItem(subMenu, "&Stencil", "stencil", False)
            AddMenuItem(subMenu, "So&rt stencil", "sortstencil", False)

            subMenu = AddSubMenu(myMenu, "&Help")
            AddMenuItem(subMenu, "&How do I...", "help_how_do_i", False)
            AddMenuItem(subMenu, "&Online", "help_online", False)
            AddMenuItem(subMenu, "&About", "help_about", False)
            ' AddMenuItem(myMenu, "test", "test", False)
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
    Private Sub AddMenuItem(ByRef myMenu As CommandBarPopup, ByVal caption As String, ByVal tag As String, ByVal newgroup As Boolean)
        Dim newmenu As New menuHandler
        ' Keep menu handlers in a collection to stop them from being garbage-collected (see MenuHandler swindle below)
        menuHandlers.Add(newmenu)
        newmenu.menuCommand = CType(myMenu.Controls.Add(Type:=msoControlButton, Temporary:=True), CommandBarButton)
        newmenu.menuCommand.Caption = caption
        newmenu.menuCommand.Tag = tag
        newmenu.menuCommand.BeginGroup = newgroup
        AddHandler newmenu.menuCommand.Click, AddressOf menuItem_Click
    End Sub
    Private Function AddSubMenu(ByRef myMenu As CommandBarPopup, ByVal caption As String) As CommandBarPopup
        Dim newButton As CommandBarPopup
        newButton = CType(myMenu.Controls.Add(Type:=msoControlPopup, Temporary:=True), CommandBarPopup)
        newButton.Caption = caption
        Return newButton
    End Function
    Friend Sub menuItem_Click(
                ByVal ctrl As CommandBarButton,
                ByRef cancelDefault As Boolean)
        Action(ctrl.Tag)
    End Sub
    Private menuHandlers As New List(Of menuHandler)
    Private Class menuHandler
        Friend WithEvents menuCommand As CommandBarButton
    End Class
End Module
