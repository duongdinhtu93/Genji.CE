using GenjiCore.Components.Notifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenjiCore.Components.Notifier
{
    internal class NotifyController : INotifierController, IController
    {
        private bool IsRuned = false;
        private static Action<string, string, Action> PopupMessageChanged = (caption, message, function) => { };

        private Action onComponentKilled = () => { };
        public Action OnComponentKilled
        {
            get
            {
                return onComponentKilled;
            }

            set
            {
                onComponentKilled = value;
            }
        }

        public bool RegisterListenEvent(Action<string, string, Action> function)
        {
            PopupMessageChanged += function;
            return true;
        }
        public void ShowTaskbarPopup(string caption, string message, Action function = null)
        {
            PopupMessageChanged(caption, message, function);
        }

        public void Run()
        {
            if (IsRuned)
                return;

            ThreadPool.QueueUserWorkItem(x => { Application.Run(new NotifierHandler()); });
            IsRuned = true;
        }

        public void KillMe()
        {
            OnComponentKilled();
        }
    }

    public interface INotifierController
    {
        void ShowTaskbarPopup(string caption, string message, Action function = null);
        bool RegisterListenEvent(Action<string, string, Action> function);
    }
}
