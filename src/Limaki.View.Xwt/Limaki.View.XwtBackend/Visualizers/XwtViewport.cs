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
using System.Linq;
using System.Text;
using Limaki.View.Visualizers;
using Limaki.Common;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class XwtViewport : Viewport {
        private DisplayBackend Backend;

        public XwtViewport (DisplayBackend Backend) {
            this.Backend = Backend;
        }

        public override Point ClipOrigin {
            get { return base.ClipOrigin; }
            set {
                if (hscroll != null)
                    hscroll.Value = value.X;
                if (vscroll != null)
                    vscroll.Value = value.Y;
                base.ClipOrigin = value;
            }
        }

        public override Size ClipSize {
            get { return Backend.Bounds.Size; }
        }

        public override Size DataSize {
            get { return base.DataSize; }
            set {
                base.DataSize = value;
                if (hscroll != null && hscroll.UpperValue != value.Width)
                    hscroll.UpperValue = value.Width;
                if (vscroll != null && vscroll.UpperValue != value.Height)
                    vscroll.UpperValue = value.Height;
            }
        }

        ScrollAdjustment hscroll;
        ScrollAdjustment vscroll;

        public void SetScrollAdjustments (ScrollAdjustment horizontal, ScrollAdjustment vertical) {
            hscroll = horizontal;
            vscroll = vertical;

            OnBoundsChanged();

            Action<object, EventArgs> changed = (s, e) => {
                base.ClipOrigin = new Point(hscroll.Value, vscroll.Value);
                Backend.QueueDraw();
            };

            hscroll.UpperValue = DataSize.Width;
            hscroll.ValueChanged += (s, e) => changed(s, e);
            vscroll.UpperValue = DataSize.Height;
            vscroll.ValueChanged += (s, e) => changed(s, e);

        }

        public void OnBoundsChanged () {
            if (vscroll == null)
                return;
            vscroll.PageSize = vscroll.PageIncrement = ClipSize.Height;
            hscroll.PageSize = hscroll.PageIncrement = ClipSize.Width;
            base.ClipOrigin = new Point(hscroll.Value, vscroll.Value);
        }


    }
}
