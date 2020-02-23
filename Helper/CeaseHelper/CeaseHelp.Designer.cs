namespace CreateTestStoreConfig
{
    partial class CeaseHelp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CeaseHelp));
            this.listViewAddins = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CreateTestStoreConfig = new System.Windows.Forms.Button();
            this.comboBoxStation = new System.Windows.Forms.ComboBox();
            this.labelStation = new System.Windows.Forms.Label();
            this.labelDut = new System.Windows.Forms.Label();
            this.comboBoxDut = new System.Windows.Forms.ComboBox();
            this.labelProject = new System.Windows.Forms.Label();
            this.comboBoxProject = new System.Windows.Forms.ComboBox();
            this.labelCablelossInitValue = new System.Windows.Forms.Label();
            this.comboBoxCablelossInitValue = new System.Windows.Forms.ComboBox();
            this.CreateCablelossItem = new System.Windows.Forms.Button();
            this.CheckCablelossItem = new System.Windows.Forms.Button();
            this.TabPage = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.CreateMD5 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Message = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.Create_CableLoss_Script = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Clear_QCOM_Cableloss = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txb_msg = new System.Windows.Forms.TextBox();
            this.TabPage.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewAddins
            // 
            this.listViewAddins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewAddins.GridLines = true;
            this.listViewAddins.Location = new System.Drawing.Point(8, 8);
            this.listViewAddins.Name = "listViewAddins";
            this.listViewAddins.Size = new System.Drawing.Size(292, 333);
            this.listViewAddins.TabIndex = 0;
            this.listViewAddins.UseCompatibleStateImageBehavior = false;
            this.listViewAddins.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Addin Name";
            this.columnHeader1.Width = 133;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Version";
            this.columnHeader2.Width = 152;
            // 
            // CreateTestStoreConfig
            // 
            this.CreateTestStoreConfig.Location = new System.Drawing.Point(6, 353);
            this.CreateTestStoreConfig.Name = "CreateTestStoreConfig";
            this.CreateTestStoreConfig.Size = new System.Drawing.Size(294, 33);
            this.CreateTestStoreConfig.TabIndex = 1;
            this.CreateTestStoreConfig.Text = "CreateTestStoreConfig";
            this.CreateTestStoreConfig.UseVisualStyleBackColor = true;
            this.CreateTestStoreConfig.Click += new System.EventHandler(this.CreateTestStoreConfig_Click);
            // 
            // comboBoxStation
            // 
            this.comboBoxStation.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxStation.FormattingEnabled = true;
            this.comboBoxStation.Location = new System.Drawing.Point(88, 22);
            this.comboBoxStation.Name = "comboBoxStation";
            this.comboBoxStation.Size = new System.Drawing.Size(123, 24);
            this.comboBoxStation.TabIndex = 2;
            // 
            // labelStation
            // 
            this.labelStation.AutoSize = true;
            this.labelStation.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelStation.Location = new System.Drawing.Point(10, 24);
            this.labelStation.Name = "labelStation";
            this.labelStation.Size = new System.Drawing.Size(72, 16);
            this.labelStation.TabIndex = 3;
            this.labelStation.Text = "Station:";
            // 
            // labelDut
            // 
            this.labelDut.AutoSize = true;
            this.labelDut.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDut.Location = new System.Drawing.Point(442, 25);
            this.labelDut.Name = "labelDut";
            this.labelDut.Size = new System.Drawing.Size(40, 16);
            this.labelDut.TabIndex = 5;
            this.labelDut.Text = "DUT:";
            // 
            // comboBoxDut
            // 
            this.comboBoxDut.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxDut.FormattingEnabled = true;
            this.comboBoxDut.Location = new System.Drawing.Point(488, 22);
            this.comboBoxDut.Name = "comboBoxDut";
            this.comboBoxDut.Size = new System.Drawing.Size(121, 24);
            this.comboBoxDut.TabIndex = 4;
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.labelProject.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelProject.Location = new System.Drawing.Point(227, 25);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(72, 16);
            this.labelProject.TabIndex = 7;
            this.labelProject.Text = "Project:";
            // 
            // comboBoxProject
            // 
            this.comboBoxProject.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxProject.FormattingEnabled = true;
            this.comboBoxProject.Location = new System.Drawing.Point(306, 22);
            this.comboBoxProject.Name = "comboBoxProject";
            this.comboBoxProject.Size = new System.Drawing.Size(122, 24);
            this.comboBoxProject.TabIndex = 6;
            // 
            // labelCablelossInitValue
            // 
            this.labelCablelossInitValue.AutoSize = true;
            this.labelCablelossInitValue.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCablelossInitValue.Location = new System.Drawing.Point(26, 33);
            this.labelCablelossInitValue.Name = "labelCablelossInitValue";
            this.labelCablelossInitValue.Size = new System.Drawing.Size(160, 16);
            this.labelCablelossInitValue.TabIndex = 7;
            this.labelCablelossInitValue.Text = "CablelossInitValue:";
            // 
            // comboBoxCablelossInitValue
            // 
            this.comboBoxCablelossInitValue.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxCablelossInitValue.FormattingEnabled = true;
            this.comboBoxCablelossInitValue.Location = new System.Drawing.Point(192, 30);
            this.comboBoxCablelossInitValue.Name = "comboBoxCablelossInitValue";
            this.comboBoxCablelossInitValue.Size = new System.Drawing.Size(62, 24);
            this.comboBoxCablelossInitValue.TabIndex = 6;
            // 
            // CreateCablelossItem
            // 
            this.CreateCablelossItem.Location = new System.Drawing.Point(29, 64);
            this.CreateCablelossItem.Name = "CreateCablelossItem";
            this.CreateCablelossItem.Size = new System.Drawing.Size(225, 37);
            this.CreateCablelossItem.TabIndex = 3;
            this.CreateCablelossItem.Text = "生成黑鲨线损表";
            this.CreateCablelossItem.UseVisualStyleBackColor = true;
            this.CreateCablelossItem.Click += new System.EventHandler(this.CreateCablelossItem_Click);
            // 
            // CheckCablelossItem
            // 
            this.CheckCablelossItem.Location = new System.Drawing.Point(29, 107);
            this.CheckCablelossItem.Name = "CheckCablelossItem";
            this.CheckCablelossItem.Size = new System.Drawing.Size(225, 39);
            this.CheckCablelossItem.TabIndex = 2;
            this.CheckCablelossItem.Text = "检测黑鲨线损表";
            this.CheckCablelossItem.UseVisualStyleBackColor = true;
            this.CheckCablelossItem.Click += new System.EventHandler(this.CheckCablelossItem_Click);
            // 
            // TabPage
            // 
            this.TabPage.Controls.Add(this.tabPage1);
            this.TabPage.Controls.Add(this.tabPage2);
            this.TabPage.Location = new System.Drawing.Point(12, 70);
            this.TabPage.Name = "TabPage";
            this.TabPage.SelectedIndex = 0;
            this.TabPage.Size = new System.Drawing.Size(623, 427);
            this.TabPage.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txb_msg);
            this.tabPage1.Controls.Add(this.CreateMD5);
            this.tabPage1.Controls.Add(this.listViewAddins);
            this.tabPage1.Controls.Add(this.CreateTestStoreConfig);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(615, 401);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Cease Addins";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // CreateMD5
            // 
            this.CreateMD5.Location = new System.Drawing.Point(318, 353);
            this.CreateMD5.Name = "CreateMD5";
            this.CreateMD5.Size = new System.Drawing.Size(294, 33);
            this.CreateMD5.TabIndex = 2;
            this.CreateMD5.Text = "CreateMD5";
            this.CreateMD5.UseVisualStyleBackColor = true;
            this.CreateMD5.Click += new System.EventHandler(this.CreateMD5_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.Message);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(615, 401);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Cableloss";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Message
            // 
            this.Message.Location = new System.Drawing.Point(302, 13);
            this.Message.Name = "Message";
            this.Message.ReadOnly = true;
            this.Message.Size = new System.Drawing.Size(303, 371);
            this.Message.TabIndex = 3;
            this.Message.Text = "Messages:";
            this.Message.WordWrap = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.Create_CableLoss_Script);
            this.groupBox4.Location = new System.Drawing.Point(6, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(286, 97);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Common";
            // 
            // Create_CableLoss_Script
            // 
            this.Create_CableLoss_Script.Location = new System.Drawing.Point(29, 25);
            this.Create_CableLoss_Script.Name = "Create_CableLoss_Script";
            this.Create_CableLoss_Script.Size = new System.Drawing.Size(225, 35);
            this.Create_CableLoss_Script.TabIndex = 0;
            this.Create_CableLoss_Script.Text = "BT测试脚本生成校线损脚本";
            this.Create_CableLoss_Script.UseVisualStyleBackColor = true;
            this.Create_CableLoss_Script.Click += new System.EventHandler(this.Create_CableLoss_Script_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Clear_QCOM_Cableloss);
            this.groupBox3.Location = new System.Drawing.Point(4, 116);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(289, 89);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "QCOM Cableloss(只清空项目目录CL工位的线损表)";
            // 
            // Clear_QCOM_Cableloss
            // 
            this.Clear_QCOM_Cableloss.Location = new System.Drawing.Point(32, 27);
            this.Clear_QCOM_Cableloss.Name = "Clear_QCOM_Cableloss";
            this.Clear_QCOM_Cableloss.Size = new System.Drawing.Size(225, 35);
            this.Clear_QCOM_Cableloss.TabIndex = 1;
            this.Clear_QCOM_Cableloss.Text = "CT线损表清空";
            this.Clear_QCOM_Cableloss.UseVisualStyleBackColor = true;
            this.Clear_QCOM_Cableloss.Click += new System.EventHandler(this.Clear_QCOM_Cableloss_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxCablelossInitValue);
            this.groupBox2.Controls.Add(this.labelCablelossInitValue);
            this.groupBox2.Controls.Add(this.CheckCablelossItem);
            this.groupBox2.Controls.Add(this.CreateCablelossItem);
            this.groupBox2.Location = new System.Drawing.Point(6, 211);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(289, 173);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Black Shark Cableloss";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelStation);
            this.groupBox1.Controls.Add(this.comboBoxStation);
            this.groupBox1.Controls.Add(this.comboBoxDut);
            this.groupBox1.Controls.Add(this.labelDut);
            this.groupBox1.Controls.Add(this.comboBoxProject);
            this.groupBox1.Controls.Add(this.labelProject);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(623, 61);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Common";
            // 
            // txb_msg
            // 
            this.txb_msg.Location = new System.Drawing.Point(318, 249);
            this.txb_msg.Multiline = true;
            this.txb_msg.Name = "txb_msg";
            this.txb_msg.Size = new System.Drawing.Size(287, 98);
            this.txb_msg.TabIndex = 3;
            // 
            // CeaseHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 507);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.TabPage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CeaseHelp";
            this.Text = "CeaseHelp";
            this.TabPage.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewAddins;
        private System.Windows.Forms.Button CreateTestStoreConfig;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ComboBox comboBoxStation;
        private System.Windows.Forms.Label labelStation;
        private System.Windows.Forms.Label labelDut;
        private System.Windows.Forms.ComboBox comboBoxDut;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.ComboBox comboBoxProject;
        private System.Windows.Forms.Button CreateCablelossItem;
        private System.Windows.Forms.Button CheckCablelossItem;
        private System.Windows.Forms.Label labelCablelossInitValue;
        private System.Windows.Forms.ComboBox comboBoxCablelossInitValue;
        private System.Windows.Forms.TabControl TabPage;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button Create_CableLoss_Script;
        private System.Windows.Forms.RichTextBox Message;
        private System.Windows.Forms.Button Clear_QCOM_Cableloss;
        private System.Windows.Forms.Button CreateMD5;
        private System.Windows.Forms.TextBox txb_msg;
    }
}

