using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace AudioSWT
{
    class AudioAnalyzer_BlackShark
    {
        public static bool WavFileProcess(string sourceWAVFile, string sourceWAVFileConfig, string recordWAVFile,int Channel,string Unit, bool removeHeaderTail,out double[] AMPL, out double[] THD, out double[] RB, out string errmsg)
        {
            AMPL = null;
            THD = null;
            RB = null;
            errmsg = "";

            if (!File.Exists(sourceWAVFile))
            {
                errmsg = "sourceWAVFile:" + sourceWAVFile + " is not exists!"; 
                return false;
            }

            if (!File.Exists(sourceWAVFileConfig))
            {
                errmsg = "sourceWAVFileConfig:" + sourceWAVFileConfig + " is not exists!"; 
                return false;
            }

            if (!File.Exists(recordWAVFile))
            {
                errmsg = "recordWAVFile:" + recordWAVFile + " is not exists!"; 
                return false;
            }

            //读取音源配置文件，获取频率列表、频率起点列表、频率采样点数、频率波形数
            _filePath = sourceWAVFileConfig;

            string[] freq = ReadFromFile("INFO", "FREQ", "").Split(',');
            string[] freq_pos = ReadFromFile("INFO", "FREQ_POS", "").Split(',');
            string[] freq_sample = ReadFromFile("INFO", "FREQ_SAMPLE_NUM", "").Split(',');
            string[] freq_waveform = ReadFromFile("INFO", "FREQ_WAVEFORM_NUM", "").Split(',');

            if (freq.Length == 1 || freq_pos.Length == 1 || freq_sample.Length == 1 || freq_waveform.Length == 1)
            {
                errmsg = "sourceWAVFileConfig: config content error!";
                return false;
            }

            AMPL = new double[freq.Length];
            THD = new double[freq.Length];
            RB = new double[freq.Length];

            List<double> freq_list = new List<double>();
            List<int> freq_pos_list = new List<int>();
            List<int> freq_sample_count_list = new List<int>();
            List<int> freq_waveform_count_list = new List<int>();
            for (int i = 0; i < freq.Length; i++)
            {
                try
                {
                    freq_list.Add(double.Parse(freq[i]));
                    freq_pos_list.Add(int.Parse(freq_pos[i]));
                    freq_sample_count_list.Add(int.Parse(freq_sample[i]));
                    freq_waveform_count_list.Add(int.Parse(freq_waveform[i]));
                }
                catch
                {
                    errmsg = "sourceWAVFileConfig: config content error!";
                    return false;
                }
            }

            //计算采样数据长度的N次冥，65536，32768，16384，8192，4096，
            int size_fftN = 1;
            int kk = 0;
            while (size_fftN < 8192)
            {
                size_fftN <<= 1;
                kk++;
            }
            int maxSampleNum = 0;
            Double[] pRealIn = new double[size_fftN + 1],
    pImagIn = new double[size_fftN + 1],
    pRealOut = new double[size_fftN + 1],
    pImagOut = new double[size_fftN + 1],
    pAmpl = new double[size_fftN >> 1];
            /*
            //*********************************************************** 读取音源文件 *************************************************************
            CWav_file sourceWav = new CWav_file(sourceWAVFile);

            List<double> source_value = new List<double>();
            if (Channel == 0)
            {
                source_value = sourceWav.wav_file.L_value.ToList();
            }
            else
            {
                source_value = sourceWav.wav_file.R_value.ToList();
            }

            Dictionary<double, List<double>> dic_source_freq_samples_collector = new Dictionary<double, List<double>>();
            Dictionary<double, double> dic_source_freq_ampl_collector = new Dictionary<double, double>();
            Dictionary<double, double> dic_source_freq_thd_collector = new Dictionary<double, double>();
            Dictionary<double, double> dic_source_freq_rb_collector = new Dictionary<double, double>();


            //分段截取


            for (int i = 0; i < freq_list.Count; i++)
            {
                int samples2cut = 0;
                List<double> freq_all_samples = new List<double>();

                if (freq_list[i] <= 1000)
                {
                    //取10个波形
                    samples2cut = (int)(1 / freq_list[i] * sourceWav.wav_header.frequency * 10);
                }
                else
                {
                    //取足够10ms
                    for (int j = 1; j < freq_waveform_count_list[i]; j++)
                    {
                        if (1 / freq_list[i] * j * 1000 > 10)
                        {
                            samples2cut = (int)(1 / freq_list[i] * sourceWav.wav_header.frequency * j);
                            break;
                        }
                    }
                }

                if (samples2cut > freq_sample_count_list[i])
                {
                    samples2cut = freq_sample_count_list[i];
                }
                freq_all_samples = source_value.GetRange(freq_pos_list[i], samples2cut);


                dic_source_freq_samples_collector.Add(freq_list[i], freq_all_samples);

                if (samples2cut > maxSampleNum)
                {
                    maxSampleNum = samples2cut;
                }

            }





            foreach (double f in dic_source_freq_samples_collector.Keys)
            {

                pRealIn = new double[size_fftN + 1];
                pImagIn = new double[size_fftN + 1];
                pRealOut = new double[size_fftN + 1];
                pImagOut = new double[size_fftN + 1];
                pAmpl = new double[(size_fftN + 1) / 2];

                double[] window_data = new double[dic_source_freq_samples_collector[f].Count];
                FFT.hannWin(dic_source_freq_samples_collector[f].Count, ref window_data);



                // 实部，虚部的数据归一化
                for (int i = 0; i < dic_source_freq_samples_collector[f].Count; i++)
                {
                    //加窗

                    pRealIn[i] = dic_source_freq_samples_collector[f][i] * window_data[i];
                }

                //fft
                FFT.FFT_Calc(pRealIn, pImagIn, size_fftN, kk, ref pRealOut, ref pImagOut, 0, 0);
                //算频谱
                FFT.FFT_Spectrum((size_fftN + 1) / 2, pRealOut, pImagOut, ref pAmpl);

                int indice = (int)(f * (double)size_fftN / (double)sourceWav.wav_header.frequency);
                double ampl = pAmpl[indice];
                //取周边峰值
                for (int i = indice - 5; i <= indice + 5; i++)
                {
                    if (pAmpl[i] > ampl)
                    {
                        ampl = pAmpl[i];
                    }
                }
                double pc = ampl / 32768;
                double pref = 20 * 0.000001;

                //声压
                double metervalue = (20.0 * Math.Log10((pc / pref)));

                List<double> Hlist = new List<double>();
                for (int i = 0; i < 34; i++)
                {
                    if (f * (i + 2) > 22000)
                    {
                        continue;
                    }
                    int idx = (int)(f * (i + 2) * (double)size_fftN / (double)sourceWav.wav_header.frequency);
                    double harmonic = pAmpl[idx];
                    for (int j = idx - 5; j <= idx + 5; j++)
                    {
                        if (pAmpl[j] > harmonic)
                        {
                            harmonic = pAmpl[j];
                        }
                    }
                    Hlist.Add(harmonic);
                }

                double total_harmonic = 0;
                for (int i = 0; i < Hlist.Count; i++)
                {
                    total_harmonic += Math.Pow(Hlist[i], 2);
                }

                total_harmonic = Math.Sqrt(total_harmonic);



                double total = Math.Pow(ampl, 2);


                int THD_Count = 4;
                if (Hlist.Count < THD_Count)
                {
                    THD_Count = Hlist.Count;
                }


                total = Math.Sqrt(total);

                double thd = total_harmonic / total * 100;

                double total_rb_harmonic = 0;
                double total_rb = Math.Pow(ampl, 2);
                double rb = 0;
                if (Hlist.Count > 9)
                {
                    for (int i = 9; i < Hlist.Count; i++)
                    {
                        total_rb_harmonic += Math.Pow(Hlist[i], 2);
                    }

                    total_rb = Math.Sqrt(total_rb + total_rb_harmonic);
                    total_rb_harmonic = Math.Sqrt(total_rb_harmonic);
                    rb = total_rb_harmonic / total_rb * 100;
                }

                dic_source_freq_ampl_collector.Add(f, metervalue);
                dic_source_freq_thd_collector.Add(f, thd);
                dic_source_freq_rb_collector.Add(f, rb);

            }
            */
            //*********************************************************** 读取录音文件 *************************************************************

            CWav_file recordWav = new CWav_file(recordWAVFile);

            List<double> record_value = new List<double>();
            double maxValue=-999;
            if (Channel == 0)
            {
                record_value = recordWav.wav_file.L_value.ToList();
                maxValue=recordWav.wav_file.L_maxAmpl;
            }
            else
            {
                record_value = recordWav.wav_file.R_value.ToList();
                maxValue=recordWav.wav_file.R_maxAmpl;
            }

            Dictionary<double, List<double>> dic_record_freq_samples_collector = new Dictionary<double, List<double>>();
            Dictionary<double, double> dic_record_freq_ampl_collector = new Dictionary<double, double>();
            Dictionary<double, double> dic_record_freq_thd_collector = new Dictionary<double, double>();
            Dictionary<double, double> dic_record_freq_rb_collector = new Dictionary<double, double>();
            Dictionary<double, double> dic_freq_dBV_collector = new Dictionary<double, double>();

            //掐头去尾去噪处理
            if (removeHeaderTail)
            {
                string err="";
                record_value = RemoveHeaderTail(record_value, 0.30, maxValue,441, ref err); //门限值是10%
                if (record_value.Count == 0)
                {
                    errmsg = err;
                    return false;
                }
            }

            //分段截取
            maxSampleNum = 0;

            for (int i = 0; i < freq_list.Count; i++)
            {
                int samples2cut = 0;
                List<double> freq_all_samples = new List<double>();

                if (freq_list[i] <= 1000)
                {
                    //取10个波形
                    samples2cut = (int)(1 / freq_list[i] * recordWav.wav_header.frequency * 10);
                }
                else
                {
                    //取足够10ms
                    for (int j = 1; j < freq_waveform_count_list[i]; j++)
                    {
                        if (1 / freq_list[i] * j * 1000 > 10)
                        {
                            samples2cut = (int)(1 / freq_list[i] * recordWav.wav_header.frequency * j);
                            break;
                        }
                    }
                }

                if (samples2cut > freq_sample_count_list[i])
                {
                    samples2cut = freq_sample_count_list[i];
                }
                freq_all_samples = record_value.GetRange(freq_pos_list[i], samples2cut);


                dic_record_freq_samples_collector.Add(freq_list[i], freq_all_samples);

                if (samples2cut > maxSampleNum)
                {
                    maxSampleNum = samples2cut;
                }

            }


            foreach (double f in dic_record_freq_samples_collector.Keys)
            {

                //--------------幅值计算-------------
                //找零点
                List<int> idxOfAllZeroPoint = new List<int>();
                for (int i = 0; i < dic_record_freq_samples_collector[f].Count - 1; i++)
                {
                    if (dic_record_freq_samples_collector[f][i] * dic_record_freq_samples_collector[f][i + 1] < 0)
                    {
                        idxOfAllZeroPoint.Add(i);
                    }
                }
                //找峰值
                List<double> peakValueCollector = new List<double>();
                for (int i = 0; i < idxOfAllZeroPoint.Count - 1; i++)
                {
                    int start = idxOfAllZeroPoint[i];
                    int end = idxOfAllZeroPoint[i + 1];
                    double max = -65536;
                    for (int j = start; j <= end; j++)
                    {
                        if (Math.Abs(dic_record_freq_samples_collector[f][j]) > max)
                        {
                            max = Math.Abs(dic_record_freq_samples_collector[f][j]);
                        }
                    }
                    peakValueCollector.Add(max / 32768);
                }
                //算峰值平均值
                double sum = 0;
                double aver = 0;
                foreach (double p in peakValueCollector)
                {
                    sum += p;
                }
                aver = sum / (double)peakValueCollector.Count;
                double dBV = 20 * Math.Log10(aver);
                dic_freq_dBV_collector.Add(f, dBV + 94);

                //--------------幅值计算-------------


                pRealIn = new double[size_fftN + 1];
                pImagIn = new double[size_fftN + 1];
                pRealOut = new double[size_fftN + 1];
                pImagOut = new double[size_fftN + 1];
                pAmpl = new double[(size_fftN + 1) / 2];


                double[] window_data = new double[dic_record_freq_samples_collector[f].Count];
                FFT.hannWin(dic_record_freq_samples_collector[f].Count, ref window_data);



                // 实部，虚部的数据归一化
                for (int i = 0; i < dic_record_freq_samples_collector[f].Count; i++)
                {
                    //加窗

                    pRealIn[i] = dic_record_freq_samples_collector[f][i] * window_data[i];
                }

                //fft
                FFT.FFT_Calc(pRealIn, pImagIn, size_fftN, kk, ref pRealOut, ref pImagOut, 0, 0);
                //算频谱
                FFT.FFT_Spectrum((size_fftN + 1) / 2, pRealOut, pImagOut, ref pAmpl);

                int indice = (int)(f * (double)size_fftN / (double)recordWav.wav_header.frequency);
                double ampl = pAmpl[indice];
                //取周边峰值
                for (int i = indice - 5; i <= indice + 5; i++)
                {
                    if (pAmpl[i] > ampl)
                    {
                        ampl = pAmpl[i];
                    }
                }
                double pc = ampl / 32768;
                double pref = 20 * 0.000001;

                //声压
                double metervalue = (20.0 * Math.Log10((pc / pref)));

                List<double> Hlist = new List<double>();
                for (int i = 0; i < 34; i++)
                {
                    if (f * (i + 2) > 22000)
                    {
                        continue;
                    }
                    int idx = (int)(f * (i + 2) * (double)size_fftN / (double)recordWav.wav_header.frequency);
                    double harmonic = pAmpl[idx];
                    for (int j = idx - 5; j <= idx + 5; j++)
                    {
                        if (pAmpl[j] > harmonic)
                        {
                            harmonic = pAmpl[j];
                        }
                    }
                    Hlist.Add(harmonic);
                }

                double total_harmonic = 0;
                for (int i = 0; i < Hlist.Count; i++)
                {
                    total_harmonic += Math.Pow(Hlist[i], 2);
                }

                total_harmonic = Math.Sqrt(total_harmonic);



                double total = Math.Pow(ampl, 2);


                int THD_Count = 4;
                if (Hlist.Count < THD_Count)
                {
                    THD_Count = Hlist.Count;
                }


                total = Math.Sqrt(total);

                double thd = total_harmonic / total * 100;

                double total_rb_harmonic = 0;
                double total_rb = Math.Pow(ampl, 2);
                double rb = 0;
                if (Hlist.Count > 9)
                {
                    for (int i = 9; i < Hlist.Count; i++)
                    {
                        total_rb_harmonic += Math.Pow(Hlist[i], 2);
                    }

                    total_rb = Math.Sqrt(total_rb + total_rb_harmonic);
                    total_rb_harmonic = Math.Sqrt(total_rb_harmonic);
                    rb = total_rb_harmonic / total_rb * 100;
                }

                dic_record_freq_ampl_collector.Add(f, metervalue);
                dic_record_freq_thd_collector.Add(f, thd);
                dic_record_freq_rb_collector.Add(f, rb);

            }

            List<double> amplData = new List<double>();
            List<double> thdData = new List<double>();
            List<double> RBData = new List<double>();
            List<double> dBVData = new List<double>();
            foreach (double key in dic_record_freq_ampl_collector.Keys)
            {
                amplData.Add(dic_record_freq_ampl_collector[key]);
            }
            foreach (double key in dic_record_freq_thd_collector.Keys)
            {
                thdData.Add(dic_record_freq_thd_collector[key]);
            }
            foreach (double key in dic_record_freq_rb_collector.Keys)
            {
                RBData.Add(dic_record_freq_rb_collector[key]);
            }
            foreach (double key in dic_freq_dBV_collector.Keys)
            {
                dBVData.Add(dic_freq_dBV_collector[key]);
            }

            if (Unit == "dBSPL")
            {
                AMPL = amplData.ToArray();
                //平均移动平滑
                MovingAverage(ref AMPL, 7, 1);
            }
            else if (Unit == "dBV")
            {
                AMPL = dBVData.ToArray();
                //平均移动平滑
                MovingAverage(ref AMPL, 7, 1);
            }
            else
            {
                errmsg = "unknown Unit input:"+Unit;
                return false;
            }
            THD = thdData.ToArray();
            RB = RBData.ToArray();

            return true;
        }


        private static List<double> RemoveHeaderTail(List<double> sourceValue, double gatePercent,double maxValue, int StartEndSignalLength,ref string errmsg)
        {
            List<double> retValue = new List<double>();
            double gate_value = gatePercent * maxValue;
            int header_pos = 0;
            int tail_pos = 0;

            for (int i = 0; i < sourceValue.Count; i++)
            {
                if (Math.Abs(sourceValue[i]) >= gate_value)
                {
                    header_pos = i + StartEndSignalLength;      //加上其实信号长度（一般为10ms长度，44.1KHz采样率的长度为441个点）

                    //左起找到满足门限值起点后，还需要向左寻找波形的起点，向 右 寻找到第一个跨0的点
                    for (int j = i; j < sourceValue.Count; j++)
                    {
                        if (sourceValue[j] * sourceValue[j + 1] < 0)
                        {
                            header_pos = j;
                            break;
                        }
                    }
                    break;
                }
            }

            for (int i = sourceValue.Count - 1; i >= 0; i--)
            {
                if (Math.Abs(sourceValue[i]) > gate_value)
                {
                    tail_pos = i - StartEndSignalLength;        //减去结尾信号长度（一般为10ms长度，44.1KHz采样率的长度为441个点）
                    //右起找到满足门限值终点后，还需要向右寻找波形的起点，向右寻找到第一个跨0的点
                    for (int j = i; j < sourceValue.Count - 1; j++)
                    {
                        if (sourceValue[j] * sourceValue[j + 1] < 0)
                        {
                            tail_pos = j + 1;
                            break;
                        }
                    }
                    break;
                }
            }

            if (header_pos < tail_pos)
            {
                retValue = sourceValue.GetRange(header_pos, tail_pos - header_pos);
            }
            else
            {
                errmsg = "error in cutting wav header >= tail";
            }

            return retValue;
        }

        private static void MovingAverage(ref double[] data, int MovingPoints, int CylceTime)
        {
            if (data.Length <= MovingPoints)
            {
                return;
            }

            if (MovingPoints < 2)
            {
                return;
            }

            if (MovingPoints % 2 == 0)
            {
                return;
            }

            if (CylceTime < 1)
            {
                return;
            }

            double[] data_moved = new double[data.Length];


            for (int t = 0; t < CylceTime; t++)
            {


                for (int i = 0; i < data.Length; i++)
                {
                    if (i < (MovingPoints - 1) / 2)
                    {
                        double sum = 0;
                        for (int j = 0; j <= i; j++)
                        {
                            sum += data[j];
                        }
                        data_moved[i] = sum / (i + 1);
                    }

                    else if (i > data.Length - (MovingPoints + 1) / 2)
                    {
                        double sum = 0;
                        for (int j = i; j < data.Length; j++)
                        {
                            sum += data[j];
                        }
                        data_moved[i] = sum / (data.Length - i);
                    }

                    else
                    {
                        double sum = 0;
                        for (int j = i - (MovingPoints - 1) / 2; j <= i + (MovingPoints - 1) / 2; j++)
                        {
                            sum += data[j];
                        }
                        data_moved[i] = sum / MovingPoints;
                    }
                }

                data = data_moved;
            }

            data_moved = new double[data.Length + MovingPoints - 1];


            //-----------------------------------头尾处理---------------------------------------
            //根据斜率对称增加头尾
            for (int i = 0; i < (MovingPoints - 1) / 2; i++)
            {
                data_moved[i] = 2 * data[i] - data[i + (MovingPoints - 1) / 2];
            }

            for (int i = (MovingPoints - 1) / 2; i < (MovingPoints - 1) / 2 + data.Length; i++)
            {
                data_moved[i] = data[i - (MovingPoints - 1) / 2];
            }

            for (int i = data_moved.Length - (MovingPoints - 1) / 2; i < data_moved.Length; i++)
            {
                data_moved[i] = 2 * data_moved[i - (MovingPoints - 1) / 2] - data_moved[i - (MovingPoints - 1)];
            }

            data = data_moved;
            data_moved = new double[data_moved.Length];

            for (int t = 0; t < CylceTime + 1; t++)
            {

                for (int i = 0; i < data.Length; i++)
                {
                    if (i < (MovingPoints - 1) / 2)
                    {
                        double sum = 0;
                        for (int j = 0; j <= i; j++)
                        {
                            sum += data[j];
                        }
                        data_moved[i] = sum / (i + 1);
                    }

                    else if (i > data.Length - (MovingPoints + 1) / 2)
                    {
                        double sum = 0;
                        for (int j = i; j < data.Length; j++)
                        {
                            sum += data[j];
                        }
                        data_moved[i] = sum / (data.Length - i);
                    }

                    else
                    {
                        double sum = 0;
                        for (int j = i - (MovingPoints - 1) / 2; j <= i + (MovingPoints - 1) / 2; j++)
                        {
                            sum += data[j];
                        }
                        data_moved[i] = sum / MovingPoints;
                    }
                }

                data = data_moved;
            }

            data = data.ToList().GetRange((MovingPoints - 1) / 2, data.Length - (MovingPoints - 1)).ToArray();

            //-----------------------------------头尾处理---------------------------------------

        }

        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        private static string _filePath;

        protected static bool WriteToFile(string section, string key, string val)
        {
            return WritePrivateProfileString(section, key, val, _filePath);
        }

        protected static string ReadFromFile(string section, string key, string def)
        {
            string res = "";
            Byte[] Buffer = new Byte[1024];
            int bufLen = GetPrivateProfileString(section, key, def, Buffer, 1024, _filePath);
            res = Encoding.GetEncoding(0).GetString(Buffer);
            res = res.Substring(0, bufLen).Trim();
            return res;
        }
    }
}
