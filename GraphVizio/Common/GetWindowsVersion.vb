Imports System.Environment
Module GetWindowsVersion_
    Function GetWindowsVersion() As String
        Dim OpSys As OperatingSystem = Environment.OSVersion
        Dim v As System.Version = OpSys.Version
        Dim answer As String = "?"
        Select Case OpSys.Platform
            Case PlatformID.MacOSX
                answer = "MacOSX"
            Case PlatformID.Unix
                answer = "Unix"
            Case PlatformID.Win32Windows ' 95 or later, Major is always 4
                Select Case v.Minor
                    Case 0
                        answer = "95"
                    Case 10
                        answer = "98"
                    Case Else
                        answer = "ME"
                End Select
            Case PlatformID.Win32NT ' NT or later
                Select Case v.Major
                    Case 4
                        answer = "NT"
                    Case 5
                        Select Case v.Minor
                            Case 0
                                answer = "2000"
                            Case 1
                                answer = "XP"
                            Case Else
                                answer = "2003"
                        End Select
                    Case 6
                        answer = "Vista"
                    Case Else
                        answer = "7"
                End Select
        End Select
        If Is64bit() Then
            answer = answer & " 64"
        End If
        Return answer
    End Function
End Module
