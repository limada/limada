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

using System;
using System.Collections.Generic;
using Limaki.Drawing;
//using System.Runtime.Serialization;

namespace Limaki.Widgets {
#if !SILVERLIGHT
    [Serializable]
#endif
    public class Widget<T> : IWidget  {
        //public Widget () {}
        public Widget():this(default(T)){}

        public Widget(T data) { Data = data; }

        private T _data = default( T );
        public virtual T Data {
            get { return _data; }
            set { _data = value; }
        }

        
        object IWidget.Data {
            get { return this.Data; }
            set {
                if (value is T) {
                    this.Data = (T)value;
                }
            }
        }

        //[NonSerialized] 
        private IStyle _style;
        public virtual IStyle Style {
            get { return _style; }
            set { _style = value; }
        }

        private IShape _shape;
        public virtual IShape Shape {
            get { return _shape; }
            set { _shape = value; }
        }


        public override string ToString() {
            if (Data != null)
                return //this.GetType().Name+"("+
                    Data.ToString();
            //+")";
            else
                return ( (char) ( 0x2260 ) ).ToString ();
        }

        public virtual SizeI Size {
            get {
                if (Shape != null)
                    return Shape.Size;
                else
                    return new SizeI();
            }
            set { Shape.Size = value; }
        }

        public virtual PointI Location {
            get {
                if (Shape != null) {
                    return Shape.Location;
                } else
                    return new PointI();
            }
            set { Shape.Location = value; }
        }

        public virtual PointI[] Hull(int delta, bool extend) {
            if (Shape != null)
                return Shape.Hull(delta, extend);
            else
                return new PointI[0];
        }

        public virtual PointI[] Hull(Matrice matrix, int delta, bool extend) {
            if (Shape != null)
                return Shape.Hull(delta, extend);
            else
                return new PointI[0];
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

    public class ToolWidget<T>:Widget<T>,IToolWidget {
        public ToolWidget(T data):base(data){}
    }
}
