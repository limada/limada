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
using System.Linq;
using Limaki.Common;
using Limaki.Common.Collections;

namespace Limaki.Contents.IO {

    /// <summary>
    /// a pool of ContentIo's
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TSink"></typeparam>
    public class ContentIoPool<TSource,TSink> : IEnumerable<IContentIo<TSource>> {

        protected ICollection<IContentIo<TSource>> _contentIos = new List<IContentIo<TSource>>();
        
        ContentInfos _contentInfoPool = null;
        /// <summary>
        /// global ContentInfos stored in Registry.Pool
        /// all specs of added ContentIos are added to the ContentInfoPool
        /// </summary>
        protected ContentInfos ContentInfoPool { get { return _contentInfoPool ?? (_contentInfoPool = Registry.Pooled<ContentInfos>()); } }
        
        public virtual void Add(IContentIo<TSource> contentIo) {
            _contentIos.Add(contentIo);
            if (contentIo.Detector != null)
                ContentInfoPool.AddRange (contentIo.Detector.ContentSpecs);
        }

        public virtual void Remove(IContentIo<TSource> contentIo) {
            _contentIos.Remove(contentIo);
        }

        protected virtual IContentIo<TSource> FirstOrDefault (IEnumerable<IContentIo<TSource>> ios) {
            return ios.FirstOrDefault ();
        }

        public virtual IContentIo<TSource> Find (string extension, IoMode mode) {
            return FirstOrDefault (_contentIos.Where (sinkIo => sinkIo.Detector.Supports (extension) && sinkIo.IoMode.HasFlag (mode)));
        }

        public virtual IContentIo<TSource> Find (long streamType, IoMode mode) {
            return FirstOrDefault (_contentIos.Where (sinkIo => sinkIo.Detector.Supports (streamType) && sinkIo.IoMode.HasFlag (mode)));
        }

        public virtual IContentIo<TSource> Find (TSource stream, IoMode mode) {
            return FirstOrDefault (_contentIos.Where (sinkIo => sinkIo.Supports (stream) && sinkIo.IoMode.HasFlag (mode)));
        }

        public virtual IContentIo<TSource> Find (long streamType) {
            return FirstOrDefault (_contentIos.Where (sinkIo => sinkIo.Detector.Supports (streamType)));
        }

        public IEnumerator<IContentIo<TSource>> GetEnumerator() {
            return _contentIos.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public int Priority (long contentType) {
            var result = 0;
            foreach (var contentIo in _contentIos) {
                foreach (var info in contentIo.Detector.ContentSpecs) {
                    if (info.ContentType == contentType)
                        return result;
                    result++;
                }
            }
            return -1;
        }

        public int Priority (IContentIo value) {
            var result = 0;
            foreach (var contentIo in _contentIos) {
                if (contentIo == value)
                    return result;
                result++;
            }

            return -1;
        }
    }

}