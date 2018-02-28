using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Components.ShutterNotifier.Appearance.Interface
{
    public interface  ITheme
    {
        Color BackgroundColor { get; }

        Color TextColor { get; }

        Color StartingBackgroundColor { get; }
    }
}
