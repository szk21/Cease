using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Cease.Addins.Log;

using System.Xml;
using PropsBox;
using System.Runtime.InteropServices;

namespace CeaseUI
{
    public partial class ConfigForm : Form
    {
        protected InterfaceLog log;
        protected Dictionary<string, string> m_dicPara;
        private CfgParaGroupDictionary cfgParaGrpDic;
        private bool m_bSuperUserEn;

        public ConfigForm(Dictionary<string, string> _dicPara, InterfaceLog _log, bool bSuperUserEn)
        {
            m_dicPara = _dicPara;
            log = _log;
            m_bSuperUserEn = bSuperUserEn;
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            try
            {
                String myPath = Application.StartupPath + "\\Project\\" + m_dicPara["Project"] + "\\" + m_dicPara["Station"];
                Path_EditBox.Text = myPath;

                LoadXmlFile(myPath + "\\" + m_dicPara["Project"] + "_" + m_dicPara["Station"] + ".xml");

                TreeNode TotalNode = new TreeNode(m_dicPara["Project"] + ":" + m_dicPara["Station"]);
                foreach (var item in cfgParaGrpDic)
                {
                    TreeNode xNode = new TreeNode(item.Key);
                    foreach (var subItem in item.Value)
                    {
                        if (subItem.Key.Contains("#comment"))    //rocky
                            continue;

                        if (!m_bSuperUserEn && subItem.Key == "Additional")
                        {
                            continue;
                        }

                        TreeNode xxnode = new TreeNode(subItem.Key);
                        xxnode.Checked = cfgParaGrpDic[xNode.Text][subItem.Key].m_bChecked;

                        if (cfgParaGrpDic[xNode.Text][subItem.Key].m_strCategory == "CHILD")
                        {
                            xNode.Nodes.Add(xxnode);
                        } 
                        else
                        {
                            string strCategory = cfgParaGrpDic[xNode.Text][subItem.Key].m_strCategory;
                            var CateNode = xNode.Nodes.Find(strCategory, true);

                            if (CateNode.Length < 1)
                            {
                                TreeNode NewCateNode = new TreeNode(strCategory);
                                NewCateNode.Name = strCategory;
                                NewCateNode.Checked = true;

                                xNode.Nodes.Add(NewCateNode);
                                NewCateNode.Nodes.Add(xxnode);
                            }
                            else
                            {
                                CateNode[0].Nodes.Add(xxnode);
                            }

                            if (!xxnode.Checked)
                            {
                                xxnode.Parent.Checked = false;
                            }
                        }
                    }

                    TotalNode.Nodes.Add(xNode);
                }

                DutTreeView.Nodes.Clear();
                DutTreeView.Nodes.Add(TotalNode);
                DutTreeView.ExpandAll();
                DutTreeView.TopNode = DutTreeView.Nodes[0];
            }
            catch (System.Exception ex)
            {
                log.err("ConfigForm_Load Fail!", ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void DutTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node == null
                    || e.Node == DutTreeView.Nodes[0]
                    || e.Node.Level == 1
                    || e.Node.Name != "")
                {
                    propertyGrid.SelectedObject = null;
                    return;
                }

                if (e.Node.Level >= 3)
                {
                    //if (!m_bSuperUserEn)
                    //{
                    //    propertyGrid.Enabled = false;
                    //}
                    //else
                    //{
                    //    propertyGrid.Enabled = true;
                    //}
                    propertyGrid.SelectedObject = cfgParaGrpDic[e.Node.Parent.Parent.Text][e.Node.Text];
                }
                else
                {
                    //if (!m_bSuperUserEn)
                    //{
                    //    if (e.Node.Parent.Text == "InitCases" || e.Node.Parent.Text == "ExitCases" || e.Node.Parent.Text == "TestCases")
                    //    {
                    //        propertyGrid.Enabled = false;
                    //    }
                    //    else
                    //    {
                    //        propertyGrid.Enabled = true;
                    //    }
                    //}  
                    propertyGrid.SelectedObject = cfgParaGrpDic[e.Node.Parent.Text][e.Node.Text];
                }
                if (m_bSuperUserEn)
                {
                    propertyGrid.Enabled = true;
                }
                else
                {
                    if (e.Node.Parent.Text == "InitCases" || e.Node.Parent.Text == "ExitCases" || e.Node.Parent.Text == "TestCases" || e.Node.Level >= 3)
                    {
                        propertyGrid.Enabled = false;
                    }
                    else
                    {
                        propertyGrid.Enabled = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                log.err("DutTreeView_AfterSelect Fail!", ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void DutTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                
                if (e.Action == TreeViewAction.Unknown)
                {
                    return;
                }

                if (!m_bSuperUserEn)
                {
                    if (e.Node.Level >= 3 || e.Node.Parent.Text == "TestCases" || e.Node.Parent.Text == "InitCases" || e.Node.Parent.Text == "ExitCases")
                    {
                        return;
                    }
                }

                if (e.Node.Level == 3)
                {
                    cfgParaGrpDic[e.Node.Parent.Parent.Text][e.Node.Text].m_bChecked = e.Node.Checked;
                    if (e.Node.Checked)
                    {
                        foreach (TreeNode tn in e.Node.Parent.Nodes)
                        {
                            if (!tn.Checked)
                            {
                                return;
                            }
                        }
                    }

                    e.Node.Parent.Checked = e.Node.Checked;
                }
                else if (e.Node.Level == 2 && e.Node.Nodes.Count > 0)
                {
                    foreach (TreeNode tn in e.Node.Nodes)
                    {
                        tn.Checked = e.Node.Checked;
                        cfgParaGrpDic[e.Node.Parent.Text][tn.Text].m_bChecked = e.Node.Checked;
                    }
                }
                else
                {
                    cfgParaGrpDic[e.Node.Parent.Text][e.Node.Text].m_bChecked = e.Node.Checked;
                }
            }
            catch (System.Exception ex)
            {
                log.err("DutTreeView_AfterCheck Fail!", ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadXmlFile(string FileName)
        {
            cfgParaGrpDic = new CfgParaGroupDictionary(FileName);
            cfgParaGrpDic.LoadPara();
        }

        private void Common_Save_Btn_Click(object sender, EventArgs e)
        {
            cfgParaGrpDic.SaveToXml();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            cfgParaGrpDic.SaveToXml();

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void tvTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            try
            {
                if (e.Node == null)
                {
                    return;
                }
                //隐藏节点前的checkbox
                if (e.Node.Level < 2 ||
                    (e.Node.Level == 2 && e.Node.Parent.Text != "InitCases" && e.Node.Parent.Text != "TestCases" && e.Node.Parent.Text != "ExitCases"))
                {
                    HideCheckBox(this.DutTreeView, e.Node);
                }
                if (!m_bSuperUserEn)
                {
                    if (e.Node.Parent.Text == "InitCases" || e.Node.Parent.Text == "ExitCases" || e.Node.Parent.Text == "TestCases" || e.Node.Level >= 3)
                    {
                        e.Node.ForeColor = Color.Gray;
                    }
                }
            }
            catch (Exception ex)
            {
                ;
            }
            e.DrawDefault = true;
        }

        private const int TVIF_STATE = 0x8;
        private const int TVIS_STATEIMAGEMASK = 0xF000;
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETITEM = TV_FIRST + 63;
        private void HideCheckBox(TreeView tvw, TreeNode node)
        {
            TVITEM tvi = new TVITEM();
            tvi.hItem = node.Handle;
            tvi.mask = TVIF_STATE;
            tvi.stateMask = TVIS_STATEIMAGEMASK;
            tvi.state = 0;
            SendMessage(tvw.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        private struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage; public int cChildren; public IntPtr lParam;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref TVITEM lParam);

    }
}
