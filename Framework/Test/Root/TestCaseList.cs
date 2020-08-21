using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;

namespace Cease.Test.Root
{
    public class TestCase
    {
        public TestUnit test;
        public Dictionary<string, string> para;

        public TestCase(TestUnit t, Dictionary<string, string> p, Cease.Addin.AddinStore a)
        {
            test = t;
            para = p;
            t.RegisterAddins(a);
        }

        public void Register(Dictionary<string, string> _dic)
        {
            test.RegisterTestPara(para, _dic);
        }
    }

    public class TestCaseList : List<TestCase>
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected ILog log;

        public Dictionary<string, string> dic { get; private set; }

        public TestCaseList(ILog _log)
        {
            this.log = _log;
            dic = new Dictionary<string, string>();
        }

        /// <summary>
        /// 执行测试系统列表中每个测试项
        /// </summary>
        /// <returns>返回执行结果</returns>
        public int Run()
        {
            try
            {
                dic["SYS_STAT"] = "RUN";
                int tmpRes = (int)TEST_STAT.SUC;
                log.Debug("TestListRun:");
                if (Count == 0)
                {
                    return (int)TEST_STAT.SUC;
                }

                int i = 0;
                foreach (var testCase in this)
                {
                    int iTestItemOrder = i++;

                    testCase.Register(dic);
                    if (testCase.test.TestRun() != (int)TEST_STAT.SUC)
                    {
                        tmpRes = (int)TEST_STAT.FAL;
                        dic["TEST_RES"] = "FAIL";

                        if (1 == testCase.test.KeyTest)         //还原keytest==1时中断测试
                        {
                            break;
                        }
                    }
                }

                return tmpRes;
            }
            catch (System.Exception ex)
            {
                log.Error("Exception in TestListRun.", ex);
                throw new Exception("Exception in TestListRun.  Exception = " + ex.Message);
            }
        }
    }
}
