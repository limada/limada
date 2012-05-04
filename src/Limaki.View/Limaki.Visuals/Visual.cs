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

//using System.Runtime.Serialization;

namespace Limaki.Visuals {
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

        public virtual Point[] Hull(Matrice matrix, int delta, bool extend) {
            if (Shape != null)
                return Shape.Hull(delta, extend);
            else
                return new Point[0];
        }

        #region IShape Member



        //public Type ShapeDataType {
        //    get {
        //        if (Shape != null)
        //            return Shape.ShapeDataType;
        //        else
        //            return null;
        //    }
        //}

        //public Point this[Anchor i] {
        //    get {
        //        if (Shape != null)
        //            return Shape[i];
        //        throw new ArgumentException("Shape must not be null.");
        //    }
        //    set {
        //        if (Shape != null)
        //            Shape[i]=value;
        //        else
        //            throw new ArgumentException("Shape must not be null.");
        //    }
        //}

        //public Anchor IsAnchorHit(Point p, int hitSize) {
        //    if (Shape != null)
        //        return Shape.IsAnchorHit(p,hitSize);
        //    throw new ArgumentException("Shape must not be null.");
        //}

        //public bool IsBorderHit(Point p, int hitSize) {
        //    if (Shape != null)
        //        return Shape.IsBorderHit(p,hitSize);
        //    throw new ArgumentException("Shape must not be null.");
        //}

        //public bool IsHit(Point p, int hitSize) {
        //    if (Shape != null)
        //        return Shape.IsHit(p,hitSize);
        //    throw new ArgumentException("Shape must not be null.");
        //}

        //public void Transform(Matrice matrice) {
        //    if (Shape != null)
        //        Shape.Transform(matrice);
        //    throw new ArgumentException("Shape must not be null.");
        //}

        //public Rectangle BoundsRect {
        //    get {
        //        if (Shape != null)
        //            return Shape.BoundsRect;
        //        throw new ArgumentException("Shape must not be null.");
        //    }
        //}

        //public IEnumerable<Anchor> Grips {
        //    get {
        //        if (Shape != null)
        //            return Shape.Grips;
        //        throw new ArgumentException("Shape must not be null.");
        //    }
        //}

        //public Point[] Hull(int delta, bool extend) {
        //    if (Shape != null)
        //        return Shape.Hull(delta,extend);
        //    throw new ArgumentException("Shape must not be null.");
        //}

        //public Point[] Hull(Matrice matrix, int delta, bool extend) {
        //    if (Shape != null)
        //        return Shape.Hull(matrix,delta,extend);
        //    throw new ArgumentException("Shape must not be null.");
        //}

        #endregion


    }

    public class VisualTool<T>:Visual<T>,IVisualTool {
        public VisualTool(T data):base(data){}
    }
}
