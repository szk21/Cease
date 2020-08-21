using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Cease.Test.Root;
using System.IO;
using System.Windows.Forms;

namespace Cease.Core
{
    public abstract class XmlBase
    {
        /// <summary>
        /// XML文件名
        /// </summary>
        public String XmlFileName;

        /// <summary>
        /// XML文档资源
        /// </summary>
        public XmlDocument DocRsce;

        /// <summary>
        /// 根节点名称
        /// </summary>
        public String RootName;

        /// <summary>
        /// XML解析类构造函数
        /// </summary>
        /// <param name="FileName">XML文件名</param>
        public XmlBase(string FileName)
        {
            XmlFileName = FileName;
            if (!System.IO.File.Exists(XmlFileName))
            {
                throw new FileNotFoundException("Can not find " + XmlFileName);
            }

            DocRsce = new XmlDocument();
            DocRsce.Load(XmlFileName);
            RootName = DocRsce.DocumentElement.Name;
        }

        public abstract void ParsePara(string section);
    }

    public class XmlDictionary : XmlBase
    {
        public Dictionary<string, Dictionary<string, string>> dic { get; protected set; }

        /// <summary>
        /// XML解析类构造函数
        /// </summary>
        /// <param name="FileName">XML文件名</param>
        public XmlDictionary(string FileName) : base(FileName)
        {
            dic = new Dictionary<string, Dictionary<string, string>>();
        }

        public override void ParsePara(string section)
        {
            XmlNode node = DocRsce.SelectSingleNode(RootName + "/" + section);
            if (node == null)
                throw new XmlException("can not find section: " + section);

            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name.Contains("#comment"))
                    continue;

                dic.Add(n.Name, new Dictionary<string, string>());
                foreach (XmlAttribute attr in n.Attributes)
                {
                    dic[n.Name].Add(attr.Name, attr.Value);
                }
            }
        }
    }

    public class XmlSysConfig : XmlDictionary
    {
        public Dictionary<string, string> dicPara { get; protected set; }
        public XmlSysConfig(string FileName = "config.xml") :base(FileName)
        {
            ParsePara("START");
        }

        public string GetTestXmlPath()
        {
            return "\\Project\\"  + dic["Station"]["Value"] + "\\" +  dic["Station"]["Value"] + ".xml";
        }

        public override void ParsePara(string section)
        {
            base.ParsePara(section);

            dicPara = new Dictionary<string, string>();
            foreach(var k in dic.Keys)
            {
                dicPara.Add(k, dic[k]["para" + k]);
            }
        }
    }

    public class XmlAddinsSetting : XmlDictionary
    {
        public XmlAddinsSetting(string fileName = "\\config.xml") : base(fileName)
        {
            ParsePara("AddinsSettings");
        }

        public string DllName(string _key)
        {
            return _key + ".dll";
        }
    }

    public class XmlTestCaseConfig : XmlDictionary
    {
        public List<Dictionary<string, string>> lst { get; private set; }

        public XmlTestCaseConfig(string fileName) : base(fileName)
        {
            lst = new List<Dictionary<string, string>>();
            base.ParsePara("Common");
            ParsePara("TestCases");
        }

        public Dictionary<string, string> GetCommonDic()
        {
            var commonDic = new Dictionary<string, string>();
            foreach(var item in dic)
            {
                commonDic.Add(item.Key, item.Value["para" + item.Key]);
            }

            return commonDic;
        }

        public override void ParsePara(string section)
        {
            XmlNode node = DocRsce.SelectSingleNode(RootName + "/" + section);
            if (node == null)
                throw new XmlException("can not find section: " + section);

            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name.Contains("#comment"))
                    continue;

                lst.Add(new Dictionary<string, string>());
                foreach (XmlAttribute attr in n.Attributes)
                {
                    lst.Last().Add(attr.Name, attr.Value);
                }

                lst.Last()["TestCaseName"] = n.Name;
                if (!lst.Last().ContainsKey("paraName"))
                {
                    lst.Last()["paraName"] = n.Name;
                }
            }
        }
    }

    public class XmlList : XmlBase
    {
        public List<Dictionary<string, string>> lst { get; private set; }

        /// <summary>
        /// XML解析类构造函数
        /// </summary>
        /// <param name="FileName">XML文件名</param>
        public XmlList(string FileName) : base(FileName)
        {
            lst = new List<Dictionary<string, string>>();
        }

        public override void ParsePara(string section)
        {
            XmlNode node = DocRsce.SelectSingleNode(RootName + "/" + section);
            if (node == null)
                throw new XmlException("can not find section: " + section);

            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name.Contains("#comment"))
                    continue;

                lst.Add(new Dictionary<string, string>());
                foreach (XmlAttribute attr in n.Attributes)
                {
                    lst.Last().Add(attr.Name, attr.Value);
                }
                lst.Last().Add("NodeName", n.Name);
            }
        }
    }
}
