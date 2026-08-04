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
            btnOpenQABank = new Button();
            chkUseQABank = new CheckBox();
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
            btnStart.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnStart.ForeColor = System.Drawing.Color.White;
            btnStart.Location = new System.Drawing.Point(12, 255);
            btnStart.Name = "btnStart";
            btnStart.Size = new System.Drawing.Size(185, 42);
            btnStart.TabIndex = 0;
            btnStart.Text = "▶ Bắt đầu (F3)";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            btnStop.Enabled = false;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnStop.ForeColor = System.Drawing.Color.White;
            btnStop.Location = new System.Drawing.Point(203, 255);
            btnStop.Name = "btnStop";
            btnStop.Size = new System.Drawing.Size(185, 42);
            btnStop.TabIndex = 1;
            btnStop.Text = "⏹ Dừng lại (F4)";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStatus.ForeColor = System.Drawing.Color.DarkBlue;
            lblStatus.Location = new System.Drawing.Point(12, 305);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(275, 19);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Trạng thái: Sẵn sàng (Bấm F3 hoặc Bắt đầu)";
            // 
            // txtLog
            // 
            txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtLog.BackColor = System.Drawing.Color.Black;
            txtLog.Font = new Font("Consolas", 9.5F);
            txtLog.ForeColor = System.Drawing.Color.LimeGreen;
            txtLog.Location = new System.Drawing.Point(12, 330);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new System.Drawing.Size(560, 190);
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
            grpAnswer.Location = new System.Drawing.Point(12, 12);
            grpAnswer.Name = "grpAnswer";
            grpAnswer.Size = new System.Drawing.Size(560, 60);
            grpAnswer.TabIndex = 4;
            grpAnswer.TabStop = false;
            grpAnswer.Text = "🎯 Chọn Đáp Án Dự Phòng (Khi Không Tìm Thấy Trong Ngân Hàng)";
            // 
            // rbRandom
            // 
            rbRandom.AutoSize = true;
            rbRandom.Checked = true;
            rbRandom.Font = new Font("Segoe UI", 9.5F);
            rbRandom.Location = new System.Drawing.Point(420, 25);
            rbRandom.Name = "rbRandom";
            rbRandom.Size = new System.Drawing.Size(124, 21);
            rbRandom.TabIndex = 4;
            rbRandom.TabStop = true;
            rbRandom.Text = "🎲 Ngẫu nhiên";
            rbRandom.UseVisualStyleBackColor = true;
            // 
            // rbD
            // 
            rbD.AutoSize = true;
            rbD.Font = new Font("Segoe UI", 9.5F);
            rbD.Location = new System.Drawing.Point(320, 25);
            rbD.Name = "rbD";
            rbD.Size = new System.Drawing.Size(81, 21);
            rbD.TabIndex = 3;
            rbD.Text = "Đáp án D";
            rbD.UseVisualStyleBackColor = true;
            // 
            // rbC
            // 
            rbC.AutoSize = true;
            rbC.Font = new Font("Segoe UI", 9.5F);
            rbC.Location = new System.Drawing.Point(220, 25);
            rbC.Name = "rbC";
            rbC.Size = new System.Drawing.Size(81, 21);
            rbC.TabIndex = 2;
            rbC.Text = "Đáp án C";
            rbC.UseVisualStyleBackColor = true;
            // 
            // rbB
            // 
            rbB.AutoSize = true;
            rbB.Font = new Font("Segoe UI", 9.5F);
            rbB.Location = new System.Drawing.Point(120, 25);
            rbB.Name = "rbB";
            rbB.Size = new System.Drawing.Size(80, 21);
            rbB.TabIndex = 1;
            rbB.Text = "Đáp án B";
            rbB.UseVisualStyleBackColor = true;
            // 
            // rbA
            // 
            rbA.AutoSize = true;
            rbA.Font = new Font("Segoe UI", 9.5F);
            rbA.Location = new System.Drawing.Point(20, 25);
            rbA.Name = "rbA";
            rbA.Size = new System.Drawing.Size(81, 21);
            rbA.TabIndex = 0;
            rbA.Text = "Đáp án A";
            rbA.UseVisualStyleBackColor = true;
            // 
            // grpSettings
            // 
            grpSettings.Controls.Add(btnOpenQABank);
            grpSettings.Controls.Add(chkUseQABank);
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
            grpSettings.Location = new System.Drawing.Point(12, 78);
            grpSettings.Name = "grpSettings";
            grpSettings.Size = new System.Drawing.Size(560, 165);
            grpSettings.TabIndex = 5;
            grpSettings.TabStop = false;
            grpSettings.Text = "⚙️ Cấu Hình Chạy Auto & Ngân Hàng Câu Hỏi";
            // 
            // btnOpenQABank
            // 
            btnOpenQABank.Font = new Font("Segoe UI", 9F);
            btnOpenQABank.Location = new System.Drawing.Point(340, 128);
            btnOpenQABank.Name = "btnOpenQABank";
            btnOpenQABank.Size = new System.Drawing.Size(180, 26);
            btnOpenQABank.TabIndex = 14;
            btnOpenQABank.Text = "📝 Mở Ngân Hàng Câu Hỏi";
            btnOpenQABank.UseVisualStyleBackColor = true;
            btnOpenQABank.Click += btnOpenQABank_Click;
            // 
            // chkUseQABank
            // 
            chkUseQABank.AutoSize = true;
            chkUseQABank.Checked = true;
            chkUseQABank.CheckState = CheckState.Checked;
            chkUseQABank.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chkUseQABank.ForeColor = System.Drawing.Color.DarkGreen;
            chkUseQABank.Location = new System.Drawing.Point(20, 131);
            chkUseQABank.Name = "chkUseQABank";
            chkUseQABank.Size = new System.Drawing.Size(305, 19);
            chkUseQABank.TabIndex = 13;
            chkUseQABank.Text = "🔍 Tự nhận diện text & Trả lời theo questions.json";
            chkUseQABank.UseVisualStyleBackColor = true;
            // 
            // chkRandomDelay
            // 
            chkRandomDelay.AutoSize = true;
            chkRandomDelay.Checked = true;
            chkRandomDelay.CheckState = CheckState.Checked;
            chkRandomDelay.Font = new Font("Segoe UI", 9F);
            chkRandomDelay.Location = new System.Drawing.Point(20, 65);
            chkRandomDelay.Name = "chkRandomDelay";
            chkRandomDelay.Size = new System.Drawing.Size(145, 19);
            chkRandomDelay.TabIndex = 12;
            chkRandomDelay.Text = "Random thời gian delay";
            chkRandomDelay.UseVisualStyleBackColor = true;
            // 
            // numDelayMax
            // 
            numDelayMax.Font = new Font("Segoe UI", 9.5F);
            numDelayMax.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numDelayMax.Location = new System.Drawing.Point(365, 62);
            numDelayMax.Maximum = new decimal(new int[] { 30000, 0, 0, 0 });
            numDelayMax.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numDelayMax.Name = "numDelayMax";
            numDelayMax.Size = new System.Drawing.Size(70, 24);
            numDelayMax.TabIndex = 11;
            numDelayMax.Value = new decimal(new int[] { 1500, 0, 0, 0 });
            // 
            // lblDelayMax
            // 
            lblDelayMax.AutoSize = true;
            lblDelayMax.Font = new Font("Segoe UI", 9F);
            lblDelayMax.Location = new System.Drawing.Point(330, 66);
            lblDelayMax.Name = "lblDelayMax";
            lblDelayMax.Size = new System.Drawing.Size(30, 15);
            lblDelayMax.TabIndex = 10;
            lblDelayMax.Text = "đến:";
            // 
            // numDelayMin
            // 
            numDelayMin.Font = new Font("Segoe UI", 9.5F);
            numDelayMin.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numDelayMin.Location = new System.Drawing.Point(255, 62);
            numDelayMin.Maximum = new decimal(new int[] { 30000, 0, 0, 0 });
            numDelayMin.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numDelayMin.Name = "numDelayMin";
            numDelayMin.Size = new System.Drawing.Size(70, 24);
            numDelayMin.TabIndex = 9;
            numDelayMin.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // lblDelayMin
            // 
            lblDelayMin.AutoSize = true;
            lblDelayMin.Font = new Font("Segoe UI", 9F);
            lblDelayMin.Location = new System.Drawing.Point(175, 66);
            lblDelayMin.Name = "lblDelayMin";
            lblDelayMin.Size = new System.Drawing.Size(78, 15);
            lblDelayMin.TabIndex = 8;
            lblDelayMin.Text = "Delay từ (ms):";
            // 
            // numScrollY
            // 
            numScrollY.Font = new Font("Segoe UI", 9.5F);
            numScrollY.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numScrollY.Location = new System.Drawing.Point(255, 95);
            numScrollY.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
            numScrollY.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numScrollY.Name = "numScrollY";
            numScrollY.Size = new System.Drawing.Size(70, 24);
            numScrollY.TabIndex = 7;
            numScrollY.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // lblScrollY
            // 
            lblScrollY.AutoSize = true;
            lblScrollY.Font = new Font("Segoe UI", 9F);
            lblScrollY.Location = new System.Drawing.Point(185, 99);
            lblScrollY.Name = "lblScrollY";
            lblScrollY.Size = new System.Drawing.Size(65, 15);
            lblScrollY.TabIndex = 6;
            lblScrollY.Text = "Độ cuộn y:";
            // 
            // chkAutoScroll
            // 
            chkAutoScroll.AutoSize = true;
            chkAutoScroll.Checked = true;
            chkAutoScroll.CheckState = CheckState.Checked;
            chkAutoScroll.Font = new Font("Segoe UI", 9F);
            chkAutoScroll.Location = new System.Drawing.Point(20, 98);
            chkAutoScroll.Name = "chkAutoScroll";
            chkAutoScroll.Size = new System.Drawing.Size(150, 19);
            chkAutoScroll.TabIndex = 5;
            chkAutoScroll.Text = "Tự động cuộn xuống (y)";
            chkAutoScroll.UseVisualStyleBackColor = true;
            // 
            // numMaxQuestions
            // 
            numMaxQuestions.Font = new Font("Segoe UI", 9.5F);
            numMaxQuestions.Location = new System.Drawing.Point(450, 28);
            numMaxQuestions.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numMaxQuestions.Name = "numMaxQuestions";
            numMaxQuestions.Size = new System.Drawing.Size(70, 24);
            numMaxQuestions.TabIndex = 4;
            // 
            // lblMaxQ
            // 
            lblMaxQ.AutoSize = true;
            lblMaxQ.Font = new Font("Segoe UI", 9F);
            lblMaxQ.Location = new System.Drawing.Point(340, 32);
            lblMaxQ.Name = "lblMaxQ";
            lblMaxQ.Size = new System.Drawing.Size(104, 15);
            lblMaxQ.TabIndex = 3;
            lblMaxQ.Text = "Số câu (0=Vô hạn):";
            // 
            // chkAutoNext
            // 
            chkAutoNext.AutoSize = true;
            chkAutoNext.Checked = true;
            chkAutoNext.CheckState = CheckState.Checked;
            chkAutoNext.Font = new Font("Segoe UI", 9F);
            chkAutoNext.Location = new System.Drawing.Point(20, 31);
            chkAutoNext.Name = "chkAutoNext";
            chkAutoNext.Size = new System.Drawing.Size(155, 19);
            chkAutoNext.TabIndex = 0;
            chkAutoNext.Text = "Tự động bấm 'Câu tiếp'";
            chkAutoNext.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(584, 530);
            Controls.Add(grpSettings);
            Controls.Add(grpAnswer);
            Controls.Add(txtLog);
            Controls.Add(lblStatus);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Giải Trắc Nghiệm Tự Động (Ngân Hàng Câu Hỏi AI/OCR)";
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
        private CheckBox chkUseQABank;
        private Button btnOpenQABank;
    }
}
