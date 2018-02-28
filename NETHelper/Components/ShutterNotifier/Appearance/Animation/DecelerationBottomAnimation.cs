using GenjiCore.Components.ShutterNotifier.Appearance.Interface;
using GenjiCore.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenjiCore.Components.ShutterNotifier.Appearance.Animation
{
    public class DecelerationBottomAnimation : IAnimation
    {
        public void In(ITheme theme, INotifierPanel notifierPanel)
        {
            int borderSizeCorrected = notifierPanel.Parent.Height;

            // If parent is a form we need this trick to get correct height
            if (notifierPanel.Parent is Form)
            {
                borderSizeCorrected = (notifierPanel.Parent as Form).ClientSize.Height;
            }

            notifierPanel.Top = notifierPanel.Parent.Height;
            var fadeOut = new Transition(new TransitionType_Deceleration(750));
            fadeOut.add(notifierPanel, "Top", borderSizeCorrected - notifierPanel.Height);

            Transition.runChain(fadeOut);

            notifierPanel.Visible = true;
        }

        public void Out(ITheme theme, INotifierPanel notifierPanel)
        {
            var fadeOut = new Transition(new TransitionType_Deceleration(250));
            fadeOut.add(notifierPanel, "Top", notifierPanel.Parent.Height);

            Transition.runChain(fadeOut);
        }
    }
}
