using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.View.Vidgets;

namespace Limaki.View.XwtBackend {
    class PrintDialog : IDisposable {
        public PrintDocument Document { get; set; }

        public void Dispose () {
            throw new NotImplementedException();
        }

        internal DialogResult Show () {
            throw new NotImplementedException();
        }
    }
}
