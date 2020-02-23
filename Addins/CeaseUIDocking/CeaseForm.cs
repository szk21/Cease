using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CeaseUI.Docking;
using CeaseUI.Customization;
using CeaseUI;

using System.ComponentModel.Composition;
using System.Net;
using System.Net.Sockets;

using Cease.Addins.UI;
using Cease.Addins.Log;

using System.Diagnostics;

namespace CeaseUI
{
    [Export(typeof(InterfaceUI))]
    [ExportMetadata("AddinName", "CeaseUI")]
    public partial class CeaseForm : Form, InterfaceUI
    {
        private string m_strCeaseMainVersion = "2.72";//CeaseMainVersion
        protected List<InterfaceLog> m_LogList = new List<InterfaceLog>();
        protected bool m_bTestListInitialEn = true;
        protected Dictionary<string, string> m_dicPara;

        private string[] TestStationStat;
        private int[] TestCount;
        private int[] m_iTestTimeS;
        private bool[] m_bTestTimeEn;
        private string[] m_strScanCode;
        private TimeSpan[] TestStartTime;
        private TimeSpan[] TestEndTime;

        private static readonly CeaseForm instance = new CeaseForm();

        private bool m_bSaveLayout = true;
        private DeserializeDockContent m_deserializeDockContent;
        private DummySolutionExplorer m_solutionExplorer = new DummySolutionExplorer();
        private TestInformationWindow m_TestInforWindow = new TestInformationWindow();
        private ControlPanel m_ctrlPanel = new ControlPanel();
        private OutputWindow m_outputWindow = new OutputWindow();
        private TestItemList m_taskList = new TestItemList();

        public CeaseForm()
        {
            InitializeComponent();

            m_solutionExplorer = new DummySolutionExplorer();
            m_solutionExplorer.RightToLeftLayout = RightToLeftLayout;
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            TestStationStat = new string[8];
            TestCount = new int[8];
            TestStartTime = new TimeSpan[8];
            TestEndTime = new TimeSpan[8]; 
            m_iTestTimeS = new int[8];
            m_bTestTimeEn = new bool[8];
            m_strScanCode = new string[8];
            for (int i = 0; i < TestStationStat.Length; i++)
            {
                TestStationStat[i] = "FAIL_ENV";
                TestCount[i] = 0;
                m_iTestTimeS[i] = 0;
                m_bTestTimeEn[i] = false;
                m_strScanCode[i] = "";
            }

            dockPanel.AllowEndUserDocking = false;
            
            //event
            ButtonClicked += SetDutStatus;
            ButtonClicked += m_taskList.ResetTestResultDisp;
            ButtonClicked += m_outputWindow.InitialOutputList;
            ButtonClicked += m_ctrlPanel.LockDutTestBtn;
            ButtonClicked += m_TestInforWindow.UpdateTestResultDisp;
            ButtonClicked += EnableDutTimer;

            ButtonInitEnd += SetDutStatus;
            ButtonInitEnd += DisableDutTimer;
            ButtonInitEnd += m_ctrlPanel.LockDutTestBtn;
            ButtonInitEnd += m_TestInforWindow.UpdateTestResultDisp;
            ButtonInitEnd += m_ctrlPanel.DisplayStartButton;
            
            ButtonStartEnd += SetDutStatus;
            ButtonStartEnd += DisableDutTimer;
            ButtonStartEnd += UpdateTestResultStatistics;
            ButtonStartEnd += m_ctrlPanel.LockDutTestBtn;
            ButtonStartEnd += m_TestInforWindow.UpdateTestResultDisp;
            ButtonStartEnd += UpdateCycleTestStat;

            ConfigSaved += m_ctrlPanel.LockDutTestBtn;
            ConfigSaved += UnlockTestListInitial;

            CeaseFormInitial += m_TestInforWindow.InitialProductInforDisp;
            CeaseFormInitial += m_taskList.InitialTestForm;
            CeaseFormInitial += m_ctrlPanel.InitialControlPanel;
            CeaseFormInitial += InitialTimer;

            ListFormInitial += m_taskList.InitialTestItemsList;
            ListFormInitial += LockTestListInitial; // must be last
        }

        private event EventHandler ButtonInitEnd; // Initial process end event
        private event EventHandler ButtonStartEnd; // Start process end event

        private event EventHandler ButtonClicked; //声明事件
        private event EventHandler ConfigSaved;
        private event EventHandler CeaseFormInitial;
        private event EventHandler ListFormInitial;

        protected virtual void OnButtonClicked(ButtonClickedEventArgs e)
        {
            TestStartTime[e.m_iDut] = new TimeSpan(DateTime.Now.Ticks);
            if (instance.ButtonClicked != null)
            { // 如果有对象注册
                instance.ButtonClicked(this, e);  // 调用所有注册对象的方法
            }
        }

        protected virtual void OnButtonInitEnd(ButtonInitEndEventArgs e)
        {
            if (instance.ButtonInitEnd != null)
            { // 如果有对象注册
                instance.ButtonInitEnd(this, e);  // 调用所有注册对象的方法
            }
        }

        protected virtual void OnButtonStartEnd(ButtonStartEndEventArgs e)
        {
            if (instance.ButtonStartEnd != null)
            { // 如果有对象注册
                instance.ButtonStartEnd(this, e);  // 调用所有注册对象的方法
            }
        }

        protected virtual void OnConfigSaved(ButtonClickedEventArgs e)
        {
            if (instance.ConfigSaved != null)
            { // 如果有对象注册
                instance.ConfigSaved(this, e);  // 调用所有注册对象的方法
            }
        }

        protected virtual void OnCeaseFormInitial(CeaseFormInitialEventArgs e)
        {
            if (instance.CeaseFormInitial != null)
            { // 如果有对象注册
                instance.CeaseFormInitial(this, e);  // 调用所有注册对象的方法
            }
        }

        protected virtual void OnListFormInitial(ListFormInitialEventArgs e)
        {
            if (instance.ListFormInitial != null && instance.m_bTestListInitialEn)
            { // 如果有对象注册
                instance.ListFormInitial(this, e);  // 调用所有注册对象的方法
            }
        }

        //AT
        private CurvesForm m_curve = new CurvesForm();

        #region Methods

        private IDockContent FindDocument(string text)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    if (form.Text == text)
                        return form as IDockContent;

                return null;
            }
            else
            {
                foreach (IDockContent content in dockPanel.Documents)
                    if (content.DockHandler.TabText == text)
                        return content;

                return null;
            }
        }

        private DummyDoc CreateNewDocument()
        {
            DummyDoc dummyDoc = new DummyDoc();

            int count = 1;
            //string text = "C:\\MADFDKAJ\\ADAKFJASD\\ADFKDSAKFJASD\\ASDFKASDFJASDF\\ASDFIJADSFJ\\ASDFKDFDA" + count.ToString();
            string text = "Document" + count.ToString();
            while (FindDocument(text) != null)
            {
                count++;
                //text = "C:\\MADFDKAJ\\ADAKFJASD\\ADFKDSAKFJASD\\ASDFKASDFJASDF\\ASDFIJADSFJ\\ASDFKDFDA" + count.ToString();
                text = "Document" + count.ToString();
            }
            dummyDoc.Text = text;
            return dummyDoc;
        }

        private DummyDoc CreateNewDocument(string text)
        {
            DummyDoc dummyDoc = new DummyDoc();
            dummyDoc.Text = text;
            return dummyDoc;
        }

        private void CloseAllDocuments()
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    form.Close();
            }
            else
            {
                for (int index = dockPanel.Contents.Count - 1; index >= 0; index--)
                {
                    if (dockPanel.Contents[index] is IDockContent)
                    {
                        IDockContent content = (IDockContent)dockPanel.Contents[index];
                        content.DockHandler.Close();
                    }
                }
            }
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(DummySolutionExplorer).ToString())
                return m_solutionExplorer;
            else if (persistString == typeof(TestInformationWindow).ToString())
                return m_TestInforWindow;
            else if (persistString == typeof(ControlPanel).ToString())
                return m_ctrlPanel;
            else if (persistString == typeof(OutputWindow).ToString())
                return m_outputWindow;
            else if (persistString == typeof(TestItemList).ToString())
                return m_taskList;
            else if (persistString == typeof(CurvesForm).ToString())
                return m_curve;
            else
            {
                // DummyDoc overrides GetPersistString to add extra information into persistString.
                // Any DockContent may override this value to add any needed information for deserialization.

                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;

                if (parsedStrings[0] != typeof(DummyDoc).ToString())
                    return null;

                DummyDoc dummyDoc = new DummyDoc();
                if (parsedStrings[1] != string.Empty)
                    dummyDoc.FileName = parsedStrings[1];
                if (parsedStrings[2] != string.Empty)
                    dummyDoc.Text = parsedStrings[2];

                return dummyDoc;
            }
        }

        private void CloseAllContents()
        {
            // we don't want to create another instance of tool window, set DockPanel to null
            m_solutionExplorer.DockPanel = null;
            m_TestInforWindow.DockPanel = null;
            m_ctrlPanel.DockPanel = null;
            m_outputWindow.DockPanel = null;
            m_taskList.DockPanel = null;
            m_curve.DockPanel = null;

            // Close all other document windows
            CloseAllDocuments();
        }

        private void SetSchema(object sender, System.EventArgs e)
        {
            CloseAllContents();

            //if (sender == menuItemSchemaVS2005)
            //    Extender.SetSchema(dockPanel, Extender.Schema.VS2005);
            //else if (sender == menuItemSchemaVS2003)
            //    Extender.SetSchema(dockPanel, Extender.Schema.VS2003);

            //menuItemSchemaVS2005.Checked = (sender == menuItemSchemaVS2005);
            //menuItemSchemaVS2003.Checked = (sender == menuItemSchemaVS2003);
        }

        private void SetDocumentStyle(object sender, System.EventArgs e)
        {
            DocumentStyle oldStyle = dockPanel.DocumentStyle;
            DocumentStyle newStyle;
            //if (sender == menuItemDockingMdi)
            //    newStyle = DocumentStyle.DockingMdi;
            //else if (sender == menuItemDockingWindow)
            //    newStyle = DocumentStyle.DockingWindow;
            //else if (sender == menuItemDockingSdi)
            //    newStyle = DocumentStyle.DockingSdi;
            //else
                newStyle = DocumentStyle.SystemMdi;

            if (oldStyle == newStyle)
                return;

            if (oldStyle == DocumentStyle.SystemMdi || newStyle == DocumentStyle.SystemMdi)
                CloseAllDocuments();

            dockPanel.DocumentStyle = newStyle;
            //menuItemDockingMdi.Checked = (newStyle == DocumentStyle.DockingMdi);
            //menuItemDockingWindow.Checked = (newStyle == DocumentStyle.DockingWindow);
            //menuItemDockingSdi.Checked = (newStyle == DocumentStyle.DockingSdi);
            //menuItemSystemMdi.Checked = (newStyle == DocumentStyle.SystemMdi);
            menuItemLayoutByCode.Enabled = (newStyle != DocumentStyle.SystemMdi);
            menuItemLayoutByXml.Enabled = (newStyle != DocumentStyle.SystemMdi);
            toolBarButtonLayoutByCode.Enabled = (newStyle != DocumentStyle.SystemMdi);
            toolBarButtonLayoutByXml.Enabled = (newStyle != DocumentStyle.SystemMdi);
        }

        private void SetDockPanelSkinOptions(bool isChecked)
        {
            if (isChecked)
            {
                // All of these options may be set in the designer.
                // This is not a complete list of possible options available in the skin.

                AutoHideStripSkin autoHideSkin = new AutoHideStripSkin();
                autoHideSkin.DockStripGradient.StartColor = Color.AliceBlue;
                autoHideSkin.DockStripGradient.EndColor = Color.Blue;
                autoHideSkin.DockStripGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                autoHideSkin.TabGradient.StartColor = SystemColors.Control;
                autoHideSkin.TabGradient.EndColor = SystemColors.ControlDark;
                autoHideSkin.TabGradient.TextColor = SystemColors.ControlText;
                autoHideSkin.TextFont = new Font("Showcard Gothic", 10);

                dockPanel.Skin.AutoHideStripSkin = autoHideSkin;

                DockPaneStripSkin dockPaneSkin = new DockPaneStripSkin();
                dockPaneSkin.DocumentGradient.DockStripGradient.StartColor = Color.Red;
                dockPaneSkin.DocumentGradient.DockStripGradient.EndColor = Color.Pink;

                dockPaneSkin.DocumentGradient.ActiveTabGradient.StartColor = Color.Green;
                dockPaneSkin.DocumentGradient.ActiveTabGradient.EndColor = Color.Green;
                dockPaneSkin.DocumentGradient.ActiveTabGradient.TextColor = Color.White;

                dockPaneSkin.DocumentGradient.InactiveTabGradient.StartColor = Color.Gray;
                dockPaneSkin.DocumentGradient.InactiveTabGradient.EndColor = Color.Gray;
                dockPaneSkin.DocumentGradient.InactiveTabGradient.TextColor = Color.Black;

                dockPaneSkin.TextFont = new Font("SketchFlow Print", 10);

                dockPanel.Skin.DockPaneStripSkin = dockPaneSkin;
            }
            else
            {
                dockPanel.Skin = new DockPanelSkin();
            }

            menuItemLayoutByXml_Click(menuItemLayoutByXml, EventArgs.Empty);
        }

        public void SetScanCode(int _iDut, string _strCode)
        {
            m_strScanCode[_iDut] = _strCode;
        }

        public void AutoScrolltoLog(int _iDut, int itemIdx)
        {
            m_outputWindow.AutoScrolltoLog(_iDut, itemIdx);
        }

        #endregion

        #region Event Handlers

        private void menuItemExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void menuItemSolutionExplorer_Click(object sender, System.EventArgs e)
        {
            m_solutionExplorer.Show(dockPanel);
        }

        private void menuItemPropertyWindow_Click(object sender, System.EventArgs e)
        {
            m_TestInforWindow.Show(dockPanel);
        }

        private void menuItemToolbox_Click(object sender, System.EventArgs e)
        {
            m_ctrlPanel.Show(dockPanel);
        }

        private void menuItemOutputWindow_Click(object sender, System.EventArgs e)
        {
            m_outputWindow.Show(dockPanel);
        }

        private void menuItemTaskList_Click(object sender, System.EventArgs e)
        {
            m_taskList.Show(dockPanel);
        }

        private void menuItemErrorMsg_Click(object sender, EventArgs e)
        {
        }

        private void menuItemAbout_Click(object sender, System.EventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog(instance.m_dicPara["Project"], instance.m_dicPara["Addins"], m_strCeaseMainVersion, instance.m_dicPara["StationVer"]);
            aboutDialog.ShowDialog(this);
        }

        private void menuItemNew_Click(object sender, System.EventArgs e)
        {
            DummyDoc dummyDoc = CreateNewDocument();
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                dummyDoc.MdiParent = this;
                dummyDoc.Show();
            }
            else
                dummyDoc.Show(dockPanel);
        }

        private void menuItemOpen_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.InitialDirectory = Application.ExecutablePath;
            openFile.Filter = "rtf files (*.rtf)|*.rtf|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFile.FilterIndex = 1;
            openFile.RestoreDirectory = true;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string fullName = openFile.FileName;
                string fileName = Path.GetFileName(fullName);

                if (FindDocument(fileName) != null)
                {
                    MessageBox.Show("The document: " + fileName + " has already opened!");
                    return;
                }

                DummyDoc dummyDoc = new DummyDoc();
                dummyDoc.Text = fileName;
                if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                {
                    dummyDoc.MdiParent = this;
                    dummyDoc.Show();
                }
                else
                    dummyDoc.Show(dockPanel);
                try
                {
                    dummyDoc.FileName = fullName;
                }
                catch (Exception exception)
                {
                    dummyDoc.Close();
                    MessageBox.Show(exception.Message);
                }

            }
        }

        private void menuItemFile_Popup(object sender, System.EventArgs e)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                menuItemClose.Enabled = menuItemCloseAll.Enabled = (ActiveMdiChild != null);
            }
            else
            {
                menuItemClose.Enabled = (dockPanel.ActiveDocument != null);
                menuItemCloseAll.Enabled = (dockPanel.DocumentsCount > 0);
            }
        }

        private void menuItemClose_Click(object sender, System.EventArgs e)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                ActiveMdiChild.Close();
            else if (dockPanel.ActiveDocument != null)
                dockPanel.ActiveDocument.DockHandler.Close();
        }

        private void menuItemCloseAll_Click(object sender, System.EventArgs e)
        {
            CloseAllDocuments();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");

            if (File.Exists(configFile))
                dockPanel.LoadFromXml(configFile, m_deserializeDockContent);

            foreach (var _log in instance.m_LogList)
            {
                _log.Logged += instance.m_outputWindow.AddMsg;
            }

            instance.Text = "Cease " + m_strCeaseMainVersion + "." + instance.m_dicPara["StationVer"];

            instance.OnCeaseFormInitial(new CeaseFormInitialEventArgs(instance.m_dicPara, instance));
            instance.OnListFormInitial(new ListFormInitialEventArgs(instance.m_dicPara));
            SetMESStatus(instance.m_dicPara["Station"] + "   " + instance.m_dicPara["Project"] + "   " + instance.m_dicPara["paraMes"]);
            if (instance.m_dicPara["Station"].Contains("AT"))       //AT
            {
                instance.m_curve.Show();
            }
            //instance.m_report.Show();
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            if (m_bSaveLayout)
                dockPanel.SaveAsXml(configFile);
            else if (File.Exists(configFile))
                File.Delete(configFile);
        }

        private void menuItemToolBar_Click(object sender, System.EventArgs e)
        {
            toolBar.Visible = menuItemToolBar.Checked = !menuItemToolBar.Checked;
        }

        private void menuItemStatusBar_Click(object sender, System.EventArgs e)
        {
            statusBar.Visible = menuItemStatusBar.Checked = !menuItemStatusBar.Checked;
        }

        private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == toolBarButtonNew)
                menuItemNew_Click(null, null);
            else if (e.ClickedItem == toolBarButtonOpen)
                menuItemOpen_Click(null, null);
            else if (e.ClickedItem == toolBarButtonSolutionExplorer)
                menuItemSolutionExplorer_Click(null, null);
            else if (e.ClickedItem == toolBarButtonPropertyWindow)
                menuItemPropertyWindow_Click(null, null);
            else if (e.ClickedItem == toolBarButtonToolbox)
                menuItemToolbox_Click(null, null);
            else if (e.ClickedItem == toolBarButtonOutputWindow)
                menuItemOutputWindow_Click(null, null);
            else if (e.ClickedItem == toolBarButtonTaskList)
                menuItemTaskList_Click(null, null);
            else if (e.ClickedItem == toolBarButtonLayoutByCode)
                menuItemLayoutByCode_Click(null, null);
            else if (e.ClickedItem == toolBarButtonLayoutByXml)
                menuItemLayoutByXml_Click(null, null);
            else if (e.ClickedItem == toolBarButtonDockPanelSkinDemo)
                SetDockPanelSkinOptions(!toolBarButtonDockPanelSkinDemo.Checked);
        }

        private void menuItemNewWindow_Click(object sender, System.EventArgs e)
        {
            CeaseForm newWindow = new CeaseForm();
            newWindow.Text = newWindow.Text + " - New";
            newWindow.Show();
        }

        private void menuItemTools_Popup(object sender, System.EventArgs e)
        {
            menuItemLockLayout.Checked = !this.dockPanel.AllowEndUserDocking;
        }

        private void menuItemLockLayout_Click(object sender, System.EventArgs e)
        {
            dockPanel.AllowEndUserDocking = !dockPanel.AllowEndUserDocking;
        }

        private void menuItemLayoutByCode_Click(object sender, System.EventArgs e)
        {
            dockPanel.SuspendLayout(true);

            CloseAllDocuments();

            m_solutionExplorer = new DummySolutionExplorer();
            m_TestInforWindow = new TestInformationWindow();
            m_ctrlPanel = new ControlPanel();
            m_outputWindow = new OutputWindow();
            m_taskList = new TestItemList();
            m_curve = new CurvesForm();

            m_solutionExplorer.Show(dockPanel, DockState.DockRight);
            m_TestInforWindow.Show(m_solutionExplorer.Pane, m_solutionExplorer);
            m_ctrlPanel.Show(dockPanel, new Rectangle(98, 133, 200, 383));
            m_outputWindow.Show(m_solutionExplorer.Pane, DockAlignment.Bottom, 0.35);
            m_curve.Show(m_solutionExplorer.Pane, DockAlignment.Bottom, 0.7);
            m_taskList.Show(m_ctrlPanel.Pane, DockAlignment.Left, 0.4);

            DummyDoc doc1 = CreateNewDocument("Document1");
            DummyDoc doc2 = CreateNewDocument("Document2");
            DummyDoc doc3 = CreateNewDocument("Document3");
            DummyDoc doc4 = CreateNewDocument("Document4");
            doc1.Show(dockPanel, DockState.Document);
            doc2.Show(doc1.Pane, null);
            doc3.Show(doc1.Pane, DockAlignment.Bottom, 0.5);
            doc4.Show(doc3.Pane, DockAlignment.Right, 0.5);

            dockPanel.ResumeLayout(true, true);
        }

        private void menuItemLayoutByXml_Click(object sender, System.EventArgs e)
        {
            dockPanel.SuspendLayout(true);

            // In order to load layout from XML, we need to close all the DockContents
            CloseAllContents();

            Assembly assembly = Assembly.GetAssembly(typeof(CeaseForm));
            Stream xmlStream = assembly.GetManifestResourceStream("CeaseUI.Resources.DockPanel.xml");
            dockPanel.LoadFromXml(xmlStream, m_deserializeDockContent);
            xmlStream.Close();

            dockPanel.ResumeLayout(true, true);
        }

        private void menuItemCloseAllButThisOne_Click(object sender, System.EventArgs e)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                Form activeMdi = ActiveMdiChild;
                foreach (Form form in MdiChildren)
                {
                    if (form != activeMdi)
                        form.Close();
                }
            }
            else
            {
                foreach (IDockContent document in dockPanel.DocumentsToArray())
                {
                    if (!document.DockHandler.IsActivated)
                        document.DockHandler.Close();
                }
            }
        }

        private void menuItemShowDocumentIcon_Click(object sender, System.EventArgs e)
        {
            dockPanel.ShowDocumentIcon = menuItemShowDocumentIcon.Checked = !menuItemShowDocumentIcon.Checked;
        }

        private void showRightToLeft_Click(object sender, EventArgs e)
        {
            CloseAllContents();
            //if (showRightToLeft.Checked)
            //{
            //    this.RightToLeft = RightToLeft.No;
            //    this.RightToLeftLayout = false;
            //}
            //else
            {
                this.RightToLeft = RightToLeft.Yes;
                this.RightToLeftLayout = true;
            }
            m_solutionExplorer.RightToLeftLayout = this.RightToLeftLayout;
            //showRightToLeft.Checked = !showRightToLeft.Checked;
        }

        private void exitWithoutSavingLayout_Click(object sender, EventArgs e)
        {
            m_bSaveLayout = false;
            Close();
            m_bSaveLayout = true;
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasswordForm mPasswordform = new PasswordForm();
            DialogResult rtRes = mPasswordform.ShowDialog();
            if (rtRes == DialogResult.OK || rtRes == DialogResult.Yes)
            {
                ConfigForm mConfigForm = new ConfigForm(instance.m_dicPara, m_LogList[0], rtRes == DialogResult.Yes ? true : false);
                mConfigForm.Owner = instance;

                rtRes = mConfigForm.ShowDialog();
                if (rtRes == DialogResult.OK)
                {
                    for (int i = 0; i < int.Parse(instance.m_dicPara["TotalDut"]); i++)
                    {
                        UpdateDutStatus(i, "CONFIG_SAVE", true);
                    }
                }
            }
            else if (rtRes == DialogResult.Abort)
            {
                MessageBox.Show("Error Password!!");
            }
            else if (rtRes == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                MessageBox.Show("Unknown Operation!!");
            }
        }

        private void cablossToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mCablossForm = new CablossForm(instance.m_dicPara, m_LogList[0]);
          //  mCablossForm.Owner = instance;

            mCablossForm.Owner = this;
            bool progressVisible = instance.m_taskList.m_TestProgressform.Visible;
            instance.m_taskList.m_TestProgressform.Visible = false;
            DialogResult rtRes = mCablossForm.ShowDialog();
            if (rtRes == DialogResult.OK)
            {

            }

            instance.m_taskList.m_TestProgressform.Visible = progressVisible;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int iTotal = int.Parse(instance.m_dicPara["TotalDut"]);
            string strDisp = "测试时间  ::";
            
            for (int i = 0; i < iTotal; i++ )
            {
                //m_iTestTimeS[i] = m_bTestTimeEn[i] ? m_iTestTimeS[i] + 1 : m_iTestTimeS[i];
                // strDisp += string.Format("  DUT{0} - {1}s  ::", i + 1, m_iTestTimeS[i]);
                TestEndTime[i] = new TimeSpan(DateTime.Now.Ticks);
                int spanTotalSeconds =(int)TestEndTime[i].Subtract(TestStartTime[i]).Duration().TotalSeconds;
                m_iTestTimeS[i] = m_bTestTimeEn[i] ? spanTotalSeconds : m_iTestTimeS[i];
                strDisp += string.Format("  DUT{0} - {1}s  ::", i + 1, m_iTestTimeS[i]);
            }

            this.toolStripStatusLabelTime.Text = strDisp; 
        }

        private void toolStripSplitButtonFrequency_ButtonClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"Packages\\WlsCommsCalc401.exe");
        }

        private void progressBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            instance.m_taskList.m_TestProgressform.Visible = progressBarToolStripMenuItem.Checked = !progressBarToolStripMenuItem.Checked;
        }

        public void InitialTimer(Object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new EventHandler(InitialTimer), new object[] { sender, e });
            }
            else
            {
                var args = (CeaseFormInitialEventArgs)e;
                int iMultiDut = int.Parse(args.m_dic["TotalDut"]);

                instance.toolStripStatusLabelTime.Text = "系统当前时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                instance.timer1.Interval = 1000;
                instance.timer1.Start();
            }
        }

        private void UpdateTestResultStatistics(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                EventHandler fun = new EventHandler(UpdateTestResultStatistics);
                BeginInvoke(fun, new object[] { sender, e });
            }
            else
            {
                var args = (ButtonClickedEventArgs)e;

                if (args.m_strDutStat != "PASS" && args.m_strDutStat != "FAIL")
                {
                    return;
                }

                instance.m_TestInforWindow.UpdateTestResultStatistics(args.m_iDut, args.m_bTestRes);

                List<string> dicStr = new List<string>() { "Total", "Dut1", "Dut2", "Dut3", "Dut4" };
                string strStatus = "";
                for (int i = 0; i < int.Parse(instance.m_dicPara["TotalDut"]) + 1; i++)
                {
                    int iPass = m_TestInforWindow.m_ListDutStatics[i].m_iPassCount;
                    int iTotal = m_TestInforWindow.m_ListDutStatics[i].m_iTotalCount;
                    double dfRate = m_TestInforWindow.m_ListDutStatics[i].PassRate();
                    strStatus += string.Format("{0}: {1}/{2} {3}%     ", dicStr[i], iPass, iTotal, dfRate.ToString("f2"));
                }

                instance.toolStripStatusLabel1.Text = strStatus;

                this.Update();
            }
        }

        private void SetDutStatus(Object sender, EventArgs e)
        {
            var args = (ButtonClickedEventArgs)e;
            TestStationStat[args.m_iDut] = args.m_strDutStat;
        }

        private void EnableDutTimer(Object sender, EventArgs e)
        {
            var args = (ButtonClickedEventArgs)e;
            m_bTestTimeEn[args.m_iDut] = true;
            m_iTestTimeS[args.m_iDut] = 0;
        }

        private void DisableDutTimer(Object sender, EventArgs e)
        {
            var args = (ButtonClickedEventArgs)e;
            m_bTestTimeEn[args.m_iDut] = false;
        }

        private void LockTestListInitial(Object sender, EventArgs e)
        {
            instance.m_bTestListInitialEn = false;
        }

        private void UnlockTestListInitial(Object sender, EventArgs e)
        {
            instance.m_bTestListInitialEn = true;
        }

        private void UpdateCycleTestStat(Object sender, EventArgs e)
        {
            var args = (ButtonStartEndEventArgs)e;

            if (++instance.TestCount[args.m_iDut] < int.Parse(instance.m_dicPara["paraTestCount"]))
            {
                System.Threading.Thread.Sleep(int.Parse(instance.m_dicPara["paraTestCountDelay"]));
                if (args.m_dic.ContainsKey("paraCycleTestFailStop") && args.m_dic["paraCycleTestFailStop"] == "1")    //sasa 20180321      压测加入failbreak定义
                {
                    if (args.m_strDutStat != "PASS")
                    {
                        return;
                    }
                }
                instance.OnButtonClicked(new ButtonClickedEventArgs(instance.m_dicPara, args.m_iDut, "START", true, false, "INIT", true, "STOP"));
            }
        }

        #endregion

        #region Interface_Method
        private void RegisterCommonParaDic(Dictionary<string, string> _dic)
        {
            instance.m_dicPara = _dic;

            if (!instance.m_dicPara.ContainsKey("paraTestCount") || instance.m_dicPara["paraMes"] != "OFFLINE")
            {
                instance.m_dicPara["paraTestCount"] = "1";
            }

            if (!instance.m_dicPara.ContainsKey("paraTestCountDelay"))
            {
                instance.m_dicPara["paraTestCountDelay"] = "1000";
            }
        }

        #endregion

        #region Interface_UI
        public Form Instance
        {
            get
            {
                return instance;
            }
        }

        public void RegisterLogger(InterfaceLog _log)
        {
            instance.m_LogList.Add(_log);
        }

        public string GetScanBarcode(int _iDut)
        {
            return instance.m_strScanCode[_iDut];
        }

        public delegate void UpdateMesDisDelegate(string message);
        void UpdateMesDis(string message)
        {
            if (instance.InvokeRequired)
            {
                UpdateMesDisDelegate md = new UpdateMesDisDelegate(UpdateMesDis);
                instance.BeginInvoke(md, new object[] { message });
            }
            else
            {
                instance.toolBarLableMES.Text = " " + message;
                if (!message.Contains("OFFLINE"))
                {
                    instance.toolBarLableMES.ForeColor = Color.Blue;
                }
                else
                {
                    instance.toolBarLableMES.ForeColor = Color.Red;
                }
            }
        }

        public void SetMESStatus(string MES)
        {
            UpdateMesDis(MES);
        }
        public bool Initialize(Dictionary<string, string> _dicPara)
        {
            instance.RegisterCommonParaDic(_dicPara);
            return true;
        }

        public void SetDutStatus(int _iDut, string _strStat)
        {
            instance.TestStationStat[_iDut] = _strStat;
        }

        public string GetDutStatus(int _iDut)
        {
            return instance.TestStationStat[_iDut];
        }

        public void UpdateDutStatus(int _iDut, string _stat, bool bRes)
        {
            switch (_stat)
            {
                case "INIT_CLICK":
                    instance.OnButtonClicked(new ButtonClickedEventArgs(instance.m_dicPara, _iDut, "INIT", bRes, true, "STOP", false, "START"));
                    break;

                case "INIT_END":
                    instance.OnListFormInitial(new ListFormInitialEventArgs(instance.m_dicPara));
                    instance.OnButtonInitEnd(new ButtonInitEndEventArgs(instance.m_dicPara, _iDut, bRes ? "READY" : "FAIL_ENV", bRes, true, "INIT", bRes, "START"));
                    break;

                case "START_CLICK":
                    instance.TestCount[_iDut] = 0;;
                    instance.OnButtonClicked(new ButtonClickedEventArgs(instance.m_dicPara, _iDut, "START", bRes, false, "INIT", true, "STOP"));
                    break;

                case "START_END":
                    instance.OnButtonStartEnd(new ButtonStartEndEventArgs(instance.m_dicPara, _iDut, bRes ? "PASS" : "FAIL", bRes, true, "INIT", true, "START"));
                    break;

                case "CONFIG_SAVE":
                    instance.OnConfigSaved(new ButtonClickedEventArgs(instance.m_dicPara,_iDut, "", bRes, true, "INIT", false, "START"));
                    break;

                case "STOP_CLICK":
                    instance.TestCount[_iDut] = int.Parse(instance.m_dicPara["paraTestCount"]);
                    instance.SetDutStatus(_iDut, "STOP");
                    break;

                default:
                    break;
            }
        }

        public void DisplaySN(int iDut, string SN)
        {
            instance.m_TestInforWindow.DisplaySN(iDut,SN);
        }
        public void UpdateTestResult(int _iDut, string itemIdx, string value, string status)
        {
            instance.m_taskList.UpdateTestResult(_iDut, itemIdx, value, status);
        }
        public void UpdateTestResult(int _iDut, string itemIdx, string testResult)
        {
            UpdateTestResult(_iDut, itemIdx, testResult, testResult);
        }

        //AT
        public void UpdateTestInforDisp_AT(Dictionary<string, string> _dic)
        {
            instance.m_TestInforWindow.UpdateTestInforDisp_AT(_dic);
        }
        //AT
        public void StartClickAction(int _dut)
        {
            instance.m_ctrlPanel.StartClickAction(_dut);
        }
        //AT
        public void ClearUI(int dut)
        {
            instance.m_TestInforWindow.ClearSN();
        }



        //AT
        public void ShowCurveForm()
        {
            //instance.m_curve.ShowCurveForm();
            instance.curvesToolStripMenuItem.PerformClick();
        }

        //AT
        public void ShowCurvePage(string pageName, string SN, double[] freq, double[] AMPL, double[] AMPL_USL, double[] AMPL_LSL,
            double[] THD, double[] THD_USL, double[] THD_LSL,
            double[] RB, double[] RB_USL, double[] RB_LSL, double[] AMPL_REF, Dictionary<string, bool> dicLoopResult, Dictionary<string, bool> dicItemResult, double[] AMPL2, double[] THD2, double[] RB2)
        {
            instance.m_curve.ShowPage(pageName, SN, freq, AMPL, AMPL_USL, AMPL_LSL,
            THD, THD_USL, THD_LSL,
            RB, RB_USL, RB_LSL, AMPL_REF, dicLoopResult, dicItemResult, AMPL2, THD2, RB2);

            instance.m_curve.CurvesTabInvalidate();
        }

        //AT
        public void HideAllPages()
        {
            instance.m_curve.HideAllPages();
        }

        //AT
        public void SaveCurvesImage(string dir,string sn)
        {
            instance.m_curve.SavePanelsImage(dir,sn);
        }

        //AT
        public void SaveCeaseMainFormImage(string dir, string SN, string testResult)
        {
            instance.m_TestInforWindow.SaveTestInfoFormImage(dir, SN, testResult);
            instance.m_taskList.SaveTestListFormImage(dir, SN, testResult);
        }

        //AT
        private void audioAnalyzerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + @"\Addins\AudioSWT\Audio-Analyser.exe");
        }

        //AT
        private void aTLimitSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + @"\Addins\AudioSW\ATLimitSetting.exe");
        }

        //AT
        private void curvesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            instance.m_curve.Show();
        }

        //AT
        public void ShowTestInfoPage()
        {
            instance.m_TestInforWindow.ShowTestInfoPage();
        }

        public void UpdateHistory(int dut, Dictionary<int, bool> dicHis)
        {
        }


        public void UpdateHistory(int dut, Dictionary<int, Dictionary<string,string>> dicHisInfo)
        {
        }


        /// <summary>
        /// 更新主界面的ReportViewr
        /// </summary>
        public void UpdateReportViewr(string fileName)
        {
        }

        #endregion

        private void audioCurvesCollectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string toolPath = Application.StartupPath + @"\Toolkit\AudioCurvesCollector\AudioCurvesCollector.exe";
            if (!System.IO.File.Exists(toolPath))
            {
                return;
            }
            //写入初始路径
            File.WriteAllText(Application.StartupPath + @"\Toolkit\AudioCurvesCollector\StartInfo.ini", instance.m_dicPara["paraLogPath"] + "\\DUT1\\" + instance.m_dicPara["Project"] + "\\" + instance.m_dicPara["Station"] + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\PASS\\ATReport");
            System.Diagnostics.Process.Start(Application.StartupPath + @"\Toolkit\AudioCurvesCollector\AudioCurvesCollector.exe");
        }

        private void logDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            m_curve.Show(dockPanel);
        }
        private void AfterSaleImport_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
	    
        private void aTSendSyncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string toolPath = Application.StartupPath + @"\Toolkit\AT_SendSync\AT_SendSync.exe";
            if (!System.IO.File.Exists(toolPath))
            {
                MessageBox.Show("Can't find " + toolPath);
                return;
            }
            System.Diagnostics.Process.Start(toolPath);
        }

        private void reportViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 装备神器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ExePath = Application.StartupPath + "\\Addins\\QPhone\\装备神器.exe";
            if (File.Exists(ExePath))
            {
                Process.Start(ExePath);
            }
        }

        private void configFileDirectoryToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Explorer.exe", Application.StartupPath + "\\Project\\" + instance.m_dicPara["Project"] + "\\" + instance.m_dicPara["Station"]);

        }

        private void logDirectoryToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (instance.m_dicPara.ContainsKey("paraLogPath"))
            {
                System.Diagnostics.Process.Start("Explorer.exe", instance.m_dicPara["paraLogPath"] + "\\DUT1\\" + instance.m_dicPara["Project"] + "\\" + instance.m_dicPara["Station"] + "\\" + DateTime.Now.ToString("yyyy-MM-dd"));

            }
        }
    }
	
    /// <summary>
    /// FormInitial事件
    /// </summary>
    public class CeaseFormInitialEventArgs : EventArgs
    {
        // class members  
        public Dictionary<string, string> m_dic;
        public CeaseForm m_CeaseForm;

        /// <summary>
        /// ButtonClickedEventArgs构造函数
        /// </summary>
        public CeaseFormInitialEventArgs(Dictionary<string, string> _dic, CeaseForm _ceaseform)
        {
            this.m_dic = new Dictionary<string, string>(_dic);
            this.m_CeaseForm = _ceaseform;
        }
    }

    /// <summary>
    /// ListInitial事件
    /// </summary>
    public class ListFormInitialEventArgs : EventArgs
    {
        // class members  
        public Dictionary<string, string> m_dic;

        /// <summary>
        /// ButtonClickedEventArgs构造函数
        /// </summary>
        public ListFormInitialEventArgs(Dictionary<string, string> _dic)
        {
            this.m_dic = new Dictionary<string, string>(_dic);
        }
    }

    /// <summary>
    /// ButtonClicked事件
    /// </summary>
    public class ButtonClickedEventArgs : EventArgs
    {
        // class members  
        public Dictionary<string, string> m_dic;

        public int m_iDut;
        public string m_strDutStat;

        public bool m_bInitBtnEn;
        public string m_strInitText;

        public bool m_bStartBtnEn;
        public string m_strStartText;

        public bool m_bTestRes;

        /// <summary>
        /// ButtonClickedEventArgs构造函数
        /// </summary>
        public ButtonClickedEventArgs(Dictionary<string, string> _dic, int _iDut, string _strDutStat, bool _bTestRes, bool _bInitBtnEn, string _strInitText, bool _bStartBtnEn, string _strStartText)
        {
            this.m_dic = new Dictionary<string, string>(_dic);
            this.m_iDut = _iDut;
            this.m_strDutStat = _strDutStat;
            this.m_bInitBtnEn = _bInitBtnEn;
            this.m_strInitText = _strInitText;
            this.m_bStartBtnEn = _bStartBtnEn;
            this.m_strStartText = _strStartText;

            this.m_bTestRes = _bTestRes;
        }
    }

    /// <summary>
    /// ButtonInitEnd事件
    /// </summary>
    public class ButtonInitEndEventArgs : ButtonClickedEventArgs
    {
        /// <summary>
        /// ButtonInitEndEventArgs构造函数
        /// </summary>
        public ButtonInitEndEventArgs(Dictionary<string, string> _dic, int _iDut, string _strDutStat, bool _bTestRes, bool _bInitBtnEn, string _strInitText, bool _bStartBtnEn, string _strStartText)
            : base(_dic, _iDut, _strDutStat, _bTestRes, _bInitBtnEn, _strInitText, _bStartBtnEn, _strStartText)
        {

        }
    }

    /// <summary>
    /// ButtonStartEnd事件
    /// </summary>
    public class ButtonStartEndEventArgs : ButtonClickedEventArgs
    {
        /// <summary>
        /// ButtonClickedEventArgs构造函数
        /// </summary>
        public ButtonStartEndEventArgs(Dictionary<string, string> _dic, int _iDut, string _strDutStat, bool _bTestRes, bool _bInitBtnEn, string _strInitText, bool _bStartBtnEn, string _strStartText)
            : base(_dic, _iDut, _strDutStat, _bTestRes, _bInitBtnEn, _strInitText, _bStartBtnEn, _strStartText)
        {
        }
    }
}