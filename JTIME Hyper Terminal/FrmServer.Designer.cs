namespace WinFormsServer
{
    partial class FrmServer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupPort = new System.Windows.Forms.GroupBox();
            this.btnClosePort = new System.Windows.Forms.Button();
            this.btnOpenPort = new System.Windows.Forms.Button();
            this.cmbBaudRate = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbPortNum = new System.Windows.Forms.ComboBox();
            this.btnLogSave = new System.Windows.Forms.Button();
            this.groupAISLog = new System.Windows.Forms.GroupBox();
            this.btnLogSaveStop = new System.Windows.Forms.Button();
            this.txtErrorLog = new System.Windows.Forms.TextBox();
            this.txtAISLog = new System.Windows.Forms.TextBox();
            this.groupPort.SuspendLayout();
            this.groupAISLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPort
            // 
            this.groupPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPort.Controls.Add(this.btnClosePort);
            this.groupPort.Controls.Add(this.btnOpenPort);
            this.groupPort.Controls.Add(this.cmbBaudRate);
            this.groupPort.Controls.Add(this.label4);
            this.groupPort.Controls.Add(this.label3);
            this.groupPort.Controls.Add(this.cmbPortNum);
            this.groupPort.Location = new System.Drawing.Point(12, 14);
            this.groupPort.Name = "groupPort";
            this.groupPort.Size = new System.Drawing.Size(507, 104);
            this.groupPort.TabIndex = 9;
            this.groupPort.TabStop = false;
            this.groupPort.Text = "통신포트";
            // 
            // btnClosePort
            // 
            this.btnClosePort.Location = new System.Drawing.Point(104, 71);
            this.btnClosePort.Name = "btnClosePort";
            this.btnClosePort.Size = new System.Drawing.Size(89, 26);
            this.btnClosePort.TabIndex = 14;
            this.btnClosePort.Text = "Port Close";
            this.btnClosePort.UseVisualStyleBackColor = true;
            this.btnClosePort.Click += new System.EventHandler(this.btnClosePort_Click);
            // 
            // btnOpenPort
            // 
            this.btnOpenPort.Location = new System.Drawing.Point(9, 71);
            this.btnOpenPort.Name = "btnOpenPort";
            this.btnOpenPort.Size = new System.Drawing.Size(89, 26);
            this.btnOpenPort.TabIndex = 13;
            this.btnOpenPort.Text = "Port Open";
            this.btnOpenPort.UseVisualStyleBackColor = true;
            this.btnOpenPort.Click += new System.EventHandler(this.btnOpenPort_Click);
            // 
            // cmbBaudRate
            // 
            this.cmbBaudRate.BackColor = System.Drawing.SystemColors.Info;
            this.cmbBaudRate.FormattingEnabled = true;
            this.cmbBaudRate.Items.AddRange(new object[] {
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cmbBaudRate.Location = new System.Drawing.Point(78, 45);
            this.cmbBaudRate.Name = "cmbBaudRate";
            this.cmbBaudRate.Size = new System.Drawing.Size(85, 20);
            this.cmbBaudRate.TabIndex = 12;
            this.cmbBaudRate.Text = "38400";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "통신속도";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "포트번호";
            // 
            // cmbPortNum
            // 
            this.cmbPortNum.BackColor = System.Drawing.SystemColors.Info;
            this.cmbPortNum.FormattingEnabled = true;
            this.cmbPortNum.Items.AddRange(new object[] {
            "COM0",
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10"});
            this.cmbPortNum.Location = new System.Drawing.Point(78, 17);
            this.cmbPortNum.Name = "cmbPortNum";
            this.cmbPortNum.Size = new System.Drawing.Size(85, 20);
            this.cmbPortNum.TabIndex = 9;
            this.cmbPortNum.Text = "COM8";
            // 
            // btnLogSave
            // 
            this.btnLogSave.Location = new System.Drawing.Point(9, 20);
            this.btnLogSave.Name = "btnLogSave";
            this.btnLogSave.Size = new System.Drawing.Size(89, 26);
            this.btnLogSave.TabIndex = 14;
            this.btnLogSave.Text = "로그 저장";
            this.btnLogSave.UseVisualStyleBackColor = true;
            this.btnLogSave.Click += new System.EventHandler(this.btnLogSave_Click);
            // 
            // groupAISLog
            // 
            this.groupAISLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupAISLog.Controls.Add(this.btnLogSaveStop);
            this.groupAISLog.Controls.Add(this.btnLogSave);
            this.groupAISLog.Controls.Add(this.txtErrorLog);
            this.groupAISLog.Controls.Add(this.txtAISLog);
            this.groupAISLog.Location = new System.Drawing.Point(12, 124);
            this.groupAISLog.Name = "groupAISLog";
            this.groupAISLog.Size = new System.Drawing.Size(507, 528);
            this.groupAISLog.TabIndex = 6;
            this.groupAISLog.TabStop = false;
            this.groupAISLog.Text = "AIS Log";
            // 
            // btnLogSaveStop
            // 
            this.btnLogSaveStop.Location = new System.Drawing.Point(104, 20);
            this.btnLogSaveStop.Name = "btnLogSaveStop";
            this.btnLogSaveStop.Size = new System.Drawing.Size(89, 26);
            this.btnLogSaveStop.TabIndex = 14;
            this.btnLogSaveStop.Text = "저장 스톱";
            this.btnLogSaveStop.UseVisualStyleBackColor = true;
            this.btnLogSaveStop.Click += new System.EventHandler(this.btnLogSaveStop_Click);
            // 
            // txtErrorLog
            // 
            this.txtErrorLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtErrorLog.BackColor = System.Drawing.Color.Honeydew;
            this.txtErrorLog.Location = new System.Drawing.Point(9, 354);
            this.txtErrorLog.Multiline = true;
            this.txtErrorLog.Name = "txtErrorLog";
            this.txtErrorLog.ReadOnly = true;
            this.txtErrorLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtErrorLog.Size = new System.Drawing.Size(488, 168);
            this.txtErrorLog.TabIndex = 6;
            // 
            // txtAISLog
            // 
            this.txtAISLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAISLog.BackColor = System.Drawing.Color.Honeydew;
            this.txtAISLog.Location = new System.Drawing.Point(9, 52);
            this.txtAISLog.Multiline = true;
            this.txtAISLog.Name = "txtAISLog";
            this.txtAISLog.ReadOnly = true;
            this.txtAISLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAISLog.Size = new System.Drawing.Size(488, 296);
            this.txtAISLog.TabIndex = 4;
            // 
            // FrmServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(531, 664);
            this.Controls.Add(this.groupAISLog);
            this.Controls.Add(this.groupPort);
            this.Name = "FrmServer";
            this.Text = "JMarineTime_AIS_Receiver";
            this.groupPort.ResumeLayout(false);
            this.groupPort.PerformLayout();
            this.groupAISLog.ResumeLayout(false);
            this.groupAISLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupPort;
        private System.Windows.Forms.ComboBox cmbBaudRate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbPortNum;

        private System.Windows.Forms.Button btnOpenPort;
        private System.Windows.Forms.Button btnClosePort;
        
        private System.Windows.Forms.GroupBox groupAISLog;
        private System.Windows.Forms.TextBox txtAISLog;
        private System.Windows.Forms.TextBox txtErrorLog;

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnLogSave;
        private System.Windows.Forms.Button btnLogSaveStop;
    }
}

