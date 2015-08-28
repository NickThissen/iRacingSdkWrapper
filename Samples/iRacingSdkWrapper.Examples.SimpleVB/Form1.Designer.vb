<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.statusLabel = New System.Windows.Forms.Label()
        Me.telemetryLabel = New System.Windows.Forms.Label()
        Me.sessionInfoLabel = New System.Windows.Forms.Label()
        Me.splitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.sessionInfoTextBox = New System.Windows.Forms.TextBox()
        Me.telemetryTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        CType(Me.splitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainer1.Panel1.SuspendLayout()
        Me.splitContainer1.Panel2.SuspendLayout()
        Me.splitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'statusLabel
        '
        Me.statusLabel.AutoSize = True
        Me.statusLabel.Location = New System.Drawing.Point(112, 17)
        Me.statusLabel.Name = "statusLabel"
        Me.statusLabel.Size = New System.Drawing.Size(37, 13)
        Me.statusLabel.TabIndex = 7
        Me.statusLabel.Text = "Status"
        '
        'telemetryLabel
        '
        Me.telemetryLabel.AutoSize = True
        Me.telemetryLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.telemetryLabel.Location = New System.Drawing.Point(0, 0)
        Me.telemetryLabel.Name = "telemetryLabel"
        Me.telemetryLabel.Size = New System.Drawing.Size(53, 13)
        Me.telemetryLabel.TabIndex = 3
        Me.telemetryLabel.Text = "Telemetry"
        '
        'sessionInfoLabel
        '
        Me.sessionInfoLabel.AutoSize = True
        Me.sessionInfoLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.sessionInfoLabel.Location = New System.Drawing.Point(0, 0)
        Me.sessionInfoLabel.Name = "sessionInfoLabel"
        Me.sessionInfoLabel.Size = New System.Drawing.Size(64, 13)
        Me.sessionInfoLabel.TabIndex = 1
        Me.sessionInfoLabel.Text = "Session info"
        '
        'splitContainer1
        '
        Me.splitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.splitContainer1.Location = New System.Drawing.Point(12, 41)
        Me.splitContainer1.Name = "splitContainer1"
        '
        'splitContainer1.Panel1
        '
        Me.splitContainer1.Panel1.Controls.Add(Me.sessionInfoTextBox)
        Me.splitContainer1.Panel1.Controls.Add(Me.sessionInfoLabel)
        '
        'splitContainer1.Panel2
        '
        Me.splitContainer1.Panel2.Controls.Add(Me.telemetryTextBox)
        Me.splitContainer1.Panel2.Controls.Add(Me.telemetryLabel)
        Me.splitContainer1.Size = New System.Drawing.Size(775, 376)
        Me.splitContainer1.SplitterDistance = 382
        Me.splitContainer1.TabIndex = 6
        '
        'sessionInfoTextBox
        '
        Me.sessionInfoTextBox.BackColor = System.Drawing.SystemColors.Control
        Me.sessionInfoTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.sessionInfoTextBox.Location = New System.Drawing.Point(0, 13)
        Me.sessionInfoTextBox.Multiline = True
        Me.sessionInfoTextBox.Name = "sessionInfoTextBox"
        Me.sessionInfoTextBox.ReadOnly = True
        Me.sessionInfoTextBox.Size = New System.Drawing.Size(382, 363)
        Me.sessionInfoTextBox.TabIndex = 0
        '
        'telemetryTextBox
        '
        Me.telemetryTextBox.BackColor = System.Drawing.SystemColors.Control
        Me.telemetryTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.telemetryTextBox.Location = New System.Drawing.Point(0, 13)
        Me.telemetryTextBox.Multiline = True
        Me.telemetryTextBox.Name = "telemetryTextBox"
        Me.telemetryTextBox.ReadOnly = True
        Me.telemetryTextBox.Size = New System.Drawing.Size(389, 363)
        Me.telemetryTextBox.TabIndex = 2
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(12, 12)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 5
        Me.startButton.Text = "Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(799, 429)
        Me.Controls.Add(Me.statusLabel)
        Me.Controls.Add(Me.splitContainer1)
        Me.Controls.Add(Me.startButton)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.splitContainer1.Panel1.ResumeLayout(False)
        Me.splitContainer1.Panel1.PerformLayout()
        Me.splitContainer1.Panel2.ResumeLayout(False)
        Me.splitContainer1.Panel2.PerformLayout()
        CType(Me.splitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents statusLabel As System.Windows.Forms.Label
    Private WithEvents telemetryLabel As System.Windows.Forms.Label
    Private WithEvents sessionInfoLabel As System.Windows.Forms.Label
    Private WithEvents splitContainer1 As System.Windows.Forms.SplitContainer
    Private WithEvents sessionInfoTextBox As System.Windows.Forms.TextBox
    Private WithEvents telemetryTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button

End Class
