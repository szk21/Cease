using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CeaseUI.Docking;

using Cease.Addins.Log;

namespace CeaseUI
{
    public partial class OutputWindow : ToolWindow
    {
        private List<ListView> m_listDebug;
        private List<ListView> m_listDut;
        private List<ListView> m_listGpib;
        private List<ListView> m_listReport;

        public OutputWindow()
        {
            InitializeComponent();

            m_listDebug = new List<ListView>() { listViewDebugDut1, listViewDebugDut2, listViewDebugDut3, listViewDebugDut4 };
            m_listDut = new List<ListView>() { listViewDutDut1, listViewDutDut2, listViewDutDut3, listViewDutDut4 };
            m_listGpib = new List<ListView>() { listViewGpibDut1, listViewGpibDut2, listViewGpibDut3, listViewGpibDut4 };
            m_listReport = new List<ListView>() { listViewReportDut1, listViewReportDut2, listViewReportDut3, listViewReportDut4 };
        }

        public void InitialOutputList(Object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                EventHandler fun = new EventHandler(InitialOutputList);
                this.BeginInvoke(fun, new object[] { sender, e });
            }
            else
            {
                var args = (ButtonClickedEventArgs)e;

                m_listDebug[args.m_iDut].Items.Clear();
                m_listDut[args.m_iDut].Items.Clear();
                m_listGpib[args.m_iDut].Items.Clear();
                m_listReport[args.m_iDut].Items.Clear();

                this.Update();
            }
        }

        public void AddMsg(Object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                EventHandler fun = new EventHandler(AddMsg);
                this.BeginInvoke(fun, new object[] { sender, e });
            }
            else
            {
                var LoggedArgs = (LoggedEventArgs)e;
                ListViewItem item = new ListViewItem(LoggedArgs.m_msg);
               
                if (LoggedArgs.m_type == LOG_TYPE.GPIB || LoggedArgs.m_type == LOG_TYPE.FIX)
                {
                    m_listGpib[LoggedArgs.m_iDut].Items.Add(item);
                    m_listGpib[LoggedArgs.m_iDut].Items[m_listGpib[LoggedArgs.m_iDut].Items.Count - 1].EnsureVisible();
                }

                if (LoggedArgs.m_type == LOG_TYPE.DUT)
                {
                    m_listDut[LoggedArgs.m_iDut].Items.Add(item);
                    m_listDut[LoggedArgs.m_iDut].Items[m_listDut[LoggedArgs.m_iDut].Items.Count - 1].EnsureVisible();
                }

                if (LoggedArgs.m_type == LOG_TYPE.RPT)
                {
                    m_listReport[LoggedArgs.m_iDut].Items.Add(item);
                    m_listReport[LoggedArgs.m_iDut].Items[m_listReport[LoggedArgs.m_iDut].Items.Count - 1].EnsureVisible();
                }
                else
                {
                    m_listDebug[LoggedArgs.m_iDut].Items.Add((ListViewItem)item.Clone());
                    m_listDebug[LoggedArgs.m_iDut].Items[m_listDebug[LoggedArgs.m_iDut].Items.Count - 1].EnsureVisible();
                }

                //this.Update();
            }
        }

        private void listViewDebugDut1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listview = (ListView)sender;
            ListViewItem lstrow = listview.GetItemAt(e.X, e.Y);
            System.Windows.Forms.ListViewItem.ListViewSubItem lstcol = lstrow.GetSubItemAt(e.X, e.Y);
            string strText = lstcol.Text;
            try
            {
                Clipboard.SetDataObject(strText);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "ÌבÊ¾", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void AutoScrolltoLog(int _idut, int selectedIdx)
        {
            int idx = tabControl1.SelectedIndex;
            ListView listview = m_listDebug[idx];
            int idxCaseTotal = 0;
            foreach (ListViewItem _item in listview.Items)
            {
                if (_item.Text.IndexOf("TestCase:") >= 0 &&
                    _item.Text.IndexOf("Begin:") >= 0 &&
                    _item.Text.IndexOf(new string('-', 20)) >= 0)
                {
                    if (++idxCaseTotal == selectedIdx + 1)
                    {
                        listview.TopItem = _item;
                    }
                }
            }
        }
    }
}