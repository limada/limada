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
 * http://limada.sourceforge.net
 * 
 */

using System;

namespace Limaki.Model.Streams {
    public interface IRealData<TKey> {
        TKey Id { get; }
        object Data { get; set; }
    }

    public interface IRealData<TKey,TData>: IRealData<TKey> {
        new TData Data { get; set; }
    }
}