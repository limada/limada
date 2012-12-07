﻿using Limaki.Drawing;
using Xwt.Html5.Backend;
using Xwt.Drawing;
using Xwt;
using Limaki.View.Rendering;
using System;
using Limaki.View.Clipping;

namespace Limaki.View.Html5 {

    public class Html5RenderEventArgs : RenderEventArgs {

        private IHtmlChunk _html = null;

        public Html5RenderEventArgs(IHtmlChunk html, Rectangle clipRect) {
            if (html == null)
                throw new ArgumentNullException("html");
            this._html = html;
            this._clipper = new BoundsClipper(clipRect);
            this._surface = new Html5Surface { Html = html };
        }

    }

    public class Html5Surface : ContextSurface {
        IHtmlChunk _html = null;

        public IHtmlChunk Html {
            get { return _html; }
            set {
                if (_html != value) {
                    _html = value;
                    if (base.Context != null)
                        base.Context.Dispose ();
                }
            }
        }

        public override Context Context {
            get {
                if (base.Context == null) {
                    var ctx = new Html5Context { Context = this.Html };
                    base.Context = new Xwt.Drawing.Context (ctx);
                }
                return base.Context;
            }
            set {
                base.Context = value;
            }
        }
        public override Matrice Matrix {
            get {
                if (base.Matrix == null) {
                    base.Matrix = new Matrice ();
                }
                return base.Matrix;
            }
            set {
                base.Matrix = value;
            }
        }
    }
}