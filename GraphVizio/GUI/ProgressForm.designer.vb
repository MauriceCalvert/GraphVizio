Partial Friend Class ProgressForm
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()> _
    Friend Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProgressForm))
        Me.CancelButton = New System.Windows.Forms.Button
        Me.Progress = New System.Windows.Forms.ProgressBar
        Me.Status = New System.Windows.Forms.TextBox
        Me.Timer = New System.Windows.Forms.Timer(Me.components)
        Me.TextBox = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'CancelButton
        '
        Me.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelButton.Location = New System.Drawing.Point(102, 26)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(53, 24)
        Me.CancelButton.TabIndex = 1
        Me.CancelButton.Text = "&Cancel"
        Me.CancelButton.UseVisualStyleBackColor = True
        '
        'Progress
        '
        Me.Progress.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Progress.Location = New System.Drawing.Point(0, 72)
        Me.Progress.Name = "Progress"
        Me.Progress.Size = New System.Drawing.Size(260, 13)
        Me.Progress.Step = 1
        Me.Progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.Progress.TabIndex = 4
        '
        'Status
        '
        Me.Status.BackColor = System.Drawing.SystemColors.Control
        Me.Status.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Status.Location = New System.Drawing.Point(12, 53)
        Me.Status.Name = "Status"
        Me.Status.Size = New System.Drawing.Size(236, 13)
        Me.Status.TabIndex = 5
        '
        'TextBox
        '
        Me.TextBox.BackColor = System.Drawing.SystemColors.Control
        Me.TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox.Location = New System.Drawing.Point(12, 8)
        Me.TextBox.Name = "TextBox"
        Me.TextBox.ReadOnly = True
        Me.TextBox.Size = New System.Drawing.Size(236, 13)
        Me.TextBox.TabIndex = 6
        Me.TextBox.Text = "Press ESC to cancel"
        Me.TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ProgressForm
        '
        Me.ClientSize = New System.Drawing.Size(260, 85)
        Me.Controls.Add(Me.TextBox)
        Me.Controls.Add(Me.Status)
        Me.Controls.Add(Me.Progress)
        Me.Controls.Add(Me.CancelButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ProgressForm"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "GraphVizio progress"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Shadows WithEvents CancelButton As System.Windows.Forms.Button
    Friend WithEvents Progress As System.Windows.Forms.ProgressBar
    Friend WithEvents Status As System.Windows.Forms.TextBox
    Friend WithEvents Timer As System.Windows.Forms.Timer
    Friend WithEvents TextBox As System.Windows.Forms.TextBox
End Class
