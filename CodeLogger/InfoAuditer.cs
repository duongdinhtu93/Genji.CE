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
    public partial class InfoAuditer : UserControl
    {
        public ObjInfo ObjInfo { get; private set; }
        public InfoAuditer(ObjInfo objInfo)
        {
            InitializeComponent();
            if (objInfo == null)
                throw new ArgumentNullException("Value can not be null.");

        }
    }
}
