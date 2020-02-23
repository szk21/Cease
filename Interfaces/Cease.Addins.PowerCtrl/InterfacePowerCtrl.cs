using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cease.Addins.Log;

namespace Cease.Addins.PowerCtrl
{

    /// <summary>
    /// 电源控制接口集合
    /// </summary>
    public interface InterfacePowerCtrl
    {

        double _dLeakCur { get; set; }
        /// <summary>
        /// 注册log接口
        /// </summary>
        /// <param name="_Ilog">Log接口</param>
        /// <returns></returns>
        void RegisterLogger(InterfaceLog _Ilog);

        /// <summary>
        /// 初始化电源
        /// </summary>
        /// <param name="strResourceName">地址</param>
        /// <param name="Timeout">超时门限</param>
        /// <param name="DoReset">仪器复位标志位</param>
        /// <returns>返回函数执行结果</returns>
        bool InitialInstr(string strResourceName, int Timeout, bool DoReset);

        /// <summary>
        /// 释放电源
        /// </summary>
        /// <returns>返回函数执行结果</returns>
        bool ReleaseInstr();

        /// <summary>
        /// 阶梯上电
        /// </summary>
        /// <returns></returns>
        void PowerStepOn(string strVolt,string strVPTVolt,int InternalTime);

        void PowerSetVolt(string strVolt, string VPTVolt, int InternalTime);
        /// <summary>
        /// 关闭电源
        /// </summary>
        /// <returns></returns>
        void PowerOff();

        /// <summary>
        /// 获取测量电流值，单位mA
        /// </summary>
        /// <param name="_dfCur">测量电流</param>
        /// <returns>返回函数执行结果</returns>
        bool MeasureCur(ref double _dfCur);

        /// <summary>
        /// 设置电源电压
        /// </summary>
        /// <param name="_volt">设置的电压值（mV）</param>
        /// <returns></returns>
        void SetVoltmv(double _volt);
    }
}
