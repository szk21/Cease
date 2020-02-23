namespace CeaseUI
{
    partial class AboutDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelFramework = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelCeaseStartup = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listViewAddins = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelMajorVersion = new System.Windows.Forms.Label();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.labelStationVer = new System.Windows.Forms.Label();
            this.alter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOK.Location = new System.Drawing.Point(388, 254);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            // 
            // labelFramework
            // 
            this.labelFramework.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFramework.Location = new System.Drawing.Point(118, 72);
            this.labelFramework.Name = "labelFramework";
            this.labelFramework.Size = new System.Drawing.Size(321, 20);
            this.labelFramework.TabIndex = 1;
            this.labelFramework.Text = "Cease Framework:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // labelCeaseStartup
            // 
            this.labelCeaseStartup.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCeaseStartup.Location = new System.Drawing.Point(118, 52);
            this.labelCeaseStartup.Name = "labelCeaseStartup";
            this.labelCeaseStartup.Size = new System.Drawing.Size(321, 20);
            this.labelCeaseStartup.TabIndex = 7;
            this.labelCeaseStartup.Text = "Cease Startup:";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(118, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(321, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "(C) 2017 BlackShark. All Rights Reserved";
            // 
            // listViewAddins
            // 
            this.listViewAddins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewAddins.GridLines = true;
            this.listViewAddins.Location = new System.Drawing.Point(12, 119);
            this.listViewAddins.Name = "listViewAddins";
            this.listViewAddins.Size = new System.Drawing.Size(370, 159);
            this.listViewAddins.TabIndex = 9;
            this.listViewAddins.UseCompatibleStateImageBehavior = false;
            this.listViewAddins.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Addin Name";
            this.columnHeader1.Width = 180;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Version";
            this.columnHeader2.Width = 180;
            // 
            // labelMajorVersion
            // 
            this.labelMajorVersion.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMajorVersion.Location = new System.Drawing.Point(118, 12);
            this.labelMajorVersion.Name = "labelMajorVersion";
            this.labelMajorVersion.Size = new System.Drawing.Size(321, 20);
            this.labelMajorVersion.TabIndex = 10;
            this.labelMajorVersion.Text = "Major Version: ";
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(388, 225);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(75, 23);
            this.buttonGenerate.TabIndex = 11;
            this.buttonGenerate.Text = "Gen";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // labelStationVer
            // 
            this.labelStationVer.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStationVer.Location = new System.Drawing.Point(118, 32);
            this.labelStationVer.Name = "labelStationVer";
            this.labelStationVer.Size = new System.Drawing.Size(321, 20);
            this.labelStationVer.TabIndex = 12;
            this.labelStationVer.Text = "Station Version: ";
            // 
            // alter
            // 
            this.alter.Location = new System.Drawing.Point(388, 119);
            this.alter.Name = "alter";
            this.alter.Size = new System.Drawing.Size(75, 23);
            this.alter.TabIndex = 13;
            this.alter.Text = "Alter";
            this.alter.UseVisualStyleBackColor = true;
            this.alter.Click += new System.EventHandler(this.alter_Click);
            // 
            // AboutDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.CancelButton = this.buttonOK;
            this.ClientSize = new System.Drawing.Size(473, 288);
            this.Controls.Add(this.alter);
            this.Controls.Add(this.labelStationVer);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.labelMajorVersion);
            this.Controls.Add(this.listViewAddins);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelCeaseStartup);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelFramework);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.Load += new System.EventHandler(this.AboutDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        private System.Windows.Forms.Label labelFramework;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelCeaseStartup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listViewAddins;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label labelMajorVersion;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.Label labelStationVer;
        private System.Windows.Forms.Button alter;
    }
}