using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Limaki.View.XwtBackend {
    class PrintDialog : IDisposable {
        public PrintDocument Document { get; set; }

        public void Dispose () {
            throw new NotImplementedException();
        }

        internal Viewers.DialogResult Show () {
            throw new NotImplementedException();
        }
    }
}
