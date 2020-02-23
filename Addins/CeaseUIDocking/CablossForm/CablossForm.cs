using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Cease.Addins.Log;

namespace CeaseUI
{
    public partial class CablossForm : Form
    {
        protected InterfaceLog log;
        protected Dictionary<string, string> m_dicPara;
        protected List<CablossList> m_listCablosslist;
        protected List<DataGridView> m_listDataGrodView;

        public CablossForm(Dictionary<string, string> _dicPara, InterfaceLog _log)
        {
            InitializeComponent();

            m_dicPara = _dicPara;
            log = _log;

            m_listDataGrodView = new List<DataGridView>() { dataGridView1, dataGridView2, dataGridView3, dataGridView4 };
        }

        private void CablossForm_Load(object sender, EventArgs e)
        {
            try
            {
                string cablelossFile = "\\Cableloss.xml";
                string cablossType = m_dicPara.ContainsKey("paraCablossType") ? m_dicPara["paraCablossType"] : "BLACKSHARK";
                if (cablossType.ToUpper() == "QCOM" )
                {
                    cablelossFile = "\\Databases\\CalDB_NET.xml";
                }
                String myPath = Application.StartupPath + "\\Project\\" + m_dicPara["Project"] + "\\" + m_dicPara["Station"] + cablelossFile;
                if (!System.IO.File.Exists(myPath))
                {
                    MessageBox.Show(myPath + " is not existed!");
                    return;
                }

                //m_dicPara["TotalDut"] = "1";
                m_listCablosslist = new List<CablossList>();
                for (int i = 0; i < int.Parse(m_dicPara["TotalDut"]); i++ )
                {
                    CablossList _list = new CablossList(i + 1, m_listDataGrodView[i]);
                    if (!_list.LoadXml(myPath))
                    {
                        MessageBox.Show(string.Format("Dut{0} load xml fail!", i + 1));
                        return;
                    }

                    m_listCablosslist.Add(_list);
                }
            }
            catch (System.Exception ex)
            {
                log.err("CablossForm_Load Fail!", ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            foreach (var item in m_listCablosslist)
            {
                item.SaveToXml();
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            foreach (var item in m_listCablosslist)
            {
                item.SaveToXml();
            }

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Close();
        }
    }

    public class CablossItem
    {
        public string ChannelName;
        public string ULFreq;
        public string DLFreq;

        public string lossTx1;
        public string lossRx1;
        public string lossTx2;
        public string lossRx2;
        public string lossTx3;
        public string lossRx3;

        public bool ParseCablossItem(XmlNode _node)
        {
            XmlAttributeCollection myAttrs = _node.Attributes;
            foreach (XmlAttribute myAtt in myAttrs)
            {
                switch (myAtt.Name)
                {
                    case "paraName":
                        ChannelName = myAtt.Value;
                        break;
                    case "dULFreq":
                        ULFreq = myAtt.Value;
                        break;
                    case "dDLFreq":
                        DLFreq = myAtt.Value;
                        break;
                    case "paraLossMainTx":
                        lossTx1 = myAtt.Value;
                        break;
                    case "paraLossMainRx":
                        lossRx1 = myAtt.Value;
                        break;
                    case "paraLossSecTx":
                        lossTx2 = myAtt.Value;
                        break;
                    case "paraLossSecRx":
                        lossRx2 = myAtt.Value;
                        break;
                    case "paraLossTrdTx":
                        lossTx3 = myAtt.Value;
                        break;
                    case "paraLossTrdRx":
                        lossRx3 = myAtt.Value;
                        break;
                    default:
                        break;
                }
            }

            return true;
        }

        public string[] ToStringArray()
        {
            string[] _arry = { ChannelName, ULFreq, DLFreq, lossTx1, lossRx1, lossTx2, lossRx2, lossTx3, lossRx3 };
            return _arry;
        }
    }

    public class CablossList : List<CablossItem>
    {
        private int m_iDut;
        private DataGridView m_ListView;
        private string m_strXml;

        /// <summary>
        /// XML文档资源
        /// </summary>
        private XmlDocument DocRsce;

        /// <summary>
        /// 根节点名称
        /// </summary>
        public String RootName;

        public XmlNode RootNode;

        public CablossList(int _iDut, DataGridView _list)
        {
            this.m_iDut = _iDut;
            this.m_ListView = _list;
        }
        public bool LoadXmlQcom(string _strXml)
        {
            m_strXml = _strXml;
            DocRsce = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(m_strXml, settings);
            DocRsce.Load(reader);
            RootName = DocRsce.DocumentElement.Name;
            RootNode = DocRsce.SelectSingleNode("CalibrationData");
            if (RootNode == null)
            {
                return false;
            }
            string[] CalName = { "WCDMA", "LTE", "GSM", "CDMA","TDSCDMA" };
            foreach (var temp in CalName)
            {
                var cablossItems = new List<CablossItem>();
                foreach (XmlNode CalConfig in RootNode.ChildNodes)
                {
                    if (CalConfig.Attributes["name"].Value == temp)
                    {
                        int cablossLength = 0;
                        bool bIndex5 = false;
                        bool bIndex4 = false;
                        bool bIndex14 = false;
                        foreach (XmlNode calPath in CalConfig.ChildNodes)
                        {
                            if (calPath.Attributes["number"].Value == "5")//UL
                            {
                                if (cablossLength < calPath.ChildNodes.Count)
                                {
                                    for (int i = cablossLength; i < calPath.ChildNodes.Count; i++)
                                    {
                                        var cablossItem = new CablossItem();
                                        cablossItems.Add(cablossItem);
                                    }
                                    cablossLength = calPath.ChildNodes.Count;
                                }
                                for (int i = 0; i < calPath.ChildNodes.Count; i++)
                                {
                                    cablossItems[i].ChannelName = temp;
                                    cablossItems[i].ULFreq = calPath.ChildNodes[i].Attributes["freq"].Value;
                                    cablossItems[i].lossTx1 = calPath.ChildNodes[i].Attributes["loss"].Value;
                                }
                                bIndex5 = true;
                            }
                            if (calPath.Attributes["number"].Value == "4")//DL
                            {
                                if (cablossLength < calPath.ChildNodes.Count)
                                {
                                    for (int i = cablossLength; i < calPath.ChildNodes.Count; i++)
                                    {
                                        var cablossItem = new CablossItem();
                                        cablossItems.Add(cablossItem);
                                    }
                                    cablossLength = calPath.ChildNodes.Count;
                                }
                                for (int i = 0; i < calPath.ChildNodes.Count; i++)
                                {
                                    cablossItems[i].ChannelName = temp;
                                    cablossItems[i].DLFreq = calPath.ChildNodes[i].Attributes["freq"].Value;
                                    cablossItems[i].lossRx1 = calPath.ChildNodes[i].Attributes["loss"].Value;
                                }
                                bIndex4 = true;
                            }
                            if (calPath.Attributes["number"].Value == "14")//SecRx
                            {
                                if (cablossLength < calPath.ChildNodes.Count)
                                {
                                    for (int i = cablossLength; i < calPath.ChildNodes.Count; i++)
                                    {
                                        var cablossItem = new CablossItem();
                                        cablossItems.Add(cablossItem);
                                    }
                                    cablossLength = calPath.ChildNodes.Count;
                                }
                                for (int i = 0; i < calPath.ChildNodes.Count; i++)
                                {
                                    cablossItems[i].ChannelName = temp;
                                    cablossItems[i].lossTx2 = calPath.ChildNodes[i].Attributes["freq"].Value;
                                    cablossItems[i].lossRx2 = calPath.ChildNodes[i].Attributes["loss"].Value;
                                }
                                bIndex14 = true;
                            }
                            if (bIndex4 & bIndex5 & bIndex14)
                            {
                                for (int i = 0; i < cablossItems.Count; i++)
                                {
                                    DataGridViewRow rowItems = new DataGridViewRow();
                                    foreach (var dta in cablossItems[i].ToStringArray())
                                    {
                                        DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                                        cell.Value = dta;
                                        rowItems.Cells.Add(cell);
                                    }
                                    m_ListView.Rows.Add(rowItems);
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            reader.Close();
            DocRsce = null;
            return true;
        }
        public bool LoadXmlBlackShark(string _strXml)
        {
            m_strXml = _strXml;
            DocRsce = new XmlDocument();
            DocRsce.Load(_strXml);
            RootName = DocRsce.DocumentElement.Name;

            RootNode = DocRsce.SelectSingleNode(RootName + "/" + "CableLossDUT" + m_iDut.ToString());
            if (RootNode == null)
            {
                return false;
            }

            foreach (XmlNode item in RootNode.ChildNodes)
            {
                var cablossItem = new CablossItem();
                if (!cablossItem.ParseCablossItem(item))
                {
                    return false;
                }

                DataGridViewRow rowItems = new DataGridViewRow();
                foreach (var dta in cablossItem.ToStringArray())
                {
                    DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                    cell.Value = dta;
                    rowItems.Cells.Add(cell);
                }
                m_ListView.Rows.Add(rowItems);
            }

            return true;
        }
        public bool LoadXml(string _strXml)
        {
            if (_strXml.Contains("Cableloss.xml"))
            {
                LoadXmlBlackShark(_strXml);
            }
            else if (_strXml.Contains("CalDB_NET.xml"))
            {
                m_ListView.Columns[0].HeaderText = "Standard";
                m_ListView.Columns[5].HeaderText = "AuxRxFreq(MHz)";
                m_ListView.Columns[5].ReadOnly = true;
                LoadXmlQcom(_strXml);
            }
            return true;
        }

        public bool SaveToXml()
        {
            if (m_strXml.Contains("Cableloss.xml"))
            {
                SaveToXmlBlackShark();
            }
            else if (m_strXml.Contains("CalDB_NET.xml"))
            {
                SaveToXmlQcom();
            }
            return true;
        }
        public bool SaveToXmlBlackShark()
        {
            List<string> listAttrs = new List<string>() { "paraName", "dULFreq", "dDLFreq", "paraLossMainTx", "paraLossMainRx", "paraLossSecTx", "paraLossSecRx", "paraLossTrdTx", "paraLossTrdRx" };
            //保存时重新加载解决每个dut都保存导致的修改没有同步的问题
            DocRsce.Load(m_strXml);
            RootNode = DocRsce.SelectSingleNode(RootName + "/" + "CableLossDUT" + m_iDut.ToString());
            RootNode.RemoveAll();// 删除旗下所有节点
            int row = m_ListView.Rows.Count;// 得到总行数      
            for (int i = 0; i < row - 1; i++)// 遍历这个 dataGridView  
            {
                XmlElement xmlElement_cableloss = DocRsce.CreateElement("cableloss");// 创建一个 <cableloss> 节点  
                for (int j = 0; j < listAttrs.Count; j++)
                {
                    xmlElement_cableloss.SetAttribute(listAttrs[j], m_ListView.Rows[i].Cells[j].EditedFormattedValue.ToString());
                }

                RootNode.AppendChild(xmlElement_cableloss);
            }
            DocRsce.Save(m_strXml);
            return true;
        }
        public bool SaveToXmlQcom()
        {
            List<string> listAttrs = new List<string>() { "paraName", "dULFreq", "dDLFreq", "paraLossMainTx", "paraLossMainRx", "paraLossSecTx", "paraLossSecRx"};
            DocRsce = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(m_strXml, settings);
            DocRsce.Load(reader);
            RootNode = DocRsce.SelectSingleNode("CalibrationData");
            int row = m_ListView.Rows.Count;//   
            for (int i = 0; i < row - 1; i++)//  
            {
                foreach (XmlNode CalConfig in RootNode.ChildNodes)
                {
                    if (CalConfig.Attributes["name"].Value == m_ListView.Rows[i].Cells[0].EditedFormattedValue.ToString()) //find TDSCDMA
                    {
                        bool bIndex5 = false;
                        bool bIndex4 = false;
                        bool bIndex14 = false;
                        foreach (XmlNode calPath in CalConfig.ChildNodes)
                        {
                            if (calPath.Attributes["number"].Value == "5")//UL
                            {
                                bIndex5 = true;
                                foreach (XmlNode calPoint in calPath.ChildNodes)
                                {
                                    if (calPoint.Attributes["freq"].Value == m_ListView.Rows[i].Cells[1].EditedFormattedValue.ToString())
                                    {
                                        calPoint.Attributes["loss"].Value = m_ListView.Rows[i].Cells[3].EditedFormattedValue.ToString();
                                        break;
                                    }
                                }
                            }

                            if (calPath.Attributes["number"].Value == "4")//DL
                            {
                                bIndex4 = true;
                                foreach (XmlNode calPoint in calPath.ChildNodes)
                                {
                                    if (calPoint.Attributes["freq"].Value == m_ListView.Rows[i].Cells[2].EditedFormattedValue.ToString())
                                    {
                                        calPoint.Attributes["loss"].Value = m_ListView.Rows[i].Cells[4].EditedFormattedValue.ToString();
                                        break;
                                    }
                                }
                            }
                            if (calPath.Attributes["number"].Value == "14")//DL
                            {
                                bIndex14 = true;
                                foreach (XmlNode calPoint in calPath.ChildNodes)
                                {
                                    if (calPoint.Attributes["freq"].Value == m_ListView.Rows[i].Cells[5].EditedFormattedValue.ToString())
                                    {
                                        calPoint.Attributes["loss"].Value = m_ListView.Rows[i].Cells[6].EditedFormattedValue.ToString();
                                        break;
                                    }
                                }
                            }
                            if (bIndex5 & bIndex4 & bIndex14)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            reader.Close();
            DocRsce.Save(m_strXml);
            DocRsce = null;
            return true;
        }
    }
}
