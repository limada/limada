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
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;

namespace Limaki.Widgets {
    public class WidgetComposite<T>:Widget<T>, IComposite<IWidget> 
        where T:ICollection<IWidget> {
        public WidgetComposite(T data):base(data) {}

        #region IWidget Member

        public override IShape Shape {
            get {
                if(base.Shape == null) {
                    base.Shape = new RectangleShape ();
                }
                return base.Shape;
            }
            set {
                base.Shape = value;
            }
        }
        public override T Data {
            get { return base.Data; }
            set { base.Data = value; }
        }

        public override SizeI Size {
            get {
                if (base.Shape != null)
                    return base.Shape.Size;
                else
                    return new SizeI();
            }
            set { base.Shape.Size = value; }
        }

        public override PointI Location {
            get {
                if (base.Shape != null) {
                    return base.Shape.Location;
                } else
                    return new PointI();
            }
            set {
                PointI oldLocation = base.Location;
                base.Location = value;
                int dx = oldLocation.X - value.X;
                int dy = oldLocation.Y - value.Y;
                foreach(IWidget widget in Data) {
                    widget.Location.Offset(dx,dy);
                }
            
            }
        }
        #endregion

        #region IComposite member

        public virtual void Add(IWidget widget) {
            if (widget!=null && !Data.Contains(widget)) {
                Data.Add (widget);
            }
        }

        public virtual bool Remove(IWidget widget) {
            if (Data.Contains(widget))
                return Data.Remove(widget);
            else
                return false;
        }
        
        public virtual bool Contains(IWidget widget) {
            return Data.Contains(widget);
        }

        public virtual void Clear() {
            Data.Clear ();
            Shape = null;
        }

        public virtual IEnumerable<IWidget> Elements {
            get { 
                foreach (IWidget widget in Data) {
                    yield return widget;
                }
            }
        }
        
        public virtual int Count {
            get { return Data.Count; }
        }
        #endregion

        public override string ToString() {
            return base.ToString()+"("+Data.ToString()+")";
        }
    }
}
