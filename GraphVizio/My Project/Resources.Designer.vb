﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("GraphVizio.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to title View or edit available shapes and connectors in the settings
        '''say GraphVizio has its own stencil, stored in
        '''say C:\Document and Settings\[Your name]\Application Data\GraphVizio\GraphVizio.VSS
        '''goto menu
        '''say To edit this template, select
        '''say Graph -&gt; Tools -&gt; Stencil
        '''showmenu 7 2
        '''openstencil
        '''goto settings
        '''say The shapes and connectors in this stencil are those you see in settings
        '''check shapeis	
        '''set shapename square
        '''uncheck shapeis	
        '''check connectoris	
        '''set connectorname dynamic
        '''uncheck conne [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property HowTos() As String
            Get
                Return ResourceManager.GetString("HowTos", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        '''</summary>
        Friend ReadOnly Property Logo32() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("Logo32", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property Pointer() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Pointer", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
        '''&lt;tooltips&gt;
        '''&lt;tooltip&gt;
        '''  &lt;name&gt;algorithm&lt;/name&gt;
        '''  &lt;text&gt;
        '''Determines which program will be used to layout your drawing.
        '''Select an algorithm to see a sample layout.
        '''&lt;/text&gt;
        '''&lt;/tooltip&gt;
        '''  &lt;tooltip&gt;
        '''    &lt;name&gt;dot&lt;/name&gt;
        '''    &lt;text&gt;
        '''The DOT algorithm is used for hierarchical diagrams, such as
        '''organisation charts and flowcharts, where some shapes have
        '''more importance (or come before) others.
        '''
        '''If arrows would make sense in your diagram (even if they are not drawn) [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property tooltips() As String
            Get
                Return ResourceManager.GetString("tooltips", resourceCulture)
            End Get
        End Property
    End Module
End Namespace