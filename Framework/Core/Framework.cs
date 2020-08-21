using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.IO;

using System.Windows.Forms;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

using System.Net;
using System.Net.Sockets;

using Cease.Core;
using Cease.Test;
using Cease.Addin;
using Cease.Interface.Log;

using log4net;
using System.Threading;

namespace Cease.Core
{
    /// <summary>
    /// 测试驱动层
    /// </summary>
    public class Framework : IFramework
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected ILog log;

        /// <summary>
        /// 测试控制单元列表
        /// </summary>
        public ITestGlobalUnit m_ITestGlobalUnit;

        /// <summary>
        /// CeaseFramework启动路径
        /// </summary>
        public string Path { get; private set; }

        #region Framework Method
        /// <summary>
        /// 内核构造
        /// </summary>
        public Framework(ILog _log)
        {
            this.log = _log;
            Path = Application.StartupPath;

            m_ITestGlobalUnit = new TestGlobalUnit(1, log);
        }

        protected void ActionStart(object _stationName)
        {
            if (!m_ITestGlobalUnit.CreateAllTestCases((string)_stationName))
            {
                throw new Exception("CreateAllTestCases Fail!");
            }

            bool bTestRes = m_ITestGlobalUnit.Run();
        }

        /// <summary>
        /// Check插件库
        /// </summary>
        /// <returns>返回执行结果</returns>
        protected bool CheckAddins()
        {
            log.Info("Check Addins Begin:");

            XmlAddinsSetting AddinsXml = new XmlAddinsSetting(Path + "\\config.xml");
            foreach (string k in AddinsXml.dic.Keys)
            {
                //check exist
                string strAddinsPath = Path + "\\Addins\\" + k;
                if (!System.IO.File.Exists(strAddinsPath + "\\" + AddinsXml.DllName(k)))
                {
                    log.Error($"Can not find {strAddinsPath}\\{AddinsXml.DllName(k)}");
                    return false;
                }

                //check version
                FileVersionInfo AddinsVersionInfo = FileVersionInfo.GetVersionInfo(strAddinsPath + "\\" + AddinsXml.DllName(k));
                string DllVersionInXml = AddinsXml.dic[k]["Ver"];
                if (AddinsVersionInfo.FileVersion != DllVersionInXml)
                {
                    log.Error($"{AddinsXml.DllName(k)} Version is {AddinsVersionInfo.FileVersion}, not {DllVersionInXml} in XML!");
                }
            }

            return true;
        }

        /// <summary>
        /// Check测试项库
        /// </summary>
        /// <returns>返回执行结果</returns>
        protected bool CheckTestStore()
        {
            log.Info("Check TestUnit Begin");

            if (!System.IO.Directory.Exists(Path + "\\TestUnits"))
            {
                log.Error("Can not find " + Path + "\\TestUnits");
                return false;
            }

            string strTestStorePath = Path + "\\TestUnits";
            if (!System.IO.File.Exists(strTestStorePath + "\\" + "Cease.TestStore" + ".dll"))
            {
                log.Error("Can not find " + strTestStorePath + "\\" + "Cease.TestStore" + ".dll");
                return false;
            }

            return true;
        }
        #endregion

        #region Framework_Interface

        /// <summary>
        /// 构造Framework测试引擎
        /// </summary>
        /// <param name="_strStation">测试工位</param>
        /// <param name="_strProductName">测试项目名称</param>
        /// <returns>返回执行结果</returns>
        public bool CreateTestEngine()
        {
            try
            {
                if (!CheckAddins())
                {
                    throw new Exception("Check Addins Fail!");
                }

                if (!CheckTestStore())
                {
                    throw new Exception("Check TestStore Fail!");
                }

                if (!m_ITestGlobalUnit.Initial())
                {
                    throw new Exception(string.Format("ITestGlobalUnit Initial Fail!"));
                }

                return true;
            }
            catch (System.Exception ex)
            {
                log.Error(string.Format("CreateTestEngine Fail!"), ex);
                MessageBox.Show(ex.Message, "Error");

                return false;
            }
        }

        /// <summary>
        /// Framework启动入口
        /// </summary>
        /// <param name="iDut">测试Dut编码</param>
        /// <returns></returns>
        public void StartEngine(string stationName)
        {
            ThreadPool.QueueUserWorkItem(ActionStart, stationName);
        }

        public void UiUpdateLogRegister(EventHandler MakeLog)
        {
            m_ITestGlobalUnit.UiUpdateLogRegister(MakeLog);
        }

        #endregion
    }
}
