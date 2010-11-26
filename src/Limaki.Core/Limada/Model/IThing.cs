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
using Id = System.Int64;

namespace Limada.Model {
    public interface IThing {
        object Data { get; set; }
        Id Id { get; }
        void SetId ( Id id );
    }

    public interface IThing<T>:IThing {
        new T Data { get; set; }
    }
}
