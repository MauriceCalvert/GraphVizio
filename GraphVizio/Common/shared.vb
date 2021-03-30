Imports System.Drawing
Imports System.Threading
Imports System.Collections.Generic
Module Shared_
    Public WithEvents myVisioApp As Visio.Application
    Friend Const MINVISIOVERSION As Integer = 11
    Friend Const TITLE As String = "GraphVizio"
    Friend Const GRAMMARFILE As String = "GraphVizio.egt"
    Friend Const INPUTFILE As String = "GraphVizioIn.txt"
    Friend Const OUTPUTFILE As String = "GraphVizioOut.txt"
    Friend Const ERRORFILE As String = "GraphVizioErr.txt"
    Friend Const STENCILNAME As String = "GraphVizio.vss"
    Friend Const COMMIT As Boolean = True
    Friend Const ROLLBACK As Boolean = False
    Friend Const SETTINGSDIRECTORY As String = "settings\" ' *MUST* have trailing slash
    Friend Const SELECTPROMPT As String = "select..." ' default item in dropdown boxes
    Friend Const ROOTGRAPHNAME As String = " Root" ' Note leading space, in case he has a layer called "RootGraph"

    Friend AllowFading As Boolean = True
    Friend ControlChars() As Char
    Friend PageTable As New Dictionary(Of Integer, Setting)
    Friend DefaultSettings As Setting = New Setting(-1)
    Friend DisplaySettings As Boolean = False
    Friend GraphvizGraphOption As New Dictionary(Of String, Boolean)
    Friend HelpInProgress As Boolean = False
    Friend Options As New Dictionary(Of String, String)
    Friend MsgWin As Messages = Nothing
    Friend ProgressLock As New Object
    Friend ProgressWin As ProgressForm = Nothing
    Friend Quotable() As Char
    Friend ScopeStack As New Stack(Of Integer)
    Friend CurrentSetting As Setting = DefaultSettings
    Friend StencilConnectors As List(Of String) = Nothing
    Friend StencilShapes As List(Of String) = Nothing
    Friend ToolTips As Dictionary(Of String, String) = Nothing
    Friend HowTos As Dictionary(Of String, HowTo) = Nothing
    Friend Warnings As New Dictionary(Of String, Integer)

    Friend GraphvizGraphOptions() As String = {
        "arrowhead",
        "arrowsize",
        "arrowtail",
        "bb",
        "bgcolor",
        "center",
        "charset",
        "clusterrank",
        "color",
        "colorscheme",
        "comment",
        "compound",
        "concentrate",
        "constraint",
        "Damping",
        "decorate",
        "defaultdist",
        "dim",
        "dir",
        "diredgeconstraints",
        "distortion",
        "dpi",
        "edgehref",
        "edgetarget",
        "edgetooltip",
        "edgeURL",
        "epsilon",
        "esep",
        "fillcolor",
        "fixedsize",
        "fontcolor",
        "fontname",
        "fontnames",
        "fontpath",
        "fontsize",
        "group",
        "headclip",
        "headhref",
        "headlabel",
        "headport",
        "headtarget",
        "headtooltip",
        "headURL",
        "height",
        "href",
        "image",
        "imagescale",
        "label",
        "labelangle",
        "labeldistance",
        "labelfloat",
        "labelfontcolor",
        "labelfontname",
        "labelfontsize",
        "labelhref",
        "labeljust",
        "labelloc",
        "labeltarget",
        "labeltooltip",
        "labelURL",
        "landscape",
        "layer",
        "layers",
        "layersep",
        "len",
        "levelsgap",
        "lhead",
        "lp",
        "ltail",
        "margin",
        "maxiter",
        "mclimit",
        "mindist",
        "minlen",
        "mode",
        "model",
        "mosek",
        "nodesep",
        "nojustify",
        "normalize",
        "nslimit",
        "ordering",
        "orientation",
        "outputorder",
        "overlap",
        "pack",
        "packmode",
        "pad",
        "page",
        "pagedir",
        "pencolor",
        "peripheries",
        "pin",
        "pos",
        "quantum",
        "rank",
        "rankdir",
        "ranksep",
        "ratio",
        "rects",
        "regular",
        "remincross",
        "resolution",
        "root",
        "rotate",
        "samehead",
        "sametail",
        "samplepoints",
        "searchsize",
        "sep",
        "shape",
        "shapefile",
        "showboxes",
        "sides",
        "size",
        "skew",
        "splines",
        "start",
        "style",
        "stylesheet",
        "tailclip",
        "tailhref",
        "taillabel",
        "tailport",
        "tailtarget",
        "tailtooltip",
        "tailURL",
        "target",
        "tooltip",
        "truecolor",
        "URL",
        "vertices",
        "viewport",
        "voro_margin",
        "weight",
        "width",
        "z"}
End Module
