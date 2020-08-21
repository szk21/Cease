using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cease.Addin;

namespace Cease.Test
{
    public interface ITestGlobalUnit
    {
        /// <summary>
        /// 系统运行
        /// </summary>
        /// <returns>返回执行结果</returns>
        bool Run();

        /// <summary>
        /// 获取测试参数，生成待测项测试列表，依赖于XML文件
        /// </summary>
        /// <param name="_dic">Framework字典</param>
        /// <param name="_testUnitDB">测试库</param>
        /// <param name="_addinDB">插件库</param>
        /// <returns>返回执行结果</returns>
        bool CreateAllTestCases(string stationName);

        bool Initial();

        void UiUpdateLogRegister(EventHandler MakeLog);
    }
}
