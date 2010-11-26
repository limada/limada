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
using Limaki.Model.Streams;
using Id = System.Int64;

namespace Limaki.Data {
    public interface IDataContainer<TKey> {
        bool Contains ( IRealData<TKey> item );
        void Add(IRealData<TKey> item);
        bool Contains(TKey id);
        IRealData<TKey> GetById(TKey id);
        bool Remove(TKey id);
        bool Remove(IRealData<TKey> item);
    }
}