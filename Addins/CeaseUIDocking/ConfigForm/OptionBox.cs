using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PropsBox
{
    public partial class OptionBox : Form
    {
        public OptionBox(String[] lines)
        {
            InitializeComponent();
            items = lines;
            richTextBox1.Lines = items;
        }
        public String[] items;

        private void button1_Click(object sender, EventArgs e)
        {
            items=richTextBox1.Lines;
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
    
}