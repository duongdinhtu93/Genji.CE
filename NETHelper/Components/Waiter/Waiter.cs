using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationCore.Components
{
    public static class Waiter
    {
        private static bool     _isWaitingExceptionHandingRequire = true;
        private const int       _cancelTimeout = 5;
        private const int       _maxDegreeOfParallelism = 10;
        private static bool     _isApplicationExit = false;

        public static void ShowWaiting(this Control container, Control control, Action[] methods, string message = "",
            Action cancelAction = null, Action<bool> callBackCompleted = null, Action<Exception, WaiterErrorType> onException = null, bool isDeepLoad = false)
        {
            #region Kiểm tra input
            if (methods == null || !methods.Any() || methods.Any(method => method == null))
                throw new ArgumentNullException("methods", "Methods required");
            if (container == null || container.IsDisposed || !container.IsHandleCreated)
                throw new ObjectDisposedException("container", "can not processing until the unit windows has been created or available");
            if ((_isWaitingExceptionHandingRequire
                && methods.Any(method => !method.Method.GetMethodBody().ExceptionHandlingClauses.Any() && onException == null)))
                throw new NotSupportedException("Invalid methods: Exception handing required inside codeblock");
            if (_isWaitingExceptionHandingRequire && onException == null
                && methods.Any(method => !method.Method.GetMethodBody().ExceptionHandlingClauses.Any(d => d.TryOffset == 1)))
                throw new NotSupportedException("The exception handing is invalid offset");
            #endregion

            #region Khai báo biến
            BackgroundWorker worker = new BackgroundWorker() { WorkerSupportsCancellation = true };
           // CurrencyDataController.DisableThreadingProblemsDetection = true;

            //Waiting dialog
            string waiterIdentity = Guid.NewGuid().ToString().Replace("-", string.Empty);
            Form waiter = InitWaitingUI(waiterIdentity, message, cancelAction != null);
            waiter.Location = new Point(control.ClientSize.Width / 2 - waiter.Size.Width / 2, control.ClientSize.Height / 2 - waiter.Size.Height / 2);
            if (control.GetType() == typeof(TextBox) || control.GetType() == typeof(ComboBox) || control.GetType() == typeof(Label) || control.GetType() == typeof(DateTimePicker) || control.GetType() == typeof(Button))
                waiter.Dock = DockStyle.Fill;
            if (!isDeepLoad)
            {
                if (container.InvokeRequired)
                    container.Invoke(new MethodInvoker(() =>
                    {
                        //Vì TabControl không cho phép chứa child là form hoặc control khác ngoài TabPage
                        waiter.Parent = ((control.GetType() == typeof(TabControl) || control.GetType().Namespace.Contains("XtraTabControl"))
                                ? control.Parent : control);
                    }));
                else waiter.Parent = (control.GetType() == typeof(TabControl) ? control.Parent : control);
            }

            //Cancel timeout
            var cancelTimeoutHandle = new System.Windows.Forms.Timer() { Interval = 1000 };
            int timeTickCounter = 0;
            cancelTimeoutHandle.Tick += (sender, @event) =>
            {
                if (worker.IsBusy)
                    timeTickCounter++;
                if (timeTickCounter >= _cancelTimeout)
                {
                    cancelTimeoutHandle.Enabled = false;
                    cancelTimeoutHandle.Dispose();
                    MessageBox.Show("Cancel timeout is over" + _cancelTimeout + "(s)");
                    container.Dispose();
                }
            };

            //Application exited
            Application.ApplicationExit += (sender, @event) => { _isApplicationExit = true; };

            //Biến ghi nhận tất cả các exception trong các tác vụ raise ra
            List<Exception> exceptionsHanding = new List<Exception>();
            #endregion

            #region Sự kiện bắt đầu thực thi tác vụ
            worker.DoWork += (s, e) =>
            {
                if (!worker.CancellationPending)
                {
                    Control.CheckForIllegalCrossThreadCalls = false;
                    if (methods != null && methods.Any() && !methods.Any(method => method == null) && container != null && !container.IsDisposed && container.IsHandleCreated)
                    {
                        try
                        {
                            //Chạy song song và bất đồng bộ các công việc
                            Parallel.Invoke(new ParallelOptions() { MaxDegreeOfParallelism = _maxDegreeOfParallelism },
                                methods);
                        }
                        catch (AggregateException exception)
                        {
                            //Handing các lỗi được throw ra từ các method chạy song song
                            exceptionsHanding.AddRange(exception.InnerExceptions);
                        }
                    }
                }
            };
            #endregion

            #region Sự kiện hủy bỏ tác vụ
            if (cancelAction != null)
            {
                //tạo button cancel và sự kiện cancel khi user click
                Button cancelButton = new Button() { Text = "Cancel", BackColor = waiter.BackColor, Dock = DockStyle.Left };
                cancelButton.Click += (sender, @event) =>
                {
                    if (!_isApplicationExit && cancelAction != null && container != null && !container.IsDisposed && container.IsHandleCreated)
                    {
                        try
                        {
                            worker.CancelAsync();
                            //Thông báo tới hàm gọi - user vừa click hủy bỏ tác vụ
                            container.Invoke(cancelAction);
                            //Bắt đầu tính cancel timeout
                            cancelTimeoutHandle.Enabled = true;
                        }
                        catch (Exception exception) { try { onException?.Invoke(exception, WaiterErrorType.OnCancelNotifyingError); } catch { } }
                    }
                };
                waiter.Controls.Add(cancelButton);
            }

            #endregion

            #region Sự kiện sau khi hoàn thành tác vụ
            worker.RunWorkerCompleted += (s, e) =>
            {
                //Unlock các control sau khi task hoàn thành
                //CurrencyDataController.DisableThreadingProblemsDetection = true;
                Control.CheckForIllegalCrossThreadCalls = false;
                SwitchControlState(control, waiterIdentity, ControlState.Unlock);
                if (!isDeepLoad)
                    waiter.Close();
                Application.DoEvents();

                //Thông báo lỗi thực thi tác vụ
                exceptionsHanding.ForEach(exc =>
                {
                    try
                    {
                        onException?.Invoke(exc, WaiterErrorType.OnBusinessError);
                    }
                    catch (Exception ex)
                    {
                        try { onException?.Invoke(ex, WaiterErrorType.OnErrorHandingError); }
                        catch { }
                    }
                });

                //Thông báo tác vụ hoàn thành và kết quả
                if (callBackCompleted != null)
                {
                    try
                    {
                        if (container != null && !container.IsDisposed && container.IsHandleCreated)
                        {
                            if (container.InvokeRequired)
                                container.Invoke(new Action(() => { callBackCompleted.Invoke(!exceptionsHanding.Any()); }));
                            else callBackCompleted.Invoke(!exceptionsHanding.Any());
                        }
                    }
                    catch (Exception ex)
                    {
                        try { onException?.Invoke(ex, WaiterErrorType.OnCompletedNotifyingError); }
                        catch { }
                    };
                }

                ////Focus control nếu có
                //if (!_isApplicationExit && container != null && !container.IsDisposed && container.IsHandleCreated
                //    && controlToFocusAfterFinish != null && controlToFocusAfterFinish.CanFocus)
                //{
                //    try
                //    {
                //        if (controlToFocusAfterFinish.InvokeRequired)
                //            controlToFocusAfterFinish.Invoke(new Action(() => { controlToFocusAfterFinish.Focus(); }));
                //        else controlToFocusAfterFinish.Focus();
                //    }
                //    catch (Exception ex)
                //    {
                //        try { onException?.Invoke(ex, WaiterErrorType.OnFocussingError); }
                //        catch
                //        { }
                //    };
                //};
            };
            #endregion

            #region Bắt đầu thực hiện tác vụ
            if (!isDeepLoad)
            {
                waiter.Show();
                waiter.BringToFront();
            }
            Application.DoEvents();
            SwitchControlState(control, waiterIdentity, ControlState.Lock);
            worker.RunWorkerAsync();
            #endregion
        }
        private static Form InitWaitingUI(string identity, string message = "", bool allowCancel = false)
        {
            var form = new Form()
            {
                TopLevel = false,
                Name = identity,
                Anchor = AnchorStyles.None,
                Text = string.Empty,
                ControlBox = false,
                ShowIcon = false,
                ShowInTaskbar = false,
                Width = 300,
                Height = 90,
                FormBorderStyle =
                FormBorderStyle.FixedDialog,
            };
            message = string.IsNullOrEmpty(message) ? "Processing" : message;
            if (!string.IsNullOrEmpty(message))
            {
                Label lblMessage = new Label() { Name = "lblMessage", BackColor = Color.White, ForeColor = Color.SteelBlue, Font = new Font("Tahoma", 10, FontStyle.Bold), Text = message, AutoSize = false, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
                form.Controls.Add(lblMessage);
            }
            form.Controls.Add(new PictureBox() { Name = "pbImage", Image = Properties.Resources.spinner, BackColor = Color.White, SizeMode = PictureBoxSizeMode.StretchImage, Dock = DockStyle.Left, BorderStyle = BorderStyle.None });
            form.SizeChanged += (sender, @event) =>
            {
                form.Controls.Find("pbImage", true).FirstOrDefault().Width = form.Height;
                if (form.Height < 30)
                {
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.Controls.Find("lblMessage", true).FirstOrDefault().Font = new Font("Tahoma", 7, FontStyle.Bold);
                };
                Application.DoEvents();
            };
            return form;
        }
        private static void SwitchControlState(Control container, string identity, ControlState _controlState = ControlState.Lock)
        {
            try
            {
                if ((container.GetType() == typeof(TabControl) || container.GetType().Namespace.Contains("XtraTabControl")))
                {
                    if (container.InvokeRequired)
                        container.Invoke(new Action(delegate ()
                        {
                            SwitchControlState(container.Parent, identity, _controlState);
                        }));
                    else SwitchControlState(container.Parent, identity, _controlState);
                    return;
                }

                foreach (var control in container.Controls)
                {
                    if (((Control)control).Name == identity)
                        continue;
                    if (((Control)control).InvokeRequired)
                    {
                        ((Control)control).Invoke(new MethodInvoker(() =>
                        { ((Control)control).Enabled = _controlState != ControlState.Lock; }));
                        continue;
                    }
                    ((Control)control).Enabled = _controlState != ControlState.Lock;
                }
            }
            catch { }
        }
        private enum ControlState
        {
            Lock = 0,
            Unlock = 1
        }
    }

    public enum WaiterErrorType
    {
        OnCancelNotifyingError = 0,
        OnCompletedNotifyingError = 1,
        OnFocussingError = 2,
        OnBusinessError = 3,
        OnErrorHandingError = 4
    }
}
