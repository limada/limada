using Limaki.View.Vidgets;
using Limaki.Viewers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Limaki.View.Headless.VidgetBackends {

    public class HeadlessMessageBoxShow : IMessageBoxShow {

        public DialogResult Show (string title, string text, MessageBoxButtons buttons) {
            Trace.WriteLine (string.Format ("{0} {1} {2}", title, text, buttons));
            return DialogResult.None;
        }
    }
}
