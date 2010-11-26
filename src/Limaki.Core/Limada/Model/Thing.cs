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

namespace Limada.Model {
    /// <summary>
    /// a Thing without data
    /// it can be used as a blank node
    /// to structure content
    /// Thing supports Descriptions
    /// </summary>
    public class Thing : IThing, IThing<Int64> {
        public Thing() : this(Common.Isaac.Long) { }
        public Thing(Int64 id) { _id = id; }

        protected Int64 _id = default(Int64);
        public virtual Int64 Id {
            get { return _id; }
        }

        public virtual void SetId(Int64 id) {
            _id = id;
        }

        object IThing.Data {
            get { return null; }
            set { }
        }

        public override string ToString() {
            return "�";
        }

        #region IThing<long> Member

        long IThing<Int64>.Data {
            get { return Id; }
            set { }
        }
        #endregion
    }
}