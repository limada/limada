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
            WidgetRegistry.CreateSharedBackend<object>(typeof (object));
            WidgetRegistry.CreateBackend<object>(typeof (object));
            var c = new Canvas();
            new Xwt.Drawing.Context(new object());
            new Xwt.Drawing.Font(new object());

        }
    }
}
