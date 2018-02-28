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
    public partial class DashBoardItem : UserControl
    {
        public DashBoardItem(ObjTask objTask)
        {
            InitializeComponent();
            if (objTask == null)
                throw new ArgumentNullException("Value can not be null");
            ObjTask = objTask;
            lblCaption.Text = ObjTask.TaskName;
            NORMAL_BACKCOLOR = lblCaption.BackColor;
            NORMAL_FORGECOLOR = lblCaption.ForeColor;
            InitEvent();
        }
        private Color SELECTED_BACKCOLOR = Color.SteelBlue;
        private Color SELECTED_FORGECOLOR = Color.White;

        private Color NORMAL_BACKCOLOR = Color.White;
        private Color NORMAL_FORGECOLOR = Color.White;

        private Color HOVER_BACKCOLOR = Color.SteelBlue;
        private Color HOVER_FORGECOLOR = Color.Black;

        public ObjTask ObjTask { get; set; }
        private void InitEvent()
        {
            if (ObjTask != null)
            {
                ObjTask.TaskRenamed += (name) => { lblCaption.Text = name; };
                ObjTask.SelecteStateChanged += (isSelect) =>
                {
                    if (isSelect)
                    {
                        this.lblCaption.BackColor = SELECTED_BACKCOLOR;
                        this.lblCaption.ForeColor = SELECTED_FORGECOLOR;
                    }
                    else
                    {
                        this.lblCaption.BackColor = NORMAL_BACKCOLOR;
                        this.lblCaption.ForeColor = NORMAL_FORGECOLOR;
                    }
                    this.Refresh();
                };
                lblCaption.Click += (s, e) =>
                {
                    if (e.GetType().GetProperty("Button").GetValue(e).ToString() == "Right")
                        OnRightClicked(this, e);
                    else
                    {
                        bool isKeepSelect = Control.ModifierKeys == Keys.Control;
                        TaskManager.ClickTask(ObjTask.TaskID, isKeepSelect);
                        if (!isKeepSelect)
                        {
                            TaskManager.FocusTask(ObjTask.TaskID);
                        }
                    }
                };
            }
        }
        public int PositionIndex { get; set; }

        public Action<object, EventArgs> OnRightClicked = (o, e) => { };
        private void lblCaption_MouseLeave(object sender, EventArgs e)
        {
            if (ObjTask.IsSelected)
                return;
            lblCaption.BackColor = NORMAL_BACKCOLOR;
            lblCaption.ForeColor = NORMAL_FORGECOLOR;
        }
        private void lblCaption_MouseEnter(object sender, EventArgs e)
        {
            if (ObjTask.IsSelected)
                return;
            lblCaption.BackColor = HOVER_BACKCOLOR;
            lblCaption.ForeColor = HOVER_FORGECOLOR;
        }
    }
}
