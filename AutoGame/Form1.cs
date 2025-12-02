using Gma.System.MouseKeyHook;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.IO.Ports;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace AutoGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            folderPath = AppDomain.CurrentDomain.BaseDirectory;
            filePath = Path.Combine(folderPath, "output.json");
            this.Load += Form1_Load;
        }
        List<KeyAction> recordedActions = new List<KeyAction>();
        Stopwatch timer = new Stopwatch();
        List<string> actions = new List<string>();
        IKeyboardMouseEvents globalHook;

        string folderPath;
        string filePath;
        bool isStop;
        private volatile bool isCancelled = false;
        private System.Windows.Forms.Timer skillTimer;
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            recordedActions.Add(new KeyAction
            {
                Key = e.KeyCode,
                IsKeyDown = true,
                TimeOffset = timer.Elapsed
            });
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var list = new List<string> { "luadoc", "sung", "danhtay", "LenXuong" };

                this.Invoke((MethodInvoker)(() =>
                {
                    comboBox1.DataSource = list;
                }));
            });
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            recordedActions.Add(new KeyAction
            {
                Key = e.KeyCode,
                IsKeyDown = false,
                TimeOffset = timer.Elapsed
            });
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //Thread t = new Thread(Run);
            //t.Start(); // chạy thread song song
            button1.Enabled = false;
            button3.Enabled = true;
            isCancelled = false;
            var port = new SerialPort(textBox1.Text, 9600);
            port.Open();
            Thread thread = new Thread(() =>
            {
                Run(port);
            }
             );
            thread.IsBackground = false;
            thread.Start();

        }
        public void Run(SerialPort port)
        {
            string selected = string.Empty;
            this.Invoke((MethodInvoker)(() =>
            {
                selected = comboBox1.SelectedValue?.ToString();
            }));

            // Gán 1 lần duy nhất sự kiện nhận dữ liệu
            port.DataReceived += (sender, e) =>
            {
                var sp = (SerialPort)sender;
                string received = sp.ReadExisting();
                Console.WriteLine("Received: " + received);
            };

            Stopwatch skillTimer = new Stopwatch();
            skillTimer.Start();

            Random rand = new Random();
            int s = 0;
            int isJump_left = 0;
            var sCounter = 0;
            int jump = int.Parse(Jump.Text);
            if (selected == "luadoc")
            {
                for (int j = 1; j <= int.Parse(domainUpDown1.Text); j++)
                {
                    if (isCancelled)
                    {
                        port.Close();
                        break;
                    }
                    s++;

                    // Tạo combo theo yêu cầu 
                    int jumpRightCount = rand.Next(jump, jump); // 4–8
                    int wCount = rand.Next(9, 12);         // 4–6
                    //int qCount = rand.Next(3, 4);         // 4–6

                    List<string> combo = new List<string>();

                    // Thêm JUMP_RIGHT
                    if (isJump_left % 2 == 1)
                    {
                        for (int i = 0; i < jumpRightCount; i++)
                        {
                            combo.Add("JUMP_RIGHT");
                            combo.Add("w");
                            combo.Add("w");
                        }
                        isJump_left++;
                    }
                    else
                    {
                        for (int i = 0; i < jumpRightCount; i++)
                        {
                            combo.Add("JUMP_LEFT");
                            combo.Add("w");
                            combo.Add("w");
                        }
                        isJump_left++;
                    }

                    // Thêm w
                    //for (int i = 0; i < wCount; i++)
                    //combo.Add("w");


                    // Thêm q
                    //for (int i = 0; i < qCount; i++)
                    //    combo.Add("q");

                    // Trộn ngẫu nhiên thứ tự combo

                    sCounter++;
                    if (sCounter == 10)
                    {
                        combo.Add("S");
                        combo.Add("A");
                        combo.Add("D");
                        combo.Add("F");
                        sCounter = 0;
                    }
                    combo = combo.OrderBy(x => rand.Next()).ToList();
                    foreach (var key in combo)
                    {
                        if (isCancelled)
                        {
                            port.Close();
                            break;
                        }
                        // Thỉnh thoảng chèn "s"

                        int delay = rand.Next(200, 400); // delay ngẫu nhiên 0.5–1s
                        Thread.Sleep(delay);
                        port.WriteLine(key);
                        Console.WriteLine(">> Sent: " + key);
                        if (key == "A" || key == "S" || key == "D" || key == "F") Thread.Sleep(1000);
                        actions.Add(key);
                    }

                    Thread.Sleep(100); // giữ nhịp vòng lặp đủ chậm để đồng hồ hoạt động chính xác
                }

            }
            else if (selected == "LenXuong")
            {
                for (int j = 1; j <= int.Parse(domainUpDown1.Text); j++)
                {
                    if (isCancelled)
                    {
                        port.Close();
                        break;
                    }
                    s++;

                    // Tạo combo theo yêu cầu 
                    int jumpRightCount = rand.Next(jump, jump); // 4–8
                    int wCount = rand.Next(2, 3);         // 4–6
                    //int qCount = rand.Next(3, 4);         // 4–6

                    List<string> combo = new List<string>();

                    // Thêm JUMP_RIGHT
                    for (int i = 0; i < jumpRightCount; i++)
                    {
                        // Hit LEFT x2
                        combo.Add("LEFT");
                        for (int j1 = 0; j1 < wCount; j1++)
                            combo.Add("w");


                        // Hit RIGHT x2
                        combo.Add("RIGHT");
                        for (int j2 = 0; j2 < wCount; j2++)
                            combo.Add("w");



                        // Alternate jump
                        if (isJump_left % 2 == 1)
                            combo.Add("JUMP_DOWN");

                        else
                            combo.Add("JUMP_UP");

                        isJump_left++;
                    }
                    
                    
                    // Thêm w
                    //for (int i = 0; i < wCount; i++)
                    //combo.Add("w");


                    // Thêm q
                    //for (int i = 0; i < qCount; i++)
                    //    combo.Add("q");

                    // Trộn ngẫu nhiên thứ tự combo

                    sCounter++;
                    if (sCounter == 15)
                    {
                        combo.Add("S");
                        combo.Add("A");
                        combo.Add("D");
                        combo.Add("F");
                        sCounter = 0;
                    }
                    combo = combo.OrderBy(x => rand.Next()).ToList();
                    foreach (var key in combo)
                    {
                        if (isCancelled)
                        {
                            port.Close();
                            break;
                        }
                        // Thỉnh thoảng chèn "s"

                        int delay = rand.Next(300, 500); // delay ngẫu nhiên 0.5–1s
                        Thread.Sleep(delay);
                        port.WriteLine(key);
                        if (key == "A" || key == "S" || key == "D" || key == "F") Thread.Sleep(1000);
                        Console.WriteLine(">> Sent: " + key);
                    }

                    Thread.Sleep(200); // giữ nhịp vòng lặp đủ chậm để đồng hồ hoạt động chính xác
                }

            }
            else if (selected == "danhtay")
            {
                for (int j = 1; j <= int.Parse(domainUpDown1.Text); j++)
                {
                    if (isCancelled)
                    {
                        port.Close();
                        break;
                    }
                    s++;

                    // Tạo combo theo yêu cầu 
                    int jumpRightCount = rand.Next(1, 1); // 4–8
                    int wCount = rand.Next(2, 2);         // 4–6
                    List<string> combo = new List<string>();

                    // Thêm JUMP_RIGHT
                    if (isJump_left % 2 == 1)
                    {
                        for (int i = 0; i < jumpRightCount; i++)
                            combo.Add("JUMP_RIGHT");
                        isJump_left++;
                    }
                    else
                    {
                        for (int i = 0; i < jumpRightCount; i++)
                            combo.Add("JUMP_LEFT");
                        isJump_left++;
                    }
                    sCounter++;
                    if (sCounter == 5)
                    {
                        combo.Add("s");
                        combo.Add("A");
                        combo.Add("D");
                        sCounter = 0;
                    }
                    // Thêm w
                    for (int i = 0; i < wCount; i++)
                        combo.Add("w");

                    // Trộn ngẫu nhiên thứ tự combo

                    combo = combo.OrderBy(x => rand.Next()).ToList();
                    foreach (var key in combo)
                    {
                        if (isCancelled)
                        {
                            port.Close();
                            break;
                        }
                        // Thỉnh thoảng chèn "s"

                        int delay = rand.Next(300, 300); // delay ngẫu nhiên 0.5–1s
                        Thread.Sleep(delay);
                        port.WriteLine(key);
                        Console.WriteLine(">> Sent: " + key);
                        if (key == "A" || key == "S" || key == "D") Thread.Sleep(1000);
                        actions.Add(key);
                    }

                    Thread.Sleep(100); // giữ nhịp vòng lặp đủ chậm để đồng hồ hoạt động chính xác
                }

            }
            else
            {
                for (int j = 1; j <= int.Parse(domainUpDown1.Text); j++)
                {
                    if (isCancelled)
                    {
                        port.Close();
                        break;
                    }
                    s++;

                    // Tạo combo theo yêu cầu 
                    int jumpRightCount = rand.Next(3, 4); // 4–8
                    int wCount = rand.Next(8, 12);         // 4–6
                    int qCount = rand.Next(4, 6);         // 4–6

                    List<string> combo = new List<string>();

                    // Thêm JUMP_RIGHT
                    if (isJump_left % 2 == 1)
                    {
                        for (int i = 0; i < jumpRightCount; i++)
                        {
                            combo.Add("JUMP_RIGHT_SUNG");
                        }

                        isJump_left++;
                    }
                    else
                    {
                        for (int i = 0; i < jumpRightCount; i++)
                        {
                            combo.Add("JUMP_LEFT_SUNG");
                        }

                        isJump_left++;
                    }

                    // Thêm w
                    for (int i = 0; i < wCount; i++)
                        combo.Add("w");
                    // Thêm q
                    for (int i = 0; i < qCount; i++)
                        combo.Add("q");
                    //for (int i = 0; i < qCount; i++)
                    //    combo.Add("A");

                    // Trộn ngẫu nhiên thứ tự combo
                    combo = combo.OrderBy(x => rand.Next()).ToList();

                    sCounter++;
                    if (sCounter == 3)
                    {
                        combo.Add("S");
                        combo.Add("A");
                        combo.Add("D");
                        sCounter = 0;
                    }
                    foreach (var key in combo)
                    {
                        if (isCancelled)
                        {
                            port.Close();
                            break;
                        }
                        // Thỉnh thoảng chèn "s"

                        int delay = rand.Next(500, 700); // delay ngẫu nhiên 0.5–1s
                        Thread.Sleep(delay);
                        port.WriteLine(key);
                        Console.WriteLine(">> Sent: " + key);
                        if (key == "A" || key == "S" || key == "D") Thread.Sleep(1000);
                        actions.Add(key);
                    }

                    Thread.Sleep(100); // giữ nhịp vòng lặp đủ chậm để đồng hồ hoạt động chính xác
                }

            }

            port.Close();
            this.Invoke((MethodInvoker)delegate
            {
                button1.Enabled = true;
                button3.Enabled = false;
            });
            MessageBox.Show("Complete");

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            isCancelled = true;
            button1.Enabled = true;
            button3.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ExportDataGridViewToJson(dataGridView1, filePath);
        }
        public void ExportDataGridViewToJson(DataGridView dgv, string filePath)
        {
            var rows = new List<Dictionary<string, object>>();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (!row.IsNewRow)
                {
                    var dict = new Dictionary<string, object>();

                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        dict[col.HeaderText] = row.Cells[col.Index].Value;
                    }

                    rows.Add(dict);
                }
            }

            string json = JsonConvert.SerializeObject(rows, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
