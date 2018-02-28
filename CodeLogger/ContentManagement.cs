using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeLogger
{
    public partial class ContentManagement : UserControl
    {
        public ContentManagement()
        {
            InitializeComponent();
            this.Load += ContentManagement_Load;
        }

        private void ContentManagement_Load(object sender, EventArgs e)
        {

        }
    }
}
