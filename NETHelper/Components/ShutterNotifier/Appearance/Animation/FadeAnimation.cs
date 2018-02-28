using GenjiCore.Components.ShutterNotifier.Appearance.Interface;
using GenjiCore.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Components.ShutterNotifier.Appearance.Animation
{
    public class FadeAnimation : IAnimation
    {
        public void In(ITheme theme, INotifierPanel notifierPanel)
        {
            notifierPanel.Top = 0;
            notifierPanel.BackColor = theme.StartingBackgroundColor;
            var fadeIn = new TransitionType_Linear(800);
            Transition.run(notifierPanel, "BackColor", theme.BackgroundColor, fadeIn);

            notifierPanel.Visible = true;
        }

        public void Out(ITheme theme, INotifierPanel notifierPanel)
        {
            notifierPanel.Visible = false;
        }

    }
}
