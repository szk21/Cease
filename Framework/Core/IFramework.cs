using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cease.Interface.Log;

namespace Cease.Core
{

    public interface IFramework
    {
        /// <summary>
        /// 构造Framework测试引擎
        /// </summary>
        /// <param name="configName">测试项目名称</param>
        /// <returns>返回执行结果</returns>
        bool CreateTestEngine();

        /// <summary>
        /// Framework启动入口
        /// </summary>
        /// <returns></returns>
        void StartEngine(string stationName);

        void UiUpdateLogRegister(EventHandler MakeLog);
    }
}
