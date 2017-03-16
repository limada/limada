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

using System.Collections.Generic;

namespace Limaki.Common.Collections {

    public class Set<T> : HashSet<T>, ICollection<T> {
        public Set() : base() {}

        public Set(IEnumerable<T> source):base() {
            AddRange (source);
        }

        public virtual void AddRange(IEnumerable<T> source) {
            if (source != null)
                foreach (var item in source)
                    if (!Contains(item))
                        Add(item);
        }
    }
}
