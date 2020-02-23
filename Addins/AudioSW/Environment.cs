using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel.Composition;
using Cease.Addins.AudioSW;
using Cease.Addins.Log;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace AudioSW
{
    public class Environment
    {
                /// <summary>
        /// 日志
        /// </summary>
        protected InterfaceLog log;

        /// <summary>
        /// 关闭某个进程
        /// </summary>
        /// <param name="sProcName">进程名称（不带.exe）</param>
        /// <returns></returns>
        protected bool KillProcess(string sProcName)
        {
            Process[] ProcArray = Process.GetProcessesByName(sProcName);
            if (ProcArray.Length > 0)
            {
                foreach (Process p in ProcArray)
                {
                    Console.WriteLine("Killing Process:" + p.ProcessName);
                    p.Kill();
                }
            }
            return true;
        }

        /// <summary>
        /// 检查某个TCP是否正在侦听
        /// </summary>
        /// <param name="sIP">IP地址</param>
        /// <param name="sPort">端口</param>
        /// <returns>如果有且非当前程序进程返回-1，如果有且为当前程序进程返回1，没有返回0，异常返回-2</returns>
        protected int IsTCPListenerExist(string sIP, string sPort)
        {
            int bIsExist = 0;

            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe ";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.StandardInput.WriteLine("netstat -an -o"); //
                p.StandardInput.WriteLine("exit ");
                string strRst = p.StandardOutput.ReadToEnd();
                strRst = strRst.Replace("\r\n", "\n");
                string[] strRstArray = strRst.Split('\n');
                p.WaitForExit();

                //筛选TCP
                List<string> tmplist = new List<string>();
                foreach (string s in strRstArray)
                {
                    if (s.Contains("TCP"))
                    {
                        tmplist.Add(s);
                    }
                }

                strRstArray = tmplist.ToArray();

                foreach (string s in strRstArray)
                {
                    if ((s.Contains(sIP + ":" + sPort) && s.Contains("LISTENING"))||(s.Contains("0.0.0.0:" + sPort) && s.Contains("LISTENING")))
                    {
                        Process curProc = Process.GetCurrentProcess();
                        string str = s.Substring(s.IndexOf("LISTENING") + 9).Replace(" ", "");
                        log.msg("socket pid:" + str + " and " + "current process pid:" + curProc.Id.ToString()+" can be reused");
                        if (str == curProc.Id.ToString())
                        {
                            log.msg("socket can be reused");
                            bIsExist = 1;
                        }
                        else
                        {
                            bIsExist = -1;
                        }
                        
                    }
                }
                return bIsExist;
            }
            catch (Exception exp)
            {
                log.err("Exception in Checking netstat -an -o.", exp);
                return -2;
            }

            
        }

        /// <summary>
        /// socket发送
        /// </summary>
        /// <param name="socClient">目标socket客户端</param>
        /// <param name="byteMessageToSend">数据包</param>
        /// <returns></returns>
        protected bool SocketSend(Socket socClient, byte[] byteMessageToSend)
        {
            socClient.SendTimeout = 3000;
            try
            {
                socClient.Send(byteMessageToSend);
                return true;
            }
            catch (Exception exp)
            {
                log.err("socket send error:", exp);
                return false;
            }
        }

        /// <summary>
        /// socket接收
        /// </summary>
        /// <param name="socClient">目标socket客户端</param>
        /// <param name="byteMessageToSend">接收数据包</param>
        /// <returns></returns>
        protected bool SocketReceive(Socket socClient, ref byte[] byteMessageToReceive)
        {
            socClient.ReceiveTimeout = 3000;
            try
            {
                byte[] buff = new byte[1024];
                int length = socClient.Receive(buff);
                byteMessageToReceive = buff;
                return true;
            }
            catch (Exception exp)
            {
                log.err("socket Receive error:", exp);
                return false;
            }
        }


        public static ManualResetEvent allDone = new ManualResetEvent(false);



        Thread thr;

        string ipadress;
        string port;

        protected bool ServerStartListening(string sIP, string sPort)
        {
            ipadress = sIP;
            port = sPort;
            //thr = new Thread(start);
            //thr.IsBackground = true;
            //thr.Start();
            return start();
        }

        protected bool start()
        {
            //创建一个新的Socket,这里我们使用最常用的基于TCP的Stream Socket（流式套接字）
            socServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socServer.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            IPAddress ip = IPAddress.Parse(ipadress);

            try
            {
                socServer.Bind(new IPEndPoint(ip, int.Parse(port)));
                socServer.Listen(0);
                //while (true)
                //{
                //    allDone.Reset();
                    socServer.BeginAccept(new AsyncCallback(AcceptCallback), socServer);
                    log.msg("服务已经启动，等待连接。。。");
                //    allDone.WaitOne();
                //}
                    return true;
            }

            catch (Exception exp)
            {
                log.socket("服务启动失败！失败原因：" + exp.ToString());
                return false;
            }
        }

        public Socket socServer { get; set; }
        public Socket socClient { get; set; }
        public bool bIsNewConnect { get; set; }

        protected void  AcceptCallback(IAsyncResult ar)
        {
            //allDone.Set();
            try
            {
                Socket listener = (Socket)ar.AsyncState;
                socClient = listener.EndAccept(ar);
                bIsNewConnect = true;
            }
            catch
            {
                log.socket("AcceptCallback异常，可能是在没有客户端连接的情况下关闭服务导致，这种情况可以忽略。");
            }
        }

        protected bool Restart()
        {
            try
            {
                socServer.BeginAccept(new AsyncCallback(AcceptCallback), socServer);
                log.msg("服务已经启动，等待连接。。。");
                return true;
            }
            catch (Exception exp)
            {
                log.err("服务启动失败！失败原因：" + exp.ToString());
                return false;
            }
            
        }

        /// <summary>
        /// 时间戳（毫秒级别）
        /// </summary>
        /// <param name="random是否带4位随机数"></param>
        /// <returns></returns>
        public static string GetTimeStampMilSec(bool random)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();
            if (random)
            {
                Random randObj = new Random();
                ret = ret + randObj.Next(1000, 9999).ToString();
            }

            return ret;
        }

        /// <summary>
        /// byte[]追加
        /// </summary>
        /// <param name="bOriginal"></param>
        /// <param name="bNew"></param>
        /// <returns></returns>
        public byte[] ByteArrayAppend(byte[] bOriginal, byte[] bNew)
        {
            List<byte> lTemp = new List<byte>();
            lTemp.AddRange(bOriginal);
            lTemp.AddRange(bNew);
            bOriginal = new byte[lTemp.Count];
            lTemp.CopyTo(bOriginal);
            return bOriginal;
        }

        /// <summary>
        /// 分隔byte[]
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="StartIndex起始序号"></param>
        /// <param name="EndIndex接收序号（包含）"></param>
        /// <returns></returns>
        public byte[] SplitArray(byte[] Source, int StartIndex, int EndIndex)
        {
            try
            {
                byte[] result = new byte[EndIndex - StartIndex + 1];
                for (int i = 0; i <= EndIndex - StartIndex; i++) result[i] = Source[i + StartIndex];
                return result;
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 解析接收的数据
        /// </summary>
        /// <param name="bData"></param>
        public static void AnalysisDataToMsg_Rev(byte[] bData)
        {
            if (bData.Length >= 100)
            {
                GV.bAction_Rev = ByteCut(bData, 0, 80);
                GV.bDataType_Rev = ByteCut(bData, 80, 3);
                GV.bTimeStamp_Rev = ByteCut(bData, 83, 17);

                GV.sAction_Rev = System.Text.Encoding.Default.GetString(GV.bAction_Rev).Replace(" ", "");
                GV.sDataType_Rev = System.Text.Encoding.Default.GetString(GV.bDataType_Rev);
                GV.sTimeStamp_Rev = System.Text.Encoding.Default.GetString(GV.bTimeStamp_Rev);

                GV.bContent_Rev = ByteCut(bData, 100, bData.Length - 100);
                GV.sContent_Rev = System.Text.Encoding.Default.GetString(GV.bContent_Rev);
            }
        }

        /// <summary>
        /// 二进制流转文件
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="savepath"></param>
        /// <param name="retStr"></param>
        /// <returns></returns>
        public static bool Bytes2File(byte[] buff, string savepath, ref string retStr)
        {

            if (System.IO.File.Exists(savepath))
            {
                System.IO.File.Delete(savepath);
            }
            FileStream fs = new FileStream(savepath, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                bw.Write(buff, 0, buff.Length);
            }
            catch (Exception exp)
            {
                retStr = retStr.ToString();
                return false;
            }
            bw.Close();
            fs.Close();
            return true;
        }

        /// <summary>
        /// 文件转二进制流
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static byte[] File2Bytes(string FilePath)
        {
            if (!System.IO.File.Exists(FilePath))
            {
                return new byte[0];
            }

            FileInfo fi = new FileInfo(FilePath);
            byte[] buff = new byte[fi.Length];

            FileStream fs = fi.OpenRead();
            fs.Read(buff, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            return buff;
        }

        /// <summary>
        /// 截取byte[]数组
        /// </summary>
        /// <param name="bSource"></param>
        /// <param name="from起点"></param>
        /// <param name="len长度"></param>
        /// <returns></returns>
        private static byte[] ByteCut(byte[] bSource, int from, int len)
        {
            return bSource.Skip(from).Take(len).ToArray();
        }

        /// <summary>
        /// 解析JSON
        /// </summary>
        /// <param name="JsonStr"></param>
        public static void JsonDeserialize(string JsonStr)
        {
            GV.mRevSocMsg = new SocketMsg();
            GV.mRevSocMsg = JsonConvert.DeserializeObject<SocketMsg>(JsonStr);
        }

        [DllImport("kernel32.dll", EntryPoint = "WinExec")]
        public static extern int WinExec(string lpCmdLine, int nCmdShow);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

    }

    [Export(typeof(InterfaceAudioSW))]
    [ExportMetadata("AddinName", "AudioSW")]
    public class AudioSW : Environment, InterfaceAudioSW
    {
        /// <summary>
        /// 关闭进程列表
        /// </summary>
        /// <param name="sProcessesNameList">进程名列表</param>
        /// <returns></returns>
        public bool KillProcesses(string[] sPorcessesNameList)
        {
            foreach (string s in sPorcessesNameList)
            {
                KillProcess(s);
            }
            return true;
        }

        public void RegisterLogger(InterfaceLog _log)
        {
            this.log = _log;
        }

        public void SocketLog(string msg)
        {
            log.socket(msg);
        }

        //struSocket _socServer;

        //public Socket socServer { get; set; }
        //public Socket socClient { get; set; }

        public bool StartServer(string sIP, string sPort)
        {
            int iStat = IsTCPListenerExist(sIP, sPort);
            if (iStat == -1)
            {
                log.err(string.Format("Socket service:{0}:{1} is now occupied by other process! 服务被其它进程占用！", sIP, sPort));
                return false;
            }
            else if (iStat == 1)
            {
                if (Restart())
                {
                    log.msg(string.Format("Socket service:{0}:{1} is already started! 服务已经启动！", sIP, sPort));
                    return true;
                }
                else
                {
                    log.msg(string.Format("Socket service:{0}:{1} is already started! 服务启动重新Accept失败！", sIP, sPort));
                    return false;
                }
                
            }
            else if (iStat == 0)
            {
                return ServerStartListening(sIP, sPort);
            }
            else
            {
                log.err(string.Format("checking netstat error! 检测TCP监听状态异常！"));
                return false;
            }

        }

        public bool SendAndReceiveSocket(string sCommandHead,string sCommandContent, int nTimeOut,ref string sReceive)
        {
            try
            {
                if (sCommandHead.Split(' ').Length != 5)
                {
                    log.socket("ERROR: wrong commandhead: "+sCommandHead);
                    sReceive = "ERROR: wrong commandhead!"+sCommandHead;
                    return false;
                }

                byte[] newline = { 10 };
                byte[] byteFullCommand = null;
                byte[] byteCmdHead = null;
                byte[] byteCmdContent = null;
                int iContentLenth = 0;
                byte[] byteByteStringCmdHead=new byte[5];
                byte[] byteContentLenth = new byte[2];
                string strbyteListCMD = sCommandHead;

                byteCmdHead = Encoding.ASCII.GetBytes(sCommandHead);
                if (sCommandContent.Length > 0)
                {
                    byteCmdContent = Encoding.ASCII.GetBytes(sCommandContent);
                    iContentLenth = byteCmdContent.Length;
                    string[] bytestringCMDHead=sCommandHead.Split(' ');
                    for(int i =0;i<5;i++)
                    {
                        byteByteStringCmdHead[i]=(byte)(int.Parse(bytestringCMDHead[i]));
                    }
                    byteContentLenth[0] = (byte)(iContentLenth % 0xff);
                    byteContentLenth[1] = (byte)(iContentLenth / 0xff);

                    List<byte> byteListCMD = new List<byte>();
                    byteListCMD.AddRange(byteByteStringCmdHead);
                    byteListCMD.AddRange(byteContentLenth);
                    byteListCMD.AddRange(byteCmdContent);

                    strbyteListCMD = "";
                    for (int i = 0; i < byteListCMD.Count; i++)
                    {
                        if (i != strbyteListCMD.Length - 1)
                        {
                            strbyteListCMD += byteListCMD[i].ToString() + " ";
                        }
                        else
                        {
                            strbyteListCMD += byteListCMD[i].ToString();
                        }

                    }

                    byteFullCommand = Encoding.ASCII.GetBytes(strbyteListCMD);

                }
                else
                {
                    byteFullCommand = byteCmdHead;
                }

                byte[] byteToSend = byteFullCommand;
                List<byte> lTemp = new List<byte>();
                lTemp.AddRange(byteToSend);
                lTemp.AddRange(newline);
                byteToSend = new byte[lTemp.Count];
                lTemp.CopyTo(byteToSend);
                socClient.SendTimeout = nTimeOut;//20180414  sasa 发送超时
                socClient.Send(byteToSend);
                log.socket("Send:[MessageString]" + sCommandContent + ";      [Client:" + ((IPEndPoint)socClient.RemoteEndPoint).Address.ToString()
                    + ":" + ((IPEndPoint)socClient.RemoteEndPoint).Port.ToString() + "]");
                log.socket("Send:[ByteArrayString]" + strbyteListCMD + ";      [Client:" + ((IPEndPoint)socClient.RemoteEndPoint).Address.ToString()
                    + ":" + ((IPEndPoint)socClient.RemoteEndPoint).Port.ToString() + "]");
                if (sCommandHead == "75 250 0 56 0")
                    try
                    {
                        Thread.Sleep(1000);
                        socClient.Close();
                        return true;
                    }
                    catch
                    {

                    }
                //Receive
                byte[] bReceiveData = new byte[1024];
                socClient.ReceiveTimeout = nTimeOut;
                int length = socClient.Receive(bReceiveData);
                string[] strAarary = new string[length];
                string MessageString = "";
                string ByteArrayString = "";

                for (int i = 0; i < length; i++)
                {
                    MessageString += bReceiveData[i].ToString() + " ";
                    strAarary[i] = bReceiveData[i].ToString() + " ";
                }
                List<byte> tmpByteList = new List<byte>();     
                for (int i = 0; i < length; i++)
                {
                    if (bReceiveData[i] != 10)
                    {
                        tmpByteList.Add(bReceiveData[i]);
                    }
                    else
                    {
                        break;
                    }
                }
                ByteArrayString = System.Text.Encoding.Default.GetString(tmpByteList.ToArray());
                string[] stringArray = ByteArrayString.Split(' ');
                tmpByteList = new List<byte>();

                if (stringArray.Length > 7)
                {
                    for (int i = 7; i < stringArray.Length; i++)
                    {
                        tmpByteList.Add(Convert.ToByte(stringArray[i]));
                    }
                }
                sReceive = System.Text.Encoding.Default.GetString(tmpByteList.ToArray());
                if (MessageString.Length > 0)
                {
                    MessageString=MessageString.Replace("\n", "");
                    log.socket("Receive:[ByteArrayString]" + ByteArrayString + ";      [Client:" + ((IPEndPoint)socClient.RemoteEndPoint).Address.ToString()
                    + ":" + ((IPEndPoint)socClient.RemoteEndPoint).Port.ToString() + "]");
                    log.socket("Receive:[MessageString]" + sReceive + ";      [Client:" + ((IPEndPoint)socClient.RemoteEndPoint).Address.ToString()
                    + ":" + ((IPEndPoint)socClient.RemoteEndPoint).Port.ToString() + "]");
                }
                return true;
            }
            catch (Exception exp)
            {
                if (sCommandContent != "ls /sdcard/audiotest/test.test")
                {
                    log.err("发送异常：" + exp.ToString());
                }
                return false;
            }

        }

        public bool SendOnlySocket(string sCommandHead, string sCommandContent, int nTimeOut, ref string sErrorMsg)
        {
            try
            {
                if (sCommandHead.Split(' ').Length != 5)
                {
                    log.socket("ERROR: wrong commandhead: " + sCommandHead);
                    sErrorMsg = "ERROR: wrong commandhead!" + sCommandHead;
                    return false;
                }

                byte[] newline = { 10 };
                byte[] byteFullCommand = null;
                byte[] byteCmdHead = null;
                byte[] byteCmdContent = null;
                int iContentLenth = 0;
                byte[] byteByteStringCmdHead = new byte[5];
                byte[] byteContentLenth = new byte[2];
                string strbyteListCMD = sCommandHead;

                byteCmdHead = Encoding.ASCII.GetBytes(sCommandHead);
                if (sCommandContent.Length > 0)
                {
                    byteCmdContent = Encoding.ASCII.GetBytes(sCommandContent);
                    iContentLenth = byteCmdContent.Length;
                    string[] bytestringCMDHead = sCommandHead.Split(' ');
                    for (int i = 0; i < 5; i++)
                    {
                        byteByteStringCmdHead[i] = (byte)(int.Parse(bytestringCMDHead[i]));
                    }
                    byteContentLenth[0] = (byte)(iContentLenth % 0xff);
                    byteContentLenth[1] = (byte)(iContentLenth / 0xff);

                    List<byte> byteListCMD = new List<byte>();
                    byteListCMD.AddRange(byteByteStringCmdHead);
                    byteListCMD.AddRange(byteContentLenth);
                    byteListCMD.AddRange(byteCmdContent);

                    strbyteListCMD = "";
                    for (int i = 0; i < byteListCMD.Count; i++)
                    {
                        if (i != strbyteListCMD.Length - 1)
                        {
                            strbyteListCMD += byteListCMD[i].ToString() + " ";
                        }
                        else
                        {
                            strbyteListCMD += byteListCMD[i].ToString();
                        }

                    }

                    byteFullCommand = Encoding.ASCII.GetBytes(strbyteListCMD);

                }
                else
                {
                    byteFullCommand = byteCmdHead;
                }

                byte[] byteToSend = byteFullCommand;
                List<byte> lTemp = new List<byte>();
                lTemp.AddRange(byteToSend);
                lTemp.AddRange(newline);
                byteToSend = new byte[lTemp.Count];
                lTemp.CopyTo(byteToSend);
                socClient.SendTimeout = nTimeOut;
                socClient.Send(byteToSend);
                log.socket("Send:[MessageString]" + sCommandContent + ";      [Client:" + ((IPEndPoint)socClient.RemoteEndPoint).Address.ToString()
                    + ":" + ((IPEndPoint)socClient.RemoteEndPoint).Port.ToString() + "]");
                log.socket("Send:[ByteArrayString]" + strbyteListCMD + ";      [Client:" + ((IPEndPoint)socClient.RemoteEndPoint).Address.ToString()
                    + ":" + ((IPEndPoint)socClient.RemoteEndPoint).Port.ToString() + "]");
                if (sCommandHead == "75 250 0 56 0")
                    try
                    {
                        Thread.Sleep(1000);
                        socClient.Close();
                        return true;
                    }
                    catch
                    {

                    }
                return true;
            }
            catch (Exception exp)
            {
                if (sCommandContent != "ls /sdcard/audiotest/test.test")
                {
                    log.err("发送异常：" + exp.ToString());
                }
                return false;
            }
        }

        public bool ReceiveOnlySocket(string sCommandHead,  int nTimeOut, ref string sReceive)
        {
            try
            {
                if (sCommandHead.Split(' ').Length != 5)
                {
                    log.socket("ERROR: wrong commandhead: " + sCommandHead);
                    sReceive = "ERROR: wrong commandhead!" + sCommandHead;
                    return false;
                }

                //Receive
                byte[] bReceiveData = new byte[1024];
                socClient.ReceiveTimeout = nTimeOut;
                int length = socClient.Receive(bReceiveData);
                string[] strAarary = new string[length];
                string MessageString = "";
                string ByteArrayString = "";

                for (int i = 0; i < length; i++)
                {
                    MessageString += bReceiveData[i].ToString() + " ";
                    strAarary[i] = bReceiveData[i].ToString() + " ";
                }


                List<byte> tmpByteList = new List<byte>();
                for (int i = 0; i < length; i++)
                {
                    if (bReceiveData[i] != 10)
                    {
                        tmpByteList.Add(bReceiveData[i]);
                    }
                    else
                    {
                        break;
                    }
                }
                ByteArrayString = System.Text.Encoding.Default.GetString(tmpByteList.ToArray());
                string[] stringArray = ByteArrayString.Split(' ');
                tmpByteList = new List<byte>();

                if (stringArray.Length > 7)
                {
                    for (int i = 7; i < stringArray.Length; i++)
                    {
                        tmpByteList.Add(Convert.ToByte(stringArray[i]));
                    }
                }
                sReceive = System.Text.Encoding.Default.GetString(tmpByteList.ToArray());
                if (MessageString.Length > 0)
                {
                    MessageString = MessageString.Replace("\n", "");
                    log.socket("Receive:[ByteArrayString]" + ByteArrayString + ";      [Client:" + ((IPEndPoint)socClient.RemoteEndPoint).Address.ToString()
                    + ":" + ((IPEndPoint)socClient.RemoteEndPoint).Port.ToString() + "]");
                    log.socket("Receive:[MessageString]" + sReceive + ";      [Client:" + ((IPEndPoint)socClient.RemoteEndPoint).Address.ToString()
                    + ":" + ((IPEndPoint)socClient.RemoteEndPoint).Port.ToString() + "]");

                    if (!ByteArrayString.Contains(sCommandHead))
                    {
                        log.err("return command head is not match with :" + sCommandHead);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception exp)
            {
               log.err("接收异常：" + exp.ToString());
                return false;
            }

        }

        public bool PullFileSocket(string sFileNameInPhone,  int iSendTimeOut,int iReceiveTimeOut,ref string sErrorMsg)
        {
            SocketAudioTest.RegisterLogger(this.log);
            log.socket("Pull File "+sFileNameInPhone+" From Phone");
            return SocketAudioTest.GetFile(socClient, sFileNameInPhone, iSendTimeOut, iReceiveTimeOut, ref sErrorMsg);
        }

        public bool CloseServer()
        {
            try
            {
                //socServer.Dispose();
                socServer.Close(1000);
                
                return true;
            }
            catch (Exception exp)
            {
                log.err("close server error:" + exp.ToString());
                return false;
            }
        }

        public bool CloseClient(int iTimeout)
        {

                try
                {
                    IPEndPoint end = (IPEndPoint)socClient.LocalEndPoint;
                }
                catch
                {
                    log.msg("Client not exist.");
                    return true;
                }
                try
                {
                byte[] newline = { 10 };
                byte[] byteToSend;
                byteToSend = System.Text.Encoding.Default.GetBytes("75 250 0 56 0");
                List<byte> lTemp = new List<byte>();
                lTemp.AddRange(byteToSend);
                lTemp.AddRange(newline);
                socClient.SendTimeout = iTimeout;
                byteToSend = new byte[lTemp.Count];
                lTemp.CopyTo(byteToSend);
                socClient.Send(byteToSend);
                Thread.Sleep(1000);
                socClient.Close();
                return true;
            }
            catch (Exception exp)
            {
                log.socket("close client error:" + exp.ToString());
                return true;
            }
        }

        public bool IsClientStatusReady()
        {
            string sReturnMsg = "";
            if (socClient != null)
            {
                try
                {
                    IPEndPoint end = (IPEndPoint)socClient.LocalEndPoint;
                    SendAndReceiveSocket("75 250 2 56 0", "ls /sdcard/audiotest/test.test", 1000, ref sReturnMsg);
                    if (sReturnMsg.Contains("test.test"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public bool IsClientNewConnection()
        {
            return bIsNewConnect;
        }
        public void ResetIsNewConnectionStatus()
        {
            bIsNewConnect = false;
        }

        public bool GetServerInfo(ref Dictionary<string, string> _dicSocketInfo)
        {
            try
            {
                IPEndPoint ipEnd = (IPEndPoint)socServer.LocalEndPoint;
                if (_dicSocketInfo.ContainsKey("SocketServer"))
                {
                    _dicSocketInfo["SocketServer"] = ipEnd.Address.ToString() + ":" + ipEnd.Port.ToString();
                }
                else
                {
                    _dicSocketInfo.Add("SocketServer", ipEnd.Address.ToString() + ":" + ipEnd.Port.ToString());
                }
                return true;
            }
            catch
            {
                _dicSocketInfo["SocketServer"] = "Not Ready";
                return false;
            }
        }

        public bool GetClientInfo(ref Dictionary<string, string> _dicSocketInfo)
        {
            try
            {
                IPEndPoint ipEnd = (IPEndPoint)socClient.RemoteEndPoint;
                if (_dicSocketInfo.ContainsKey("SocketClient"))
                {
                    _dicSocketInfo["SocketClient"] = ipEnd.Address.ToString() + ":" + ipEnd.Port.ToString();
                }
                else
                {
                    _dicSocketInfo.Add("SocketClient", ipEnd.Address.ToString() + ":" + ipEnd.Port.ToString());
                }
                return true;
            }
            catch
            {
                if (_dicSocketInfo.ContainsKey("SocketClient"))
                {
                    _dicSocketInfo["SocketClient"] = "No Connection";
                }
                else
                {
                    _dicSocketInfo.Add("SocketClient", "No Connection");
                }
                return false;
            }
        }


        public bool SendCommandStringToSerialPort(string sPort, string sCmd,int nTimeout,ref string sReturnMsg)
        {
            return SerialPortCtrl.SendCommand(sPort, sCmd,nTimeout, ref sReturnMsg);
        }

        public bool GetComPortReceive(ref string sReturnMsg, int nTimeout)
        {
            return SerialPortCtrl.Receive(ref sReturnMsg, nTimeout);
        }

        public bool ReStartSocketServerBeginAccept()
        {
            return Restart();
        }



        // G66T APP New function
        public bool SocketAppSendAndReceive(string sCommandHead, string sCommandContent, int iSendTimeOut, int iReceiveTimeOut, ref string sReceive)
        {
            try
            {
                sReceive = "";
                if (sCommandHead.Length != 83)
                {
                    log.socket("ERROR: wrong commandhead: " + sCommandHead);
                    sReceive = "ERROR: wrong commandhead!" + sCommandHead;
                    return false;
                }

                GV.sTimeStamp_Send = GetTimeStampMilSec(true);
                string cmdStr = sCommandHead + GV.sTimeStamp_Send + sCommandContent;
                byte[] byteToSend = System.Text.Encoding.Default.GetBytes(cmdStr);
                byte[] NewLine = { 10 };
                socClient.SendTimeout = iSendTimeOut;
                socClient.Send(ByteArrayAppend(byteToSend, NewLine));
                log.socket("【Send】" + cmdStr);

                //接收
                socClient.ReceiveTimeout = iReceiveTimeOut;
                byte[] arrServerRecMsg = new byte[1024];
                int totalLength = 0;
                int length = 0;
                byte[] buffer = null;
                while (true)
                {
                    arrServerRecMsg = new byte[512]; //缓存放小一点试试 2018.10.24 sasa
                    try
                    {
                        length = socClient.Receive(arrServerRecMsg);
                    }
                    catch
                    {

                    }
                    if (length > 0)
                    {
                        arrServerRecMsg = SplitArray(arrServerRecMsg, 0, length - 1);
                        totalLength += length;
                        if (buffer == null)
                        {
                            buffer = arrServerRecMsg;
                        }
                        else
                        {
                            buffer = ByteArrayAppend(buffer, arrServerRecMsg);
                        }
                        if (arrServerRecMsg[length - 1] == NewLine[0])      //  '\n'结束符
                        {
                            log.socket("meet new line finished");
                            break;
                        }
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }


                }
                if (buffer.Length < 100)        //遇到接收的数据格式不正确时，打印出接收的数据，然后清空接收缓存队列
                {
                    log.err("socket receive less than 100 bytes：" + buffer.Length.ToString());
                    try
                    {
string strError = System.Text.Encoding.Default.GetString(buffer);
                    log.socket("【Receive】" + strError);
                        arrServerRecMsg = new byte[1024];
                        length = socClient.Receive(arrServerRecMsg);
                    }
                    catch
                    { 

                    }
                    return false;
                }
                buffer = SplitArray(buffer, 0, buffer.Length - 2);
                string strstr = System.Text.Encoding.Default.GetString(buffer);
                log.socket("【Receive】" + strstr);

                AnalysisDataToMsg_Rev(buffer);
                if (GV.sTimeStamp_Send == GV.sTimeStamp_Rev)
                {
                    if (GV.sDataType_Rev == "MSG")
                    {
                        JsonDeserialize(GV.sContent_Rev);
                        if (GV.mRevSocMsg == null)
                        {
                            log.err("【Exception】返回Content空或非Json格式");
                            return false;

                        }
                        if (GV.mRevSocMsg.result == null)
                        {
                            log.err("【Exception】返回Content有误");
                            return false;
                        }

                        if (GV.mRevSocMsg.result != "P")
                        {
                            log.err("【Error】返回result失败");
                            return false;
                        }
                        if (GV.mRevSocMsg.message == null)
                        {
                            log.err("【Exception】返回Content有误");
                            return false;
                        }
                        sReceive = GV.mRevSocMsg.message;
                    }
                }
                return true;
            }
            catch (Exception exp)
            {
                if (sCommandContent != "ls /sdcard/audiotest/test.test")
                {
                    log.err("发送异常：" + exp.ToString());
                }
                return false;
            }

        }

        //使用指令池发收
        public bool SocketAppSendAndReceive2(string sCommandHead, string sCommandContent, int iSendTimeOut, int iReceiveTimeOut, ref string sReceive)
        {
            try
            {
                sReceive = "";
                if (sCommandHead.Length != 83)
                {
                    log.socket("ERROR: wrong commandhead: " + sCommandHead);
                    sReceive = "ERROR: wrong commandhead!" + sCommandHead;
                    return false;
                }

                string sTimeStamp_Send = GetTimeStampMilSec(true);
                string cmdStr = sCommandHead + sTimeStamp_Send + sCommandContent;
                byte[] byteToSend = System.Text.Encoding.Default.GetBytes(cmdStr);
                byte[] NewLine = { 10 };
                socClient.SendTimeout = iSendTimeOut;
                try
                {
                    lock (this)
                    {
                        socClient.Send(ByteArrayAppend(byteToSend, NewLine));
                    }
                    log.socket("【Send】" + cmdStr);
                }
                catch
                {
                    sReceive = "Send timeout!   " + cmdStr;
                    return false;
                }
                    string sContent_Rev = GetRecString(sCommandHead + sTimeStamp_Send, iReceiveTimeOut);
                    if (sContent_Rev.Length == 0)
                    {
                        sReceive = "Receive timeout!    " + sCommandHead + sTimeStamp_Send;
                        return false;
                    }

                    log.socket("【Receive】" + sCommandHead + sTimeStamp_Send + sContent_Rev);

                    SocketMsg mRevSocMsg = JsonConvert.DeserializeObject<SocketMsg>(sContent_Rev);
                    if (mRevSocMsg == null)
                    {
                        log.err("【Exception】返回Content空或非Json格式");
                        return false;

                    }
                    if (mRevSocMsg.result == null)
                    {
                        log.err("【Exception】返回Content有误");
                        return false;
                    }

                    if (mRevSocMsg.result != "P")
                    {
                        log.err("【Error】返回result失败");
                        return false;
                    }
                    if (mRevSocMsg.message == null)
                    {
                        log.err("【Exception】返回Content有误");
                        return false;
                    }
                    sReceive = mRevSocMsg.message; ;
                

                return true;
            }
            catch (Exception exp)
            {
                log.err("异常：" + exp.ToString());
                return false;
            }

        }

        //启动指令池机制
        public bool SocketStartReceivingProc(ref string ErrMsg)
        {
            ErrMsg = "";
            if (socClient != null)
            {
                GV.RecDataStore = new List<byte>();
                GV.RecDataStoreAll = new List<byte>();
                GV.RecDataAndTimeStoreAll = new Dictionary<string, byte[]>();
                GV.RecStringStore = new List<string>();
                GV.dicRecStr = new Dictionary<string, string>();
                GV.dicRecBytes = new Dictionary<string, byte[]>();
                GV.IsRecevingFile = false;
                GV.thrReceiving = new Thread(ReceiveProc);
                GV.thrReceiving.IsBackground = true;
                GV.thrReceiving.Start();
                return true;
            }
            else
            {
                ErrMsg = "Socket Client is Null";
                return false;
            }
        }

        //终止指令池机制
        public bool SocketStopReceivingProc(string SN, string Project, string Station, ref string ErrMsg)
        {
            ErrMsg = "";
            try
            {
                GV.thrReceiving.Abort();

                //string Dir = @"D:\TestLog\CeaseLog\DUT1\\" + Project + "\\" + Station + "\\" + DateTime.Today.ToString("yyyy-MM-dd") + "\\Socket";
                //if (!Directory.Exists(Dir))
                //{
                //    Directory.CreateDirectory(Dir);
                //}
                //string socketDataLogFileName = Dir + "\\" + SN + "_" + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss.fff") + ".dat";
                ////Bytes2File(GV.RecDataStoreAll.ToArray(), Dir + "\\" + SN + "_" + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss.fff") + ".dat", ref ErrMsg);//保存接收的socket数据
                //foreach (string time in GV.RecDataAndTimeStoreAll.Keys)
                //{
                //    File.AppendAllText(socketDataLogFileName, "[" + time + "]" + System.Text.Encoding.Default.GetString(GV.RecDataAndTimeStoreAll[time]));
                //}
                return true;
            }
            catch (Exception exp)
            {
                ErrMsg = "Thread Receiving Abort Fail!" + exp.ToString();
                return false;
            }
        }

        //指令池接收数据线程
        private void ReceiveProc()
        {
            
            int length = 0;
            while (true)
            {
                length = 0;
                GV.buff = new byte[1024*1024];
                try
                {
                    socClient.ReceiveTimeout = 100;
                    length = socClient.Receive(GV.buff);
                    GV.buff = SplitArray(GV.buff, 0, length - 1);
                    GV.RecDataStore.AddRange(GV.buff);
                    GV.RecDataStoreAll.AddRange(GV.buff);
                    GV.RecDataAndTimeStoreAll.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), GV.buff);
                    
                }
                catch
                {
                    //File.AppendAllText(@"d:\socketlog.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "   No Msg" + "\r\n");
                }

                if (GV.RecDataStore.Count > 0)
                {
                    List<byte> tmpBytes = GV.RecDataStore.FindAll(s => s == 10);
                    int idx = 0;
                    if (tmpBytes.Count == 1)
                    {
                        idx = GV.RecDataStore.FindIndex(s => s == 10);
                    }
                    else if (tmpBytes.Count > 1)
                    {
                        idx = GV.RecDataStore.FindIndex(s => s == 10);
                    }

                    if (idx >= 99 && !GV.IsRecevingFile)
                    {
                        byte[] strByte = SplitArray(GV.RecDataStore.ToArray(), 0, idx);
                        string recstr = System.Text.Encoding.Default.GetString(strByte);
                        GV.RecStringStore.Add(recstr);
                        if (recstr.Length >= 100)
                        {
                            if (recstr.Contains("MSG") && (!recstr.Contains("FILE_PULL")))
                            {
                                lock (this)
                                {
                                    //File.AppendAllText(@"d:\socketlog.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "Start Add   " + recstr.Substring(0, 100) + recstr.Substring(100, recstr.Length - 100 - 1) + "\r\n");
                                    GV.dicRecStr.Add(recstr.Substring(0, 100), recstr.Substring(100, recstr.Length - 100 - 1));
                                    //File.AppendAllText(@"d:\socketlog.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "Finish Add   " + recstr.Substring(0, 100) + recstr.Substring(100, recstr.Length - 100 - 1)+"\r\n");
                                    GV.RecDataStore.RemoveRange(0, idx + 1);
                                }
                            }
                            else if (recstr.Contains("MSG") && (recstr.Contains("FILE_PULL")))       //接收消息为Pull文件的MSG消息，需要解析文件大小
                            {
                                lock (this)
                                {
                                    //File.AppendAllText(@"d:\socketlog.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "Start Add   " + recstr.Substring(0, 100) + recstr.Substring(100, recstr.Length - 100 - 1) + "\r\n");
                                    GV.dicRecStr.Add(recstr.Substring(0, 100), recstr.Substring(100, recstr.Length - 100 - 1));
                                    //File.AppendAllText(@"d:\socketlog.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "Finish Add   " + recstr.Substring(0, 100) + recstr.Substring(100, recstr.Length - 100 - 1) + "\r\n");
                                }
                                JsonDeserialize(recstr.Substring(100));
                                //GV.JsonObject.ContainsKey("size");
                                GV.iFileSize = int.Parse(GV.mRevSocMsg.size);
                                GV.RecDataStore.RemoveRange(0, idx + 1);
                                GV.IsRecevingFile = true;
                            }
                            else if (recstr.Contains("DAT") && (recstr.Contains("FILE_PUSH")))     //接收消息为Push文件成功后消息
                            {
                                lock (this)
                                {
                                    //File.AppendAllText(@"d:\socketlog.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "Start Add   " + recstr.Substring(0, 100) + recstr.Substring(100, recstr.Length - 100 - 1) + "\r\n");
                                    GV.dicRecStr.Add(recstr.Substring(0, 100), recstr.Substring(100, recstr.Length - 100 - 1));
                                    //File.AppendAllText(@"d:\socketlog.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "Finish Add   " + recstr.Substring(0, 100) + recstr.Substring(100, recstr.Length - 100 - 1) + "\r\n");
                                }
                                GV.RecDataStore.RemoveRange(0, idx + 1);
                            }

                        }
                        else
                        {
                        }
                    }
                    else if (GV.RecDataStore.Count >= (100 + GV.iFileSize + 1) && GV.IsRecevingFile)          //接收PULL的数据长度为指令头（100）+文件长度+换行符（1）
                    {

                        byte[] strByte = SplitArray(GV.RecDataStore.ToArray(), 0, 99);
                        string recstr = System.Text.Encoding.Default.GetString(strByte);
                        if (recstr.Contains("DAT") && (recstr.Contains("FILE_PULL")))
                        {
                            GV.dicRecBytes.Add(recstr, SplitArray(GV.RecDataStore.ToArray(), 100, 100 + GV.iFileSize - 1));
                            GV.RecDataStore.RemoveRange(0, 100 + GV.iFileSize + 1);//把最后换行符也清理
                            GV.IsRecevingFile = false;
                            GV.iFileSize = 0;
                        }
                    }
                }
                else
                {
                    Thread.Sleep(20);
                }
            }
        }

        //指令池搜索接收字符串
        private string GetRecString(string key, int times)
        {
            for (int i = 0; i <= times / 50; i++)
            {
                    if (GV.dicRecStr.ContainsKey(key))
                    {
                        return GV.dicRecStr[key];
                    }
                Thread.Sleep(50);
                if (i == times / 50)
                {
                    return "";
                }
            }
            return "";
        }

        //指令池搜索接收byte数组
        private byte[] GetRecBytes(string key, int times)
        {
            for (int i = 0; i <= times / 50; i++)
            {
                if (GV.dicRecBytes.ContainsKey(key))
                {
                    return GV.dicRecBytes[key];
                }
                Thread.Sleep(50);
                if (i == times / 50)
                {
                    return null;
                }
            }
            return null;
        }

        //使用指令池Pull
        public bool SocketAppPullFile2(string sCommandHead, string sFileName, string sCommandContent, int iSendTimeOut, int iReceiveTimeOut, ref string sReceive)
        {
                sReceive = "";
                if (sCommandHead.Length != 83)
                {
                    log.socket("ERROR: wrong commandhead: " + sCommandHead);
                    sReceive = "ERROR: wrong commandhead!" + sCommandHead;
                    return false;
                }

                string sTimeStamp_Send = GetTimeStampMilSec(true);
                string cmdStr = sCommandHead + sTimeStamp_Send + sCommandContent;
                byte[] byteToSend = System.Text.Encoding.Default.GetBytes(cmdStr);
                byte[] NewLine = { 10 };
                socClient.SendTimeout = iSendTimeOut;
                
                    lock (this)
                    {
                        try
                        {
                            socClient.Send(ByteArrayAppend(byteToSend, NewLine));
                        }
                        catch
                        {
                            log.err("Send timeout!" + cmdStr);
                            return false;
                        }
                    }
                    log.socket("【Send】" + cmdStr);

                

                string sContent_Rev = GetRecString(sCommandHead + sTimeStamp_Send, iReceiveTimeOut);
                if (sContent_Rev.Length == 0)
                {
                    log.err("Receive timeout!   " + sCommandHead + sTimeStamp_Send);
                    return false;
                }

                log.socket("【Receive】" + sCommandHead + sTimeStamp_Send + sContent_Rev);

                SocketMsg mRevSocMsg = JsonConvert.DeserializeObject<SocketMsg>(sContent_Rev);
                if (mRevSocMsg == null)
                {
                    log.err("【Exception】返回Content空或非Json格式");
                    return false;

                }
                if (mRevSocMsg.result == null)
                {
                    log.err("【Exception】返回Content有误");
                    return false;
                }

                if (mRevSocMsg.result != "P")
                {
                    log.err("【Error】返回result失败");
                    return false;
                }
                if (mRevSocMsg.message == null)
                {
                    log.err("【Exception】返回Content有误");
                    return false;
                }
                sReceive = mRevSocMsg.message;




            //请求数据
            string strToSend = "FILE_PULL                                                                       DAT" + sTimeStamp_Send;
            byte[] byteToSend2 = System.Text.Encoding.Default.GetBytes(strToSend);
            lock (this)
            {
                socClient.Send(byteToSend2);//去掉换行符
            }
            log.socket("【Send】" + strToSend);

            GV.bReceiveFileByte = GetRecBytes(strToSend, iReceiveTimeOut);
            if (GV.bReceiveFileByte == null)
            {
                log.err("Receive file timeout!");
                return false;
            }

            log.socket("【ReceiveFile】" + strToSend + " +   FileSzie " + GV.bReceiveFileByte.Length.ToString() + "bytes");

            string sss = "";
            Bytes2File(GV.bReceiveFileByte, "D:\\" + sFileName, ref sss);
            log.msg("【Message】save file to " + "D:\\" + sFileName);

            return true;
        }

        public bool SocketAppPullFile(string sCommandHead, string sFileName, string sCommandContent, int iSendTimeOut, int iReceiveTimeOut, ref string sErrorMsg)
        {
            SocketAudioTest.RegisterLogger(this.log);
            log.socket("Pull File  From Phone");
            return SocketAudioTest.SocketAppPullFile(socClient, sFileName, sCommandHead, sCommandContent, iSendTimeOut, iReceiveTimeOut, ref sErrorMsg);
        }

        //使用指令池Push
        public bool SocketAppPushFile2(string sLocalFileName, string sCommandHead, string sCommandContent, int iSendTimeOut, int iReceiveTimeOut, ref string sReceive)
        {
            byte[] dataFileToPush;
            try
            {
                sReceive = "";
                if (sCommandHead.Length != 83)
                {
                    log.socket("ERROR: wrong commandhead: " + sCommandHead);
                    sReceive = "ERROR: wrong commandhead!" + sCommandHead;
                    return false;
                }
                    dataFileToPush = File2Bytes(sLocalFileName);
                    int FileSize = dataFileToPush.Length;
                    string sTimeStamp_Send = Environment.GetTimeStampMilSec(true);
                    string strToSend = sCommandHead + sTimeStamp_Send + sCommandContent.Replace("65535", FileSize.ToString());
                    byte[] byteToSend = System.Text.Encoding.Default.GetBytes(strToSend);
                    socClient.SendTimeout = iSendTimeOut;
                    try
                    {
                        lock (this)
                        {
                            socClient.Send(ByteArrayAppend(byteToSend, GV.bNewLine));
                        }

                        log.socket("【Send】" + strToSend);
                    }
                    catch
                    {
                        log.err("Send timeout!  " + strToSend);
                        return false;
                    }

                    string sContent_Rev = GetRecString(sCommandHead + sTimeStamp_Send, iReceiveTimeOut);
                    if (sContent_Rev.Length == 0)
                    {
                        log.err("Receive timeout!   " + sCommandHead + sTimeStamp_Send);
                        return false;
                    }

                    log.socket("【Receive】" + sCommandHead + sTimeStamp_Send + sContent_Rev);

                    SocketMsg mRevSocMsg = JsonConvert.DeserializeObject<SocketMsg>(sContent_Rev);
                    if (mRevSocMsg == null)
                    {
                        log.err("【Exception】返回Content空或非Json格式");
                        return false;

                    }
                    if (mRevSocMsg.result == null)
                    {
                        log.err("【Exception】返回Content有误");
                        return false;
                    }

                    if (mRevSocMsg.result != "P")
                    {
                        log.err("【Error】返回result失败");
                        return false;
                    }
                    if (mRevSocMsg.message == null)
                    {
                        log.err("【Exception】返回Content有误");
                        return false;
                    }
                    sReceive = mRevSocMsg.message;




                    //发数据
                    string strToSend2 = "FILE_PUSH                                                                       DAT" + sTimeStamp_Send;
                    byte[] byteToSend2 = dataFileToPush;
                    byteToSend2 = ByteArrayAppend(System.Text.Encoding.Default.GetBytes(strToSend2), byteToSend2);
                    //Client.socket.Send(ByteArrayAppend(byteToSend, GV.bNewLine));
                    lock (this)
                    {
                        socClient.Send(byteToSend2);//去掉换行符
                    }
                    log.socket("【Send】" + strToSend2 + " +   FileSzie " + FileSize.ToString() + "Bytes");


                    sContent_Rev = GetRecString(strToSend2, iReceiveTimeOut);
                    if (sContent_Rev.Length == 0)
                    {
                        log.err("Receive timeout!   " + strToSend2);
                        return false;
                    }

                    log.socket("【Receive】" + strToSend2 + sContent_Rev);
                    mRevSocMsg = JsonConvert.DeserializeObject<SocketMsg>(sContent_Rev);
                    if (mRevSocMsg == null)
                    {
                        log.err("【Exception】返回Content空或非Json格式");
                        return false;

                    }
                    if (mRevSocMsg.result == null)
                    {
                        log.err("【Exception】返回Content有误");
                        return false;
                    }

                    if (mRevSocMsg.result != "P")
                    {
                        log.err("【Error】返回result失败");
                        return false;
                    }
                    if (mRevSocMsg.message == null)
                    {
                        log.err("【Exception】返回Content有误");
                        return false;
                    }
                    sReceive = mRevSocMsg.message;
                

            }
            catch (Exception exp)
            {
                log.err("异常：" + exp.ToString());
                return false;
            }
            return true;
        }

        public bool SocketAppPushFile(string sLocalFileName, string sCommandHead, string sCommandContent, int iSendTimeOut, int iReceiveTimeOut, ref string sErrorMsg)
        {
            SocketAudioTest.RegisterLogger(this.log);
            log.socket("Push File " + sLocalFileName + " To Phone");
            return SocketAudioTest.SocketAppPushFile(socClient, sLocalFileName, sCommandHead, sCommandContent, iSendTimeOut, iReceiveTimeOut, ref sErrorMsg);
        }

        public bool SocketAppSendOnly(string sCommandHead, string sCommandContent, int iSendTimeOut)
        {
            try
            {
                if (sCommandHead.Length != 83)
                {
                    log.socket("ERROR: wrong commandhead: " + sCommandHead);
                    return false;
                }

                GV.sTimeStamp_Send = GetTimeStampMilSec(true);
                string cmdStr = sCommandHead + GV.sTimeStamp_Send + sCommandContent;
                byte[] byteToSend = System.Text.Encoding.Default.GetBytes(cmdStr);
                byte[] NewLine = { 10 };
                socClient.SendTimeout = 2000;
                socClient.Send(ByteArrayAppend(byteToSend, NewLine));
                log.socket("【Send】" + cmdStr);
            }
            catch
            {
                log.socket("socket send timeout! ");
                return false;
            }
            return true;
        }

        //指令池sendonly
        public bool SocketAppSendOnly2(string sCommandHead, string sCommandContent, int iSendTimeOut, out string sTimeStamp_Send)
        {
            sTimeStamp_Send = "";
            try
            {
                if (sCommandHead.Length != 83)
                {
                    log.socket("ERROR: wrong commandhead: " + sCommandHead);
                    return false;
                }

                sTimeStamp_Send = GetTimeStampMilSec(true);
                string cmdStr = sCommandHead + sTimeStamp_Send + sCommandContent;
                byte[] byteToSend = System.Text.Encoding.Default.GetBytes(cmdStr);
                byte[] NewLine = { 10 };
                socClient.SendTimeout = iSendTimeOut;
                try
                {
                    lock (this)
                    {
                        socClient.Send(ByteArrayAppend(byteToSend, NewLine));
                    }
                    log.socket("【Send】" + cmdStr);
                }
                catch
                {
                    return true;
                }
            }
            catch
            {
                log.socket("socket send timeout! ");
                return true;
            }
            return true;
        }

        //指令池ReceiveOnly
        public bool SocketAppReceiveOnly2(string sTimeStamp_Send, string sCommandHead, string sCommandContent, int iReceiveTimeOut, ref string sReceive)
        {

            string sContent_Rev = GetRecString(sCommandHead + sTimeStamp_Send, iReceiveTimeOut);
            if (sContent_Rev.Length == 0)
            {
                sReceive = "Receive timeout!    " + sCommandHead + sTimeStamp_Send;
                return false;
            }

            log.socket("【Receive】" + sCommandHead + sTimeStamp_Send + sContent_Rev);
            SocketMsg mRevSocMsg = JsonConvert.DeserializeObject<SocketMsg>(sContent_Rev);
            if (mRevSocMsg == null)
            {
                log.err("【Exception】返回Content空或非Json格式");
                return false;

            }
            if (mRevSocMsg.result == null)
            {
                log.err("【Exception】返回Content有误");
                return false;
            }

            if (mRevSocMsg.result != "P")
            {
                log.err("【Error】返回result失败");
                return false;
            }
            if (mRevSocMsg.message == null)
            {
                log.err("【Exception】返回Content有误");
                return false;
            }
            sReceive = mRevSocMsg.message;

            return true;
        }

        public bool SocketAppColseClient(string sCommandHead, string sCommandContent, int iTimeOut)
        {
            try
            {
                IPEndPoint end = (IPEndPoint)socClient.LocalEndPoint;
            }
            catch
            {
                log.msg("Client not exist.");
                return true;
            }
            if (SocketAppSendOnly(sCommandHead, sCommandContent, iTimeOut))
            {
                try
                {
                    Thread.Sleep(1000);
                    socClient.Close();
                    return true;
                }
                catch (Exception exp)
                {
                    log.socket("close client error:" + exp.ToString());
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        //-------------------------LP Code JSON Operation--------------------

        public string JSONSerialCode(Js code)
        {
            string jstr = "";
            jstr = Newtonsoft.Json.JsonConvert.SerializeObject(code);
            return jstr;
        }

        public string MCSONSerialCode(mccheck code)
        {
            string jstr = "";
            jstr = Newtonsoft.Json.JsonConvert.SerializeObject(code);
            return jstr;
        }

        public rs JSONDserialCode(string code)
        {
            rs jstr = Newtonsoft.Json.JsonConvert.DeserializeObject<rs>(code);
            return jstr;
        }

        public mcquery MCSONDserialCode(string code)
        {
            mcquery jstr = Newtonsoft.Json.JsonConvert.DeserializeObject<mcquery>(code);
            return jstr;
        }

        public query_netcode JSONDserialNetCode(string code)
        {
            query_netcode jstr = Newtonsoft.Json.JsonConvert.DeserializeObject<query_netcode>(code);
            return jstr;
        }
        //-------------------------LP Code JSON Operation--------------------
    }
}
