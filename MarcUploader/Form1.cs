using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarcUploader
{
    public partial class Form1 : Form
    {
       
        string destination = @"";
        string host = "";
        string username = "";
        string password = "";
        int port = 22;
        Size oldsize;
        Boolean opened = false;

        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);
            oldsize = this.Size;
        }

        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
           

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                Random r = new Random();
                int i2 = r.Next(1000000, 10000000);
                String i = i2.ToString();

                
                
                using (SftpClient client = new SftpClient(host, port, username, password))
                {
                    checkBox1.Checked = true;
                    client.Connect();
                    client.ChangeDirectory("upload/");
                    using (FileStream fs = new FileStream(file, FileMode.Open))
                    {
                        
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(fs, Path.GetFileName(file));
                        client.RenameFile(Path.GetFileName(file), i2 + fi.Extension);
                        label3.Text = "https://marc.enjuu.click/download/" + i2 + fi.Extension;
                        label4.Text = "aka (" + Path.GetFileName(file)+ ")";
                    }
                }
                checkBox1.Checked = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            System.Windows.Forms.Clipboard.SetText(label3.Text);
        }

        private bool dragging;
        private Point pointClicked;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                pointClicked = new Point(e.X, e.Y);
            }
            else
            {
                dragging = false;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point pointMoveTo;
                pointMoveTo = this.PointToScreen(new Point(e.X, e.Y));
                pointMoveTo.Offset(-pointClicked.X, -pointClicked.Y);
                this.Location = pointMoveTo;
            }
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(opened == true)
            {
                this.ClientSize = new System.Drawing.Size(446, 262);
                opened = false;
            }
            else
            {
                this.ClientSize = new System.Drawing.Size(446, 568);
                opened = true;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(label3.Text);
            }
            catch
            {
                MessageBox.Show("No latest File or no Browser installed");
            }
           
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
        }
    }
}
