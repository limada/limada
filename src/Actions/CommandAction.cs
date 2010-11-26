/*
 * Limaki 
 * Version 0.063
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
using System.Collections.Generic;
using System.Text;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.Drawing;
using Limaki.Widgets;

namespace Limaki.Actions {
    public abstract class CommandAction<TData,TItem>:ActionBase, ICommandAction<TData,TItem> {
        private Handler<TData> _dataHandler = null;
        public Handler<TData> DataHandler {
            get { return _dataHandler; }
            set { _dataHandler = value; }
        }        
        
        public virtual TData Data {
            get { return DataHandler(); }
        }

        ///<directed>True</directed>
        public ILayout<TData, TItem> Layout {
            get { return _layout; }
            set { _layout = value; }
        }

        protected IControl control = null;
        protected ITransformer transformer = null;
        protected IScrollTarget scrollTarget = null;
		///<directed>True</directed>
		private ILayout<TData, TItem> _layout = null;

        public CommandAction( Handler<TData> dataHandler, 
                       IControl control, 
                       IScrollTarget scrollTarget,
                       ITransformer transformer,
                       ILayout<TData, TItem> layout) {
            this.DataHandler = dataHandler;
            this.control = control;
            this.scrollTarget = scrollTarget;
            this.transformer = transformer;
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
