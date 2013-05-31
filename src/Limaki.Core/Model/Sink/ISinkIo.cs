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

using Limaki.Common;
using System;

namespace Limaki.Model.Content.IO {

    public interface ISinkIo<TSource> : ISink<TSource, ContentInfo>, IProgress {

        InOutMode IoMode { get; }
        ContentInfoSink InfoSink { get; }
        bool Supports(TSource source);

        Action<string, int, int> Progress { get; set; }
    }

    public abstract class SinkIo<TSource> : ISinkIo<TSource> {
        protected SinkIo (ContentInfoSink supportedContents) {
            this.InfoSink = supportedContents;
        }

        public virtual InOutMode IoMode { get; protected set; }
        public virtual ContentInfoSink InfoSink { get; protected set; }
        public Action<string, int, int> Progress { get; set; }

        public abstract bool Supports(TSource source);

        public abstract ContentInfo Use(TSource source);

        public abstract ContentInfo Use(TSource source, ContentInfo sink);
    }
}