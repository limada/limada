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

namespace Limaki.Contents {

    public interface IIdContent<TId> {
        TId Id { get; }
        object Data { get; set; }
    }

    public interface IIdContent<TId, TData> : IIdContent<TId> {
        new TData Data { get; set; }
    }
}