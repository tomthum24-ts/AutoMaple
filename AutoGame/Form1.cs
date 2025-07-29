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
            for (int j = 1; j <= int.Parse(domainUpDown1.Text); j++)
            {
                if (isCancelled)
                {
                    port.Close();
                    break;
                }
                s++;

                // Tạo combo theo yêu cầu
                int jumpRightCount = rand.Next(7, 11); // 4–8
                int wCount = rand.Next(2, 5);         // 4–6
                int qCount = rand.Next(2, 3);         // 4–6

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

                // Thêm w
                for (int i = 0; i < wCount; i++)
                    combo.Add("w");

                // Thêm q
                for (int i = 0; i < qCount; i++)
                    combo.Add("q");

                // Trộn ngẫu nhiên thứ tự combo
                combo = combo.OrderBy(x => rand.Next()).ToList();


                // Thỉnh thoảng chèn "s"
                sCounter++;
                if (sCounter == 2)
                {
                    combo.Add("s")  ;
                    sCounter = 0;
                }
                foreach (var key in combo)
                {
                    if (isCancelled)
                    {
                        port.Close();
                        break;
                    }


                    int delay = rand.Next(500, 1000); // delay ngẫu nhiên 0.5–1s
                    Thread.Sleep(delay);
                    port.WriteLine(key);
                    Console.WriteLine(">> Sent: " + key);
                    actions.Add(key);
                }

                Thread.Sleep(100); // giữ nhịp vòng lặp đủ chậm để đồng hồ hoạt động chính xác
            }

            port.Close();
            this.Invoke((MethodInvoker)delegate {
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
    }
}
