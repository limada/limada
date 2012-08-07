using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;
using Xwt.Drawing;
using Xwt.Engine;

namespace Limaki.XwtAdapter {

    public class XwtToChange {
        public void ChangeMe() {
            WidgetRegistry.MainRegistry.CreateSharedBackend<object>(typeof (object));
            WidgetRegistry.MainRegistry.CreateBackend<object>(typeof(object));
            var c = new Canvas();
            new Context(new object());
            new Font(new object());

        }
    }
}
