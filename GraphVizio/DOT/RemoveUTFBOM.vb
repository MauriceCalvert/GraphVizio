Imports System.IO

Public Module RemoveUTFBOM_

    Public Function RemoveUTFBOM(source As String) As String

        Const BUFSIZE = 32768

        Using stream As FileStream = File.OpenRead(source)

            Dim reader As New BinaryReader(stream)
            Dim bom As Byte() = {0, 0, 0}

            Dim count As Integer = stream.Read(bom, 0, 3)

            If count = 3 AndAlso
                 bom(0) = &HEF AndAlso
                 bom(1) = &HBB AndAlso
                 bom(2) = &HBF Then

                Dim result As String = Path.GetTempFileName

                Using writeStream As FileStream = File.OpenWrite(result)

                    Dim writer As New BinaryWriter(writeStream)

                    ' create a buffer to hold the bytes 
                    Dim buffer As Byte() = New [Byte](BUFSIZE - 1) {}
                    Dim bytesRead As Integer

                    ' while the read method returns bytes
                    ' keep writing them to the output stream
                    Do
                        bytesRead = stream.Read(buffer, 0, BUFSIZE)
                        If bytesRead = 0 Then
                            Exit Do
                        End If
                        writeStream.Write(buffer, 0, bytesRead)
                    Loop

                    writeStream.Close()

                End Using

                stream.Close()
                Return result

            Else
                stream.Close()
                Return source
            End If

        End Using

    End Function

End Module
