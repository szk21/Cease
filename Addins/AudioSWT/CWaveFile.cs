using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AudioSWT
{
    public class CWav_file
    {

        private static byte[] riff = new byte[] { 0x52, 0x49, 0x46, 0x46 }; //4  “RIFF”的Ascii字符              0~3
        private static byte[] riffSize; //4 文件长度         4~7
        private static byte[] waveID = new byte[] { 0x57, 0x41, 0x56, 0x45 }; //4    文件格式，“WAVE”的Ascii字符 8~11
        private static byte[] fmtID = new byte[] { 0x66, 0x6D, 0x74, 0x20 };  //4  “fmt ”的Ascii字符 12~15
        private static byte[] notDefinition = new byte[] { 0x10, 0, 0, 0 }; //4       16~19
        private static byte[] waveType = new byte[] { 0x01, 0 };   //2     20~21        格式类别，1表示为PCM形式的声音数据
        private static byte[] channel;  //2          22~23      通道数
        private static byte[] sample;  //4           24~27      采样率
        private static byte[] send = new byte[] { 0x10, 0xB1, 0x2, 0x0 };   //4                28~31    每秒数据量=采样率*通道数*每样本的数据字节数，例如44100*1*2
        private static byte[] blockAjust = new byte[] { 4, 0 };  //2           32~33        数据块的调整数
        private static byte[] bitNum = new byte[] { 16, 0 };  //2               34~35
        private static byte[] unknown = new byte[] { 0x66, 0x61, 0x63, 0x74, 0x4, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 }; //12               36~47
        private static byte[] dataID = new byte[] { 0x64, 0x61, 0x74, 0x61 };   //4               48~51 data
        private static byte[] dataLength;  //4           52~55  

        public string czFileName;
        public Wav_Header wav_header;
        public Wav_file wav_file;

        public CWav_file(string sFileName)
        {
            czFileName = sFileName;
            wav_header = new Wav_Header();
            wav_file = new Wav_file();
            open_wav_file();
        }

        public struct Wav_Header
        {
            public long file_size;        //文件大小
            public short channel;            //通道数
            public long frequency;        //采样频率
            public long Bps;                //Byte率
            public short sample_num_bit;    //一个样本的位数
            public long data_size;        //数据大小
            public long num_sample;    //样本数量
            public byte[] head_data; //头数据
            public int idx_data; //"data"字符串的位置
            public double totalDuration;  //时长
            public int one_piont_data_cnt; //单点字节长度
        }

        public struct Wav_file
        {
            public string p_file;
            public long magic_num;
            public Wav_Header wav_header;
            public double[] R_value;
            public double[] L_value;
            public byte[] R_data;
            public byte[] L_data;
            public byte[] All_data;
            public double L_maxAmpl;
            public double R_maxAmpl;
            public List<byte[]> data_all_chn;
        }

        public void open_wav_file()
        {
            FileInfo wavfileinfo = new FileInfo(czFileName);
            wav_header.file_size = wavfileinfo.Length;

            FileStream fs = new FileStream(czFileName, FileMode.Open, FileAccess.Read);

            fs.Position = 22;
            byte[] datatmp = new byte[2];
            fs.Read(datatmp, 0, 2);
            wav_header.channel = (short)getHexToInt(datatmp);

            fs.Position = 24;
            datatmp = new byte[4];
            fs.Read(datatmp, 0, 4);
            wav_header.frequency = getHexToInt(datatmp);

            fs.Position = 28;
            datatmp = new byte[4];
            fs.Read(datatmp, 0, 4);
            wav_header.Bps = getHexToInt(datatmp);

            fs.Position = 34;
            datatmp = new byte[2];
            fs.Read(datatmp, 0, 2);
            wav_header.sample_num_bit = (short)getHexToInt(datatmp);

            //读取wav head 信息 先确定"data"的位置，再读取data的长度，目前有四种wav，
            //第一种是dataID在36~39位，第二种是在38~41位，第三种是在48~51位，第三种是在60~63位
            byte[] allData = null;

            fs.Position = 36;
            datatmp = new byte[4];
            fs.Read(datatmp, 0, 4);
            if (System.Text.Encoding.Default.GetString(datatmp) == "data")
            {
                fs.Position = 40;
                datatmp = new byte[4];
                fs.Read(datatmp, 0, 4);
                wav_header.data_size = getHexToInt(datatmp);
                wav_header.idx_data = 36;
                //获取左右声道数据
                allData = new byte[wav_header.data_size];
                wav_file.All_data = allData;
                fs.Position = 44;
                fs.Read(allData, 0, (int)wav_header.data_size);

                //文件头数据
                fs.Position = 0;
                datatmp = new byte[44];
                fs.Read(datatmp, 0, 44);
                wav_header.head_data = datatmp;
            }

            fs.Position = 38;
            datatmp = new byte[4];
            fs.Read(datatmp, 0, 4);
            if (System.Text.Encoding.Default.GetString(datatmp) == "data")
            {
                fs.Position = 42;
                datatmp = new byte[4];
                fs.Read(datatmp, 0, 4);
                wav_header.data_size = getHexToInt(datatmp);
                wav_header.idx_data = 38;
                //获取左右声道数据
                allData = new byte[wav_header.data_size];
                wav_file.All_data = allData;
                fs.Position = 46;
                fs.Read(allData, 0, (int)wav_header.data_size);

                //文件头数据
                fs.Position = 0;
                datatmp = new byte[46];
                fs.Read(datatmp, 0, 46);
                wav_header.head_data = datatmp;
            }

            fs.Position = 48;
            datatmp = new byte[4];
            fs.Read(datatmp, 0, 4);
            if (System.Text.Encoding.Default.GetString(datatmp) == "data")
            {
                fs.Position = 52;
                datatmp = new byte[4];
                fs.Read(datatmp, 0, 4);
                wav_header.data_size = getHexToInt(datatmp);
                wav_header.idx_data = 48;

                //获取左右声道数据
                allData = new byte[wav_header.data_size];
                wav_file.All_data = allData;
                fs.Position = 56;
                fs.Read(allData, 0, (int)wav_header.data_size);

                //文件头数据
                fs.Position = 0;
                datatmp = new byte[56];
                fs.Read(datatmp, 0, 56);
                wav_header.head_data = datatmp;

            }

            fs.Position = 60;
            datatmp = new byte[4];
            fs.Read(datatmp, 0, 4);
            if (System.Text.Encoding.Default.GetString(datatmp) == "data")
            {
                fs.Position = 64;
                datatmp = new byte[4];
                fs.Read(datatmp, 0, 4);
                wav_header.data_size = getHexToInt(datatmp);
                wav_header.idx_data = 60;

                //获取左右声道数据
                allData = new byte[wav_header.data_size];
                wav_file.All_data = allData;
                fs.Position = 68;
                fs.Read(allData, 0, (int)wav_header.data_size);

                //文件头数据
                fs.Position = 0;
                datatmp = new byte[68];
                fs.Read(datatmp, 0, 68);
                wav_header.head_data = datatmp;

            }

            fs.Close();


            double[] sample = null;
            try
            {
                wav_header.one_piont_data_cnt = wav_header.sample_num_bit / 8;
                sample = new double[allData.Length / wav_header.one_piont_data_cnt];
                sample = getSample(allData, wav_header.one_piont_data_cnt);
                wav_header.num_sample = sample.Length / wav_header.channel;
                wav_header.totalDuration = (double)wav_header.num_sample / ((double)wav_header.frequency);
            }
            catch (Exception exp)
            {
                string str = exp.ToString();
            }

            //如果是单声道
            if (wav_header.channel == 1)
            {
                wav_file.L_value = sample;
                wav_file.L_data = allData;
                wav_header.num_sample = sample.Length;
                double max_L = -1;
                for (int i = 0; i < sample.Length; i++)
                {
                    if (Math.Abs(sample[i]) > max_L)
                    {
                        max_L = Math.Abs(sample[i]);
                    }
                }
                wav_file.L_maxAmpl = max_L;
            }
            else if (wav_header.channel == 2)//如果是双声道
            {
                wav_file.L_value = new double[sample.Length / 2];
                wav_file.R_value = new double[sample.Length / 2];

                wav_file.L_data = new byte[allData.Length / 2];
                wav_file.R_data = new byte[allData.Length / 2];
                double max_L = -1;
                double max_R = -1;
                wav_header.num_sample = sample.Length / 2;
                for (int i = 0; i < sample.Length / 2; i++)
                {
                    wav_file.L_value[i] = sample[2 * i];
                    wav_file.R_value[i] = sample[2 * i + 1];
                    if (Math.Abs(sample[2 * i]) > max_L)
                    {
                        max_L = Math.Abs(sample[2 * i]);
                    }
                    if (Math.Abs(sample[2 * i + 1]) > max_R)
                    {
                        max_R = Math.Abs(sample[2 * i + 1]);
                    }
                }
                wav_file.L_maxAmpl = max_L;
                wav_file.R_maxAmpl = max_R;
                for (int i = 0; i < allData.Length / 4; i++)
                {
                    wav_file.L_data[2 * i] = allData[4 * i];
                    wav_file.L_data[2 * i + 1] = allData[4 * i + 1];
                    wav_file.R_data[2 * i] = allData[4 * i + 2];
                    wav_file.R_data[2 * i + 1] = allData[4 * i + 3];
                }
            }
            else if (wav_header.channel == 6)
            {
                wav_file.data_all_chn = new List<byte[]>();
                for (int i = 0; i < 6; i++)
                {
                    byte[] tmpBytes = new byte[allData.Length / 6];
                    wav_file.data_all_chn.Add(tmpBytes);
                }
                for (int i = 0; i < allData.Length / 12; i++)
                {
                    wav_file.data_all_chn[0][2 * i] = allData[12 * i];
                    wav_file.data_all_chn[0][2 * i + 1] = allData[12 * i + 1];
                    wav_file.data_all_chn[1][2 * i] = allData[12 * i + 2];
                    wav_file.data_all_chn[1][2 * i + 1] = allData[12 * i + 3];
                    wav_file.data_all_chn[2][2 * i] = allData[12 * i + 4];
                    wav_file.data_all_chn[2][2 * i + 1] = allData[12 * i + 5];
                    wav_file.data_all_chn[3][2 * i] = allData[12 * i + 6];
                    wav_file.data_all_chn[3][2 * i + 1] = allData[12 * i + 7];
                    wav_file.data_all_chn[4][2 * i] = allData[12 * i + 8];
                    wav_file.data_all_chn[4][2 * i + 1] = allData[12 * i + 9];
                    wav_file.data_all_chn[5][2 * i] = allData[12 * i + 10];
                    wav_file.data_all_chn[5][2 * i + 1] = allData[12 * i + 11];
                }

            }
            else if (wav_header.channel == 3)
            {
                wav_file.data_all_chn = new List<byte[]>();
                for (int i = 0; i < 3; i++)
                {
                    byte[] tmpBytes = new byte[allData.Length / 3];
                    wav_file.data_all_chn.Add(tmpBytes);
                }
                for (int i = 0; i < allData.Length / 6; i++)
                {
                    wav_file.data_all_chn[0][2 * i] = allData[6 * i];
                    wav_file.data_all_chn[0][2 * i + 1] = allData[6 * i + 1];
                    wav_file.data_all_chn[1][2 * i] = allData[6 * i + 2];
                    wav_file.data_all_chn[1][2 * i + 1] = allData[6 * i + 3];
                    wav_file.data_all_chn[2][2 * i] = allData[6 * i + 4];
                    wav_file.data_all_chn[2][2 * i + 1] = allData[6 * i + 5];
                }

            }


            //为啥等于这个值？
            wav_file.magic_num = 0x7FFE7FFE;

        }


        /// <summary>
        /// 十六进制的字符串转整数     //有效数据长度用4个字节表示 顺序是：低低 低高 高低 高高，例如B8 1F 02 00表示 00021FB8
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static int getHexToInt(byte[] x)
        {
            string retValue = "";
            for (int i = x.Length - 1; i >= 0; i--)
            {
                retValue += x[i].ToString("X2");
            }
            return Convert.ToInt32(retValue, 16);
        }

        /// <summary>
        /// 十六进制采样点转换为数值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="byteSample"></param>
        /// <returns></returns>
        static double[] getSample(byte[] x, int byteSample)
        {
            double[] retValue = new double[x.Length / byteSample];
            for (int i = 0; i < retValue.Length; i++)
            {
                string valuetmpStr = "";
                for (int j = (i + 1) * byteSample - 1; j >= i * byteSample; j--)
                {
                    string oneByteStr = x[j].ToString("X");
                    if (oneByteStr.Length == 1)
                    {
                        oneByteStr = "0" + oneByteStr;
                    }
                    valuetmpStr += oneByteStr;

                }
                // 16位的数据能表示的有符号整数范围是：二进制最小值 1111 1111 1111 1111（16进制FFFF，十进制 -32767） ，二进制最大值 0111 1111 1111 1111（16进制7FFF，十进制 32767），
                //以32768等于1为基准来计算数值，精度为1/32768=0.00003，保留五位小数
                //retValue[i] = Math.Round(((double)Convert.ToInt16(valuetmpStr, 16) / 32768D), 5);
                retValue[i] = (double)Convert.ToInt16(valuetmpStr, 16);
            }
            return retValue;
        }

        public static void Save2WAV(byte[] data, string fileName)
        {
            List<byte> dataTmp = new List<byte>();

            //叠加文件头和数据
            dataTmp.AddRange(riff);

            int fileTotalSize = 44 + data.Length;
            riffSize = IntegerToBytes(fileTotalSize, 4, 8);
            dataTmp.AddRange(riffSize);
            dataTmp.AddRange(waveID);
            dataTmp.AddRange(fmtID);
            dataTmp.AddRange(notDefinition);
            dataTmp.AddRange(waveType);
            channel = new byte[] { 0x01, 0 };
            dataTmp.AddRange(channel);
            int sampleRate = 44100;
            sample = IntegerToBytes(sampleRate, 4, 8);
            dataTmp.AddRange(sample);
            int send_rate = 44100 * 1 * 2;
            send = IntegerToBytes(send_rate, 4, 8);
            dataTmp.AddRange(send);
            blockAjust = new byte[] { 2, 0 };
            dataTmp.AddRange(blockAjust);
            dataTmp.AddRange(bitNum);
            //dataTmp.AddRange(unknown);
            dataTmp.AddRange(dataID);
            dataLength = IntegerToBytes(data.Length, 4, 8);
            dataTmp.AddRange(dataLength);

            dataTmp.AddRange(data);

            string err = "";
            Bytes2File(dataTmp.ToArray(), fileName, ref err);
        }


        /// <summary>
        /// 整数数值转Byte[]
        /// </summary>
        /// <param name="value"></param>
        /// <param name="BytesLen">位数</param>
        /// <param name="hexLen">十六进制字符串的长度</param>
        /// <returns></returns>
        private static byte[] IntegerToBytes(int value, int BytesLen, int hexLen)
        {
            string str = value.ToString("X" + hexLen.ToString());
            byte[] bytes = new byte[BytesLen];
            for (int i = 0; i < BytesLen; i++)
            {
                bytes[i] = (byte)Convert.ToInt32(str.Substring(hexLen - (2 * (i + 1)), 2), 16);
            }

            return bytes;
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
            if (!Directory.Exists(savepath.Substring(0, savepath.LastIndexOf("\\"))))
            {
                Directory.CreateDirectory(savepath.Substring(0, savepath.LastIndexOf("\\")));
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

        public void CutWaveFile(double percentage)
        {

            if (percentage <= 0 || percentage >= 1)
            {
                File.Copy(czFileName, czFileName.Replace(".wav", "_cut_0.wav"), true);
                File.Copy(czFileName, czFileName.Replace(".wav", "_cut_1.wav"), true);
                return;
            }

            double totalsec = wav_header.totalDuration;
            double startsec = totalsec * percentage;
            int startPiontPos = (int)(wav_header.frequency * startsec);
            int startDataPos = startPiontPos * wav_header.one_piont_data_cnt;

            byte[] cutData_0 = SplitArray(wav_file.L_data, 0, startDataPos);
            CWav_file.Save2WAV(cutData_0, czFileName.Replace(".wav", "_cut_0.wav"));
            byte[] cutData_1 = SplitArray(wav_file.L_data, startDataPos, wav_file.L_data.Length - 1);
            CWav_file.Save2WAV(cutData_1, czFileName.Replace(".wav", "_cut_1.wav"));
        }

        /// <summary>
        /// 截取Byte数组
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="StartIndex"></param>
        /// <param name="EndIndex"></param>
        /// <returns></returns>
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
    }
}
