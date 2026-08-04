namespace AutoCheckBox
{
    partial class QABankForm
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
            dgvQuestions = new DataGridView();
            grpInput = new GroupBox();
            btnSave = new Button();
            btnDelete = new Button();
            btnUpdate = new Button();
            btnAdd = new Button();
            cmbOption = new ComboBox();
            lblOption = new Label();
            txtAnswer = new TextBox();
            lblAnswer = new Label();
            txtQuestion = new TextBox();
            lblQuestion = new Label();
            txtSearch = new TextBox();
            lblSearch = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvQuestions).BeginInit();
            grpInput.SuspendLayout();
            SuspendLayout();
            // 
            // dgvQuestions
            // 
            dgvQuestions.AllowUserToAddRows = false;
            dgvQuestions.AllowUserToDeleteRows = false;
            dgvQuestions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvQuestions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvQuestions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvQuestions.Location = new System.Drawing.Point(12, 45);
            dgvQuestions.MultiSelect = false;
            dgvQuestions.Name = "dgvQuestions";
            dgvQuestions.ReadOnly = true;
            dgvQuestions.RowHeadersVisible = false;
            dgvQuestions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvQuestions.Size = new System.Drawing.Size(760, 240);
            dgvQuestions.TabIndex = 0;
            dgvQuestions.SelectionChanged += dgvQuestions_SelectionChanged;
            // 
            // grpInput
            // 
            grpInput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpInput.Controls.Add(btnSave);
            grpInput.Controls.Add(btnDelete);
            grpInput.Controls.Add(btnUpdate);
            grpInput.Controls.Add(btnAdd);
            grpInput.Controls.Add(cmbOption);
            grpInput.Controls.Add(lblOption);
            grpInput.Controls.Add(txtAnswer);
            grpInput.Controls.Add(lblAnswer);
            grpInput.Controls.Add(txtQuestion);
            grpInput.Controls.Add(lblQuestion);
            grpInput.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpInput.Location = new System.Drawing.Point(12, 295);
            grpInput.Name = "grpInput";
            grpInput.Size = new System.Drawing.Size(760, 210);
            grpInput.TabIndex = 1;
            grpInput.TabStop = false;
            grpInput.Text = "✏️ Nhập / Chỉnh Sửa Câu Hỏi";
            // 
            // btnSave
            // 
            btnSave.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSave.ForeColor = System.Drawing.Color.White;
            btnSave.Location = new System.Drawing.Point(475, 160);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(140, 38);
            btnSave.TabIndex = 9;
            btnSave.Text = "💾 Lưu File (JSON)";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnDelete.ForeColor = System.Drawing.Color.White;
            btnDelete.Location = new System.Drawing.Point(340, 160);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(120, 38);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "🗑️ Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnUpdate.ForeColor = System.Drawing.Color.Black;
            btnUpdate.Location = new System.Drawing.Point(205, 160);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new System.Drawing.Size(120, 38);
            btnUpdate.TabIndex = 7;
            btnUpdate.Text = "✏️ Cập Nhật";
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnAdd.ForeColor = System.Drawing.Color.White;
            btnAdd.Location = new System.Drawing.Point(70, 160);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(120, 38);
            btnAdd.TabIndex = 6;
            btnAdd.Text = "➕ Thêm Mới";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // cmbOption
            // 
            cmbOption.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOption.Font = new Font("Segoe UI", 9.5F);
            cmbOption.FormattingEnabled = true;
            cmbOption.Items.AddRange(new object[] { "A", "B", "C", "D" });
            cmbOption.Location = new System.Drawing.Point(120, 115);
            cmbOption.Name = "cmbOption";
            cmbOption.Size = new System.Drawing.Size(80, 25);
            cmbOption.TabIndex = 5;
            // 
            // lblOption
            // 
            lblOption.AutoSize = true;
            lblOption.Font = new Font("Segoe UI", 9F);
            lblOption.Location = new System.Drawing.Point(15, 120);
            lblOption.Name = "lblOption";
            lblOption.Size = new System.Drawing.Size(97, 15);
            lblOption.TabIndex = 4;
            lblOption.Text = "Đáp án (A/B/C/D):";
            // 
            // txtAnswer
            // 
            txtAnswer.Font = new Font("Segoe UI", 9.5F);
            txtAnswer.Location = new System.Drawing.Point(310, 115);
            txtAnswer.Name = "txtAnswer";
            txtAnswer.Size = new System.Drawing.Size(430, 24);
            txtAnswer.TabIndex = 3;
            // 
            // lblAnswer
            // 
            lblAnswer.AutoSize = true;
            lblAnswer.Font = new Font("Segoe UI", 9F);
            lblAnswer.Location = new System.Drawing.Point(220, 120);
            lblAnswer.Name = "lblAnswer";
            lblAnswer.Size = new System.Drawing.Size(84, 15);
            lblAnswer.TabIndex = 2;
            lblAnswer.Text = "Chi tiết đáp án:";
            // 
            // txtQuestion
            // 
            txtQuestion.Font = new Font("Segoe UI", 9.5F);
            txtQuestion.Location = new System.Drawing.Point(120, 25);
            txtQuestion.Multiline = true;
            txtQuestion.Name = "txtQuestion";
            txtQuestion.ScrollBars = ScrollBars.Vertical;
            txtQuestion.Size = new System.Drawing.Size(620, 75);
            txtQuestion.TabIndex = 1;
            // 
            // lblQuestion
            // 
            lblQuestion.AutoSize = true;
            lblQuestion.Font = new Font("Segoe UI", 9F);
            lblQuestion.Location = new System.Drawing.Point(15, 30);
            lblQuestion.Name = "lblQuestion";
            lblQuestion.Size = new System.Drawing.Size(102, 15);
            lblQuestion.TabIndex = 0;
            lblQuestion.Text = "Nội dung câu hỏi:";
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 9.5F);
            txtSearch.Location = new System.Drawing.Point(132, 12);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(640, 24);
            txtSearch.TabIndex = 2;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblSearch.Location = new System.Drawing.Point(12, 15);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new System.Drawing.Size(114, 17);
            lblSearch.TabIndex = 3;
            lblSearch.Text = "🔍 Tìm kiếm nhanh:";
            // 
            // QABankForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(784, 517);
            Controls.Add(lblSearch);
            Controls.Add(txtSearch);
            Controls.Add(grpInput);
            Controls.Add(dgvQuestions);
            Name = "QABankForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quản Lý Ngân Hàng Câu Hỏi & Đáp Án (questions.json)";
            Load += QABankForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvQuestions).EndInit();
            grpInput.ResumeLayout(false);
            grpInput.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvQuestions;
        private GroupBox grpInput;
        private TextBox txtQuestion;
        private Label lblQuestion;
        private ComboBox cmbOption;
        private Label lblOption;
        private TextBox txtAnswer;
        private Label lblAnswer;
        private Button btnSave;
        private Button btnDelete;
        private Button btnUpdate;
        private Button btnAdd;
        private TextBox txtSearch;
        private Label lblSearch;
    }
}
