using System;
using Xwt.Drawing;
using Xwt;
using Limaki.Drawing;
using Limaki.View.Viz.Modelling;
using Limaki.Drawing.XwtBackend;
using Limaki.Common;
using Limaki.Drawing.Styles;

namespace Limaki.View.XwtBackend {

    public static class XwtDrawingExtensions {

        public static Image AsImage (Action<Context> render, Size size) {
			using (var ib = new ImageBuilder (size.Width, size.Height)) {
				render (ib.Context);
				return ib.ToBitmap (ImageFormat.ARGB32);
			}
        }

        public static Action<Context> Render (IShape shape, Size size, UiState uiState, IStyleSheet styleSheet) {

            if (styleSheet == null)
                styleSheet = Registry.Pooled<StyleSheets> ().DefaultStyleSheet;

            shape.Size = size;
            var layout = new ShapeLayout (styleSheet);
            var painter = layout.GetPainter (shape.GetType ());
            
            painter.Style = layout.GetStyle (shape, uiState);
            painter.Shape = shape;
            return ctx => painter.Render (new ContextSurface { Context = ctx });
        }

    }
}