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
using System.Runtime.Serialization;
using Limaki.Common;
using Id = System.Int64;

namespace Limada.Model {
    /// <summary>
    /// a Thing without data
    /// it can be used as a blank node
    /// to structure content
    /// Thing supports Descriptions
    /// </summary>
    [DataContract]
    public class Thing : IThing, IThing<Id> {
        
        public Thing() : this(Common.Isaac.Long) {}
        public Thing(Id id) {
            _id = id;
        }

        protected Id _id = default(Id);

        [Transient]
        private State _state = null;
        public State State {
            get {
                if (_state == null) {
                    _state = new State ();
                    _state.SetDirty = () => {
                        _state.Dirty = true;
                        if (!_state.Creating) {
                            this._changeDate = DateTime.Now;
                        }
                    };
                }
                return _state;
            }
        }

        [DataMember]
        public virtual Id Id {
            get { return _id; }
#if ! SILVERLIGHT
            private 
#endif
            set { _id = value; }
        }

        public virtual void SetId(Id id) {
            _id = id;
        }

        object IThing.Data {
            get { return null; }
            set { }
        }

        DateTime _changeDate = DateTime.MinValue;
        [DataMember]
        public virtual DateTime ChangeDate {
            get {  return _changeDate; }
#if ! SILVERLIGHT
            private
#endif
            set { _changeDate = value; }
        }

        DateTime _creationDate = DateTime.MinValue;
        [DataMember]
        public virtual DateTime CreationDate {
            get { return _creationDate; }
#if ! SILVERLIGHT
            private
#endif
            set { _creationDate = value; }
        }

        public virtual void SetCreationDate(DateTime date) {
            _creationDate = date;
            _changeDate = date;
        }
        public virtual void SetChangeDate(DateTime date) {
            _changeDate = date;
        }
        public virtual void SetId(Id id, DateTime creationDate, DateTime writeDate) {
            _id = id;
            _changeDate = writeDate;
            _creationDate = creationDate;
        }

        public virtual void MakeEqual(IThing thing) {}

        public override string ToString() {
            return "°";
        }

        #region IThing<long> Member

        long IThing<Id>.Data {
            get { return Id; }
            set { }
        }
        #endregion
    }
}