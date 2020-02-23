namespace CeaseUI
{
    partial class ConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.Path_EditBox = new System.Windows.Forms.TextBox();
            this.DutTreeView = new System.Windows.Forms.TreeView();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.Common_Save_Btn = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Path_EditBox
            // 
            this.Path_EditBox.Location = new System.Drawing.Point(12, 12);
            this.Path_EditBox.Name = "Path_EditBox";
            this.Path_EditBox.Size = new System.Drawing.Size(641, 21);
            this.Path_EditBox.TabIndex = 3;
            // 
            // DutTreeView
            // 
            this.DutTreeView.AllowDrop = true;
            this.DutTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.DutTreeView.CheckBoxes = true;
            this.DutTreeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.DutTreeView.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DutTreeView.Location = new System.Drawing.Point(12, 39);
            this.DutTreeView.Name = "DutTreeView";
            this.DutTreeView.Size = new System.Drawing.Size(447, 620);
            this.DutTreeView.TabIndex = 5;
            this.DutTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.DutTreeView_AfterCheck);
            this.DutTreeView.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.tvTreeView_DrawNode);
            this.DutTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.DutTreeView_AfterSelect);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.propertyGrid.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGrid.Location = new System.Drawing.Point(465, 39);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGrid.Size = new System.Drawing.Size(553, 620);
            this.propertyGrid.TabIndex = 7;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // Common_Save_Btn
            // 
            this.Common_Save_Btn.Location = new System.Drawing.Point(659, 12);
            this.Common_Save_Btn.Name = "Common_Save_Btn";
            this.Common_Save_Btn.Size = new System.Drawing.Size(57, 24);
            this.Common_Save_Btn.TabIndex = 8;
            this.Common_Save_Btn.Text = "Save";
            this.Common_Save_Btn.UseVisualStyleBackColor = true;
            this.Common_Save_Btn.Click += new System.EventHandler(this.Common_Save_Btn_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(740, 12);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(57, 24);
            this.buttonOk.TabIndex = 9;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(819, 12);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(57, 24);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 664);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.Common_Save_Btn);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.DutTreeView);
            this.Controls.Add(this.Path_EditBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConfigForm";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Path_EditBox;
        private System.Windows.Forms.TreeView DutTreeView;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button Common_Save_Btn;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
    }
}