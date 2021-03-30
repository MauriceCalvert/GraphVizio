Imports Visio
Module _Action
    Sub Action(ByVal action As String)
        Dim preset As String = ""
        Try
            Warnings.Clear()
            If action = "show_messages" Then
                If MsgWin IsNot Nothing Then
                    MsgWin.Display("", Drawing.Color.Black)
                End If
                Exit Sub
            End If

            MsgWin = Nothing

            ' Either
            '   preset_save
            ' or
            '   preset name-of-preset
            If action.StartsWith("preset") Then
                If action.Substring(6, 1) = "_" Then
                    preset = " save"
                Else
                    preset = action.Substring(7)
                End If
                action = "preset"
            End If
            SwitchCurrentSettings()
            Select Case action
                Case "connectors_fix_unglued"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ConnectorsFixUnglued), False)
                Case "connectors_select_connected"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ConnectorSelector2), False)
                Case "connectors_select_partlyconnected"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ConnectorSelector1), False)
                Case "connectors_select_unconnected"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ConnectorSelector0), False)
                Case "diagram_exportgraphviz"
                    ExportDOT()
                Case "diagram_importcsv"
                    ImportCSV()
                Case "diagram_importgraphviz"
                    ImportDOT()
                Case "GraphMenu"
                    AddGraphVizioMenu()
                Case "help_about"
                    Dim About As New About
                    About.ShowDialog()
                Case "help_how_do_i"
                    Dim howdoi As New HowDoI
                    howdoi.ShowDialog()
                Case "help_online"
                    System.Diagnostics.Process.Start("http://www.calvert.ch/graphvizio/")
                Case "layout_current"
                    Dim executable As String = GetGraphvizDir() ' Who will prompt for path if necessary
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf Layout), True)
                Case "options"
                    Dim Options As New Options
                    Options.ShowDialog()
                Case "preset"
                    If preset = " save" Then
                        SavePreset()
                    Else
                        CurrentSetting.Load(preset)
                        CurrentSetting.ToDocument()
                    End If
                Case "settings"
                    CurrentSetting.ShowForm()
                    CurrentSetting.BringToFront()
                    DisplaySettings = True
                Case "shapes_autocluster"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ShapesAutoCluster), False)
                Case "shapes_cluster"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ShapesCluster), False)
                Case "shapes_select_connected"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ShapesSelectConnected), False)
                Case "shapes_select_unconnected"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ShapesSelectUnconnected), False)
                Case "shapes_select_root"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ShapesSelectRoot), False)
                Case "shapes_set_root"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ShapesSetRoot), False)
                Case "shapes_size_narrower"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ShapesSizeNarrower), False)
                Case "shapes_size_shorter"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ShapesSizeShorter), False)
                Case "shapes_size_taller"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ShapesSizeTaller), False)
                Case "shapes_size_to_fit"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ShapesSizeToFit), False)
                Case "shapes_size_to_widest"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ShapesSizeToWidest), False)
                Case "shapes_size_wider"
                    DispatchWithProgress(New ProgressTaskToDo(AddressOf ShapesSizeWider), False)
                Case "stencil"
                    OpenStencil(VisOpenSaveArgs.visOpenRW)
                Case "sortstencil"
                    SortStencil()
#If DEBUG Then
                Case "test"
                    Tester()
#End If
                Case Else
                    Throw New GraphVizioException("Menu action '" & action & "' not implemented")
            End Select
        Catch ex As Exception
            HandleError(ex)
        End Try
    End Sub
End Module
