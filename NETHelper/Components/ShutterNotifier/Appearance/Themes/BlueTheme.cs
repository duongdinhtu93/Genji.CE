using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GenjiCore.Components.ShutterNotifier.Appearance.Interface;

namespace GenjiCore.Components.ShutterNotifier.Appearance.Themes
{
    public class BlueTheme : ITheme
    {
        public Color BackgroundColor
        {
            get
            {
                return Color.FromArgb(255, 0, 104, 139);
            }
        }

        public Color StartingBackgroundColor
        {
            get
            {
                return Color.White;
            }
        }

        public Color TextColor
        {
            get
            {
                return Color.White;
            }
        }
    }
}
