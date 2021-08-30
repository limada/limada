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

using System.Collections.Generic;
using Limaki.Common.Collections;

namespace Limaki.Contents {

    public interface IPipePool<TSource, TSink> : IEnumerable<IPipe<TSource,TSink>> {}

    public class PipePool<TSource, TSink> : IPipePool<TSource, TSink> {

        protected ICollection<IPipe<TSource, TSink>> _sinks = new Set<IPipe<TSource, TSink>>();

        public virtual void Add (IPipe<TSource, TSink> sink) {
            _sinks.Add(sink);
        }

        public virtual void Remove (IPipe<TSource, TSink> sink) {
            _sinks.Remove(sink);
        }

        public virtual IEnumerator<IPipe<TSource, TSink>> GetEnumerator () {
            return _sinks.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
            return this.GetEnumerator();
        }

        public virtual TSink Use(TSource source, TSink sink) {
            var result = sink;
            foreach(var use in this) {
                result = use.Use(source, sink);
            }
            return result;
        }
    }
}