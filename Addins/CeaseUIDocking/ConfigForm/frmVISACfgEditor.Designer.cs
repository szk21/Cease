namespace PropsBox
{
    partial class frmVISACfg
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tB_Port = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmB_Index = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmBox_Type = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.InstrUSBAddr = new System.Windows.Forms.TextBox();
            this.InstrIPAddr = new System.Windows.Forms.TextBox();
            this.InstrComPort = new System.Windows.Forms.ComboBox();
            this.InstrGPIBAddr = new System.Windows.Forms.ComboBox();
            this.GPIBIndex = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.USBCheck = new System.Windows.Forms.RadioButton();
            this.EthnetCheck = new System.Windows.Forms.RadioButton();
            this.COMPortCheck = new System.Windows.Forms.RadioButton();
            this.GPIBCheck = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.VISASource = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tB_Port);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cmB_Index);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.cmBox_Type);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.ApplyBtn);
            this.groupBox1.Controls.Add(this.InstrUSBAddr);
            this.groupBox1.Controls.Add(this.InstrIPAddr);
            this.groupBox1.Controls.Add(this.InstrComPort);
            this.groupBox1.Controls.Add(this.InstrGPIBAddr);
            this.groupBox1.Controls.Add(this.GPIBIndex);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.USBCheck);
            this.groupBox1.Controls.Add(this.EthnetCheck);
            this.groupBox1.Controls.Add(this.COMPortCheck);
            this.groupBox1.Controls.Add(this.GPIBCheck);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(476, 273);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Interface";
            // 
            // tB_Port
            // 
            this.tB_Port.Enabled = false;
            this.tB_Port.Location = new System.Drawing.Point(242, 157);
            this.tB_Port.Name = "tB_Port";
            this.tB_Port.Size = new System.Drawing.Size(76, 21);
            this.tB_Port.TabIndex = 23;
            this.tB_Port.Text = "56001";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(206, 160);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 22;
            this.label9.Text = "Port";
            // 
            // cmB_Index
            // 
            this.cmB_Index.FormattingEnabled = true;
            this.cmB_Index.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.cmB_Index.Location = new System.Drawing.Point(121, 124);
            this.cmB_Index.Name = "cmB_Index";
            this.cmB_Index.Size = new System.Drawing.Size(60, 20);
            this.cmB_Index.TabIndex = 21;
            this.cmB_Index.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(81, 128);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 20;
            this.label8.Text = "Index";
            // 
            // cmBox_Type
            // 
            this.cmBox_Type.FormattingEnabled = true;
            this.cmBox_Type.Items.AddRange(new object[] {
            "INSTR",
            "SOCKET"});
            this.cmBox_Type.Location = new System.Drawing.Point(121, 157);
            this.cmBox_Type.Name = "cmBox_Type";
            this.cmBox_Type.Size = new System.Drawing.Size(60, 20);
            this.cmBox_Type.TabIndex = 19;
            this.cmBox_Type.Text = "INSTR";
            this.cmBox_Type.SelectedIndexChanged += new System.EventHandler(this.cmBox_Type_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(87, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "Type";
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Location = new System.Drawing.Point(384, 236);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(75, 23);
            this.ApplyBtn.TabIndex = 17;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // InstrUSBAddr
            // 
            this.InstrUSBAddr.Location = new System.Drawing.Point(121, 201);
            this.InstrUSBAddr.Name = "InstrUSBAddr";
            this.InstrUSBAddr.Size = new System.Drawing.Size(331, 21);
            this.InstrUSBAddr.TabIndex = 16;
            this.InstrUSBAddr.Text = "USB0::0x0B5B::0xFFF9::749129_146_14::INSTR";
            // 
            // InstrIPAddr
            // 
            this.InstrIPAddr.Location = new System.Drawing.Point(242, 124);
            this.InstrIPAddr.Name = "InstrIPAddr";
            this.InstrIPAddr.Size = new System.Drawing.Size(102, 21);
            this.InstrIPAddr.TabIndex = 16;
            this.InstrIPAddr.Text = "192.168.1.1";
            this.InstrIPAddr.Leave += new System.EventHandler(this.InstrIPAddr_Leave);
            // 
            // InstrComPort
            // 
            this.InstrComPort.FormattingEnabled = true;
            this.InstrComPort.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.InstrComPort.Location = new System.Drawing.Point(121, 75);
            this.InstrComPort.Name = "InstrComPort";
            this.InstrComPort.Size = new System.Drawing.Size(60, 20);
            this.InstrComPort.TabIndex = 12;
            this.InstrComPort.Text = "1";
            // 
            // InstrGPIBAddr
            // 
            this.InstrGPIBAddr.FormattingEnabled = true;
            this.InstrGPIBAddr.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40"});
            this.InstrGPIBAddr.Location = new System.Drawing.Point(252, 31);
            this.InstrGPIBAddr.Name = "InstrGPIBAddr";
            this.InstrGPIBAddr.Size = new System.Drawing.Size(60, 20);
            this.InstrGPIBAddr.TabIndex = 13;
            this.InstrGPIBAddr.Text = "1";
            // 
            // GPIBIndex
            // 
            this.GPIBIndex.FormattingEnabled = true;
            this.GPIBIndex.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.GPIBIndex.Location = new System.Drawing.Point(121, 31);
            this.GPIBIndex.Name = "GPIBIndex";
            this.GPIBIndex.Size = new System.Drawing.Size(60, 20);
            this.GPIBIndex.TabIndex = 14;
            this.GPIBIndex.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(75, 205);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "VI Src";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(188, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "IP Addr";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(87, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(217, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Addr";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(81, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "Index";
            // 
            // USBCheck
            // 
            this.USBCheck.AutoSize = true;
            this.USBCheck.Location = new System.Drawing.Point(13, 203);
            this.USBCheck.Name = "USBCheck";
            this.USBCheck.Size = new System.Drawing.Size(41, 16);
            this.USBCheck.TabIndex = 4;
            this.USBCheck.Text = "USB";
            this.USBCheck.UseVisualStyleBackColor = true;
            // 
            // EthnetCheck
            // 
            this.EthnetCheck.AutoSize = true;
            this.EthnetCheck.Location = new System.Drawing.Point(13, 126);
            this.EthnetCheck.Name = "EthnetCheck";
            this.EthnetCheck.Size = new System.Drawing.Size(53, 16);
            this.EthnetCheck.TabIndex = 4;
            this.EthnetCheck.Text = "TCPIP";
            this.EthnetCheck.UseVisualStyleBackColor = true;
            // 
            // COMPortCheck
            // 
            this.COMPortCheck.AutoSize = true;
            this.COMPortCheck.Location = new System.Drawing.Point(13, 77);
            this.COMPortCheck.Name = "COMPortCheck";
            this.COMPortCheck.Size = new System.Drawing.Size(41, 16);
            this.COMPortCheck.TabIndex = 5;
            this.COMPortCheck.Text = "COM";
            this.COMPortCheck.UseVisualStyleBackColor = true;
            // 
            // GPIBCheck
            // 
            this.GPIBCheck.AutoSize = true;
            this.GPIBCheck.Checked = true;
            this.GPIBCheck.Location = new System.Drawing.Point(13, 33);
            this.GPIBCheck.Name = "GPIBCheck";
            this.GPIBCheck.Size = new System.Drawing.Size(47, 16);
            this.GPIBCheck.TabIndex = 6;
            this.GPIBCheck.TabStop = true;
            this.GPIBCheck.Text = "GPIB";
            this.GPIBCheck.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 308);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "VISA Source";
            // 
            // VISASource
            // 
            this.VISASource.Location = new System.Drawing.Point(87, 305);
            this.VISASource.Name = "VISASource";
            this.VISASource.Size = new System.Drawing.Size(384, 21);
            this.VISASource.TabIndex = 16;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(213, 340);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 17;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmVISACfg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 374);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.VISASource);
            this.MaximizeBox = false;
            this.Name = "frmVISACfg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VISA Config";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ApplyBtn;
        private System.Windows.Forms.TextBox InstrIPAddr;
        private System.Windows.Forms.ComboBox InstrComPort;
        private System.Windows.Forms.ComboBox InstrGPIBAddr;
        private System.Windows.Forms.ComboBox GPIBIndex;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton EthnetCheck;
        private System.Windows.Forms.RadioButton COMPortCheck;
        private System.Windows.Forms.RadioButton GPIBCheck;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox VISASource;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RadioButton USBCheck;
        private System.Windows.Forms.TextBox InstrUSBAddr;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmBox_Type;
        private System.Windows.Forms.ComboBox cmB_Index;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tB_Port;
        private System.Windows.Forms.Label label9;
    }
}