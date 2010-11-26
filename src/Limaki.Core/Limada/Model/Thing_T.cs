/*
 * Limada 
 * Version 0.08
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
using System.Text;
using Limaki.Graphs;
using Id = System.Int64;

namespace Limada.Model {
    public class Thing<T> : Thing, IThing<T> {

        public Thing(T data) : this(Common.Isaac.Long, data) { }

        public Thing(Id id, T data): base(id) {
            this.Data = data;
        }

        protected T _data = default(T);
        public virtual T Data {
            get { return _data; }
            set { _data = value; }
        }

        object IThing.Data {
            get { return this.Data; }
            set {
                if (value is T)
                    Data = (T)value;
            }
        }

        public override string ToString() {
            return Data.ToString();
        }


    }
}
