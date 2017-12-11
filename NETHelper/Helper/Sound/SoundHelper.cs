using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Helper
{
    public static class SoundHelper
    {
        [DllImport("kernel32.dll")]
        private static extern bool Beep(int freq, int dur);

        public static void PlaySound(int count, int frequency, int duration)
        {
            for (int index = 0; index < count; ++index)
                SoundHelper.Beep(frequency, duration);
        }
    }
}
