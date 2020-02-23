using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using Cease.Addins.Log;

namespace Cease.Addins.UI
{
    /// <summary>
    /// UI控制接口集合
    /// </summary>
    public interface InterfaceUI
    {
        /// <summary>
        /// 注册log接口
        /// </summary>
        /// <param name="_Ilog">Log接口</param>
        /// <returns></returns>
        void RegisterLogger(InterfaceLog _Ilog);

        /// <summary>
        /// 生成Form对象
        /// </summary>
        /// <returns>返回生成的Form对象</returns>
        Form Instance { get; }

        /// <summary>
        /// 设置系统状态
        /// </summary>
        /// <param name="_iDut">Dut编号</param>
        /// <returns>返回扫描的条码</returns>
        string GetScanBarcode(int _iDut);


        /// <summary>
        /// 设置显示Mes状态
        /// </summary>
        /// <param name="MES">Mes类型</param>
        void SetMESStatus(string MES);

        /// <summary>
        /// 设置系统状态
        /// </summary>
        /// <param name="_iDut">Dut编号</param>
        /// <param name="_stat">状态</param>
        /// <returns>返回当前系统状态</returns>
        void SetDutStatus(int _iDut, string _stat);

        /// <summary>
        /// 获取系统状态
        /// </summary>
        /// <param name="_iDut">Dut编号</param>
        /// <returns>返回当前系统状态</returns>
        string GetDutStatus(int _iDut);

        /// <summary>
        /// 界面初始化
        /// </summary>
        /// <param name="_dicPara">初始化参数字典</param>
        /// <returns>返回函数执行结果</returns>
        bool Initialize(Dictionary<string, string> _dicPara);

        /// <summary>
        /// 更新Dut测试状态
        /// </summary>
        /// <param name="_status">测试结果</param>
        /// <returns>返回函数执行结果</returns>
        void UpdateDutStatus(int _iDut, string _stat, bool bRes);

        /// <summary>
        /// 显示手机SN号
        /// </summary>
        /// <param name="iDut"></param>
        /// <param name="SN"></param>
        void DisplaySN(int iDut,string SN);

        /// <summary>
        /// 显示测试结果
        /// </summary>
        /// <param name="itemIdx">测试项序号</param>
        /// <param name="testvalue">测试值</param>
        /// <param name="testresult">测试结果</param>
        /// <returns>返回函数执行结果</returns>
        void UpdateTestResult(int _iDut, string itemIdx, string testvalue, string testresult);

        /// <summary>
        /// 显示测试结果
        /// </summary>
        /// <param name="itemIdx">测试项序号</param>
        /// <param name="testResult">测试结果</param>
        /// <returns>返回函数执行结果</returns>
        void UpdateTestResult(int _iDut, string itemIdx, string testResult);

        /// <summary>
        /// 更新测试信息:SN、socket服务和客户端状态       //AT
        /// </summary>
        /// <param name="_status">测试结果</param>
        /// <returns>返回函数执行结果</returns>
        void UpdateTestInforDisp_AT(Dictionary<string, string> _dic);

        /// <summary>
        /// 点击开始，用于音频设备抽屉进入开始测试         //AT
        /// </summary>
        /// <param name="dut"></param>
        void StartClickAction(int dut);

        /// <summary>
        /// 初始化完成后，开始测试之前的清空界面          //AT
        /// </summary>
        /// <param name="dut"></param>
        void ClearUI(int dut);


        /// <summary>
        /// 更新显示各个dut的测试历史记录
        /// </summary>
        /// <param name="dut"></param>
        /// <param name="dicHis"></param>
        void UpdateHistory(int dut, Dictionary<int, bool> dicHis);

        /// <summary>
        /// 更新显示各个dut的测试历史记录
        /// </summary>
        /// <param name="dut"></param>
        /// <param name="dicHis"></param>
        void UpdateHistory(int dut, Dictionary<int, Dictionary<string,string>> dicHisInfo);

        void UpdateReportViewr(string fileName);

        //AT
        /// <summary>
        /// 显示曲线图
        /// </summary>
        void ShowCurveForm();

        //AT
        void ShowCurvePage(string pageName, string SN, double[] freq, double[] AMPL, double[] AMPL_USL, double[] AMPL_LSL,
            double[] THD, double[] THD_USL, double[] THD_LSL,
            double[] RB, double[] RB_USL, double[] RB_LSL, double[] AMPL_REF, Dictionary<string, bool> dicLoopResult, Dictionary<string, bool> dicItemResult, double[] AMPL2, double[] THD2, double[] RB2);

        //AT
        /// <summary>
        /// 隐藏曲线图
        /// </summary>
        void HideAllPages();

        //AT
        /// <summary>
        /// 保存曲线图
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="sn"></param>
        void SaveCurvesImage(string dir,string sn);

        //AT
        /// <summary>
        /// 截图CeaseForm
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="SN"></param>
        /// <param name="testResult"></param>
        void SaveCeaseMainFormImage(string dir, string SN, string testResult);

        //AT
        /// <summary>
        /// 显示Info
        /// </summary>
        void ShowTestInfoPage();
    }
}
