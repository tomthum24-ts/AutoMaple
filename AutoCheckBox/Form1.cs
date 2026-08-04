using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoCheckBox
{
    public class QuestionItem
    {
        public string question { get; set; } = "";
        public string answer { get; set; } = "";
        public string option { get; set; } = "";
    }

    public class OcrLineInfo
    {
        public string Text { get; set; } = "";
        public double Y { get; set; }
    }

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

        private void btnOpenQABank_Click(object sender, EventArgs e)
        {
            using var qaForm = new QABankForm();
            qaForm.ShowDialog(this);
        }

        private List<QuestionItem> LoadQuestionBank()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(baseDir, "questions.json");

            if (!File.Exists(path))
            {
                string? altPath = FindTemplateFile("questions.json");
                if (!string.IsNullOrEmpty(altPath)) path = altPath;
            }

            if (!File.Exists(path)) return new List<QuestionItem>();

            try
            {
                string json = File.ReadAllText(path);
                var items = JsonSerializer.Deserialize<List<QuestionItem>>(json);
                return items ?? new List<QuestionItem>();
            }
            catch
            {
                return new List<QuestionItem>();
            }
        }

        private async Task<(string FullText, List<OcrLineInfo> Lines)> PerformOcrDetailedAsync(Bitmap bitmap)
        {
            var linesInfo = new List<OcrLineInfo>();
            try
            {
                using var stream = new MemoryStream();
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;

                var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream.AsRandomAccessStream());
                using var softwareBitmap = await decoder.GetSoftwareBitmapAsync();

                var ocrEngine = Windows.Media.Ocr.OcrEngine.TryCreateFromUserProfileLanguages() ??
                                 Windows.Media.Ocr.OcrEngine.TryCreateFromLanguage(new Windows.Globalization.Language("en-US"));

                if (ocrEngine == null) return (string.Empty, linesInfo);

                var result = await ocrEngine.RecognizeAsync(softwareBitmap);
                int screenHeight = bitmap.Height;

                foreach (var line in result.Lines)
                {
                    double minY = double.MaxValue;
                    foreach (var word in line.Words)
                    {
                        if (word.BoundingRect.Y < minY) minY = word.BoundingRect.Y;
                    }

                    // 🎯 Lọc bỏ nhiễu: Loại các dòng chữ nằm ở thanh tab/URL trình duyệt (Y < 80px) và Taskbar ở đáy (Y > Height - 50px)
                    double realY = minY == double.MaxValue ? 0 : minY;
                    if (realY >= 80 && realY <= (screenHeight - 50))
                    {
                        linesInfo.Add(new OcrLineInfo { Text = line.Text, Y = realY });
                    }
                }

                string filteredFullText = string.Join(" ", linesInfo.Select(l => l.Text));
                return (filteredFullText, linesInfo);
            }
            catch
            {
                return (string.Empty, linesInfo);
            }
        }

        // 🔍 BỘ THUẬT TOÁN SO KHỚP GẦN ĐÚNG & KHẮC PHỤC LỖI PHÔNG CHỮ OCR DỰ THI
        private static string RemoveVietnameseAccents(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";

            string normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC)
                     .Replace("đ", "d").Replace("Đ", "D");
        }

        private static string NormalizeOcrFontErrors(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";

            string unaccented = RemoveVietnameseAccents(text).ToLowerInvariant();

            // Sửa triệt để các sai sót phông chữ OCR mã Telex/Châu Âu (dd -> d, ü -> u, ö -> o, å -> a, etc.)
            unaccented = unaccented.Replace("dd", "d")
                                   .Replace("ü", "u").Replace("ü", "u").Replace("û", "u").Replace("Ü", "u")
                                   .Replace("ö", "o").Replace("ô", "o").Replace("ö", "o").Replace("Ö", "o")
                                   .Replace("å", "a").Replace("ä", "a").Replace("å", "a").Replace("Å", "a")
                                   .Replace("hifdng", "huong").Replace("hifng", "huong").Replace("hddng", "huong")
                                   .Replace("durgc", "duoc").Replace("dugc", "duoc")
                                   .Replace("khöng", "khong")
                                   .Replace("tén", "ten").Replace("müi", "mui").Replace("nhÜng", "nhung");

            var cleanSb = new StringBuilder();
            foreach (char c in unaccented)
            {
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                {
                    cleanSb.Append(c);
                }
                else
                {
                    cleanSb.Append(' ');
                }
            }

            return cleanSb.ToString();
        }

        private static List<string> ExtractWordTokens(string text)
        {
            string normalized = NormalizeOcrFontErrors(text);
            string[] words = normalized.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Where(w => w.Length >= 2).ToList();
        }

        private static double CalculateWordTokenOverlapScore(string text1, string text2)
        {
            var words1 = ExtractWordTokens(text1);
            var words2 = ExtractWordTokens(text2);

            if (words1.Count == 0 || words2.Count == 0) return 0;

            int matchCount = 0;
            var copy2 = new List<string>(words2);

            foreach (var w1 in words1)
            {
                int idx = copy2.FindIndex(w2 => w2 == w1 || (w1.Length >= 3 && w2.Length >= 3 && (w1.Contains(w2) || w2.Contains(w1))));
                if (idx >= 0)
                {
                    matchCount++;
                    copy2.RemoveAt(idx);
                }
            }

            double ratio1 = (double)matchCount / words1.Count;
            double ratio2 = (double)matchCount / words2.Count;

            return Math.Max(ratio1, ratio2);
        }

        private static double CalculateFuzzySimilarity(string str1, string str2)
        {
            string s1 = NormalizeOcrFontErrors(str1).Replace(" ", "");
            string s2 = NormalizeOcrFontErrors(str2).Replace(" ", "");

            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2)) return 0;
            if (s1 == s2) return 1.0;

            if (s1.Contains(s2) || s2.Contains(s1)) return 0.95;

            var chunks1 = ChunkString(s1, 3);
            var chunks2 = ChunkString(s2, 3);

            int intersectCount = chunks1.Count(w => chunks2.Contains(w));
            int unionCount = chunks1.Union(chunks2).Count();
            double chunkScore = unionCount == 0 ? 0 : (double)intersectCount / unionCount;

            double tokenOverlapScore = CalculateWordTokenOverlapScore(str1, str2);

            return Math.Max(chunkScore, tokenOverlapScore);
        }

        private static HashSet<string> ChunkString(string str, int chunkSize)
        {
            var set = new HashSet<string>();
            if (str.Length < chunkSize)
            {
                set.Add(str);
                return set;
            }
            for (int i = 0; i <= str.Length - chunkSize; i++)
            {
                set.Add(str.Substring(i, chunkSize));
            }
            return set;
        }

        private QuestionItem? MatchQuestionInBank(string recognizedText, List<QuestionItem> qBank, double minSimilarity = 0.35)
        {
            if (string.IsNullOrWhiteSpace(recognizedText) || qBank.Count == 0) return null;

            QuestionItem? bestMatch = null;
            double bestScore = 0;

            foreach (var q in qBank)
            {
                if (string.IsNullOrWhiteSpace(q.question)) continue;

                double score = CalculateFuzzySimilarity(recognizedText, q.question);
                if (score > bestScore && score >= minSimilarity)
                {
                    bestScore = score;
                    bestMatch = q;
                }
            }

            if (bestMatch != null)
            {
                Log($"🔍 [Fuzzy Match] Khớp xấp xỉ câu hỏi thành công! Độ khớp: {(int)(bestScore * 100)}%");
            }

            return bestMatch;
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
            bool useQABank = chkUseQABank.Checked;
            string selectedOption = GetSelectedOptionName();

            string delayInfo = isRandomDelay ? $"Random {delayMin}-{delayMax}ms" : $"{delayMin}ms";
            Log($"📋 Lựa chọn: [{selectedOption}] | Q&A Bank: {useQABank} | Delay: {delayInfo} | Auto Next: {autoNext}");

            try
            {
                var token = _cts.Token;
                await Task.Run(() => RunAutoQuizSolverAsync(selectedOption, useQABank, isRandomDelay, delayMin, delayMax, maxQuestions, autoNext, autoScroll, scrollY, token), token);
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

        private async Task RunAutoQuizSolverAsync(string optionChoice, bool useQABank, bool isRandomDelay, int delayMin, int delayMax, int maxQuestions, bool autoNext, bool autoScroll, int scrollY, CancellationToken token)
        {
            int totalExamsCompleted = 0;
            var qBank = useQABank ? LoadQuestionBank() : new List<QuestionItem>();
            if (useQABank)
            {
                Log($"📚 Đã nạp {qBank.Count} câu hỏi từ ngân hàng câu hỏi (questions.json).");
            }

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

                    // 1️⃣ BƯỚC 1: TỰ ĐỘNG CUỘN MÀN HÌNH XUỐNG ĐẦU TIÊN (Scroll Down FIRST)
                    if (autoScroll)
                    {
                        token.ThrowIfCancellationRequested();
                        Log($"📜 [1] Cuộn màn hình xuống {scrollY}px trước...");
                        ScrollDown(scrollY);

                        // ⏳ Chờ 500ms để trình duyệt cuộn mượt và dừng vị trí hoàn toàn ổn định
                        await Task.Delay(500, token);
                    }

                    // 2️⃣ BƯỚC 2: SAU KHI ĐÃ CUỘN XUỐNG DỪNG HẲN MỚI CHỤP MÀN HÌNH & ĐỌC TEXT OCR!
                    token.ThrowIfCancellationRequested();
                    string screenPath = CaptureScreen();

                    string currentTargetOption = optionChoice;
                    int forcedChoiceIndex = -1;
                    double matchedChoiceY = -1;
                    bool skipAnswerClick = false;

                    string? templatePathOption = null;
                    if (currentTargetOption == "RANDOM")
                    {
                        string[] options = { "A", "B", "C", "D" };
                        string randOpt = options[_rand.Next(options.Length)];
                        templatePathOption = FindTemplateFile(randOpt, $"option_{randOpt}");
                    }
                    else
                    {
                        templatePathOption = FindTemplateFile(currentTargetOption, $"option_{currentTargetOption}");
                    }

                    if (string.IsNullOrEmpty(templatePathOption))
                    {
                        templatePathOption = FindTemplateFile("checkbox_unchecked", "A", "option_A", "radio");
                    }

                    var optionPoints = !string.IsNullOrEmpty(templatePathOption)
                        ? FindMatchingPoints(screenPath, templatePathOption)
                        : new List<OpenCvSharp.Point>();

                    optionPoints.Sort((p1, p2) => p1.Y.CompareTo(p2.Y));
                    double firstCheckboxY = optionPoints.Count > 0 ? optionPoints[0].Y : double.MaxValue;

                    // 3️⃣ BƯỚC 3: ĐỌC TEXT OCR TRÊN MÀN HÌNH MỚI ĐÃ CUỘN
                    if (useQABank && qBank.Count > 0)
                    {
                        try
                        {
                            using var bmp = new Bitmap(screenPath);
                            var (ocrText, ocrLines) = await PerformOcrDetailedAsync(bmp);

                            var questionLinesAbove = ocrLines.Where(l => l.Y < firstCheckboxY).Select(l => l.Text).ToList();
                            string questionTextAbove = string.Join(" ", questionLinesAbove).Trim();

                            if (!string.IsNullOrWhiteSpace(questionTextAbove))
                            {
                                Log($"📖 [OCR Câu Hỏi (Sau khi cuộn)]: \"{questionTextAbove}\"");
                            }
                            else if (!string.IsNullOrWhiteSpace(ocrText))
                            {
                                string cleanPreview = ocrText.Replace("\r", " ").Replace("\n", " ").Trim();
                                while (cleanPreview.Contains("  ")) cleanPreview = cleanPreview.Replace("  ", " ");
                                if (cleanPreview.Length > 150) cleanPreview = cleanPreview.Substring(0, 150) + "...";
                                Log($"📖 [OCR Màn hình (Sau khi cuộn)]: \"{cleanPreview}\"");
                                questionTextAbove = ocrText;
                            }
                            else
                            {
                                Log("📖 [OCR Màn hình]: (Không nhận diện được văn bản)");
                            }

                            // 🔍 SO KHỚP GẦN ĐÚNG VÀ TRÍCH XUẤT TỪ KHÓA CHỐNG SAI LỖI PHÔNG CHỮ OCR
                            var matchedQ = MatchQuestionInBank(questionTextAbove, qBank, 0.35);
                            if (matchedQ == null && !string.IsNullOrWhiteSpace(ocrText))
                            {
                                matchedQ = MatchQuestionInBank(ocrText, qBank, 0.35);
                            }

                            if (matchedQ != null)
                            {
                                Log($"🧠 [AI/OCR] Nhận diện câu hỏi thành công: \"{matchedQ.question.Trim()}\"");

                                if (!string.IsNullOrWhiteSpace(matchedQ.option))
                                {
                                    string opt = matchedQ.option.Trim().ToUpperInvariant();
                                    currentTargetOption = opt;

                                    if (opt == "A") forcedChoiceIndex = 0;
                                    else if (opt == "B") forcedChoiceIndex = 1;
                                    else if (opt == "C") forcedChoiceIndex = 2;
                                    else if (opt == "D") forcedChoiceIndex = 3;

                                    Log($"🎯 [Q&A Bank] Tìm thấy đáp án chuẩn theo chữ cái: [{opt}]");
                                }

                                if (forcedChoiceIndex < 0 && !string.IsNullOrWhiteSpace(matchedQ.answer) && ocrLines.Count > 0)
                                {
                                    double maxAnsScore = 0;
                                    int bestLineIdx = -1;

                                    for (int i = 0; i < ocrLines.Count; i++)
                                    {
                                        double score = CalculateFuzzySimilarity(ocrLines[i].Text, matchedQ.answer);
                                        if (score > maxAnsScore && score >= 0.30)
                                        {
                                            maxAnsScore = score;
                                            bestLineIdx = i;
                                        }
                                    }

                                    if (bestLineIdx >= 0)
                                    {
                                        matchedChoiceY = ocrLines[bestLineIdx].Y;
                                        forcedChoiceIndex = -2;
                                        Log($"🎯 [Q&A Bank] Khớp xấp xỉ chữ đáp án \"{matchedQ.answer}\" với dòng OCR \"{ocrLines[bestLineIdx].Text}\" (Y={matchedChoiceY}, Độ khớp: {(int)(maxAnsScore * 100)}%)");
                                    }
                                }
                            }
                            else
                            {
                                Log($"ℹ️ [OCR] Không thấy câu hỏi trong ngân hàng ➔ Bấm 'Tiếp tục' để qua câu tiếp theo!");
                                skipAnswerClick = true;
                            }
                        }
                        catch
                        {
                            Log($"ℹ️ [OCR] Lỗi nhận diện ➔ Bấm 'Tiếp tục' để qua câu!");
                            skipAnswerClick = true;
                        }
                    }

                    // 4️⃣ Tìm vị trí các đáp án trên màn hình & Click đáp án chính xác (Nếu không bị bỏ qua)
                    if (!skipAnswerClick)
                    {
                        if (optionPoints.Count == 0)
                        {
                            Log($"⚠️ Không tìm thấy ô đáp án trên màn hình hiện tại ➔ Bấm 'Tiếp tục' để qua câu!");
                        }
                        else
                        {
                            int selectedIndex = 0;

                            if (forcedChoiceIndex >= 0)
                            {
                                selectedIndex = Math.Min(forcedChoiceIndex, optionPoints.Count - 1);
                            }
                            else if (forcedChoiceIndex == -2 && matchedChoiceY > 0)
                            {
                                int bestIdx = 0;
                                double minDiff = double.MaxValue;
                                for (int i = 0; i < optionPoints.Count; i++)
                                {
                                    double diff = Math.Abs(optionPoints[i].Y - matchedChoiceY);
                                    if (diff < minDiff)
                                    {
                                        minDiff = diff;
                                        bestIdx = i;
                                    }
                                }
                                selectedIndex = bestIdx;
                                Log($"🎯 Tự động khớp ô chọn thứ #{selectedIndex + 1} khớp với văn bản đáp án.");
                            }
                            else if (currentTargetOption == "RANDOM")
                            {
                                selectedIndex = _rand.Next(optionPoints.Count);
                                string[] optionLabels = { "A", "B", "C", "D" };
                                string label = selectedIndex < optionLabels.Length ? optionLabels[selectedIndex] : $"#{selectedIndex + 1}";
                                Log($"🎲 Chọn ngẫu nhiên đáp án [{label}] (vị trí {selectedIndex + 1}/{optionPoints.Count})");
                            }
                            else if (currentTargetOption == "A") selectedIndex = 0;
                            else if (currentTargetOption == "B") selectedIndex = Math.Min(1, optionPoints.Count - 1);
                            else if (currentTargetOption == "C") selectedIndex = Math.Min(2, optionPoints.Count - 1);
                            else if (currentTargetOption == "D") selectedIndex = Math.Min(3, optionPoints.Count - 1);

                            selectedIndex = Math.Clamp(selectedIndex, 0, optionPoints.Count - 1);
                            using var tmplOpt = Cv2.ImRead(templatePathOption!, ImreadModes.Color);
                            var pt = optionPoints[selectedIndex];
                            int clickX = pt.X + tmplOpt.Width / 2;
                            int clickY = pt.Y + tmplOpt.Height / 2;

                            Log($"🖱️ [2] Click chọn đáp án tại ({clickX}, {clickY})");
                            ClickAt(clickX, clickY);
                        }
                    }

                    int delayMs = GetNextDelay();
                    Log($"⏳ Nghỉ ngẫu nhiên {delayMs}ms trước thao tác tiếp theo...");
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(delayMs, token);

                    // 5️⃣ Tự động bấm nút "Câu tiếp theo" (Next) hoặc "Kết thúc bài thi" (Finish / Submit)
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
                                    examFinished = true;
                                    break;
                                }
                            }

                            Log("⌨️ Không tìm thấy ảnh nút Next/Kết thúc ➔ Gửi phím ENTER.");
                            PressEnterKey();

                            // 💡 Kiểm tra ngay sau khi ấn ENTER xem màn hình đã chuyển sang trang Kết quả có nút "Luyện tất cả" chưa!
                            await Task.Delay(1000, token);
                            string? templatePathRetakeCheck = FindTemplateFile("luyen_tat_ca", "practice_all", "luyen_lai", "retake", "retry", "restart", "lam_lai", "btn_practice");

                            if (!string.IsNullOrEmpty(templatePathRetakeCheck))
                            {
                                string checkScreen = CaptureScreen();
                                var retakePointsCheck = FindMatchingPoints(checkScreen, templatePathRetakeCheck);
                                if (retakePointsCheck.Count > 0)
                                {
                                    Log($"🏆 Ấn ENTER thành công ➔ Đã hoàn thành lần thi #{totalExamsCompleted}!");
                                    examFinished = true;
                                    break;
                                }
                            }
                        }

                        int nextDelayMs = GetNextDelay();
                        Log($"⏳ Nghỉ ngẫu nhiên {nextDelayMs}ms trước thao tác tiếp...");
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(nextDelayMs, token);
                    }
                }

                // ==============================================================
                // 6️⃣ Tự động bấm nút "Luyện tất cả" / "Luyện lại" để lặp lại lần thi mới
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
