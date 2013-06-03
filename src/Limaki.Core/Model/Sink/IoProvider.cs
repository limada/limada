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

using Limaki.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Limaki.Common.Linqish;
using System.IO;
using Limaki.Common;

namespace Limaki.Model.Content.IO {

    public class IoProvider<TSource,TSink> : IEnumerable<ISinkIo<TSource>> {

        private ICollection<ISinkIo<TSource>> _sinkIos = new Set<ISinkIo<TSource>>();
        
        ContentInfos _contentInfos = null;
        protected ContentInfos ContentInfos { get { return _contentInfos ?? (_contentInfos = Registry.Pool.TryGetCreate<ContentInfos>()); } }
        
        public virtual void Add(ISinkIo<TSource> sinkIo) {
            _sinkIos.Add(sinkIo);
            ContentInfos.AddRange(sinkIo.InfoSink.SupportedContents);
        }

        public virtual void Remove(ISinkIo<TSource> sinkIo) {
            _sinkIos.Remove(sinkIo);
        }

        public virtual ISinkIo<TSource> Find (string extension, IoMode mode) {
            return _sinkIos.Where(provider => provider.InfoSink.Supports(extension)).FirstOrDefault(i => i.IoMode.HasFlag(mode));
        }

        public virtual ISinkIo<TSource> Find (long streamType, IoMode mode) {
            return _sinkIos.Where(provider => provider.InfoSink.Supports(streamType)).FirstOrDefault(i => i.IoMode.HasFlag(mode));
        }

        public virtual ISinkIo<TSource> Find (TSource stream, IoMode mode) {
            return _sinkIos.Where(provider => provider.Supports(stream)).FirstOrDefault(i => i.IoMode.HasFlag(mode));
        }

        public virtual ISinkIo<TSource> Find(long streamType) {
            return _sinkIos.Where(provider => provider.InfoSink.Supports(streamType)).FirstOrDefault();
        }


        public IEnumerator<ISinkIo<TSource>> GetEnumerator() {
            return _sinkIos.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

       
    }

}