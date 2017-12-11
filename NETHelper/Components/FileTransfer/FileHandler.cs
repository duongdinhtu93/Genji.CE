using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationCore.Components.FileTransfer
{
    public partial class FileHandler : Form
    {
        private const int DownloadItemHeight = 4;
        public FileHandler()
        {
            InitializeComponent();
            ((IController)CoreControllerCenter.FileController).OnComponentKilled += () => { OnParentExit(); };
            this.Shown += FileHandler_Shown;
        }

        private void FileHandler_Shown(object sender, EventArgs e)
        {
            Hide();
            CoreControllerCenter.FileController.RegisterListenEvent(RunUpload);
        }
        private void OnUploading(Guid identity, string filename, long max, long step)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                if (!Controls.Find(identity.ToString().Replace("-", string.Empty), true).Any())
                {
                    Top = 0;
                    Left = 0;
                    Width = Screen.PrimaryScreen.WorkingArea.Width;
                    Height = DownloadItemHeight;
                    FormBorderStyle = FormBorderStyle.None;
                    ShowInTaskbar = false;
                    ControlBox = false;
                    TopMost = true;
                    ProgressBar prgs = new ProgressBar();
                    prgs.Dock = DockStyle.Fill;
                    prgs.Maximum = (int)max;
                    prgs.Margin = new Padding(0, 0, 0, 0);
                    prgs.Name = identity.ToString().Replace("-", string.Empty);
                    Controls.Add(prgs);
                    prgs.Validated += Prgs_Validated;
                    this.Show();

                }
                else
                {
                    var prgs = Controls.Find(identity.ToString().Replace("-", string.Empty), true).FirstOrDefault();
                    if (prgs != null)
                    {
                        ((ProgressBar)prgs).Value = (int)step;
                    }
                }
            }));
        }

        private void Prgs_Validated(object sender, EventArgs e)
        {
            if (((ProgressBar)sender).Value >= ((ProgressBar)sender).Maximum)
            {
                Controls.Remove(Controls.Find(((ProgressBar)sender).Name.ToString(), true).FirstOrDefault());
                Height = Controls.Count * DownloadItemHeight;
            }
        }
        private void OnParentExit()
        {
            try { Application.Exit(); } catch { }
        }

        void RunUpload(FTPAuthentication authentication, Action<bool> callback)
        {
            Form uploadProgress = new Form();
            uploadProgress.ShowInTaskbar = false;
            uploadProgress.ControlBox = false;
            uploadProgress.TopMost = true;
            uploadProgress.Padding = new Padding(0, 0, 0, 0);
            uploadProgress.FormBorderStyle = FormBorderStyle.None;
            uploadProgress.Width = Screen.PrimaryScreen.WorkingArea.Width;
            uploadProgress.StartPosition = FormStartPosition.Manual;
            uploadProgress.Top = 0;
            uploadProgress.Left = 0;

            Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (var fileName in fileDialog.FileNames)
                    {
                        var stream = System.IO.File.Open(fileName, FileMode.Open);
                        var request = (FtpWebRequest)WebRequest.Create(authentication.Url + Path.GetFileName(fileName));
                        request.Method = WebRequestMethods.Ftp.UploadFile;
                        request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheIfAvailable);
                        if (!string.IsNullOrEmpty(authentication.UserName) && !string.IsNullOrEmpty(authentication.Password))
                            request.Credentials = new NetworkCredential(authentication.UserName, authentication.Password);
                        Stream ftpStream = request.GetRequestStream();
                        byte[] buffer = new byte[10240];
                        int read;
                        ProgressBar progressBar = new ProgressBar()
                        { Maximum = (int)stream.Length, Dock = DockStyle.Fill };
                        uploadProgress.Controls.Add(progressBar);
                        uploadProgress.Show();
                        progressBar.Height = 4;
                        uploadProgress.Height = 4;
                        new Thread(new ThreadStart(() =>
                        {
                            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                progressBar.Invoke(new MethodInvoker(() =>
                                    {
                                        progressBar.Value = (int)stream.Position;
                                    }));
                                ftpStream.Write(buffer, 0, read);
                            }
                            stream.Close();
                            ftpStream.Close();
                            uploadProgress.Close();
                            CoreControllerCenter.NotifyController.ShowTaskbarPopup("Thông báo", "File uploaded: " + fileName);
                        })).Start();

                    }
                    try
                    {
                        callback?.Invoke(true);
                    }
                    catch { }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                callback?.Invoke(false);
            }
            finally
            {
                
            }
        }
    }
}
