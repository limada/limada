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
 * 
 */

using System;
using System.ComponentModel;

namespace Limaki.Actions {
    public abstract class ActionBase : IAction {
        protected bool _resolved = false;

        [Browsable(false)]
        public virtual bool Resolved {
            get { return _resolved; }
            protected set { _resolved = value; }
        }

        [Browsable(false)]
        public virtual bool Exclusive {
            get { return Resolved; }
            protected set { Resolved = value; }
        }

        private bool _enabled = true;

        public virtual bool Enabled {
            get { return _enabled; }
            set { _enabled = value; }
        }

        private int _priority = 0;
        public int Priority {
            get { return _priority; }
            set { _priority = value; }
        }


        public virtual void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose stub
        /// override this if you have something to dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose ( bool disposing ) {}
    }
}