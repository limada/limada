/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Windows.Forms;
using Limaki.Common;
using Xwt.Gdi.Backend;
using Limaki.Viewers.Vidgets;
using Xwt;

namespace Limaki.View.Swf.Backends {

    public class CanvasVidgetBackend : UserControl, ICanvasVidgetBackend, IDragDopControl {

        protected override void OnPaint (PaintEventArgs e) {

            base.OnPaint(e);

            if (Frontend != null)
                using (var graphics = new GdiContext(e.Graphics)) {
                    Frontend.DrawContext(new Xwt.Drawing.Context(graphics, Toolkit.Engine<GdiEngine>()), e.ClipRectangle.ToXwt());
                }
        }


        #region IVidgetBackend-Implementation

        public ICanvasVidget Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (ICanvasVidget) frontend;
        }

        Xwt.Rectangle IVidgetBackend.ClientRectangle {
            get { return this.ClientRectangle.ToXwt(); }
        }

        Xwt.Size IVidgetBackend.Size {
            get { return this.Size.ToXwt(); }
        }

        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate(rect.ToGdi());
        }

        Xwt.Point IDragDopControl.PointToClient (Xwt.Point source) { return PointToClient(source.ToGdi()).ToXwt(); }

        #endregion

    }
}