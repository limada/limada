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
 * http://limada.sourceforge.net
 * 
 */

using Xwt.Drawing;
using Xwt.Gdi;
using Xwt.Gdi.Backend;

namespace Limaki.Drawing.Gdi {

    public class GdiPen:Pen {
        public GdiPen() : base() { }
        public GdiPen ( Color color ) : base (color){}
        protected GdiPen(Pen pen) : base(pen) { }

        System.Drawing.Pen _backend = null;
        public System.Drawing.Pen Backend {
            get {
                if (_backend == null) {
                    _backend = new System.Drawing.Pen(GdiConverter.ToGdi(this.Color));
                    GdiUtils.SetBackend (this, _backend);
                }
                return _backend;
            }
        }
        public override double Thickness {
            get {
                return base.Thickness;
            }
            set {
                if (base.Thickness != value) {
                    base.Thickness = value;
                    GdiUtils.SetBackend(this, _backend);
                }
                
            }
        }
        public override PenLineJoin LineJoin {
            get {
                return base.LineJoin;
            }
            set {
                if (base.LineJoin != value) {
                    base.LineJoin = value;
                    GdiUtils.SetBackend(this, _backend);
                }
            }
        }
        public override Color Color {
            get {
                return base.Color;
            }
            set {
                if (!base.Color.Equals(value)) {
                    base.Color = value;
                    GdiUtils.SetBackend(this, _backend);
                }
            }
        }

        public override PenLineCap EndCap {
            get {
                return base.EndCap;
            }
            set {
                if (base.EndCap != value) {
                    base.EndCap = value;
                    GdiUtils.SetBackend(this, _backend);
                }
            }
        }

        public override PenLineCap StartCap {
            get {
                return base.StartCap;
            }
            set {
                if (base.StartCap != value) {
                    base.StartCap = value;
                    GdiUtils.SetBackend(this, _backend);
                }
            }
        }

        public override object Clone() {
            return new GdiPen(this);
        }

        protected void ClearNative() {
            if (_backend != null) {
                _backend.Dispose();
                _backend = null;
            }
        }

        public override void Dispose(bool disposing) {
            base.Dispose(disposing);
            ClearNative();
        }

        
    }
}