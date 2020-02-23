namespace CeaseUI
{
    partial class ControlPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlPanel));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.StartDut1 = new System.Windows.Forms.Button();
            this.InitDut1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.InitDut4 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.InitDut3 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.InitDut2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.StartDut4 = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.StartDut3 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.StartDut2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Mouse.bmp");
            // 
            // StartDut1
            // 
            this.StartDut1.Enabled = false;
            this.StartDut1.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartDut1.Location = new System.Drawing.Point(6, 15);
            this.StartDut1.Name = "StartDut1";
            this.StartDut1.Size = new System.Drawing.Size(161, 53);
            this.StartDut1.TabIndex = 11;
            this.StartDut1.Text = "START";
            this.StartDut1.UseVisualStyleBackColor = true;
            this.StartDut1.Click += new System.EventHandler(this.Start_Click);
            // 
            // InitDut1
            // 
            this.InitDut1.Enabled = false;
            this.InitDut1.Font = new System.Drawing.Font("Courier New", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InitDut1.Location = new System.Drawing.Point(6, 14);
            this.InitDut1.Name = "InitDut1";
            this.InitDut1.Size = new System.Drawing.Size(161, 70);
            this.InitDut1.TabIndex = 10;
            this.InitDut1.Text = "INIT";
            this.InitDut1.UseVisualStyleBackColor = true;
            this.InitDut1.Click += new System.EventHandler(this.Init_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(735, 133);
            this.tabControl1.TabIndex = 12;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.SelectIndex_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(727, 107);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "INIT";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.InitDut4);
            this.groupBox5.Location = new System.Drawing.Point(548, 8);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(173, 93);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "DUT4";
            // 
            // InitDut4
            // 
            this.InitDut4.Enabled = false;
            this.InitDut4.Font = new System.Drawing.Font("Courier New", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InitDut4.Location = new System.Drawing.Point(6, 14);
            this.InitDut4.Name = "InitDut4";
            this.InitDut4.Size = new System.Drawing.Size(161, 70);
            this.InitDut4.TabIndex = 10;
            this.InitDut4.Text = "INIT";
            this.InitDut4.UseVisualStyleBackColor = true;
            this.InitDut4.Click += new System.EventHandler(this.InitDut4_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.InitDut3);
            this.groupBox4.Location = new System.Drawing.Point(367, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(173, 93);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "DUT3";
            // 
            // InitDut3
            // 
            this.InitDut3.Enabled = false;
            this.InitDut3.Font = new System.Drawing.Font("Courier New", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InitDut3.Location = new System.Drawing.Point(6, 14);
            this.InitDut3.Name = "InitDut3";
            this.InitDut3.Size = new System.Drawing.Size(161, 70);
            this.InitDut3.TabIndex = 10;
            this.InitDut3.Text = "INIT";
            this.InitDut3.UseVisualStyleBackColor = true;
            this.InitDut3.Click += new System.EventHandler(this.InitDut3_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.InitDut2);
            this.groupBox3.Location = new System.Drawing.Point(188, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(173, 93);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "DUT2";
            // 
            // InitDut2
            // 
            this.InitDut2.Enabled = false;
            this.InitDut2.Font = new System.Drawing.Font("Courier New", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InitDut2.Location = new System.Drawing.Point(6, 14);
            this.InitDut2.Name = "InitDut2";
            this.InitDut2.Size = new System.Drawing.Size(161, 70);
            this.InitDut2.TabIndex = 10;
            this.InitDut2.Text = "INIT";
            this.InitDut2.UseVisualStyleBackColor = true;
            this.InitDut2.Click += new System.EventHandler(this.InitDut2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.InitDut1);
            this.groupBox1.Location = new System.Drawing.Point(6, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(173, 93);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DUT1";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox8);
            this.tabPage2.Controls.Add(this.groupBox7);
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(727, 107);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "START";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.textBox4);
            this.groupBox8.Controls.Add(this.StartDut4);
            this.groupBox8.Location = new System.Drawing.Point(543, 6);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(173, 93);
            this.groupBox8.TabIndex = 13;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "DUT4";
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Location = new System.Drawing.Point(6, 67);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(161, 21);
            this.textBox4.TabIndex = 15;
            this.textBox4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox4_KeyUp);
            // 
            // StartDut4
            // 
            this.StartDut4.Enabled = false;
            this.StartDut4.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartDut4.Location = new System.Drawing.Point(6, 15);
            this.StartDut4.Name = "StartDut4";
            this.StartDut4.Size = new System.Drawing.Size(161, 53);
            this.StartDut4.TabIndex = 11;
            this.StartDut4.Text = "START";
            this.StartDut4.UseVisualStyleBackColor = true;
            this.StartDut4.Click += new System.EventHandler(this.StartDut4_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.textBox3);
            this.groupBox7.Controls.Add(this.StartDut3);
            this.groupBox7.Location = new System.Drawing.Point(364, 6);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(173, 93);
            this.groupBox7.TabIndex = 13;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "DUT3";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(6, 67);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(161, 21);
            this.textBox3.TabIndex = 14;
            this.textBox3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox3_KeyUp);
            // 
            // StartDut3
            // 
            this.StartDut3.Enabled = false;
            this.StartDut3.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartDut3.Location = new System.Drawing.Point(6, 15);
            this.StartDut3.Name = "StartDut3";
            this.StartDut3.Size = new System.Drawing.Size(161, 53);
            this.StartDut3.TabIndex = 11;
            this.StartDut3.Text = "START";
            this.StartDut3.UseVisualStyleBackColor = true;
            this.StartDut3.Click += new System.EventHandler(this.StartDut3_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.textBox2);
            this.groupBox6.Controls.Add(this.StartDut2);
            this.groupBox6.Location = new System.Drawing.Point(185, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(173, 93);
            this.groupBox6.TabIndex = 13;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "DUT2";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(6, 67);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(161, 21);
            this.textBox2.TabIndex = 13;
            this.textBox2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyUp);
            // 
            // StartDut2
            // 
            this.StartDut2.Enabled = false;
            this.StartDut2.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartDut2.Location = new System.Drawing.Point(6, 15);
            this.StartDut2.Name = "StartDut2";
            this.StartDut2.Size = new System.Drawing.Size(161, 53);
            this.StartDut2.TabIndex = 11;
            this.StartDut2.Text = "START";
            this.StartDut2.UseVisualStyleBackColor = true;
            this.StartDut2.Click += new System.EventHandler(this.StartDut2_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.StartDut1);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(173, 93);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DUT1";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(7, 67);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(161, 21);
            this.textBox1.TabIndex = 12;
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            // 
            // ControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(735, 137);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("ËÎÌו", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ControlPanel";
            this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ShowHint = CeaseUI.Docking.DockState.DockLeftAutoHide;
            this.TabText = "Control Panel";
            this.Text = "Control Panel";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button StartDut1;
        private System.Windows.Forms.Button InitDut1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button InitDut4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button InitDut3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button InitDut2;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button StartDut4;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button StartDut3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button StartDut2;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
    }
}