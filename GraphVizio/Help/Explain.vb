Imports System.Windows.Forms
Imports System.Windows.Forms.Application
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Threading.Thread
Public Class Explain
    Private Const AW_HOR_POSITIVE As Integer = &H1 'Animates the window from left to right. This flag can be used with roll or slide animation.
    Private Const AW_HOR_NEGATIVE As Integer = &H2 'Animates the window from right to left. This flag can be used with roll or slide animation.
    Private Const AW_VER_POSITIVE As Integer = &H4 'Animates the window from top to bottom. This flag can be used with roll or slide animation.
    Private Const AW_VER_NEGATIVE As Integer = &H8 'Animates the window from bottom to top. This flag can be used with roll or slide animation.
    Private Const AW_CENTER As Integer = &H10 'Makes the window appear to collapse inward if AW_HIDE is used or expand outward if the AW_HIDE is not used.
    Private Const AW_HIDE As Integer = &H10000 'Hides the window. By default, the window is shown.
    Private Const AW_ACTIVATE As Integer = &H20000 'Activates the window.
    Private Const AW_SLIDE As Integer = &H40000 'Uses slide animation. By default, roll animation is used.
    Private Const AW_BLEND As Integer = &H80000 'Uses a fade effect. This flag can be used only if hwnd is a top-level window. 

    Private Declare Auto Function AnimateWindow Lib _
                    "user32.dll" (ByVal hWnd As Integer, _
                    ByVal dwTime As Integer, _
                    ByVal dwFlags As Integer _
                    ) As Boolean
    Public Event Abort()
    Private closerect As New Rectangle(245, 5, 10, 10)
    Dim line() As String
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        WriteText()
    End Sub
    Private Sub WriteText()
        If line IsNot Nothing Then
            Const leftmargin As Integer = 10
            Const rightmargin As Integer = 10
            Const tbmargin As Integer = 8
            Dim textsize As SizeF
            Dim textbounds As Rectangle = New Rectangle(leftmargin, tbmargin, Me.Width - leftmargin - rightmargin, Me.Height - 2 * tbmargin)
            Dim bounds As SizeF = New SizeF(Me.Width - leftmargin - rightmargin, Me.Height - 2 * tbmargin)
            Dim textbrush As Brush = Brushes.DarkBlue
            Dim g As Graphics = Me.CreateGraphics
            Dim b As Brush = Brushes.White
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            For l = 0 To line.GetUpperBound(0)
                g.DrawString(line(l), Me.Font, textbrush, textbounds)
                textsize = g.MeasureString(line(l), Me.Font, bounds)
                textbounds.Y = textbounds.Y + CInt(textsize.Height) + 6
            Next
        End If
    End Sub
    Public Sub Display(ByVal msg As String)
        If line IsNot Nothing Then
            AnimateWindow(Me.Handle.ToInt32, 250, AW_HIDE + AW_HOR_NEGATIVE)
            AnimateWindow(Me.Handle.ToInt32, 250, AW_ACTIVATE + AW_HOR_POSITIVE)
        End If
        Me.Show()
        line = msg.Split(vbCr.ToCharArray)
        ' Make bloody sure we stay in front. Activate seems to be the key, the rest can't hurt
        Me.Activate()
        Me.BringToFront()
        Me.Focus()
        Me.Refresh()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub Explain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim screen As System.Windows.Forms.Screen = System.Windows.Forms.Screen.PrimaryScreen
        Dim bounds As Rectangle = screen.Bounds

        Me.Left = bounds.Width \ 4
        Me.Top = (bounds.Height - Me.Height) \ 2
    End Sub
    Private Sub Explain_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) _
        Handles Me.MouseClick
        If MousePointerInArea(e.X, e.Y, closerect) Then
            Me.Close()
            RaiseEvent Abort()
        End If
    End Sub
    Private Function MousePointerInArea(ByVal mousex As Integer, ByVal mousey As Integer, ByVal area As Rectangle) As Boolean
        Dim relativePoint As New Point(mousex, mousey)
        Return area.Contains(relativePoint)
    End Function
End Class
