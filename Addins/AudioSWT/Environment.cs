using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel.Composition;
using Cease.Addins.AudioSWT;
using Cease.Addins.Log;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AudioSWT
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


        public static ManualResetEvent allDone = new ManualResetEvent(false);

        protected bool StartAudioAnalyer(string _sProjName,bool bVisble,ref int _iErrorCode)
        {
            StringBuilder proj = new StringBuilder(_sProjName, 128);
            Int32 code = -999;
            code = AudioAnalyzer_Honze.Audio_GetDevice(proj);
            WinExec("Audio-Analyser.exe", 0);
            IntPtr hand = FindWindow(null, "Audio-Analyser");//Calcutator
            if (!bVisble)
            {
                ShowWindow(hand, 2);
            }
            if (code >= 1)
            {
                _iErrorCode = code;
                return true;
            }
            else
            {
                _iErrorCode = code;
                return false;
            }
        }


        [DllImport("kernel32.dll", EntryPoint = "WinExec")]
        public static extern int WinExec(string lpCmdLine, int nCmdShow);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

    }

    [Export(typeof(InterfaceAudioSWT))]
    [ExportMetadata("AddinName", "AudioSWT")]
    public class AudioSWT : Environment, InterfaceAudioSWT
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

        public void CWAVFileRead(string wavFile)
        {
            CWav_file wav = new CWav_file(wavFile);
        }

        public void DivideWAV2OneCHN(string wavFile)
        {
            string wavName=wavFile.Substring(wavFile.LastIndexOf("\\")+1).Replace(".wav","");
            string dir = wavFile.Substring(0, wavFile.LastIndexOf("\\") );
            CWav_file wav = new CWav_file(wavFile);
            if (wav.wav_header.channel == 6)
            {
                for (int i = 0; i < 6; i++)
                {
                    CWav_file.Save2WAV(wav.wav_file.data_all_chn[i], dir+"\\"+wavName + "_" + i.ToString() + ".wav");
                }
            }
            if (wav.wav_header.channel == 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    CWav_file.Save2WAV(wav.wav_file.data_all_chn[i], dir + "\\" + wavName + "_" + i.ToString() + ".wav");
                }
            }
            else if(wav.wav_header.channel==2)
            {
                CWav_file.Save2WAV(wav.wav_file.L_data, dir + "\\" + wavName + "_L.wav");
                CWav_file.Save2WAV(wav.wav_file.R_data, dir + "\\" + wavName + "_R.wav");
            }
        }

        public void CutWAVFile(string wavFile, double Percentage)
        {
            CWav_file wav = new CWav_file(wavFile);
            wav.CutWaveFile(Percentage);
        }

        //Honze function


        #region//------------------------1204------------------------------------------
        /*
        public bool StartAudioAnalyzer(string _sSupplierName, string sProjName, ref int iErrorCode)
        {
            return StartAudioAnalyer(sProjName, ref iErrorCode);
        }

        public bool _Audio_InitData(ref int iErrCode)
        {
            try
            {
                iErrCode = AudioAnalyzer_Honze.Audio_InitData(ref iErrCode);
                return true;
            }
            catch
            {
                iErrCode = -999;
                return false;
            }
        }

        public bool _Audio_SetSpec(string sLoopName, string sSN, ref int iFreqCount)
        {
            StringBuilder sbProj = new StringBuilder(sLoopName, 128);
            StringBuilder sbSN = new StringBuilder(sSN, 128);
            try
            {
                iFreqCount = AudioAnalyzer_Honze.Audio_SetSpec(sbProj, sbSN, ref iFreqCount);
                return true;
            }
            catch
            {
                iFreqCount = -999;
                return false;
            }
        }

        public bool _Audio_SignalCapture(ref int iErrCode)
        {
            try
            {
                iErrCode = AudioAnalyzer_Honze.Audio_SignalCapture(ref iErrCode);
                return true;
            }
            catch
            {
                iErrCode = -999;
                return false;
            }
        }

        public bool _Audio_SignalPlay(ref int iErrCode)
        {
            try
            {
                iErrCode = AudioAnalyzer_Honze.Audio_SignalPlay(ref iErrCode);
                return true;
            }
            catch
            {
                iErrCode = -999;
                return false;
            }
        }

        public bool _Audio_QueryCaptureStatus(ref int iErrCode)
        {
            try
            {
                iErrCode = AudioAnalyzer_Honze.Audio_QueryCaptureStatus(ref iErrCode);
                return true;
            }
            catch
            {
                iErrCode = -999;
                return false;
            }
        }

        public bool _Audio_AnalyzeSweepSignal(ref int iErrCode)
        {
            try
            {
                iErrCode = AudioAnalyzer_Honze.Audio_AnalyzeSweepSignal(1,ref iErrCode);
                return true;
            }
            catch
            {
                iErrCode = -999;
                return false;
            }
        }

        public bool CalculateMicLeak(string sLocalRecordFileFullPath, ref double dValue, ref string sErrorCode)
        {
            StringBuilder path = new StringBuilder(sLocalRecordFileFullPath, 128);
            double value = -999;
            int code = -999;
            try
            {
                AudioAnalyzer_Honze.Audio_AnalyzeMicLeak(path, ref value, ref code);
                dValue = Math.Round(value, 2);
                sErrorCode = code.ToString();
            }
            catch (Exception exp)
            {
                sErrorCode = "Analyse mic leak exception, error is " + exp.ToString();
                return false;

            }

            return true;

        }

        public bool AudioAnalyzeSweepWav(string sWavPath,ref int iErrCode)
        {
            StringBuilder sbPath = new StringBuilder(sWavPath, 128);
            try
            {
                iErrCode = AudioAnalyzer_Honze.AudioAnalyzeSweepWav(sbPath,1,0);
                return true;
            }
            catch
            {
                iErrCode = -999;
                return false;
            }
        }

        public bool _Audio_FetchVerifyResult_Example(ref int iErrCode)
        {
            double[] FreqResult = new double[1024];
            double[] FreqResultUpper = new double[1024];
            double[] FreqResultLower = new double[1024];

            double[] AmplResult = new double[1024];
            double[] AmplResultUpper = new double[1024];
            double[] AmplResultLower = new double[1024];

            double[] ThdResult = new double[1024];
            double[] ThdResultUpper = new double[1024];
            double[] ThdResultLower = new double[1024];

            double[] RubResult = new double[1024];
            double[] RubResultUpper = new double[1024];
            double[] RubResultLower = new double[1024];

            double[] ThdNResult = new double[1024];
            double[] ThdNResultUpper = new double[1024];
            double[] ThdNResultLower = new double[1024];
            double[] SNRResult = new double[1024];
            Int32 ResultCount = 1024;
            Int32 FreqResultLen = 1024;
            Int32 AmplResultLen = 1024;
            Int32 AmplResultUpperLen = 1024;
            Int32 AmplResultLowerLen = 1024;
            Int32 ThdResultLen = 1024;
            Int32 ThdResultUpperLen = 1024;
            Int32 ThdResultLowerLen = 1024;
            Int32 SNRResultLen = 1024;
            Int32 ThdNResultUpperLen = 1024;
            Int32 ThdNResultLowerLen = 1024;
            Int32 RubResultLen = 1024;
            Int32 RubResultUpperLen = 1024;
            Int32 RubResultLowerLen = 1024;
            Int32 FreqCount = 1024;


                AudioAnalyzer_Honze.Audio_FetchVerifyResult_Example(1, FreqResult, ref FreqResultLen, AmplResult,
                ref AmplResultLen, AmplResultUpper, ref AmplResultUpperLen, AmplResultLower,
                ref AmplResultLowerLen, ThdResult, ref ThdResultLen, ThdResultUpper,
                ref ThdResultUpperLen, ThdResultLower, ref ThdResultLowerLen, SNRResult,
                ref SNRResultLen, ThdNResultUpper, ref ThdNResultUpperLen, ThdNResultLower,
                ref ThdNResultLowerLen, RubResult, ref RubResultLen, RubResultUpper,
                ref RubResultUpperLen, RubResultLower, ref RubResultLowerLen, ref iErrCode);

                //iErrorCode = AudioAnalyzer_Honze.Audio_FetchVerifyResult_Example(1,  tmpResult.FreqResult, ref tmpResult.FreqResultlen,
                //     tmpResult.AmplResult,  tmpResult.AmplResultlen,  tmpResult.AmplResultUpper, ref tmpResult.AmplResultUpperlen,
                //     tmpResult.AmplResultLower, ref tmpResult.AmplResultLowerlen,  tmpResult.ThdResult, ref tmpResult.ThdResultlen,
                //     tmpResult.ThdResultUpper, ref tmpResult.ThdResultUpperlen,  tmpResult.ThdResultLower, ref tmpResult.ThdResultLowerlen,
                //     tmpResult.SNRResult, ref tmpResult.SNRResultlen,  tmpResult.ThdNResultUpper, ref tmpResult.ThdNResultUpperlen,
                //     tmpResult.ThdNResultLower,  tmpResult.ThdNResultLowerlen,  tmpResult.RubResult, ref tmpResult.RubResultlen,
                //     tmpResult.RubResultUpper,  tmpResult.RubResultUpperlen,  tmpResult.RubResultLower, ref tmpResult.RubResultLowerlen,
                //    ref tmpResult.resultcount);
                return true;
        }

        public bool _Audio_FetchVerifyResult_Example(ref AT.SweepTestResult struAT)
        {
                AT.SweepTestResult tmpResult = struAT;
                AudioAnalyzer_Honze.Audio_FetchVerifyResult_Example(1, tmpResult.FreqResult, ref tmpResult.FreqResultLen,
                 tmpResult.AmplResult, ref tmpResult.AmplResultLen, tmpResult.AmplResultUpper, ref tmpResult.AmplResultUpperLen,
                 tmpResult.AmplResultLower, ref tmpResult.AmplResultLowerLen, tmpResult.ThdResult, ref tmpResult.ThdResultLen,
                 tmpResult.ThdResultUpper, ref tmpResult.ThdResultUpperLen, tmpResult.ThdResultLower, ref tmpResult.ThdResultLowerLen,
                 tmpResult.SNRResult, ref tmpResult.SNRResultLen, tmpResult.ThdNResultUpper, ref tmpResult.ThdNResultUpperLen,
                 tmpResult.ThdNResultLower, ref tmpResult.ThdNResultLowerLen, tmpResult.RubResult, ref tmpResult.RubResultLen,
                 tmpResult.RubResultUpper, ref tmpResult.RubResultUpperLen, tmpResult.RubResultLower, ref tmpResult.RubResultLowerLen,
                ref tmpResult.ResultCount);
                struAT = tmpResult;
            return true;
        }
        */
        #endregion//------------------------1204------------------------------------------


        //------------------------1227------------------------------------------

        public bool StartAudioAnalyzer(string _sSupplierName, string sProjName, bool bVisble, ref int iErrorCode)
        {
            return StartAudioAnalyer(sProjName,bVisble, ref iErrorCode);
        }

        public bool _Audio_InitData(ref int iErrCode)
        {
            try
            {
                iErrCode = AudioAnalyzer_Honze.Audio_InitData();
                return true;
            }
            catch
            {
                iErrCode = -999;
                return false;
            }
        }

        public bool _Audio_SetSpec(string sLoopName, string sSN, ref int iFreqCount)
        {
            StringBuilder sbProj = new StringBuilder(sLoopName, 128);
            StringBuilder sbSN = new StringBuilder(sSN, 128);
            try
            {
                iFreqCount = AudioAnalyzer_Honze.Audio_SetSpec(sbProj, sbSN);
                return true;
            }
            catch
            {
                iFreqCount = -999;
                return false;
            }
        }

        public bool _Audio_SignalCapture(ref int iErrCode)
        {
            try
            {
                iErrCode = AudioAnalyzer_Honze.Audio_SignalCapture();
                return true;
            }
            catch
            {
                iErrCode = -999;
                return false;
            }
        }

        public bool _Audio_SignalPlay(ref int iErrCode)
        {
            try
            {
                iErrCode = AudioAnalyzer_Honze.Audio_SignalPlay();
                return true;
            }
            catch
            {
                iErrCode = -999;
                return false;
            }
        }

        public bool _Audio_QueryCaptureStatus(ref int iErrCode)
        {
            try
            {
                iErrCode = AudioAnalyzer_Honze.Audio_QueryCaptureStatus();
                return true;
            }
            catch
            {
                iErrCode = -999;
                return false;
            }
        }

        public bool _Audio_AnalyzeSweepSignal(ref int iErrCode)
        {
            try
            {
                iErrCode = AudioAnalyzer_Honze.Audio_AnalyzeSweepSignal(1);
                return true;
            }
            catch
            {
                iErrCode = -999;
                return false;
            }
        }

        public bool CalculateMicLeak(string sLocalRecordFileFullPath, ref double dValue, ref string sErrorCode)
        {
            StringBuilder path = new StringBuilder(sLocalRecordFileFullPath, 128);
            double value = -999;
            int code = -999;
            try
            {
                AudioAnalyzer_Honze.Audio_AnalyzeMicLeak(path, ref value, ref code);
                dValue = Math.Round(value, 2);
                sErrorCode = code.ToString();
            }
            catch (Exception exp)
            {
                sErrorCode = "Analyse mic leak exception, error is " + exp.ToString();
                return false;

            }

            return true;

        }

        public bool AudioAnalyzeSweepWav(string sWavPath, ref int iErrCode)
        {
            StringBuilder sbPath = new StringBuilder(sWavPath, 128);
            try
            {
                iErrCode = AudioAnalyzer_Honze.AudioAnalyzeSweepWav(sbPath, 1, 0);
                return true;
            }
            catch
            {
                iErrCode = -999;
                return false;
            }
        }

        public bool _Audio_FetchVerifyResult_Example(ref int iErrCode)
        {
            double[] FreqResult = new double[1024];
            double[] FreqResultUpper = new double[1024];
            double[] FreqResultLower = new double[1024];

            double[] AmplResult = new double[1024];
            double[] AmplResultUpper = new double[1024];
            double[] AmplResultLower = new double[1024];

            double[] ThdResult = new double[1024];
            double[] ThdResultUpper = new double[1024];
            double[] ThdResultLower = new double[1024];

            double[] RubResult = new double[1024];
            double[] RubResultUpper = new double[1024];
            double[] RubResultLower = new double[1024];

            double[] ThdNResult = new double[1024];
            double[] ThdNResultUpper = new double[1024];
            double[] ThdNResultLower = new double[1024];
            double[] SNRResult = new double[1024];
            Int32 ResultCount = 1024;
            Int32 FreqResultLen = 1024;
            Int32 AmplResultLen = 1024;
            Int32 AmplResultUpperLen = 1024;
            Int32 AmplResultLowerLen = 1024;
            Int32 ThdResultLen = 1024;
            Int32 ThdResultUpperLen = 1024;
            Int32 ThdResultLowerLen = 1024;
            Int32 SNRResultLen = 1024;
            Int32 ThdNResultUpperLen = 1024;
            Int32 ThdNResultLowerLen = 1024;
            Int32 RubResultLen = 1024;
            Int32 RubResultUpperLen = 1024;
            Int32 RubResultLowerLen = 1024;
            Int32 FreqCount = 1024;


            AudioAnalyzer_Honze.Audio_FetchVerifyResult_Example(1, RubResult, ThdNResultUpper, ThdResultLower,
    ThdResult, AmplResultLower, ref iErrCode, AmplResultUpper, RubResultLower, AmplResult,
     RubResultUpper, ThdNResultLower, SNRResult, ThdResultUpper, FreqResult, ref RubResultLen, ref  ThdNResultUpperLen,
     ref ThdResultLowerLen, ref ThdResultLen, ref AmplResultLowerLen, ref AmplResultUpperLen,
     ref RubResultLowerLen, ref AmplResultLen,ref RubResultUpperLen, ref ThdNResultLowerLen, ref  SNRResultLen, ref  ThdResultUpperLen,
     ref FreqResultLen);

            //iErrorCode = AudioAnalyzer_Honze.Audio_FetchVerifyResult_Example(1,  tmpResult.FreqResult, ref tmpResult.FreqResultlen,
            //     tmpResult.AmplResult,  tmpResult.AmplResultlen,  tmpResult.AmplResultUpper, ref tmpResult.AmplResultUpperlen,
            //     tmpResult.AmplResultLower, ref tmpResult.AmplResultLowerlen,  tmpResult.ThdResult, ref tmpResult.ThdResultlen,
            //     tmpResult.ThdResultUpper, ref tmpResult.ThdResultUpperlen,  tmpResult.ThdResultLower, ref tmpResult.ThdResultLowerlen,
            //     tmpResult.SNRResult, ref tmpResult.SNRResultlen,  tmpResult.ThdNResultUpper, ref tmpResult.ThdNResultUpperlen,
            //     tmpResult.ThdNResultLower,  tmpResult.ThdNResultLowerlen,  tmpResult.RubResult, ref tmpResult.RubResultlen,
            //     tmpResult.RubResultUpper,  tmpResult.RubResultUpperlen,  tmpResult.RubResultLower, ref tmpResult.RubResultLowerlen,
            //    ref tmpResult.resultcount);
            return true;
        }

        public bool _Audio_FetchVerifyResult_Example(ref AT.SweepTestResult struAT,ref int iErrCode)
        {
            AT.SweepTestResult tmpResult = struAT;
            double[] limits = new double[tmpResult.AmplResult.Length];
    //        AudioAnalyzer_Honze.Audio_FetchVerifyResult_Example(1, tmpResult.RubResult, tmpResult.ThdNResultUpper, tmpResult.ThdResultLower,
    //tmpResult.ThdResult, tmpResult.AmplResultLower, ref iErrCode, tmpResult.AmplResultUpper, tmpResult.RubResultLower, tmpResult.AmplResult,
    // tmpResult.RubResultUpper, tmpResult.ThdNResultLower, tmpResult.SNRResult, tmpResult.ThdResultUpper, tmpResult.FreqResult, ref tmpResult.RubResultLen, ref  tmpResult.ThdNResultUpperLen,
    // ref tmpResult.ThdResultLowerLen, ref tmpResult.ThdResultLen, ref tmpResult.AmplResultLowerLen, ref tmpResult.AmplResultUpperLen,
    // ref tmpResult.RubResultLowerLen, ref tmpResult.AmplResultLen,
    //        ref tmpResult.RubResultUpperLen, ref tmpResult.ThdNResultLowerLen, ref  tmpResult.SNRResultLen, ref  tmpResult.ThdResultUpperLen,
    // ref tmpResult.FreqResultLen);
            AudioAnalyzer_Honze.Audio_FetchVerifyResult_Example(1, tmpResult.RubResult, limits, limits,
tmpResult.ThdResult, limits, ref iErrCode, limits, limits, tmpResult.AmplResult,
limits, limits, tmpResult.SNRResult, limits, tmpResult.FreqResult, ref tmpResult.RubResultLen, ref  tmpResult.ThdNResultUpperLen,
ref tmpResult.ThdResultLowerLen, ref tmpResult.ThdResultLen, ref tmpResult.AmplResultLowerLen, ref tmpResult.AmplResultUpperLen,
ref tmpResult.RubResultLowerLen, ref tmpResult.AmplResultLen,
ref tmpResult.RubResultUpperLen, ref tmpResult.ThdNResultLowerLen, ref  tmpResult.SNRResultLen, ref  tmpResult.ThdResultUpperLen,
ref tmpResult.FreqResultLen);
            struAT = tmpResult;
            return true;
        }

        //------------------------1227------------------------------------------

        //-----------------------------2018-06-12----------------------------
        public bool _AudioAnalyzeSweepWav(string WavPath,ref AT.SweepTestResult struAT, ref int iErrCode)
        {
            AT.SweepTestResult tmpResult = struAT;
            StringBuilder path=new StringBuilder(WavPath,128);
            AudioAnalyzer_Honze.AudioAnalyzeSweepWav(path, 0, 0, ref iErrCode, struAT.AmplResult, struAT.ThdResult, struAT.RubResult, struAT.AmplResultLen, struAT.ThdResultLen, struAT.RubResultLen);
            return true;
        }

        //-----------------------audioProcess blackshark-----------
        public bool _AudioAnalyzeSweepBlackShark(string sourceWAVFile, string sourceWAVFileConfig, string recordWAVFile, int Channel,string Unit, bool removeHeaderTail, out double[] AMPL, out double[] THD, out double[] RB, out string errmsg)
        {
            return AudioAnalyzer_BlackShark.WavFileProcess(sourceWAVFile, sourceWAVFileConfig, recordWAVFile, Channel, Unit,removeHeaderTail, out AMPL, out THD, out RB, out errmsg);
        }
    }
}
