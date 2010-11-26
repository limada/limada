/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

namespace Limaki.Drawing.GDI {
    public class GDIPen:Pen {


        public GDIPen() : base() { }
        public GDIPen ( Color color ) : base (color){}
        protected GDIPen(Pen pen) : base(pen) { }

        System.Drawing.Pen _natvive = null;
        public System.Drawing.Pen Native {
            get {
                if (_natvive == null) {
                    _natvive = new System.Drawing.Pen(GDIConverter.Convert(this.Color));
                    GDIUtils.SetNativePen (this, _natvive);
                }
                return _natvive;
            }
        }
        public override double Thickness {
            get {
                return base.Thickness;
            }
            set {
                if (base.Thickness != value) {
                    base.Thickness = value;
                    GDIUtils.SetNativePen(this, _natvive);
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
                    GDIUtils.SetNativePen(this, _natvive);
                }
            }
        }
        public override Color Color {
            get {
                return base.Color;
            }
            set {
                if (base.Color != value) {
                    base.Color = value;
                    GDIUtils.SetNativePen(this, _natvive);
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
                    GDIUtils.SetNativePen(this, _natvive);
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
                    GDIUtils.SetNativePen(this, _natvive);
                }
            }
        }

        public override object Clone() {
            return new GDIPen(this);
        }

        protected void ClearNative() {
            if (_natvive != null) {
                _natvive.Dispose();
                _natvive = null;
            }
        }

        public override void Dispose(bool disposing) {
            base.Dispose(disposing);
            ClearNative();
        }

        
    }
}