namespace AutoCheckBox
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnStart = new Button();
            btnStop = new Button();
            lblStatus = new Label();
            txtLog = new TextBox();
            grpAnswer = new GroupBox();
            rbRandom = new RadioButton();
            rbD = new RadioButton();
            rbC = new RadioButton();
            rbB = new RadioButton();
            rbA = new RadioButton();
            grpSettings = new GroupBox();
            chkRandomDelay = new CheckBox();
            numDelayMax = new NumericUpDown();
            lblDelayMax = new Label();
            numDelayMin = new NumericUpDown();
            lblDelayMin = new Label();
            numScrollY = new NumericUpDown();
            lblScrollY = new Label();
            chkAutoScroll = new CheckBox();
            numMaxQuestions = new NumericUpDown();
            lblMaxQ = new Label();
            chkAutoNext = new CheckBox();
            grpAnswer.SuspendLayout();
            grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDelayMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDelayMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numScrollY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxQuestions).BeginInit();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.FromArgb(40, 167, 69);
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnStart.ForeColor = Color.White;
            btnStart.Location = new Point(14, 293);
            btnStart.Margin = new Padding(3, 4, 3, 4);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(211, 56);
            btnStart.TabIndex = 0;
            btnStart.Text = "▶ Bắt đầu (F3)";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.FromArgb(220, 53, 69);
            btnStop.Enabled = false;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnStop.ForeColor = Color.White;
            btnStop.Location = new Point(232, 293);
            btnStop.Margin = new Padding(3, 4, 3, 4);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(211, 56);
            btnStop.TabIndex = 1;
            btnStop.Text = "⏹ Dừng lại (F4)";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStatus.ForeColor = Color.DarkBlue;
            lblStatus.Location = new Point(14, 360);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(359, 23);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Trạng thái: Sẵn sàng (Bấm F3 hoặc Bắt đầu)";
            // 
            // txtLog
            // 
            txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtLog.BackColor = Color.Black;
            txtLog.Font = new Font("Consolas", 9.5F);
            txtLog.ForeColor = Color.LimeGreen;
            txtLog.Location = new Point(14, 393);
            txtLog.Margin = new Padding(3, 4, 3, 4);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(639, 252);
            txtLog.TabIndex = 3;
            // 
            // grpAnswer
            // 
            grpAnswer.Controls.Add(rbRandom);
            grpAnswer.Controls.Add(rbD);
            grpAnswer.Controls.Add(rbC);
            grpAnswer.Controls.Add(rbB);
            grpAnswer.Controls.Add(rbA);
            grpAnswer.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpAnswer.Location = new Point(14, 16);
            grpAnswer.Margin = new Padding(3, 4, 3, 4);
            grpAnswer.Name = "grpAnswer";
            grpAnswer.Padding = new Padding(3, 4, 3, 4);
            grpAnswer.Size = new Size(640, 80);
            grpAnswer.TabIndex = 4;
            grpAnswer.TabStop = false;
            grpAnswer.Text = "🎯 Chọn Đáp Án Trắc Nghiệm Ưu Tiên";
            // 
            // rbRandom
            // 
            rbRandom.AutoSize = true;
            rbRandom.Checked = true;
            rbRandom.Font = new Font("Segoe UI", 9.5F);
            rbRandom.Location = new Point(480, 33);
            rbRandom.Margin = new Padding(3, 4, 3, 4);
            rbRandom.Name = "rbRandom";
            rbRandom.Size = new Size(136, 25);
            rbRandom.TabIndex = 4;
            rbRandom.TabStop = true;
            rbRandom.Text = "🎲 Ngẫu nhiên";
            rbRandom.UseVisualStyleBackColor = true;
            // 
            // rbD
            // 
            rbD.AutoSize = true;
            rbD.Font = new Font("Segoe UI", 9.5F);
            rbD.Location = new Point(366, 33);
            rbD.Margin = new Padding(3, 4, 3, 4);
            rbD.Name = "rbD";
            rbD.Size = new Size(95, 25);
            rbD.TabIndex = 3;
            rbD.Text = "Đáp án D";
            rbD.UseVisualStyleBackColor = true;
            // 
            // rbC
            // 
            rbC.AutoSize = true;
            rbC.Font = new Font("Segoe UI", 9.5F);
            rbC.Location = new Point(251, 33);
            rbC.Margin = new Padding(3, 4, 3, 4);
            rbC.Name = "rbC";
            rbC.Size = new Size(94, 25);
            rbC.TabIndex = 2;
            rbC.Text = "Đáp án C";
            rbC.UseVisualStyleBackColor = true;
            // 
            // rbB
            // 
            rbB.AutoSize = true;
            rbB.Font = new Font("Segoe UI", 9.5F);
            rbB.Location = new Point(137, 33);
            rbB.Margin = new Padding(3, 4, 3, 4);
            rbB.Name = "rbB";
            rbB.Size = new Size(93, 25);
            rbB.TabIndex = 1;
            rbB.Text = "Đáp án B";
            rbB.UseVisualStyleBackColor = true;
            // 
            // rbA
            // 
            rbA.AutoSize = true;
            rbA.Font = new Font("Segoe UI", 9.5F);
            rbA.Location = new Point(23, 33);
            rbA.Margin = new Padding(3, 4, 3, 4);
            rbA.Name = "rbA";
            rbA.Size = new Size(94, 25);
            rbA.TabIndex = 0;
            rbA.Text = "Đáp án A";
            rbA.UseVisualStyleBackColor = true;
            // 
            // grpSettings
            // 
            grpSettings.Controls.Add(chkRandomDelay);
            grpSettings.Controls.Add(numDelayMax);
            grpSettings.Controls.Add(lblDelayMax);
            grpSettings.Controls.Add(numDelayMin);
            grpSettings.Controls.Add(lblDelayMin);
            grpSettings.Controls.Add(numScrollY);
            grpSettings.Controls.Add(lblScrollY);
            grpSettings.Controls.Add(chkAutoScroll);
            grpSettings.Controls.Add(numMaxQuestions);
            grpSettings.Controls.Add(lblMaxQ);
            grpSettings.Controls.Add(chkAutoNext);
            grpSettings.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpSettings.Location = new Point(14, 104);
            grpSettings.Margin = new Padding(3, 4, 3, 4);
            grpSettings.Name = "grpSettings";
            grpSettings.Padding = new Padding(3, 4, 3, 4);
            grpSettings.Size = new Size(640, 173);
            grpSettings.TabIndex = 5;
            grpSettings.TabStop = false;
            grpSettings.Text = "⚙️ Cấu Hình Chạy Auto & Thời Gian";
            // 
            // chkRandomDelay
            // 
            chkRandomDelay.AutoSize = true;
            chkRandomDelay.Checked = true;
            chkRandomDelay.CheckState = CheckState.Checked;
            chkRandomDelay.Font = new Font("Segoe UI", 9F);
            chkRandomDelay.Location = new Point(23, 87);
            chkRandomDelay.Margin = new Padding(3, 4, 3, 4);
            chkRandomDelay.Name = "chkRandomDelay";
            chkRandomDelay.Size = new Size(190, 24);
            chkRandomDelay.TabIndex = 12;
            chkRandomDelay.Text = "Random thời gian delay";
            chkRandomDelay.UseVisualStyleBackColor = true;
            // 
            // numDelayMax
            // 
            numDelayMax.Font = new Font("Segoe UI", 9.5F);
            numDelayMax.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numDelayMax.Location = new Point(417, 83);
            numDelayMax.Margin = new Padding(3, 4, 3, 4);
            numDelayMax.Maximum = new decimal(new int[] { 30000, 0, 0, 0 });
            numDelayMax.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numDelayMax.Name = "numDelayMax";
            numDelayMax.Size = new Size(80, 29);
            numDelayMax.TabIndex = 11;
            numDelayMax.Value = new decimal(new int[] { 4500, 0, 0, 0 });
            // 
            // lblDelayMax
            // 
            lblDelayMax.AutoSize = true;
            lblDelayMax.Font = new Font("Segoe UI", 9F);
            lblDelayMax.Location = new Point(377, 88);
            lblDelayMax.Name = "lblDelayMax";
            lblDelayMax.Size = new Size(37, 20);
            lblDelayMax.TabIndex = 10;
            lblDelayMax.Text = "đến:";
            // 
            // numDelayMin
            // 
            numDelayMin.Font = new Font("Segoe UI", 9.5F);
            numDelayMin.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numDelayMin.Location = new Point(291, 83);
            numDelayMin.Margin = new Padding(3, 4, 3, 4);
            numDelayMin.Maximum = new decimal(new int[] { 30000, 0, 0, 0 });
            numDelayMin.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numDelayMin.Name = "numDelayMin";
            numDelayMin.Size = new Size(80, 29);
            numDelayMin.TabIndex = 9;
            numDelayMin.Value = new decimal(new int[] { 2500, 0, 0, 0 });
            // 
            // lblDelayMin
            // 
            lblDelayMin.AutoSize = true;
            lblDelayMin.Font = new Font("Segoe UI", 9F);
            lblDelayMin.Location = new Point(200, 88);
            lblDelayMin.Name = "lblDelayMin";
            lblDelayMin.Size = new Size(101, 20);
            lblDelayMin.TabIndex = 8;
            lblDelayMin.Text = "Delay từ (ms):";
            // 
            // numScrollY
            // 
            numScrollY.Font = new Font("Segoe UI", 9.5F);
            numScrollY.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numScrollY.Location = new Point(291, 127);
            numScrollY.Margin = new Padding(3, 4, 3, 4);
            numScrollY.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
            numScrollY.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numScrollY.Name = "numScrollY";
            numScrollY.Size = new Size(80, 29);
            numScrollY.TabIndex = 7;
            numScrollY.Value = new decimal(new int[] { 200, 0, 0, 0 });
            // 
            // lblScrollY
            // 
            lblScrollY.AutoSize = true;
            lblScrollY.Font = new Font("Segoe UI", 9F);
            lblScrollY.Location = new Point(211, 132);
            lblScrollY.Name = "lblScrollY";
            lblScrollY.Size = new Size(79, 20);
            lblScrollY.TabIndex = 6;
            lblScrollY.Text = "Độ cuộn y:";
            // 
            // chkAutoScroll
            // 
            chkAutoScroll.AutoSize = true;
            chkAutoScroll.Checked = true;
            chkAutoScroll.CheckState = CheckState.Checked;
            chkAutoScroll.Font = new Font("Segoe UI", 9F);
            chkAutoScroll.Location = new Point(23, 131);
            chkAutoScroll.Margin = new Padding(3, 4, 3, 4);
            chkAutoScroll.Name = "chkAutoScroll";
            chkAutoScroll.Size = new Size(189, 24);
            chkAutoScroll.TabIndex = 5;
            chkAutoScroll.Text = "Tự động cuộn xuống (y)";
            chkAutoScroll.UseVisualStyleBackColor = true;
            // 
            // numMaxQuestions
            // 
            numMaxQuestions.Font = new Font("Segoe UI", 9.5F);
            numMaxQuestions.Location = new Point(514, 37);
            numMaxQuestions.Margin = new Padding(3, 4, 3, 4);
            numMaxQuestions.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numMaxQuestions.Name = "numMaxQuestions";
            numMaxQuestions.Size = new Size(80, 29);
            numMaxQuestions.TabIndex = 4;
            // 
            // lblMaxQ
            // 
            lblMaxQ.AutoSize = true;
            lblMaxQ.Font = new Font("Segoe UI", 9F);
            lblMaxQ.Location = new Point(389, 43);
            lblMaxQ.Name = "lblMaxQ";
            lblMaxQ.Size = new Size(133, 20);
            lblMaxQ.TabIndex = 3;
            lblMaxQ.Text = "Số câu (0=Vô hạn):";
            // 
            // chkAutoNext
            // 
            chkAutoNext.AutoSize = true;
            chkAutoNext.Checked = true;
            chkAutoNext.CheckState = CheckState.Checked;
            chkAutoNext.Font = new Font("Segoe UI", 9F);
            chkAutoNext.Location = new Point(23, 41);
            chkAutoNext.Margin = new Padding(3, 4, 3, 4);
            chkAutoNext.Name = "chkAutoNext";
            chkAutoNext.Size = new Size(186, 24);
            chkAutoNext.TabIndex = 0;
            chkAutoNext.Text = "Tự động bấm 'Câu tiếp'";
            chkAutoNext.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(667, 667);
            Controls.Add(grpSettings);
            Controls.Add(grpAnswer);
            Controls.Add(txtLog);
            Controls.Add(lblStatus);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Giải Trắc Nghiệm Tự Động";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            grpAnswer.ResumeLayout(false);
            grpAnswer.PerformLayout();
            grpSettings.ResumeLayout(false);
            grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDelayMax).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDelayMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)numScrollY).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxQuestions).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStart;
        private Button btnStop;
        private Label lblStatus;
        private TextBox txtLog;
        private GroupBox grpAnswer;
        private RadioButton rbRandom;
        private RadioButton rbD;
        private RadioButton rbC;
        private RadioButton rbB;
        private RadioButton rbA;
        private GroupBox grpSettings;
        private NumericUpDown numScrollY;
        private Label lblScrollY;
        private CheckBox chkAutoScroll;
        private NumericUpDown numMaxQuestions;
        private Label lblMaxQ;
        private CheckBox chkAutoNext;
        private CheckBox chkRandomDelay;
        private NumericUpDown numDelayMax;
        private Label lblDelayMax;
        private NumericUpDown numDelayMin;
        private Label lblDelayMin;
    }
}
