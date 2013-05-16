/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Common;

namespace Limaki.Model.Content.IO {
    public static class SinkExtensions {
        public static TSink Sink<TSource, TSink> (TSource source, TSink sink, Func<TSource, TSink> sinkOf) {
            return new Copier<TSink>(CopierOptions.ValueTypes).Copy(sinkOf(source), sink);
        }
    }
}