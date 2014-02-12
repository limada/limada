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

    public class ContentIoPool<TSource,TSink> : IEnumerable<IContentIo<TSource>> {

        private ICollection<IContentIo<TSource>> _contentIos = new Set<IContentIo<TSource>>();
        
        ContentInfos _contentInfoPool = null;
        /// <summary>
        /// global ContentInfos stored in Registry.Pool
        /// all specs of added ContentIos are added to the ContentInfoPool
        /// </summary>
        protected ContentInfos ContentInfoPool { get { return _contentInfoPool ?? (_contentInfoPool = Registry.Pool.TryGetCreate<ContentInfos>()); } }
        
        public virtual void Add(IContentIo<TSource> contentIo) {
            _contentIos.Add(contentIo);
            ContentInfoPool.AddRange(contentIo.Detector.ContentSpecs);
        }

        public virtual void Remove(IContentIo<TSource> contentIo) {
            _contentIos.Remove(contentIo);
        }

        public virtual IContentIo<TSource> Find (string extension, IoMode mode) {
            return _contentIos.Where(sinkIo => sinkIo.Detector.Supports(extension)).FirstOrDefault(i => i.IoMode.HasFlag(mode));
        }

        public virtual IContentIo<TSource> Find (long streamType, IoMode mode) {
            return _contentIos.Where(sinkIo => sinkIo.Detector.Supports(streamType)).FirstOrDefault(i => i.IoMode.HasFlag(mode));
        }

        public virtual IContentIo<TSource> Find (TSource stream, IoMode mode) {
            return _contentIos.Where(sinkIo => sinkIo.Supports(stream)).FirstOrDefault(i => i.IoMode.HasFlag(mode));
        }

        public virtual IContentIo<TSource> Find(long streamType) {
            return _contentIos.Where(sinkIo => sinkIo.Detector.Supports(streamType)).FirstOrDefault();
        }


        public IEnumerator<IContentIo<TSource>> GetEnumerator() {
            return _contentIos.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

       
    }

}