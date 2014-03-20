/*
 * Limada
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
using Limaki.Contents;
using Id = System.Int64;

namespace Limaki.Contents {

    public interface IContentContainer<TId> {

        bool Contains (TId id);
        bool Contains (IIdContent<TId> item);

        void Add (IIdContent<TId> item);
        IIdContent<TId> GetById (TId id);

        bool Remove (TId id);
        bool Remove (IIdContent<TId> item);

    }
}