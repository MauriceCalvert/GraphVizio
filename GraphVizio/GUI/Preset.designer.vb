<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Preset
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Preset))
        Me.Label1 = New System.Windows.Forms.Label
        Me.presetname = New System.Windows.Forms.TextBox
        Me.save = New System.Windows.Forms.Button
        Me.cancel = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(66, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Preset name"
        '
        'presetname
        '
        Me.presetname.Location = New System.Drawing.Point(84, 6)
        Me.presetname.Name = "presetname"
        Me.presetname.Size = New System.Drawing.Size(227, 20)
        Me.presetname.TabIndex = 1
        '
        'save
        '
        Me.save.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.save.Location = New System.Drawing.Point(84, 32)
        Me.save.Name = "save"
        Me.save.Size = New System.Drawing.Size(49, 22)
        Me.save.TabIndex = 2
        Me.save.Text = "Save"
        Me.save.UseVisualStyleBackColor = True
        '
        'cancel
        '
        Me.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cancel.Location = New System.Drawing.Point(262, 32)
        Me.cancel.Name = "cancel"
        Me.cancel.Size = New System.Drawing.Size(49, 22)
        Me.cancel.TabIndex = 2
        Me.cancel.Text = "Cancel"
        Me.cancel.UseVisualStyleBackColor = True
        '
        'Preset
        '
        Me.AcceptButton = Me.save
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cancel
        Me.ClientSize = New System.Drawing.Size(316, 59)
        Me.ControlBox = False
        Me.Controls.Add(Me.cancel)
        Me.Controls.Add(Me.save)
        Me.Controls.Add(Me.presetname)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Preset"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Save Preset"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents presetname As System.Windows.Forms.TextBox
    Friend WithEvents save As System.Windows.Forms.Button
    Friend WithEvents cancel As System.Windows.Forms.Button
End Class
