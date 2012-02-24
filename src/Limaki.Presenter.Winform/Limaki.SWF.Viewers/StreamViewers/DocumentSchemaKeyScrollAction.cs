using System;
using Limaki.Drawing;
using Limaki.View.UI;
using Xwt;

namespace Limaki.View.Winform.Controls {

    public class DocumentSchemaKeyScrollAction:KeyScrollAction {
        protected override RectangleD ProcessKey(KeyActionEventArgs e) {
            var result = base.ProcessKey(e);
            if (KeyProcessed != null) {
                KeyProcessed(result);
            }
            return result;
        }
        public Action<RectangleD> KeyProcessed { get; set; }
    }
}