using System;
using Limaki.Drawing;
using Limaki.Presenter.UI;

namespace Limaki.Presenter.Winform.Controls {
    public class DocumentSchemaKeyScrollAction:KeyScrollAction {
        protected override RectangleI ProcessKey(KeyActionEventArgs e) {
            var result = base.ProcessKey(e);
            if (KeyProcessed != null) {
                KeyProcessed(result);
            }
            return result;
        }
        public Action<RectangleI> KeyProcessed { get; set; }
    }
}