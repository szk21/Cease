using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace PropsBox
{
    public partial class frmVISACfg : Form
    {
        public frmVISACfg(String viResource)
        {
            InitializeComponent();
            VISASource.Text = viResource;
            sVISASource = VISASource.Text;
        }
        public String sVISASource;
        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            if (GPIBCheck.Checked == true)
            {
                sVISASource = "GPIB" + GPIBIndex.Text + "::" + InstrGPIBAddr.Text + "::INSTR";
            }
            else if (COMPortCheck.Checked == true)
            {
                sVISASource = "ASRL" + InstrComPort.Text + "::INSTR";
            }
            else if (EthnetCheck.Checked == true)
            {
               //TCPIP0::192.168.20.3::56001::SOCKET
                if (cmBox_Type.Text == "SOCKET")
                {
                    sVISASource = "TCPIP" + cmB_Index.Text + "::" + InstrIPAddr.Text + "::" + tB_Port.Text + "::SOCKET";
                }
                else
                {
                    //cmB_Index
                    sVISASource = "TCPIP" + cmB_Index.Text + "::" + InstrIPAddr.Text + "::" + "inst0" + "::INSTR";
                }
            }
            else if (USBCheck.Checked == true)
            {
                sVISASource = InstrUSBAddr.Text;
            }
            VISASource.Text = sVISASource;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (VISASource.Text != "")
            {
                sVISASource = VISASource.Text;
                Close();
            }
        }

        private void InstrIPAddr_Leave(object sender, EventArgs e)
        {
            bool blnTest = false;
            //bool _Result = true;

            Regex regex = new Regex("^[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}$");
            blnTest = regex.IsMatch(InstrIPAddr.Text);
            if (blnTest == true)
            {
                string[] strTemp = this.InstrIPAddr.Text.Split(new char[] { '.' }); // textBox1.Text.Split(new char[] { '.' });
                for (int i = 0; i < strTemp.Length; i++)
                {
                    if (Convert.ToInt32(strTemp[i]) > 255)
                    { //大于255则提示，不符合IP格式 
                        MessageBox.Show("不符合IP格式");
                        InstrIPAddr.Focus();
                    }
                }
            }
            else
            {
                //输入非数字则提示，不符合IP格式 
                MessageBox.Show("不符合IP格式");
                InstrIPAddr.Focus();
            }
        }

        private void cmBox_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmBox_Type.Text == "SOCKET")
            {
                tB_Port.Enabled = true;
            }
            else
            {
                tB_Port.Enabled = false;
            }
        }
    }
}