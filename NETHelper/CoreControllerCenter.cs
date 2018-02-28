using GenjiCore.Components.FileTransfer;
using GenjiCore.Components.Notifier;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenjiCore
{
    public static class CoreControllerCenter
    {
        public static bool IsStarted { get; private set; }
       
        public static INotifierController NotifyController { get; private set; }
        public static IFileTransferController FileController { get; private set; }

        [STAThread]
        public static void Start()
        {
            if (IsStarted)
                return;

            NotifyController = new NotifyController();
            FileController = new FileTransferController();

            ((IController)NotifyController)?.Run();
            ((IController)FileController)?.Run();

            Application.ThreadExit += (s, e) => { KillComponents(); };
            IsStarted = true;
        }


        private static void KillComponents()
        {
            ((IController)NotifyController)?.KillMe();
            ((IController)FileController)?.KillMe();
        }
    }
}
