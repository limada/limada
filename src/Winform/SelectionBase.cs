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

using System;
using System.Windows.Forms;
using System.Drawing;
using Limaki.Actions;
using Limaki.Drawing;

namespace Limaki.Winform {
    /// <summary>
    /// base class for selection with drag behaviour
    /// </summary>
    public abstract class SelectionBase : MouseDragActionBase, IPaintAction {
        ///<directed>True</directed>
        protected IWinControl control = null;
        ///<directed>True</directed>
        protected ITransformer transformer = null;
        public SelectionBase():base() {
            this.Priority = ActionPriorities.SelectionPriority;   
        }
        public SelectionBase(IWinControl control, ITransformer transformer):this() {
            this.control = control;
            this.transformer = transformer;
        }

        private Point _mouseDownPos = Point.Empty;
        protected virtual Point MouseDownPos {
            get { return _mouseDownPos; }
            set { _mouseDownPos = value; }
        }
        
        public override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            MouseDownPos = e.Location;
        }

        public abstract void OnPaint ( PaintActionEventArgs e );
        public abstract void Clear();


    }
}