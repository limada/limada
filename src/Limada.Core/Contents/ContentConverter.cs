/*
 * Limada 
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Limaki.Contents;

namespace Limaki.Contents {

    public interface IContentConverter<T> : IPipe<Content<T>, Content<T>> {
        /// <summary>
        /// a list of ContentTypes, where Item1 is source, Item2 is sink-Type
        /// </summary>
        IEnumerable<Tuple<long, long>> SupportedTypes { get; }

        Content<T> Use (Content<T> source, long sinkType);
    }

    public class ConverterPool<T>:List<IContentConverter<T>> {
        public IContentConverter<T> Find(long source, long sink) {
            return this.Where (c => c.SupportedTypes.Any (t => t.Item1 == source && t.Item2 == sink))
                .FirstOrDefault();
        }
    }

    public abstract class ContentConverter<T> : IContentConverter<T> {
        public virtual Content<T> Use (Content<T> source) {
            var conv = SupportedTypes.Where(t => t.Item1 == source.ContentType);
            if (conv.Count() > 1 || conv.Count() == 0)
                throw new ArgumentException("No unique conversion found");
            return Use(source, conv.First().Item2);
        }

        public virtual Content<T> Use (Content<T> source, long sinkType) {
            return Use(source, new Content<T> { ContentType = sinkType });
        }

        protected bool ProveTypes (Content<Stream> source, long sourceType, Content<Stream> sink, long sinkType) {
            if (source.ContentType != sourceType || sink.ContentType != sinkType)
                throw new ArgumentException ("Conversion not supported");
            return true;
        }

        public abstract IEnumerable<Tuple<long, long>> SupportedTypes { get; }

        public abstract Content<T> Use (Content<T> source, Content<T> sink);
    }
}