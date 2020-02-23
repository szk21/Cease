using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using CEASE.Test.Root;
using System.ComponentModel.Composition;
using System.Xml;
using System.Xml.Linq;

using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Net.NetworkInformation;
using CEASE.Core;
using System.Xml.XPath;
using System.Net;
using System.Net.Sockets;
using System.Data.OleDb;
using System.Data;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
namespace Cease.TestStore
{
    #region Example
    /// <summary>
    /// TestAdd
    /// </summary>
    [Export(typeof(TestUnit))]
    [ExportMetadata("AddinName", "TestAdd")]
    public class TestAdd : TestUnit
    {
        /// <summary>
        /// 测试项构造函数
        /// </summary>
        [ImportingConstructor]
        public TestAdd() : base("1000001", "TestAdd", 0) { }

        /// <summary>
        /// 测试项实际执行处理函数
        /// </summary>
        /// <returns>测试结果</returns>
        protected override int TestProcess()
        {
            float paraAdd1 = float.Parse(GetPara("paraAdd1", "10"));
            float paraAdd2 = float.Parse(GetPara("paraAdd2", "10"));
            float paraSum = float.Parse(GetPara("paraSum", "10"));

            Thread.Sleep(1000);

            //if (IF_TEST_STOP())
            //{
            //    return (int)TEST_STAT.FAL;
            //}

            //string strCode = m_Addins.UI.GetScanBarcode(int.Parse(GetPara("DutNum", "0")) - 1);
            //m_Addins.log.msg("Scan Code : " + strCode);

            m_dicImpTestInfo["BSN"] = "XXXXXXXXXXXXXXXX";

            Random rd = new Random();
          //  paraAdd1 = rd.Next();
            if (paraAdd1 + paraAdd2 == paraSum)
            {
                m_Addins.log.msg(string.Format("{0} + {1} = {2}", paraAdd1, paraAdd2, paraSum));
                result = (int)TEST_STAT.SUC;

            }
            else
            {
                m_Addins.log.err(string.Format("{0} + {1} = {2}, not {3}!", paraAdd1, paraAdd2, paraAdd1 + paraAdd2, paraSum));
                result = (int)TEST_STAT.WAR;
            }
            m_TestVal = (paraAdd1 + paraAdd2).ToString();

            return result;
        }
    }

    /// <summary>
    /// TestRunCmd
    /// </summary>
    [Export(typeof(TestUnit))]
    [ExportMetadata("AddinName", "TestRunCmd")]
    public class TestRunCmd : TestUnit
    {
        /// <summary>
        /// 测试项构造函数
        /// </summary>
        [ImportingConstructor]
        public TestRunCmd() : base("1000001", "TestRunCmd", 0) { }

        /// <summary>
        /// 测试项实际执行处理函数
        /// </summary>
        /// <returns>测试结果</returns>
        protected override int TestProcess()
        {
            string paraCmdExe = GetPara("paraCmdExe", "");
            string paraCmdStr = GetPara("paraCmdStr", "");
            string paraResultFlag = GetPara("paraResultFlag", "!@#$%");

            string strOutputMsg = "";
            using (Process myPro = new Process())
            {
                myPro.StartInfo.FileName = "cmd.exe";
                myPro.StartInfo.UseShellExecute = false;
                myPro.StartInfo.RedirectStandardInput = true;
                myPro.StartInfo.RedirectStandardOutput = true;
                myPro.StartInfo.RedirectStandardError = true;
                myPro.StartInfo.CreateNoWindow = true;
                myPro.Start();
                //如果调用程序路径中有空格时，cmd命令执行失败，可以用双引号括起来 ，在这里两个引号表示一个引号（转义）
                string str = string.Format(@"""{0}"" {1} {2}", paraCmdExe, paraCmdStr, "&exit");
                m_Addins.log.msg(string.Format(@"""{0}"" {1} {2}", paraCmdExe, paraCmdStr, "&exit"));

                myPro.StandardInput.WriteLine(str);
                myPro.StandardInput.AutoFlush = true;

                strOutputMsg = myPro.StandardOutput.ReadToEnd();
                m_Addins.log.msg("ReadMsg: " + strOutputMsg);

                myPro.WaitForExit();
            }

            if (strOutputMsg.Contains(paraResultFlag))
            {
                m_Addins.log.msg(string.Format(@"""{0}"" {1} {2} Success!", paraCmdExe, paraCmdStr, "&exit"));
                result = (int)TEST_STAT.SUC;
            }
            else
            {
                m_Addins.log.msg(string.Format(@"""{0}"" {1} {2} Fail, Cant`t find flag({3})!", paraCmdExe, paraCmdStr, "&exit", paraResultFlag));
                result = (int)TEST_STAT.FAL;
            }
            //m_TestVal = (paraAdd1 + paraAdd2).ToString();

            return result;
        }
    }
    #endregion

    #region InitList
    /// <summary>
    /// TestDelay
    /// </summary>
    [Export(typeof(TestUnit))]
    [ExportMetadata("AddinName", "TestDelay")]
    public class TestDelay : TestUnit
    {
        /// <summary>
        /// 测试项构造函数
        /// </summary>
        [ImportingConstructor]
        public TestDelay() : base("GL081", "TestDelay", 0) { }

        /// <summary>
        /// 测试项实际执行处理函数
        /// </summary>
        /// <returns>测试结果</returns>
        protected override int TestProcess()
        {
            //get test para

            int Delay = int.Parse(GetPara("paraDelayTime","0"));
            m_Addins.log.msg(string.Format("Sleep {0} ms",Delay));
            Thread.Sleep(Delay);
            return (int)TEST_STAT.SUC;
        }
    }

    /// <summary>
    /// TestInitialPowerSupply
    /// </summary>
    [Export(typeof(TestUnit))]
    [ExportMetadata("AddinName", "TestInitialPowerSupply")]
    public class TestInitialPowerSupply : TestUnit
    {
        /// <summary>
        /// 测试项构造函数
        /// </summary>
        [ImportingConstructor]
        public TestInitialPowerSupply() : base("INIT001", "TestInitialPowerSupply", 0){}

        /// <summary>
        /// 测试项实际执行处理函数
        /// </summary>
        /// <returns>测试结果</returns>
        protected override int TestProcess()
        {
            //get test para
            string gpibAddr = GetPara("paraPowerAddr", "GPIB0::5::INSTR");
            int iTimeOut = int.Parse(GetPara("paraTimeOut", "1000"));
            bool bDoRst = GetPara("paraReset", "TRUE").ToUpper() == "TRUE" ? true : false;

            string paraPowerSupplyEnable = GetPara("paraPowerSupplyEnable", "TRUE").ToUpper();
            if (paraPowerSupplyEnable == "FALSE")
            {
                return (int)TEST_STAT.SUC;
            }
            
            //Main Process        
            if (m_Addins.pwr.InitialInstr(gpibAddr, iTimeOut, bDoRst))
            {
                m_Addins.log.msg(string.Format("Connect to {0} Success!", gpibAddr));
                result = (int)TEST_STAT.SUC;
            }
            else
            {
                m_Addins.log.msg(string.Format("Connect to {0} Fail!", gpibAddr));
                result = (int)TEST_STAT.FAL;
            }
            
            return result;
        }
    }
    #endregion

    #region ExitList
    /// <summary>
    /// TestReleasePowerSupply
    /// </summary>
    [Export(typeof(TestUnit))]
    [ExportMetadata("AddinName", "TestReleasePowerSupply")]
    public class TestReleasePowerSupply : TestUnit
    {
        /// <summary>
        /// 测试项构造函数
        /// </summary>
        [ImportingConstructor]
        public TestReleasePowerSupply() : base("EXIT003", "TestReleasePowerSupply", 0){}

        /// <summary>
        /// 测试项实际执行处理函数
        /// </summary>
        /// <returns>测试结果</returns>
        protected override int TestProcess()
        {
            string OutputStatus = GetPara("paraOutputOff", "TRUE");
            string paraPowerSupplyEnable = GetPara("paraPowerSupplyEnable", "TRUE").ToUpper();
            if (paraPowerSupplyEnable == "FALSE")
            {
                return (int)TEST_STAT.SUC;
            }
            if (OutputStatus.ToUpper() == "TRUE")
            {
                m_Addins.pwr.PowerOff();
            }
            m_Addins.pwr.ReleaseInstr();

            return (int)TEST_STAT.SUC;
        }
    }
    #endregion

    #region SysCases

    /// <summary>
    /// TestInitialSys
    /// </summary>
    [Export(typeof(TestUnit))]
    [ExportMetadata("AddinName", "TestInitialSys")]
    public class TestInitialSys : TestUnit
    {
        /// <summary>
        /// 测试项构造函数
        /// </summary>
        [ImportingConstructor]
        public TestInitialSys() : base("INIT100", "TestInitialSys", 0) { }

        /// <summary>
        /// 测试项初始化函数
        /// </summary>
        /// <returns>测试结果</returns>
        protected override int TestInitial()
        {
            return (int)TEST_STAT.SUC;
        }

        /// <summary>
        /// 测试项实际执行处理函数
        /// </summary>
        /// <returns>测试结果</returns>
        protected override int TestProcess()
        {
            //if paraLogEn TRUE
            if (GetPara("paraLogEn", "TRUE").ToUpper() == "TRUE")
            {
                //get test para
                string paraLogPath = GetPara("LogPath", "D:\\");

                //Main Process
                m_Addins.log.InitialLog(paraLogPath);
                m_Addins.log.msg("Initial Log Success!");
            }

            string Station = GetPara("Station","");
            string Project = GetPara("Project","");


            //Clear sys para
            m_dicImpTestInfo.Clear();
            foreach (var key in m_dicImpTestInfo.Keys.ToArray().Where(key => key != "SYS_STAT"))
            {
                m_dicImpTestInfo.Remove(key);
            }
            m_Addins.log.msg("Clear m_dicImpTestInfo Success!");

            string BeforDT = System.DateTime.Now.ToString();
            m_Addins.log.msg("Test start time:" + BeforDT);
            m_dicImpTestInfo.Add("TestTime", BeforDT);
            result = (int)TEST_STAT.SUC;

            return result;
        }
    }

    /// <summary>
    /// TestExitSys
    /// </summary>
    [Export(typeof(TestUnit))]
    [ExportMetadata("AddinName", "TestExitSys")]
    public class TestExitSys : TestUnit
    {
        /// <summary>
        /// 测试项构造函数
        /// </summary>
        [ImportingConstructor]
        public TestExitSys() : base("EXIT100", "TestExitSys", 0) { }

        /// <summary>
        /// 测试项实际执行处理函数
        /// </summary>
        /// <returns>测试结果</returns>
        protected override int TestProcess()
        {
            //if paraLogEn TRUE
            if (GetPara("LogEn", "TRUE").ToUpper() != "TRUE")
            {
                m_Addins.log.msg(string.Format("TestExitSys() paraLogEn is FALSE"));
                return (int)TEST_STAT.SUC;
            }

            //get test para
            string paraLogPath = GetPara("LogPath", "D:\\");
            string paraStation = GetPara("Station", "NO_STATION");
            string paraProject = GetPara("Project", "NO_PROJECT");

            string paraRes = m_dicImpTestInfo.ContainsKey("TEST_RES") ? m_dicImpTestInfo["TEST_RES"] : "FAIL";
            bool bRes = paraRes.ToUpper() == "PASS" ? true : false;

            DateTime dt = System.DateTime.Now;
            if (m_dicImpTestInfo.ContainsKey("TestTime"))
            {
                string BefortDT = m_dicImpTestInfo["TestTime"];
                dt = Convert.ToDateTime(BefortDT);

            }
            DateTime TestEnd = System.DateTime.Now;
            double AfterDT = (TestEnd - dt).TotalSeconds;
            m_Addins.log.msg("Test End time:" + TestEnd.ToString());

            m_Addins.log.msg(string.Format("Total Test time:{0} second", AfterDT));

            dt = System.DateTime.Now;
            string ShortDate = dt.ToString("yyyy-MM-dd");
            string LocalTime = dt.ToString("yyyy-MM-dd HH.mm.ss.fff");
            m_Addins.log.WirteLogToFile(int.Parse(GetPara("DutNum", "1")), paraStation, paraLogPath, bRes, ShortDate, LocalTime);//log的日期和时间从外部输入，便于在UI界面查找
            m_Addins.log.msg("Write Log Success!");

            return (int)TEST_STAT.SUC;
        }
    }
    #endregion

}