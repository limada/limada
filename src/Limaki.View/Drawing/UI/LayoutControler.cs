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
 */

using System;
using Limaki.Actions;

namespace Limaki.Drawing.UI {
    /// <summary>
    /// Decouples Data (Model) from Control (View)
    /// with usage of a Layout
    /// is responsible for invalidating the Control and 
    /// its handling of the extend of Data (srolling etc)
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public abstract class LayoutControler<TData,TItem>:ActionBase, ILayoutControler<TData,TItem> {
        private Func<TData> _dataHandler = null;
        public Func<TData> DataHandler {
            get { return _dataHandler; }
            set { _dataHandler = value; }
        }        
        
        public virtual TData Data {
            get { return DataHandler(); }
        }

        private ILayout<TData, TItem> _layout = null;
        ///<directed>True</directed>
        public ILayout<TData, TItem> Layout {
            get { return _layout; }
            set { _layout = value; }
        }

        ILayout ILayoutControler.Layout {
            get { return this.Layout; }
            set { if (value is ILayout<TData, TItem>)
                this.Layout = (ILayout<TData, TItem>)value;
            }
        }

        protected IControl control = null;
        protected ICamera camera = null;
        protected IScrollTarget scrollTarget = null;
        ///<directed>True</directed>
		

        public LayoutControler( Func<TData> dataHandler, 
                                IControl control, 
                                IScrollTarget scrollTarget,
                                ICamera camera,
                                ILayout<TData, TItem> layout) {
            this.DataHandler = dataHandler;
            this.control = control;
            this.scrollTarget = scrollTarget;
            this.camera = camera;
            this._layout = layout;
        }

        public abstract void Invoke();

        public abstract void Execute(ICommand<TItem> command);
        /// <summary>
        /// justifies all pending items
        /// calls control.invalidate()
        /// </summary>
        public abstract void Execute();

        public abstract void Done();
    }
}