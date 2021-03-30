Module Is64Bit_
    Private Declare Function GetProcAddress Lib "kernel32" _
        (ByVal hModule As Integer, _
        ByVal lpProcName As String) As Integer

    Private Declare Function GetModuleHandle Lib "kernel32" _
        Alias "GetModuleHandleA" _
        (ByVal lpModuleName As String) As Integer

    Private Declare Function GetCurrentProcess Lib "kernel32" _
        () As Integer

    Private Declare Function IsWow64Process Lib "kernel32" _
        (ByVal hProc As Integer, _
        ByRef bWow64Process As Boolean) As Integer
    Friend Function Is64bit() As Boolean
        Dim handle As Long, answer As Boolean

        If IntPtr.Size = 8 Then ' If pointers are 64-bit, not only is the OS 64-bit, but we are too
            Return True
        End If

        ' Assume initially that this is not a Wow64 process
        answer = False

        ' Now check to see if IsWow64Process function exists
        handle = GetProcAddress(GetModuleHandle("kernel32"), "IsWow64Process")

        If handle > 0 Then ' IsWow64Process function exists
            ' Now use it to determine if 
            ' we are running under Wow64
            IsWow64Process(GetCurrentProcess(), answer)
        End If

        Return answer

    End Function

End Module
