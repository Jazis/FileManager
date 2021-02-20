using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;


namespace FileManager
{
    public partial class Form1 : Form
    {
        public static string url = "";
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] drives = Environment.GetLogicalDrives();
            foreach (string drive in drives)
            {
                DriveInfo di = new DriveInfo(drive);
                //+ @":\"
                TreeNode node = new TreeNode(drive.Substring(0, 1) + @":\");
                node.Tag = drive;

                if (di.IsReady == true)
                    node.Nodes.Add("...");

                treeView1.Nodes.Add(node);
            }
        }

        public void runner()
        {
            while (true)
            {
                if (url == "") { }
                else
                {
                    webBrowser1.Navigate(url);
                    textBox1.Invoke((MethodInvoker)(() => textBox1.Text = url));
                    //SearchNode(treeView1.Nodes, url);
                    url = "";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread0 = new Thread(runner);
            thread0.Start();
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                if (e.Node.Nodes[0].Text == "..." && e.Node.Nodes[0].Tag == null)
                {
                    e.Node.Nodes.Clear();
                    string[] dirs = Directory.GetDirectories(e.Node.Tag.ToString());
                    string[] files = Directory.GetDirectories(e.Node.Tag.ToString());
                    foreach (string dir in dirs)
                    {
                        DirectoryInfo di = new DirectoryInfo(dir);
                        TreeNode node = new TreeNode(di.Name, 0, 1);
                        try
                        {
                            node.Tag = dir;
                            if (di.GetDirectories().Count() > 0)
                                node.Nodes.Add(null, "...", 0, 0);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            node.ImageIndex = 12;
                            node.SelectedImageIndex = 12;
                        }
                        catch { }
                        finally
                        {
                            e.Node.Nodes.Add(node);
                        }

                    }

                }
            }
        }

        //public void test()
        //{
        //    string[] path = @"E:\metasploit-framework\".Split('\\');
        //    foreach (string elem in path)
        //    {
        //        foreach (TreeNode node in treeView1.Nodes)
        //        {
        //            if (node.ToString().Contains(elem.Replace(":", string.Empty)))
        //            {
        //                string p = node.FullPath;
        //            }
                    
        //        }
        //            MessageBox.Show(elem);
        //    }
        //    //treeView1.
        //    //treeView1.Nodes[0].Checked = true;
        //    MessageBox.Show(treeView1.Nodes.Count.ToString());

        //}

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            textBox1.Text = treeView1.SelectedNode.FullPath;
            webBrowser1.Navigate(treeView1.SelectedNode.FullPath.Replace("\\\\", "\\"));
        }

        //public void openTree(string path)
        //{

        //    string elem = "";
        //    for(int i = 0; i< path.Split('\\').Length; i++)
        //    {
        //        elem += path.Split('\\')[i];
        //        treeView1.Invoke((MethodInvoker)(() => treeView1.Nodes.Add(elem)));
        //    }
        //    MessageBox.Show(treeView1.Nodes.Count.ToString());
        //}

        //private void SearchNode(TreeNodeCollection tncoll, string strNode)
        //{
        //    foreach (TreeNode tnode in tncoll)
        //    {
        //        if (tnode.Text.Contains(strNode))
        //        {
        //            tnode.Expand();
        //        }
        //        else
        //        {
        //        }
        //        SearchNode(tnode.Nodes, strNode);
        //    }
        //}

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            textBox1.Text = webBrowser1.Url.ToString().Replace("file:///", "").Replace("/", "\\");
        }
    }
}
