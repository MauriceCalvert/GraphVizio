<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Settings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Settings))
        Me.btnLayout = New System.Windows.Forms.Button
        Me.algorithm = New System.Windows.Forms.GroupBox
        Me.circo = New System.Windows.Forms.RadioButton
        Me.twopi = New System.Windows.Forms.RadioButton
        Me.fdp = New System.Windows.Forms.RadioButton
        Me.neato = New System.Windows.Forms.RadioButton
        Me.dot = New System.Windows.Forms.RadioButton
        Me.TabControl = New System.Windows.Forms.TabControl
        Me.tpGraph = New System.Windows.Forms.TabPage
        Me.aspectratio = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.rankbyposition = New System.Windows.Forms.CheckBox
        Me.leafstack = New System.Windows.Forms.ComboBox
        Me.lblStackLeaves = New System.Windows.Forms.Label
        Me.rankdir = New System.Windows.Forms.GroupBox
        Me.lr = New System.Windows.Forms.RadioButton
        Me.tb = New System.Windows.Forms.RadioButton
        Me.tpShapes = New System.Windows.Forms.TabPage
        Me.shapelinecolour = New System.Windows.Forms.PictureBox
        Me.shapelinecolours = New System.Windows.Forms.CheckBox
        Me.shapetextcolour = New System.Windows.Forms.PictureBox
        Me.shapetextcolours = New System.Windows.Forms.CheckBox
        Me.shapefillcolour = New System.Windows.Forms.PictureBox
        Me.shapefillcolours = New System.Windows.Forms.CheckBox
        Me.shapename = New System.Windows.Forms.ComboBox
        Me.shapeis = New System.Windows.Forms.CheckBox
        Me.tpConnectors = New System.Windows.Forms.TabPage
        Me.connectto = New System.Windows.Forms.GroupBox
        Me.centre = New System.Windows.Forms.RadioButton
        Me.glue = New System.Windows.Forms.RadioButton
        Me.quadrant = New System.Windows.Forms.RadioButton
        Me.topbottom = New System.Windows.Forms.RadioButton
        Me.ideal = New System.Windows.Forms.RadioButton
        Me.connectorstyle = New System.Windows.Forms.GroupBox
        Me.existing = New System.Windows.Forms.RadioButton
        Me.connectorname = New System.Windows.Forms.ComboBox
        Me.connectoris = New System.Windows.Forms.RadioButton
        Me.splines = New System.Windows.Forms.RadioButton
        Me.straight = New System.Windows.Forms.RadioButton
        Me.tpAdvanced = New System.Windows.Forms.TabPage
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lockseed = New System.Windows.Forms.CheckBox
        Me.digraph = New System.Windows.Forms.CheckBox
        Me.seed = New System.Windows.Forms.TextBox
        Me.lblSeed = New System.Windows.Forms.Label
        Me.commandoptions = New System.Windows.Forms.RichTextBox
        Me.lblCommandLineOptions = New System.Windows.Forms.Label
        Me.drawboundingboxes = New System.Windows.Forms.CheckBox
        Me.strict = New System.Windows.Forms.ComboBox
        Me.lblStrict = New System.Windows.Forms.Label
        Me.overlap = New System.Windows.Forms.ComboBox
        Me.lblOverlap = New System.Windows.Forms.Label
        Me.PictureBox = New System.Windows.Forms.PictureBox
        Me.btnImportDOT = New System.Windows.Forms.Button
        Me.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.ColorDialog = New System.Windows.Forms.ColorDialog
        Me.setroot = New System.Windows.Forms.Button
        Me.showroot = New System.Windows.Forms.Button
        Me.ToolTipWidget = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblSample = New System.Windows.Forms.Label
        Me.btnMenu = New System.Windows.Forms.Button
        Me.algorithm.SuspendLayout()
        Me.TabControl.SuspendLayout()
        Me.tpGraph.SuspendLayout()
        Me.rankdir.SuspendLayout()
        Me.tpShapes.SuspendLayout()
        CType(Me.shapelinecolour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.shapetextcolour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.shapefillcolour, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpConnectors.SuspendLayout()
        Me.connectto.SuspendLayout()
        Me.connectorstyle.SuspendLayout()
        Me.tpAdvanced.SuspendLayout()
        CType(Me.PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnLayout
        '
        Me.btnLayout.Location = New System.Drawing.Point(4, 180)
        Me.btnLayout.Name = "btnLayout"
        Me.btnLayout.Size = New System.Drawing.Size(68, 25)
        Me.btnLayout.TabIndex = 0
        Me.btnLayout.Text = "&Layout"
        Me.btnLayout.UseVisualStyleBackColor = True
        '
        'algorithm
        '
        Me.algorithm.Controls.Add(Me.circo)
        Me.algorithm.Controls.Add(Me.twopi)
        Me.algorithm.Controls.Add(Me.fdp)
        Me.algorithm.Controls.Add(Me.neato)
        Me.algorithm.Controls.Add(Me.dot)
        Me.algorithm.Location = New System.Drawing.Point(8, 4)
        Me.algorithm.Name = "algorithm"
        Me.algorithm.Size = New System.Drawing.Size(93, 142)
        Me.algorithm.TabIndex = 1
        Me.algorithm.TabStop = False
        Me.algorithm.Text = "Algorithm"
        '
        'circo
        '
        Me.circo.AutoSize = True
        Me.circo.Location = New System.Drawing.Point(8, 119)
        Me.circo.Name = "circo"
        Me.circo.Size = New System.Drawing.Size(60, 17)
        Me.circo.TabIndex = 4
        Me.circo.Text = "Circul&ar"
        Me.circo.UseVisualStyleBackColor = True
        '
        'twopi
        '
        Me.twopi.AutoSize = True
        Me.twopi.Location = New System.Drawing.Point(8, 94)
        Me.twopi.Name = "twopi"
        Me.twopi.Size = New System.Drawing.Size(76, 17)
        Me.twopi.TabIndex = 3
        Me.twopi.Text = "Co&ncentric"
        Me.twopi.UseVisualStyleBackColor = True
        '
        'fdp
        '
        Me.fdp.AutoSize = True
        Me.fdp.Location = New System.Drawing.Point(8, 69)
        Me.fdp.Name = "fdp"
        Me.fdp.Size = New System.Drawing.Size(69, 17)
        Me.fdp.TabIndex = 2
        Me.fdp.Text = "Clu&stered"
        Me.fdp.UseVisualStyleBackColor = True
        '
        'neato
        '
        Me.neato.AutoSize = True
        Me.neato.Location = New System.Drawing.Point(8, 44)
        Me.neato.Name = "neato"
        Me.neato.Size = New System.Drawing.Size(42, 17)
        Me.neato.TabIndex = 1
        Me.neato.Text = "&Flat"
        Me.neato.UseVisualStyleBackColor = True
        '
        'dot
        '
        Me.dot.AutoSize = True
        Me.dot.Location = New System.Drawing.Point(8, 19)
        Me.dot.Name = "dot"
        Me.dot.Size = New System.Drawing.Size(81, 17)
        Me.dot.TabIndex = 0
        Me.dot.Text = "&Hierarchical"
        Me.dot.UseVisualStyleBackColor = True
        '
        'TabControl
        '
        Me.TabControl.Controls.Add(Me.tpGraph)
        Me.TabControl.Controls.Add(Me.tpShapes)
        Me.TabControl.Controls.Add(Me.tpConnectors)
        Me.TabControl.Controls.Add(Me.tpAdvanced)
        Me.TabControl.Location = New System.Drawing.Point(0, 0)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(306, 178)
        Me.TabControl.TabIndex = 5
        '
        'tpGraph
        '
        Me.tpGraph.Controls.Add(Me.aspectratio)
        Me.tpGraph.Controls.Add(Me.Label5)
        Me.tpGraph.Controls.Add(Me.Label4)
        Me.tpGraph.Controls.Add(Me.rankbyposition)
        Me.tpGraph.Controls.Add(Me.leafstack)
        Me.tpGraph.Controls.Add(Me.lblStackLeaves)
        Me.tpGraph.Controls.Add(Me.rankdir)
        Me.tpGraph.Controls.Add(Me.algorithm)
        Me.tpGraph.Location = New System.Drawing.Point(4, 22)
        Me.tpGraph.Name = "tpGraph"
        Me.tpGraph.Padding = New System.Windows.Forms.Padding(3)
        Me.tpGraph.Size = New System.Drawing.Size(298, 152)
        Me.tpGraph.TabIndex = 0
        Me.tpGraph.Text = "Diagram"
        Me.tpGraph.UseVisualStyleBackColor = True
        '
        'aspectratio
        '
        Me.aspectratio.Location = New System.Drawing.Point(197, 97)
        Me.aspectratio.Name = "aspectratio"
        Me.aspectratio.Size = New System.Drawing.Size(79, 20)
        Me.aspectratio.TabIndex = 24
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(110, 100)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 13)
        Me.Label5.TabIndex = 23
        Me.Label5.Text = "As&pect ratio"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(110, 75)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(86, 13)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "&Rank by position"
        '
        'rankbyposition
        '
        Me.rankbyposition.AutoSize = True
        Me.rankbyposition.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rankbyposition.Location = New System.Drawing.Point(197, 75)
        Me.rankbyposition.Name = "rankbyposition"
        Me.rankbyposition.Size = New System.Drawing.Size(15, 14)
        Me.rankbyposition.TabIndex = 20
        Me.rankbyposition.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rankbyposition.UseVisualStyleBackColor = True
        '
        'leafstack
        '
        Me.leafstack.FormattingEnabled = True
        Me.leafstack.Items.AddRange(New Object() {"none", "all", "lowest"})
        Me.leafstack.Location = New System.Drawing.Point(197, 122)
        Me.leafstack.Name = "leafstack"
        Me.leafstack.Size = New System.Drawing.Size(95, 21)
        Me.leafstack.TabIndex = 17
        Me.leafstack.Text = "none"
        '
        'lblStackLeaves
        '
        Me.lblStackLeaves.AutoSize = True
        Me.lblStackLeaves.Location = New System.Drawing.Point(110, 125)
        Me.lblStackLeaves.Name = "lblStackLeaves"
        Me.lblStackLeaves.Size = New System.Drawing.Size(69, 13)
        Me.lblStackLeaves.TabIndex = 16
        Me.lblStackLeaves.Text = "Stac&k leaves"
        '
        'rankdir
        '
        Me.rankdir.Controls.Add(Me.lr)
        Me.rankdir.Controls.Add(Me.tb)
        Me.rankdir.Location = New System.Drawing.Point(109, 4)
        Me.rankdir.Name = "rankdir"
        Me.rankdir.Size = New System.Drawing.Size(112, 63)
        Me.rankdir.TabIndex = 3
        Me.rankdir.TabStop = False
        Me.rankdir.Text = "Rank Direction"
        '
        'lr
        '
        Me.lr.AutoSize = True
        Me.lr.Location = New System.Drawing.Point(11, 40)
        Me.lr.Name = "lr"
        Me.lr.Size = New System.Drawing.Size(83, 17)
        Me.lr.TabIndex = 1
        Me.lr.Text = "&Left to Right"
        Me.lr.UseVisualStyleBackColor = True
        '
        'tb
        '
        Me.tb.AutoSize = True
        Me.tb.Location = New System.Drawing.Point(11, 17)
        Me.tb.Name = "tb"
        Me.tb.Size = New System.Drawing.Size(92, 17)
        Me.tb.TabIndex = 0
        Me.tb.Text = "&Top to Bottom"
        Me.tb.UseVisualStyleBackColor = True
        '
        'tpShapes
        '
        Me.tpShapes.Controls.Add(Me.shapelinecolour)
        Me.tpShapes.Controls.Add(Me.shapelinecolours)
        Me.tpShapes.Controls.Add(Me.shapetextcolour)
        Me.tpShapes.Controls.Add(Me.shapetextcolours)
        Me.tpShapes.Controls.Add(Me.shapefillcolour)
        Me.tpShapes.Controls.Add(Me.shapefillcolours)
        Me.tpShapes.Controls.Add(Me.shapename)
        Me.tpShapes.Controls.Add(Me.shapeis)
        Me.tpShapes.Location = New System.Drawing.Point(4, 22)
        Me.tpShapes.Name = "tpShapes"
        Me.tpShapes.Padding = New System.Windows.Forms.Padding(3)
        Me.tpShapes.Size = New System.Drawing.Size(298, 152)
        Me.tpShapes.TabIndex = 1
        Me.tpShapes.Text = "Shapes"
        Me.tpShapes.UseVisualStyleBackColor = True
        '
        'shapelinecolour
        '
        Me.shapelinecolour.BackColor = System.Drawing.Color.White
        Me.shapelinecolour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.shapelinecolour.Location = New System.Drawing.Point(85, 90)
        Me.shapelinecolour.Name = "shapelinecolour"
        Me.shapelinecolour.Size = New System.Drawing.Size(25, 16)
        Me.shapelinecolour.TabIndex = 7
        Me.shapelinecolour.TabStop = False
        '
        'shapelinecolours
        '
        Me.shapelinecolours.AutoSize = True
        Me.shapelinecolours.Location = New System.Drawing.Point(13, 90)
        Me.shapelinecolours.Name = "shapelinecolours"
        Me.shapelinecolours.Size = New System.Drawing.Size(74, 17)
        Me.shapelinecolours.TabIndex = 6
        Me.shapelinecolours.Text = "&line colour"
        Me.shapelinecolours.UseVisualStyleBackColor = True
        '
        'shapetextcolour
        '
        Me.shapetextcolour.BackColor = System.Drawing.Color.White
        Me.shapetextcolour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.shapetextcolour.Location = New System.Drawing.Point(85, 67)
        Me.shapetextcolour.Name = "shapetextcolour"
        Me.shapetextcolour.Size = New System.Drawing.Size(25, 16)
        Me.shapetextcolour.TabIndex = 5
        Me.shapetextcolour.TabStop = False
        '
        'shapetextcolours
        '
        Me.shapetextcolours.AutoSize = True
        Me.shapetextcolours.Location = New System.Drawing.Point(13, 67)
        Me.shapetextcolours.Name = "shapetextcolours"
        Me.shapetextcolours.Size = New System.Drawing.Size(75, 17)
        Me.shapetextcolours.TabIndex = 4
        Me.shapetextcolours.Text = "&text colour"
        Me.shapetextcolours.UseVisualStyleBackColor = True
        '
        'shapefillcolour
        '
        Me.shapefillcolour.BackColor = System.Drawing.Color.White
        Me.shapefillcolour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.shapefillcolour.Location = New System.Drawing.Point(85, 43)
        Me.shapefillcolour.Name = "shapefillcolour"
        Me.shapefillcolour.Size = New System.Drawing.Size(25, 16)
        Me.shapefillcolour.TabIndex = 3
        Me.shapefillcolour.TabStop = False
        '
        'shapefillcolours
        '
        Me.shapefillcolours.AutoSize = True
        Me.shapefillcolours.Location = New System.Drawing.Point(13, 44)
        Me.shapefillcolours.Name = "shapefillcolours"
        Me.shapefillcolours.Size = New System.Drawing.Size(67, 17)
        Me.shapefillcolours.TabIndex = 2
        Me.shapefillcolours.Text = "&fill colour"
        Me.shapefillcolours.UseVisualStyleBackColor = True
        '
        'shapename
        '
        Me.shapename.FormattingEnabled = True
        Me.shapename.Location = New System.Drawing.Point(85, 10)
        Me.shapename.Name = "shapename"
        Me.shapename.Size = New System.Drawing.Size(131, 21)
        Me.shapename.TabIndex = 1
        '
        'shapeis
        '
        Me.shapeis.AutoSize = True
        Me.shapeis.Location = New System.Drawing.Point(13, 12)
        Me.shapeis.Name = "shapeis"
        Me.shapeis.Size = New System.Drawing.Size(55, 17)
        Me.shapeis.TabIndex = 0
        Me.shapeis.Text = "&shape"
        Me.shapeis.UseVisualStyleBackColor = True
        '
        'tpConnectors
        '
        Me.tpConnectors.Controls.Add(Me.connectto)
        Me.tpConnectors.Controls.Add(Me.connectorstyle)
        Me.tpConnectors.Location = New System.Drawing.Point(4, 22)
        Me.tpConnectors.Name = "tpConnectors"
        Me.tpConnectors.Size = New System.Drawing.Size(298, 152)
        Me.tpConnectors.TabIndex = 2
        Me.tpConnectors.Text = "Connectors"
        Me.tpConnectors.UseVisualStyleBackColor = True
        '
        'connectto
        '
        Me.connectto.Controls.Add(Me.centre)
        Me.connectto.Controls.Add(Me.glue)
        Me.connectto.Controls.Add(Me.quadrant)
        Me.connectto.Controls.Add(Me.topbottom)
        Me.connectto.Controls.Add(Me.ideal)
        Me.connectto.Location = New System.Drawing.Point(156, 8)
        Me.connectto.Name = "connectto"
        Me.connectto.Size = New System.Drawing.Size(125, 134)
        Me.connectto.TabIndex = 1
        Me.connectto.TabStop = False
        Me.connectto.Text = "Connection points"
        '
        'centre
        '
        Me.centre.AutoSize = True
        Me.centre.Location = New System.Drawing.Point(9, 42)
        Me.centre.Name = "centre"
        Me.centre.Size = New System.Drawing.Size(56, 17)
        Me.centre.TabIndex = 4
        Me.centre.TabStop = True
        Me.centre.Text = "&Centre"
        Me.centre.UseVisualStyleBackColor = True
        '
        'glue
        '
        Me.glue.AutoSize = True
        Me.glue.Location = New System.Drawing.Point(9, 19)
        Me.glue.Name = "glue"
        Me.glue.Size = New System.Drawing.Size(91, 17)
        Me.glue.TabIndex = 3
        Me.glue.TabStop = True
        Me.glue.Text = "&Dynamic Glue"
        Me.glue.UseVisualStyleBackColor = True
        '
        'quadrant
        '
        Me.quadrant.AutoSize = True
        Me.quadrant.Location = New System.Drawing.Point(9, 111)
        Me.quadrant.Name = "quadrant"
        Me.quadrant.Size = New System.Drawing.Size(107, 17)
        Me.quadrant.TabIndex = 2
        Me.quadrant.TabStop = True
        Me.quadrant.Text = "&Nearest quadrant"
        Me.quadrant.UseVisualStyleBackColor = True
        '
        'topbottom
        '
        Me.topbottom.AutoSize = True
        Me.topbottom.Location = New System.Drawing.Point(9, 88)
        Me.topbottom.Name = "topbottom"
        Me.topbottom.Size = New System.Drawing.Size(100, 17)
        Me.topbottom.TabIndex = 1
        Me.topbottom.TabStop = True
        Me.topbottom.Text = "&Top and bottom"
        Me.topbottom.UseVisualStyleBackColor = True
        '
        'ideal
        '
        Me.ideal.AutoSize = True
        Me.ideal.Location = New System.Drawing.Point(9, 65)
        Me.ideal.Name = "ideal"
        Me.ideal.Size = New System.Drawing.Size(74, 17)
        Me.ideal.TabIndex = 0
        Me.ideal.TabStop = True
        Me.ideal.Text = "&Ideal point"
        Me.ideal.UseVisualStyleBackColor = True
        '
        'connectorstyle
        '
        Me.connectorstyle.Controls.Add(Me.existing)
        Me.connectorstyle.Controls.Add(Me.connectorname)
        Me.connectorstyle.Controls.Add(Me.connectoris)
        Me.connectorstyle.Controls.Add(Me.splines)
        Me.connectorstyle.Controls.Add(Me.straight)
        Me.connectorstyle.Location = New System.Drawing.Point(9, 8)
        Me.connectorstyle.Name = "connectorstyle"
        Me.connectorstyle.Size = New System.Drawing.Size(141, 134)
        Me.connectorstyle.TabIndex = 0
        Me.connectorstyle.TabStop = False
        Me.connectorstyle.Text = "Draw connectors with"
        '
        'existing
        '
        Me.existing.AutoSize = True
        Me.existing.Location = New System.Drawing.Point(8, 19)
        Me.existing.Name = "existing"
        Me.existing.Size = New System.Drawing.Size(61, 17)
        Me.existing.TabIndex = 4
        Me.existing.TabStop = True
        Me.existing.Text = "E&xisting"
        Me.existing.UseVisualStyleBackColor = True
        '
        'connectorname
        '
        Me.connectorname.FormattingEnabled = True
        Me.connectorname.Location = New System.Drawing.Point(28, 87)
        Me.connectorname.Name = "connectorname"
        Me.connectorname.Size = New System.Drawing.Size(106, 21)
        Me.connectorname.TabIndex = 3
        Me.connectorname.Tag = ""
        '
        'connectoris
        '
        Me.connectoris.AutoSize = True
        Me.connectoris.Location = New System.Drawing.Point(8, 90)
        Me.connectoris.Name = "connectoris"
        Me.connectoris.Size = New System.Drawing.Size(14, 13)
        Me.connectoris.TabIndex = 2
        Me.connectoris.TabStop = True
        Me.connectoris.UseVisualStyleBackColor = True
        '
        'splines
        '
        Me.splines.AutoSize = True
        Me.splines.Location = New System.Drawing.Point(8, 65)
        Me.splines.Name = "splines"
        Me.splines.Size = New System.Drawing.Size(124, 17)
        Me.splines.TabIndex = 1
        Me.splines.TabStop = True
        Me.splines.Text = "Cur&ved lines (splines)"
        Me.splines.UseVisualStyleBackColor = True
        '
        'straight
        '
        Me.straight.AutoSize = True
        Me.straight.Location = New System.Drawing.Point(8, 42)
        Me.straight.Name = "straight"
        Me.straight.Size = New System.Drawing.Size(85, 17)
        Me.straight.TabIndex = 0
        Me.straight.TabStop = True
        Me.straight.Text = "&Straight lines"
        Me.straight.UseVisualStyleBackColor = True
        '
        'tpAdvanced
        '
        Me.tpAdvanced.Controls.Add(Me.Label3)
        Me.tpAdvanced.Controls.Add(Me.Label2)
        Me.tpAdvanced.Controls.Add(Me.Label1)
        Me.tpAdvanced.Controls.Add(Me.lockseed)
        Me.tpAdvanced.Controls.Add(Me.digraph)
        Me.tpAdvanced.Controls.Add(Me.seed)
        Me.tpAdvanced.Controls.Add(Me.lblSeed)
        Me.tpAdvanced.Controls.Add(Me.commandoptions)
        Me.tpAdvanced.Controls.Add(Me.lblCommandLineOptions)
        Me.tpAdvanced.Controls.Add(Me.drawboundingboxes)
        Me.tpAdvanced.Controls.Add(Me.strict)
        Me.tpAdvanced.Controls.Add(Me.lblStrict)
        Me.tpAdvanced.Controls.Add(Me.overlap)
        Me.tpAdvanced.Controls.Add(Me.lblOverlap)
        Me.tpAdvanced.Location = New System.Drawing.Point(4, 22)
        Me.tpAdvanced.Name = "tpAdvanced"
        Me.tpAdvanced.Size = New System.Drawing.Size(298, 152)
        Me.tpAdvanced.TabIndex = 8
        Me.tpAdvanced.Text = "Advanced"
        Me.tpAdvanced.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(201, 66)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 13)
        Me.Label3.TabIndex = 26
        Me.Label3.Text = "&Lock seed"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(201, 39)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 13)
        Me.Label2.TabIndex = 25
        Me.Label2.Text = "&Bounding Boxes"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(201, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "&Directed graph"
        '
        'lockseed
        '
        Me.lockseed.AutoSize = True
        Me.lockseed.Location = New System.Drawing.Point(186, 66)
        Me.lockseed.Name = "lockseed"
        Me.lockseed.Size = New System.Drawing.Size(15, 14)
        Me.lockseed.TabIndex = 23
        Me.lockseed.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lockseed.UseVisualStyleBackColor = True
        '
        'digraph
        '
        Me.digraph.AutoSize = True
        Me.digraph.Location = New System.Drawing.Point(186, 13)
        Me.digraph.Name = "digraph"
        Me.digraph.Size = New System.Drawing.Size(15, 14)
        Me.digraph.TabIndex = 22
        Me.digraph.UseVisualStyleBackColor = True
        '
        'seed
        '
        Me.seed.Location = New System.Drawing.Point(55, 63)
        Me.seed.Name = "seed"
        Me.seed.Size = New System.Drawing.Size(107, 20)
        Me.seed.TabIndex = 21
        '
        'lblSeed
        '
        Me.lblSeed.AutoSize = True
        Me.lblSeed.Location = New System.Drawing.Point(17, 66)
        Me.lblSeed.Name = "lblSeed"
        Me.lblSeed.Size = New System.Drawing.Size(32, 13)
        Me.lblSeed.TabIndex = 20
        Me.lblSeed.Text = "S&eed"
        '
        'commandoptions
        '
        Me.commandoptions.Location = New System.Drawing.Point(8, 106)
        Me.commandoptions.Name = "commandoptions"
        Me.commandoptions.Size = New System.Drawing.Size(274, 38)
        Me.commandoptions.TabIndex = 19
        Me.commandoptions.Text = ""
        '
        'lblCommandLineOptions
        '
        Me.lblCommandLineOptions.AutoSize = True
        Me.lblCommandLineOptions.Location = New System.Drawing.Point(8, 90)
        Me.lblCommandLineOptions.Name = "lblCommandLineOptions"
        Me.lblCommandLineOptions.Size = New System.Drawing.Size(158, 13)
        Me.lblCommandLineOptions.TabIndex = 18
        Me.lblCommandLineOptions.Text = "&GraphViz command-line options:"
        '
        'drawboundingboxes
        '
        Me.drawboundingboxes.AutoSize = True
        Me.drawboundingboxes.Location = New System.Drawing.Point(186, 39)
        Me.drawboundingboxes.Name = "drawboundingboxes"
        Me.drawboundingboxes.Size = New System.Drawing.Size(15, 14)
        Me.drawboundingboxes.TabIndex = 15
        Me.drawboundingboxes.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.drawboundingboxes.UseVisualStyleBackColor = True
        '
        'strict
        '
        Me.strict.FormattingEnabled = True
        Me.strict.Items.AddRange(New Object() {"false", "true"})
        Me.strict.Location = New System.Drawing.Point(55, 9)
        Me.strict.Name = "strict"
        Me.strict.Size = New System.Drawing.Size(107, 21)
        Me.strict.TabIndex = 14
        Me.strict.Text = "false"
        '
        'lblStrict
        '
        Me.lblStrict.AutoSize = True
        Me.lblStrict.Location = New System.Drawing.Point(17, 12)
        Me.lblStrict.Name = "lblStrict"
        Me.lblStrict.Size = New System.Drawing.Size(31, 13)
        Me.lblStrict.TabIndex = 13
        Me.lblStrict.Text = "&Strict"
        '
        'overlap
        '
        Me.overlap.FormattingEnabled = True
        Me.overlap.Items.AddRange(New Object() {"false", "true", "scale", "compress", "ipsep", "scalexy", "ortho", "orthoxy", "orthoyx", "ortho_yx", "portho", "porthoxy", "porthoyx", "portho_yx", "prism", "vpsc"})
        Me.overlap.Location = New System.Drawing.Point(55, 36)
        Me.overlap.Name = "overlap"
        Me.overlap.Size = New System.Drawing.Size(107, 21)
        Me.overlap.TabIndex = 12
        Me.overlap.Text = "false"
        '
        'lblOverlap
        '
        Me.lblOverlap.AutoSize = True
        Me.lblOverlap.Location = New System.Drawing.Point(5, 39)
        Me.lblOverlap.Name = "lblOverlap"
        Me.lblOverlap.Size = New System.Drawing.Size(44, 13)
        Me.lblOverlap.TabIndex = 11
        Me.lblOverlap.Text = "&Overlap"
        '
        'PictureBox
        '
        Me.PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox.Location = New System.Drawing.Point(312, 21)
        Me.PictureBox.Name = "PictureBox"
        Me.PictureBox.Size = New System.Drawing.Size(156, 156)
        Me.PictureBox.TabIndex = 17
        Me.PictureBox.TabStop = False
        '
        'btnImportDOT
        '
        Me.btnImportDOT.Location = New System.Drawing.Point(78, 180)
        Me.btnImportDOT.Name = "btnImportDOT"
        Me.btnImportDOT.Size = New System.Drawing.Size(68, 25)
        Me.btnImportDOT.TabIndex = 1
        Me.btnImportDOT.Text = "&Import..."
        Me.btnImportDOT.UseVisualStyleBackColor = True
        '
        'OpenFileDialog
        '
        Me.OpenFileDialog.Filter = "Graphviz files|*.gv|All files|*.*"
        '
        'setroot
        '
        Me.setroot.Location = New System.Drawing.Point(152, 180)
        Me.setroot.Name = "setroot"
        Me.setroot.Size = New System.Drawing.Size(68, 25)
        Me.setroot.TabIndex = 2
        Me.setroot.Text = "Set &Root"
        Me.setroot.UseVisualStyleBackColor = True
        '
        'showroot
        '
        Me.showroot.Location = New System.Drawing.Point(226, 180)
        Me.showroot.Name = "showroot"
        Me.showroot.Size = New System.Drawing.Size(68, 25)
        Me.showroot.TabIndex = 3
        Me.showroot.Text = "&Show Root"
        Me.showroot.UseVisualStyleBackColor = True
        '
        'ToolTipWidget
        '
        Me.ToolTipWidget.AutomaticDelay = 1000
        Me.ToolTipWidget.IsBalloon = True
        '
        'lblSample
        '
        Me.lblSample.AutoSize = True
        Me.lblSample.Location = New System.Drawing.Point(312, 4)
        Me.lblSample.Name = "lblSample"
        Me.lblSample.Size = New System.Drawing.Size(42, 13)
        Me.lblSample.TabIndex = 18
        Me.lblSample.Text = "Sample"
        '
        'btnMenu
        '
        Me.btnMenu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMenu.Location = New System.Drawing.Point(392, 180)
        Me.btnMenu.Name = "btnMenu"
        Me.btnMenu.Size = New System.Drawing.Size(76, 25)
        Me.btnMenu.TabIndex = 4
        Me.btnMenu.Text = "&Graph Menu"
        Me.btnMenu.UseVisualStyleBackColor = True
        '
        'Settings
        '
        Me.AcceptButton = Me.btnLayout
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(473, 210)
        Me.Controls.Add(Me.btnMenu)
        Me.Controls.Add(Me.lblSample)
        Me.Controls.Add(Me.PictureBox)
        Me.Controls.Add(Me.showroot)
        Me.Controls.Add(Me.setroot)
        Me.Controls.Add(Me.btnImportDOT)
        Me.Controls.Add(Me.btnLayout)
        Me.Controls.Add(Me.TabControl)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Settings"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "GraphVizio Settings"
        Me.TopMost = True
        Me.algorithm.ResumeLayout(False)
        Me.algorithm.PerformLayout()
        Me.TabControl.ResumeLayout(False)
        Me.tpGraph.ResumeLayout(False)
        Me.tpGraph.PerformLayout()
        Me.rankdir.ResumeLayout(False)
        Me.rankdir.PerformLayout()
        Me.tpShapes.ResumeLayout(False)
        Me.tpShapes.PerformLayout()
        CType(Me.shapelinecolour, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.shapetextcolour, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.shapefillcolour, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpConnectors.ResumeLayout(False)
        Me.connectto.ResumeLayout(False)
        Me.connectto.PerformLayout()
        Me.connectorstyle.ResumeLayout(False)
        Me.connectorstyle.PerformLayout()
        Me.tpAdvanced.ResumeLayout(False)
        Me.tpAdvanced.PerformLayout()
        CType(Me.PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend Shadows WithEvents btnLayout As System.Windows.Forms.Button
    Friend WithEvents algorithm As System.Windows.Forms.GroupBox
    Friend WithEvents circo As System.Windows.Forms.RadioButton
    Friend WithEvents twopi As System.Windows.Forms.RadioButton
    Friend WithEvents fdp As System.Windows.Forms.RadioButton
    Friend WithEvents neato As System.Windows.Forms.RadioButton
    Friend WithEvents dot As System.Windows.Forms.RadioButton
    Friend WithEvents TabControl As System.Windows.Forms.TabControl
    Friend WithEvents tpGraph As System.Windows.Forms.TabPage
    Friend WithEvents tpShapes As System.Windows.Forms.TabPage
    Friend WithEvents tpConnectors As System.Windows.Forms.TabPage
    Friend WithEvents tpAdvanced As System.Windows.Forms.TabPage
    Friend WithEvents OpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnImportDOT As System.Windows.Forms.Button
    Friend WithEvents rankdir As System.Windows.Forms.GroupBox
    Friend WithEvents lr As System.Windows.Forms.RadioButton
    Friend WithEvents tb As System.Windows.Forms.RadioButton
    Friend WithEvents shapename As System.Windows.Forms.ComboBox
    Friend WithEvents shapeis As System.Windows.Forms.CheckBox
    Friend WithEvents shapefillcolours As System.Windows.Forms.CheckBox
    Friend WithEvents shapefillcolour As System.Windows.Forms.PictureBox
    Friend WithEvents ColorDialog As System.Windows.Forms.ColorDialog
    Friend WithEvents shapelinecolour As System.Windows.Forms.PictureBox
    Friend WithEvents shapelinecolours As System.Windows.Forms.CheckBox
    Friend WithEvents shapetextcolour As System.Windows.Forms.PictureBox
    Friend WithEvents shapetextcolours As System.Windows.Forms.CheckBox
    Friend WithEvents setroot As System.Windows.Forms.Button
    Friend WithEvents showroot As System.Windows.Forms.Button
    Friend WithEvents connectorstyle As System.Windows.Forms.GroupBox
    Friend WithEvents splines As System.Windows.Forms.RadioButton
    Friend WithEvents straight As System.Windows.Forms.RadioButton
    Friend WithEvents connectoris As System.Windows.Forms.RadioButton
    Friend WithEvents connectorname As System.Windows.Forms.ComboBox
    Friend WithEvents connectto As System.Windows.Forms.GroupBox
    Friend WithEvents quadrant As System.Windows.Forms.RadioButton
    Friend WithEvents topbottom As System.Windows.Forms.RadioButton
    Friend WithEvents ideal As System.Windows.Forms.RadioButton
    Friend WithEvents drawboundingboxes As System.Windows.Forms.CheckBox
    Friend WithEvents strict As System.Windows.Forms.ComboBox
    Friend WithEvents lblStrict As System.Windows.Forms.Label
    Friend WithEvents overlap As System.Windows.Forms.ComboBox
    Friend WithEvents lblOverlap As System.Windows.Forms.Label
    Friend WithEvents ToolTipWidget As System.Windows.Forms.ToolTip
    Friend WithEvents leafstack As System.Windows.Forms.ComboBox
    Friend WithEvents lblStackLeaves As System.Windows.Forms.Label
    Friend WithEvents PictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents lblSample As System.Windows.Forms.Label
    Friend WithEvents commandoptions As System.Windows.Forms.RichTextBox
    Friend WithEvents lblCommandLineOptions As System.Windows.Forms.Label
    Friend WithEvents rankbyposition As System.Windows.Forms.CheckBox
    Friend WithEvents existing As System.Windows.Forms.RadioButton
    Friend WithEvents glue As System.Windows.Forms.RadioButton
    Friend WithEvents centre As System.Windows.Forms.RadioButton
    Friend WithEvents btnMenu As System.Windows.Forms.Button
    Friend WithEvents digraph As System.Windows.Forms.CheckBox
    Friend WithEvents seed As System.Windows.Forms.TextBox
    Friend WithEvents lblSeed As System.Windows.Forms.Label
    Friend WithEvents lockseed As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents aspectratio As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
