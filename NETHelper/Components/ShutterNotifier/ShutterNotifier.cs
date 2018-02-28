using GenjiCore.Components.ShutterNotifier.Appearance;
using GenjiCore.Components.ShutterNotifier.Appearance.Interface;
using GenjiCore.Components.ShutterNotifier.Appearance.Subcontrols;
using GenjiCore.Components.ShutterNotifier.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenjiCore.Components.ShutterNotifier
{
    public partial class ShutterNotifier : Component
    {

        #region Public properties

        [Browsable(true)]
        public Control ParentControl { get; set; }

        [Browsable(true)]
        public Constants.Themes Theme { get; set; }

        [Browsable(true)]
        public Constants.Icons Icon { get; set; }

        [Browsable(true)]
        public Constants.Animations Animation { get; set; }

        [Browsable(true)]
        public Constants.ClickableControls ConcealmentMethod { get; set; }

        [Browsable(true)]
        public bool IsWarningListItemClickable { get; set; }

        #endregion


        #region Private declarations

        private INotifierPanel _notifierArea;

        #endregion

        #region Public actions

        #region Overrides
        public void In(string text)
        {
            In(new NotifierMessage(text, "", new List<WarningItem>()));
        }

        public void In(string text, string textAdditional)
        {
            In(new NotifierMessage(text, textAdditional, new List<WarningItem>()));
        }

        public void In(string text, string textAdditional, IEnumerable<string> warningItems)
        {
            In(new NotifierMessage(text,
                                   textAdditional,
                                   warningItems.Select(x => new WarningItem(x)).ToList()));
        }

        public void In(string text, IEnumerable<string> warningItems)
        {
            In(new NotifierMessage(text,
                                   "",
                                   warningItems.Select(x => new WarningItem(x)).ToList()));
        }

        public void In(string text, string textAdditional, IEnumerable<WarningItem> warningItems)
        {
            In(new NotifierMessage(text,
                                   textAdditional,
                                   warningItems));
        }

        public void In(string text, IEnumerable<WarningItem> warningItems)
        {
            In(new NotifierMessage(text,
                                   "",
                                   warningItems));
        }

        #endregion

        public void In(NotifierMessage message)
        {
            CreateNotifyerArea();

            _notifierArea.SetTheme(EnumMapper.GetThemeByName(Theme));

            _notifierArea.SetIcon(EnumMapper.GetIconByName(Icon));

            _notifierArea.SetText(message);

            AnimateIn(EnumMapper.GetAnimationByName(Animation));

        }

        public void Out()
        {
            AnimateOut(EnumMapper.GetAnimationByName(Animation));
        }

        #endregion


        #region Private methods

        #region In

        private void AnimateIn(IAnimation animation)
        {
            animation.In(EnumMapper.GetThemeByName(Theme), _notifierArea);
        }

        #endregion

        #region Out

        private void AnimateOut(IAnimation animation)
        {
            animation.Out(EnumMapper.GetThemeByName(Theme), _notifierArea);
        }

        #endregion

        #region Factory

        private void CreateNotifyerArea()
        {
            if (ParentControl == null) throw new ArgumentNullException("ParentControl");

            if (_notifierArea == null)
            {
                _notifierArea = new NotifierPanel { Width = ParentControl.Width, Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top, Visible = false };
                if (ParentControl.Height < _notifierArea.Height)
                    _notifierArea.Height = ParentControl.Height;

                ParentControl.Controls.Add(_notifierArea as Control); // ...eh

                _notifierArea.HideNotifierPanelEvent += (o, k) => Out();

                ParentControl.Click += (o, k) => Out();

            }

            _notifierArea.Visible = false;

            _notifierArea.SetClickableArea(ConcealmentMethod);

            _notifierArea.BringToFront();

        }

        #endregion

        #endregion



        public ShutterNotifier()
        {
            InitializeComponent();
        }

        public ShutterNotifier(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
