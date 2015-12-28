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

using System.Windows.Forms;
using Limaki.View;
using Limaki.View.Vidgets;
using Xwt.GdiBackend;
using Xwt;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class CanvasVidgetBackend : VidgetBackend<UserControl>, ICanvasVidgetBackend {

        public new ICanvasVidget Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (ICanvasVidget) frontend;
        }

        protected override void Compose () {

            base.Compose ();

            Control.Paint += (s, e) => {
                if (Frontend != null)
                    using (var graphics = new GdiContext (e.Graphics)) {
                        Frontend.DrawContext (new Xwt.Drawing.Context (graphics, Toolkit.CurrentEngine), e.ClipRectangle.ToXwt ());
                    }
            };
        }

    }
}