using Limaki.Drawing;
using Limaki.Drawing.XwtBackend;
using Limaki.View.Viz.Rendering;
using Xwt.Html5.Backend;
using Xwt.Drawing;
using Xwt;
using System;

//using Matrice = Limaki.Drawing.Matrice;

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
                    base.Context = new Xwt.Drawing.Context(ctx, Toolkit.Engine<Html5Engine>());
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
                    base.Matrix = new Matrix ();
                }
                return base.Matrix;
            }
            protected set {
                base.Matrix = value;
            }
        }
    }
}