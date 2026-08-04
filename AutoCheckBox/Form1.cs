using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoCheckBox
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource? _cts;
        private readonly Random _rand = new Random();

        // 🖱️ Win32 API: Chuột & Bàn phím & Hotkey & Scroll
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_WHEEL = 0x0800;

        private const byte VK_RETURN = 0x0D;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        private const int HOTKEY_START_ID = 1;
        private const int HOTKEY_STOP_ID = 2;
        private const uint VK_F3 = 0x72;
        private const uint VK_F4 = 0x73;
        private const int WM_HOTKEY = 0x0312;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RegisterHotKey(this.Handle, HOTKEY_START_ID, 0, VK_F3);
            RegisterHotKey(this.Handle, HOTKEY_STOP_ID, 0, VK_F4);
            Log("💡 Mẹo: Bấm F3 để Bắt đầu Auto, bấm F4 để Dừng Auto nhanh.");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, HOTKEY_START_ID);
            UnregisterHotKey(this.Handle, HOTKEY_STOP_ID);
            StopAutoProcess();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();
                if (id == HOTKEY_START_ID && btnStart.Enabled)
                {
                    btnStart_Click(this, EventArgs.Empty);
                }
                else if (id == HOTKEY_STOP_ID && btnStop.Enabled)
                {
                    btnStop_Click(this, EventArgs.Empty);
                }
            }
            base.WndProc(ref m);
        }

        private static void ClickAt(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }

        private static void ScrollDown(int amountY)
        {
            int dwData = -amountY;
            mouse_event(MOUSEEVENTF_WHEEL, 0, 0, (uint)dwData, UIntPtr.Zero);
        }

        private static void PressEnterKey()
        {
            keybd_event(VK_RETURN, 0, 0, UIntPtr.Zero);
            keybd_event(VK_RETURN, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        private void Log(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Log(message)));
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        private void UpdateUIState(bool isRunning)
        {
            btnStart.Enabled = !isRunning;
            btnStop.Enabled = isRunning;
            grpAnswer.Enabled = !isRunning;
            grpSettings.Enabled = !isRunning;

            if (isRunning)
            {
                lblStatus.Text = "Trạng thái: 🟢 Đang chạy Auto... (Bấm F4 để Dừng)";
                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                lblStatus.Text = "Trạng thái: 🔴 Đã dừng (Bấm F3 để Bắt đầu)";
                lblStatus.ForeColor = Color.DarkRed;
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            _cts = new CancellationTokenSource();
            UpdateUIState(true);
            Log("🚀 ===== BẮT ĐẦU AUTO ĐÁNH TRẮC NGHIỆM LIÊN TỤC =====");

            int delayMin = (int)numDelayMin.Value;
            int delayMax = (int)numDelayMax.Value;
            bool isRandomDelay = chkRandomDelay.Checked;
            int maxQuestions = (int)numMaxQuestions.Value;
            bool autoNext = chkAutoNext.Checked;
            bool autoScroll = chkAutoScroll.Checked;
            int scrollY = (int)numScrollY.Value;
            string selectedOption = GetSelectedOptionName();

            string delayInfo = isRandomDelay ? $"Random {delayMin}-{delayMax}ms" : $"{delayMin}ms";
            Log($"📋 Lựa chọn: [{selectedOption}] | Delay: {delayInfo} | Auto Next: {autoNext} | Scroll Trước: {autoScroll} ({scrollY}px)");

            try
            {
                var token = _cts.Token;
                await Task.Run(() => RunAutoQuizSolverAsync(selectedOption, isRandomDelay, delayMin, delayMax, maxQuestions, autoNext, autoScroll, scrollY, token), token);
                if (!token.IsCancellationRequested)
                {
                    Log("🎉 ===== HOÀN THÀNH CHUỖI CÁC LẦN THI =====");
                }
            }
            catch (OperationCanceledException)
            {
                Log("🛑 Đã dừng Auto theo yêu cầu người dùng.");
            }
            catch (AggregateException ex) when (ex.InnerException is OperationCanceledException)
            {
                Log("🛑 Đã dừng Auto theo yêu cầu người dùng.");
            }
            catch (Exception ex)
            {
                Log($"❌ Lỗi ngoài dự kiến: {ex.Message}");
            }
            finally
            {
                UpdateUIState(false);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            StopAutoProcess();
        }

        private void StopAutoProcess()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
                Log("🛑 Đang gửi tín hiệu dừng...");
            }
        }

        private string GetSelectedOptionName()
        {
            if (rbA.Checked) return "A";
            if (rbB.Checked) return "B";
            if (rbC.Checked) return "C";
            if (rbD.Checked) return "D";
            return "RANDOM";
        }

        private string? FindTemplateFile(params string[] baseNames)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string currentDir = Directory.GetCurrentDirectory();

            string[] dirsToSearch = new string[]
            {
                baseDir,
                currentDir,
                Path.Combine(baseDir, ".."),
                Path.Combine(baseDir, "..", ".."),
                Path.Combine(baseDir, "..", "..", ".."),
                Path.Combine(baseDir, "..", "..", "..", "..")
            };

            List<string> candidateFiles = new();
            foreach (var name in baseNames)
            {
                candidateFiles.Add($"{name}.png");
                candidateFiles.Add($"{name}.png.png");
                candidateFiles.Add($"{name}.jpg");
                candidateFiles.Add($"{name}.jpeg");
            }

            foreach (var dir in dirsToSearch)
            {
                if (string.IsNullOrWhiteSpace(dir)) continue;
                string fullDir = Path.GetFullPath(dir);

                foreach (var fileName in candidateFiles)
                {
                    string candidate = Path.Combine(fullDir, fileName);
                    if (File.Exists(candidate))
                    {
                        return candidate;
                    }
                }
            }

            return null;
        }

        private string CaptureScreen()
        {
            var primaryScreen = Screen.PrimaryScreen;
            if (primaryScreen == null)
            {
                throw new Exception("Không thể chụp ảnh màn hình chính.");
            }

            using Bitmap screenshot = new Bitmap(primaryScreen.Bounds.Width, primaryScreen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(0, 0, 0, 0, screenshot.Size);
            }

            string screenPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "screen.png");
            screenshot.Save(screenPath);
            return screenPath;
        }

        private List<OpenCvSharp.Point> FindMatchingPoints(string screenPath, string templatePath, double threshold = 0.8, int minDistance = 20)
        {
            using var img = Cv2.ImRead(screenPath, ImreadModes.Color);
            using var template = Cv2.ImRead(templatePath, ImreadModes.Color);

            if (template.Empty()) return new List<OpenCvSharp.Point>();

            using var result = new Mat();
            Cv2.MatchTemplate(img, template, result, TemplateMatchModes.CCoeffNormed);

            List<OpenCvSharp.Point> rawPoints = new();
            for (int y = 0; y < result.Rows; y++)
            {
                for (int x = 0; x < result.Cols; x++)
                {
                    if (result.At<float>(y, x) >= threshold)
                    {
                        rawPoints.Add(new OpenCvSharp.Point(x, y));
                    }
                }
            }

            List<OpenCvSharp.Point> filteredPoints = new();
            foreach (var pt in rawPoints)
            {
                bool tooClose = false;
                foreach (var fp in filteredPoints)
                {
                    if (Math.Abs(pt.X - fp.X) < minDistance && Math.Abs(pt.Y - fp.Y) < minDistance)
                    {
                        tooClose = true;
                        break;
                    }
                }
                if (!tooClose)
                    filteredPoints.Add(pt);
            }

            return filteredPoints;
        }

        private async Task RunAutoQuizSolverAsync(string optionChoice, bool isRandomDelay, int delayMin, int delayMax, int maxQuestions, bool autoNext, bool autoScroll, int scrollY, CancellationToken token)
        {
            int totalExamsCompleted = 0;

            int GetNextDelay()
            {
                if (isRandomDelay && delayMax >= delayMin)
                {
                    return _rand.Next(delayMin, delayMax + 1);
                }
                return delayMin;
            }

            while (!token.IsCancellationRequested)
            {
                totalExamsCompleted++;
                Log($"\n🔄 ============================================");
                Log($"🏆 BẮT ĐẦU LẦN THI / LUYỆN THỨ #{totalExamsCompleted}");
                Log($"🔄 ============================================");

                int questionCounter = 0;
                bool examFinished = false;

                while (!token.IsCancellationRequested && !examFinished)
                {
                    token.ThrowIfCancellationRequested();
                    questionCounter++;
                    if (maxQuestions > 0 && questionCounter > maxQuestions)
                    {
                        Log($"🏁 Đã hoàn thành đủ số câu giới hạn ({maxQuestions} câu).");
                        break;
                    }

                    Log($"\n📝 --- Đang xử lý câu thứ #{questionCounter} (Lần thi #{totalExamsCompleted}) ---");

                    // 1️⃣ Tự động cuộn xuống TRƯỚC khi chọn đáp án (nếu bật Auto Scroll)
                    if (autoScroll)
                    {
                        token.ThrowIfCancellationRequested();
                        Log($"📜 [1] Cuộn màn hình xuống {scrollY}px trước...");
                        ScrollDown(scrollY);
                        await Task.Delay(250, token);
                    }

                    // 2️⃣ Tìm file mẫu cho đáp án (Ưu tiên file riêng A/B/C/D, nếu không có thì dùng checkbox_unchecked)
                    string? templatePathOption = null;
                    string currentTargetOption = optionChoice;

                    if (optionChoice == "RANDOM")
                    {
                        string[] options = { "A", "B", "C", "D" };
                        currentTargetOption = options[_rand.Next(options.Length)];
                        templatePathOption = FindTemplateFile(currentTargetOption, $"option_{currentTargetOption}");
                    }
                    else
                    {
                        templatePathOption = FindTemplateFile(optionChoice, $"option_{optionChoice}");
                    }

                    if (string.IsNullOrEmpty(templatePathOption))
                    {
                        templatePathOption = FindTemplateFile("checkbox_unchecked", "A", "option_A", "radio");
                    }

                    if (string.IsNullOrEmpty(templatePathOption))
                    {
                        Log($"❌ Không tìm thấy bất kỳ file ảnh mẫu đáp án nào!");
                        return;
                    }

                    // 3️⃣ Chụp màn hình & tìm điểm đáp án (SAU KHI ĐÃ CUỘN XUỐNG)
                    token.ThrowIfCancellationRequested();
                    string screenPath = CaptureScreen();
                    var optionPoints = FindMatchingPoints(screenPath, templatePathOption);

                    if (optionPoints.Count == 0)
                    {
                        Log($"⚠️ Không tìm thấy đáp án trên màn hình hiện tại.");
                    }
                    else
                    {
                        optionPoints.Sort((p1, p2) => p1.Y.CompareTo(p2.Y));

                        int selectedIndex = 0;
                        if (optionChoice == "RANDOM")
                        {
                            selectedIndex = _rand.Next(optionPoints.Count);
                            string[] optionLabels = { "A", "B", "C", "D" };
                            string label = selectedIndex < optionLabels.Length ? optionLabels[selectedIndex] : $"#{selectedIndex + 1}";
                            Log($"🎲 Chọn ngẫu nhiên đáp án [{label}] (vị trí {selectedIndex + 1}/{optionPoints.Count})");
                        }
                        else if (optionChoice == "A") selectedIndex = 0;
                        else if (optionChoice == "B") selectedIndex = Math.Min(1, optionPoints.Count - 1);
                        else if (optionChoice == "C") selectedIndex = Math.Min(2, optionPoints.Count - 1);
                        else if (optionChoice == "D") selectedIndex = Math.Min(3, optionPoints.Count - 1);

                        using var tmplOpt = Cv2.ImRead(templatePathOption, ImreadModes.Color);
                        var pt = optionPoints[selectedIndex];
                        int clickX = pt.X + tmplOpt.Width / 2;
                        int clickY = pt.Y + tmplOpt.Height / 2;

                        Log($"🖱️ [2] Click chọn đáp án tại ({clickX}, {clickY})");
                        ClickAt(clickX, clickY);
                    }

                    int delayMs = GetNextDelay();
                    Log($"⏳ Nghỉ ngẫu nhiên {delayMs}ms trước thao tác tiếp theo...");
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(delayMs, token);

                    // 4️⃣ Tự động bấm nút "Câu tiếp theo" (Next) hoặc "Kết thúc bài thi" (Finish / Submit)
                    if (autoNext)
                    {
                        token.ThrowIfCancellationRequested();
                        Log("📌 [3] Tìm nút Chuyển câu tiếp theo hoặc Kết thúc...");

                        string? templatePathNext = FindTemplateFile("next", "button_next", "tiep_tuc", "cau_tiep", "tiep", "btn_next");
                        bool clickedNext = false;

                        if (!string.IsNullOrEmpty(templatePathNext))
                        {
                            string screenNextPath = CaptureScreen();
                            var nextPoints = FindMatchingPoints(screenNextPath, templatePathNext);

                            if (nextPoints.Count > 0)
                            {
                                using var tmplNext = Cv2.ImRead(templatePathNext, ImreadModes.Color);
                                var ptNext = nextPoints[0];
                                int clickX = ptNext.X + tmplNext.Width / 2;
                                int clickY = ptNext.Y + tmplNext.Height / 2;

                                Log($"🖱️ Click nút Next tại ({clickX}, {clickY})");
                                ClickAt(clickX, clickY);
                                clickedNext = true;
                            }
                        }

                        if (!clickedNext)
                        {
                            // Kiểm tra nếu có nút "Kết thúc bài thi" / "Nộp bài" trên màn hình
                            string? templatePathFinish = FindTemplateFile("finish", "submit", "ket_thuc", "nop_bai", "finish_exam", "btn_finish", "btn_submit");

                            if (!string.IsNullOrEmpty(templatePathFinish))
                            {
                                string screenFinishPath = CaptureScreen();
                                var finishPoints = FindMatchingPoints(screenFinishPath, templatePathFinish);

                                if (finishPoints.Count > 0)
                                {
                                    using var tmplFinish = Cv2.ImRead(templatePathFinish, ImreadModes.Color);
                                    var ptFinish = finishPoints[0];
                                    int clickX = ptFinish.X + tmplFinish.Width / 2;
                                    int clickY = ptFinish.Y + tmplFinish.Height / 2;

                                    Log($"🏆 [4] Tìm thấy nút KẾT THÚC BÀI THI tại ({clickX}, {clickY}) ➔ Click Nộp bài!");
                                    ClickAt(clickX, clickY);
                                    Log($"🎉 Lần thi #{totalExamsCompleted} đã hoàn tất!");
                                    examFinished = true; // Kết thúc lần thi hiện tại
                                    break;
                                }
                            }

                            Log("⌨️ Không tìm thấy ảnh nút Next/Kết thúc ➔ Gửi phím ENTER.");
                            PressEnterKey();
                        }

                        int nextDelayMs = GetNextDelay();
                        Log($"⏳ Nghỉ ngẫu nhiên {nextDelayMs}ms trước thao tác tiếp...");
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(nextDelayMs, token);
                    }
                }

                // ==============================================================
                // 5️⃣ Tự động bấm nút "Luyện tất cả" / "Luyện lại" để lặp lại lần thi mới
                // ==============================================================
                if (token.IsCancellationRequested) break;

                Log("⏳ Chờ 1500ms để trang kết quả hiển thị nút 'Luyện tất cả'...");
                await Task.Delay(1500, token);

                string? templatePathRetake = FindTemplateFile("luyen_tat_ca", "practice_all", "luyen_lai", "retake", "retry", "restart", "lam_lai", "btn_practice");

                if (!string.IsNullOrEmpty(templatePathRetake))
                {
                    string screenRetakePath = CaptureScreen();
                    var retakePoints = FindMatchingPoints(screenRetakePath, templatePathRetake);

                    if (retakePoints.Count > 0)
                    {
                        using var tmplRetake = Cv2.ImRead(templatePathRetake, ImreadModes.Color);
                        var ptRetake = retakePoints[0];
                        int clickX = ptRetake.X + tmplRetake.Width / 2;
                        int clickY = ptRetake.Y + tmplRetake.Height / 2;

                        Log($"🔁 [5] Tìm thấy nút 'LUYỆN TẤT CẢ' tại ({clickX}, {clickY}) ➔ Click để làm bài mới!");
                        ClickAt(clickX, clickY);

                        Log("⏳ Chờ 2000ms để trang bài thi mới khởi tạo...");
                        await Task.Delay(2000, token);
                    }
                    else
                    {
                        Log("ℹ️ Không tìm thấy nút 'Luyện tất cả' trên màn hình ➔ Kết thúc tiến trình.");
                        break;
                    }
                }
                else
                {
                    Log("ℹ️ Chưa có file ảnh mẫu nút 'Luyện tất cả' (ví dụ: luyen_tat_ca.png hay practice_all.png) ➔ Kết thúc tiến trình.");
                    break;
                }
            }
        }
    }
}
