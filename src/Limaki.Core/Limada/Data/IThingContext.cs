/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limada.Model;
using Limaki.Contents;
using Limaki.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Id = System.Int64;

namespace Limada.Data {

    public interface IThingContext:IDisposable {

        IDataContext DataContext { get; }

        IQueryable<IThing> Things { get; }

        IQueryable<IThing<string>> StringThings { get; }
        IQueryable<INumberThing> NumberThings { get; }

        IQueryable<IStreamThing> StreamThings { get; }
        IQueryable<IIdContent<Id, Byte[]>> StreamBytes { get; }

        IQueryable<ILink> Links { get; }
        IQueryable<ILink<Id>> IdLinks { get; }

        void Upsert<T> (IEnumerable<T> things) where T : IThing;

        void UpsertContents (IEnumerable<IIdContent<Id>> contents);

        void Remove (IEnumerable<Id> ids) ;

    }
}