using GenjiCore.Components.ShutterNotifier.Appearance.Interface;
using GenjiCore.Components.ShutterNotifier.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenjiCore.Components.ShutterNotifier.Appearance.Interface
{
    public interface INotifierPanel
    {
        void SetTheme(ITheme theme);

        void SetText(NotifierMessage message);

        void SetIcon(System.Drawing.Image icon);

        void SetClickableArea(Constants.ClickableControls concealmentMethod);

        event EventHandler<EventArgs> HideNotifierPanelEvent;

        bool Visible { get; set; }

        int Top { get; set; }

        int Height { get; set; }

        Color BackColor { get; set; }

        Control Parent { get; set; }

        void BringToFront();

    }
}
