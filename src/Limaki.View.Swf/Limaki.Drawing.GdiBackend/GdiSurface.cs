/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */


using Limaki.Drawing.XwtBackend;
using Xwt.Drawing;
using Xwt.GdiBackend;
using Xwt;

namespace Limaki.View.GdiBackend {

    public class GdiSurface : ContextSurface {
        System.Drawing.Graphics _graphics = null;

        public System.Drawing.Graphics Graphics {
            get { return _graphics; }
            set {
                if (_graphics != value) {
                    _graphics = value;
                    if (base.Context != null)
                        base.Context.Dispose ();
                }
            }
        }

        public override Context Context {
            get {
                if (base.Context == null) {
                    var ctx = new GdiContext { Graphics = this.Graphics };
                    base.Context = new Xwt.Drawing.Context(ctx, Toolkit.CurrentEngine);
                }
                return base.Context;
            }
            set {
                base.Context = value;
            }
        }
        public override Matrix Matrix {
            get {
                if (base.Matrix == null) {
                    base.Matrix = this.Graphics.Transform.ToXwt();
                }
                return base.Matrix;
            }
        }
    }
}