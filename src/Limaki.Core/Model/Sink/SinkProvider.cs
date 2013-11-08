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

namespace Limaki.Model.Content.IO {

    public interface ISinkProvider<TSource, TSink> : IEnumerable<ISink<TSource,TSink>> {}

    public class SinkProvider<TSource, TSink> : ISinkProvider<TSource, TSink> {
        protected ICollection<ISink<TSource, TSink>> _sinks = new Set<ISink<TSource, TSink>>();
        public virtual void Add (ISink<TSource, TSink> sinkIo) {
            _sinks.Add(sinkIo);
        }

        public virtual void Remove (ISink<TSource, TSink> sinkIo) {
            _sinks.Remove(sinkIo);
        }

        public virtual IEnumerator<ISink<TSource, TSink>> GetEnumerator () {
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