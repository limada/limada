using System;
using System.Drawing;
using Limaki.View;
using Limaki.Drawing.GDI;
using Limaki.View.Clipping;
using Limaki.View.Rendering;

namespace Limaki.View.GDI.UI {
    public class GDIRenderEventArgs : RenderEventArgs {
        private Graphics graphics = null;
        #region Public Constructors
        public GDIRenderEventArgs(Graphics graphics, Rectangle clipRect) {
            if (graphics == null)
                throw new ArgumentNullException("graphics");
            this.graphics = graphics;
            this._clipper = new BoundsClipper(GDIConverter.Convert(clipRect));
            this._surface = new GDISurface() { Graphics = graphics };
        }

        #endregion	// Public Constructors

        /// <summary>
        /// sets the graphics
        ///  </summary>
        /// <param name="g"></param>
        /// <returns>the previouse graphics</returns>
        internal Graphics SetGraphics(Graphics g) {
            var res = graphics;
            graphics = g;

            return res;
        }
    }
}