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
 * 
 */

using System;
using Limaki.Common;
using Id = System.Int64;
using Limaki.Model.Streams;
using System.Runtime.Serialization;

namespace Limada.Model {
    [DataContract]
    public class RealData: IRealData<Id> {
        
		protected Id _id = default(Id);
        [DataMember]
        public virtual Int64 Id {
            get { return _id; }
#if ! SILVERLIGHT
            private 
#endif
            set { _id = value; }
        }

        object IRealData<Id>.Data {
            get { throw new Exception("The method or operation is not implemented.");}
            set {throw new Exception("The method or operation is not implemented.");}
        }
    }


	// the use of the base class is necessary in db4o.DataContainer for indexing
	// see there: realDataType
    [DataContract]
    public class RealData<T> : RealData, IRealData<Id,T> {
        protected RealData() { }
        
        public RealData(Id id) { this._id = id; }

        public RealData(Id id, T data)
            : this(id) {
            this.Data = data;
        }

        //protected Id _id = default(Id);
        //public virtual Int64 Id {
        //    get { return _id; }
        //}
		
        [Transient] 
        private bool _isDirty = false;
        public bool IsDirty {
            get { return _isDirty; }
            set { _isDirty = value; }
        }

        protected T _data = default(T);
        [DataMember]
        public virtual T Data {
            get { return _data; }
            set {
                if (value!=null)
                    if (!value.Equals(_data)) {
                        _data = value;
                        _isDirty = true;
                }
            }
        }

        object IRealData<Id>.Data {
            get { return this.Data; }
            set {
                if (value is T) {
                    this.Data = (T)(object)value;
                }
            }
        }



        public override string ToString() {
            return Data.ToString();
        }


    }
}