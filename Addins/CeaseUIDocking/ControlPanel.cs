using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CeaseUI.Docking;
using System.Threading;


namespace CeaseUI
{
    public partial class ControlPanel : ToolWindow
    {
        public CeaseForm m_ceaseform;
        int m_iTotalDut;

        protected List<Button> m_btnInitList;
        protected List<Button> m_btnStartList;
        protected List<TextBox> m_tbxStartList;

        static ReaderWriterLockSlim CodeWriteLock = new ReaderWriterLockSlim();

        public ControlPanel()
        {
            InitializeComponent();

            m_btnInitList = new List<Button>() { InitDut1, InitDut2, InitDut3, InitDut4 };
            m_btnStartList = new List<Button>() { StartDut1, StartDut2, StartDut3, StartDut4 };
            m_tbxStartList = new List<TextBox>() { textBox1, textBox2, textBox3, textBox4 };
        }

        public void InitialControlPanel(Object sender, EventArgs e)
        {
            var args = (CeaseFormInitialEventArgs)e;

            m_ceaseform = args.m_CeaseForm;

            m_iTotalDut = int.Parse(args.m_dic["TotalDut"]);
            for (int i = 0; i < m_iTotalDut; i++)
            {
                m_btnInitList[i].Enabled = true;
            }

            //start or scan
            bool bScanEn = (args.m_dic.ContainsKey("paraCodeScanEn") && args.m_dic["paraCodeScanEn"].ToUpper() == "TRUE");
            for (int i = 0; i < m_tbxStartList.Count; i++)
            {
                m_tbxStartList[i].Visible = bScanEn;
            }
        }

        public void DisplayStartButton(Object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new EventHandler(DisplayStartButton), new object[] { sender, e });
            }
            else
            {
                var args = (ButtonInitEndEventArgs)e;

                if (args.m_strDutStat == "READY" && int.Parse(args.m_dic["TotalDut"]) == 1)
                {
                    tabControl1.SelectedIndex = 1;
                }
            }
        }

        public void LockDutTestBtn(Object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                EventHandler fun = new EventHandler(LockDutTestBtn);
                BeginInvoke(fun, new object[] { sender, e });
            }
            else
            {
                var args = (ButtonClickedEventArgs)e;

                m_btnInitList[args.m_iDut].Enabled = args.m_bInitBtnEn;
                m_btnInitList[args.m_iDut].Text = args.m_strInitText;

                m_btnStartList[args.m_iDut].Enabled = args.m_bStartBtnEn;
                m_btnStartList[args.m_iDut].Text = args.m_strStartText;

                if (args.m_dic.ContainsKey("paraCodeScanEn") && args.m_dic["paraCodeScanEn"].ToUpper() == "TRUE")
                {
                    //update textBox
                    m_tbxStartList[args.m_iDut].Enabled = (args.m_bStartBtnEn && args.m_strStartText == "START");

                    //start click, save code
                    if (args.m_bStartBtnEn && args.m_strStartText == "STOP")
                    {
                        CodeWriteLock.EnterWriteLock();
                        m_ceaseform.SetScanCode(args.m_iDut, m_tbxStartList[args.m_iDut].Text);
                        CodeWriteLock.ExitWriteLock();

                        FindNextFocus(args.m_iDut);
                    }

                    // start end, focus enable if others disable
                    if (args.m_bStartBtnEn && args.m_strStartText == "START")
                    {
                        //clear code
                        m_tbxStartList[args.m_iDut].Text = "";

                        bool bEn = false;
                        for (int i = 0; i < m_tbxStartList.Count; i++)
                        {
                            if (i != args.m_iDut)
                            {
                                bEn = bEn || m_tbxStartList[i].Enabled;
                            }
                        }

                        if (!bEn)
                        {
                            m_tbxStartList[args.m_iDut].Focus();
                        }

                        //clear code
                        CodeWriteLock.EnterWriteLock();
                        m_ceaseform.SetScanCode(args.m_iDut, "");
                        CodeWriteLock.ExitWriteLock();
                    }
                }
            }
        }

        private void SelectIndex_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text == "START")
            {
                FindNextFocus(0);
            }
        }

        private void Init_Click(object sender, EventArgs e)
        {
            m_ceaseform.UpdateDutStatus(0, InitDut1.Text + "_CLICK", true);
        }

        private void InitDut2_Click(object sender, EventArgs e)
        {
            m_ceaseform.UpdateDutStatus(1, InitDut2.Text + "_CLICK", true);
        }

        private void InitDut3_Click(object sender, EventArgs e)
        {
            m_ceaseform.UpdateDutStatus(2, InitDut3.Text + "_CLICK", true);
        }

        private void InitDut4_Click(object sender, EventArgs e)
        {
            m_ceaseform.UpdateDutStatus(3, InitDut4.Text + "_CLICK", true);
        }

        private void Start_Click(object sender, EventArgs e)
        {
            m_ceaseform.UpdateDutStatus(0, StartDut1.Text + "_CLICK", true);
        }

        private void StartDut2_Click(object sender, EventArgs e)
        {
            m_ceaseform.UpdateDutStatus(1, StartDut2.Text + "_CLICK", true);
        }

        private void StartDut3_Click(object sender, EventArgs e)
        {
            m_ceaseform.UpdateDutStatus(2, StartDut3.Text + "_CLICK", true);
        }

        private void StartDut4_Click(object sender, EventArgs e)
        {
            m_ceaseform.UpdateDutStatus(3, StartDut4.Text + "_CLICK", true);
        }

        private void FindNextFocus(int _dutNum)
        {
            int i = 0;
            while(i < m_iTotalDut) 
            {
                i++;
                if (++_dutNum == m_iTotalDut)
                {
                    _dutNum = 0;
                }

                if (m_tbxStartList[_dutNum].Enabled)
                {
                    m_tbxStartList[_dutNum].Focus();
                    break;
                }
            } 

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.Enter)
            {
                m_ceaseform.UpdateDutStatus(0, StartDut1.Text + "_CLICK", true);
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.Enter)
            {
                m_ceaseform.UpdateDutStatus(1, StartDut2.Text + "_CLICK", true);
            }
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.Enter)
            {
                m_ceaseform.UpdateDutStatus(2, StartDut3.Text + "_CLICK", true);
            }
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.Enter)
            {
                m_ceaseform.UpdateDutStatus(3, StartDut4.Text + "_CLICK", true);
            }
        }

        //AT
        delegate void StartClickActionCallBack(int _dut);
        public void StartClickAction(int _dut)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new StartClickActionCallBack(StartClickAction), new object[] { _dut });
            }
            else
            {

                switch (_dut)
                {
                    case 0:
                        if (StartDut1.Enabled)
                        {
                            StartDut1.PerformClick();
                        }
                        else
                        {

                        }
                        break;
                    default:
                        break;
                }
            }

        }
    }

    public class TestStationStat
    {
        public string m_strStat;
        public int m_iTestCount;
    }
}