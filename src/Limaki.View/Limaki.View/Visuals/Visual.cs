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
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Drawing;
using Xwt;
using Xwt.Drawing;

//using System.Runtime.Serialization;

namespace Limaki.View.Visuals {
#if !SILVERLIGHT
    [Serializable]
#endif
    public class Visual<T> : IVisual  {
        //public Visual () {}
        public Visual():this(default(T)){}

        public Visual(T data) { Data = data; }

        public virtual IStyleGroup Style { get; set; }
        public virtual IShape Shape { get; set; }

        public virtual T Data { get; set; }
        object IVisual.Data {
            get { return this.Data; }
            set {
                if (value is T) {
                    this.Data = (T)value;
                }
            }
        }

        public override string ToString() {
            if (Data != null)
                return //this.GetType().Name+"("+
                    Data.ToString();
            //+")";
            else
                return ( (char) ( 0x2260 ) ).ToString ();
        }

        public virtual Size Size {
            get {
                if (Shape != null)
                    return Shape.Size;
                else
                    return new Size();
            }
            set { Shape.Size = value; }
        }

        public virtual Point Location {
            get {
                if (Shape != null) {
                    return Shape.Location;
                } else
                    return new Point();
            }
            set { Shape.Location = value; }
        }

        public virtual Point[] Hull(int delta, bool extend) {
            if (Shape != null)
                return Shape.Hull(delta, extend);
            else
                return new Point[0];
        }

        public virtual Point[] Hull(Matrix matrix, int delta, bool extend) {
            if (Shape != null)
                return Shape.Hull(delta, extend);
            else
                return new Point[0];
        }

    }
    
}
