Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Friend Class Pointer
    Dim arrow As System.Drawing.Bitmap
    Dim myleft As Integer
    Dim mytop As Integer
    Sub New(ByVal p As Point)
        InitializeComponent()
        myleft = p.X
        mytop = p.Y
    End Sub
    Protected Overrides Sub OnPaintBackground(ByVal e As System.Windows.Forms.PaintEventArgs)
        e.Graphics.CompositingQuality = CompositingQuality.HighQuality
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality
        e.Graphics.Clip = Me.Region
        e.Graphics.DrawImage(arrow, New Point(0, 0))
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
    End Sub
    Private Sub Pointer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        arrow = My.Resources.Pointer
        Me.Left = myleft - 55
        Me.Top = mytop + 25
        Me.Region = ConvertBitmapToRegion(arrow)
    End Sub
End Class