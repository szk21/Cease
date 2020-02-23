using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Props;
using Microsoft.Win32;
using System.IO.Ports;
using System.Linq;

namespace PropsBox
{
    public class FilePathEditor : UITypeEditor
    {
        public int Mode;
        public  String DefPath;
        //////////////////////////////////////////////////////////////////////////
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            // UITypeEditorEditStyle有三种，Modal是弹出式，DropDown是下拉式，None是没有。 
            return UITypeEditorEditStyle.Modal;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            // 得到editor service，可由其创建弹出窗口。 
            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            // context.Instance —— 可以得到当前的Demo3对象。 
            // ((Demo3)context.Instance).Grade —— 可以得到当前Grade的值。 
            int x = Mode;
            string strCurrentDir = Application.StartupPath;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = DefPath;
            dialog.Filter = "All files (*.*)|*.*|(*.txt)|*.txt|(*.xml)|*.xml|(*.xtt)|*.xtt";
            dialog.RestoreDirectory = true;
            dialog.InitialDirectory=Application.StartupPath;
            dialog.ShowDialog();
            String grade = dialog.FileName;
            dialog.Dispose();

            return grade;
        }
    }

    public class FolderEditor : UITypeEditor
    {
        public int Mode;
        public String DefPath;
        //////////////////////////////////////////////////////////////////////////
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            // UITypeEditorEditStyle有三种，Modal是弹出式，DropDown是下拉式，None是没有。 
            return UITypeEditorEditStyle.Modal;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            // 得到editor service，可由其创建弹出窗口。 
            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            // context.Instance —— 可以得到当前的Demo3对象。 
            // ((Demo3)context.Instance).Grade —— 可以得到当前Grade的值。 
            int x = Mode;
            string strCurrentDir = DefPath;
            String grade = DefPath;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = DefPath;
            DialogResult res = dialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                grade = dialog.SelectedPath;
            }

            dialog.Dispose();

            return grade;
        }
    }

    public class VISASourceBoxEditor : UITypeEditor
    {
        public int Mode;
        public String DefSource;
        //////////////////////////////////////////////////////////////////////////
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            // UITypeEditorEditStyle有三种，Modal是弹出式，DropDown是下拉式，None是没有。 
            return UITypeEditorEditStyle.Modal;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            // 得到editor service，可由其创建弹出窗口。 
            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            // context.Instance —— 可以得到当前的Demo3对象。 
            // ((Demo3)context.Instance).Grade —— 可以得到当前Grade的值。 
            int x = Mode;
            frmVISACfg dialog = new frmVISACfg(DefSource);
            editorService.ShowDialog(dialog);
            String grade = dialog.sVISASource;
            dialog.Dispose();

            return grade;
        }
    }

    public class FileNameConverter : System.ComponentModel.StringConverter
    {
        /// <summary>
        /// 根据返回值确定是否支持下拉框的形式
        /// </summary>
        /// <returns>
        /// true: 下来框的形式
        /// false: 普通文本编辑的形式
        /// </returns>

        public string[] sOption;

        public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }
        /// <summary>
        /// 下拉框中具体的内容
        /// </summary>
        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(sOption);//new string[] { "File1.bat", "File2.exe", "File3.dll" });
        }
        /// <summary>
        /// 根据返回值确定是否是不可编辑的文本框
        /// </summary>
        /// <returns>
        /// true: 文本框不可以编辑
        /// flase: 文本框可以编辑
        /// </returns>
        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }
    }
    
    public class CfgParaDictionary : Dictionary<string, XProps>
    {
        protected void LoadTestCaseNode(XmlNode paraGrpNode, int _idx)
        {
            if (paraGrpNode.Attributes.Count == 0)
            {
                return;
            }

            string strGrpNode = ((paraGrpNode.Attributes["paraName"] != null) ? 
                paraGrpNode.Attributes["paraName"].Value : paraGrpNode.Name)
                + "@" + _idx.ToString();

            var attDesc = paraGrpNode.Attributes["Description"];
            string strDesc = strGrpNode + (attDesc != null ? ": " + attDesc.Value : "");
            foreach (XmlAttribute att in paraGrpNode.Attributes)
            {
                if (att.Name == "Category" && att.Value.Trim() != "")
                {
                    this[strGrpNode].m_strCategory = att.Value;
                }

                if (!att.Name.StartsWith("para") || att.Name == "paraName")
                {
                    continue;
                }

                if (att.Name == "paraChecked")
                {
                    this[strGrpNode].m_bChecked = att.Value == "FALSE" ? false : true;
                    continue;
                }

                if (att.Name == "paraLimit" && paraGrpNode.Name.StartsWith("Suit"))
                {
                    foreach (var items in att.Value.Split(';'))
                    {
                        if (items == "")
                        {
                            continue;
                        }

                        this[strGrpNode].Add(new XProp(items.Split(':')[0] + "@" + items.Split(':')[3], items.Split(':')[1] + ":" + items.Split(':')[2], strDesc));
                    }
                }
                else
                {
                    this[strGrpNode].Add(new XProp(att.Name, att.Value, strDesc));
                }
            }
        }

        protected void LoadParaNode(XmlNode paraGrpNode)
        {
            foreach (XmlNode paraNode in paraGrpNode.ChildNodes)
            {
                if (paraNode.Attributes.Count == 0)
                {
                    continue;
                }

                XProp xProp = new XProp();

                bool speciType = false;
                string OptionArray = null;
                foreach (XmlAttribute att in paraNode.Attributes)
                {
                    if (att.Name.StartsWith("para"))
                    {
                        xProp.Name = att.Name;
                        xProp.Value = att.Value;
                        if (att.Value.ToString().ToUpper() == "TRUE")
                        {
                            xProp.Value = true;
                        }
                        else if (att.Value.ToString().ToUpper() == "FALSE")
                        {
                            xProp.Value = false;
                        }
                    }
                    else
                    {
                        switch (att.Name)
                        {
                            case "Type":
                                xProp.EditText = att.Value;
                                speciType = true;
                                break;
                            case "Enable":
                                xProp.IsReadOnly = att.Value;
                                break;
                            case "Description":
                                xProp.Description = att.Value;
                                break;
                            case "Options":
                                OptionArray = att.Value;
                                break;
                            case "Category":
                                xProp.Category = att.Value;
                                break;
                        }
                    }
                }

                if (speciType)
                {
                    xPropConfig(xProp, OptionArray);
                }

                this[paraGrpNode.Name].Add(xProp);
            }
        }

        protected void xPropConfig(XProp xProp, string OptionArray)
        {
            string PropType = xProp.EditText;
            xProp.EditText = xProp.Value.ToString();

            switch (PropType)
            {
                case "FILEPATHBOX":
                    FilePathEditor fpbox = new FilePathEditor();
                    fpbox.DefPath = (string)xProp.Value;
                    xProp.Value = fpbox;
                    xProp.EditorType = typeof(FilePathEditor);
                    break;
                case "VISABOX":
                    VISASourceBoxEditor VISAbox = new VISASourceBoxEditor();
                    VISAbox.DefSource = (string)xProp.Value;
                    xProp.Value = VISAbox;
                    xProp.EditorType = typeof(VISASourceBoxEditor);
                    break;
                case "OPTIONBOX":
                    FileNameConverter optionBox = new FileNameConverter();
                    optionBox.sOption = OptionArray.Split(',');
                    xProp.Value = optionBox;
                    xProp.sOptions = optionBox.sOption;
                    xProp.ConverType = typeof(FileNameConverter);
                    break;
                case "FOLDERBOX":
                    FolderEditor folderBox = new FolderEditor();
                    folderBox.DefPath = (string)xProp.Value;
                    xProp.Value = folderBox;
                    xProp.EditorType = typeof(FolderEditor);
                    break;
                case "COMBOX":
                   
                    FileNameConverter comBox = new FileNameConverter();
                    string[] Com = new string[255];
                    for (int i = 1; i < 256; i++)
                    {
                        Com[i - 1] = "COM" + i.ToString();
                    }

                    // List<string> COM = new List<string>();
                    //GetRegCom(ref COM);
                    //foreach (string PortName in SerialPort.GetPortNames())
                    //{
                    //     COM.Add(PortName);
                    //}
                    
                    //List<string> vCom = new List<string>();
                    //vCom = COM.Distinct().ToList();
                    //vCom.Sort();
                    //string[] Com = new string[vCom.Count];
                    //for (int i = 0; i < vCom.Count; i++)
                    //{
                    //    Com[i] = vCom[i];
                    //}
                    comBox.sOption = Com;
                    xProp.Value = comBox;
                    xProp.sOptions = comBox.sOption;
                    xProp.ConverType = typeof(FileNameConverter);
                    break;
            }
        }

        //获取USB注册信息
        void GetRegCom( ref List<string> lCom)
        {
             try
            {
                //定义注册表顶级节点 其命名空间是using Microsoft.Win32;
                RegistryKey USBKey;
                //检索子项    
                USBKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum\USB", false);
                //检索所有子项USB下的字符串数组
                foreach (string sub1 in USBKey.GetSubKeyNames())
                {
                    if (sub1.Contains("VID_05C6") && sub1.Contains("&MI_00"))
                    {
                        RegistryKey sub1key = USBKey.OpenSubKey(sub1, false);
                        foreach (string sub2 in sub1key.GetSubKeyNames())
                        {
                            //打开sub1key的子项
                            RegistryKey sub2key = sub1key.OpenSubKey(sub2, false);
                            String friendlyName = (string)sub2key.GetValue("FriendlyName", "");
                            int StartPos = friendlyName.IndexOf('(');
                            int EndPos = friendlyName.IndexOf(')');
                            lCom.Add(friendlyName.Substring(StartPos + 1, EndPos - StartPos - 1));   
                        }
                    }
                }
            }
            catch (Exception msg) //异常处理
            {
                MessageBox.Show(msg.Message);
            }
        }
          

        protected void WriteTestCaseNode(ref XmlNode paraGrpNode, int _idx)
        {
            if (paraGrpNode.Attributes.Count == 0)
            {
                return;
            }

            //modify XmlNode
            string strGrpName = paraGrpNode.Name + "@" + _idx.ToString();
            if (paraGrpNode.Attributes["paraName"] != null)
            {
                strGrpName = paraGrpNode.Attributes["paraName"].Value + "@" + _idx.ToString();
            }

            int propIdx = 0;
            for (int i = 0; i < paraGrpNode.Attributes.Count; i++)
            {
                var att = paraGrpNode.Attributes[i];
                if (!att.Name.StartsWith("para") || att.Name == ("paraName"))
                {
                    continue;
                }

                if (att.Name == "paraChecked")
                {
                    att.Value = this[strGrpName].m_bChecked ? "TRUE" : "FALSE";
                    continue;
                }

                var prop = this[strGrpName][propIdx++];
                if (att.Name == "paraLimit" && paraGrpNode.Name.StartsWith("Suit"))
                {
                    string attVal = "";
                    while (!prop.Name.StartsWith("para")) 
                    {
                        attVal += string.Join(":", new string[] { prop.Name.Split('@')[0], (string)prop.Value, prop.Name.Split('@')[1] }) + ";";
                        if (propIdx < this[strGrpName].Count)
                        {
                            prop = this[strGrpName][propIdx++];
                        } 
                        else
                        {
                            break;
                        }
                    }
                    propIdx--;
                    att.Value = attVal;
                }
                else
                {
                    if (prop.Value.GetType() == typeof(bool))
                    {
                        att.Value = (bool)prop.Value ? "TRUE" : "FALSE";
                    }
                    else if (prop.EditorType == null && prop.ConverType == null)
                    {
                        att.Value = (string)prop.Value;
                    }
                    else
                    {
                        att.Value = prop.EditText;
                    }
                }
            }
        }

        protected void WriteParaNode(ref XmlNode paraGrpNode)
        {
            for (int i = 0; i < paraGrpNode.ChildNodes.Count; i++ )
            {
                var paraNode = paraGrpNode.ChildNodes[i];
                var prop = this[paraGrpNode.Name][i];
                if (paraNode.Attributes.Count == 0)
                {
                    continue;
                }

                //modify XmlNode
                foreach (XmlAttribute att in paraNode.Attributes)
                {
                    if (!att.Name.StartsWith("para"))
                    {
                        continue;
                    }

                    if (prop.Value.GetType() == typeof(bool))
                    {
                        att.Value = (bool)prop.Value ? "TRUE" : "FALSE";
                    }
                    else if (prop.EditorType == null && prop.ConverType == null)
                    {
                        att.Value = (string)prop.Value;
                    }
                    else
                    {
                        att.Value = prop.EditText;
                    }

                    break;
                }
            }
        }

        public bool GetParaListFromXml(XmlNodeList grpParaList )
        {
            int testcaseIdx = 1;
            foreach (XmlNode paraGrpNode in grpParaList)
            {
                if (paraGrpNode.Name.StartsWith("Test") || paraGrpNode.Name.StartsWith("Suit"))
                {
                    string strGrpNode = ((paraGrpNode.Attributes["paraName"] != null) ? 
                        paraGrpNode.Attributes["paraName"].Value : paraGrpNode.Name)
                        + "@" + testcaseIdx.ToString();

                    Add(strGrpNode, new XProps());
                    LoadTestCaseNode(paraGrpNode, testcaseIdx++);
                } 
                else
                {
                    if (ContainsKey(paraGrpNode.Name))
                    {
                        this[paraGrpNode.Name] = new XProps();
                    }
                    else
                    {
                        Add(paraGrpNode.Name, new XProps());
                    }

                    LoadParaNode(paraGrpNode);
                }
            }

            return true;
        }

        public bool SaveToXml(ref XmlNodeList grpParaList)
        {
            int testcaseIdx = 1;
            foreach (XmlNode paraGrpNode in grpParaList)
            {
                XmlNode refGrpNode = paraGrpNode;
                if (paraGrpNode.Name.StartsWith("Test") || paraGrpNode.Name.StartsWith("Suit"))
                {
                    WriteTestCaseNode(ref refGrpNode, testcaseIdx++);
                }
                else
                {
                    WriteParaNode(ref refGrpNode);
                }
            }

            return true;
        }
    }

    public class CfgParaGroupDictionary : Dictionary<string, CfgParaDictionary>
    {
        protected String XmlName = "";

        public CfgParaGroupDictionary(string _xmlName)
        {
            XmlName = _xmlName;
        }

        public bool LoadPara()
        {
            if (!File.Exists(XmlName))
            {
                MessageBox.Show("Xml file not exist! ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XmlName);
            XmlNodeList rootlist = xmlDoc.DocumentElement.ChildNodes;
            if (null == rootlist)
            {
                MessageBox.Show("Xml file not exist the Note", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            foreach (XmlNode paraListNode in rootlist)
            {
                if (paraListNode.Name.Contains("#comment"))    //rocky
                    continue;

                //if (!paraListNode.Name.ToUpper().StartsWith("DUT") && !paraListNode.Name.ToUpper().Equals("COMMON"))
                //{
                //    continue;
                //}

                CfgParaDictionary cfgParaDic = new CfgParaDictionary();
                cfgParaDic.GetParaListFromXml(paraListNode.ChildNodes);
                this.Add(paraListNode.Name, cfgParaDic);
            }

            return true;
        }

        public bool SaveToXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XmlName);
            XmlNodeList rootlist = xmlDoc.DocumentElement.ChildNodes;

            if (null == rootlist)
            {
                MessageBox.Show("Xml file not exist the Note", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            foreach (XmlNode paraListNode in rootlist)
            {
                //if (!paraListNode.Name.ToUpper().StartsWith("DUT") && !paraListNode.Name.ToUpper().Equals("COMMON"))
                //{
                //    continue;
                //}

                var _xmlList = paraListNode.ChildNodes;
                this[paraListNode.Name].SaveToXml(ref _xmlList);
            }

            xmlDoc.Save(XmlName);

            return true;
        }
    }
}