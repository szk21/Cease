namespace CeaseUI
{
    partial class TestItemList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestItemList));
            this.TestListView = new CeaseUI.CeaseListView();
            this.SuspendLayout();
            // 
            // TestListView
            // 
            this.TestListView.AllowColumnReorder = true;
            this.TestListView.AutoArrange = false;
            this.TestListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TestListView.FullRowSelect = true;
            this.TestListView.GridLines = true;
            this.TestListView.HideSelection = false;
            this.TestListView.Location = new System.Drawing.Point(0, 3);
            this.TestListView.MultiSelect = false;
            this.TestListView.Name = "TestListView";
            this.TestListView.Size = new System.Drawing.Size(665, 370);
            this.TestListView.TabIndex = 0;
            this.TestListView.UseCompatibleStateImageBehavior = false;
            this.TestListView.View = System.Windows.Forms.View.Details;
            this.TestListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TestListView_MouseDoubleClick);
            // 
            // TestItemList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(665, 376);
            this.Controls.Add(this.TestListView);
            this.Font = new System.Drawing.Font("ËÎÌו", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestItemList";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.ShowHint = CeaseUI.Docking.DockState.DockTopAutoHide;
            this.TabText = "Task List";
            this.Text = "TestCase List";
            this.ResumeLayout(false);

		}
		#endregion

        CeaseListView TestListView;
    }
}