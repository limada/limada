using System;
using System.Drawing;
using Limaki.Presenter;
using Limaki.Presenter.UI;
using Limaki.Drawing.GDI;

namespace Limaki.Presenter.GDI.UI {
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