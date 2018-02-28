using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenjiCore.Components.Notifier
{
    internal partial class NotifierPopup : Form
    {
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern System.IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        private static extern bool DeleteObject(System.IntPtr hObject);



        //Các trạng thái animation khác nhau của popup
        public enum TaskbarStates
        {
            hidden = 0,
            appearing = 1,
            visible = 2,
            disappearing = 3
        }

        protected Rectangle WorkAreaRectangle;
        protected System.Timers.Timer timer = new System.Timers.Timer();
        protected TaskbarStates taskbarState = TaskbarStates.hidden;
        protected int nShowEvents;
        protected int nHideEvents;
        protected int nVisibleEvents;
        protected int nIncrementShow;
        protected int nIncrementHide;
        private const int PopupWidth = 245;
        private const int PopupHeight = 80;

        public NotifierPopup()
        {
            InitializeComponent();
            base.Show();
            base.Hide();
            TopMost = true;
            timer.Enabled = true;
            timer.Elapsed += OnTimer;
            this.FormClosing += (sender, @event) =>
                {
                    timer.Stop();
                };
        }

        protected void OnTimer(object sender, System.Timers.ElapsedEventArgs ea)
        {
            switch (taskbarState)
            {
                case TaskbarStates.appearing:
                    if (Height < PopupHeight)
                    {
                        SetBounds(Left, Top - nIncrementShow, Width, Height + nIncrementShow);
                    }
                    else
                    {
                        timer.Stop();
                        Height = PopupHeight;
                        timer.Interval = nVisibleEvents;
                        taskbarState = TaskbarStates.visible;
                        timer.Start();
                    }
                    break;

                case TaskbarStates.visible:
                    timer.Stop();
                    timer.Interval = nHideEvents;
                    taskbarState = TaskbarStates.disappearing;
                    timer.Start();
                    break;

                case TaskbarStates.disappearing:
                    if (Top < WorkAreaRectangle.Bottom)
                        SetBounds(Left, Top + nIncrementHide, Width, Height - nIncrementHide);
                    else
                        Hide();
                    break;
            }

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            timer.Stop();
            this.Close();
        }


        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);
        public void Show(string strTitle, string strContent, int nTimeToShow, int nTimeToStay, int nTimeToHide, Action hyperLink = null)
        {
            lblTitle.Text = strTitle;
            lblContent.Text = strContent;
            WorkAreaRectangle = Screen.GetWorkingArea(WorkAreaRectangle);
            nVisibleEvents = nTimeToStay;
            int nEvents;


            if(hyperLink != null)
            {
                lblContent.Click += (sender, @event) =>
                {
                    try
                    {
                        hyperLink();
                    }
                    catch(Exception exception)
                    {
                        Process.Start("https://stackoverflow.com/search?q=" + exception.Message);
                    }
                };
            }



            if (nTimeToShow > 10)
            {
                nEvents = Math.Min((nTimeToShow / 10), PopupHeight);
                nShowEvents = nTimeToShow / nEvents;
                nIncrementShow = PopupHeight / nEvents;
            }
            else
            {
                nShowEvents = 10;
                nIncrementShow = PopupHeight;
            }
            if (nTimeToHide > 10)
            {
                nEvents = Math.Min((nTimeToHide / 10), PopupHeight);
                nHideEvents = nTimeToHide / nEvents;
                nIncrementHide = PopupHeight / nEvents;
            }
            else
            {
                nHideEvents = 10;
                nIncrementHide = PopupHeight;
            }
            switch (taskbarState)
            {
                case TaskbarStates.hidden:
                    taskbarState = TaskbarStates.appearing;
                    SetBounds(WorkAreaRectangle.Right - PopupWidth - 17, WorkAreaRectangle.Bottom - 1, PopupWidth, 0);
                    timer.Interval = nShowEvents;
                    timer.Start();
                    ShowWindow(this.Handle, 4);
                    break;
                case TaskbarStates.appearing:
                    Refresh();
                    break;

                case TaskbarStates.visible:
                    timer.Stop();
                    timer.Interval = nVisibleEvents;
                    timer.Start();
                    Refresh();
                    break;

                case TaskbarStates.disappearing:
                    timer.Stop();
                    taskbarState = TaskbarStates.visible;
                    SetBounds(WorkAreaRectangle.Right - PopupWidth - 17, WorkAreaRectangle.Bottom - PopupHeight - 1, PopupWidth, PopupHeight);
                    timer.Interval = nVisibleEvents;
                    timer.Start();
                    Refresh();
                    break;
            }
        }

        public new void Hide()
        {
            if (taskbarState != TaskbarStates.hidden)
            {
                timer.Stop();
                taskbarState = TaskbarStates.hidden;
                base.Hide();
            }
        }

        private void lblContent_MouseLeave(object sender, EventArgs e)
        {
            lblContent.BackColor = DefaultBackColor;
        }

        private void lblContent_MouseEnter(object sender, EventArgs e)
        {
            lblContent.BackColor = Color.DeepSkyBlue;
        }
    }
}
