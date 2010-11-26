/*
 * Limaki 
 * Version 0.064
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

using System.Drawing;
using Limaki.Drawing;

namespace Limaki.Widgets {

    public class Widget<T> : IWidget {
        //public Widget () {}

        public Widget(T data) {
            Data = data;
        }
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
        public override string ToString() {
            return base.ToString()+"("+Data.ToString()+")";
        }
    }
    public class ToolWidget<T>:Widget<T>,IToolWidget {
        public ToolWidget(T data):base(data){}
    }
}
