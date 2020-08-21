using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using System.Windows.Forms;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;

using Cease.Addin;
using Cease.Test.Root;
using Cease.Core;

using log4net;
using System.Collections;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;

namespace Cease.Test
{
    /// <summary>
    /// 导入的测试项全集
    /// </summary>
    public class TestUnitDB
    {
        [ImportMany]
        public IEnumerable<Lazy<TestUnit, IAddinMetaData>> ITestUnits;
    }

    /// <summary>
    /// 测试控制单元
    /// </summary>
    public class TestGlobalUnit : ITestGlobalUnit
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected ILog log;

        /// <summary>
        /// 插件库对象列表
        /// </summary>
        public AddinDB m_AddinDB;

        /// <summary>
        /// 测试项库对象列表
        /// </summary>
        private TestUnitDB m_TestUnitDB;

        protected TestCaseList m_TestList;

        protected int index { get; private set; }

        /// <summary>
        /// 测试结果
        /// </summary>
        protected int result { get; private set; }

        /// <summary>
        /// CeaseFramework启动路径
        /// </summary>
        protected string Path { get; private set; }

        /// <summary>
        /// GlobalUnit构造
        /// </summary>
        public TestGlobalUnit(int idx, ILog _log)
        {
            this.index = idx;
            this.log = _log;
            Path = Application.StartupPath;
            this.result = (int)TEST_STAT.RDY;
        }

        /// <summary>
        /// 拷贝插件
        /// </summary>
        /// <param name="iDut">Dut编号</param>
        /// <param name="strType">插件类型名（Addins or TestUnits）</param>
        protected void CopyAddins(string strType)
        {
            //Copy Addins to DUTx
            string strMultiDutPath = Path + "\\MultiDut\\DUT" + index.ToString();
            if (!Directory.Exists(strMultiDutPath + "\\" + strType))
            {
                // not exist, then create
                Directory.CreateDirectory(strMultiDutPath + "\\" + strType);
            }
            else
            {
                //exist, then delete all
                DirectoryInfo dir = new DirectoryInfo(strMultiDutPath + "\\" + strType);
                dir.Delete(true);
            }
            CopyDirectory(Path + "\\" + strType, strMultiDutPath);
        }

        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="srcdir">源文件夹目录</param>
        /// <param name="desdir">目的文件夹目录</param>
        protected void CopyDirectory(string srcdir, string desdir)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

            string desfolderdir = desdir + "\\" + folderName;

            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }

                    CopyDirectory(file, desfolderdir);
                }
                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);
                    srcfileName = desfolderdir + "\\" + srcfileName;

                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }

                    File.Copy(file, srcfileName);
                }
            }//foreach 
        }

        /// <summary>
        /// 加载插件库
        /// </summary>
        /// <returns>返回执行结果</returns>
        protected bool loadAddinStore()
        {
            log.Info("Dut[" + index.ToString() + "] Load Addins Begin");

            //Copy Addins to DUTx
            string strMultiDutPath = Path + "\\MultiDut\\DUT" + index.ToString();
            CopyAddins("Addins");

            m_AddinDB = new AddinDB();
            var catalog = new AggregateCatalog();

            var AddinsXml = new XmlAddinsSetting(Path + "\\config.xml");
            foreach (string k in AddinsXml.dic.Keys)
            {
                //check exist
                string strAddinsPath = strMultiDutPath + "\\Addins\\" + k;
                if (!File.Exists(strAddinsPath + "\\" + AddinsXml.DllName(k)))
                {
                    log.Error("Dut[" + index.ToString() + "] Can not find " + strAddinsPath + "\\" + AddinsXml.DllName(k));
                    return false;
                }

                //check version
                FileVersionInfo AddinsVersionInfo = FileVersionInfo.GetVersionInfo(strAddinsPath + "\\" + AddinsXml.DllName(k));
                string DllVersionInXml = AddinsXml.dic[k]["Ver"];
                if (AddinsVersionInfo.FileVersion != DllVersionInXml)
                {
                    log.Error(AddinsXml.DllName(k) + " Version is " + AddinsVersionInfo.FileVersion + ", not " + DllVersionInXml + " in XML!");
                    return false;
                }

                //Add addins
                catalog.Catalogs.Add(new DirectoryCatalog(strAddinsPath, AddinsXml.DllName(k)));
            }
            new CompositionContainer(catalog).ComposeParts(m_AddinDB);

            foreach (string k in AddinsXml.dic.Keys)
            {
                m_AddinDB.AddinFactory(AddinsXml.dic[k]["Interface"], k);
            }

            return true;
        }

        /// <summary>
        /// 加载测试项库
        /// </summary>
        /// <returns></returns>
        protected bool loadTestUnitStore()
        {
            log.Info("Load TestUnit Begin");
            //Copy TestStore to DUTx
            string strMultiDutPath = Path + "\\MultiDut\\DUT" + index.ToString();
            CopyAddins("TestUnits");

            m_TestUnitDB = new TestUnitDB();
            var catalog = new AggregateCatalog();

            if (!System.IO.Directory.Exists(strMultiDutPath + "\\TestUnits"))
            {
                log.Error("Dut[" + index.ToString() + "] Can not find " + strMultiDutPath + "\\TestUnits");
                return false;
            }

            catalog.Catalogs.Add(new DirectoryCatalog(Path + "\\TestUnits", "*.dll"));
            new CompositionContainer(catalog).ComposeParts(m_TestUnitDB);

            return true;
        }

        /// <summary>
        /// 系统运行
        /// </summary>
        /// <returns>返回执行结果</returns>
        public bool Run()
        {
            try
            {
                log.Debug("System Test Run Begin:");
                result = m_TestList.Run();
            }
            catch (System.Exception ex)
            {
                result = (int)TEST_STAT.FAL;
                log.Error("Exception in Run.", ex);
                string strErrMsg = "Exception in Run.  Exception = " + ex.Message;
            }

            return result == (int)TEST_STAT.SUC ? true : false;
        }

        public bool Initial()
        {
            //addins
            if (!loadAddinStore())
            {
                throw new Exception(string.Format("loadAddinStore Fail!"));
            }

            if (!m_AddinDB.AddinRegisterLog())
            {
                throw new Exception(string.Format("m_AddinDB.AddinRegisterLog Fail!"));
            }

            //TestUnit
            if (!loadTestUnitStore())
            {
                throw new Exception(string.Format("loadTestUnitStore Fail!"));
            }

            return true;
        }

        /// <summary>
        /// 生成待测项测试列表，依赖于XML文件
        /// </summary>
        /// <param name="_strStation">测试工站名称</param>
        /// <returns>返回执行结果</returns>
        public bool CreateAllTestCases(string stationName)
        {
            m_TestList = new TestCaseList(log);

            // for para
            XmlSysConfig configDic = new XmlSysConfig(Path + "\\config.xml");
            XmlTestCaseConfig TestConfigXml = new XmlTestCaseConfig(Path + "\\Project\\" + stationName + "\\" + stationName + ".xml");
            var dicPara = TestConfigXml.GetCommonDic().Concat(configDic.dicPara).ToDictionary(k => k.Key, v => v.Value);
            dicPara["Station"] = stationName;

            foreach (var caseDic in TestConfigXml.lst)
            {
                foreach (var c in m_TestUnitDB.ITestUnits.Where(t => t.Metadata.AddinName == caseDic["TestCaseName"]))
                {
                    //parse para
                    if (caseDic.ContainsKey("paraChecked") && caseDic["paraChecked"].ToUpper() == "FALSE")
                    {
                        continue;
                    }

                    //register TestUnit
                    m_TestList.Add(new TestCase
                        (c.Value, 
                        caseDic.Concat(dicPara).ToDictionary(k => k.Key, v => v.Value), 
                        m_AddinDB.Itfs));
                    break;
                }
            }

            return true;
        }

        public void UiUpdateLogRegister(EventHandler MakeLog)
        {
            m_AddinDB.Itfs.log.Logged += MakeLog;
        }
    }
}