using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace FileManager
{
    public partial class Form2 : Form
    {
        public static string disk = "";
        public static string sought = "";
        public static bool _isWorking = false;
        public static bool _isRegex = false;
        public static int foldersChecked = 0;
        public static int filesChecked = 0;
        public static Stopwatch sw = new Stopwatch();
        public static List<string> ls = new List<string>();
        public Thread thread0;

        public Form2()
        {
            InitializeComponent();
        }

        public void DiscoverDirs(string where, List<string> files)
        {
            try
            {
                files.AddRange(Directory.GetFiles(where));
                foreach (string file in Directory.GetFiles(where))
                {
                    if (_isWorking == false) { Thread.CurrentThread.Abort(); }
                    label5.Invoke((MethodInvoker)(() => listBox1.Items.Count.ToString()));
                    if (_isRegex == true)
                    {
                        Regex regex = new Regex(@textBox1.Text);
                        MatchCollection matches = regex.Matches(file);
                        if (matches.Count > 0)
                        {
                            foreach (Match match in matches)
                            {
                                listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add(match.Value)));
                                filesChecked += 1;
                                label5.Invoke((MethodInvoker)(() => listBox1.Items.Count.ToString()));
                                label4.Invoke((MethodInvoker)(() => label4.Text = $"Files checked: {filesChecked}"));
                            }
                        }
                        else { }
                    }
                    else
                    {
                        if (file.Contains(textBox1.Text))
                        {
                            listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add(file)));
                            ls.Add(file);
                            
                        }
                    }
                    filesChecked += 1;
                    label5.Invoke((MethodInvoker)(() => listBox1.Items.Count.ToString()));
                    label4.Invoke((MethodInvoker)(() => label4.Text = $"Files checked: {filesChecked}"));

                }
                foreach (var dir in Directory.GetDirectories(where))
                {
                    if (_isRegex == true)
                    {
                        Regex regex = new Regex(@textBox1.Text);
                        MatchCollection matches = regex.Matches(dir);
                        if (matches.Count > 0)
                        {
                            foreach (Match match in matches)
                            {
                                listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add(match.Value)));
                            }
                        }
                        else { }
                    }
                    else
                    {
                        if (dir.Contains(textBox1.Text))
                        {
                            listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add(dir)));
                            ls.Add(dir);
                            foldersChecked += 1;
                            label5.Invoke((MethodInvoker)(() => listBox1.Items.Count.ToString()));
                            label3.Invoke((MethodInvoker)(() => label3.Text = $"Folders checked: {filesChecked}"));
                        }
                    }
                    foldersChecked += 1;
                    label5.Invoke((MethodInvoker)(() => listBox1.Items.Count.ToString()));
                    label3.Invoke((MethodInvoker)(() => label3.Text = $"Folders checked: {foldersChecked}"));
                    DiscoverDirs(dir, files);
                }
            }
            catch { }
            label6.Invoke((MethodInvoker)(() => label6.Text = "Time(sec):" + (sw.ElapsedMilliseconds / 1000).ToString()));
        }

        public void searching()
        {
            //MessageBox.Show(disk);
            var list = new List<string>();
            DiscoverDirs(disk, list);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            disk = listBox2.SelectedItem.ToString();
            if (_isWorking == false)
            {
                sw.Start();
                _isWorking = true;
                if (textBox1.Text.Contains(@"\") ||
                textBox1.Text.Contains(@"[") ||
                textBox1.Text.Contains(@"/") ||
                textBox1.Text.Contains(@"]"))
                {
                    _isRegex = true;
                    sought = textBox1.Text;

                }
                else
                {
                    _isRegex = false;
                    sought = textBox1.Text;
                }
                thread0 = new Thread(searching);
                thread0.IsBackground = true;
                thread0.Start();
            }
            else
            {
                sw.Stop();
                label6.Text = "Time(sec):" + (sw.ElapsedMilliseconds / 1000).ToString();
                _isWorking = false;
                MessageBox.Show("Searching stopped!\n Waiting for the end of the stream");
            }
        }

        public void settings()
        {
            using (StreamReader str = new StreamReader("cfg.ini", Encoding.Default))
            {
                string source = str.ReadLine();
                List<string> lines = new List<string>();
                foreach (string line in File.ReadAllLines("cfg.ini"))
                {
                    if (line.Contains("[LSEARCH]"))
                    {
                        textBox1.Invoke((MethodInvoker)(() => textBox1.Text = line.Split('=')[1]));
                    }
                    if (line.Contains("[PATHS]"))
                    {
                        if (listBox2.Items.Contains(line.Split('=')[1])) { }
                        else { listBox2.Invoke((MethodInvoker)(() => listBox2.Items.Add(line.Split('=')[1]))); }
                    }
                }

            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try { settings(); } catch { }
            DriveInfo[] driveInfo = DriveInfo.GetDrives();
            foreach (DriveInfo elem in driveInfo)
            {
                listBox2.Items.Add(elem.ToString());
            }
            listBox2.SetSelected(0, true);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.Delete("cfg.ini");
            //File.Create("cfg.ini");
            using (StreamWriter streamWriter = new StreamWriter("cfg.ini"))
            {
                streamWriter.WriteLine($"[LSEARCH]={textBox1.Text}");
                foreach (string item in listBox2.Items)
                {
                    streamWriter.WriteLine($"[PATHS]={item}");
                }
            }
            try { thread0.Abort(); } catch { }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Form1.url = listBox1.SelectedItem.ToString();
            }
            catch{ }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add(textBox2.Text);
        }
    }
}
