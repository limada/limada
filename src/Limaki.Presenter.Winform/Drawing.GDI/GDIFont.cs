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


//using System.Drawing;
using Limaki.Drawing;


namespace Limaki.Drawing.GDI {
    public class GDIFont:Font {
        public GDIFont():base(){}
        protected GDIFont(Font font):base(font) {
        }

        public GDIFont(System.Drawing.Font font){
            GDIUtils.SetFont (this,font);
            this._native = font;
        }

        System.Drawing.Font _native = null;
        public System.Drawing.Font Native {
            get {
                if (_native == null) {
                    System.Drawing.FontStyle style = GDIUtils.GetFontStyle (this);
                    _native = new System.Drawing.Font (
                        this.Family, 
                        (float) this.Size, 
                        style,
                        System.Drawing.GraphicsUnit.Point);
                    base.Size = _native.Size;
                    base.Family = _native.Name;
                }
                return _native;
            }
        }

        protected void ClearNative() {
            if (_native != null) {
                _native.Dispose();
                _native = null;
            }
        }

        public override string Family {
            get {
                if (_native != null) {
                    return _native.Name;
                } else {
                    return base.Family;
                }
            }
            set {
                if (base.Family != value){
                    ClearNative ();
                    base.Family = value;
                }
            }
        }

        public override double Size {
            get { return base.Size; }
            set {
                if (base.Size != value) {
                    ClearNative();
                    base.Size = value;
                }
                
            }
        }

        public override FontStyle Style {
            get { return base.Style; }
            set {
                if (base.Style != value) {
                    ClearNative();
                    base.Style = value;
                }
            }
        }

        //public override FontWeight Weight {
        //    get { return base.Weight; }
        //    set {
        //        if (base.Weight != value) {
        //            ClearNative();
        //            base.Weight = value;
        //        }
        //    }
        //}

        public override void Dispose(bool disposing) {
            base.Dispose(disposing);
            ClearNative ();
        }

        public override object Clone() {
            return new GDIFont(this);
        }
    }
}