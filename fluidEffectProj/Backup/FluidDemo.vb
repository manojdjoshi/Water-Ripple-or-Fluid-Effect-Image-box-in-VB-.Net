Public Class fluidEffectDemo
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
   
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_dropRadius As System.Windows.Forms.Label
    Friend WithEvents lbl_Scale As System.Windows.Forms.Label
    Friend WithEvents lbl_dropHeight As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Dampening As System.Windows.Forms.Label
    Friend WithEvents bar_Scale As System.Windows.Forms.TrackBar
    Friend WithEvents bar_Radius As System.Windows.Forms.TrackBar
    Friend WithEvents bar_dropHeight As System.Windows.Forms.TrackBar
    Friend WithEvents bar_Dampening As System.Windows.Forms.TrackBar
    Friend WithEvents btn_Freeze As System.Windows.Forms.Button
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    'Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As fluidEffect.fluidEffectControl

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(fluidEffectDemo))
        Me.Panel1 = New fluidEffect.fluidEffectControl()
        Me.bar_Scale = New System.Windows.Forms.TrackBar()
        Me.bar_Radius = New System.Windows.Forms.TrackBar()
        Me.lbl_dropRadius = New System.Windows.Forms.Label()
        Me.lbl_Scale = New System.Windows.Forms.Label()
        Me.bar_dropHeight = New System.Windows.Forms.TrackBar()
        Me.lbl_dropHeight = New System.Windows.Forms.Label()
        Me.btn_Freeze = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.bar_Dampening = New System.Windows.Forms.TrackBar()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.lbl_Dampening = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        CType(Me.bar_Scale, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bar_Radius, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bar_dropHeight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bar_Dampening, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Dampener = 4
        Me.Panel1.dropHeight = CType(200, Short)
        Me.Panel1.dropRadius = 20
        Me.Panel1.ImageBitmap = CType(resources.GetObject("Panel1.ImageBitmap"), System.Drawing.Bitmap)
        Me.Panel1.Location = New System.Drawing.Point(144, 176)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(450, 150)
        Me.Panel1.StopRipples = False
        Me.Panel1.TabIndex = 0
        Me.Panel1.TheScale = 0
        '
        'bar_Scale
        '
        Me.bar_Scale.LargeChange = 1
        Me.bar_Scale.Location = New System.Drawing.Point(72, 0)
        Me.bar_Scale.Name = "bar_Scale"
        Me.bar_Scale.Size = New System.Drawing.Size(192, 45)
        Me.bar_Scale.TabIndex = 1
        '
        'bar_Radius
        '
        Me.bar_Radius.LargeChange = 10
        Me.bar_Radius.Location = New System.Drawing.Point(104, 48)
        Me.bar_Radius.Maximum = 40
        Me.bar_Radius.Name = "bar_Radius"
        Me.bar_Radius.Size = New System.Drawing.Size(296, 45)
        Me.bar_Radius.TabIndex = 3
        Me.bar_Radius.TickFrequency = 5
        Me.bar_Radius.Value = 10
        '
        'lbl_dropRadius
        '
        Me.lbl_dropRadius.Location = New System.Drawing.Point(400, 56)
        Me.lbl_dropRadius.Name = "lbl_dropRadius"
        Me.lbl_dropRadius.Size = New System.Drawing.Size(40, 24)
        Me.lbl_dropRadius.TabIndex = 5
        Me.lbl_dropRadius.Text = "10"
        '
        'lbl_Scale
        '
        Me.lbl_Scale.Location = New System.Drawing.Point(264, 0)
        Me.lbl_Scale.Name = "lbl_Scale"
        Me.lbl_Scale.Size = New System.Drawing.Size(32, 24)
        Me.lbl_Scale.TabIndex = 6
        Me.lbl_Scale.Text = "0"
        '
        'bar_dropHeight
        '
        Me.bar_dropHeight.LargeChange = 50
        Me.bar_dropHeight.Location = New System.Drawing.Point(128, 96)
        Me.bar_dropHeight.Maximum = 200
        Me.bar_dropHeight.Name = "bar_dropHeight"
        Me.bar_dropHeight.Size = New System.Drawing.Size(312, 45)
        Me.bar_dropHeight.SmallChange = 10
        Me.bar_dropHeight.TabIndex = 7
        Me.bar_dropHeight.TickFrequency = 40
        Me.bar_dropHeight.Value = 50
        '
        'lbl_dropHeight
        '
        Me.lbl_dropHeight.Location = New System.Drawing.Point(440, 104)
        Me.lbl_dropHeight.Name = "lbl_dropHeight"
        Me.lbl_dropHeight.Size = New System.Drawing.Size(40, 23)
        Me.lbl_dropHeight.TabIndex = 9
        Me.lbl_dropHeight.Text = "50"
        '
        'btn_Freeze
        '
        Me.btn_Freeze.Location = New System.Drawing.Point(448, 64)
        Me.btn_Freeze.Name = "btn_Freeze"
        Me.btn_Freeze.TabIndex = 10
        Me.btn_Freeze.Text = "Freeze"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(0, 128)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(136, 264)
        Me.TextBox1.TabIndex = 17
        Me.TextBox1.Text = "* Click on the graphic to place a drop." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "* The sliders work in realtime." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "* The v" & _
        "alue defaults/ranges shown are for demo purposes only. You can pick a larger ran" & _
        "ge in your own app depending on how big your image is." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "* After you click 'Freez" & _
        "e', just click on the image again to place a new drop and to unfreeze all existi" & _
        "ng ripples." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "* See http://www.x-caiver.com/Software for more information." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "* If " & _
        "you have VS.NET 2003 go to the above location to download a modified version."
        '
        'bar_Dampening
        '
        Me.bar_Dampening.LargeChange = 1
        Me.bar_Dampening.Location = New System.Drawing.Point(432, 8)
        Me.bar_Dampening.Maximum = 4
        Me.bar_Dampening.Name = "bar_Dampening"
        Me.bar_Dampening.Size = New System.Drawing.Size(104, 45)
        Me.bar_Dampening.TabIndex = 18
        Me.bar_Dampening.Value = 4
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(304, 0)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(128, 40)
        Me.TextBox2.TabIndex = 19
        Me.TextBox2.Text = "Dampening Power (0-4)" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Default = 4"
        '
        'lbl_Dampening
        '
        Me.lbl_Dampening.Location = New System.Drawing.Point(536, 16)
        Me.lbl_Dampening.Name = "lbl_Dampening"
        Me.lbl_Dampening.Size = New System.Drawing.Size(40, 23)
        Me.lbl_Dampening.TabIndex = 20
        Me.lbl_Dampening.Text = "4"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(0, 48)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(104, 24)
        Me.TextBox3.TabIndex = 21
        Me.TextBox3.Text = "Drop Radius (0-40)"
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(0, 96)
        Me.TextBox4.Multiline = True
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        Me.TextBox4.Size = New System.Drawing.Size(128, 24)
        Me.TextBox4.TabIndex = 22
        Me.TextBox4.Text = "Drop Height (0-200)"
        '
        'TextBox5
        '
        Me.TextBox5.Multiline = True
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ReadOnly = True
        Me.TextBox5.Size = New System.Drawing.Size(72, 24)
        Me.TextBox5.TabIndex = 23
        Me.TextBox5.Text = "Scale (0-10)"
        '
        'fluidEffectDemo
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(614, 396)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.Panel1, Me.TextBox5, Me.TextBox4, Me.TextBox3, Me.lbl_Dampening, Me.TextBox2, Me.bar_Dampening, Me.TextBox1, Me.btn_Freeze, Me.lbl_dropHeight, Me.bar_dropHeight, Me.lbl_Scale, Me.lbl_dropRadius, Me.bar_Radius, Me.bar_Scale})
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "fluidEffectDemo"
        Me.Text = "Fluid Demo App"
        CType(Me.bar_Scale, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bar_Radius, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bar_dropHeight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bar_Dampening, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region



    Private Sub bar_Radius_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bar_Radius.Scroll
        lbl_dropRadius.Text = bar_Radius.Value
        Panel1.dropRadius = bar_Radius.Value
    End Sub

    Private Sub bar_dropHeight_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bar_dropHeight.Scroll
        lbl_dropHeight.Text = bar_dropHeight.Value
        Panel1.dropHeight = bar_dropHeight.Value
    End Sub

    Private Sub bar_Scale_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bar_Scale.Scroll
        lbl_Scale.Text = bar_Scale.Value
        Panel1.TheScale = bar_Scale.Value
    End Sub

    Private Sub bar_Dampening_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bar_Dampening.Scroll
        lbl_Dampening.Text = bar_Dampening.Value
        Panel1.Dampener = bar_Dampening.Value
    End Sub

    Private Sub btn_Freeze_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Freeze.Click
        Panel1.StopRipples = True
    End Sub


End Class
