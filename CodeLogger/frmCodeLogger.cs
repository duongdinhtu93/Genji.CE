using GenjiCore.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeLogger
{
    public partial class frmCodeLogger : Form
    {
        public frmCodeLogger()
        {
            InitializeComponent();
            Load += FrmCodeLogger_Load;
            TaskManager.LoadTasks();
        }

        private void FrmCodeLogger_Load(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskManager.AddTask("4632 - [VTS] Yêu cầu kho thương mại điện tử");
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            TaskManager.SaveContext();
        }
    }
}
