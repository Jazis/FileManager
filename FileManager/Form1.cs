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
        private string StartPath = "C:\\";
        private bool _isFile = false;
        public static string url = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }



        private void Form1_Load(object sender, EventArgs e)
        {
            //get a list of the drives
            string[] drives = Environment.GetLogicalDrives();
            foreach (string drive in drives)
            {
                DriveInfo di = new DriveInfo(drive);
                int driveImage;

                switch (di.DriveType)    //set the drive's icon
                {
                    case DriveType.CDRom:
                        driveImage = 3;
                        break;
                    case DriveType.Network:
                        driveImage = 6;
                        break;
                    case DriveType.NoRootDirectory:
                        driveImage = 8;
                        break;
                    case DriveType.Unknown:
                        driveImage = 8;
                        break;
                    default:
                        driveImage = 2;
                        break;
                }
                //+ @":\"
                TreeNode node = new TreeNode(drive.Substring(0, 1) + @":\", driveImage, driveImage);
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
                    textBox1.Text = url;
                    openTree(url);
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


        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                if (e.Node.Nodes[0].Text == "..." && e.Node.Nodes[0].Tag == null)
                {
                    e.Node.Nodes.Clear();

                    //get the list of sub direcotires
                    string[] dirs = Directory.GetDirectories(e.Node.Tag.ToString());

                    foreach (string dir in dirs)
                    {
                        DirectoryInfo di = new DirectoryInfo(dir);
                        TreeNode node = new TreeNode(di.Name, 0, 1);

                        try
                        {
                            //keep the directory's full path in the tag for use later
                            node.Tag = dir;

                            //if the directory has sub directories add the place holder
                            if (di.GetDirectories().Count() > 0)
                                node.Nodes.Add(null, "...", 0, 0);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            //display a locked folder icon
                            node.ImageIndex = 12;
                            node.SelectedImageIndex = 12;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "DirectoryLister",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            e.Node.Nodes.Add(node);
                        }
                    }
                }
            }
        }

        public void test()
        {
            string[] path = @"E:\metasploit-framework\".Split('\\');
            foreach (string elem in path)
            {
                foreach (TreeNode node in treeView1.Nodes)
                {
                    if (node.ToString().Contains(elem.Replace(":", string.Empty)))
                    {
                        string p = node.FullPath;
                    }
                    
                }
                    MessageBox.Show(elem);
            }
            //treeView1.
            //treeView1.Nodes[0].Checked = true;
            MessageBox.Show(treeView1.Nodes.Count.ToString());

        }

        private void button4_Click(object sender, EventArgs e)
        {
            test();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //string[] qwe = treeView1.SelectedNode.FullPath.ToString().Split('\\');
            //string qwe1 = qwe[0].Replace(qwe[0], string.Empty);
            //string url = treeView1.SelectedNode.FullPath.ToString().Split('\\')[0] + ":" + qwe1;
            
            
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            textBox1.Text = treeView1.SelectedNode.FullPath;
            webBrowser1.Navigate(treeView1.SelectedNode.FullPath.Replace("\\\\", "\\"));
        }

        private void openWait(TreeNode node, List<string> path)
        {
            path.RemoveAt(0);
            node.Expand();
            if (path.Count == 0)
                return;
            foreach (TreeNode mynode in node.Nodes)
                if (mynode.Text == path[0])
                    openWait(mynode, path);


        }

        public void openTree(string path)
        {
            List<string> pathList = path.Split('\\').ToList();
            foreach (TreeNode node in treeView1.Nodes)
                if (node.Text == pathList[0])
                    openWait(node, pathList);
        }
    }
}
