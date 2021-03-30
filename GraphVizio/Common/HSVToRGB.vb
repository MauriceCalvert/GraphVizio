Imports System
Imports System.Drawing
Friend Class RGBHSL
    Friend Class HSL
        Friend Sub New()
            _h = 0
            _s = 0
            _l = 0
        End Sub 'New
        Private _h As Double
        Private _s As Double
        Private _l As Double
        Friend Property H() As Double
            Get
                Return _h
            End Get

            Set(ByVal value As Double)
                _h = value
                If _h > 1 Then
                    _h = 1
                ElseIf _h < 0 Then
                    _h = 0
                End If
            End Set
        End Property
        Friend Property S() As Double
            Get
                Return _s
            End Get

            Set(ByVal value As Double)
                _s = value
                If _s > 1 Then
                    _s = 1
                ElseIf _s < 0 Then
                    _s = 0
                End If
            End Set
        End Property
        Friend Property L() As Double
            Get
                Return _l
            End Get

            Set(ByVal value As Double)
                _l = value
                If _l > 1 Then
                    _l = 1
                ElseIf _l < 0 Then
                    _l = 0
                End If
            End Set
        End Property
    End Class 'HSL
    Friend Sub New()

    End Sub 'New
    '/ <summary>

    '/ Sets the absolute brightness of a colour

    '/ </summary>

    '/ <param name="c">Original colour</param>

    '/ <param name="brightness">The luminance level to impose</param>

    '/ <returns>an adjusted colour</returns>

    Friend Shared Function SetBrightness(ByVal c As Color, ByVal brightness As Double) As Color

        Dim hsl As HSL = RGB_to_HSL(c)

        hsl.L = brightness

        Return HSL_to_RGB(hsl)

    End Function 'SetBrightness





    '/ <summary>

    '/ Modifies an existing brightness level

    '/ </summary>

    '/ <remarks>

    '/ To reduce brightness use a number smaller than 1. To increase brightness use a number larger tnan 1

    '/ </remarks>

    '/ <param name="c">The original colour</param>

    '/ <param name="brightness">The luminance delta</param>

    '/ <returns>An adjusted colour</returns>

    Friend Shared Function ModifyBrightness(ByVal c As Color, ByVal brightness As Double) As Color

        Dim hsl As HSL = RGB_to_HSL(c)

        hsl.L *= brightness

        Return HSL_to_RGB(hsl)

    End Function 'ModifyBrightness





    '/ <summary>

    '/ Sets the absolute saturation level

    '/ </summary>

    '/ <remarks>Accepted values 0-1</remarks>

    '/ <param name="c">An original colour</param>

    '/ <param name="Saturation">The saturation value to impose</param>

    '/ <returns>An adjusted colour</returns>

    Friend Shared Function SetSaturation(ByVal c As Color, ByVal Saturation As Double) As Color

        Dim hsl As HSL = RGB_to_HSL(c)

        hsl.S = Saturation

        Return HSL_to_RGB(hsl)

    End Function 'SetSaturation





    '/ <summary>

    '/ Modifies an existing Saturation level

    '/ </summary>

    '/ <remarks>

    '/ To reduce Saturation use a number smaller than 1. To increase Saturation use a number larger tnan 1

    '/ </remarks>

    '/ <param name="c">The original colour</param>

    '/ <param name="Saturation">The saturation delta</param>

    '/ <returns>An adjusted colour</returns>

    Friend Shared Function ModifySaturation(ByVal c As Color, ByVal Saturation As Double) As Color

        Dim hsl As HSL = RGB_to_HSL(c)

        hsl.S *= Saturation

        Return HSL_to_RGB(hsl)

    End Function 'ModifySaturation





    '/ <summary>

    '/ Sets the absolute Hue level

    '/ </summary>

    '/ <remarks>Accepted values 0-1</remarks>

    '/ <param name="c">An original colour</param>

    '/ <param name="Hue">The Hue value to impose</param>

    '/ <returns>An adjusted colour</returns>

    Friend Shared Function SetHue(ByVal c As Color, ByVal Hue As Double) As Color

        Dim hsl As HSL = RGB_to_HSL(c)

        hsl.H = Hue

        Return HSL_to_RGB(hsl)

    End Function 'SetHue





    '/ <summary>

    '/ Modifies an existing Hue level

    '/ </summary>

    '/ <remarks>

    '/ To reduce Hue use a number smaller than 1. To increase Hue use a number larger tnan 1

    '/ </remarks>

    '/ <param name="c">The original colour</param>

    '/ <param name="Hue">The Hue delta</param>

    '/ <returns>An adjusted colour</returns>

    Friend Shared Function ModifyHue(ByVal c As Color, ByVal Hue As Double) As Color

        Dim hsl As HSL = RGB_to_HSL(c)

        hsl.H *= Hue

        Return HSL_to_RGB(hsl)

    End Function 'ModifyHue





    '/ <summary>

    '/ Converts a colour from HSL to RGB

    '/ </summary>

    '/ <remarks>Adapted from the algoritm in Foley and Van-Dam</remarks>

    '/ <param name="hsl">The HSL value</param>

    '/ <returns>A Color structure containing the equivalent RGB values</returns>

    Friend Shared Function HSL_to_RGB(ByVal hsl As HSL) As Color

        Dim r As Double = 0

        Dim g As Double = 0

        Dim b As Double = 0

        Dim temp1, temp2 As Double



        If hsl.L = 0 Then

            r = 0

            g = 0

            b = 0

        Else

            If hsl.S = 0 Then

                r = hsl.L

                g = hsl.L

                b = hsl.L

            Else

                If hsl.L <= 0.5 Then
                    temp2 = hsl.L * (1.0 + hsl.S)
                Else
                    temp2 = hsl.L + hsl.S - hsl.L * hsl.S
                End If
                temp1 = 2.0 * hsl.L - temp2



                Dim t3() As Double = {hsl.H + 1.0 / 3.0, hsl.H, hsl.H - 1.0 / 3.0}

                Dim clr() As Double = {0, 0, 0}

                Dim i As Integer

                For i = 0 To 2

                    If t3(i) < 0 Then

                        t3(i) += 1.0

                    End If

                    If t3(i) > 1 Then

                        t3(i) -= 1.0

                    End If

                    If 6.0 * t3(i) < 1.0 Then

                        clr(i) = temp1 + (temp2 - temp1) * t3(i) * 6.0

                    ElseIf 2.0 * t3(i) < 1.0 Then

                        clr(i) = temp2

                    ElseIf 3.0 * t3(i) < 2.0 Then

                        clr(i) = temp1 + (temp2 - temp1) * (2.0 / 3.0 - t3(i)) * 6.0

                    Else

                        clr(i) = temp1

                    End If

                Next i

                r = clr(0)

                g = clr(1)

                b = clr(2)

            End If

        End If



        Return Color.FromArgb(CInt(255 * r), CInt(255 * g), CInt(255 * b))

    End Function 'HSL_to_RGB







    '

    '/ <summary>

    '/ Converts RGB to HSL

    '/ </summary>

    '/ <remarks>Takes advantage of whats already built in to .NET by using the Color.GetHue, Color.GetSaturation and Color.GetBrightness methods</remarks>

    '/ <param name="c">A Color to convert</param>

    '/ <returns>An HSL value</returns>

    Friend Shared Function RGB_to_HSL(ByVal c As Color) As HSL

        Dim hsl As New HSL()



        hsl.H = c.GetHue() / 360.0 ' we store hue as 0-1 as opposed to 0-360

        hsl.L = c.GetBrightness()

        hsl.S = c.GetSaturation()



        Return hsl

    End Function 'RGB_to_HSL

End Class 'RGBHSL
