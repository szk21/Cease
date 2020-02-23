using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CeaseUI.Docking;

namespace CeaseUI
{
    public partial class TestInformationWindow : ToolWindow
    {
        public class DutStaticsInfor
        {
            public int m_iTotalCount;
            public int m_iPassCount;

            public TextBox m_TotalBox;
            public TextBox m_PassBox;
            public TextBox m_RateBox;

            public DutStaticsInfor(int _iTotal, int _iPass, TextBox _Total, TextBox _Pass, TextBox _Rate)
            {
                m_iTotalCount = _iTotal;
                m_iPassCount = _iPass;

                m_TotalBox = _Total;
                m_PassBox = _Pass;
                m_RateBox = _Rate;
            }

            public double PassRate()
            {
                if (m_iTotalCount == 0)
                {
                    return 0;
                }

                return (m_iPassCount * 1.0 / m_iTotalCount) * 100;
            }

            public void InitStatics()
            {
                m_iPassCount = 0;
                m_iTotalCount = 0;

                m_TotalBox.Text = "0";
                m_PassBox.Text = "0";
                m_RateBox.Text = "0";
            }

            public void UpdateStatics(bool _res)
            {
                m_iTotalCount++;
                if (_res)
                {
                    m_iPassCount++;
                }

                m_TotalBox.Text = m_iTotalCount.ToString();
                m_PassBox.Text = m_iPassCount.ToString();
                m_RateBox.Text = PassRate().ToString("f2");
            }
        }

        public List<DutStaticsInfor> m_ListDutStatics = new List<DutStaticsInfor>();
        public List<Label> m_listDutResLabel = new List<Label>();
        public List<TextBox> m_listTextBoxSN = new List<TextBox>();

        public TestInformationWindow()
        {
            InitializeComponent();
            InitialWindow();
        }

        protected void InitialWindow()
        {
            m_ListDutStatics.Clear();
            m_ListDutStatics.Add(new DutStaticsInfor(0, 0, textBoxTestTotal, textBoxTestPass, textBoxTestRate));
            m_ListDutStatics.Add(new DutStaticsInfor(0, 0, textBoxTotalDut1, textBoxPassDut1, textBoxRateDut1));
            m_ListDutStatics.Add(new DutStaticsInfor(0, 0, textBoxTotalDut2, textBoxPassDut2, textBoxRateDut2));
            m_ListDutStatics.Add(new DutStaticsInfor(0, 0, textBoxTotalDut3, textBoxPassDut3, textBoxRateDut3));
            m_ListDutStatics.Add(new DutStaticsInfor(0, 0, textBoxTotalDut4, textBoxPassDut4, textBoxRateDut4));

            m_listDutResLabel.Clear();
            m_listDutResLabel.Add(labelResDut1);
            m_listDutResLabel.Add(labelResDut2);
            m_listDutResLabel.Add(labelResDut3);
            m_listDutResLabel.Add(labelResDut4);

            m_listTextBoxSN.Clear();
            m_listTextBoxSN.Add(txb1);
            m_listTextBoxSN.Add(txb2);
            m_listTextBoxSN.Add(txb3);
            m_listTextBoxSN.Add(txb4);

            tabControl1.SelectedIndex = 2;
        }

        public void InitialProductInforDisp(Object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new EventHandler(InitialProductInforDisp), new object[] { sender, e });
            }
            else
            {
                var args = (CeaseFormInitialEventArgs)e;

                foreach (var dut in m_ListDutStatics)
                {
                    dut.InitStatics();
                }

                //textBoxProduct.Text = args.m_dic["Project"];
                //textBoxStation.Text = args.m_dic["paraStation"];
            }
        }

        delegate bool UpdateTestResultStatisticsCallBack(int _idut, bool _res);
        public bool UpdateTestResultStatistics(int _iDut, bool _res)
        {
            if (this.InvokeRequired)
            {
                UpdateTestResultStatisticsCallBack fun = new UpdateTestResultStatisticsCallBack(UpdateTestResultStatistics);
                BeginInvoke(fun, new object[] { _iDut, _res });
            }
            else
            {
                m_ListDutStatics[0].UpdateStatics(_res);
                m_ListDutStatics[_iDut + 1].UpdateStatics(_res);

                this.Update();
            }

            return true;
        }

        public delegate void DisplaySNDelegate(int Index, string SN);
        public void DisplaySN(int Index, string SN)
        {
            if (m_listTextBoxSN[Index].InvokeRequired)
            {
                DisplaySNDelegate md = new DisplaySNDelegate(DisplaySN);
                m_listTextBoxSN[Index].BeginInvoke(md, new object[] { Index, SN });
            }
            else
            {
                m_listTextBoxSN[Index].Text = SN;
                m_listTextBoxSN[Index].ForeColor = Color.Blue;
            }
        }

        public void UpdateTestResultDisp(Object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                EventHandler fun = new EventHandler(UpdateTestResultDisp);
                BeginInvoke(fun, new object[] { sender, e });
            }
            else
            {
                var args = (ButtonClickedEventArgs)e;

                m_listDutResLabel[args.m_iDut].Text = args.m_strDutStat;
                switch (args.m_strDutStat)
                {
                    case "FAIL_ENV":
                    case "FAIL":
                        m_listDutResLabel[args.m_iDut].BackColor = System.Drawing.Color.Red;
                        break;

                    case "READY":
                    case "PASS":
                        m_listDutResLabel[args.m_iDut].BackColor = System.Drawing.Color.Green;
                        break;

                    case "":
                    case "INIT":
                        m_listDutResLabel[args.m_iDut].Text = "Initializing";
                        m_listDutResLabel[args.m_iDut].BackColor = System.Drawing.Color.Empty;
                        break;

                    case "START":
                        m_listDutResLabel[args.m_iDut].Text = "RUNNING";
                        m_listDutResLabel[args.m_iDut].BackColor = System.Drawing.Color.Blue;
                        m_listTextBoxSN[args.m_iDut].Text = "";
                        break;

                    default:
                        break;
                }

                this.Update();
            }
        }

        //AT
        delegate void UpdateTestInforDisp_ATCallBack(Dictionary<string, string> _dic);
        public void UpdateTestInforDisp_AT(Dictionary<string, string> _dic)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new UpdateTestInforDisp_ATCallBack(UpdateTestInforDisp_AT), new object[] { _dic });
            }
            else
            {
                lock (this)
                {
                    if (_dic.ContainsKey("BSN"))
                    {
                        textBoxBSN.Text = _dic["BSN"];
                    }
                    if (_dic.ContainsKey("SocketServer"))
                    {
                        textBoxService.Text = _dic["SocketServer"];
                    }
                    if (_dic.ContainsKey("SerialPort"))
                    {
                        textBox_serialport.Text = _dic["SerialPort"];
                    }

                    if (_dic.ContainsKey("SocketClient"))
                    {
                        textBoxClient.Text = _dic["SocketClient"];
                        if (textBoxClient.Text == "No Connection")
                        {
                            textBoxClient.BackColor = Color.Yellow;
                        }
                        else
                        {
                            textBoxClient.BackColor = Color.Green;
                        }
                    }

                    if (_dic.ContainsKey("ClientTwinkle"))
                    {
                        textBoxClient.Text = _dic["ClientTwinkle"];
                        textBoxClient.BackColor = Color.Yellow;
                    }

                    if (_dic.ContainsKey("ClientConnected"))
                    {
                        textBoxClient.BackColor = Color.YellowGreen;
                    }

                    if (_dic.ContainsKey("FixNum"))
                    {
                        textBoxFixNum.Text = _dic["FixNum"];
                    }

                    if (_dic.ContainsKey("MESIP"))
                    {
                        textBox_MESIP.Text = _dic["MESIP"];
                    }
                }
            }
        }
        //AT
        delegate void ClearSNCallBack();
        public void ClearSN()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new ClearSNCallBack(ClearSN));
            }
            else
            {
                textBoxBSN.Text = "";
            }
        }


        //AT
        delegate void SaveTestInfoFormImageCallBack(string dir, string SN, string testResult);
        public void SaveTestInfoFormImage(string dir, string SN, string testResult)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new SaveTestInfoFormImageCallBack(SaveTestInfoFormImage), new object[] { dir, SN, testResult });
            }
            else
            {
                ControlImage.SaveControlImage(this, dir, "TestInfoForm_" + SN + "_" + testResult + ".jpg");
            }
        }

        //AT
        delegate void ShowTestInfoPageCallBack();
        public void ShowTestInfoPage()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new ShowTestInfoPageCallBack(ShowTestInfoPage), new object[] { });
            }
            else
            {
                tabControl1.SelectedIndex = 0;
            }
        }
    }
}