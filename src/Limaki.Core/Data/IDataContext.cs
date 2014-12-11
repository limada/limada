/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010 - 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Limaki.Data {

    public interface IDataContext : IDisposable {

        IGateway Gateway { get; set; }

        TextWriter Log { get; set; }

        IQueryable<T> GetQuery<T> () where T : class;

        void SetQuery<T> (IQueryable<T> querable) where T : class;

        void Upsert<T> (IEnumerable<T> entities) where T : class;

        void Remove<T> (IEnumerable<T> entities) where T : class;

    }
}