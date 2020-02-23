using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Runtime.InteropServices;
using System.Xml;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Xml.XPath;

using CEASE.Core;

namespace CreateTestStoreConfig
{
    public partial class CeaseHelp : Form
    {
        private static string _addinsName = "";//"CeaseDualCamera,CeaseLog,CeaseMes,QPhone,GpibPowerCtrl,ZeusisFixture,CeaseUI,RFInstrument,QC_RFCalServer,SocketListener,CeaseUsbDevice";
        protected string m_strPath;
        protected List<string> m_listAddinsName;
        private XmlDocument DocTestStoreConfig;
        private XmlDocument DocTestCaseXml;
        private XmlDocument DocCablelossXml;
        private string myXMLPath;
        public CeaseHelp()
        {
            InitializeComponent();
            m_strPath = Application.StartupPath;
            var xmlConfig = new XmlSysConfig();

            comboBoxStation.Items.Add(xmlConfig.dic["Station"]);
            comboBoxDut.Items.Add(1);
            for (int i = 0; i <= 30; i++ )
            {
                comboBoxCablelossInitValue.Items.Add(i);
            }
            comboBoxProject.Text = xmlConfig.dic["Project"]["Value"];
            comboBoxStation.Text = xmlConfig.dic["Station"]["Value"];
            comboBoxDut.Text = "0";
            comboBoxCablelossInitValue.Text = "0";
            GetTestStoreConfigFile(comboBoxProject.Text, ref _addinsName);
            m_listAddinsName = new List<string>(_addinsName.Split(','));
            InitListVersion();
        }

        private void CreateTestStoreConfig_Click(object sender, EventArgs e)
        {
            if (SaveListVersion())
            {
                MessageBox.Show(string.Format("{0}项目文件TestStoreConfig.xml已更新", comboBoxProject.Text));
            }
        }
        private bool SaveListVersion()
        {
            //addins & TestStore
            FileVersionInfo AddinsVersionInfo = FileVersionInfo.GetVersionInfo(m_strPath + "\\TestUnits\\Cease.TestStore.dll");
            ListViewItem item = new ListViewItem((AddinsVersionInfo.OriginalFilename + "," + AddinsVersionInfo.FileVersion).Split(','));

            foreach (var _name in m_listAddinsName)
            {
                AddinsVersionInfo = FileVersionInfo.GetVersionInfo(m_strPath + "\\Addins\\" + _name + "\\" + _name + ".dll");
                SetTestStoreConfigFile(comboBoxProject.Text, _name, AddinsVersionInfo.FileVersion);
            }
            return true;
        }
        private void InitListVersion()
        {
            //addins & TestStore
            try
            {
                FileVersionInfo AddinsVersionInfo = FileVersionInfo.GetVersionInfo(m_strPath + "\\TestUnits\\Cease.TestStore.dll");
                ListViewItem item = new ListViewItem((AddinsVersionInfo.OriginalFilename + "," + AddinsVersionInfo.FileVersion).Split(','));
                //listViewAddins.Items.Add(item);

                foreach (var _name in m_listAddinsName)
                {
                    AddinsVersionInfo = FileVersionInfo.GetVersionInfo(m_strPath + "\\Addins\\" + _name + "\\" + _name + ".dll");
                    item = new ListViewItem((AddinsVersionInfo.OriginalFilename + "," + AddinsVersionInfo.FileVersion).Split(','));
                    listViewAddins.Items.Add(item);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private bool CheckTestStoreConfigFile(string strProject)
        {
            myXMLPath = m_strPath + "\\project\\TestStoreConfig.xml";
            if (!System.IO.File.Exists(myXMLPath))
            {
                MessageBox.Show(string.Format("请检查 project 项目目录下是否有TestStoreConfig.xml文件"));
                return false;
            }
            return true;
        }
        private bool GetTestStoreConfigFile(string strProject, ref string AddinName)
        {
            if (!CheckTestStoreConfigFile(comboBoxProject.Text))
            {
                return false;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(m_strPath + "\\project\\TestStoreConfig.xml");
            string RootName = xmlDoc.DocumentElement.Name;
            XmlNode RootNode = xmlDoc.SelectSingleNode(RootName + "/" + "AddinsSettings");
            XmlNodeList AddinsNameNode = RootNode.ChildNodes;
            foreach (XmlNode node in AddinsNameNode)
            {
                AddinName += node.Name + ",";
            }
            AddinName = AddinName.TrimEnd(',');
            return true;
        }
        private bool SetTestStoreConfigFile(string strProject,string AddinName, string AddinVersion)
        {
            DocTestStoreConfig = new XmlDocument();
            DocTestStoreConfig.Load(myXMLPath);
            string RootName = DocTestStoreConfig.DocumentElement.Name;
            XmlNode RootNode = DocTestStoreConfig.SelectSingleNode(RootName + "/" + "AddinsSettings");
            XmlNodeList AddinsNameNode = RootNode.ChildNodes;

            foreach (XmlNode node in AddinsNameNode)
            {
                if (AddinName == node.Name)
                {
                    node.Attributes.GetNamedItem("Ver").Value = AddinVersion;
                }
                else
                {
                    continue;
                }
            }

            DocTestStoreConfig.Save(myXMLPath);

            return true;
        }

        private void CheckCablelossItem_Click(object sender, EventArgs e)
        {
            string[] Stations = {"BT","MT","BTW","MTW","MT2"};
            var CheckMesg = new List<string>();
            if (!Stations.Contains(comboBoxStation.Text))
            {
                MessageBox.Show(string.Format("请选择正确的工位，工位名称应是BT,MT,BTW,MTW,MT2"));
                return;
            }

            string FileTestCaseXml = System.Environment.CurrentDirectory + "\\Project\\" + comboBoxProject.Text + "\\" + comboBoxStation.Text + "\\" + comboBoxProject.Text + "_" + comboBoxStation.Text + ".xml";
            string FileCablelossXml = System.Environment.CurrentDirectory + "\\Project\\" + comboBoxProject.Text + "\\" + comboBoxStation.Text + "\\Cableloss.xml";

            if (!System.IO.File.Exists(FileTestCaseXml))
            {
                MessageBox.Show(string.Format("检测到在{0}\\{1}目录下没有{2}_{3}.xml文件", comboBoxProject.Text, comboBoxStation.Text, comboBoxProject.Text, comboBoxStation.Text));
                return;
            }
            if (!System.IO.File.Exists(FileCablelossXml))
            {
                MessageBox.Show(string.Format("检测到在{0}\\{1}目录下没有Cableloss.xml文件", comboBoxProject.Text, comboBoxDut.Text));
                return;
            }
            DocCablelossXml = new XmlDocument();
            DocCablelossXml.Load(FileCablelossXml);
            if (DocCablelossXml.SelectSingleNode("CableLossConfig" + "/" + "CableLossDUT" + comboBoxDut.Text) == null)
            {
                MessageBox.Show(string.Format("检测到在{0}\\{1}目录下Cableloss.xml文件中没有DUT{2}的线损设置", comboBoxProject.Text, comboBoxStation.Text, comboBoxDut.Text));
                return;
            }
            XmlNode rootNode = DocCablelossXml.SelectSingleNode("CableLossConfig" + "/" + "CableLossDUT" + comboBoxDut.Text);
            XmlNodeList NodeCableloss = rootNode.ChildNodes;


            DocTestCaseXml = new XmlDocument();
            DocTestCaseXml.Load(FileTestCaseXml);
            string RootNameTestcase = DocTestCaseXml.DocumentElement.Name;
            XmlNodeList NodeTestcase = DocTestCaseXml.SelectSingleNode(RootNameTestcase + "/" + "TestCases").ChildNodes;
            bool checkResult = true;
            foreach (XmlNode r in NodeTestcase)
            {
                XmlAttributeCollection myAttrs = r.Attributes;
                if (myAttrs["paraName"] == null)
                {
                    continue;
                }
                string paraNameTestCase = myAttrs.GetNamedItem("paraName").Value;
                bool findstring = false;
                foreach(XmlNode m in NodeCableloss)
                {
                    string temp = null;
                    if (m.Attributes.GetNamedItem("paraName").Value.Contains("LTE") || m.Attributes.GetNamedItem("paraName").Value.Contains("MIMO") || m.Attributes.GetNamedItem("paraName").Value.Contains("DPDT"))
                    {
                        temp = m.Attributes.GetNamedItem("paraName").Value.Split(' ')[0] + ' ' + m.Attributes.GetNamedItem("paraName").Value.Split(' ')[1] + ' ' + m.Attributes.GetNamedItem("paraName").Value.Split(' ')[2];
                    }
                    else
                    {
                        temp = m.Attributes.GetNamedItem("paraName").Value;
                    }
                    if (paraNameTestCase.Contains(temp))
                    {
                        findstring = true;
                        break;//find
                    }
                    else
                    {
                        continue;
                    }
                }
               
                if (!findstring)
                {
                    checkResult = false;
                    Message.SelectionColor = Color.Red;
                    Message.Text += (string.Format("检测到{0}\\{1}目录下Cableloss.xml文件中没有对{2}设置线损\r\n", comboBoxProject.Text, comboBoxStation.Text, paraNameTestCase));
                }
            }
            if (checkResult)
            {
                Message.SelectionColor = Color.Green;
                Message.Text = (string.Format("检测到{0}\\{1}目录下Cableloss.xml与{2}_{3}.xml中的测试项一一对应成功", comboBoxProject.Text, comboBoxStation.Text, comboBoxProject.Text, comboBoxStation.Text));
            }
        }

        private void CreateCablelossItem_Click(object sender, EventArgs e)
        {
            string[] Stations = { "BT", "MT", "BTW", "MTW","MT2" };
            var CablossSet = new List<string>();
            if (!Stations.Contains(comboBoxStation.Text))
            {
                MessageBox.Show(string.Format("请选择正确的工位，工位名称应是BT,MT,BTW,MTW,MT2"));
                return;
            }
            string FileTestCaseXml = Application.StartupPath + "\\Project\\" + comboBoxProject.Text + "\\" + comboBoxStation.Text + "\\" + comboBoxProject.Text + "_" + comboBoxStation.Text + ".xml";

            string FileCablelossXml = Application.StartupPath + "\\Project\\" + comboBoxProject.Text + "\\" + comboBoxStation.Text + "\\Cableloss.xml";
            if (comboBoxStation.Text == "BT")
            {
                FileCablelossXml = Application.StartupPath + "\\Project\\" + comboBoxProject.Text + "\\" + "CL" + "\\Cableloss.xml";
            }
            if (!System.IO.File.Exists(FileTestCaseXml))
            {
                MessageBox.Show(string.Format("检测到在{0}\\{1}目录下没有{2}_{3}.xml文件", comboBoxProject.Text, comboBoxStation.Text, comboBoxProject.Text, comboBoxStation.Text));
                return;
            }
            //System.IO.File.Delete(FileCablelossXml);
            DocCablelossXml = new XmlDocument();
            XmlNode rootNode;
            if (!System.IO.File.Exists(FileCablelossXml))
            {
                XmlNode node = DocCablelossXml.CreateXmlDeclaration("1.0", "utf-8", "");
                DocCablelossXml.AppendChild(node);
                rootNode = DocCablelossXml.CreateElement("CableLossConfig");
                DocCablelossXml.AppendChild(rootNode);
            }
            else
            {
                DocCablelossXml.Load(FileCablelossXml);
                rootNode = DocCablelossXml.SelectSingleNode("CableLossConfig");
                if (DocCablelossXml.SelectSingleNode("CableLossConfig" + "/" + "CableLossDUT" + comboBoxDut.Text) != null)
                {
                    rootNode.RemoveChild(DocCablelossXml.SelectSingleNode("CableLossConfig" + "/" + "CableLossDUT" + comboBoxDut.Text));
                }
            }
            rootNode.AppendChild(DocCablelossXml.CreateElement("CableLossDUT" + comboBoxDut.Text));
            rootNode = DocCablelossXml.SelectSingleNode("CableLossConfig" + "/" + "CableLossDUT" + comboBoxDut.Text);

            DocTestCaseXml = new XmlDocument();
            DocTestCaseXml.Load(FileTestCaseXml);
            string RootNameTestcase = DocTestCaseXml.DocumentElement.Name;
            XmlNodeList NodeTestcase = DocTestCaseXml.SelectSingleNode(RootNameTestcase + "/" + "TestCases").ChildNodes;

           foreach (XmlNode r in NodeTestcase)
           {
               XmlAttributeCollection myAttrs = r.Attributes;
               foreach (XmlAttribute att in myAttrs)
               {
                   if (att.Name == "paraName") //find paraName
                   {
                       XmlElement xe1 = DocCablelossXml.CreateElement("cableloss");
                       List<string> paraName = new List<string>(att.Value.Split(' '));
                       if (att.Value.Contains("Check"))
                       {
                           continue;
                       }
                       double ULfreqMHz = 0.0;
                       double DLfreqMHz = 0.0;
                       try
                       {
                           ChannelToFrequency(paraName[0], paraName[1], Convert.ToDouble(paraName[2].Replace("CH", "")), ref ULfreqMHz, ref DLfreqMHz);
                       }
                       catch (System.Exception ex)
                       {
                           MessageBox.Show(string.Format("请检查在{0}\\{1}目录下的{2}_{3}.xml文件配置是否正确", comboBoxProject.Text, comboBoxStation.Text, comboBoxProject.Text, comboBoxStation.Text));
                           return;
                       }
                       if (att.Value.Contains("LTE") || att.Value.Contains("TDSCDMA") || att.Value.Contains("GMSK") || att.Value.Contains("C2K") || att.Value.Contains("WCDMA") || att.Value.Contains("DPDT") || att.Value.Contains("MIMO") || att.Value.Contains("WIFI"))
                       {
                           att.Value = att.Value.Split(' ')[0] + ' ' + att.Value.Split(' ')[1] +' ' + att.Value.Split(' ')[2];
                       }
                   
                       if (CablossSet.Contains(att.Value))
                       {
                           break;
                       }
                       CablossSet.Add(att.Value);
                       xe1.SetAttribute("paraName", att.Value);
                       xe1.SetAttribute("dULFreq", ULfreqMHz.ToString());
                       xe1.SetAttribute("dDLFreq", DLfreqMHz.ToString());
                       xe1.SetAttribute("paraLossMainTx", comboBoxCablelossInitValue.Text);
                       xe1.SetAttribute("paraLossMainRx", comboBoxCablelossInitValue.Text);
                       xe1.SetAttribute("paraLossSecTx", comboBoxCablelossInitValue.Text);
                       xe1.SetAttribute("paraLossSecRx", comboBoxCablelossInitValue.Text);
                       xe1.SetAttribute("paraLossTrdTx", comboBoxCablelossInitValue.Text);
                       xe1.SetAttribute("paraLossTrdRx", comboBoxCablelossInitValue.Text);
                       rootNode.AppendChild(xe1);
                   }
               }
           }
           DocCablelossXml.Save(FileCablelossXml);
           Message.SelectionColor = Color.Green;
           if (comboBoxStation.Text == "BT")
           {
               Message.Text = (string.Format("在{0}\\{1}目录下生成DUT{2}的Cableloss.xml文件成功", comboBoxProject.Text, "CL", comboBoxDut.Text));
           }
           else
           {
               Message.Text = (string.Format("在{0}\\{1}目录下生成DUT{2}的Cableloss.xml文件成功", comboBoxProject.Text, comboBoxStation.Text, comboBoxDut.Text));
           }
        }

        private void Create_CableLoss_Script_Click(object sender, EventArgs e)
        {
            string _currentXml = Application.StartupPath + "\\Project\\" + comboBoxProject.Text + "\\" + "BT" + "\\" + comboBoxProject.Text + "_" + "BT" + ".xml";
            if (!System.IO.File.Exists(_currentXml))
            {
                Message.SelectionColor = Color.Red;
                Message.Text = (string.Format("检测到{0}\\BT目录下没有XML配置文件\r\n", comboBoxProject.Text));
                return;
            }
            
            string _CablossXml = Application.StartupPath + "\\Project\\" + comboBoxProject.Text + "\\" + "CL" + "\\" + comboBoxProject.Text + "_" + "CL" + ".xml";
            if (System.IO.File.Exists(_CablossXml))
            {
                System.IO.File.Delete(_CablossXml);
            }
            File.Copy(_currentXml, _CablossXml, true);//每次都重新复制一个？

            XmlDocument DocRsceCableloss = new XmlDocument();
            DocRsceCableloss.Load(_CablossXml);
            XmlNode TestCasesNode = DocRsceCableloss.SelectSingleNode(DocRsceCableloss.DocumentElement.Name + "/" + "TestCases");

            //main testcase
            XmlNodeList myNodes = TestCasesNode.ChildNodes;
            for (int i = myNodes.Count - 1; i >= 0; i--)
            {
                XmlAttributeCollection myAttrs = myNodes[i].Attributes;
                if (myAttrs["Category"] != null && myAttrs["Category"].Value.ToUpper().Contains("DPDT"))
                {
                    myNodes[i].ParentNode.RemoveChild(myNodes[i]);
                    continue;
                }

                if ((myNodes[i].Name.Contains("SuitTDSCDMA")) 
                    || (myNodes[i].Name.Contains("SuitLTE")) 
                    || (myNodes[i].Name.Contains("SuitWCDMA")) 
                    || (myNodes[i].Name.Contains("SuitGSM")) 
                    || (myNodes[i].Name.Contains("SuitC2K")))
                {
                    if (myAttrs["paraCablelossLimit"] == null)
                    {
                        myNodes[i].ParentNode.RemoveChild(myNodes[i]);
                        continue;
                    }

                    //new node
                    XmlElement elem = DocRsceCableloss.CreateElement(myNodes[i].Name + "CABLELOSS");

                    //add all attributes
                    foreach (XmlAttribute att in myAttrs)
                    {
                        elem.SetAttribute(att.Name, att.Value);
                    }

                    //modify attributes
                    elem.Attributes["paraLimit"].Value = elem.Attributes["paraCablelossLimit"].Value;
                    elem.RemoveAttribute("paraCablelossLimit");

                    //replace node
                    myNodes[i].ParentNode.ReplaceChild(elem, myNodes[i]);
                }
                else if (myNodes[i].Name == "TestSFCCheck"
                    || myNodes[i].Name == "TestReadOemData"
                    || myNodes[i].Name == "TestCheckRamRom"
                    || myNodes[i].Name == "TestCheckRomVersion")
                {
                    myNodes[i].Attributes["paraChecked"].Value = "FALSE";
                }
            }

            XmlElement xe = (XmlElement)DocRsceCableloss.SelectSingleNode(DocRsceCableloss.DocumentElement.Name + "/ExitCases/TestDisconnectDut");
            xe.SetAttribute("paraWriteOem", "FALSE");

            //dpdt testcase
            for (int i = 0; i < myNodes.Count; i++)
            {
                XmlAttributeCollection myAttrs = myNodes[i].Attributes;
                if (myAttrs["Category"] != null && myAttrs["Category"].Value.ToUpper().Contains("DPDT"))
                {
                    continue;
                }

                if (myAttrs["Category"] == null || myAttrs["Category"].Value == ""
                    || (myAttrs["Category"].Value.ToUpper().Contains("TDSCDMA")
                    && myAttrs["Category"].Value.ToUpper().Contains("LTE")
                    && myAttrs["Category"].Value.ToUpper().Contains("WCDMA")
                    && myAttrs["Category"].Value.ToUpper().Contains("GSM")
                    && myAttrs["Category"].Value.ToUpper().Contains("C2K")))
                {
                    continue;
                }

                //new node
                XmlElement elem = DocRsceCableloss.CreateElement(myNodes[i].Name);

                //add all attributes
                foreach (XmlAttribute att in myAttrs)
                {
                    elem.SetAttribute(att.Name, att.Value);
                }

                //modify attributes
                if (myNodes[i].Name.Contains("InitializeBSE") || myNodes[i].Name.Contains("Stop"))
                {
                    //paraAntMode="DPDT" paraAntSwitch="TRUE"
                    elem.SetAttribute("paraAntSwitch", "TRUE");
                    elem.SetAttribute("paraName", myNodes[i].Name);
                }
                elem.SetAttribute("paraAntMode", "DPDT");
                elem.Attributes["paraName"].Value += " DPDT";
                elem.Attributes["Category"].Value += "-DPDT";

                if (elem.HasAttribute("paraLimit"))
                {
                    string[] strLimitAry = elem.Attributes["paraLimit"].Value.Split(';');
                    elem.Attributes["paraLimit"].Value = "";
                    myNodes[i].Attributes["paraLimit"].Value = "";
                    foreach (string strlimit in strLimitAry)
                    {
                        if (!strlimit.ToUpper().Contains("MAIN"))
                        {
                            elem.Attributes["paraLimit"].Value += strlimit + ";";
                        }
                        else
                        {
                            myNodes[i].Attributes["paraLimit"].Value += strlimit + ";";
                        }
                    }
                }

                //add node
                TestCasesNode.AppendChild(elem);
            }

            XmlElement eQCOMCableloss = DocRsceCableloss.CreateElement("TestCreatQCOMCableloss");
            eQCOMCableloss.SetAttribute("paraChecked", "TRUE");
            eQCOMCableloss.SetAttribute("Description", "");
            TestCasesNode.AppendChild(eQCOMCableloss);
            
            DocRsceCableloss.Save(_CablossXml);

            Message.SelectionColor = Color.Green;
            Message.Text = (string.Format("在目录{0}\\CL下生成校线损XML通过\r\n", comboBoxProject.Text));
            return ;
        }
        private void Clear_QCOM_Cableloss_Click(object sender, EventArgs e)
        {
            string QCOM_CablossXml = Application.StartupPath + "\\Project\\" + comboBoxProject.Text + "\\" + "CL" + "\\" + "Databases\\"  + "CalDB_NET.xml";
            XmlDocument DocRsce = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(QCOM_CablossXml, settings);
            DocRsce.Load(reader);
            string RootName = DocRsce.DocumentElement.Name;
            XmlNode RootNode = DocRsce.SelectSingleNode("CalibrationData");
            if (RootNode == null)
            {
                Message.SelectionColor = Color.Red;
                Message.Text = (string.Format("在目录{0}\\CL\\Databases下CalDB_NET.xml线损表异常\r\n", comboBoxProject.Text));
                return ;
            }
            string[] CalName = { "WCDMA", "LTE", "TDSCDMA", "GSM", "CDMA" };
            foreach (var temp in CalName)
            {
                foreach (XmlNode CalConfig in RootNode.ChildNodes)
                {
                    if (CalConfig.Attributes["name"].Value == temp)
                    {
                        foreach (XmlNode calPath in CalConfig.ChildNodes)
                        {
                            string value = calPath.Attributes["number"] != null ? calPath.Attributes["number"].Value : "200";
                            if (calPath.Attributes["number"] != null)
                            {
                                if (value == "5" || value == "4" || value == "14")//UL
                                {
                                    for (int i = calPath.ChildNodes.Count - 1 ; i >= 0 ; i--)
                                    {
                                        calPath.RemoveChild(calPath.ChildNodes[i]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            reader.Close();
            DocRsce.Save(QCOM_CablossXml);
            Message.SelectionColor = Color.Green;
            Message.Text = (string.Format("在目录{0}\\CL\\Databases下CalDB_NET.xml线损表已被清空\r\n", comboBoxProject.Text));
            return ;
        }
        public bool ChannelToFrequency(string sMode, string sBand, double ULChannel, ref double ULfreqMHz, ref double DLfreqMHz)
        {
            if (sBand.Contains("Band"))
            {
               sBand = sBand.Replace("Band", "");
            }
            sBand = sBand[sBand.Length - 1] == 'A' ? sBand.Substring(0, sBand.Length - 1) : sBand;
            sBand = sBand[sBand.Length - 1] == 'B' ? sBand.Substring(0, sBand.Length - 1) : sBand;
            if (sMode == "")
            {
                return false;
            }

            else if (sMode.ToUpper() == "WIFI")
            {

                if (sBand == "2.4G")
                {
                    ULfreqMHz = 2412 + (ULChannel - 1) * 5;
                    DLfreqMHz = ULfreqMHz;
                    return true;
                }
                else if (sBand == "5G")
                {
                    if (ULChannel >= 183 && ULChannel <= 196)
                    {
                        ULfreqMHz = 4915 + (ULChannel - 183) * 5;
                    }
                    else if ((ULChannel >= 7 && ULChannel <= 165))
                    {
                        ULfreqMHz = 5035 + (ULChannel - 7) * 5;
                    }
                    DLfreqMHz = ULfreqMHz;
                    return true;
                }
            }
            else if (sMode.ToUpper() == "BT")
            {
                ULfreqMHz = ULChannel + 2402;
                DLfreqMHz = ULfreqMHz;
                return true;
            }
            else if (sMode == "Gps")
            {
                ULfreqMHz = DLfreqMHz = ULChannel;
            }
            else
            {
                byte iBand;
                if (sBand == "GSM900")
                {
                    iBand = 1;
                }
                else if (sBand == "GSM850")
                {
                    iBand = 4;
                }
                else if (sBand == "DCS1800")
                {
                    iBand = 3;
                }
                else if (sBand == "PCS1900")
                {
                    iBand = 2;
                }

                else
                {
                    iBand = byte.Parse(sBand);
                }
                if (sMode == "TDSCDMA")
                {
                    TDSCDMAChannelToFrequency(iBand, Convert.ToInt32(ULChannel), ref ULfreqMHz, ref DLfreqMHz);
                }
                else if (sMode == "WCDMA")
                {
                    WcdmaChannelToFrequency(iBand, Convert.ToInt32(ULChannel), ref ULfreqMHz, ref DLfreqMHz);
                }
                else if (sMode == "LTE")
                {
                    LTEChannelToFrequency(iBand, Convert.ToInt32(ULChannel), ref ULfreqMHz, ref DLfreqMHz);
                }
                else if (sMode == "C2K")
                {
                    CdmaChannelToFrequency(iBand, Convert.ToInt32(ULChannel), ref ULfreqMHz, ref DLfreqMHz);
                }
                else if (sMode == "GMSK")
                {
                    GsmChannelToFrequency(iBand, Convert.ToInt32(ULChannel), ref ULfreqMHz, ref DLfreqMHz);
                }
            }

            return true;
        }
        protected bool WcdmaChannelToFrequency(int Band, double ULChannel, ref double ULfreqMHz, ref double DLfreqMHz)
        {
            double freq_current_MHz;
            double freq_current_MHz_DL;
            if (Band == 1)
            {
                if (ULChannel >= 9612 && ULChannel <= 9888)
                    freq_current_MHz = ULChannel / 5.0;
                else
                    return false;
                //9612 to 9888 1922.4~1977.6
                freq_current_MHz_DL = freq_current_MHz + 190;
            }
            else if (Band == 2)
            {
                if (ULChannel >= 9262 && ULChannel <= 9538)
                    freq_current_MHz = ULChannel / 5.0;
                else if (ULChannel == 12 || ULChannel == 37 || ULChannel == 62 || ULChannel == 87 || ULChannel == 112
                    || ULChannel == 137 || ULChannel == 162 || ULChannel == 187 || ULChannel == 212 || ULChannel == 237
                    || ULChannel == 262 || ULChannel == 287)
                    freq_current_MHz = ULChannel / 5.0 + 1850.1;
                else
                    return false;
                /*9 262 to 9 538 and 12, 37, 62, 87, 112, 137, 162, 187, 212, 237, 262, 287*/
                /*1852.4 to 1907.6 and 1852.5, 1857.5, 1862.5, 1867.5, 1872.5, 1877.5, 1882.5, 1887.5, 1892.5, 1897.5, 1902.5, 1907.5*/
                freq_current_MHz_DL = freq_current_MHz + 80;
            }
            else if (Band == 3)
            {
                if (ULChannel >= 937 && ULChannel <= 1288)
                    freq_current_MHz = ULChannel / 5.0 + 1525; //937 to 1288 , 1712.4~1782.6
                else return false;
                freq_current_MHz_DL = freq_current_MHz + 95;
            }
            else if (Band == 4)
            {
                if (ULChannel >= 1312 && ULChannel <= 1513)
                    freq_current_MHz = ULChannel / 5.0 + 1450;
                else if (ULChannel == 1662 || ULChannel == 1687 || ULChannel == 1687 || ULChannel == 1737
                    || ULChannel == 1762 || ULChannel == 1787 || ULChannel == 1812 || ULChannel == 1837 || ULChannel == 1862)
                    freq_current_MHz = ULChannel / 5.0 + 1380.1;
                else return false;
                freq_current_MHz_DL = freq_current_MHz + 400;

                //1312 to 1513 and 1662, 1687, 1712, 1737, 1762, 1787, 1812, 1837, 1862
                //1712.4~1752.6 and 1712.5, 1717.5, 1722.5, 1727.5, 1732.5, 1737.5 1742.5, 1747.5, 1752.5
            }
            else if (Band == 5)
            {
                if (ULChannel >= 4132 && ULChannel <= 4233)
                    freq_current_MHz = ULChannel / 5.0;
                else if (ULChannel == 782 || ULChannel == 787 || ULChannel == 807 || ULChannel == 812 || ULChannel == 837 || ULChannel == 862)
                    freq_current_MHz = ULChannel / 5.0 + 670.1;
                else return false;
                //4132 to 4233 and 782, 787, 807, 812, 837, 862
                //826.4~846.6 ans 670.1	826.5, 827.5, 831.5, 832.5, 837.5, 842.5
                freq_current_MHz_DL = freq_current_MHz + 45;
            }
            else if (Band == 6)
            {
                if (ULChannel >= 4162 && ULChannel <= 4188)
                    freq_current_MHz = ULChannel / 5.0;
                else if (ULChannel == 812 || ULChannel == 837)
                    freq_current_MHz = ULChannel / 5.0 + 670.1;
                else return false;
                //4162 to 4188 and 812, 837
                //832.4~837.6 and 832.5, 837.5
                freq_current_MHz_DL = freq_current_MHz + 45;
            }
            else if (Band == 7)
            {
                if (ULChannel >= 2012 && ULChannel <= 2338)
                    freq_current_MHz = ULChannel / 5.0 + 2100;
                else if (ULChannel == 2362 || ULChannel == 2387 || ULChannel == 2412 || ULChannel == 2437 || ULChannel == 2462 || ULChannel == 2487
                    || ULChannel == 2512 || ULChannel == 2537 || ULChannel == 2562 || ULChannel == 2587 || ULChannel == 2612
                    || ULChannel == 2637 || ULChannel == 2662 || ULChannel == 2687)
                    freq_current_MHz = ULChannel / 5.0 + 2030.1;
                else return false;
                //2012 to 2338 and 2362, 2387, 2412, 2437, 2462, 2487, 2512, 2537, 2562, 2587, 2612, 2637, 2662, 2687 
                //2502.4~2567.6 and 2502.5, 2507.5, 2512.5, 2517.5, 2522.5, 2527.5, 2532.5, 2537.5, 2542.5, 2547.5, 2552.5, 2557.5, 2562.5, 2567.5
                freq_current_MHz_DL = freq_current_MHz + 120;
            }
            else if (Band == 8)
            {
                if (ULChannel >= 2712 && ULChannel <= 2863)
                    freq_current_MHz = ULChannel / 5.0 + 340;
                else return false;
                //2712 to 2863, 882.4~912.6
                freq_current_MHz_DL = freq_current_MHz + 45;
            }
            else if (Band == 9)
            {
                if (ULChannel >= 8762 && ULChannel <= 8912)
                    freq_current_MHz = ULChannel / 5.0;
                else return false;
                //8762 to 8912 , 1752.4	1782.4
                freq_current_MHz_DL = freq_current_MHz + 95;
            }
            else if (Band == 10)
            {
                if (ULChannel >= 2887 && ULChannel <= 3163)
                    freq_current_MHz = ULChannel / 5.0 + 1135;
                else if (ULChannel == 3187 || ULChannel == 3212 || ULChannel == 3237 || ULChannel == 3262 || ULChannel == 3287 || ULChannel == 3312
                    || ULChannel == 3337 || ULChannel == 3362 || ULChannel == 3387 || ULChannel == 3412 || ULChannel == 3437 || ULChannel == 3462)
                    freq_current_MHz = ULChannel / 5.0 + 1075.1;
                else return false;
                //2887 to 3163 and 3187, 3212, 3237, 3262, 3287, 3312, 3337, 3362, 3387, 3412, 3437, 3462
                //1712.4~1767.6 and 1712.5, 1717.5, 1722.5, 1727.5, 1732.5, 1737.5, 1742.5, 1747.5, 1752.5, 1757.5, 1762.5, 1767.5
                freq_current_MHz_DL = freq_current_MHz + 400;

            }
            else if (Band == 11)
            {
                if (ULChannel >= 3487 && ULChannel <= 3587)
                    freq_current_MHz = ULChannel / 5.0 + 733;
                else return false;
                //3487 to 3587 , 1430.4~1450.4
                freq_current_MHz_DL = freq_current_MHz + 48;
            }
            else
                return false;

            ULfreqMHz = freq_current_MHz;
            DLfreqMHz = freq_current_MHz_DL;
            return true;
        }

        protected bool LTEChannelToFrequency(int Band, int ULChannel, ref double ULfreqMHz, ref double DLfreqMHz)
        {
            double dUlStartFreq = 0.0;
            const double dChannelSpacing = 0.1;
            double dInterval = 0.0;
            int iChannelOffset = 0;

            if (Band == 1)
            {
                dUlStartFreq = 1920.0;
                dInterval = 190.0;
                iChannelOffset = 18000;
            }
            else if (Band == 2)
            {
                dUlStartFreq = 1850.0;
                dInterval = 80.0;
                iChannelOffset = 18600;
            }
            else if (Band == 3)
            {
                dUlStartFreq = 1710.0;
                dInterval = 95.0;
                iChannelOffset = 19200;
            }
            else if (Band == 4)
            {
                dUlStartFreq = 1710.0;
                dInterval = 400.0;
                iChannelOffset = 19950;
            }
            else if (Band == 5)
            {
                dUlStartFreq = 824.0;
                dInterval = 45.0;
                iChannelOffset = 20400;
            }
            else if (Band == 7)
            {
                dUlStartFreq = 2500.0;
                dInterval = 120.0;
                iChannelOffset = 20750;
            }
            else if (Band == 8)
            {
                dUlStartFreq = 880.0;
                dInterval = 45.0;
                iChannelOffset = 21450;
            }
            else if (Band == 11)
            {
                dUlStartFreq = 1427.9;
                dInterval = 48.0;
                iChannelOffset = 22750;
            }
            else if (Band == 12)
            {
                dUlStartFreq = 698.0;
                dInterval = 30.0;
                iChannelOffset = 23000;
            }
            else if (Band == 13)
            {
                dUlStartFreq = 777.0;
                dInterval = -31.0;
                iChannelOffset = 23180;
            }
            else if (Band == 17)
            {
                dUlStartFreq = 704.0;
                dInterval = 30.0;
                iChannelOffset = 23730;
            }
            else if (Band == 18)
            {
                dUlStartFreq = 815.0;
                dInterval = 45.0;
                iChannelOffset = 23850;
            }
            else if (Band == 19)
            {
                dUlStartFreq = 830;
                dInterval = 45.0;
                iChannelOffset = 24000;
            }
            else if (Band == 20)
            {
                dUlStartFreq = 832.0;
                dInterval = -41.0;
                iChannelOffset = 24150;
            }
            else if (Band == 25)
            {
                dUlStartFreq = 1850.0;
                dInterval = 80.0;
                iChannelOffset = 26040;
            }
            else if (Band == 26)
            {
                dUlStartFreq = 814.0;
                dInterval = 45.0;
                iChannelOffset = 26690;
            }
            else if (Band == 28)  //Jerry  20181024
            {
                dUlStartFreq = 703.0;
                dInterval = 55.0;
                iChannelOffset = 27210;
            }
            else if (Band == 29)
            {
                /***** Begin Augus.hu 2016-11-23 modify *****/
                //Band 29只在CA时候使用，而且3GPP协议只定义了DLFrequency，没有ULFrequency
                //和射频Sean.yang确认使用Band 30的ULFrequency做同步
                //ULfreqMHz = 717.0;
                dUlStartFreq = 717.0;
                dInterval = 0.0;
                iChannelOffset = 9660;
                /***** End Augus.hu 2016-11-23 modify *****/
            }
            else if (Band == 30)  //16/8/2 andy.zhang
            {
                dUlStartFreq = 2305.0;
                dInterval = 45.0;
                iChannelOffset = 27660;
            }
            else if (Band == 33)
            {
                dUlStartFreq = 1900.0;
                dInterval = 0.0;
                iChannelOffset = 36000;
            }
            else if (Band == 34)
            {
                dUlStartFreq = 2010.0;
                dInterval = 0.0;
                iChannelOffset = 36200;
            }
            else if (Band == 38)
            {
                dUlStartFreq = 2570.0;
                dInterval = 0.0;
                iChannelOffset = 37750;
            }
            else if (Band == 39)
            {
                dUlStartFreq = 1880.0;
                dInterval = 0.0;
                iChannelOffset = 38250;
            }
            else if (Band == 40)
            {
                dUlStartFreq = 2300.0;
                dInterval = 0.0;
                iChannelOffset = 38650;
            }
            else if (Band == 41)
            {
                dUlStartFreq = 2496.0;
                dInterval = 0.0;
                iChannelOffset = 39650;
            }
            else
            {
                return false;
            }

            /***** Begin Augus.hu 2016-11-23 modify *****/
            //Band 29只在CA时候使用，而且3GPP协议只定义了DLFrequency，没有ULFrequency
            //和射频Sean.yang确认使用Band 30的ULFrequency做同步
            if (Band == 29)
            {
                DLfreqMHz = (ULChannel - iChannelOffset) * dChannelSpacing + dUlStartFreq;
                ULfreqMHz = 2310.0;
            }
            else
            {
                ULfreqMHz = (ULChannel - iChannelOffset) * dChannelSpacing + dUlStartFreq;
                DLfreqMHz = ULfreqMHz + dInterval;
            }
            /***** End Augus.hu 2016-11-23 modify *****/
            return true;
        }

        protected bool TDSCDMAChannelToFrequency(int Band, int ULChannel, ref double ULfreqMHz, ref double DLfreqMHz)
        {

            ULfreqMHz = ULChannel / 5.0;
            DLfreqMHz = ULfreqMHz;

            return true;
        }

        //this is not correct
        protected bool GSMChannelToFrequency(int Band, int ULChan, ref double ULfreqMHz, ref double DLfreqMHz)
        {
            if (Band == 1)//EGSM
            {
                ULfreqMHz = 890.0 + ULChan * 0.2;
                DLfreqMHz = ULfreqMHz + 45.0;
                //ULfreqMHz = 685.2 + ULChan * 0.2;
                //DLfreqMHz = ULfreqMHz + 45.0;
            }
            else if (Band == 0)//PGSM
            {
                ULfreqMHz = 685.2 + ULChan * 0.2;
                DLfreqMHz = ULfreqMHz + 45.0;
                //ULfreqMHz = 890.0 + ULChan * 0.2;
                //DLfreqMHz = ULfreqMHz + 45.0;
            }
            else if (Band == 4)//GSM850
            {
                ULfreqMHz = 824.2 + ULChan * 0.2;
                //ULfreqMHz = 798.6 + ULChan * 0.2;
                DLfreqMHz = ULfreqMHz + 45.0;
            }
            else if (Band == 3)//DCS1800
            {
                ULfreqMHz = 1607.8 + ULChan * 0.2;
                // ULfreqMHz = 1710.0 + ULChan * 0.2;
                DLfreqMHz = ULfreqMHz + 95.0;
            }
            else if (Band == 2)//PCS1900
            {
                ULfreqMHz = 1850.2 + ULChan * 0.2;
                //ULfreqMHz = 1747.8 + ULChan * 0.2;
                DLfreqMHz = ULfreqMHz + 80.0;
            }
            return true;
        }

        protected bool WcdmaUlChannelToDlChannel(int Band, int ULChannel, ref int DLChannel)
        {
            decimal freq_current_MHz;
            decimal freq_current_MHz_DL;
            if (Band == 1)
            {
                if (ULChannel >= 9612 && ULChannel <= 9888)
                    freq_current_MHz = ULChannel / 5.0M;
                else
                    return false;
                //9612 to 9888 1922.4~1977.6
                freq_current_MHz_DL = freq_current_MHz + 190M;

                DLChannel = (int)(5M * freq_current_MHz_DL);
            }
            else if (Band == 2)
            {
                if (ULChannel >= 9262 && ULChannel <= 9538)
                {
                    freq_current_MHz = ULChannel / 5.0M;
                    freq_current_MHz_DL = freq_current_MHz + 80M;
                    DLChannel = (int)(5M * freq_current_MHz_DL);
                }
                else if (ULChannel == 12 || ULChannel == 37 || ULChannel == 62 || ULChannel == 87 || ULChannel == 112
                    || ULChannel == 137 || ULChannel == 162 || ULChannel == 187 || ULChannel == 212 || ULChannel == 237
                    || ULChannel == 262 || ULChannel == 287)
                {
                    freq_current_MHz = ULChannel / 5.0M + 1850.1M;
                    freq_current_MHz_DL = freq_current_MHz + 80M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 1850.1M));
                }
                else
                    return false;
                /*9 262 to 9 538 and 12, 37, 62, 87, 112, 137, 162, 187, 212, 237, 262, 287*/
                /*1852.4 to 1907.6 and 1852.5, 1857.5, 1862.5, 1867.5, 1872.5, 1877.5, 1882.5, 1887.5, 1892.5, 1897.5, 1902.5, 1907.5*/


            }
            else if (Band == 3)
            {
                if (ULChannel >= 937 && ULChannel <= 1288)
                    freq_current_MHz = ULChannel / 5.0M + 1525M; //937 to 1288 , 1712.4~1782.6
                else return false;
                freq_current_MHz_DL = freq_current_MHz + 95M;
                DLChannel = (int)(5M * (freq_current_MHz_DL - 1575M));

            }
            else if (Band == 4)
            {
                if (ULChannel >= 1312 && ULChannel <= 1513)
                {
                    freq_current_MHz = ULChannel / 5.0M + 1450M;
                    freq_current_MHz_DL = freq_current_MHz + 400M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 1805M));
                }
                else if (ULChannel == 1662 || ULChannel == 1687 || ULChannel == 1687 || ULChannel == 1737
                    || ULChannel == 1762 || ULChannel == 1787 || ULChannel == 1812 || ULChannel == 1837 || ULChannel == 1862)
                {
                    freq_current_MHz = ULChannel / 5.0M + 1380.1M;
                    freq_current_MHz_DL = freq_current_MHz + 400M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 1735.1M));
                }
                else return false;


                //1312 to 1513 and 1662, 1687, 1712, 1737, 1762, 1787, 1812, 1837, 1862
                //1712.4~1752.6 and 1712.5, 1717.5, 1722.5, 1727.5, 1732.5, 1737.5 1742.5, 1747.5, 1752.5
            }
            else if (Band == 5)
            {
                if (ULChannel >= 4132 && ULChannel <= 4233)
                {
                    freq_current_MHz = ULChannel / 5.0M;
                    freq_current_MHz_DL = freq_current_MHz + 45M;
                    DLChannel = (int)(5M * freq_current_MHz_DL);

                }
                else if (ULChannel == 782 || ULChannel == 787 || ULChannel == 807 || ULChannel == 812 || ULChannel == 837 || ULChannel == 862)
                {
                    freq_current_MHz = ULChannel / 5.0M + 670.1M;
                    freq_current_MHz_DL = freq_current_MHz + 45M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 670.1M));
                }
                else return false;
                //4132 to 4233 and 782, 787, 807, 812, 837, 862
                //826.4~846.6 ans 670.1	826.5, 827.5, 831.5, 832.5, 837.5, 842.5

            }
            else if (Band == 6)
            {
                if (ULChannel >= 4162 && ULChannel <= 4188)
                {
                    freq_current_MHz = ULChannel / 5.0M;
                    freq_current_MHz_DL = freq_current_MHz + 45M;
                    DLChannel = (int)(5M * freq_current_MHz_DL);
                }
                else if (ULChannel == 812 || ULChannel == 837)
                {
                    freq_current_MHz = ULChannel / 5.0M + 670.1M;
                    freq_current_MHz_DL = freq_current_MHz + 45M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 670.1M));
                }
                else return false;
                //4162 to 4188 and 812, 837
                //832.4~837.6 and 832.5, 837.5
            }
            else if (Band == 7)
            {
                if (ULChannel >= 2012 && ULChannel <= 2338)
                {
                    freq_current_MHz = ULChannel / 5.0M + 2100M;
                    freq_current_MHz_DL = freq_current_MHz + 120M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 2175M));
                }
                else if (ULChannel == 2362 || ULChannel == 2387 || ULChannel == 2412 || ULChannel == 2437 || ULChannel == 2462 || ULChannel == 2487
                    || ULChannel == 2512 || ULChannel == 2537 || ULChannel == 2562 || ULChannel == 2587 || ULChannel == 2612
                    || ULChannel == 2637 || ULChannel == 2662 || ULChannel == 2687)
                {
                    freq_current_MHz = ULChannel / 5.0M + 2030.1M;
                    freq_current_MHz_DL = freq_current_MHz + 120M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 2105.1M));
                }
                else return false;
                //2012 to 2338 and 2362, 2387, 2412, 2437, 2462, 2487, 2512, 2537, 2562, 2587, 2612, 2637, 2662, 2687 
                //2502.4~2567.6 and 2502.5, 2507.5, 2512.5, 2517.5, 2522.5, 2527.5, 2532.5, 2537.5, 2542.5, 2547.5, 2552.5, 2557.5, 2562.5, 2567.5

            }
            else if (Band == 8)
            {
                if (ULChannel >= 2712 && ULChannel <= 2863)
                    freq_current_MHz = ULChannel / 5.0M + 340M;
                else return false;
                //2712 to 2863, 882.4~912.6
                freq_current_MHz_DL = freq_current_MHz + 45M;
                DLChannel = (int)(5M * (freq_current_MHz_DL - 340M));
            }
            else if (Band == 9)
            {
                if (ULChannel >= 8762 && ULChannel <= 8912)
                    freq_current_MHz = ULChannel / 5.0M;
                else return false;
                //8762 to 8912 , 1752.4	1782.4
                freq_current_MHz_DL = freq_current_MHz + 95M;
                DLChannel = (int)(5M * freq_current_MHz_DL);
            }
            else if (Band == 10)
            {
                if (ULChannel >= 2887 && ULChannel <= 3163)
                {
                    freq_current_MHz = ULChannel / 5.0M + 1135M;
                    freq_current_MHz_DL = freq_current_MHz + 400M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 1490M));
                }
                else if (ULChannel == 3187 || ULChannel == 3212 || ULChannel == 3237 || ULChannel == 3262 || ULChannel == 3287 || ULChannel == 3312
                    || ULChannel == 3337 || ULChannel == 3362 || ULChannel == 3387 || ULChannel == 3412 || ULChannel == 3437 || ULChannel == 3462)
                {
                    freq_current_MHz = ULChannel / 5.0M + 1075.1M;
                    freq_current_MHz_DL = freq_current_MHz + 400M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 1430.1M));
                }
                else return false;
                //2887 to 3163 and 3187, 3212, 3237, 3262, 3287, 3312, 3337, 3362, 3387, 3412, 3437, 3462
                //1712.4~1767.6 and 1712.5, 1717.5, 1722.5, 1727.5, 1732.5, 1737.5, 1742.5, 1747.5, 1752.5, 1757.5, 1762.5, 1767.5             

            }
            else if (Band == 11)
            {
                if (ULChannel >= 3487 && ULChannel <= 3587)
                    freq_current_MHz = ULChannel / 5.0M + 733M;
                else return false;
                //3487 to 3587 , 1430.4~1450.4
                freq_current_MHz_DL = freq_current_MHz + 48M;
                DLChannel = (int)(5M * (freq_current_MHz_DL - 736M));
            }
            else
                return false;

            return true;
        }

        protected bool LTEUlChannelToDlChannel(int Band, int ULChannel, ref int DLChannel)
        {
            if (Band < 33 && Band != 30 && Band != 29)//andy.zhang /8/5
            {
                DLChannel = ULChannel - 18000;
            }
            else if (Band == 30)//andy.zhang /8/5
            {
                DLChannel = ULChannel - 17890;
            }
            else
            {
                DLChannel = ULChannel;
            }
            return true;
        }

        protected bool TDSCDMAUlChannelToDlChannel(int Band, int ULChannel, ref int DLChannel)
        {
            DLChannel = ULChannel;
            return true;
        }

        protected bool GsmChannelToFrequency(int Band, int chan, ref double ULfreqMHz, ref double DLfreqMHz)
        {

            switch (Band)
            {
                case 4: // GSM 850
                    if (chan < 128 || chan > 251) return false;
                    break;
                case 1:	   //EGSM
                    if (chan < 0 || (chan > 124 && chan < 975) || chan > 1023) return false;
                    break;
                case 3:		//DCS
                    if (chan < 512 || chan > 885) return false;
                    break;
                case 2:		//PCS
                    if (chan < 512 || chan > 810) return false;
                    break;

            }

            switch (Band)
            {
                case 4:  // GSM850
                    DLfreqMHz = Math.Round(824.2 + 0.2 * (chan - 128) + 45.0, 1);
                    ULfreqMHz = Math.Round(824.2 + 0.2 * (chan - 128), 1);
                    break;
                case 1:  // EGSM
                    if (chan >= 975 && chan <= 1023)
                    {
                        DLfreqMHz = Math.Round(880.2 + 0.2 * (chan - 975) + 45.0, 1);
                        ULfreqMHz = Math.Round(880.2 + 0.2 * (chan - 975), 1);
                    }
                    else
                    {
                        DLfreqMHz = Math.Round(890.0 + 0.2 * chan + 45.0, 1);
                        ULfreqMHz = Math.Round(890.000000 + 0.200000 * chan, 1);
                    }
                    break;
                case 3:  //DCS
                    DLfreqMHz = Math.Round(1710.2 + 0.2 * (chan - 512) + 95.0, 1);
                    ULfreqMHz = Math.Round(1710.2 + 0.2 * (chan - 512), 1);
                    break;
                case 2:  //PCS
                    //DLfreqMHz = Math.Round(1850.2 + 0.2 * chan  + 80.0, 1);
                    //ULfreqMHz = Math.Round(1850.2 + 0.2 * chan , 1);
                    DLfreqMHz = Math.Round(1850.2 + 0.2 * (chan - 512) + 80.0, 1);
                    ULfreqMHz = Math.Round(1850.2 + 0.2 * (chan - 512), 1);
                    break;

            }
            return true;
        }

        protected bool CdmaChannelToFrequency(int Band, int chan, ref double ULfreqMHz, ref double DLfreqMHz)
        {

            switch (Band)
            {
                case 0:	  //BC0 800M
                    if (chan < 1 || (chan > 799 && chan < 991) || chan > 1023)
                        return false;
                    break;
                case 1:	   //BC1 1900M
                    if (chan < 0 || chan > 1199)
                        return false;
                    break;
                case 6:       //BC6 IMT
                    if (chan < 0 || chan > 1199)
                        return false;
                    break;
                case 15:      //BC15
                    if (chan < 0 || chan > 899)
                        return false;
                    break;

                default:
                    return false;
            }


            switch (Band)
            {
                case 0:  //BC0 800M
                    if (chan >= 991 && chan <= 1023)
                    {
                        DLfreqMHz = (double)(825000 + 30 * (chan - 1023) + 45000) / 1000;
                        ULfreqMHz = (double)(825000 + 30 * (chan - 1023)) / 1000;
                    }
                    else
                    {
                        DLfreqMHz = (double)(825000 + 30 * chan + 45000) / 1000;
                        ULfreqMHz = (double)(825000 + 30 * chan) / 1000;
                    }
                    break;
                case 1:  //BC1 1900M
                    if (chan >= 0 && chan <= 1199)
                    {
                        DLfreqMHz = (double)(1850000 + 50 * chan + 80000) / 1000;
                        ULfreqMHz = (double)(1850000 + 50 * chan) / 1000;
                    }
                    break;
                case 6:  //BC6 IMT
                    if (chan >= 0 && chan <= 1199)
                    {
                        DLfreqMHz = (double)(2110000 + 50 * chan) / 1000;
                        ULfreqMHz = (double)(1920000 + 50 * chan) / 1000;
                    }
                    break;
                case 15:  //BC15
                    if (chan >= 0 && chan <= 899)
                    {
                        DLfreqMHz = (double)(2110000 + 50 * chan) / 1000;
                        ULfreqMHz = (double)(1710000 + 50 * chan) / 1000;
                    }
                    break;

                default:
                    return false;
            }
            return true;
        }

        protected bool CdmaUlChannelToDlChannel(int Band, int ULChannel, ref int DLChannel)
        {
            DLChannel = ULChannel;
            return true;
        }

        // using System.Security.Cryptography;
        public string GetMd5Hash(String input)
        {
            if (input == null)
            {
                return null;
            }
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private void CreateMD5_Click(object sender, EventArgs e)
        {
            try
            {
                txb_msg.Text = "";
                Dictionary<string, string> dMD5 = new Dictionary<string, string>();
                string ProjectDirPath = Application.StartupPath + "\\Project\\" + comboBoxProject.Text;
                string MD5FilePath = ProjectDirPath + "\\ConfigMD5.txt";
                FileStream fs;

                foreach (string _station in Directory.GetFileSystemEntries(ProjectDirPath))// 遍历所有的文件和目录
                {
                    if (!Directory.Exists(_station))
                    {
                        continue;
                    }
                    string stationName = _station.Substring(_station.LastIndexOf("\\") + 1);
                    string xmlFile = comboBoxProject.Text + "_" + stationName + ".xml";

                    string xmlFilePath = _station + "\\" + comboBoxProject.Text + "_" + stationName + ".xml";
                    XElement XmlItem = null;
                    XElement products = XElement.Load(xmlFilePath);
                    IEnumerable<XElement> query = products.XPathSelectElements("/TestCases");
                    foreach (XElement item in query)
                    {
                        XmlItem = item;
                    }
                    if (XmlItem == null)
                    {
                        return;
                    }
                    string strXml = XmlItem.ToString();
                    string XmlMD5 = GetMd5Hash(strXml);
                    dMD5.Add(xmlFile, XmlMD5);
                }
                if (!File.Exists(MD5FilePath))
                {
                    fs = new FileStream(MD5FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Close();
                }
                Dictionary<string, string>.Enumerator d = new Dictionary<string, string>.Enumerator();

                string strWriter = "";
                d = dMD5.GetEnumerator();
                for (int Index = 0; Index < dMD5.Count; Index++)
                {
                    d.MoveNext();
                    strWriter += d.Current.Key + "," + d.Current.Value + "\r\n";
                }
                File.WriteAllText(MD5FilePath, strWriter);
                txb_msg.Text = "create md5 file suc";    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }


    public class IniFileCls
    {
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        private string _filePath;

        public IniFileCls(string filePath)
        {
            _filePath = filePath;
        }

        public bool WriteToFile(string section, string key, string val)
        {
            return WritePrivateProfileString(section, key, val, _filePath);
        }

        public string ReadFromFile(string section, string key, string def)
        {
            string res = "";
            Byte[] Buffer = new Byte[1024];
            int bufLen = GetPrivateProfileString(section, key, def, Buffer, 1024, _filePath);
            res = Encoding.GetEncoding(0).GetString(Buffer);
            res = res.Substring(0, bufLen).Trim();
            return res;
        }
    }
}
