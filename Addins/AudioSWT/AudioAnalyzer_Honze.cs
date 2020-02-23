using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace AudioSWT
{
    class AudioAnalyzer_Honze
    {
        ///// <summary>
        ///// 初始化，启动Audio-Analyzer.exe，加载项目
        ///// </summary>
        ///// <param name="proj">项目名</param>
        ///// <returns>错误代码>=1</returns>
        //[DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        //public static extern int Audio_GetDevice(StringBuilder proj);

        ///// <summary>
        ///// 初始化数据，检测加密狗
        ///// </summary>
        ///// <returns>错误代码>=1</returns>
        //[DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        //public static extern int Audio_InitData();

        ///// <summary>
        ///// 读取门限值到全局
        ///// </summary>
        ///// <param name="loop">测试通道例如“EarMic_SPK”</param>
        ///// <param name="sn">"SN号"</param>
        ///// <returns>频点数量</returns>        
        //[DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        //public static extern int Audio_SetSpec(StringBuilder loop, StringBuilder sn);

        ///// <summary>
        ///// 让声卡处于捕捉状态
        ///// </summary>
        ///// <returns>状态值=1</returns>        
        //[DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        //public static extern int Audio_SignalCapture();

        ///// <summary>
        ///// 声卡播放音源
        ///// </summary>
        ///// <returns>播放状态=0</returns>
        //[DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        //public static extern int Audio_SignalPlay();

        ///// <summary>
        ///// 检测声卡是否捕获信号
        ///// </summary>
        ///// <returns>状态=1</returns>
        //[DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        //public static extern int Audio_QueryCaptureStatus();

        ///// <summary>
        ///// 分析信号
        ///// </summary>
        ///// <param name="type">分析类型，扫频0</param>
        ///// <returns>频点数</returns>
        //[DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        //public static extern int Audio_AnalyzeSweepSignal(int type);

        ///// <summary>
        ///// 结果校验和展示
        ///// </summary>
        ///// <param name="meaturetime"></param>
        ///// <param name="FreqResult"></param>
        ///// <param name="FreqResultLen"></param>
        ///// <param name="AmplResult"></param>
        ///// <param name="AmplResultLen"></param>
        ///// <param name="AmplResultUpper"></param>
        ///// <param name="AmplResultUpperLen"></param>
        ///// <param name="AmplResultLower"></param>
        ///// <param name="AmplResultLowerLen"></param>
        ///// <param name="ThdResult"></param>
        ///// <param name="ThdResultLen"></param>
        ///// <param name="ThdResultUpper"></param>
        ///// <param name="ThdResultUpperLen"></param>
        ///// <param name="ThdResultLower"></param>
        ///// <param name="ThdResultLowerLen"></param>
        ///// <param name="SNRResult"></param>
        ///// <param name="SNRResultLen"></param>
        ///// <param name="ThdNResultUpper"></param>
        ///// <param name="ThdNResultUpperLen"></param>
        ///// <param name="ThdNResultLower"></param>
        ///// <param name="ThdNResultLowerLen"></param>
        ///// <param name="RubResult"></param>
        ///// <param name="RubResultLen"></param>
        ///// <param name="RubResultUpper"></param>
        ///// <param name="RubResultUpperLen"></param>
        ///// <param name="RubResultLower"></param>
        ///// <param name="RubResultLowerLen"></param>
        ///// <param name="freqcount"></param>
        ///// <returns></returns>
        //[DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.Cdecl,CharSet = CharSet.Ansi)]
        //public static extern int Audio_FetchVerifyResult_Example(int meaturetime, double[] FreqResult, ref int FreqResultLen, double[] AmplResult, ref int AmplResultLen, double[] AmplResultUpper, ref int AmplResultUpperLen, double[] AmplResultLower, ref int AmplResultLowerLen, double[] ThdResult, ref int ThdResultLen, double[] ThdResultUpper, ref int ThdResultUpperLen, double[] ThdResultLower, ref int ThdResultLowerLen, double[] SNRResult, ref int SNRResultLen, double[] ThdNResultUpper, ref int ThdNResultUpperLen, double[] ThdNResultLower, ref int ThdNResultLowerLen, double[] RubResult, ref int RubResultLen, double[] RubResultUpper, ref int RubResultUpperLen, double[] RubResultLower, ref int RubResultLowerLen, ref int freqcount);
        ////public static extern int Audio_FetchVerifyResult_Example(int meaturetime, double[] FreqResult,
        ////    ref int FreqResultLen,  double[] AmplResult,  int AmplResultLen,  double[] AmplResultUpper,
        ////    ref int AmplResultUpperLen,  double[] AmplResultLower, ref int AmplResultLowerLen,  double[] ThdResult,
        ////    ref int ThdResultLen,  double[] ThdResultUpper, ref int ThdResultUpperLen,  double[] ThdResultLower,
        ////    ref int ThdResultLowerLen,  double[] SNRResult, ref int SNRResultLen,  double[] ThdNResultUpper,
        ////    ref int ThdNResultUpperLen,  double[] ThdNResultLower,  int ThdNResultLowerLen,  double[] RubResult,
        ////    ref int RubResultLen,  double[] RubResultUpper,  int RubResultUpperLen,  double[] RubResultLower, 
        ////    ref int RubResultLowerLen, ref int freqcount);

        ///// <summary>
        ///// 测试结束
        ///// </summary>
        //[DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        //public static extern void Audio_TestEnd();

//-----------------------------------12-04 version-------------------------------------------------------------------------
        /*
        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Audio_GetDevice(StringBuilder proj, ref int len2);

        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Audio_InitData(ref int code);

        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Audio_SetSpec(StringBuilder loop, StringBuilder sn, ref int count);

        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Audio_SignalCapture(ref int count);

        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Audio_SignalPlay(ref int count);

        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Audio_QueryCaptureStatus(ref int count);

        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Audio_AnalyzeSweepSignal(int type, ref int count);

        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        // public static extern int Audio_FetchVerifyResult_Example(int meaturetime, double[] RubResult, double[] ThdNResultUpper, double[] ThdResultLower, double[] ThdResult, double[] AmplResultLower, double[] AmplResultUpper, double[] RubResultLower, double[] AmplResult, double[] RubResultUpper, double[] ThdNResultLower, double[] SNRResult, double[] ThdResultUpper, double[] FreqResult, Int32 len, Int32 len2, Int32 len3, Int32 len4, Int32 len5, Int32 len6, Int32 len7, Int32 len8, Int32 len9, Int32 len10, Int32 len11, Int32 len12, Int32 len13, ref int freqcount);
        public static extern int Audio_FetchVerifyResult_Example(int meaturetime, double[] FreqResult, ref int FreqResultLen, double[] AmplResult, ref int AmplResultLen, double[] AmplResultUpper, ref int AmplResultUpperLen, double[] AmplResultLower, ref int AmplResultLowerLen, double[] ThdResult, ref int ThdResultLen, double[] ThdResultUpper, ref int ThdResultUpperLen, double[] ThdResultLower, ref int ThdResultLowerLen, double[] SNRResult, ref int SNRResultLen, double[] ThdNResultUpper, ref int ThdNResultUpperLen, double[] ThdNResultLower, ref int ThdNResultLowerLen, double[] RubResult, ref int RubResultLen, double[] RubResultUpper, ref int RubResultUpperLen, double[] RubResultLower, ref int RubResultLowerLen, ref int freqcount);
        */
        /// <summary>
        /// 气密性分析
        /// </summary>
        /// <param name="Wav_Path">气密性录音文件本地路径</param>
        /// <param name="Leak_Result">接收结果</param>
        /// <param name="code">错误代码</param>
        /// <returns></returns>
        [DllImport("MicLeak.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Audio_AnalyzeMicLeak(StringBuilder Wav_Path, ref double Leak_Result, ref int code);

//-----------------------------------------------------------------------------12-04----------------------------------------------------------

        /// <summary>
        /// 扫频录音文件分析
        /// </summary>
        /// <param name="Wav_Path">录音文件路径</param>
        /// <param name="AnaChan">1</param>
        /// <param name="Type">0</param>
        /// <returns>错误代码</returns>
        [DllImport("MicLeak.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int AudioAnalyzeSweepWav(StringBuilder Wav_Path, int AnaChan, int Type);
//---------------------------------------------------------------------------12-27----------------------------------------------------------------

        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Audio_GetDevice(StringBuilder proj);

        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Audio_InitData();

        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Audio_SetSpec(StringBuilder loop, StringBuilder sn);

        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Audio_SignalCapture();

        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Audio_SignalPlay();

        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Audio_QueryCaptureStatus();

        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Audio_AnalyzeSweepSignal(int type);

        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Audio_FetchVerifyResult_Example(int MeasureTime, double[] RubResult, double[] ThdNResultUpper, double[] ThdResultLower,
    double[] ThdResult, double[] AmplResultLower, ref int Result, double[] AmplResultUpper, double[] RubResultLower, double[] AmplResult,
    double[] RubResultUpper, double[] ThdNResultLower, double[] SNRResult, double[] ThdResultUpper, double[] FreqResult, ref int RubResultLen, ref  int ThdNResultUpperLen,
     ref int ThdResultLowerLen, ref int ThdResultLen, ref int AmplResultLowerLen, ref int AmplResultUpperLen, ref int RubResultLowerLen, ref int AmplResultLen,
            ref int RubResultUpperLen, ref int ThdNResultLowerLen, ref  int SNRResultLen, ref  int ThdResultUpperLen,
     ref int FreqResultLen);

        //-------------------------------------------2018-06-13-----------------------
        /// <summary>
        /// 传录音文件，分析结果
        /// </summary>
        /// <param name="Wav_Path"></param>
        /// <param name="AnaChan"></param>
        /// <param name="Type"></param>
        /// <param name="code"></param>
        /// <param name="FR_Result"></param>
        /// <param name="THD_Result"></param>
        /// <param name="Rub_Result"></param>
        /// <param name="len"></param>
        /// <param name="len2"></param>
        /// <param name="len3"></param>
        [DllImport("AudioAnaLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void AudioAnalyzeSweepWav(StringBuilder Wav_Path, int AnaChan, int Type, ref int code, double[] FR_Result, double[] THD_Result,
    double[] Rub_Result, int len, int len2, int len3);
    }
}
