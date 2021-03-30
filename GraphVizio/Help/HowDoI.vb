Imports System.Windows.Forms
Imports System.Drawing
Imports System.Collections.Generic
Imports Visio.VisWindowStates
Friend Class HowDoI
    Private Sub HowDoI_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadHowDoIs()
    End Sub
    Private Sub LoadHowDoIs()
        Dim ll As LinkLabel
        Dim top As Integer = 5
        Dim left As Integer = 5
        Dim graphics As System.Drawing.Graphics = Me.CreateGraphics
        Dim textwidth As Single
        Dim widestlabel As Single

        For Each ht As String In HowTos.Keys
            ll = New LinkLabel
            ll.Top = top
            ll.Left = left
            textwidth = graphics.MeasureString(ht & "MMM", ll.Font).Width
            ll.Width = CInt(textwidth)
            ll.Text = ht
            If textwidth > widestlabel Then
                widestlabel = textwidth
            End If
            top = top + ll.Height
            Me.Controls.Add(ll)
            AddHandler ll.Click, AddressOf HowDoI_Click
        Next
        If Me.Width < widestlabel Then
            Me.Width = CInt(widestlabel) + 10
        End If
        If Me.Height < top Then
            Me.Height = top + 32
        End If
        Dim screen As System.Windows.Forms.Screen = System.Windows.Forms.Screen.PrimaryScreen
        Dim bounds As Rectangle = screen.Bounds
        Me.Top = (bounds.Height - Me.Height) \ 2
        Me.Left = (bounds.Width - Me.Width) \ 2
    End Sub
    Private Sub HowDoI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim topic As String = CStr(sender.text)

        Me.Close()
        DispatchSilent(AddressOf ShowHowTo, topic)
        'DispatchWithProgress(New ProgressTaskToDo(AddressOf ShowHowTo), True, topic)
    End Sub
End Class

