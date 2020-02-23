using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CeaseUI
{
    public partial class PasswordForm : Form
    {
        public PasswordForm()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string password = textBoxPassword.Text;

            switch(password)
            {
                case "":
                    //this.DialogResult = DialogResult.OK;
                    this.DialogResult = DialogResult.Yes;
                    break;

                case "lovedfx":
                    this.DialogResult = DialogResult.Yes;
                    break;

                default:
                    this.DialogResult = DialogResult.Abort;
                    break;
            }

            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Close();
        }

        private void textBoxPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.buttonOk_Click(sender, e);
            }
        }
    }
}
