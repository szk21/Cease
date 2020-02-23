namespace StartCease
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cmbStation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbModel = new System.Windows.Forms.ComboBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.cmbDut = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxToolVer = new System.Windows.Forms.ComboBox();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.notifyIconCease = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripCease = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripCease.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbStation
            // 
            this.cmbStation.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbStation.FormattingEnabled = true;
            this.cmbStation.ItemHeight = 16;
            this.cmbStation.Items.AddRange(new object[] {
            "BC",
            "CT",
            "BT",
            "BTW",
            "PT",
            "MT",
            "MTW",
            "LP",
            "MC"});
            this.cmbStation.Location = new System.Drawing.Point(181, 63);
            this.cmbStation.Name = "cmbStation";
            this.cmbStation.Size = new System.Drawing.Size(121, 24);
            this.cmbStation.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(118, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "工站";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(118, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "机型";
            // 
            // cmbModel
            // 
            this.cmbModel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbModel.FormattingEnabled = true;
            this.cmbModel.Items.AddRange(new object[] {
            "G66"});
            this.cmbModel.Location = new System.Drawing.Point(181, 111);
            this.cmbModel.Name = "cmbModel";
            this.cmbModel.Size = new System.Drawing.Size(121, 24);
            this.cmbModel.TabIndex = 2;
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonStart.Location = new System.Drawing.Point(181, 205);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(79, 26);
            this.buttonStart.TabIndex = 4;
            this.buttonStart.Text = "确定";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // cmbDut
            // 
            this.cmbDut.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbDut.FormattingEnabled = true;
            this.cmbDut.Items.AddRange(new object[] {
            "G66"});
            this.cmbDut.Location = new System.Drawing.Point(181, 158);
            this.cmbDut.Name = "cmbDut";
            this.cmbDut.Size = new System.Drawing.Size(121, 24);
            this.cmbDut.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(118, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "数量";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(118, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "版本";
            // 
            // comboBoxToolVer
            // 
            this.comboBoxToolVer.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxToolVer.FormattingEnabled = true;
            this.comboBoxToolVer.ItemHeight = 16;
            this.comboBoxToolVer.Items.AddRange(new object[] {
            "BC",
            "CT",
            "BT",
            "BTW",
            "PT",
            "MT",
            "MTW",
            "LP",
            "MC"});
            this.comboBoxToolVer.Location = new System.Drawing.Point(181, 19);
            this.comboBoxToolVer.Name = "comboBoxToolVer";
            this.comboBoxToolVer.Size = new System.Drawing.Size(310, 24);
            this.comboBoxToolVer.TabIndex = 7;
            this.comboBoxToolVer.SelectedIndexChanged += new System.EventHandler(this.comboBoxToolVer_SelectedIndexChanged);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonUpdate.Location = new System.Drawing.Point(513, 17);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(79, 26);
            this.buttonUpdate.TabIndex = 9;
            this.buttonUpdate.Text = "Check";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // notifyIconCease
            // 
            this.notifyIconCease.ContextMenuStrip = this.contextMenuStripCease;
            this.notifyIconCease.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconCease.Icon")));
            this.notifyIconCease.Text = "CeaseDaemon";
            this.notifyIconCease.Visible = true;
            this.notifyIconCease.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIconCease_MouseDoubleClick);
            // 
            // contextMenuStripCease
            // 
            this.contextMenuStripCease.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.contextMenuStripCease.Name = "contextMenuStripCease";
            this.contextMenuStripCease.Size = new System.Drawing.Size(101, 48);
            this.contextMenuStripCease.Text = "Cease Daemon";
            // 
            // 显示ToolStripMenuItem
            // 
            this.显示ToolStripMenuItem.Name = "显示ToolStripMenuItem";
            this.显示ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.显示ToolStripMenuItem.Text = "显示";
            this.显示ToolStripMenuItem.Click += new System.EventHandler(this.显示ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 249);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxToolVer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbDut);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbModel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbStation);
            this.Name = "Form1";
            this.Text = "CeaseDaemon";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form_SizeChanged);
            this.contextMenuStripCease.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbStation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbModel;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.ComboBox cmbDut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxToolVer;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.NotifyIcon notifyIconCease;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripCease;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
    }
}

