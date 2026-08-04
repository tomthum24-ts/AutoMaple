using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace AutoCheckBox
{
    public partial class QABankForm : Form
    {
        private List<QuestionItem> _questionList = new List<QuestionItem>();
        private string _jsonPath = "";
        private bool _isRefreshing = false;

        public QABankForm()
        {
            InitializeComponent();
        }

        private void QABankForm_Load(object sender, EventArgs e)
        {
            _jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "questions.json");
            cmbOption.SelectedIndex = 0;
            LoadData();
        }

        private void LoadData()
        {
            if (File.Exists(_jsonPath))
            {
                try
                {
                    string json = File.ReadAllText(_jsonPath);
                    _questionList = JsonSerializer.Deserialize<List<QuestionItem>>(json) ?? new List<QuestionItem>();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi nạp file questions.json: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _questionList = new List<QuestionItem>();
                }
            }

            RefreshGrid();
        }

        private void RefreshGrid(string filter = "")
        {
            _isRefreshing = true;
            try
            {
                dgvQuestions.DataSource = null;

                var filtered = string.IsNullOrWhiteSpace(filter)
                    ? _questionList
                    : _questionList.Where(q => (q.question ?? "").ToLower().Contains(filter.ToLower()) ||
                                               (q.answer ?? "").ToLower().Contains(filter.ToLower())).ToList();

                dgvQuestions.DataSource = filtered;

                if (dgvQuestions.Columns.Contains("question"))
                    dgvQuestions.Columns["question"].HeaderText = "Nội dung câu hỏi";
                if (dgvQuestions.Columns.Contains("option"))
                {
                    dgvQuestions.Columns["option"].HeaderText = "Đáp án";
                    dgvQuestions.Columns["option"].Width = 80;
                }
                if (dgvQuestions.Columns.Contains("answer"))
                    dgvQuestions.Columns["answer"].HeaderText = "Chi tiết đáp án";
            }
            catch
            {
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void dgvQuestions_SelectionChanged(object sender, EventArgs e)
        {
            if (_isRefreshing) return;

            try
            {
                if (dgvQuestions.SelectedRows.Count > 0)
                {
                    var row = dgvQuestions.SelectedRows[0];
                    if (row.DataBoundItem is QuestionItem item)
                    {
                        txtQuestion.Text = item.question ?? "";
                        txtAnswer.Text = item.answer ?? "";
                        string opt = (item.option ?? "A").Trim().ToUpper();
                        int idx = cmbOption.Items.IndexOf(opt);
                        cmbOption.SelectedIndex = idx >= 0 ? idx : 0;
                    }
                }
            }
            catch
            {
                // Safe guard against transient CurrencyManager indexing exception
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQuestion.Text))
            {
                MessageBox.Show("Vui lòng nhập nội dung câu hỏi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newItem = new QuestionItem
            {
                question = txtQuestion.Text.Trim(),
                option = cmbOption.SelectedItem?.ToString() ?? "A",
                answer = txtAnswer.Text.Trim()
            };

            _questionList.Add(newItem);
            RefreshGrid(txtSearch.Text);
            SaveToJsonQuiet();
            MessageBox.Show("Đã thêm câu hỏi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvQuestions.SelectedRows.Count > 0 && dgvQuestions.SelectedRows[0].DataBoundItem is QuestionItem selected)
            {
                selected.question = txtQuestion.Text.Trim();
                selected.option = cmbOption.SelectedItem?.ToString() ?? "A";
                selected.answer = txtAnswer.Text.Trim();

                RefreshGrid(txtSearch.Text);
                SaveToJsonQuiet();
                MessageBox.Show("Đã cập nhật câu hỏi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvQuestions.SelectedRows.Count > 0 && dgvQuestions.SelectedRows[0].DataBoundItem is QuestionItem selected)
            {
                var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa câu hỏi: \"{selected.question}\"?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    _questionList.Remove(selected);
                    RefreshGrid(txtSearch.Text);
                    SaveToJsonQuiet();
                }
            }
        }

        private void SaveToJsonQuiet()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_questionList, options);
                File.WriteAllText(_jsonPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveToJsonQuiet();
            MessageBox.Show($"Đã lưu thành công {_questionList.Count} câu hỏi vào questions.json!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshGrid(txtSearch.Text);
        }
    }
}
