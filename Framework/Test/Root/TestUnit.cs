using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cease.Addin;

namespace Cease.Test.Root
{
    /// <summary>
    /// 测试状态
    /// </summary>
    public enum TEST_STAT
    {
        FAL,
        SUC,
        ING,
        RDY,
        STP,
        WAR
    };

    /// <summary>
    /// 测试项抽象类
    /// </summary>
    public abstract class TestUnit : BaseClass, InterfaceTest
    {
        /// <summary>
        /// 测试名称
        /// </summary>
        readonly string name;
        public string Name { get { return name; } }

        /// <summary>
        /// 测试ID
        /// </summary>
        readonly string id;
        public string Id { get { return id; } }

        /// <summary>
        /// 关键测试项标志位
        /// </summary>
        readonly int keyTest;
        public int KeyTest { get { return keyTest; } }

        /// <summary>
        /// 测试项标序号
        /// </summary>
        public string itemIdx { get { return "ItemIdx"; } }

        /// <summary>
        /// 测试项标序号
        /// </summary>
        public string testInfo { get { return "TestInfo"; } }

        /// <summary>
        /// 测试值
        /// </summary>
        public string m_TestVal { get; set; }

        /// <summary>
        /// 测试结果
        /// </summary>
        public int result { get; set; }

        /// <summary>
        /// 插件库接口
        /// </summary>
        protected AddinStore m_Addins;

        /// <summary>
        /// 测试参数字典
        /// </summary>
        protected Dictionary<string, string> m_dicTestPara;

        /// <summary>
        /// 测试过程相关信息记录字典，用于和GlobalUnit交互数据
        /// </summary>
        protected Dictionary<string, string> m_dicImpTestInfo;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="case_num">测试项名称</param>
        /// <param name="case_name">测试项ID</param>
        /// <param name="isKey">关键测试项标志位</param>
        public TestUnit(string case_num, string case_name, int isKey)
        {
            this.name = case_name;
            this.id = case_num;
            this.keyTest = isKey;

            this.result = (int)TEST_STAT.RDY;
        }

        /// <summary>
        /// 注册插件库
        /// </summary>
        /// <param name="_addinsStore">插件库</param>
        /// <returns>返回执行结果</returns>
        public int RegisterAddins(AddinStore _addinsStore)
        {
            m_Addins = _addinsStore;
            return 1;
        }

        /// <summary>
        /// 从参数字典获取参数的接口
        /// </summary>
        /// <param name="_key">对应Key值</param>
        /// <param name="_dftVal">缺省值</param>
        /// <returns>返回执行结果</returns>
        protected string GetPara(string _key, string _dftVal)
        {
            if (m_dicTestPara.ContainsKey(_key))
            {
                m_Addins.log.msg(string.Format("Get key:{0}, value:{1}", _key, m_dicTestPara[_key]));
                return m_dicTestPara[_key];
            }
            else
            {
                m_Addins.log.warn(string.Format("Can not find key:{0} in m_dicTestPara, use default {1} instead!", _key, _dftVal));
                return _dftVal;
            }
        }

        /// <summary>
        /// 从数据交换的参数字典获取参数的接口
        /// </summary>
        /// <param name="_key">对应Key值</param>
        /// <param name="_dftVal">缺省值</param>
        /// <returns>返回执行结果</returns>
        protected string GetImlPara(string _key, string _dftVal)   //rocky
        {
            if (m_dicImpTestInfo.ContainsKey(_key))
            {
                return m_dicImpTestInfo[_key];
            }
            else
            {
                m_Addins.log.warn(string.Format("Can not find key:{0} in m_dicImpTestInfo, use default {1} instead!", _key, _dftVal));
                return _dftVal;
            }
        }

        /// <summary>
        /// 注册测试参数
        /// </summary>
        /// <param name="_dicTestPara">测试参数字典</param>
        /// <returns>返回执行结果</returns>
        public int RegisterTestPara(Dictionary<string, string> _dicTestPara, Dictionary<string, string> _dicImpInfo)
        {
            m_dicTestPara = _dicTestPara;
            m_dicImpTestInfo = _dicImpInfo;

            return 1;
        }

        /// <summary>
        /// 执行测试
        /// </summary>
        /// <returns>返回测试结果</returns>
        public int TestRun()
        {
            try
            {
                result = (int)TEST_STAT.SUC;
                result = TestInitial();
                if (result == (int)TEST_STAT.SUC)
                {
                    result = TestProcess();
                }
            }
            catch (System.Exception ex)
            {
                result = (int)TEST_STAT.FAL;
                m_Addins.log.err("Exception in " + name + "-TestRun()." + ex.Message);
            }
            finally
            {
                TestEnd();
            }

            return result;
        }

        /// <summary>
        /// 测试项初始化
        /// </summary>
        /// <returns>返回初始化结果</returns>
        protected virtual int TestInitial()
        {
            m_Addins.log.msg(new string('-', 50) + "TestCase:" + id + "-" + name + " Begin:" + new string('-', 50));
            m_Addins.log.msg("TestInitial Begin");
            m_TestVal = " ";

            //if (IF_TEST_STOP())
            //{
            //    return (int)TEST_STAT.FAL;
            //}

            return (int)TEST_STAT.SUC;
        }

        /// <summary>
        /// 测试主体函数
        /// </summary>
        /// <returns>返回测试结果</returns>
        protected abstract int TestProcess();

        /// <summary>
        /// 测试结束，释放资源
        /// </summary>
        /// <returns>返回测试结果</returns>
        protected virtual int TestEnd()
        {
            m_Addins.log.msg("TestEnd Begin");

            //failItem & failMsg
            if (result != (int)TEST_STAT.SUC)
            {
                m_dicImpTestInfo["FailItem"] = id + '-' + name;
                m_dicImpTestInfo["FailMsg"] = m_Addins.log.GetLastErrMsg();
            }
            m_Addins.log.msg(new string('-', 50) + "TestCase:" + id + "-" + name + " End" + new string('-', 50));

            return result;
        }

        /// <summary>
        /// 是否手动点测试停止
        /// </summary>
        /// <returns>true,手动点停止;</returns>
        protected bool IF_TEST_STOP()
        {
            //if (m_Addins.UI.GetDutStatus(int.Parse(GetPara("DutNum", "1")) - 1) != "STOP")
            //{
            //    return false;
            //}
            m_Addins.log.msg("Test Stop");

            return true;
        }
    }
}
