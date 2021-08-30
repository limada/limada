using System;
using Limaki.View.Vidgets;
using Limaki.View.Viz.UI;
using Xwt;

namespace Limada.View.ContentViewers {

    public class DigidocKeyScrollAction:KeyScrollAction {
        protected override Rectangle ProcessKey(KeyActionEventArgs e) {
            var result = base.ProcessKey(e);
            if (KeyProcessed != null) {
                KeyProcessed(result);
            }
            return result;
        }
        public Action<Rectangle> KeyProcessed { get; set; }
    }
}