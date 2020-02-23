using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using Cease.Addins.Log;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json;
using Cease.Addins.AudioSW;

namespace AudioSW
{
    public class SocketAudioTest
    {
        private static InterfaceLog log;

        public static void RegisterLogger(InterfaceLog _log)
        {
            log = _log;
        }

        public static bool GetFile(Socket socClient, string sFileName,int iSendTimeOut,int iReceiveTimeOut, ref string errMsg)
        {
            if (socClient != null)
            {

                try
                {
                    string cmd = "";
                    if (sFileName == "main.wav")
                    {
                        cmd = "75 250 4 56 0 26 0 47 115 100 99 97 114 100 47 97 117 100 105 111 116 101 115 116 47 109 97 105 110 46 119 97 118";
                    }
                    else if (sFileName == "second.wav")
                    {
                        cmd = "75 250 4 56 0 28 0 47 115 100 99 97 114 100 47 97 117 100 105 111 116 101 115 116 47 115 101 99 111 110 100 46 119 97 118";
                    }
                    else if (sFileName == "aanc.wav")
                    {
                        cmd = "75 250 4 56 0 26 0 47 115 100 99 97 114 100 47 97 117 100 105 111 116 101 115 116 47 97 97 110 99 46 119 97 118";
                    }
                    else if (sFileName == "aanc_sweep.wav")
                    {
                        cmd = "75 250 4 56 0 32 0 47 115 100 99 97 114 100 47 97 117 100 105 111 116 101 115 116 47 97 97 110 99 95 115 119 101 101 112 46 119 97 118";
                    }
                    else if (sFileName == "Limits.ini")
                    {
                        cmd = "75 250 4 56 0 27 0 47 115 100 99 97 114 100 47 97 117 100 105 111 116 101 115 116 47 76 105 109 105 116 46 105 110 105";
                    }
                    else
                    {
                        errMsg = "filename "+sFileName+" is wrong";
                        return false;
                    }
                    byte[] newline = { 10 };
                    string[] byteAarray = cmd.Split(' ');
                    byte[] byteToSend;
                    byteToSend = new byte[byteAarray.Length];
                    for (int i = 0; i < byteAarray.Length; i++)
                    {
                        try
                        {
                            int tmp = Convert.ToInt32(byteAarray[i]);
                            if (tmp < 256)
                            {
                                byteToSend[i] = Convert.ToByte(tmp);
                            }
                            else
                            {
                            }
                        }
                        catch (Exception ee)
                        {

                        }
                    }
                    byteToSend = System.Text.Encoding.Default.GetBytes(cmd);

                    List<byte> lTemp = new List<byte>();
                    lTemp.AddRange(byteToSend);
                    lTemp.AddRange(newline);
                    byteToSend = new byte[lTemp.Count];
                    lTemp.CopyTo(byteToSend);
                    socClient.SendTimeout = iSendTimeOut;
                    try
                    {
                        log.socket("Sending pull wav command...");
                        socClient.Send(byteToSend);
                    }
                    catch(Exception exp)
                    {
                        throw new Exception(exp.ToString());
                        return false;
                    }

                    socClient.ReceiveTimeout = iReceiveTimeOut;
                    byte[] arrServerRecMsg = new byte[1024];
                    int length = socClient.Receive(arrServerRecMsg);
                    log.socket("Receive pull wav response...");
                    string str = "";
                    for (int i = 0; i < length; i++)
                    {
                        str += Convert.ToChar(arrServerRecMsg[i]).ToString();
                    }
                    log.socket(str);
                    if (!str.Contains("75 250 4 56 0 2 0 45 49") && str.Contains("75 250 4 56 0"))
                    {
                        string str1 = str.Substring(18);
                        string[] str2 = str1.Split(' ');
                        List<int> int1 = new List<int>();
                        foreach (string s in str2)
                        {
                            try
                            {
                                int1.Add(int.Parse(s));
                            }
                            catch
                            {
                                
                            }
                        }
                        List<byte> byte1 = new List<byte>();
                        foreach (int i in int1)
                        {
                            byte1.Add(Convert.ToByte(i));
                        }
                        string str3 = Encoding.ASCII.GetString(byte1.ToArray());
                        int totalsize = int.Parse(str3);
                        log.socket("wav file total size is "+str3);
                        int sizesum = 0;
                        byte[] ReceiveBytes = new byte[0];

                        while (true)
                        {
                            arrServerRecMsg = new byte[1024];
                            length = socClient.Receive(arrServerRecMsg);
                            //LogUpdate("Receive size:" + length.ToString());
                            sizesum += length;
                            byte[] tmpbyte = SplitArray(arrServerRecMsg, 0, length - 1);
                            ReceiveBytes = ByteArrayAppend(ReceiveBytes, tmpbyte);
                            if (totalsize == sizesum)
                            {
                                string sss = "";
                                if (File.Exists("D:\\" + sFileName))
                                {
                                    File.Delete("D:\\" + sFileName);
                                }
                                if (Bytes2File(ReceiveBytes, "D:\\" + sFileName, ref sss))
                                {
                                    errMsg = "success to save file to " + "D:\\" + sFileName;
                                    return true;
                                }
                                else
                                {
                                    errMsg = sss;
                                    return false;
                                }

                            }
                        }
                    }
                    else
                    {
                        errMsg = "file is not exist in phone!";
                        log.socket(str);
                        return false;
                    }
                }
                catch(Exception exp)
                {
                    errMsg = "error:"+exp.ToString();
                    return false;
                }

                return true;
            }
            else
            {

                errMsg = "socket is null";
                return false;
            }
        }

        public static bool PushFile(Socket socClient, string sLocalFileName, string sCommandHead, string sCommandContent, int iSendTimeOut, int iReceiveTimeOut, ref string errMsg)
        {
            if (socClient != null)
            {
                byte[] dataFileToPush = File2Bytes(sLocalFileName);
                int FileSize = dataFileToPush.Length;
                byte[] byteToSend = System.Text.Encoding.Default.GetBytes(sCommandHead + sCommandContent);
                return true;
            }
            else
            {

                errMsg = "socket is null";
                return false;
            }
        }

        public static bool SocketAppPullFile(Socket socClient, string sFileName, string sCommandHead, string sCommandContent, int iSendTimeOut, int iReceiveTimeOut, ref string errMsg)
        {
            if (socClient != null)
            {
                try
                {
                    GV.sPullPushFileName = sFileName;
                    GV.sTimeStamp_Send = Environment.GetTimeStampMilSec(true);
                    string strToSend = sCommandHead + GV.sTimeStamp_Send + sCommandContent;
                    byte[] byteToSend = System.Text.Encoding.Default.GetBytes(strToSend);
                    socClient.SendTimeout = 5000;
                    socClient.Send(ByteArrayAppend(byteToSend, GV.bNewLine));
                    log.socket("【Send】" + strToSend);
                    socClient.ReceiveTimeout = iReceiveTimeOut;
                    byte[] arrServerRecMsg = new byte[1024];
                    int totalLength = 0;
                    int length = 0;
                    byte[] buffer = null;
                    while (true)
                    {
                        try
                        {
                            arrServerRecMsg = new byte[1024];
                            length = socClient.Receive(arrServerRecMsg);
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
                                if (arrServerRecMsg[length - 1] == GV.bNewLine[0])      //  '\n'结束符
                                {
                                    break;
                                }
                            }
                            else
                            {
                                Thread.Sleep(50);
                            }


                        }
                        catch
                        {
                            log.err("【Exception】超时未返回消息");
                            return false;

                        }
                    }

                    buffer = SplitArray(buffer, 0, buffer.Length - 2);
                    Environment.AnalysisDataToMsg_Rev(buffer);
                    string strstr = System.Text.Encoding.Default.GetString(buffer);
                    log.socket("【Receive】" + strstr);

                    if (totalLength >= 100)
                    {
                        if (GV.sTimeStamp_Send == GV.sTimeStamp_Rev)
                        {
                            if (!GV.sContent_Rev.Contains(":\"P\""))
                            {
                                log.err("【Error】不允许pull");
                                return false;
                            }
                            Environment.JsonDeserialize(GV.sContent_Rev);
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

                        }
                        else
                        {
                            log.err("【Error】返回时间戳不是当前发送的时间戳");
                        }
                    }

                    Thread.Sleep(100);

                    //请求数据
                    strToSend = "FILE_PULL                                                                       DAT" + GV.sTimeStamp_Send;
                    byteToSend = System.Text.Encoding.Default.GetBytes(strToSend);
                    socClient.Send(byteToSend);//去掉换行符
                    log.socket("【Send】" + strToSend);

                    //接收数据
                    GV.iFileSize = int.Parse(GV.mRevSocMsg.size);
                    GV.iFileSizeSum = 0;
                    GV.bReceiveFileByte = new byte[0];
                    arrServerRecMsg = new byte[1024];
                    length = socClient.Receive(arrServerRecMsg);
                    Environment.AnalysisDataToMsg_Rev(arrServerRecMsg);
                    if (GV.sTimeStamp_Send != GV.sTimeStamp_Rev)            //校验是否为有效数据，否则抛弃
                    {
                        arrServerRecMsg = new byte[10240];
                        socClient.Receive(arrServerRecMsg);
                        log.msg("【Message】" + "丢弃数据");
                        return false;
                    }

                    if (length > 100)
                    {
                        GV.bReceiveFileByte = SplitArray(arrServerRecMsg, 100, length);
                        GV.iFileSizeSum = length - 100;
                    }

                    while (true)
                    {
                        arrServerRecMsg = new byte[1024];            //可根据文件大小正比例开缓存，文件越大缓存越大
                        length = socClient.Receive(arrServerRecMsg);
                        //LogUpdate("Receive size:" + length.ToString());
                        GV.iFileSizeSum += length;
                        byte[] tmpbyte = SplitArray(arrServerRecMsg, 0, length - 1);
                        GV.bReceiveFileByte = ByteArrayAppend(GV.bReceiveFileByte, tmpbyte);
                        if (GV.iFileSizeSum >= GV.iFileSize)
                        {
                            string sss = "";
                            log.msg("【Message】total size :" + (GV.iFileSizeSum - 1).ToString());//返回的文件流最后一位为10（'\n'）应去除
                            if (File.Exists("D:\\" + GV.sPullPushFileName.Substring(GV.sPullPushFileName.LastIndexOf("/") + 1)))
                            {
                                File.Delete("D:\\" + GV.sPullPushFileName.Substring(GV.sPullPushFileName.LastIndexOf("/") + 1));
                            }
                            GV.bReceiveFileByte = SplitArray(GV.bReceiveFileByte, 0, GV.bReceiveFileByte.Length - 2);
                            Bytes2File(GV.bReceiveFileByte, "D:\\" + GV.sPullPushFileName.Substring(GV.sPullPushFileName.LastIndexOf("/") + 1), ref sss);
                            log.msg("【Message】save file to " + "D:\\" + GV.sPullPushFileName.Substring(GV.sPullPushFileName.LastIndexOf("/") + 1));

                            break;
                        }
                    }
                    return true;
                }
                catch (Exception exp)
                {
                    errMsg = exp.ToString();
                    return false;
                }
            }
            else
            {

                errMsg = "socket is null";
                return false;
            }

        }

        public static bool SocketAppPushFile(Socket socClient, string sLocalFileName, string sCommandHead, string sCommandContent, int iSendTimeOut, int iReceiveTimeOut, ref string errMsg)
        {
            if (socClient != null)
            {
                byte[] dataFileToPush = File2Bytes(sLocalFileName);
                int FileSize = dataFileToPush.Length;
                GV.sTimeStamp_Send = Environment.GetTimeStampMilSec(true);
                string strToSend = sCommandHead + GV.sTimeStamp_Send + sCommandContent.Replace("65535", FileSize.ToString());
                byte[] byteToSend = System.Text.Encoding.Default.GetBytes(strToSend);
                socClient.SendTimeout = 5000;
                socClient.Send(ByteArrayAppend(byteToSend, GV.bNewLine));
                log.socket("【Send】" + strToSend);

                socClient.ReceiveTimeout = iReceiveTimeOut;
                byte[] arrServerRecMsg = new byte[1024];
                int totalLength = 0;
                int length = 0;
                byte[] buffer = null;
                while (true)
                {
                    try
                    {
                        arrServerRecMsg = new byte[1024];
                        length = socClient.Receive(arrServerRecMsg);
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
                            if (arrServerRecMsg[length - 1] == GV.bNewLine[0])      //  '\n'结束符
                            {
                                break;
                            }
                        }
                        else
                        {
                            Thread.Sleep(50);
                        }


                    }
                    catch
                    {
                        log.err("【Exception】超时未返回消息");
                        return false;

                    }
                }

                buffer = SplitArray(buffer, 0, buffer.Length - 2);
                Environment.AnalysisDataToMsg_Rev(buffer);
                string strstr = System.Text.Encoding.Default.GetString(buffer);
                log.socket("【Receive】" + strstr);

                if (totalLength >= 100)
                {
                    if (GV.sTimeStamp_Send == GV.sTimeStamp_Rev)
                    {
                        if (!GV.sContent_Rev.Contains(":\"P\""))
                        {
                            log.err("【Error】不允许push");
                            return false;
                        }

                    }
                    else
                    {
                        log.err("【Error】返回时间戳不是当前发送的时间戳");
                    }
                }

                Thread.Sleep(100);

                //发数据
                strToSend = "FILE_PUSH                                                                       DAT" + GV.sTimeStamp_Send;
                byteToSend = dataFileToPush;
                byteToSend = ByteArrayAppend(System.Text.Encoding.Default.GetBytes(strToSend), byteToSend);
                //Client.socket.Send(ByteArrayAppend(byteToSend, GV.bNewLine));
                socClient.Send(byteToSend);//去掉换行符
                log.socket("【Send】" + strToSend + " +   FileSzie " + FileSize.ToString() + "Bytes");

                log.socket("【Message】Waiting response...");

                //Receive
                arrServerRecMsg = new byte[1024];
                totalLength = 0;
                length = 0;
                buffer = null;
                while (true)
                {
                    try
                    {
                        arrServerRecMsg = new byte[1024];
                        length = socClient.Receive(arrServerRecMsg);
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
                            if (arrServerRecMsg[length - 1] == GV.bNewLine[0])      //  '\n'结束符
                            {
                                break;
                            }
                        }
                        else
                        {
                            Thread.Sleep(50);
                        }


                    }
                    catch
                    {
                        log.err("【Exception】超时未返回消息");
                        return false;

                    }
                }

                buffer = SplitArray(buffer, 0, buffer.Length - 2);
                strstr = System.Text.Encoding.Default.GetString(buffer);
                log.socket("【Receive】" + strstr);

                if (totalLength >= 100)
                {
                    Environment.AnalysisDataToMsg_Rev(buffer);
                    if (GV.sTimeStamp_Send == GV.sTimeStamp_Rev)
                    {

                    }
                    else
                    {
                        log.err("【Error】返回时间戳不是当前发送的时间戳");
                    }
                }

                return true;
            }
            else
            {

                errMsg = "socket is null";
                return false;
            }
        }

        private static byte[] SplitArray(byte[] Source, int StartIndex, int EndIndex)
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

        private static byte[] ByteArrayAppend(byte[] bOriginal, byte[] bNew)
        {
            List<byte> lTemp = new List<byte>();
            lTemp.AddRange(bOriginal);
            lTemp.AddRange(bNew);
            bOriginal = new byte[lTemp.Count];
            lTemp.CopyTo(bOriginal);
            return bOriginal;
        }

        /// <summary>
        /// 二进制流转文件
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="savepath"></param>
        /// <param name="retStr"></param>
        /// <returns></returns>
        private static bool Bytes2File(byte[] buff, string savepath, ref string retStr)
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
    }
}
