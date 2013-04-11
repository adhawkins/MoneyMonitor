using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoneyMonitor
{
    public partial class WarningForm : Form
    {
        public WarningForm()
        {
            InitializeComponent();
        }

        private void WarningForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CanClose;
        }

        private bool m_CanClose = false;

        public bool CanClose
        {
            get { return m_CanClose; }
            set { m_CanClose = value; }
        }

        public void DoClose()
        {
            CanClose=true;
            Close();
        }
    }
}
