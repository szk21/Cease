using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using log4net;
using Cease.Core;
using Cease.Interface.Log;

namespace FormTestATE
{
    public partial class Form1 : Form
    {
        ILog logger;
        IFramework Cease;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cease.StartEngine("TT");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logger = LogManager.GetLogger(typeof(Form1));
            logger.Info("Construct Framework");
            Cease = new Framework(logger);
            if (!Cease.CreateTestEngine())
            {
                logger.Error("Create TestEngine Fail!");
                return;
            }

            Cease.UiUpdateLogRegister(AddMsg);
        }

        public void AddMsg(Object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                EventHandler fun = new EventHandler(AddMsg);
                this.BeginInvoke(fun, new object[] { sender, e });
            }
            else
            {
                var LoggedArgs = (LoggedEventArgs)e;
                ListViewItem item = new ListViewItem(LoggedArgs.m_msg);
                listViewLog.Items.Add((ListViewItem)item.Clone());
                listViewLog.Items[listViewLog.Items.Count - 1].EnsureVisible();
            }
        }
    }
}
