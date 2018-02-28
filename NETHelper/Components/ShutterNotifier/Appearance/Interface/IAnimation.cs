using GenjiCore.Components.ShutterNotifier.Appearance.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Components.ShutterNotifier.Appearance.Interface
{
    public interface IAnimation
    {
        void In(ITheme theme, INotifierPanel notifierPanel);

        void Out(ITheme theme, INotifierPanel notifierPanel);

    }
}
