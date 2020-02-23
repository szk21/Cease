using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace CeaseUI
{
    public class CeaseListView : ListView
    {
        public CeaseListView()
        {
            SetStyle(ControlStyles.DoubleBuffer |
                              ControlStyles.OptimizedDoubleBuffer |
                              ControlStyles.AllPaintingInWmPaint,
                              true);
            UpdateStyles();
        }

    }
}
