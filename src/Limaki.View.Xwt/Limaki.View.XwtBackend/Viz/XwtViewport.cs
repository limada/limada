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
using Limaki.Common;
using Limaki.View.Viz;
using Limaki.Drawing;
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
            get { return Backend.Size; }
        }

        public override Size DataSize {
            get { return base.DataSize; }
            set {
                base.DataSize = value;
                UpdateScrollAdjustment (value);
            }
        }

        protected void UpdateScrollAdjustment (Size value) { 
			
            if (reallocating)
                return;
            var upperSize = new Size (Math.Ceiling (value.Width), Math.Ceiling (value.Height));
            if (hscroll != null && Math.Ceiling (hscroll.UpperValue) != upperSize.Width)
                hscroll.UpperValue = upperSize.Width;
            if (vscroll != null && Math.Ceiling (vscroll.UpperValue) != upperSize.Height)
                vscroll.UpperValue = upperSize.Height;
        }

        ScrollAdjustment hscroll;
        ScrollAdjustment vscroll;

        public void SetScrollAdjustments (ScrollAdjustment horizontal, ScrollAdjustment vertical) {
            hscroll = horizontal;
            vscroll = vertical;

            OnBoundsChanged ();

            Action<object, EventArgs> changed = (s, e) => {
                base.ClipOrigin = new Point (hscroll.Value, vscroll.Value);
                Backend.QueueDraw ();
            };

            hscroll.UpperValue = Math.Ceiling (DataSize.Width);
            hscroll.ValueChanged += (s, e) => changed (s, e);
            vscroll.UpperValue = Math.Ceiling (DataSize.Height);
            vscroll.ValueChanged += (s, e) => changed (s, e);

        }

        public void OnBoundsChanged () {
            if (vscroll == null || hscroll == null)
                return;

            vscroll.PageSize = vscroll.PageIncrement = ClipSize.Height;
            hscroll.PageSize = hscroll.PageIncrement = ClipSize.Width;
            base.ClipOrigin = new Point (hscroll.Value, vscroll.Value);
        }

        bool reallocating = false;

        public virtual void OnReallocate () {
            reallocating = true;
            var zoom = ZoomFactor;
            UpdateZoom ();
            var zoomChanged = zoom != ZoomFactor;
            Backend.QueueForReallocate ();
            // (Backend as IVidgetBackend).QueueDraw ();

            reallocating = false;
            if (zoomChanged) {
                Application.MainLoop.QueueExitAction (() => UpdateScrollAdjustment (DataSize));
            }        
        }

        public override void UpdateZoom () {
            base.UpdateZoom ();
        }

        public override void Update () {
            base.Update ();
            Backend.QueueDraw ();
        }
    }
}
