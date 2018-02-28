using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenjiCore.Helper.Utils
{
    public partial class frmSuggester : Form
    {
        public void AppendData(string[] data)
        {
            listBoxRecommendation.DataSource = data;
        }
        public frmSuggester()
        {
            InitializeComponent();
        }
    }
}
