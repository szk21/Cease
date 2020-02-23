namespace StartCease
{
    partial class FormDownload
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
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxToolVerInFtp = new System.Windows.Forms.ComboBox();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.progressBarDownload = new System.Windows.Forms.ProgressBar();
            this.textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(98, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "可下载版本";
            // 
            // comboBoxToolVerInFtp
            // 
            this.comboBoxToolVerInFtp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxToolVerInFtp.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxToolVerInFtp.FormattingEnabled = true;
            this.comboBoxToolVerInFtp.ItemHeight = 16;
            this.comboBoxToolVerInFtp.Items.AddRange(new object[] {
            "BC",
            "CT",
            "BT",
            "BTW",
            "PT",
            "MT",
            "MTW",
            "LP",
            "MC"});
            this.comboBoxToolVerInFtp.Location = new System.Drawing.Point(206, 25);
            this.comboBoxToolVerInFtp.Name = "comboBoxToolVerInFtp";
            this.comboBoxToolVerInFtp.Size = new System.Drawing.Size(139, 24);
            this.comboBoxToolVerInFtp.TabIndex = 10;
            // 
            // buttonDownload
            // 
            this.buttonDownload.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonDownload.Location = new System.Drawing.Point(180, 269);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(87, 26);
            this.buttonDownload.TabIndex = 9;
            this.buttonDownload.Text = "开始下载";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // progressBarDownload
            // 
            this.progressBarDownload.Location = new System.Drawing.Point(12, 240);
            this.progressBarDownload.Name = "progressBarDownload";
            this.progressBarDownload.Size = new System.Drawing.Size(422, 23);
            this.progressBarDownload.TabIndex = 12;
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(12, 55);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(422, 179);
            this.textBox.TabIndex = 13;
            // 
            // FormDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 307);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.progressBarDownload);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxToolVerInFtp);
            this.Controls.Add(this.buttonDownload);
            this.Name = "FormDownload";
            this.Text = "FormDownload";
            this.Load += new System.EventHandler(this.FormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxToolVerInFtp;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.ProgressBar progressBarDownload;
        private System.Windows.Forms.TextBox textBox;
    }
}