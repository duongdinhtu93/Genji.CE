using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Components.ShutterNotifier.Model
{
    public class NotifierMessage
    {
        public string Text { get; private set; }

        public string TextAdditional { get; private set; }

        public IEnumerable<WarningItem> WarningItems { get; private set; }

        public NotifierMessage(string text, string textAdditional, IEnumerable<WarningItem> warningItems)
        {
            Text = text;
            TextAdditional = textAdditional;
            WarningItems = warningItems;
        }
    }
}
