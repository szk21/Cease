using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;


namespace CeaseUI
{
    public partial class TestingProgressForm : Form
    {
        [DllImport("user32.dll")]
        public static extern byte ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern byte SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        public TestingProgressForm()
        {
            InitializeComponent();
        }

        public int Initialize(int iDutNumber)
        {
            int iLength = 60;
            int iWide = 190;

            this.lProgressBarColor = new List<Color>();
            this.lProgressBarColor.Add(System.Drawing.Color.DarkBlue);
            this.lProgressBarColor.Add(System.Drawing.Color.DarkCyan);
            this.lProgressBarColor.Add(System.Drawing.Color.DarkGoldenrod);
            this.lProgressBarColor.Add(System.Drawing.Color.DarkGreen);
            this.lProgressBarColor.Add(System.Drawing.Color.DarkOrange);
            this.lProgressBarColor.Add(System.Drawing.Color.DarkOrchid);
            this.lProgressBarColor.Add(System.Drawing.Color.DarkRed);
            this.lProgressBarColor.Add(System.Drawing.Color.DarkSalmon);

            this.lProgressBar = new List<VerticalProgressBar>();
            this.lProgressBar.Clear();
            this.Controls.Clear();
            
            if (iDutNumber == 1)
            {
                VerticalProgressBar progressBar = new VerticalProgressBar();
                this.lProgressBar.Add(progressBar);
                this.SuspendLayout();

                progressBar.ForeColor = System.Drawing.Color.Green;
                progressBar.Location = new System.Drawing.Point(0, 0);
                progressBar.Name = "progressBar";
                progressBar.Size = new System.Drawing.Size(iLength, iWide);
                progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;

                progressBar.Value = 0;
                progressBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TestingProgressForm_MouseDown);

                //显示进度数据
                this.Controls.Add(progressBar);
            }
            else if (iDutNumber > 1)//多Dut
            {
                for (int iIndex = 0; iIndex < iDutNumber; iIndex++)
                {
                    VerticalProgressBar progressBar = new VerticalProgressBar();
                    this.lProgressBar.Add(progressBar);
                    this.SuspendLayout();

                    progressBar.ForeColor = this.lProgressBarColor[iIndex];
                    progressBar.Location = new System.Drawing.Point(iIndex * iLength, 0);
                    progressBar.Name = "progressBar" + (iIndex + 1).ToString();
                    progressBar.Size = new System.Drawing.Size(iLength, iWide);
                    progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
                    //progressBar.TabIndex = iIndex;
                    progressBar.Value = 0;
                    progressBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TestingProgressForm_MouseDown);

                    //显示进度数据
                    this.Controls.Add(progressBar);
                }
            }
            return 0;
        }

        public void SetProgressBarMaxSize(int itemsNumber)
        {
            for (int i = 0; i < this.lProgressBar.Count; i++ )
            {
                this.lProgressBar[i].Maximum = itemsNumber * 10;
            }
        }

        delegate void ResetProgressBarCallBack(int iDutNum);
        public void UpdateProgressBar(int iDutNumber)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new ResetProgressBarCallBack(UpdateProgressBar), new object[] { iDutNumber });
            }
            else
            {
                int value = this.lProgressBar[iDutNumber - 1].Value + this.lProgressBar[iDutNumber - 1].Step;
                this.lProgressBar[iDutNumber - 1].Value = value > lProgressBar[iDutNumber - 1].Maximum ? lProgressBar[iDutNumber - 1].Maximum : value;

                this.Update();
            }
        }
        
        public void ResetProgressBar(int iDutNumber)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new ResetProgressBarCallBack(ResetProgressBar), new object[] { iDutNumber });
            }
            else
            {
                lProgressBar[iDutNumber].Value = 0;
            }
        }

        //关联进度窗口和测试结果窗口
        private void TestingProgressForm_MouseClick(object sender, MouseEventArgs e)
        {
            //关联测试结果窗口

        }

        //移动进度窗口
        private void TestingProgressForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
    }
}
