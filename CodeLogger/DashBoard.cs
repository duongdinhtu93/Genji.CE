using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenjiCore;
using System.Diagnostics;
using GenjiCore.Helper;

namespace CodeLogger
{
    public partial class DashBoard : UserControl
    {
        private const int DEFAULT_ITEM_HEIGHT = 25;
        public DashBoardState State { get; private set; }
        public List<DashBoardItem> Items { get; private set; }
        public DashBoard()
        {
            InitializeComponent();
            Items = new List<DashBoardItem>();
            this.Load += DashBoard_Load;
        }
        private void DashBoard_Load(object sender, EventArgs e)
        {
            TaskManager.TaskAdded += TaskAdded;
            TaskManager.TaskRemoved += TaskRemoved;
            TaskManager.ObjTasks.ForEach(task =>
            {
                TaskAdded(task);
            });
        }
        private void OnItemRightClicked(object sender, EventArgs e) { }
        private void TaskAdded(ObjTask objTask)
        {
            if (Items.Any(d => objTask.TaskID == d.ObjTask.TaskID))
                throw new Exception("ID already existed in list.");
            var dshbItem = new DashBoardItem(objTask) { Dock = DockStyle.Top };
            dshbItem.OnRightClicked += OnItemRightClicked;
            Items.Add(dshbItem);
            pnMenuItem.Controls.Add(dshbItem);
        }
        private void TaskRemoved(ObjTask objTask)
        {
            var dashBoardItem = Items.FirstOrDefault(d => d.ObjTask.TaskID == objTask.TaskID);
            if (!dashBoardItem.IsNull())
                pnMenuItem.Controls.Remove(dashBoardItem);
        }
    }
}
