using GenjiCore.Components.ShutterNotifier.Appearance;
using GenjiCore.Components.ShutterNotifier.Appearance.Animation;
using GenjiCore.Components.ShutterNotifier.Appearance.Interface;
using GenjiCore.Components.ShutterNotifier.Appearance.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Components.ShutterNotifier
{
    public static class EnumMapper
    {
        public static ITheme GetThemeByName(Constants.Themes theme)
        {
            switch (theme)
            {
                case Constants.Themes.DarkGray: return new DarkGrayTheme();
                case Constants.Themes.White: return new WhiteTheme();
                case Constants.Themes.Blue: return new BlueTheme();
                case Constants.Themes.Orange: return new OrangeTheme();
                default:
                    throw new ArgumentOutOfRangeException("theme");
            }
        }

        public static Image GetIconByName(Constants.Icons icon)
        {
            switch (icon)
            {
                case Constants.Icons.Nothing: return new Bitmap(16, 16);
                case Constants.Icons.Warning: return Properties.Resources.warning_triangle;
                case Constants.Icons.Information: return Properties.Resources.information;
                case Constants.Icons.Error: return Properties.Resources.error;
                case Constants.Icons.ThumbUp: return Properties.Resources.thumb_up;
                case Constants.Icons.ThumbDown: return Properties.Resources.thumb_down;
                case Constants.Icons.Smile: return Properties.Resources.smiley_happy;
                case Constants.Icons.MarkerBlue: return Properties.Resources.marker_rounded_light_blue;
                case Constants.Icons.MarkerRed: return Properties.Resources.marker_rounded_red;
                case Constants.Icons.MarkerYellow: return Properties.Resources.marker_rounded_yellow;
                case Constants.Icons.StarFull: return Properties.Resources.star_full;
                case Constants.Icons.StarHalf: return Properties.Resources.star_half;
                default:
                    throw new ArgumentOutOfRangeException("icon");
            }
        }

        public static IAnimation GetAnimationByName(Constants.Animations animation)
        {
            switch (animation)
            {
                case Constants.Animations.FadeIn: return new FadeAnimation();
                case Constants.Animations.DecelerationTop: return new TopAnimation();
                case Constants.Animations.DecelerationBottom: return new DecelerationBottomAnimation();
                default:
                    throw new ArgumentOutOfRangeException("animation");
            }
        }
    }
}
