<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Options
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Options))
        Me.chkDrawingUpdates = New System.Windows.Forms.CheckBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtTooltipDelay = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmbHelpSpeed = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtFadeDelay = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtFadePct = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtresizeamount = New System.Windows.Forms.TextBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtBinaries = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'chkDrawingUpdates
        '
        Me.chkDrawingUpdates.AutoSize = True
        Me.chkDrawingUpdates.Location = New System.Drawing.Point(139, 114)
        Me.chkDrawingUpdates.Name = "chkDrawingUpdates"
        Me.chkDrawingUpdates.Size = New System.Drawing.Size(15, 14)
        Me.chkDrawingUpdates.TabIndex = 45
        Me.chkDrawingUpdates.UseVisualStyleBackColor = True
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(22, 114)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(111, 13)
        Me.Label14.TabIndex = 44
        Me.Label14.Text = "Update whilst drawing"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(175, 61)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(47, 13)
        Me.Label8.TabIndex = 43
        Me.Label8.Text = "seconds"
        '
        'txtTooltipDelay
        '
        Me.txtTooltipDelay.Location = New System.Drawing.Point(139, 58)
        Me.txtTooltipDelay.Name = "txtTooltipDelay"
        Me.txtTooltipDelay.Size = New System.Drawing.Size(30, 20)
        Me.txtTooltipDelay.TabIndex = 42
        Me.txtTooltipDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(25, 61)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(108, 13)
        Me.Label7.TabIndex = 41
        Me.Label7.Text = "Settings Tooltip delay"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbHelpSpeed
        '
        Me.cmbHelpSpeed.FormattingEnabled = True
        Me.cmbHelpSpeed.Items.AddRange(New Object() {"Slow", "Normal", "Fast"})
        Me.cmbHelpSpeed.Location = New System.Drawing.Point(139, 84)
        Me.cmbHelpSpeed.Name = "cmbHelpSpeed"
        Me.cmbHelpSpeed.Size = New System.Drawing.Size(80, 21)
        Me.cmbHelpSpeed.TabIndex = 40
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(50, 87)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(83, 13)
        Me.Label5.TabIndex = 39
        Me.Label5.Text = "Help play speed"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(256, 35)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(47, 13)
        Me.Label6.TabIndex = 38
        Me.Label6.Text = "seconds"
        '
        'txtFadeDelay
        '
        Me.txtFadeDelay.Location = New System.Drawing.Point(220, 32)
        Me.txtFadeDelay.Name = "txtFadeDelay"
        Me.txtFadeDelay.Size = New System.Drawing.Size(30, 20)
        Me.txtFadeDelay.TabIndex = 37
        Me.txtFadeDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(175, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 13)
        Me.Label4.TabIndex = 36
        Me.Label4.Text = "% after"
        '
        'txtFadePct
        '
        Me.txtFadePct.Location = New System.Drawing.Point(139, 32)
        Me.txtFadePct.Name = "txtFadePct"
        Me.txtFadePct.Size = New System.Drawing.Size(30, 20)
        Me.txtFadePct.TabIndex = 35
        Me.txtFadePct.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(121, 13)
        Me.Label3.TabIndex = 34
        Me.Label3.Text = "Fade settings window to"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(256, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(15, 13)
        Me.Label2.TabIndex = 33
        Me.Label2.Text = "%"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(194, 13)
        Me.Label1.TabIndex = 32
        Me.Label1.Text = "Wider, Narrower, Taller, Shorter amount"
        '
        'txtresizeamount
        '
        Me.txtresizeamount.Location = New System.Drawing.Point(220, 6)
        Me.txtresizeamount.Name = "txtresizeamount"
        Me.txtresizeamount.Size = New System.Drawing.Size(30, 20)
        Me.txtresizeamount.TabIndex = 31
        Me.txtresizeamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(115, 183)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 46
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(12, 141)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(117, 13)
        Me.Label9.TabIndex = 47
        Me.Label9.Text = "Graphviz binaries folder"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtBinaries
        '
        Me.txtBinaries.Location = New System.Drawing.Point(15, 157)
        Me.txtBinaries.Name = "txtBinaries"
        Me.txtBinaries.Size = New System.Drawing.Size(283, 20)
        Me.txtBinaries.TabIndex = 48
        Me.txtBinaries.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Options
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(310, 213)
        Me.Controls.Add(Me.txtBinaries)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.chkDrawingUpdates)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtTooltipDelay)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.cmbHelpSpeed)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtFadeDelay)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtFadePct)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtresizeamount)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Options"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "GraphVizio Options"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkDrawingUpdates As System.Windows.Forms.CheckBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtTooltipDelay As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cmbHelpSpeed As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtFadeDelay As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtFadePct As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtresizeamount As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As Windows.Forms.Button
    Friend WithEvents Label9 As Windows.Forms.Label
    Friend WithEvents txtBinaries As Windows.Forms.TextBox
End Class
