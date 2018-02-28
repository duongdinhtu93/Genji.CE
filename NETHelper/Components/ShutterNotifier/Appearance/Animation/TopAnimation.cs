using GenjiCore.Components.ShutterNotifier.Appearance.Interface;
using GenjiCore.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Components.ShutterNotifier.Appearance.Animation
{
    public class TopAnimation : IAnimation
    {
        public void In(ITheme theme, INotifierPanel notifierPanel)
        {
            notifierPanel.Top = -1 * notifierPanel.Height;
            var fadeOut = new Transition(new TransitionType_Deceleration(750));
            fadeOut.add(notifierPanel, "Top", 0);

            Transition.runChain(fadeOut);

            notifierPanel.Visible = true;
        }

        public void Out(ITheme theme, INotifierPanel notifierPanel)
        {
            var fadeOut = new Transition(new TransitionType_Deceleration(250));
            fadeOut.add(notifierPanel, "Top", -1 * notifierPanel.Height);

            Transition.runChain(fadeOut);
        }
    }
}
