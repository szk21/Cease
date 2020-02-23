using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace CeaseUI
{
    public partial class CurvesForm : ToolWindow
    {
        public CurvesForm()
        {
            InitializeComponent();
            _dicLoopPage.Clear();
            _dicLoopPage.Add("tabPage_EarMic_Spk", tabPage_EarMic_Spk);
            _dicLoopPage.Add("tabPage_EarMic_Rev", tabPage_EarMic_Rev);
            _dicLoopPage.Add("tabPage_MainMic_EarRevL", tabPage_MainMic_EarRevL);
            _dicLoopPage.Add("tabPage_SecMic_EarRevR", tabPage_SecMic_EarRevR);
            _dicLoopPage.Add("tabPage_ThrMic_EarRevL", tabPage_ThrMic_EarRevL);

            _dicItemPage.Clear();
            _dicItemPage.Add("tabPage_EarMic_Spk_AMPL", tabPage_EarMic_Spk_AMPL);
            _dicItemPage.Add("tabPage_EarMic_Spk_THD", tabPage_EarMic_Spk_THD);
            _dicItemPage.Add("tabPage_EarMic_Spk_RB", tabPage_EarMic_Spk_RB);
            _dicItemPage.Add("tabPage_EarMic_Rev_AMPL", tabPage_EarMic_Rev_AMPL);
            _dicItemPage.Add("tabPage_EarMic_Rev_THD", tabPage_EarMic_Rev_THD);
            _dicItemPage.Add("tabPage_EarMic_Rev_RB", tabPage_EarMic_Rev_RB);
            _dicItemPage.Add("tabPage_MainMic_EarRevL_AMPL", tabPage_MainMic_EarRevL_AMPL);
            _dicItemPage.Add("tabPage_MainMic_EarRevL_THD", tabPage_MainMic_EarRevL_THD);
            _dicItemPage.Add("tabPage_MainMic_EarRevL_RB", tabPage_MainMic_EarRevL_RB);
            _dicItemPage.Add("tabPage_SecMic_EarRevR_AMPL", tabPage_SecMic_EarRevR_AMPL);
            _dicItemPage.Add("tabPage_SecMic_EarRevR_THD", tabPage_SecMic_EarRevR_THD);
            _dicItemPage.Add("tabPage_SecMic_EarRevR_RB", tabPage_SecMic_EarRevR_RB);
            _dicItemPage.Add("tabPage_ThrMic_EarRevL_AMPL", tabPage_ThrMic_EarRevL_AMPL);
            _dicItemPage.Add("tabPage_ThrMic_EarRevL_THD", tabPage_ThrMic_EarRevL_THD);
            _dicItemPage.Add("tabPage_ThrMic_EarRevL_RB", tabPage_ThrMic_EarRevL_RB);

            InitialTabColor();

            _dicLoopTab.Clear();
            _dicLoopTab.Add("tabControl_EarMic_Spk", tabControl_EarMic_Spk);
            _dicLoopTab.Add("tabControl_EarMic_Rev", tabControl_EarMic_Rev);
            _dicLoopTab.Add("tabControl_MainMic_EarRevL", tabControl_MainMic_EarRevL);
            _dicLoopTab.Add("tabControl_SecMic_EarRevR", tabControl_SecMic_EarRevR);
            _dicLoopTab.Add("tabControl_ThrMic_EarRevL", tabControl_ThrMic_EarRevL);

            _dicPanel.Clear();
            _dicPanel.Add("panel_EarMic_Spk", panel_EarMic_Spk);
            _dicPanel.Add("panel_EarMic_Rev", panel_EarMic_Rev);
            _dicPanel.Add("panel_MainMic_EarRevL", panel_MainMic_EarRevL);
            _dicPanel.Add("panel_SecMic_EarRevR", panel_SecMic_EarRevR);
            _dicPanel.Add("panel_ThrMic_EarRevL", panel_ThrMic_EarRevL);

            _dicCurve.Clear();
            _dicCurve.Add("Curve_EarMic_Spk_AMPL", Curve_EarMic_Spk_AMPL);
            _dicCurve.Add("Curve_EarMic_Spk_THD", Curve_EarMic_Spk_THD);
            _dicCurve.Add("Curve_EarMic_Spk_RB", Curve_EarMic_Spk_RB);
            _dicCurve.Add("Curve_EarMic_Rev_AMPL", Curve_EarMic_Rev_AMPL);
            _dicCurve.Add("Curve_EarMic_Rev_THD", Curve_EarMic_Rev_THD);
            _dicCurve.Add("Curve_EarMic_Rev_RB", Curve_EarMic_Rev_RB);
            _dicCurve.Add("Curve_MainMic_EarRevL_AMPL", Curve_MainMic_EarRevL_AMPL);
            _dicCurve.Add("Curve_MainMic_EarRevL_THD", Curve_MainMic_EarRevL_THD);
            _dicCurve.Add("Curve_MainMic_EarRevL_RB", Curve_MainMic_EarRevL_RB);
            _dicCurve.Add("Curve_SecMic_EarRevR_AMPL", Curve_SecMic_EarRevR_AMPL);
            _dicCurve.Add("Curve_SecMic_EarRevR_THD", Curve_SecMic_EarRevR_THD);
            _dicCurve.Add("Curve_SecMic_EarRevR_RB", Curve_SecMic_EarRevR_RB);
            _dicCurve.Add("Curve_ThrMic_EarRevL_AMPL", Curve_ThrMic_EarRevL_AMPL);
            _dicCurve.Add("Curve_ThrMic_EarRevL_THD", Curve_ThrMic_EarRevL_THD);
            _dicCurve.Add("Curve_ThrMic_EarRevL_RB", Curve_ThrMic_EarRevL_RB);



            CurveTab.DrawMode = TabDrawMode.OwnerDrawFixed;
            CurveTab.DrawItem += new DrawItemEventHandler(TabctrolAttribute_DrawItem_Loop);

            tabControl_EarMic_Spk.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl_EarMic_Spk.DrawItem += new DrawItemEventHandler(TabctrolAttribute_DrawItem_Item);
            tabControl_EarMic_Rev.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl_EarMic_Rev.DrawItem += new DrawItemEventHandler(TabctrolAttribute_DrawItem_Item);
            tabControl_MainMic_EarRevL.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl_MainMic_EarRevL.DrawItem += new DrawItemEventHandler(TabctrolAttribute_DrawItem_Item);
            tabControl_SecMic_EarRevR.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl_SecMic_EarRevR.DrawItem += new DrawItemEventHandler(TabctrolAttribute_DrawItem_Item);
            tabControl_ThrMic_EarRevL.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl_ThrMic_EarRevL.DrawItem += new DrawItemEventHandler(TabctrolAttribute_DrawItem_Item);


        }

        private void InitialTabColor()
        {
            _dicLoopTabColor = new Dictionary<string, Color>();
            _dicLoopTabColor.Add("tabPage_EarMic_Spk", Color.Gray);
            _dicLoopTabColor.Add("tabPage_EarMic_Rev", Color.LightGray);
            _dicLoopTabColor.Add("tabPage_MainMic_EarRevL", Color.Gray);
            _dicLoopTabColor.Add("tabPage_SecMic_EarRevR", Color.LightGray);
            _dicLoopTabColor.Add("tabPage_ThrMic_EarRevL", Color.Gray);

            _dicItemTabColor = new Dictionary<string, Color>();
            _dicItemTabColor.Add("tabPage_EarMic_Spk_AMPL", Color.Gray);
            _dicItemTabColor.Add("tabPage_EarMic_Spk_THD", Color.LightGray);
            _dicItemTabColor.Add("tabPage_EarMic_Spk_RB", Color.Gray);
            _dicItemTabColor.Add("tabPage_EarMic_Rev_AMPL", Color.Gray);
            _dicItemTabColor.Add("tabPage_EarMic_Rev_THD", Color.LightGray);
            _dicItemTabColor.Add("tabPage_EarMic_Rev_RB", Color.Gray);
            _dicItemTabColor.Add("tabPage_MainMic_EarRevL_AMPL", Color.Gray);
            _dicItemTabColor.Add("tabPage_MainMic_EarRevL_THD", Color.LightGray);
            _dicItemTabColor.Add("tabPage_MainMic_EarRevL_RB", Color.Gray);
            _dicItemTabColor.Add("tabPage_SecMic_EarRevR_AMPL", Color.Gray);
            _dicItemTabColor.Add("tabPage_SecMic_EarRevR_THD", Color.LightGray);
            _dicItemTabColor.Add("tabPage_SecMic_EarRevR_RB", Color.Gray);
            _dicItemTabColor.Add("tabPage_ThrMic_EarRevL_AMPL", Color.Gray);
            _dicItemTabColor.Add("tabPage_ThrMic_EarRevL_THD", Color.LightGray);
            _dicItemTabColor.Add("tabPage_ThrMic_EarRevL_RB", Color.Gray);

        }

        private void ClearTabColor()
        {
            InitialTabColor();
            CurveTab.Invalidate();
            foreach (string key in _dicLoopTab.Keys)
            {
                _dicLoopTab[key].Invalidate();
                _dicLoopTab[key].SelectedIndex = 0;
            }

            CurveTab.SelectedIndex = 0;
            
        }

        private void ClearAllCurves()
        {
            foreach (string key in _dicCurve.Keys)
            {
                _dicCurve[key].LineValueAll = null;
                _dicCurve[key].Title = "";
            }
        }

        public void addPage(string[] keys)
        {
            
            hocylan_Curve.hocy_Curve cur = new hocylan_Curve.hocy_Curve();
            cur.Keys = keys;
            
            TabPage tp=new TabPage();
            tp.Controls.Add(cur);
            CurveTab.SelectTab(2);
            this.Invalidate();
        }

        delegate void HideAllPagesCallBack();
        public void HideAllPages()
        {
            if (this.InvokeRequired)
            {
                HideAllPagesCallBack fun = new HideAllPagesCallBack(HideAllPages);
                BeginInvoke(fun, new object[] { });
            }
            else
            {
                ClearTabColor();
                ClearAllCurves();
            }
        }

        delegate void ShowFormCallBack();
        public void ShowCurveForm()
        {
            if (this.InvokeRequired)
            {
                ShowFormCallBack fun = new ShowFormCallBack(ShowCurveForm);
                BeginInvoke(fun, new object[] { });
            }
            else
            {
                this.Show();
            }
        }

        public void SavePanelImage(string panelName, string savePath)
        {
            ControlImage.GetPanel(_dicPanel["panel_" + panelName]).Save(savePath);
        }

        delegate void SavePanelsImageCallBack(string dir, string sn);
        public void SavePanelsImage(string dir,string sn)
        {
            if (this.InvokeRequired)
            {
                SavePanelsImageCallBack fun = new SavePanelsImageCallBack(SavePanelsImage);
                BeginInvoke(fun, new object[] { dir,sn });
            }
            else
            {

                    //if (_dicLoopPage[page].Parent == this.CurveTab)
                    //{
                    //    CurveTab.SelectedTab = _dicLoopPage[page];
                    //    string cur = "Curve_" + page.Replace("tabPage_", "")+"_AMPL";
                    //    if (_dicCurve[cur].Title.Contains(sn))
                    //    {
                    //        ControlImage.SaveControlImage(_dicLoopPage[page], dir, page.Replace("tabPage_", "")+"_"+sn+ ".jpg");
                    //    }
                    //}

                    foreach (string curve in _dicCurve.Keys)
                    {
                        if (_dicCurve[curve].Title.Contains(sn))
                        {
                            string loopName = curve.Replace("_AMPL", "").Replace("_THD", "").Replace("_RB", "");
                            loopName = loopName.Replace("Curve_", "");
                            CurveTab.SelectedTab = _dicLoopPage["tabPage_"+loopName];
                            string itemName = curve.Replace("Curve_", "tabPage_");
                            _dicLoopTab["tabControl_" + loopName].SelectedTab = _dicItemPage[itemName];
                            ControlImage.SaveControlImage(_dicCurve[curve], dir, _dicCurve[curve].Title.Replace(":", "_") + ".jpg");
                        }
                        Thread.Sleep(50);
                    }

                    foreach (string key in _dicLoopTabColor.Keys)
                    {
                        if (_dicLoopTabColor[key] == Color.Red)
                        {
                            CurveTab.SelectedTab = _dicLoopPage[key];
                            break;
                        }
                    }

                    foreach (string key in _dicItemTabColor.Keys)
                    {
                        if (_dicItemTabColor[key] == Color.Red)
                        {
                            string loop = key.Replace("_AMPL", "").Replace("_THD", "").Replace("_RB", "").Replace("tabPage_", "");
                            string item = key.Replace("tabPage_", "");
                            _dicLoopTab["tabControl_" + loop].SelectedTab = _dicItemPage[key];
                            break;
                        }
                    }

            }
        }

        delegate void CurvesTabInvalidateCallBack();
        public void CurvesTabInvalidate()
        {
            if (this.InvokeRequired)
            {
                CurvesTabInvalidateCallBack fun = new CurvesTabInvalidateCallBack(CurvesTabInvalidate);
                BeginInvoke(fun, new object[] { });
            }
            else
            {
                CurveTab.Invalidate();
            }
        }

        delegate void ShowPageCallBack(string pageName, string SN, double[] freq, double[] AMPL, double[] AMPL_USL, double[] AMPL_LSL,
            double[] THD, double[] THD_USL, double[] THD_LSL,
            double[] RB, double[] RB_USL, double[] RB_LSL, double[] AMPL_REF, Dictionary<string, bool> dicLoopResult, Dictionary<string, bool> dicItemResult, double[] AMPL2, double[] THD2, double[] RB2);
        public void ShowPage(string pageName, string SN, double[] freq, double[] AMPL, double[] AMPL_USL, double[] AMPL_LSL,
            double[] THD, double[] THD_USL, double[] THD_LSL,
            double[] RB, double[] RB_USL, double[] RB_LSL, double[] AMPL_REF, Dictionary<string, bool> dicLoopResult, Dictionary<string, bool> dicItemResult,double[] AMPL2,double[] THD2,double[] RB2)
        {
            if (this.InvokeRequired)
            {
                ShowPageCallBack fun = new ShowPageCallBack(ShowPage);
                BeginInvoke(fun, new object[] { pageName, SN, freq, AMPL, AMPL_USL, AMPL_LSL, THD, THD_USL, THD_LSL, RB, RB_USL, RB_LSL, AMPL_REF, dicLoopResult, dicItemResult, AMPL2, THD2, RB2 });
            }
            else
            {
            List<string> XAxisKeys = new List<string>();
            foreach (double s in freq)
            {
                if (s ==300)
                {
                    XAxisKeys.Add("300");
                }
                else if (s == 400)
                {
                    XAxisKeys.Add("400");
                }
                else if (s== 500)
                {
                    XAxisKeys.Add("500");
                }
                else if (s == 1000)
                {
                    XAxisKeys.Add("1000");
                }
                else if (s== 5000)
                {
                    XAxisKeys.Add("5000");
                }
                else if (s== 8000)
                {
                    XAxisKeys.Add("8000");
                }
                else
                {
                    XAxisKeys.Add("");
                }
            }

            _dicCurve["Curve_" + pageName + "_AMPL"].Keys = XAxisKeys.ToArray();
            _dicCurve["Curve_" + pageName + "_THD"].Keys = XAxisKeys.ToArray();
            _dicCurve["Curve_" + pageName + "_RB"].Keys = XAxisKeys.ToArray();

            List<float> value = new List<float>();
            List<float> value2 = new List<float>();
            List<float> usl = new List<float>();
            List<float> lsl = new List<float>();
            List<float> AmplREF = new List<float>();

            double MaxAMPLValue = -999;
            double MinAMPLValue = 999;
            for (int i = 0; i < AMPL.Length; i++)
            {
                if (AMPL[i].ToString() == "NaN" || AMPL[i].ToString()=="非数字")
                {
                    AMPL[i] = 0;
                }
                if (AMPL[i] > MaxAMPLValue)
                {
                    MaxAMPLValue = AMPL[i];
                }
                if (AMPL[i] < MinAMPLValue)
                {
                    MinAMPLValue = AMPL[i];
                }

                value.Add((float)Math.Round(AMPL[i], 2));
                value2.Add((float)Math.Round(AMPL2[i], 2));
            }
            foreach (double d in AMPL_USL)
            {
                if (d > MaxAMPLValue)
                {
                    MaxAMPLValue = d;
                }
                if (d < MinAMPLValue)
                {
                    MinAMPLValue = d;
                }
                usl.Add((float)Math.Round(d, 2));
            }
            foreach (double d in AMPL_LSL)
            {
                if (d > MaxAMPLValue)
                {
                    MaxAMPLValue = d;
                }
                if (d < MinAMPLValue)
                {
                    MinAMPLValue = d;
                }
                lsl.Add((float)Math.Round(d, 2));
            }

            foreach (double d in AMPL_REF)
            {
                AmplREF.Add((float)Math.Round(d, 2));
            }
            float[][] m_lineValue = new float[][] { value.ToArray(), value2.ToArray(), usl.ToArray(), lsl.ToArray(), AmplREF.ToArray() };
            _dicCurve["Curve_" + pageName + "_AMPL"].LineValueAll = m_lineValue;
            Color[] colors = new Color[] { Color.Green, Color.Blue, Color.Red, Color.Red, Color.Black };
            _dicCurve["Curve_" + pageName + "_AMPL"].LineColor = colors;

            value = new List<float>();
            value2 = new List<float>();
            usl = new List<float>();
            lsl = new List<float>();
            double MaxTHDValue = -999;
            double MinTHDValue = 999;
            foreach (double d in THD)
            {
                if (d > MaxTHDValue)
                {
                    MaxTHDValue = d;
                }
                if (d < MinTHDValue)
                {
                    MinTHDValue = d;
                }
                value.Add((float)Math.Round(d, 2));
            }
            foreach (double d in THD2)
            {
                value2.Add((float)Math.Round(d, 2));
            }
            foreach (double d in THD_USL)
            {
                if (d > MaxTHDValue)
                {
                    MaxTHDValue = d;
                }
                if (d < MinTHDValue)
                {
                    MinTHDValue = d;
                }
                usl.Add((float)Math.Round(d, 2));
            }
            foreach (double d in THD_LSL)
            {
                if (d > MaxTHDValue)
                {
                    MaxTHDValue = d;
                }
                if (d < MinTHDValue)
                {
                    MinTHDValue = d;
                }
                lsl.Add((float)Math.Round(d, 2));
            }
            m_lineValue = new float[][] { value.ToArray(), value2.ToArray(), usl.ToArray(), lsl.ToArray() };
            colors = new Color[] { Color.Green, Color.Blue, Color.Red, Color.Red };
            _dicCurve["Curve_" + pageName + "_THD"].LineValueAll = m_lineValue;
            _dicCurve["Curve_" + pageName + "_THD"].LineColor = colors;

            value = new List<float>();
            value2 = new List<float>();
            usl = new List<float>();
            lsl = new List<float>();
            double MaxRBValue = -999;
            double MinRBValue = 999;
            foreach (double d in RB)
            {
                if (d > MaxRBValue)
                {
                    MaxRBValue = d;
                }
                if (d < MinRBValue)
                {
                    MinRBValue = d;
                }
                value.Add((float)Math.Round(d, 2));
            }
            foreach (double d in RB)
            {
                value2.Add((float)Math.Round(d, 2));
            }
            foreach (double d in RB_USL)
            {
                if (d > MaxRBValue)
                {
                    MaxRBValue = d;
                }
                if (d < MinRBValue)
                {
                    MinRBValue = d;
                }
                usl.Add((float)Math.Round(d, 2));
            }
            foreach (double d in RB_LSL)
            {
                if (d > MaxRBValue)
                {
                    MaxRBValue = d;
                }
                if (d < MinRBValue)
                {
                    MinRBValue = d;
                }
                lsl.Add((float)Math.Round(d, 2));
            }
            m_lineValue = new float[][] { value.ToArray(), value2.ToArray(), usl.ToArray(), lsl.ToArray() };
            _dicCurve["Curve_" + pageName + "_RB"].LineValueAll = m_lineValue;
            _dicCurve["Curve_" + pageName + "_RB"].LineColor = colors;

            int DecimalNumber_AMPL = 0;
            int DecimalNumber_THD = 0;
            int DecimalNumber_RB = 0;
            if (Math.Abs(MaxAMPLValue) > 20)
            {
                DecimalNumber_AMPL = 0;
            }
            else
            {
                DecimalNumber_AMPL = 1;
            }

            if (Math.Abs(MaxTHDValue) > 20)
            {
                DecimalNumber_THD = 0;
            }
            else
            {
                DecimalNumber_THD = 1;
            }
            if (Math.Abs(MaxRBValue) > 20)
            {
                DecimalNumber_RB = 0;
            }
            else
            {
                DecimalNumber_RB = 1;
            }
            _dicCurve["Curve_" + pageName + "_AMPL"].XPointScaleNum = value.Count;
            _dicCurve["Curve_" + pageName + "_THD"].XPointScaleNum = value.Count;
            _dicCurve["Curve_" + pageName + "_RB"].XPointScaleNum = value.Count;

            _dicCurve["Curve_" + pageName + "_AMPL"].YSliceValue = (float)Math.Round(((MaxAMPLValue - MinAMPLValue) / 6), DecimalNumber_AMPL);
            _dicCurve["Curve_" + pageName + "_THD"].YSliceValue = (float)Math.Round(((MaxTHDValue - MinTHDValue) / 6), DecimalNumber_THD);
            _dicCurve["Curve_" + pageName + "_RB"].YSliceValue = (float)Math.Round(((MaxRBValue - MinRBValue) / 6), DecimalNumber_RB);

            _dicCurve["Curve_" + pageName + "_AMPL"].YSliceBegin = (float)Math.Round((float)(MinAMPLValue), DecimalNumber_AMPL);
            _dicCurve["Curve_" + pageName + "_THD"].YSliceBegin = (float)Math.Round((float)(MinTHDValue), DecimalNumber_THD);
            _dicCurve["Curve_" + pageName + "_RB"].YSliceBegin = (float)Math.Round((float)(MinRBValue), DecimalNumber_RB);

            _dicCurve["Curve_" + pageName + "_AMPL"].YSliceEnd = (float)Math.Round((float)(MaxAMPLValue), DecimalNumber_AMPL);
            _dicCurve["Curve_" + pageName + "_THD"].YSliceEnd = (float)Math.Round((float)(MaxTHDValue), DecimalNumber_THD);
            _dicCurve["Curve_" + pageName + "_RB"].YSliceEnd = (float)Math.Round((float)(MaxRBValue), DecimalNumber_RB);

            _dicCurve["Curve_" + pageName + "_AMPL"].Width = 10 * value.Count;
            _dicCurve["Curve_" + pageName + "_THD"].Width = 10 * value.Count;
            _dicCurve["Curve_" + pageName + "_RB"].Width = 10 * value.Count;

            _dicCurve["Curve_" + pageName + "_AMPL"].Title = pageName+"_AMPL_SN:"+SN;
            _dicCurve["Curve_" + pageName + "_THD"].Title = pageName + "_THD_SN:" + SN;
            _dicCurve["Curve_" + pageName + "_RB"].Title = pageName + "_RB_SN:" + SN;


            _dicCurve["Curve_" + pageName + "_AMPL"].Invalidate();
            _dicCurve["Curve_" + pageName + "_THD"].Invalidate();
            _dicCurve["Curve_" + pageName + "_RB"].Invalidate();

            if (_dicLoopPage["tabPage_" + pageName].Parent == null)
            {
                _dicLoopPage["tabPage_" + pageName].Parent = this.CurveTab;
            }

            this.CurveTab.SelectTab("tabPage_" + pageName);

            foreach (string key in dicLoopResult.Keys)
            {
                if (dicLoopResult[key])
                {
                    _dicLoopTabColor["tabPage_"+key]= Color.Green;
                }
                else
                {
                    _dicLoopTabColor["tabPage_" + key] = Color.Red;
                }
            }
            CurveTab.Invalidate();

            foreach (string key in dicItemResult.Keys)
            {
                if (dicItemResult[key])
                {
                    _dicItemTabColor["tabPage_" +pageName+"_"+ key] = Color.Green;
                }
                else
                {
                    _dicItemTabColor["tabPage_" + pageName + "_" + key] = Color.Red;
                }

            }
            _dicLoopTab["tabControl_" + pageName].Invalidate();
        }
        }



        private Dictionary<string, TabPage> _dicLoopPage = new Dictionary<string, TabPage>();
        private Dictionary<string, TabPage> _dicItemPage = new Dictionary<string, TabPage>();
        private Dictionary<string, TabControl> _dicLoopTab = new Dictionary<string, TabControl>();
        private Dictionary<string, hocylan_Curve.hocy_Curve> _dicCurve = new Dictionary<string, hocylan_Curve.hocy_Curve>();
        private Dictionary<string, Panel> _dicPanel = new Dictionary<string, Panel>();
        public Dictionary<string, Color> _dicLoopTabColor;
        public Dictionary<string, Color> _dicItemTabColor;

        private void button1_Click(object sender, EventArgs e)
        {
            HideAllPages();
        }

        private void Curve_MainMic_EarRevL_AMPL_THD_Load(object sender, EventArgs e)
        {

        }

        private void CurvesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void Curve_ThrMic_EarRevL_THD_Load(object sender, EventArgs e)
        {

        }


        void TabctrolAttribute_DrawItem_Loop(object sender, DrawItemEventArgs e)
        {
            TabControl _tabcontrol = (TabControl)sender;
            Graphics g = e.Graphics;
            if (_dicLoopTabColor != null)
            {
                for (int i = 0; i < _tabcontrol.TabCount; i++)
                {
                    try
                    {

                        //标签背景填充颜色
                        SolidBrush BackBrush = new SolidBrush(_dicLoopTabColor[_tabcontrol.TabPages[i].Name]);
                        //标签文字填充颜色
                        SolidBrush FrontBrush = new SolidBrush(Color.Black);
                        StringFormat StringF = new StringFormat();
                        //设置文字对齐方式
                        StringF.Alignment = StringAlignment.Center;
                        StringF.LineAlignment = StringAlignment.Center;
                        //获取标签头工作区域
                        Rectangle Rec = CurveTab.GetTabRect(i);
                        //绘制标签头背景颜色
                        e.Graphics.FillRectangle(BackBrush, Rec);
                        //绘制标签头文字
                        e.Graphics.DrawString(_tabcontrol.TabPages[i].Text
                                           , _tabcontrol.Font, new SolidBrush(Color.Black), Rec, StringF);
                    }
                    catch
                    { 
                        
                    }
                }
            }

        }
        void TabctrolAttribute_DrawItem_Item(object sender, DrawItemEventArgs e)
        {
            TabControl _tabcontrol = (TabControl)sender;
            Graphics g = e.Graphics;
            if (_dicLoopTabColor != null)
            {
                for (int i = 0; i < _tabcontrol.TabCount; i++)
                {

                    try
                    {
                        //标签背景填充颜色
                        SolidBrush BackBrush = new SolidBrush(_dicItemTabColor[_tabcontrol.TabPages[i].Name]);
                        //标签文字填充颜色
                        SolidBrush FrontBrush = new SolidBrush(Color.Black);
                        StringFormat StringF = new StringFormat();
                        //设置文字对齐方式
                        StringF.Alignment = StringAlignment.Center;
                        StringF.LineAlignment = StringAlignment.Center;
                        //获取标签头工作区域
                        Rectangle Rec = CurveTab.GetTabRect(i);
                        //绘制标签头背景颜色
                        e.Graphics.FillRectangle(BackBrush, Rec);
                        //绘制标签头文字
                        e.Graphics.DrawString(_tabcontrol.TabPages[i].Text
                                           , _tabcontrol.Font, new SolidBrush(Color.Black), Rec, StringF);
                    }
                    catch
                    { }
                }
            }

        }
    }
}
