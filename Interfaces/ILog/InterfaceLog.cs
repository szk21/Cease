using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cease.Interface.Log
{
    /// <summary>
    /// log类型
    /// </summary>
    public enum LOG_TYPE    //AT
    {
        RPT,
        GPIB,
        DUT,
        FIX,
        DBG,
        WARN,
        ERR,
        SOC,
        XMI
    }

    /// <summary>
    /// log事件
    /// </summary>
    public class LoggedEventArgs : EventArgs
    {
        // class members  
        public int m_iDut;
        public string m_msg;
        public LOG_TYPE m_type;

        /// <summary>
        /// LoggedEventArgs构造函数
        /// </summary>
        public LoggedEventArgs(int _iDut, LOG_TYPE _type, string _msg)
        {
            this.m_iDut = _iDut;
            this.m_msg = _msg;
            this.m_type = _type;
        }
    }

    /// <summary>
    /// log插件的接口集合
    /// </summary>
    public interface InterfaceLog
    {
        /// <summary>
        /// log记录事件
        /// </summary>
        event EventHandler Logged;

        /// <summary>
        /// 初始化log
        /// </summary>
        /// <param name="_filePath">log路径</param>
        /// <returns></returns>
        void InitialLog(string _filePath);

        /// <summary>
        /// 设置Log的DutNum
        /// </summary>
        /// <param name="_dut">DutNum</param>
        void SetDut(int _dut);

        /// <summary>
        /// 记录debug日志
        /// </summary>
        /// <param name="strMessage">日志信息</param>
        /// <returns></returns>
        void msg(string strMessage);

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="strMessage">警告信息</param>
        /// <returns></returns>
        void warn(string strMessage);

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="strMessage">错误信息</param>
        /// <returns></returns>
        void err(string strMessage);

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="strMessage">错误信息</param>
        /// <param name="exp">异常信息</param>
        /// <returns></returns>
        void err(string strMessage, Exception exp);

        /// <summary>
        /// 获取最后一条错误信息
        /// </summary>
        /// <returns>最后一条错误信息</returns>
        string GetLastErrMsg();

        /// <summary>
        /// 写日志数据到本地文件
        /// </summary>
        /// <param name="DutNum">Dut序号</param>
        /// <param name="_model">产品型号</param>
        /// <param name="_station">工位</param>
        /// <param name="_sn">条码</param>
        /// <param name="_path">log路径</param>
        /// <param name="_bRes">测试结果</param>
        /// <returns></returns>
        void WirteLogToFile(int DutNum, string _station, string _path, bool _bRes, string ShortDate, string LocalTime);

    }
}
