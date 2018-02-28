using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenjiCore.Components.ShutterNotifier.Model
{
    public class WarningItem
    {
        public string Text { get; private set; }

        public Control DependentControl { get; private set; }


        public WarningItem(string text)
        {
            Text = text;
            DependentControl = null;
        }

        public WarningItem(string text, Control dependentControl)
        {
            Text = text;
            DependentControl = dependentControl;
        }

    }
}
