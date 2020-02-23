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
    public partial class TestItemList : ToolWindow
    {
        int ScrollStop = 0;
        public TestingProgressForm m_TestProgressform = new TestingProgressForm();
        public CeaseForm m_ceaseform;
        public TestItemList()
        {
            InitializeComponent();
        }

        //测试进度条窗口
        public void InitialTestProgressForm(int _iTotalDut)
        {
            int iMultiDut = _iTotalDut;
            m_TestProgressform.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            m_TestProgressform.BackColor = System.Drawing.Color.WhiteSmoke;
            m_TestProgressform.ControlBox = false;
            m_TestProgressform.Location = new System.Drawing.Point(0, 0);
            m_TestProgressform.MaximizeBox = false;
            m_TestProgressform.MinimizeBox = false;
            m_TestProgressform.Name = "testprogressform";
            m_TestProgressform.ShowIcon = false;
            m_TestProgressform.ShowInTaskbar = false;
            m_TestProgressform.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            m_TestProgressform.Text = "TestingProgressForm";
            m_TestProgressform.TopMost = true;
            m_TestProgressform.Visible = false;
            m_TestProgressform.MinimumSize = new Size(1, 1);
            m_TestProgressform.Size = new Size(60 * iMultiDut, 190);

            //生成测试进度条
            m_TestProgressform.Initialize(iMultiDut);//测试进度图

            m_TestProgressform.Show(this);
            m_TestProgressform.Visible = false;//shawn
        }

        public void InitialTestForm(Object sender, EventArgs e)
        {
            var args = (CeaseFormInitialEventArgs)e;
            m_ceaseform = args.m_CeaseForm;

            int iMultiDut = int.Parse(args.m_dic["TotalDut"]);
            TestListView.Clear();
           
            //创建列
            var ColumnList = new List<string> { "Test Item,300", "Low,50", "Up,50", "Unit,35" };
            foreach (string item in ColumnList)
            {
                ColumnHeader column = new ColumnHeader();
                column.Text = item.Split(',')[0];
                column.Width = int.Parse(item.Split(',')[1]);
                if (ColumnList[0] != item)
                {
                    column.TextAlign = HorizontalAlignment.Center;
                }

                TestListView.Columns.Add(column);
            }

            //创建DUT列
            for (int i = 0; i < iMultiDut; i++)
            {
                ColumnHeader dutColumn = new ColumnHeader();
                dutColumn.Text = "Dut " + (i + 1).ToString() + " Result";
                dutColumn.Width = 75;
                
                dutColumn.TextAlign = HorizontalAlignment.Center;
                TestListView.Columns.Add(dutColumn);
            }

            InitialTestProgressForm(iMultiDut);
        }

        /// <summary>
        /// 初始化测试项List
        /// </summary>
        /// <returns>返回函数执行结果</returns>
        public void InitialTestItemsList(Object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                EventHandler fun = new EventHandler(InitialTestItemsList);
                this.BeginInvoke(fun, new object[] { sender, e });
            }
            else
            {
                var args = (ListFormInitialEventArgs)e;

                TestListView.Items.Clear();

                //Name:Low:Up:Unit;
                string[] testItemsArray = args.m_dic["TestItems"].Trim(';').Split(';');
                foreach (string tmpItem in testItemsArray)
                {
                    string[] itemArry = new string[TestListView.Columns.Count];
                    tmpItem.Split(':').CopyTo(itemArry, 0);
                    ListViewItem item = new ListViewItem(itemArry);
                    item.UseItemStyleForSubItems = false;
                    TestListView.Items.Add(item);
                }

                //设置进度条长度
                m_TestProgressform.SetProgressBarMaxSize(TestListView.Items.Count);
            }
        }

        public void ResetTestResultDisp(Object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                EventHandler fun = new EventHandler(ResetTestResultDisp);
                BeginInvoke(fun, new object[] { sender, e });
            }
            else
            {
                var args = (ButtonClickedEventArgs)e;
                int iDut = args.m_iDut + 1;
                for (int i = 0; i < TestListView.Items.Count; i++)
                {
                    TestListView.Items[i].SubItems[iDut + 3].Text = "";
                    TestListView.Items[i].SubItems[iDut + 3].BackColor = Color.Empty;
                    
                }

                m_TestProgressform.ResetProgressBar(args.m_iDut);
            }
        }

        delegate void UpdateTestResultCallBack(int _iDut, string itemIdx, string value, string status);
        public void UpdateTestResult(int _iDut, string itemIdx, string value, string status)
        {
            
            if (_iDut != 1)
            {
                ScrollStop = 1;
            }
            if (TestListView.InvokeRequired)
            {
                UpdateTestResultCallBack fun = new UpdateTestResultCallBack(UpdateTestResult);
                this.BeginInvoke(fun, new object[] { _iDut, itemIdx, value, status });
            }
            else
            {
                int iDut = _iDut;
                string strTestVal = "";
                if (string.IsNullOrEmpty(value.Trim()))
                {
                    strTestVal = status;
                }
                else
                {
                    strTestVal = value;
                }

                int idx = int.Parse(itemIdx);
                if (idx >= TestListView.Items.Count)
                {
                    //log.warn("itemIdx >= dataGridViewTestItems.Rows.Count in UpdateTestResult().");
                    return;
                }

                if (iDut + 4 > TestListView.Items[idx].SubItems.Count)
                {
                    //log.warn("iDut + 4 >= dataGridViewTestItems.Rows.Cells.Count in UpdateTestResult().");
                    return;
                }

                TestListView.Items[idx].SubItems[iDut + 3].Text = strTestVal;
                if (status.ToLower() == "pass")
                {
                    TestListView.Items[idx].SubItems[iDut + 3].BackColor = Color.LimeGreen;
                    m_TestProgressform.UpdateProgressBar(iDut);
                }
                else if (status.ToLower() == "fail")
                {
                    TestListView.Items[idx].SubItems[iDut + 3].BackColor = Color.Red;
                    m_TestProgressform.UpdateProgressBar(iDut);
                }
                else if (status.ToLower() == "warn")
                {
                    TestListView.Items[idx].SubItems[iDut + 3].BackColor = Color.Orange;
                    m_TestProgressform.UpdateProgressBar(iDut);
                }
                else if (status.ToLower() == "run")
                {
                    TestListView.Items[idx].SubItems[iDut + 3].BackColor = Color.RoyalBlue;
                }
                if (ScrollStop == 1)
                {
                    return;
                }
                if (iDut == 1)
                {
                    TestListView.Items[idx].EnsureVisible();
                   
                }
            }
        }

        private void TestListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int selectedIdx = TestListView.SelectedItems[0].Index;
            while (TestListView.Items[selectedIdx].Text.StartsWith(" "))
            {
                if (--selectedIdx < 0)
                {
                    return;
                }
            }

            m_ceaseform.AutoScrolltoLog(0, selectedIdx);
        }

        //AT
        delegate void SaveTestListFormImageCallBack(string dir, string SN, string testResult);
        public void SaveTestListFormImage(string dir, string SN, string testResult)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new SaveTestListFormImageCallBack(SaveTestListFormImage), new object[] { dir, SN, testResult });
            }
            else
            {
                ControlImage.SaveControlImage(this, dir, "TestListForm_" + SN + "_" + testResult + ".jpg");
            }
        }
    }
}