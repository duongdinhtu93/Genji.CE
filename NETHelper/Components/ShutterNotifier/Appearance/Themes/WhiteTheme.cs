using GenjiCore.Components.ShutterNotifier.Appearance.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Components.ShutterNotifier.Appearance.Themes
{
    public class WhiteTheme : ITheme
    {
        public Color BackgroundColor
        {
            get { return Color.White; }
        }

        public Color TextColor
        {
            get { return Color.Black; }
        }

        public Color StartingBackgroundColor
        {
            get { return Color.Gray; }
        }
    }
}
