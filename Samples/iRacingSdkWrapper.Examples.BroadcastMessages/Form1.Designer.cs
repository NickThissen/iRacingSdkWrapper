namespace iRacingSdkWrapper.Examples.BroadcastMessages
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.numSetPos = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSetPos = new System.Windows.Forms.Button();
            this.btnSetPosEnd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numSetPosEnd = new System.Windows.Forms.NumericUpDown();
            this.cboReplayEvents = new System.Windows.Forms.ComboBox();
            this.btnJump = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSetSpeed = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numSpeed = new System.Windows.Forms.NumericUpDown();
            this.chkSlowmo = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSetPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSetPosEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(760, 389);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkSlowmo);
            this.tabPage1.Controls.Add(this.btnSetSpeed);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.numSpeed);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.btnJump);
            this.tabPage1.Controls.Add(this.cboReplayEvents);
            this.tabPage1.Controls.Add(this.btnSetPosEnd);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.numSetPosEnd);
            this.tabPage1.Controls.Add(this.btnSetPos);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.numSetPos);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(752, 363);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Replay";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(905, 441);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Camera";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // numSetPos
            // 
            this.numSetPos.Location = new System.Drawing.Point(191, 20);
            this.numSetPos.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numSetPos.Name = "numSetPos";
            this.numSetPos.Size = new System.Drawing.Size(120, 20);
            this.numSetPos.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Set position from beginning frame:";
            // 
            // btnSetPos
            // 
            this.btnSetPos.Location = new System.Drawing.Point(317, 17);
            this.btnSetPos.Name = "btnSetPos";
            this.btnSetPos.Size = new System.Drawing.Size(75, 23);
            this.btnSetPos.TabIndex = 3;
            this.btnSetPos.Text = "Set";
            this.btnSetPos.UseVisualStyleBackColor = true;
            this.btnSetPos.Click += new System.EventHandler(this.btnSetPos_Click);
            // 
            // btnSetPosEnd
            // 
            this.btnSetPosEnd.Location = new System.Drawing.Point(317, 46);
            this.btnSetPosEnd.Name = "btnSetPosEnd";
            this.btnSetPosEnd.Size = new System.Drawing.Size(75, 23);
            this.btnSetPosEnd.TabIndex = 6;
            this.btnSetPosEnd.Text = "Set";
            this.btnSetPosEnd.UseVisualStyleBackColor = true;
            this.btnSetPosEnd.Click += new System.EventHandler(this.btnSetPosEnd_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Set position from end frame:";
            // 
            // numSetPosEnd
            // 
            this.numSetPosEnd.Location = new System.Drawing.Point(191, 49);
            this.numSetPosEnd.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numSetPosEnd.Name = "numSetPosEnd";
            this.numSetPosEnd.Size = new System.Drawing.Size(120, 20);
            this.numSetPosEnd.TabIndex = 4;
            // 
            // cboReplayEvents
            // 
            this.cboReplayEvents.FormattingEnabled = true;
            this.cboReplayEvents.Location = new System.Drawing.Point(191, 77);
            this.cboReplayEvents.Name = "cboReplayEvents";
            this.cboReplayEvents.Size = new System.Drawing.Size(121, 21);
            this.cboReplayEvents.TabIndex = 7;
            // 
            // btnJump
            // 
            this.btnJump.Location = new System.Drawing.Point(317, 75);
            this.btnJump.Name = "btnJump";
            this.btnJump.Size = new System.Drawing.Size(75, 23);
            this.btnJump.TabIndex = 8;
            this.btnJump.Text = "Jump";
            this.btnJump.UseVisualStyleBackColor = true;
            this.btnJump.Click += new System.EventHandler(this.btnJump_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Jump to event:";
            // 
            // btnSetSpeed
            // 
            this.btnSetSpeed.Location = new System.Drawing.Point(317, 104);
            this.btnSetSpeed.Name = "btnSetSpeed";
            this.btnSetSpeed.Size = new System.Drawing.Size(75, 23);
            this.btnSetSpeed.TabIndex = 12;
            this.btnSetSpeed.Text = "Set";
            this.btnSetSpeed.UseVisualStyleBackColor = true;
            this.btnSetSpeed.Click += new System.EventHandler(this.btnSetSpeed_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Set playback speed:";
            // 
            // numSpeed
            // 
            this.numSpeed.Location = new System.Drawing.Point(191, 107);
            this.numSpeed.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numSpeed.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            -2147483648});
            this.numSpeed.Name = "numSpeed";
            this.numSpeed.Size = new System.Drawing.Size(120, 20);
            this.numSpeed.TabIndex = 10;
            // 
            // chkSlowmo
            // 
            this.chkSlowmo.AutoSize = true;
            this.chkSlowmo.Location = new System.Drawing.Point(398, 107);
            this.chkSlowmo.Name = "chkSlowmo";
            this.chkSlowmo.Size = new System.Drawing.Size(80, 17);
            this.chkSlowmo.TabIndex = 13;
            this.chkSlowmo.Text = "Slowmotion";
            this.chkSlowmo.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 401);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSetPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSetPosEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnSetPosEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numSetPosEnd;
        private System.Windows.Forms.Button btnSetPos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numSetPos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnJump;
        private System.Windows.Forms.ComboBox cboReplayEvents;
        private System.Windows.Forms.Button btnSetSpeed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numSpeed;
        private System.Windows.Forms.CheckBox chkSlowmo;
    }
}

