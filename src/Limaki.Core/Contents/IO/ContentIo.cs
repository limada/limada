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
using Limaki.Model.Content;

namespace Limaki.Contents.IO {

    public interface IContentIo {
        IoMode IoMode { get; }
        ContentDetector Detector { get; }
    }

    /// <summary>
    /// a pipe that returns the ContentInfo of source
    /// uses IoMode
    /// uses a ContentDetector
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface IContentIo<TSource> : IPipe<TSource, ContentInfo>, IContentIo, IProgress {
        bool Supports(TSource source);
    }


    /// <summary>
    /// a pipe that returns the ContentInfo of source
    /// uses IoMode
    /// uses a ContentDetector
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public abstract class ContentIo<TSource> : IContentIo<TSource> {
        protected ContentIo (ContentDetector detector) {
            this.Detector = detector;
        }

        public virtual IoMode IoMode { get; protected set; }
        public virtual ContentDetector Detector { get; protected set; }
        public Action<string, int, int> Progress { get; set; }

        public abstract bool Supports(TSource source);

        public abstract ContentInfo Use(TSource source);

        public abstract ContentInfo Use(TSource source, ContentInfo sink);
    }
}