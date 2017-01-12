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

using System.Collections.Generic;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Xwt;

namespace Limaki.View.Visuals {

    public class VisualComposite<T>:Visual<T>, IComposite<IVisual> 
        where T:ICollection<IVisual> {

        public VisualComposite(T data):base(data) {}

        #region IVisual Member

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

        public override Size Size {
            get {
                if (base.Shape != null)
                    return base.Shape.Size;
                return new Size();
            }
            set { base.Shape.Size = value; }
        }

        public override Point Location {
            get {
                if (base.Shape != null) {
                    return base.Shape.Location;
                } 
                return new Point();
            }
            set {
                var oldLocation = base.Location;
                base.Location = value;
                var dx = oldLocation.X - value.X;
                var dy = oldLocation.Y - value.Y;
                foreach(var visual in Data) {
                    visual.Location.Offset(dx,dy);
                }
            
            }
        }
        #endregion

        #region IComposite member

        public virtual void Add(IVisual visual) {
            if (visual!=null && !Data.Contains(visual)) {
                Data.Add (visual);
            }
        }

        public virtual bool Remove(IVisual visual) {
            if (Data.Contains(visual))
                return Data.Remove(visual);
            return false;
        }
        
        public virtual bool Contains(IVisual visual) {
            return Data.Contains(visual);
        }

        public virtual void Clear() {
            Data.Clear ();
            Shape = null;
        }

        public virtual IEnumerable<IVisual> Elements {
            get { 
                foreach (var visual in Data) {
                    yield return visual;
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
