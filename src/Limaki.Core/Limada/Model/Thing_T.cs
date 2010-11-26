/*
 * Limada 
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
using System.Text;
using Limaki.Graphs;
using Id = System.Int64;
using System.Runtime.Serialization;

namespace Limada.Model {
    [DataContract]
    public class Thing<T> : Thing, IThing<T> {
        protected Thing(){}
        public Thing(T data) : this(Common.Isaac.Long, data) { }

        public Thing(Id id, T data): base(id) {
            this.Data = data;
        }

        protected T _data = default(T);


        [DataMember]
        public virtual T Data {
            get { return _data; }
            set { this.State.Setter(ref _data, value); }
        }

        
        object IThing.Data {
            get { return this.Data; }
            set {
                if (value is T)
                    Data = (T)value;
            }
        }

        public override void MakeEqual(IThing thing) {
            base.MakeEqual(thing);
            var data = thing.Data;
            if (data != null && typeof(T).Equals(data.GetType())) {
                this.Data = (T)thing.Data;
            }
        }

        public override string ToString() {
            return Data.ToString();
        }


    }
}
