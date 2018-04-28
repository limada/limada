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
using System.ComponentModel;

namespace Limaki.Actions {

    public abstract class ActionBase : IAction {
        
        [Browsable (false)]
        public virtual bool Resolved { get; protected set; } = false;

        [Browsable(false)]
        public virtual bool Exclusive {
            get { return Resolved; }
            protected set { Resolved = value; }
        }

        public virtual bool Enabled { get; set; } = true;

        public int Priority { get; set; } = 0;


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