using Limaki.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Limaki.View.Headless.VidgetBackends {
    public class HeadlessProgressHandler : IProgressHandler {
        public void Write (string m, int progress, int count, params object[] param) {
            Trace.WriteLine ("Progress " + string.Format (m, param));
        }

        public void Show (string m) {
            Trace.WriteLine ("Progress " + m);
        }

        public void Close () {
            
        }
    }
}
