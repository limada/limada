using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;
using Xwt.Drawing;
using Xwt.Engine;
using Xwt.Backends;

namespace Limaki.XwtAdapter {

    public class XwtToChange {
        public void ChangeMe() {
            WidgetRegistry.MainRegistry.CreateSharedBackend<object>(typeof (object));
            WidgetRegistry.MainRegistry.CreateBackend<object>(typeof(object));
            WidgetRegistry.MainRegistry.Clear();
            WidgetRegistry.MainRegistry = null;
            var c = new Canvas();
            //new Context(new object());
            new Font(WidgetRegistry.MainRegistry, new object());

        }
    }
}
