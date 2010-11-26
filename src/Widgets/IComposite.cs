/*
 * Limaki 
 * Version 0.07
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

using System.Collections.Generic;
using Limaki.Widgets;

namespace Limaki.Common {
    public interface IComposite<T> {
        void Add(T item);
        bool Remove ( T item );
        bool Contains(T item);
        void Clear();

        IEnumerable<T> Elements { get; }
        int Count { get; }
    }
}