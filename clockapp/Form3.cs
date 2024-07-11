//using ExCSS;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Controls.Ribbon;
using System.Windows.Media.Media3D;
using System.Data;
using clockapp;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms;
using System.Windows.Shapes;
using System.DirectoryServices.ActiveDirectory;
//using Org.BouncyCastle.Bcpg;

namespace calendar
{


    public partial class Form3 : Form
    {
        private Stopwatch stopwatch;
        private Size formSize;
        private int borderSize = 5;
        [System.ComponentModel.Browsable(true)]
        public delegate void datageteventhadler3_1(int x); // Form1 �� show �۽� �ڵ�
        public datageteventhadler3_1 Datasendevent3_1;

        public delegate void datageteventhadler3_2(int x);//form2 .
                                                          // System.Windows.Forms.Label label0;
        public override bool AutoSize { get; set; }

        int cnt = 0;
        int colorcnt = 0;
        bool Flag = true;
        string BackgroundfilePath;
        public Form3()
        {

            InitializeComponent();

            label1.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            //panel6.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;

            // textbox3.Visible = false;

            monthCalendar1.SelectionStart = DateTime.Now;

            //monthCalendar1.SelectionEnd = DateTime.Now;

            this.TopMost = true;
            // trackBar2.BackColor = Color.Transparent;
            CollapsMenu();
            this.Padding = new Padding(borderSize);
            // this.BackColor = Color.FromArgb(98, 102, 244); // border ����
            panel5.Visible = false;

            panel6.AutoScroll = false;
            panel6.HorizontalScroll.Enabled = false;
            panel6.VerticalScroll.Visible = false;
            panel6.HorizontalScroll.Minimum = 0;
            panel6.AutoScroll = true;

            panel6.VerticalScroll.SmallChange = 0;
            panel6.VerticalScroll.LargeChange = 5;

            label2.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
        }



        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        //Dllimport �� �ܺ� dll �Լ� ȣ�� �̰��� win 32 api ȣ�� �Ѵ� ���콺 ĸó�� �����Ѵ�? �𸣰ڴ� ����߾˵�
        //entrypoint�� ȣ���� �Լ� �������ִ°� 

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWind, int wMsg, int wParam, int Iparam);



        private void panel2_MouseDown(object sender, MouseEventArgs e) // ���콺 �ٿ��� ���� �ڵ��� �߰�
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }//���콺 �ٿ� �̺�Ʈ�� �߻��ϸ� ȣ�� 
        //sender �� �̺�Ʈ�� �߻���Ų ��ü�� ��Ÿ����
        //mouseEventargs �� ���콺 �̺�Ʈ ���� ���� ���콺 ��ư Ŭ�� ��ġ �˼�����
        //sendmessage �� Ư�� �����쿡 �޼����� ���� 0x112 �� ������ �̵� 

        protected override void WndProc(ref Message m)
        {
            const int WM_SETREDRAW = 0x0083;
            const int WM_NCCALCSIZE = 0x0083;//Standar Title Bar - Snap Window
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MINIMIZE = 0xF020; //Minimize form (Before)
            const int SC_RESTORE = 0xF120; //Restore form (Before)
            const int WM_NCHITTEST = 0x0084;//Win32, Mouse Input Notification: Determine what part of the window corresponds to a point, allows to resize the form.
            const int resizeAreaSize = 10;
            #region Form Resize
            // Resize/WM_NCHITTEST values
            const int HTCLIENT = 1; //Represents the client area of the window
            const int HTLEFT = 10;  //Left border of a window, allows resize horizontally to the left
            const int HTRIGHT = 11; //Right border of a window, allows resize horizontally to the right
            const int HTTOP = 12;   //Upper-horizontal border of a window, allows resize vertically up
            const int HTTOPLEFT = 13;//Upper-left corner of a window border, allows resize diagonally to the left
            const int HTTOPRIGHT = 14;//Upper-right corner of a window border, allows resize diagonally to the right
            const int HTBOTTOM = 15; //Lower-horizontal border of a window, allows resize vertically down
            const int HTBOTTOMLEFT = 16;//Lower-left corner of a window border, allows resize diagonally to the left
            const int HTBOTTOMRIGHT = 17;//Lower-right corner of a window border, allows resize diagonally to the right
            ///<Doc> More Information: https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-nchittest </Doc>

            if (m.Msg == WM_NCHITTEST)
            { //If the windows m is WM_NCHITTEST

                base.WndProc(ref m);
                if (this.WindowState == FormWindowState.Normal)//Resize the form if it is in normal state
                {
                    if ((int)m.Result == HTCLIENT)//If the result of the m (mouse pointer) is in the client area of the window
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32()); //Gets screen point coordinates(X and Y coordinate of the pointer)                           
                        Point clientPoint = this.PointToClient(screenPoint); //Computes the location of the screen point into client coordinates                          
                        if (clientPoint.Y <= resizeAreaSize)//If the pointer is at the top of the form (within the resize area- X coordinate)
                        {
                            if (clientPoint.X <= resizeAreaSize) //If the pointer is at the coordinate X=0 or less than the resizing area(X=10) in 
                                m.Result = (IntPtr)HTTOPLEFT; //Resize diagonally to the left
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize))//If the pointer is at the coordinate X=11 or less than the width of the form(X=Form.Width-resizeArea)
                                m.Result = (IntPtr)HTTOP; //Resize vertically up
                            else //Resize diagonally to the right
                                m.Result = (IntPtr)HTTOPRIGHT;
                        }
                        else if (clientPoint.Y <= (this.Size.Height - resizeAreaSize)) //If the pointer is inside the form at the Y coordinate(discounting the resize area size)
                        {
                            if (clientPoint.X <= resizeAreaSize)//Resize horizontally to the left
                                m.Result = (IntPtr)HTLEFT;
                            else if (clientPoint.X > (this.Width - resizeAreaSize))//Resize horizontally to the right
                                m.Result = (IntPtr)HTRIGHT;
                        }
                        else
                        {
                            if (clientPoint.X <= resizeAreaSize)//Resize diagonally to the left
                                m.Result = (IntPtr)HTBOTTOMLEFT;

                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize)) //Resize vertically down
                                m.Result = (IntPtr)HTBOTTOM;
                            else //Resize diagonally to the right
                                m.Result = (IntPtr)HTBOTTOMRIGHT;
                        }
                    }
                }
                return;
            }
            #endregion
            if (m.Msg == WM_SETREDRAW && m.WParam.ToInt32() == 1)
            {
                return;
            }
            if (m.Msg == WM_SYSCOMMAND)
            {
                /// <see cref="https://docs.microsoft.com/en-us/windows/win32/menurc/wm-syscommand"/>
                /// Quote:
                /// In WM_SYSCOMMAND messages, the four low - order bits of the wParam parameter 
                /// are used internally by the system.To obtain the correct result when testing 
                /// the value of wParam, an application must combine the value 0xFFF0 with the 
                /// wParam value by using the bitwise AND operator.
                int wParam = (m.WParam.ToInt32() & 0xFFF0);
                if (wParam == SC_MINIMIZE)  //Before
                    formSize = this.ClientSize;
                if (wParam == SC_RESTORE)// Restored form(Before)
                    this.Size = formSize;
            }
            base.WndProc(ref m);
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            AdjustForm();
        }
        private void AdjustForm()
        {
            switch (this.WindowState)
            {
                case FormWindowState.Maximized:
                    this.Padding = new Padding(0, 11, 11, 0);
                    break;
                case FormWindowState.Normal:
                    if (this.Padding.Top != borderSize) ;
                    this.Padding = new Padding(borderSize);
                    break;

            }
        }
        private void exitbotton_MouseClick(object sender, MouseEventArgs e)
        {
            //Application.Exit();

            this.Hide();

            int count = 0;
            for (int i = 0; i < Application.OpenForms.Count; i++)
            {
                if (Application.OpenForms[i].Visible == false)
                    count++;
            }
            //label1.Text = count.ToString(); // Active Forms value :)
            if (count == 3)
            {
                Application.Exit();
            }

        }


        private void maxwindow_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;

            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void btnmenu_MouseClick(object sender, MouseEventArgs e)
        {
            CollapsMenu();

            if (panel5.Visible == true)
            {
                optionMenucollapso();
            }

        }
        private void CollapsMenu()
        {
            if (this.panel1.Width > 104)
            {
                panel1.Width = 40;//40
                panel3.Width = 40;
                pictureBox1.Visible = false;
                btnmenu.Dock = DockStyle.Top;
                foreach (Button menubtn in this.panel3.Controls.OfType<Button>())
                {
                    menubtn.Text = "";
                    menubtn.ImageAlign = ContentAlignment.MiddleLeft;
                    menubtn.Padding = new Padding(0);

                }
            }

            else
            {
                panel1.Width = 105;
                panel3.Width = 105;
                pictureBox1.Visible = true;
                btnmenu.Dock = DockStyle.None;
                foreach (Button menubtn in this.panel3.Controls.OfType<Button>())
                {
                    menubtn.Text = "  " + menubtn.Tag.ToString();
                    menubtn.ImageAlign = ContentAlignment.MiddleLeft;
                    menubtn.Padding = new Padding(0, 0, 0, 0);

                }
            }

        }



        private void trackbutton_Click(object sender, EventArgs e)
        {
            if (trackBar2.Visible == true)
            {
                trackbutton.IconChar = FontAwesome.Sharp.IconChar.Egg;
                trackBar2.Visible = false;
            }
            else
            {
                trackbutton.IconChar = FontAwesome.Sharp.IconChar.Dove;
                trackBar2.Visible = true;
            }
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            this.Opacity = trackBar2.Value * 0.01;
        }



        private void optionbutton_Click(object sender, EventArgs e)
        {
            if (panel5.Visible == true)
            {
                panel5.Visible = false;
            }
            else
            {
                panel5.Visible = true;
            }
            optionMenucollapso();

        }

        private void optionMenucollapso()
        {
            if (this.panel1.Width == 40)
            {
                panel5.Width = 37;
                foreach (Button menubtn in this.panel5.Controls.OfType<Button>())
                {
                    menubtn.Text = "";
                    menubtn.ImageAlign = ContentAlignment.MiddleLeft;
                    menubtn.Padding = new Padding(0);

                }
            }
            else
            {
                panel5.Width = 105;
                foreach (Button menubtn in this.panel5.Controls.OfType<Button>())
                {
                    menubtn.Text = "  " + menubtn.Tag.ToString();
                    menubtn.ImageAlign = ContentAlignment.MiddleLeft;
                    menubtn.Padding = new Padding(0, 0, 0, 0);
                }
            }
        }


        private void sizebutton_Click(object sender, EventArgs e)
        {
            // 

            if (monthCalendar1.Visible == true)
            {
                monthCalendar1.Visible = false;
                //label1.Location = new Point(19, 32);
                //trackBar2.Location = new Point(197, 16);
                panel6.Location = new Point(19, 30);
                panel6.Size = new Size(334, 400);
            }
            else
            {
                monthCalendar1.Visible = true;
                //label1.Location = new Point(19, 226);
                //trackBar2.Location = new Point(197, 213);
                panel6.Location = new Point(19, 203);
                panel6.Size = new Size(334, 220);
            }

        }

        private void colorbutton_Click(object sender, EventArgs e)
        {
            colorcnt++;

            if (colorcnt == 3)
            {
                colorcnt = 0;
            }

            if (colorcnt == 2)
            {
                panel3.BackColor = Color.Linen;
                panel5.BackColor = Color.Linen;
                this.BackColor = Color.LightPink;
                trackBar2.BackColor = Color.LightPink;
            }
            else if (colorcnt == 1)
            {
                panel3.BackColor = Color.LightSkyBlue;
                panel5.BackColor = Color.LightSkyBlue;
                this.BackColor = Color.DeepSkyBlue;
                trackBar2.BackColor = Color.PaleTurquoise;

            }
            else if (colorcnt == 0)
            {
                panel3.BackColor = Color.LightGray;
                panel5.BackColor = Color.LightGray;
                this.BackColor = Color.Gray;

                trackBar2.BackColor = Color.LightGray;

            }

        }
        private void Dataget(int x)
        {
            this.Visible = true;
        }


        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            label1.Text = monthCalendar1.SelectionStart.ToString("yy/MM/dd");
            Flag = false;
            readfile();
        }

        private void label1_Paint(object sender, PaintEventArgs e)
        {
            if (Flag == true)
            {
                //label1.Text = DateTime.Now.ToString("yy/MM/dd");
                label1.Text = monthCalendar1.SelectionStart.ToString("yy/MM/dd");

            }
        }


        private void iconButton1_Click(object sender, EventArgs e)
        {
            int x = 1;
            Datasendevent3_1(x);
        }

        private void readfile()
        {
            string path = "C:\\clocktxtfolder\\clockapptest.txt";

            if (System.IO.File.Exists(path) == false)
            {
                label2.Visible = false;
                label5.Visible = false;
                label6.Visible = false;

                return;
            }
            label2.Visible = true;
            label5.Visible = true;
            label6.Visible = true;


            StreamReader sr2 = new StreamReader(File.Open(path, FileMode.Open, FileAccess.ReadWrite));
            label4.Text = "Record your daily life!";

            string line = "";
            string linecheck;
            int o = 0; int c = 1;
            int checcnt = 0;
            //label4.Text = sr2.ReadLine();
            while (true)
            {
                linecheck = sr2.ReadLine();

                if (linecheck == label1.Text)
                {
                    checcnt++;
                    o = 1;
                    c++;
                    continue;
                }
                if (linecheck == null)
                {
                    break;
                }
                if (o == 1 && c == 2)
                {
                    line += linecheck;
                    line += "\n";
                    line += "\n";
                    line += "   ";
                    c++;
                    continue;
                }
                if (o == 1 && c == 3)
                {

                    line += linecheck;
                    line += "\n";
                    line += "\n";
                    c = 1;
                    o = 0;
                    continue;
                }
            }
            if (checcnt == 0)
            {
                label4.Text = label4.Text;
                label2.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
            }
            else
            {
                label4.Text = line;
            }
            sr2.Close();
        }
        private void label4_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                DialogResult result = this.color3.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // Set form background to the selected color.
                    label4.ForeColor = color3.Color;
                }
            }
        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            if (panel5.Visible == true)
            {
                panel5.Visible = false;
            }

            if (e.Button == MouseButtons.Right)
            {
                DialogResult result = this.color3.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // Set form background to the selected color.

                    this.BackColor = color3.Color;
                }
            }
        }
        private void panel3_MouseClick(object sender, MouseEventArgs e)
        {
            if (panel5.Visible == true)
            {
                panel5.Visible = false;
            }

            if (e.Button == MouseButtons.Right)
            {

                DialogResult result = this.color3.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // Set form background to the selected color.
                    panel3.BackColor = color3.Color;
                    panel5.BackColor = color3.Color;
                }
            }
        }

        private void panel4_DragDrop(object sender, DragEventArgs e)
        {
            string[] filess = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (filess.Length > 0 && File.Exists(filess[0]))
            {
                BackgroundfilePath = filess[0];

                pictureBox2.Visible = true;

                // label3.Parent = pictureBox2;
                //  label1.Parent = pictureBox2;

                pictureBox2.Image = Bitmap.FromFile(BackgroundfilePath);

            }

        }

        private void panel4_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void backgroundbutton_Click(object sender, EventArgs e)
        {
            if (this.TopMost == true)
            {
                this.TopMost = false;
            }
            else
            {
                this.TopMost = true;

            }
        }

        private void iconButton1_Click_1(object sender, EventArgs e)
        {
            if (panel6.Visible == true)
            {
                panel6.Visible = false;
            }
            else
            {
                panel6.Visible = true;
            }
        }
        private void trackBar2_MouseDown(object sender, MouseEventArgs e)
        {

            if (panel5.Visible == true)
            {
                panel5.Visible = false;
            }

            if (e.Button == MouseButtons.Right)
            {

                DialogResult result = this.color3.ShowDialog();
                if (result == DialogResult.OK)
                {

                    trackBar2.BackColor = color3.Color;

                }

            }
        }
        private void label1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                DialogResult result = this.color3.ShowDialog();
                DialogResult result2 = fontDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // Set form background to the selected color.
                    label1.ForeColor = color3.Color;
                }

                if (result2 == DialogResult.OK)
                {
                    // Set form background to the selected color.
                    label1.Font = fontDialog1.Font;
                }


            }
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (panel5.Visible == true)
            {
                panel5.Visible = false;
            }

        }

        private void panel4_MouseClick(object sender, MouseEventArgs e)
        {
            if (panel5.Visible == true)
            {
                panel5.Visible = false;
            }

        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (pictureBox2.Image == null)
            {
                BackgroundfilePath = "";
            }
            clockapp.Properties.Settings.Default.backgroundimg3 = BackgroundfilePath;
            clockapp.Properties.Settings.Default.bordercolor3 = this.BackColor;
            clockapp.Properties.Settings.Default.panelcolor3 = panel3.BackColor;
            clockapp.Properties.Settings.Default.label1color3 = label1.ForeColor;
            clockapp.Properties.Settings.Default.label4color3 = label4.ForeColor;
            clockapp.Properties.Settings.Default.form3location = this.Location;
            clockapp.Properties.Settings.Default.form3label1font = label1.Font;



            clockapp.Properties.Settings.Default.track3 = trackBar2.BackColor;

            clockapp.Properties.Settings.Default.Save();

        }

        private void Form3_Load(object sender, EventArgs e)
        {

            BackgroundfilePath = clockapp.Properties.Settings.Default.backgroundimg3;

            this.BackColor = clockapp.Properties.Settings.Default.bordercolor3;
            this.panel3.BackColor = clockapp.Properties.Settings.Default.panelcolor3;
            this.panel5.BackColor = clockapp.Properties.Settings.Default.panelcolor3;
            label1.ForeColor = clockapp.Properties.Settings.Default.label1color3;
            label4.ForeColor = clockapp.Properties.Settings.Default.label4color3;
            this.Location = clockapp.Properties.Settings.Default.form3location;
            label1.Font = clockapp.Properties.Settings.Default.form3label1font;

            trackBar2.BackColor = clockapp.Properties.Settings.Default.track3;

            if (System.IO.File.Exists(BackgroundfilePath) && BackgroundfilePath != "")
            {
                pictureBox2.Visible = true;


                pictureBox2.Image = Bitmap.FromFile(BackgroundfilePath);
            }




        }

        private void initial_button_Click(object sender, EventArgs e)
        {
            label1.Font = new Font("���õ������V Bold", 24, FontStyle.Bold);
            label1.ForeColor = Color.Black;
            trackBar2.BackColor = Color.LightGray;
            pictureBox2.Image = null;
            panel4.BackgroundImage = null;
            label4.ForeColor = Color.Black;


        }
    }
}



