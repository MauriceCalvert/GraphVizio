Imports Visio
Imports vb = Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Collections
Imports System.Globalization
Friend Class Node
    Implements IComparable
    ' FOr dictionary debugging
    'Public Class MyDictionary
    '    Inherits Dictionary(Of String, String)
    '    Default Public Shadows Property Item(ByVal key As String) As String
    '        Get
    '            Return MyBase.Item(key)
    '        End Get
    '        Set(ByVal value As String)
    '            MyBase.Item(key) = value
    '        End Set
    '    End Property
    '    Public Shadows Sub Add(ByVal key As String, ByVal value As String)
    '        MyBase.Add(key, value)
    '    End Sub
    'End Class
    'Friend Attributes As New MyDictionary
    Friend Attributes As New Dictionary(Of String, String)
    Friend Edges As New List(Of Edge)
    Friend Graph As Graph = Nothing
    Friend Height As Double = 0
    Friend ID As String = ""
    Friend Marked As Boolean = False
    Friend NewName As String = ""
    Friend Phantom As Boolean = False ' Node is just to modify layout, never drawn
    Friend Rank As Integer = 0
    Friend Shape As Shape = Nothing
    Friend VisioID As String = ""
    Friend Width As Double = 0
    Friend Xpos As Double = 0
    Friend Ypos As Double = 0
    '
    Private Root_ As Boolean = True
    Private Leaf_ As Boolean = True
    'Friend Property Xpos() As Double
    '    Get
    '        Return myXpos
    '    End Get
    '    Set(ByVal value As Double)
    '        myXpos = value
    '    End Set
    'End Property
    Friend Function CompareTo(ByVal y As Object) As Integer Implements System.IComparable.CompareTo
        Dim ans As Integer
        ans = CompareNode(Me, CType(y, Node))
        Return ans
    End Function
    Public Shared Operator =(ByVal x As Node, ByVal y As Node) As Boolean
        Return CompareNode(x, y) = 0
    End Operator
    Public Shared Operator <>(ByVal x As Node, ByVal y As Node) As Boolean
        Return CompareNode(x, y) <> 0
    End Operator
    Public Shared Operator >(ByVal x As Node, ByVal y As Node) As Boolean
        Return CompareNode(x, y) = 1
    End Operator
    Public Shared Operator <(ByVal x As Node, ByVal y As Node) As Boolean
        Return CompareNode(x, y) = -1
    End Operator
    Public Shared Function CompareNode(ByRef x As Node, ByRef y As Node) As Integer
        ' -1 = Y is greater (y is below or to the right of x)
        '  0 = equal
        '  1 = X is greater (x is below or to the right of y)
        If x Is Nothing Then
            If y Is Nothing Then
                ' If x is Nothing and y is Nothing, they're equal
                Return 0
            Else
                ' If x is Nothing and y is not Nothing, y is greater
                Return -1
            End If
        Else
            ' If x is not Nothing...
            If y Is Nothing Then
                ' ...and y is Nothing, x is greater.
                Return 1
            Else
                ' ...and y is not Nothing, compare their positions
                If x.Ypos < y.Ypos Then
                    Return 1 ' x below y -> x is greater 
                ElseIf x.Ypos > y.Ypos Then
                    Return -1 ' x above y -> y is greater 
                Else
                    If x.Xpos > y.Xpos Then
                        Return 1 ' x to the right of y -> x is greater
                    ElseIf x.Xpos < y.Xpos Then
                        Return -1 ' x to the left of y -> y is greater
                    Else
                        Return 0
                    End If
                End If
            End If
        End If
    End Function
    Friend Sub New()
    End Sub
    Friend Sub New(ByVal newid As String)
        ID = newid
    End Sub
    Sub MoveTo(ByVal X As Double, ByVal Y As Double)
        Dim xs As String = Convert.ToString(X * 72, CultureInfo.InvariantCulture)
        Dim ys As String = Convert.ToString(Y * 72, CultureInfo.InvariantCulture)
        Me.SetAttribute("pos", xs & "," & ys)
    End Sub
    Function Attribute(ByVal Key As String) As String
        Dim s As String = ""
        Attributes.TryGetValue(Key, s)
        Return s
    End Function
    Function InheritedAttribute(ByVal Key As String) As String
        Dim s As String = ""
        If Attributes.TryGetValue(key, s) Then
            Return s
        End If
        Dim pg As Graph = Me.Graph
        Do While pg IsNot Nothing
            If pg.DefaultNodeAttrs.TryGetValue(key, s) Then
                Return s
            End If
            pg = pg.Parent
        Loop
        Return ""
    End Function
    Function Label() As String
        If Attributes.ContainsKey("label") Then
            Return Attributes("label")
        Else
            Return "(" & ID & ")"
        End If
    End Function
    Public Sub SetAttribute(ByVal key As String, ByVal value As String)
        If Attributes.ContainsKey(key) Then
            Attributes.Item(key) = value
        Else
            Attributes.Add(key, value)
        End If
        Select Case key
            Case "pos"
                If value <> "" Then
                    value = CleanupPos(value)
                    Try
                        Dim xy As String() = value.Split(New [Char]() {","c})
                        Xpos = Convert.ToDouble(xy(0), CultureInfo.InvariantCulture) / 72
                        Ypos = Convert.ToDouble(xy(1), CultureInfo.InvariantCulture) / 72
                    Catch
                    End Try
                End If
            Case "width"
                Me.Width = Convert.ToDouble(value, CultureInfo.InvariantCulture)
            Case "height"
                Me.Height = Convert.ToDouble(value, CultureInfo.InvariantCulture)
        End Select
    End Sub
    Property Leaf() As Boolean
        Get
            Return Leaf_
        End Get
        Set(ByVal value As Boolean)
            Leaf_ = value
        End Set
    End Property
    Property Root() As Boolean
        Get
            Return Root_
        End Get
        Set(ByVal value As Boolean)
            Root_ = value
        End Set
    End Property
End Class
