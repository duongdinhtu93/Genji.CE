using ApplicationCore.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationCore.Components.Notifier
{
    internal partial class NotifierHandler : Form
    {
        public NotifierHandler()
        {
            InitializeComponent();
            ((IController)CoreControllerCenter.NotifyController).OnComponentKilled += () => { OnParentExit(); };
            this.Shown += NotifierHandler_Shown;
        }

        private void NotifierHandler_Shown(object sender, EventArgs e)
        {
            Hide();
            CoreControllerCenter.NotifyController.RegisterListenEvent(ShowTaskbarPopup);
        }

        private void ShowTaskbarPopup(string caption, string message, Action hyperLink)
        {
            try
            {
                new Thread(new ThreadStart(() =>
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        Control.CheckForIllegalCrossThreadCalls = false;
                        NotifierPopup popup = new NotifierPopup();
                        popup.Show(caption, message, 200, 3000, 100, hyperLink);
                    }));
                })).Start();
            }
            catch { }
        }

        private void OnParentExit()
        {
            try { Application.Exit(); } catch { }
        }
    }
}
