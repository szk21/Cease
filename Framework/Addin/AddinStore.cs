using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel.Composition;

using Cease.Addins.PowerCtrl;
using Cease.Addins.Log;

namespace CEASE.Addin
{
    /// <summary>
    /// 名称元数据接口
    /// </summary>
    public interface IAddinMetaData
    {
        /// <summary>
        /// 名称
        /// </summary>
        string AddinName { get; }
    }

    /// <summary>
    /// 插件管理控制类
    /// </summary>
    public class AddinDB
    {
        /// <summary>
        /// 电源控制接口迭代器
        /// </summary>
        [ImportMany]
        public IEnumerable<Lazy<InterfacePowerCtrl, IAddinMetaData>> IPowerCtrlCommands;

        /// <summary>
        /// Log接口迭代器
        /// </summary>
        [ImportMany]
        public IEnumerable<Lazy<InterfaceLog, IAddinMetaData>> ILogCommands;

        /// <summary>
        /// 插件库属性
        /// </summary>
        public AddinStore Itfs { get; private set; }

        /// <summary>
        /// 插件管理控制构造类
        /// </summary>
        public AddinDB()
        {
            Itfs = new AddinStore();
        }

        /// <summary>
        /// 插件注册
        /// </summary>
        /// <param name="itfEnu">插件迭代器</param>
        /// <param name="_name">插件名称</param>
        /// <returns>插件接口</returns>
        private T GetAddinInterface<T>(ref IEnumerable<Lazy<T, IAddinMetaData>> itfEnu, string _name)
        {
            T itf = default(T);
            foreach (var c in itfEnu.Where(i => i.Metadata.AddinName == _name))
            {
                itf = c.Value;
                break;
            }

            return itf;
        }

        /// <summary>
        /// 插件工厂
        /// </summary>
        /// <param name="_type">插件接口类型</param>
        /// <param name="_name">插件名称</param>
        /// <returns>成功返回1，失败返回0</returns>
        public int AddinFactory(string _type, string _name)
        {
            switch (_type)
            {
                case "IPowerCtrl":
                    Itfs.pwr = GetAddinInterface<InterfacePowerCtrl>(ref IPowerCtrlCommands, _name);
                    break;
                case "ILog":
                    Itfs.log = GetAddinInterface<InterfaceLog>(ref ILogCommands, _name);
                    break;

                default:
                    return 0;
            }

            return 1;
        }

        /// <summary>
        /// 各个插件注册log接口
        /// </summary>
        /// <param name="_type">插件接口类型</param>
        /// <param name="_name">插件名称</param>
        /// <returns>成功返回1，失败返回0</returns>
        public bool AddinRegisterLog()
        {
            if (Itfs.log == null)
            {
                return false;
            }

            Itfs.log.SetDut(0);

            if (Itfs.pwr != null)
            {
                Itfs.pwr.RegisterLogger(Itfs.log);
            }

            return true;
        }
    }

    /// <summary>
    /// 插件库
    /// </summary>
    public class AddinStore
    {
        /// <summary>
        /// 电源接口
        /// </summary>
        public InterfacePowerCtrl pwr = null;

        /// <summary>
        /// Log接口
        /// </summary>
        public InterfaceLog log = null;
    }
}
