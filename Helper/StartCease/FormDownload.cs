using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using WinSCP;
using Ionic.Zip;

namespace StartCease
{
    public partial class FormDownload : Form
    {
        [System.Runtime.InteropServices.DllImport("Shlwapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string psz1, string psz2);


        Font m_font = new Font("Times New Roman", (float)11, FontStyle.Regular);

        IniFile m_Config;
        private string m_strConfigFile = "\\config.ini";

        private string remotePath = "/";
        private string localPath = Application.StartupPath + "\\";
        private string localFileMd5;

        private bool m_bIsCancel = false;

        public FormDownload()
        {
            InitializeComponent();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            string _strToolVer = comboBoxToolVerInFtp.Text;
            if (buttonDownload.Text == "开始下载")
            {
                m_bIsCancel = false;
                buttonDownload.Text = "取消下载";
                Thread p = new Thread(new ParameterizedThreadStart(DownloadNewTool));
                p.Start(_strToolVer);
            }
            else if (buttonDownload.Text == "取消下载")
            {
                m_bIsCancel = true;
            }
            else if (buttonDownload.Text == "取消解压")
            {
                m_bIsCancel = true;
            }

        }

        private void FormLoad(object sender, EventArgs e)
        {
            try
            {
                m_Config = new IniFile(Application.StartupPath + "\\" + m_strConfigFile);

                comboBoxToolVerInFtp.Items.Clear();
                var listTool = GetNewToolList();
                foreach (var _item in listTool)
                {
                    comboBoxToolVerInFtp.Items.Add(_item);
                }
                comboBoxToolVerInFtp.Text = listTool[0];

                progressBarDownload.Maximum = 100;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DownloadNewTool(object obj)
        {
            string _strToolVer = obj.ToString();
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = m_Config.ReadFromFile("FTP", "HOST", "127.0.0.1"),
                    UserName = m_Config.ReadFromFile("FTP", "USER", "blackshark"),
                    Password = m_Config.ReadFromFile("FTP", "PASSWORD", "blackshark"),
                };

                using (Session session = new Session())
                {
                    // Will continuously report progress of transfer
                    session.FileTransferProgress += SessionFileTransferProgress;

                    // Connect
                    session.Open(sessionOptions);
                    if (!session.FileExists(remotePath + _strToolVer))
                    {
                        throw new Exception("No file found");
                    }

                    UpdateTextBox(0, "Start Download.");
                    // Download the selected file
                    session.GetFiles(
                        RemotePath.EscapeFileMask(remotePath + _strToolVer), localPath).Check();
                    
                    session.Dispose();
                    
                    if (!m_bIsCancel)
                    {
                        localFileMd5 = GetMD5HashFromFile(localPath + _strToolVer);
                        UpdateTextBox(0, "Local MD5: " + localFileMd5);
                        UpdateTextBox(0, "Download completed.");

                        UpdateButtonText(0, "取消解压");
                        Thread p = new Thread(new ParameterizedThreadStart(ExtractNewTool));
                        p.Start(_strToolVer);
                    }
                    else
                    {
                        UpdateButtonText(0, "开始下载");
                    }
                }
            }
            catch (Exception obje)
            {
                Console.WriteLine("Error: {0}", obje);
                UpdateTextBox(0, string.Format("Error: {0}", obje));
                throw new Exception("Download " + _strToolVer + " fail!");
            }
        }
        
        private List<string> GetNewToolList()
        {
            // Setup session options
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = m_Config.ReadFromFile("FTP", "HOST", "127.0.0.1"),
                UserName = m_Config.ReadFromFile("FTP", "USER", "blackshark"),
                Password = m_Config.ReadFromFile("FTP", "PASSWORD", "blackshark"),
            };

            List<string> listTool = new List<string>() { };
            using (Session session = new Session())
            {
                // Will continuously report progress of transfer
                session.FileTransferProgress += SessionFileTransferProgress;

                // Connect
                session.Open(sessionOptions);

                // Get list of files in the directory
                RemoteDirectoryInfo directoryInfo = session.ListDirectory(remotePath);
                foreach (var f in directoryInfo.Files.Where(file => !file.IsDirectory && file.Name.EndsWith(".zip")))
                {
                    listTool.Add(f.Name);
                }

                string[] zipFiles = listTool.ToArray();
                Array.Sort(zipFiles, StrCmpLogicalW);
                Array.Reverse(zipFiles);
                listTool = zipFiles.ToList();

                session.Dispose();
            }

            return listTool;
        }

        private void ExtractNewTool(object obj)
        {
            string toolName = obj.ToString();
            UpdateTextBox(0, "Delete directory " + toolName.Substring(0, toolName.LastIndexOf(".zip") + 1));
            DeleteDir(localPath + toolName.Substring(0, toolName.LastIndexOf(".zip") + 1));
            using (ZipFile zip = ZipFile.Read(localPath + toolName))
            {
                zip.ExtractProgress += ExtractFileProgress;
                zip.ExtractAll(localPath, ExtractExistingFileAction.OverwriteSilently);
                UpdateTextBox(0, m_bIsCancel ? "Extract Uncompleted." : "Extract Completed.");
            }
            UpdateTextBox(0, "Delete zip file " + toolName);
            File.Delete(localPath + toolName);

            if (!m_bIsCancel)
            {
                UpdateTextBox(0, "      _             (done)");
                UpdateTextBox(0, "     | |                 ");
                UpdateTextBox(0, "   __| | ___  _ __   ___ ");
                UpdateTextBox(0, "  / _` |/ _ \\| '_ \\ / _ \\");
                UpdateTextBox(0, " | (_| | (_) | | | |  __/");
                UpdateTextBox(0, "  \\__,_|\\___/|_| |_|\\___|");
                UpdateTextBox(0, " {All Finished Successfully}");
            }

            UpdateButtonText(0, "开始下载");
        }

        private void SessionFileTransferProgress(object sender, FileTransferProgressEventArgs e)
        {
            // Print transfer progress
            UpdateTextBox(0, string.Format("{0} ({1:P0})", e.FileName, e.FileProgress));

            int progressVal = (int)(e.FileProgress * 100);
            string str = "下载进度: " + progressVal.ToString() + "%";
            UpdateProgress(progressVal, "");

            if (m_bIsCancel)
            {
                e.Cancel = true;
                UpdateTextBox(0, string.Format("Download Cancelled!!"));
            }
        }

        private void ExtractFileProgress(object sender, ExtractProgressEventArgs e)
        {
            // Print transfer progress
            if (e.EventType == ZipProgressEventType.Extracting_EntryBytesWritten)
            {
                double totalBytes = (double)e.TotalBytesToTransfer;
                double BytesTransferred = (double)e.BytesTransferred;
                UpdateProgress((int)(BytesTransferred / totalBytes * 100), "");
                UpdateTextBox(0, string.Format("Extract {0}...)", e.CurrentEntry.FileName));
            }

            if (m_bIsCancel)
            {
                e.Cancel = true;
                UpdateTextBox(0, string.Format("Extract Cancelled!!)"));
            }
        }

        delegate void UpdateProgressCallBack(int _val, string _str);
        public void UpdateProgress(int _val, string _str)
        {
            if (this.InvokeRequired)
            {
                UpdateProgressCallBack fun = new UpdateProgressCallBack(UpdateProgress);
                BeginInvoke(fun, new object[] { _val, _str });
            }
            else
            {
                progressBarDownload.Value = _val;
                //PointF pt = _str.EndsWith("%") ? 
                //    new PointF(this.progressBarDownload.Width / 2 - 50, this.progressBarDownload.Height / 2 - 10) : 
                //    new PointF(5, this.progressBarDownload.Height / 2 - 10);
                //this.progressBarDownload.CreateGraphics().DrawString(_str, m_font, Brushes.Black, pt); 
            }
        }

        public void UpdateButtonText(int _val, string _str)
        {
            if (this.InvokeRequired)
            {
                UpdateProgressCallBack fun = new UpdateProgressCallBack(UpdateButtonText);
                BeginInvoke(fun, new object[] { _val, _str });
            }
            else
            {
                buttonDownload.Text = _str;
            }
        }

        public void UpdateTextBox(int _val, string _str)
        {
            if (this.InvokeRequired)
            {
                UpdateProgressCallBack fun = new UpdateProgressCallBack(UpdateTextBox);
                BeginInvoke(fun, new object[] { _val, _str });
            }
            else
            {
                textBox.AppendText(_str + "\r\n");
                textBox.ScrollToCaret();
            }
        }

        /// <summary>
        /// 递归删除文件夹，避免只读文件导致删除不了的情况
        /// </summary>
        /// <param name="dir"> 文件夹全路径 </param>
        private static void DeleteDir(string dir)
        {
            if (!Directory.Exists(dir)) // 判断是否存在   
            {
                return;
            }

            foreach (string childName in Directory.GetFileSystemEntries(dir))// 获取子文件和子文件夹
            {
                if (File.Exists(childName)) // 如果是文件
                {
                    FileInfo fi = new FileInfo(childName);
                    if (fi.IsReadOnly)
                    {
                        fi.IsReadOnly = false; // 更改文件的只读属性
                    }
                    File.Delete(childName); // 直接删除其中的文件    
                }
                else// 不是文件就是文件夹
                {
                    DeleteDir(childName); // 递归删除子文件夹   
                }
            }
            DirectoryInfo di = new DirectoryInfo(dir);
            di.Attributes = FileAttributes.Normal & FileAttributes.Directory;
            Directory.Delete(dir, true); // 删除空文件夹                    
        }

        private static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

    }
}
