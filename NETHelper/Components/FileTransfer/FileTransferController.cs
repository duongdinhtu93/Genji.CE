using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenjiCore.Components.FileTransfer
{
    internal class FileTransferController : IFileTransferController, IController
    {
        private static Action<FTPAuthentication, Action<bool>> OnUploadRequested = (url, callbackCompleted) =>{};
        public FileTransferController()
        {
        }
        private Action onComponentKilled = () => { };
        public Action OnComponentKilled
        {
            get { return onComponentKilled;  }
            set
            {
                onComponentKilled = value;
            }
        }
       
        public void Upload(FTPAuthentication authentication, Action<bool> callbackCompleted = null)
        {
            OnUploadRequested(authentication, callbackCompleted);
        }

        public void DownLoad(FTPAuthentication authentication, Action<bool> callbackCompleted = null)
        {
            throw new NotImplementedException();
        }

        public void KillMe()
        {
            return;
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem(x => { Application.Run(new FileHandler()); });
        }

        public bool RegisterListenEvent(Action<FTPAuthentication, Action<bool>> action)
        {
            OnUploadRequested += action;
            return true;
        }

    }

    public interface IFileTransferController
    {
        bool RegisterListenEvent(Action<FTPAuthentication, Action<bool>> action);
        void Upload(FTPAuthentication ftpContext, Action<bool> callbackCompleted = null);
        void DownLoad(FTPAuthentication ftpContext, Action<bool> callbackCompleted = null);
    }

    public class FTPAuthentication
    {
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
