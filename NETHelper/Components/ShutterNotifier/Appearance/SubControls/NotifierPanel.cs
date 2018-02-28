using GenjiCore.Components.ShutterNotifier.Appearance.Interface;
using GenjiCore.Components.ShutterNotifier.Model;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GenjiCore.Components.ShutterNotifier.Appearance.Subcontrols
{
    
    public partial class NotifierPanel : UserControl, INotifierPanel
    {

        #region Declarations

        public event EventHandler<EventArgs> HideNotifierPanelEvent;

        private Constants.ClickableControls _concealmentMethod;

        #endregion

        #region Events

        private void CommonNotifyerPanelClick(object sender, EventArgs e)
        {
            if (_concealmentMethod == Constants.ClickableControls.Area)
            {
                HideNotifyerPanel();
            }
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            HideNotifyerPanel();
        }

        #endregion

        #region Private methods
            
        private void HideNotifyerPanel()
        {
            if (HideNotifierPanelEvent != null)
            {
                HideNotifierPanelEvent(this, null);
            }
        }

        #endregion

        #region Public methods

        public void SetClickableArea(Constants.ClickableControls concealmentMethod)
        {
            _concealmentMethod = concealmentMethod;

            switch (concealmentMethod)
            {
                case Constants.ClickableControls.Area:
                    this.Cursor = Cursors.Hand;
                    this.btnHide.Visible = false;
                    break;
                case Constants.ClickableControls.Button:
                    this.Cursor = Cursors.Default;
                    this.btnHide.Visible = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("concealmentMethod");
            }
        }

        public void SetTheme(ITheme theme)
        {
            this.BackColor = theme.BackgroundColor;
            this.ForeColor = theme.TextColor;
        }

        public void SetIcon(Image icon)
        {
            this.pbIcon.Image = icon;
        }

        public void SetText(NotifierMessage message)
        {
            this.lblText.Text = message.Text;
            this.lblText.Font = new Font(lblText.Font, FontStyle.Bold);
            this.Height = this.lblText.Height + this.lblText.Location.Y * 2;
           

            if (!string.IsNullOrEmpty(message.TextAdditional))
            {
                this.lblTextAdditional.Text = message.TextAdditional;

                this.Height = this.lblTextAdditional.Location.Y + this.lblTextAdditional.Height + this.lblText.Location.Y;
            }

            if (message.WarningItems.Any())
            {
                int y = lblText.Location.Y * 2 + lblText.Height;
                int index = 0;
                if (!String.IsNullOrEmpty(message.TextAdditional)) y = lblTextAdditional.Location.Y + lblTextAdditional.Height * 2;
                foreach (var warningItem in message.WarningItems)
                {
                    y += (index++) * lblText.Height;

                    var tempLabelSeparator = new System.Windows.Forms.Label { Location = new Point(lblText.Location.X, y), Text = "-", AutoSize = true};
                    var tempLabelText = new Label { Location = new Point(lblText.Location.X + 12, y), Text = warningItem.Text, AutoSize = true };
                    
                    if (warningItem.DependentControl != null)
                    {
                        var item = warningItem;
                        tempLabelText.Cursor = Cursors.Hand;
                        tempLabelText.Click += (o, k) => { HideNotifyerPanel();
                                                           item.DependentControl.Focus(); };
                    }

                    this.Controls.Add(tempLabelSeparator);
                    this.Controls.Add(tempLabelText);
                }
                this.Height = y + this.lblText.Height + this.lblText.Location.Y;

            }
        }

        #endregion

        #region Constructor

        public NotifierPanel()
        {
            InitializeComponent();
        }

        #endregion

    }
}
