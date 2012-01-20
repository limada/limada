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

using System;

namespace Limaki.Drawing.Painters {
    public abstract class Painter<T> : IPainter<T>, IPainter<IShape<T>, T> {

        #region IPainter<T> Member

		protected IShape<T> _shape;
        public virtual IShape<T> Shape {
            get { return _shape; }
            set { _shape = value; }
        }

        #endregion

        #region IPainter Member


		///<directed>True</directed>
        protected IStyle _style;
        public virtual IStyle Style {
            get { return _style; }
            set { _style = value; }
        }

        IShape IPainter.Shape {
            get { return Shape; }
            set {
                if (value is IShape<T>)
                    Shape = (IShape<T>)value;
            }
        }

        protected RenderType _renderType = RenderType.DrawAndFill;
        public virtual RenderType RenderType {
            get { return _renderType; }
            set { _renderType = value;  }
        }

        public abstract void Render ( ISurface surface );

        public virtual PointI[] Measure(Matrice matrix, int delta, bool extend) {
            return Shape.Hull (matrix, delta, extend);
        }

        public virtual PointI[] Measure(int delta, bool extend) {
            return Shape.Hull(delta, extend);
        }

        #endregion

        #region IDisposable Member
        public virtual void Dispose(bool disposing) { }
        public virtual void Dispose() {
            Dispose(true);
        }

        #endregion
    }
}