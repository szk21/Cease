using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net.NetworkInformation;
using System.IO;
using System.Threading;
using System.Configuration.Install;
using System.Text.RegularExpressions;
using WinSCP;

namespace StartCease
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("Shlwapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string psz1, string psz2);

        List<ComboBox> m_listCmb;
        List<string> m_listItem;

        IniFile m_Startup;
        IniFile m_Config;
        private string m_strStartupFile = "\\Startup.ini";
        private string m_strConfigFile = "\\config.ini";

        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void KillExistProcess(List<string> _processList)
        {
            foreach (var p in Process.GetProcesses())
            {
                foreach (var pName in _processList)
                {
                    if (p.ProcessName.Contains(pName))
                    {
                        p.Kill();//结束进程
                    }
                }
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            List<string> listProcess = new List<string>() { "Cease.Startup", "Audio-Analyser", "cmd" };
            KillExistProcess(listProcess);

            for (int i = 0; i < m_listCmb.Count; i++)
            {
                m_Startup.WriteToFile("START", m_listItem[i], m_listCmb[i].Text);
            }
            m_Config.WriteToFile("START", "VERSION", comboBoxToolVer.Text);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = Application.StartupPath + "\\" + comboBoxToolVer.Text + "\\Cease.Startup.exe"; //"输入完整的路径"
            process.StartInfo.Arguments = ""; //启动参数 
            process.Start();
            Application.ExitThread();
            Application.Exit();
          //  WindowState = FormWindowState.Minimized;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var downloadForm = new FormDownload();
                downloadForm.ShowDialog();

                InitForm();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool InitForm()
        {
            //check exist tool version
            string strToolVer = m_Config.ReadFromFile("START", "VERSION", "Debug");
            comboBoxToolVer.Items.Clear();
            DirectoryInfo rootFolder = new DirectoryInfo(Application.StartupPath);
            string[] dirs = new string[rootFolder.GetDirectories().Length];
            for (int i = 0; i < dirs.Length; i++ )
            {
                dirs[i] = rootFolder.GetDirectories()[i].Name;
            }
            Array.Sort(dirs, StrCmpLogicalW);
            for (int i = dirs.Length - 1; i >= 0; i-- )
            {
                comboBoxToolVer.Items.Add(dirs[i]);
            }            
            comboBoxToolVer.Text = comboBoxToolVer.Items.Contains(strToolVer) ? strToolVer : dirs[dirs.Length - 1];

            //check startup.ini
            if (!File.Exists(Application.StartupPath + "\\" + comboBoxToolVer.Text + m_strStartupFile))
            {
                MessageBox.Show(m_strStartupFile + " not exist!");
                return false;
            }
            m_Startup = new IniFile(Application.StartupPath + "\\" + comboBoxToolVer.Text + m_strStartupFile);

            //initial form
            m_listCmb = new List<ComboBox> { cmbStation, cmbModel, cmbDut };
            m_listItem = new List<string> { "STATION", "PROJECT", "DUT" };
            for (int i = 0; i < m_listCmb.Count; i++)
            {
                string strItem = m_Startup.ReadFromFile("START", m_listItem[i], "");
                string strItems = m_Startup.ReadFromFile("SOURCE", m_listItem[i] + "S", "");

                m_listCmb[i].Items.Clear();
                foreach (var _item in strItems.Split(','))
                {
                    m_listCmb[i].Items.Add(_item);
                }
                m_listCmb[i].Text = strItem;
            }

            buttonUpdate.Enabled = 
                "1" == m_Config.ReadFromFile("FTP", "ENABLE", "0") ? true : false;

            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //check config.ini
            if (!File.Exists(Application.StartupPath + "\\" + m_strConfigFile))
            {
                MessageBox.Show(m_strConfigFile + " not exist!");
                Close();
                return;
            }
            m_Config = new IniFile(Application.StartupPath + "\\" + m_strConfigFile);
            
            if (!InitForm())
            {
                Close();
                return;
            }
        }

        private void notifyIconCease_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                // 还原窗体显示    
                WindowState = FormWindowState.Normal;
                // 激活窗体并给予它焦点
                this.Activate();
                // 任务栏区显示图标
                this.ShowInTaskbar = true;
                // 托盘区图标隐藏
                notifyIconCease.Visible = false;
            }
        }

        /// <summary>
        /// 判断是否最小化, 然后显示托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_SizeChanged(object sender, EventArgs e)
        {
            // 判断是否选择的是最小化按钮
            if (WindowState == FormWindowState.Minimized)
            {
                // 隐藏任务栏区图标
                this.ShowInTaskbar = false;
                // 图标显示在托盘区
                notifyIconCease.Visible = true;
            }
        }
        /// <summary>
        /// 确认是否退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否确认退出程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // 关闭所有的线程
                this.Dispose();
                this.Close();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认退出程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // 关闭所有的线程
                this.Dispose();
                this.Close();
            }
        }

        private void comboBoxToolVer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check startup.ini
            if (!File.Exists(Application.StartupPath + "\\" + comboBoxToolVer.Text + m_strStartupFile))
            {
                MessageBox.Show(m_strStartupFile + " not exist!");
                return ;
            }
            m_Startup = new IniFile(Application.StartupPath + "\\" + comboBoxToolVer.Text + m_strStartupFile);

            //initial form
            m_listCmb = new List<ComboBox> { cmbStation, cmbModel, cmbDut };
            m_listItem = new List<string> { "STATION", "PROJECT", "DUT" };
            for (int i = 0; i < m_listCmb.Count; i++)
            {
                string strItem = m_Startup.ReadFromFile("START", m_listItem[i], "");
                string strItems = m_Startup.ReadFromFile("SOURCE", m_listItem[i] + "S", "");

                m_listCmb[i].Items.Clear();
                foreach (var _item in strItems.Split(','))
                {
                    m_listCmb[i].Items.Add(_item);
                }
                m_listCmb[i].Text = strItem;
            }
        }

    }
}
