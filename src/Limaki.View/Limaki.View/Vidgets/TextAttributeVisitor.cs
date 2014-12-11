/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Xwt.Drawing;

namespace Limaki.View.Vidgets {

    public class TextAttributeVisitor {

        public Action<FontTextAttribute> FontTextAttribute { get; set; }
        public Action<FontDataAttribute> FontDataAttribute { get; set; }
        public Action<FontWeightTextAttribute> FontWeightTextAttribute { get; set; }
        public Action<FontStyleTextAttribute> FontStyleTextAttribute { get; set; }
        public Action<StrikethroughTextAttribute> StrikethroughTextAttribute { get; set; }
        public Action<UnderlineTextAttribute> UnderlineTextAttribute { get; set; }
        public Action<BackgroundTextAttribute> BackgroundTextAttribute { get; set; }
        public Action<ColorTextAttribute> ColorTextAttribute { get; set; }

        public void Visit (TextAttribute a) {
            var ft = a as FontTextAttribute;
            if (ft != null && FontTextAttribute != null) {
                FontTextAttribute (ft);
                return;
            }

            var fdt = a as FontDataAttribute;
            if (fdt != null && FontDataAttribute != null) {
                FontDataAttribute (fdt);
                return;
            }
            var fw = a as FontWeightTextAttribute;
            if (fw != null && FontWeightTextAttribute != null) {
                FontWeightTextAttribute (fw);
                return;
            }

            var fs = a as FontStyleTextAttribute;
            if (fs != null && FontStyleTextAttribute != null) {
                FontStyleTextAttribute (fs);
                return;
            }

            var st = a as StrikethroughTextAttribute;
            if (st != null && StrikethroughTextAttribute != null) {
                StrikethroughTextAttribute (st);
                return;
            }

            var ul = a as UnderlineTextAttribute;
            if (ul != null && UnderlineTextAttribute != null) {
                UnderlineTextAttribute (ul);
                return;
            }

            var bc = a as BackgroundTextAttribute;
            if (bc != null && BackgroundTextAttribute != null) {
                BackgroundTextAttribute (bc);
                return;
            }

            var tc = a as ColorTextAttribute;
            if (tc != null && ColorTextAttribute != null) {
                ColorTextAttribute (tc);
                return;
            }
        }

        public void Visit (IEnumerable<TextAttribute> attributes) { foreach (var a in attributes) Visit (a); }
    }
}