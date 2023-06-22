Imports System
Imports System.Collections
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices


Namespace fluidEffect


    Public Class FluidEffectControl

        'An example app, and additional documentation are available
        'at http://www.x-caiver.com/Software
        'This class was ported and enhanced by Don Pattee
        'based on a C# version by Christian Tratz

        Inherits System.Windows.Forms.Panel
        Private WithEvents effectTimer As System.Windows.Forms.Timer
        Private components As System.ComponentModel.IContainer

#Region "Variables - including information on what they do"
        'ImageBitmap, The Scale, Drop Height,  Drop Radius
        'the Dampener value and StopRipples are all exposed in the
        'Property page for this control in VS.NET

        'this only works with bitmaps for now
        'though in the dev environment you can assign
        'any type of image to the control.
        'use ImageBitmap to change the pictures
        Private _bmp As Bitmap
        Private _waves As Short(,,)
        Private _waveWidth As Integer
        Private _waveHeight As Integer
        Private _activeBuffer As Integer = 0
        Private _weHaveWaves As Boolean
        Private _bmpHeight, _bmpWidth As Integer
        Private _bmpBytes As Byte()
        Private _bmpBitmapData As BitmapData
        Private isMouseOver As Boolean = False
        'use TheScale to get/set this value
        '_theScale can be changed when you have a big image
        'or have scaled an image. a value of 1 is a wave
        'bitmap of half the resolution of the original image
        'which is fine for most uses
        'larger values help when the image gets big
        'setting a larger value with a small graphic
        'lets you shake the entire image since the ripple
        'is bigger than the image
        'a value of zero should be a map of the same resolution
        'as the original image but it yields bad results in the 4th
        'quadrant of your image if you have scaled the image box
        Private _theScale As Integer

        'use dropRadius to get/set this value
        '_dropRadius defines the ripple radius
        'you can change this value to change the
        'look and dynamics of the fluid
        'pick smaller values for small images
        Private _dropRadius As Integer = 20

        'use dropHeight to get/set this value
        '_dropHeight defines the height from which the
        'initial drop starts. Change this to adjust the initial
        'intensity of the ripples
        Private _dropHeight As Short = 200

        'use dampener to get/set this value
        '_dampener controls the consistency of the fluid
        'value of 4 ripples a bunch, value of 1 ripples very briefly
        'and a value of 0 makes no ripple
        'if you want to stop the effect, you can dampen down to 1
        'and the existing waves will collapse quickly
        Private _dampener As Integer = 4

        'use StopRipples to trigger this or get the status
        '_stopRipples this doesn't make the surface stop rippling
        'it actually stops the ripples. i.e. it 'freezes' the image
        'in its current form, including existing ripples
        'the next drop (mousedown, etc) will unfreeze the ripples
        Private _stopRipples As Boolean = False
        Private _waveColor As Color
        Private _backgroundImage As Image
        Private _waveOpacity As Single
        Private _interactionEnabled As Boolean
        Private _wavePropagationSpeed As Double
        Private _waveAmplitude As Double

#End Region

#Region "The Subroutines - You don't need to fiddle with these. Play with the Events section instead"


        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Me.effectTimer = New System.Windows.Forms.Timer(Me.components)
        End Sub

        Public Sub New()
            InitializeComponent()
            effectTimer = New System.Windows.Forms.Timer()
            effectTimer.Enabled = True
            effectTimer.Interval = 50
            SetStyle(ControlStyles.UserPaint, True)
            SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            SetStyle(ControlStyles.DoubleBuffer, True)
            BackColor = Color.White
            _weHaveWaves = False
        End Sub

        Public Function FluidEffectControl(ByVal bmp As Bitmap) As FluidEffectControl
            Me.ImageBitmap = bmp
            Return Me
        End Function


        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If

            _bmp.UnlockBits(_bmpBitmapData)
            _bmp.Dispose()

            MyBase.Dispose(disposing)
        End Sub


        Private Sub effectTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles effectTimer.Tick
            If Not (_stopRipples) AndAlso _weHaveWaves Then
                Invalidate()
                ProcessWaves()
            End If
        End Sub


        Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
            Dim tmp As Bitmap = _bmp.Clone()
            Dim x, y As Integer
            Dim xOffset, yOffset As Integer
            Dim alpha As Byte
            Dim waveX, waveY As Integer

            If (_weHaveWaves) Then
                Dim tmpData As BitmapData = tmp.LockBits(New Rectangle(0, 0, _bmpWidth, _bmpHeight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb)
                Dim tmpBytes(_bmpWidth * _bmpHeight * 4) As Byte
                Marshal.Copy(tmpData.Scan0, tmpBytes, 0, _bmpWidth * _bmpHeight * 4)

                x = 1
                Do While (x < (_bmpWidth - 1))
                    y = 1
                    Do While (y < (_bmpHeight - 1))

                        If _theScale = 0 Then
                            waveX = x
                            waveY = y
                        Else
                            waveX = BSRight(CInt(x), _theScale)
                            waveY = BSRight(CInt(y), _theScale)
                        End If

                        'check bounds
                        If (waveX <= 0) Then
                            waveX = 1
                        ElseIf (waveX >= _waveWidth - 1) Then
                            waveX = _waveWidth - 2
                        End If

                        If (waveY <= 0) Then
                            waveY = 1
                        ElseIf (waveY >= _waveHeight - 1) Then
                            waveY = _waveHeight - 2
                        End If

                        'this gives us the effect of fluid breaking the light
                        xOffset = BSRight((_waves(waveX - 1, waveY, _activeBuffer) - _waves(waveX + 1, waveY, _activeBuffer)), 3)
                        yOffset = BSRight((_waves(waveX, waveY - 1, _activeBuffer) - _waves(waveX, waveY + 1, _activeBuffer)), 3)

                        If ((Not (xOffset = 0)) Or (Not (yOffset = 0))) Then

                            'check bounds
                            If (x + xOffset >= _bmpWidth - 1) Then xOffset = _bmpWidth - x - 1
                            If (x + xOffset < 0) Then xOffset = -x

                            If (y + yOffset >= _bmpHeight - 1) Then yOffset = _bmpHeight - y - 1
                            If (y + yOffset < 0) Then yOffset = -y

                            'generate alpha
                            Dim newAlpha As Integer = 200 - xOffset
                            alpha = If(newAlpha < 0, 0, If(newAlpha > 255, 255, CByte(newAlpha)))

                            'set colors
                            tmpBytes(4 * (x + y * _bmpWidth)) = _bmpBytes(4 * (x + xOffset + (y + yOffset) * _bmpWidth))
                            tmpBytes(4 * (x + y * _bmpWidth) + 1) = _bmpBytes(4 * (x + xOffset + (y + yOffset) * _bmpWidth) + 1)
                            tmpBytes(4 * (x + y * _bmpWidth) + 2) = _bmpBytes(4 * (x + xOffset + (y + yOffset) * _bmpWidth) + 2)
                            tmpBytes(4 * (x + y * _bmpWidth) + 3) = alpha

                        End If
                        y += 1
                    Loop 'y
                    x += 1
                Loop 'x

                'copy data back
                Marshal.Copy(tmpBytes, 0, tmpData.Scan0, _bmpWidth * _bmpHeight * 4)
                tmp.UnlockBits(tmpData)

            End If

            e.Graphics.DrawImage(tmp, 0, 0, ClientRectangle.Width, ClientRectangle.Height)


        End Sub

        Private Sub ProcessWaves()
            ' process the waves & simulates the behaviour of fluid
            'VB.NET isn't a math powerhouse, sorry. This
            'bogs down on larger images. I could only get about 
            '1 fps on a 500x400 image. Small images work great.
            'the app i ported this for has a 100x90 image that
            'ripples in realtime.
            Dim wavesFound As Boolean = False
            Dim x, y As Integer
            Dim newBuffer As Integer

            If (_activeBuffer = 0) Then
                newBuffer = 1
            Else
                newBuffer = 0
            End If
            x = 1
            Do While (x < (_waveWidth - 1))
                y = 1
                Do While (y < (_waveHeight - 1))
                    Try
                        _waves(x, y, newBuffer) = CShort((BSRight(_waves(x - 1, y - 1, _activeBuffer) +
                                           _waves(x, y - 1, _activeBuffer) +
                                           _waves(x + 1, y - 1, _activeBuffer) +
                                           _waves(x - 1, y, _activeBuffer) +
                                           _waves(x + 1, y, _activeBuffer) +
                                           _waves(x - 1, y + 1, _activeBuffer) +
                                           _waves(x, y + 1, _activeBuffer) +
                                           _waves(x + 1, y + 1, _activeBuffer), 2)) - _waves(x, y, newBuffer))

                    Catch ex As Exception
                        _waves(x, y, newBuffer) = 0
                    End Try

                    'damping
                    If Not (_waves(x, y, newBuffer) = 0) Then
                        _waves(x, y, newBuffer) -= CShort(BSRight(_waves(x, y, newBuffer), _dampener))

                        wavesFound = True
                    End If
                    y += 1
                Loop 'y++
                x += 1
            Loop 'x++
            _weHaveWaves = wavesFound
            _activeBuffer = newBuffer

        End Sub

        Private Sub PutDrop(ByVal x As Integer, ByVal y As Integer, ByVal height As Short)
            'start a wave by simulating a round drop
            'x,y are the position of the drop
            'height is the height of the first drop
            'the event handles can use the _dropHeight variable
            'or can use that as a starting point, and call this function
            'with different values to generate different effects
            _weHaveWaves = True
            Dim radius As Integer = _dropRadius
            Dim dist As Double
            Dim i As Integer = -radius
            Dim tmpX, tmpY As Integer

            Do While (i <= radius)
                Dim j As Integer = -radius
                Do While (j <= radius)
                    tmpX = x + i
                    tmpY = y + j

                    If (((tmpX >= 0) And (tmpX < _waveWidth - 1)) And ((tmpY >= 0) And (tmpY < _waveHeight - 1))) Then
                        dist = Math.Sqrt(i * i + j * j)
                        If (dist < radius) Then
                            _waves(x + i, y + j, _activeBuffer) = CShort(Math.Cos(dist * Math.PI / radius) * height)
                        End If
                    End If
                    j += 1
                Loop 'j
                i += 1
            Loop 'i
        End Sub
#End Region

#Region "Events - MouseDown by default, add extra events here"
        'There is a mousedown event here. 
        'You can duplicate the code and make a mousedrag event easily 
        'if you want to be able to "draw" in the fluid but VB.NET bogs down under
        'the load rather quickly.
        Private Sub fluidEffectControl_MouseDown(ByVal send As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown
            If (e.Button = MouseButtons.Left) Then
                'this helps align the coordinates correctly
                'if you were to just pass e.X and e.Y to the PutDrop you would see
                'a drop out of line with the mouse cursor relative
                'to the scaling of the bitmap
                Dim realX As Integer = CInt((e.X / CDbl(Me.ClientRectangle.Width)) * _waveWidth)
                Dim realY As Integer = CInt((e.Y / CDbl(Me.ClientRectangle.Height)) * _waveHeight)

                _stopRipples = False
                PutDrop(realX, realY, _dropHeight)


            End If
        End Sub
        Private Sub fluidEffectControl_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
            If isMouseOver Then
                Dim realX As Integer = CInt((e.X / CDbl(Me.ClientRectangle.Width)) * _waveWidth)
                Dim realY As Integer = CInt((e.Y / CDbl(Me.ClientRectangle.Height)) * _waveHeight)
                PutDrop(realX, realY, _dropHeight)
            End If
        End Sub

        Private Sub fluidEffectControl_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.MouseEnter
            isMouseOver = True
            Me.BackColor = Color.LightGray
        End Sub

        Private Sub fluidEffectControl_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.MouseLeave
            isMouseOver = False
            Me.BackColor = Color.White
        End Sub
#End Region

#Region "Property interfaces"
        'see the Variables section for information on what these do
        Public Property ImageBitmap() As Bitmap

            Get
                Return _bmp
            End Get
            Set(ByVal Value As Bitmap)
                _bmp = Value
                _bmpHeight = _bmp.Height
                _bmpWidth = _bmp.Width
                _waveWidth = BSRight(_bmpWidth, _theScale)
                _waveHeight = BSRight(_bmpHeight, _theScale)
                Dim _wavesTmp(_waveWidth, _waveHeight, 2) As Short
                Me._waves = _wavesTmp
                Dim _bmpBytesTmp(_bmpWidth * _bmpHeight * 4) As Byte
                Me._bmpBytes = _bmpBytesTmp
                _bmpBitmapData = _bmp.LockBits(New Rectangle(0, 0, _bmpWidth, _bmpHeight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb)
                Marshal.Copy(_bmpBitmapData.Scan0, _bmpBytes, 0, _bmpWidth * _bmpHeight * 4)
            End Set

        End Property

        Public Property StopRipples() As Boolean
            Get
                Return _stopRipples
            End Get
            Set(ByVal Value As Boolean)
                _stopRipples = Value
            End Set
        End Property

        Public Property Dampener() As Integer
            Get
                Return _dampener
            End Get
            Set(ByVal Value As Integer)
                _dampener = Value
            End Set
        End Property

        Public Property TheScale() As Integer

            Get
                Return _theScale
            End Get
            Set(ByVal Value As Integer)
                _theScale = Value
            End Set
        End Property

        Public Property dropHeight() As Short
            Get
                Return _dropHeight
            End Get
            Set(ByVal Value As Short)
                _dropHeight = Value
            End Set
        End Property

        Public Property dropRadius() As Integer
            Get
                Return _dropRadius
            End Get
            Set(ByVal Value As Integer)
                _dropRadius = Value
            End Set
        End Property
        Public Property WaveColor() As Color
            Get
                Return _waveColor
            End Get
            Set(ByVal value As Color)
                _waveColor = value
            End Set
        End Property

        Public Property InteractionEnabled() As Boolean
            Get
                Return _interactionEnabled
            End Get
            Set(ByVal value As Boolean)
                _interactionEnabled = value
            End Set
        End Property
        Public Property WavePropagationSpeed() As Double
            Get
                Return _wavePropagationSpeed
            End Get
            Set(ByVal value As Double)
                _wavePropagationSpeed = value
            End Set
        End Property
        Public Property WaveAmplitude() As Double
            Get
                Return _waveAmplitude
            End Get
            Set(ByVal value As Double)
                _waveAmplitude = value
            End Set
        End Property




#End Region
        'This function will clear all the ripples and restore the image to its original state.
        Public Sub ClearRipples()
            Array.Clear(_waves, 0, _waves.Length)
            _weHaveWaves = False
            Invalidate()
        End Sub

        'This function allows you to dynamically change the drop radius of the ripples.
        Public Sub SetDropRadius(ByVal radius As Integer)
            _dropRadius = radius
        End Sub

        'This function enables you to set the height of the initial drop for the ripples.
        Public Sub SetDropHeight(ByVal height As Short)
            _dropHeight = height
        End Sub

        'This function allows you to adjust the dampening effect of the ripples.
        Public Sub SetDampener(ByVal dampener As Integer)
            _dampener = dampener
        End Sub

        'This function stops the ripples from propagating further, but keeps the existing waves frozen on the image.
        Public Sub SetStopRipples()
            _stopRipples = True
        End Sub

        'This function resumes the propagation of ripples after they have been stopped.
        Public Sub ResumeRipples()
            _stopRipples = False
        End Sub

        'SetWaveColor: This function allows you to change the color of the waves created by the ripples.
        Public Sub SetWaveColor(ByVal color As Color)
            _waveColor = color
        End Sub


        'SetBackgroundImage: This function enables you to set a new background image for the control.
        Public Sub SetBackgroundImage(ByVal image As Image)
            _backgroundImage = image
            'Invalidate()
        End Sub

        'SetWaveOpacity: This function allows you to adjust the opacity of the waves created by the ripples.
        Public Sub SetWaveOpacity(ByVal opacity As Integer)
            _waveOpacity = opacity
        End Sub

        'ToggleInteraction: This function toggles the interaction mode of the control. When interaction is enabled, users can create ripples by clicking on the control.
        Public Sub ToggleInteraction()
            _interactionEnabled = Not _interactionEnabled
        End Sub

        'SetWavePropagationSpeed: This Function allows() you To control the speed at which the waves propagate across the image.
        Public Sub SetWavePropagationSpeed(ByVal speed As Integer)
            _wavePropagationSpeed = speed
        End Sub

        'SetWaveAmplitude: This function enables you to adjust the amplitude of the waves created by the ripples.
        Public Sub SetWaveAmplitude(ByVal amplitude As Integer)
            _waveAmplitude = amplitude
        End Sub

        'SetWaveOpacity(opacity As Single): This function can be used to set the opacity of the waves in the fluid effect control. It updates the _waveOpacity variable with the specified opacity value.
        Public Sub SetWaveOpacity(ByVal opacity As Single)
            _waveOpacity = opacity
        End Sub

        'ToggleRipples(): This function can be used to toggle the ripple effect on or off. It changes the value of the _stopRipples variable to freeze or unfreeze the ripples.
        Public Sub ToggleRipples()
            _stopRipples = Not _stopRipples
        End Sub

        'ClearWaves(): This function can be used to clear all the waves in the fluid effect control. It sets all the wave values to zero, effectively removing any ripples or disturbances.
        Public Sub ClearWaves()
            For x As Integer = 0 To _waveWidth - 1
                For y As Integer = 0 To _waveHeight - 1
                    _waves(x, y, 0) = 0
                    _waves(x, y, 1) = 0
                Next y
            Next x
            _weHaveWaves = False
        End Sub

        Public Sub Reset()
            _waves = New Short(_waveWidth, _waveHeight, 2) {}
            _weHaveWaves = False
            Refresh()
        End Sub
        Public Sub SetImage(ByVal bmp As Bitmap)
            ImageBitmap = bmp
            _bmpWidth = bmp.Width
            _bmpHeight = bmp.Height
            Reset()
        End Sub

#Region "Bit Shift Functions"
        'VS.NET 2002 / v7 version of VB.NET does not have bitshift operators built in. 
        'The following function mimics the C# >> operator.
        'If you have VS.NET 2003 it does have the operator so there is a version 
        'of this class available at http://www.x-caiver.com/Software that uses the built
        'in >> for slightly better performance.
        Public Function BSRight(ByVal Value As Long, ByVal Count As Integer) As Long
            Dim i As Integer
            BSRight = Value
            For i = 1 To Count
                BSRight = BSRight \ 2
            Next
        End Function


#End Region

    End Class


End Namespace
