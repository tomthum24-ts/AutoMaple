using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices;

namespace AutoCheckBox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // 🖱️ Khai báo hàm click thật bằng WinAPI
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);
        const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        const uint MOUSEEVENTF_LEFTUP = 0x04;

        static void ClickAt(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=== Auto Click Checkbox ===");
            Console.WriteLine("Đang chụp màn hình và tìm checkbox...");

            // 1️⃣ Chụp ảnh màn hình
            Bitmap screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(0, 0, 0, 0, screenshot.Size);
            }

            string screenPath = "screen.png";
            screenshot.Save(screenPath);

            // 2️⃣ Đọc ảnh bằng OpenCV
            using var img = Cv2.ImRead(screenPath, ImreadModes.Color);
            using var template = Cv2.ImRead("checkbox_unchecked.png", ImreadModes.Color);

            if (template.Empty())
            {
                Console.WriteLine("❌ Không tìm thấy file checkbox_unchecked.png!");
                Console.WriteLine("→ Hãy để file ảnh mẫu cùng thư mục với .exe");
                return;
            }

            // 3️⃣ So khớp ảnh
            using var result = new Mat();
            Cv2.MatchTemplate(img, template, result, TemplateMatchModes.CCoeffNormed);

            double threshold = 0.8;
            List<OpenCvSharp.Point> foundPoints = new();

            // 4️⃣ Tìm tất cả vị trí khớp
            for (int y = 0; y < result.Rows; y++)
            {
                for (int x = 0; x < result.Cols; x++)
                {
                    if (result.At<float>(y, x) >= threshold)
                    {
                        foundPoints.Add(new OpenCvSharp.Point(x, y));
                    }
                }
            }

            // 5️⃣ Lọc bớt điểm trùng gần nhau
            int minDistance = 20;
            List<OpenCvSharp.Point> filteredPoints = new();
            foreach (var pt in foundPoints)
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

            Console.WriteLine($"✅ Tìm thấy {filteredPoints.Count} checkbox!");

            // 6️⃣ Click từng checkbox
            foreach (var pt in filteredPoints)
            {
                int clickX = pt.X + template.Width / 2;
                int clickY = pt.Y + template.Height / 2;

                Console.WriteLine($"🖱️ Click tại ({clickX}, {clickY})");
                ClickAt(clickX, clickY);
                Thread.Sleep(300);
            }

            Console.WriteLine("🎯 Hoàn tất!");
        }
    }

}
