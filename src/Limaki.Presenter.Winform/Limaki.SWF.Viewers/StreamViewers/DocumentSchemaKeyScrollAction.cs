﻿using System;
using Limaki.Drawing;
using Limaki.View.UI;
using Xwt;

namespace Limaki.View.Winform.Controls {

    public class DocumentSchemaKeyScrollAction:KeyScrollAction {
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