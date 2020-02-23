using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using CeaseUI.Docking;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace CeaseUI
{
    public partial class AboutDialog : Form
    {
        protected string m_strPath;
        protected string m_strProject;
        protected string m_strMainVersion;
        protected string m_strXmlVersion;

        protected List<string> m_listAddinsName;
        protected List<System.Windows.Forms.Label> m_LabelList;

        public AboutDialog(string _project, string _addinsName, string _mainVer, string _xmlVer)
        {
            InitializeComponent();

            m_strPath = System.Windows.Forms.Application.StartupPath;
            m_strProject = _project;
            m_strMainVersion = _mainVer;
            m_strXmlVersion = _xmlVer;
            m_listAddinsName = new List<string>(_addinsName.Split(','));
            m_LabelList = new List<System.Windows.Forms.Label>() { labelFramework, labelCeaseStartup };
        }

        private void AboutDialog_Load(object sender, EventArgs e)
        {
            InitLabelVersion();
            InitListVersion();
        }

        private void InitLabelVersion()
        {
            string[] dllArray = { "CEASE.dll", "Cease.Startup.exe" };
            string[] temp = { "\\Interfaces\\", "\\" };
            for (int i = 0; i < m_LabelList.Count; i++)
            {
                FileVersionInfo AddinsVersionInfo = FileVersionInfo.GetVersionInfo(m_strPath + temp[i] + dllArray[i]);
                m_LabelList[i].Text += "\t" + AddinsVersionInfo.FileVersion;
            }

            labelMajorVersion.Text += m_strMainVersion;
            labelStationVer.Text += m_strXmlVersion;
        }

        private void InitListVersion()
        {
            //addins & TestStore
            FileVersionInfo AddinsVersionInfo = FileVersionInfo.GetVersionInfo(m_strPath + "\\TestUnits\\Cease.TestStore.dll");
            ListViewItem item = new ListViewItem((AddinsVersionInfo.OriginalFilename + "," + AddinsVersionInfo.FileVersion).Split(','));
            listViewAddins.Items.Add(item);

            foreach (var _name in m_listAddinsName)
            {
                AddinsVersionInfo = FileVersionInfo.GetVersionInfo(m_strPath + "\\Addins\\" + _name + "\\" + _name + ".dll");
                item = new ListViewItem((AddinsVersionInfo.OriginalFilename + "," + AddinsVersionInfo.FileVersion).Split(','));
                listViewAddins.Items.Add(item);
            }

            //station xml version

        }

        private bool GetStationVersion(string _strFileName, ref string _ver)
        {
            _ver = "";
            if (!System.IO.File.Exists(_strFileName))
            {
                return false;
            }
            var DocRsce = new XmlDocument();
            DocRsce.Load(_strFileName);

            if (DocRsce.ChildNodes.Count < 2)
            {
                return false;
            }

            XmlAttributeCollection myAttrs = DocRsce.FirstChild.NextSibling.Attributes;
            if (myAttrs == null)
            {
                return false;
            }

            foreach (XmlAttribute myAtt in myAttrs)
            {
                if (myAtt.Name == "Ver")
                {
                    _ver = myAtt.Value;
                    return true;
                }
            }

            return false;
        }

        private bool SetStationVersion(string _strFileName, string _ver)
        {
           
            if (!System.IO.File.Exists(_strFileName))
            {
                return false;
            }
            var DocRsce = new XmlDocument();
            DocRsce.Load(_strFileName);

            if (DocRsce.ChildNodes.Count < 2)
            {
                return false;
            }

            XmlAttributeCollection myAttrs = DocRsce.FirstChild.NextSibling.Attributes;
            if (myAttrs == null)
            {
                return false;
            }

            foreach (XmlAttribute myAtt in myAttrs)
            {
                if (myAtt.Name == "Ver")
                {
                   myAtt.Value = _ver ;
                   DocRsce.Save(_strFileName);
                    return true;
                }
            }

            return false;
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            string strFileName = m_strPath + "\\Ver" + m_strMainVersion + ".xls";

            if (File.Exists(strFileName))
            {
                File.Delete(strFileName);
            }
        }

        private void alter_Click(object sender, EventArgs e)
        {
            string strFileName = m_strPath + "\\Ver" + m_strMainVersion + ".xls";

            if (File.Exists(strFileName))
            {
                File.Delete(strFileName);
            }
        }
    }
}