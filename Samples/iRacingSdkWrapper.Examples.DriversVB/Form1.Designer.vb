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
        Me.driversGrid = New System.Windows.Forms.DataGridView()
        Me.statusLabel = New System.Windows.Forms.Label()
        Me.startButton = New System.Windows.Forms.Button()
        CType(Me.driversGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'driversGrid
        '
        Me.driversGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.driversGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.driversGrid.Location = New System.Drawing.Point(13, 48)
        Me.driversGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.driversGrid.Name = "driversGrid"
        Me.driversGrid.ReadOnly = True
        Me.driversGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.driversGrid.Size = New System.Drawing.Size(1138, 526)
        Me.driversGrid.TabIndex = 10
        '
        'statusLabel
        '
        Me.statusLabel.AutoSize = True
        Me.statusLabel.Location = New System.Drawing.Point(146, 19)
        Me.statusLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.statusLabel.Name = "statusLabel"
        Me.statusLabel.Size = New System.Drawing.Size(48, 17)
        Me.statusLabel.TabIndex = 9
        Me.statusLabel.Text = "Status"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(13, 13)
        Me.startButton.Margin = New System.Windows.Forms.Padding(4)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(100, 28)
        Me.startButton.TabIndex = 8
        Me.startButton.Text = "Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1164, 587)
        Me.Controls.Add(Me.driversGrid)
        Me.Controls.Add(Me.statusLabel)
        Me.Controls.Add(Me.startButton)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.driversGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents driversGrid As System.Windows.Forms.DataGridView
    Private WithEvents statusLabel As System.Windows.Forms.Label
    Private WithEvents startButton As System.Windows.Forms.Button

End Class
