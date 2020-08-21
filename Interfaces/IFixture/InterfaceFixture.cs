using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cease.Addins.Log;

namespace Cease.Addins.Fixture
{
    /// <summary>
    /// 夹具接口集合
    /// </summary>
    public interface InterfaceFixture
    {
        /// <summary>
        /// 注册log接口
        /// </summary>
        /// <param name="_Ilog">Log接口</param>
        /// <returns></returns>
        void RegisterLogger(InterfaceLog _Ilog);

        /// <summary>
        /// 打开夹具端口
        /// </summary>
        /// <param name="_strPort">端口号</param>
        /// <param name="_subAddr">子载位号，从1开始索引</param>
        /// <param name="_strPortType">端口类型</param>
        /// <returns>返回函数执行结果</returns>
        bool OpenFixPort(string _strPort, int _subAddr, string _strPortType);

        /// <summary>
        /// 关闭端口
        /// </summary>
        /// <returns></returns>
        void CloseFixPort();

        /// <summary>
        /// 获取端口状态
        /// </summary>
        /// <returns>返回端口状态</returns>
        bool GetPortStat();

        /// <summary>
        /// 夹具操作
        /// </summary>
        /// <param name="_cmd">操作命令</param>
        /// <param name="_strStat">夹具状态</param>
        /// <returns>返回函数执行结果</returns>
        bool FixOperation(string _cmd, ref string _strStat);

        /// <summary>
        /// 闭合夹具某一路
        /// </summary>
        /// <param name="Index">夹具第n路</param>
        /// <param name="_strStat">夹具状态</param>
        /// <returns></returns>
        bool FixOn(int Index, ref string _strStat);


        /// <summary>
        /// 断开夹具某一路
        /// </summary>
        /// <param name="Index">夹具第n路</param>
        /// <param name="_strStat"></param>
        /// <returns></returns>
        bool FixOff(int Index, ref string _strStat);

        /// <summary>
        /// 检测夹具是否闭合
        /// </summary>
        /// <param name="_strStat">夹具状态</param>
        /// <returns>1表示闭合，0表示未闭合</returns>
        bool CheckFixClosed(ref string _strStat);


        /// <summary>
        /// 相机读取条码
        /// </summary>
        /// <param name="CamerAddr">相机地址</param>
        /// <param name="CodeValue">条码内容</param>
        /// <returns></returns>
        bool ReadCode(int CamerAddr, ref string CodeValue);

        /// <summary>
        /// 初始化相机
        /// </summary>
        /// <param name="CamerName"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="W"></param>
        /// <param name="H"></param>
        /// <param name="SingleReadTime"></param>
        /// <param name="MaxReadTime"></param>
        void InitCamera(string CamerName, int X, int Y, int W, int H, int SingleReadTime, int MaxReadTime);
       
    }
}
