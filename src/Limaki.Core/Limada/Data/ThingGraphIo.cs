/*
 * Limada 
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

using System.Linq;
using Limaki.Model.Content;
using Limaki.Model.Content.IO;
using System;
using System.IO;
using Limaki.Data;

namespace Limada.Data {

    public abstract class ThingGraphIo : ContentIo<Iori>, ISink<Iori,ThingGraphContent>, ISink<ThingGraphContent, Iori> {

        protected ThingGraphIo(ContentDetector detector) : base(detector) {}

        public override bool Supports (Iori source) {
            return Detector.Supports(source.Extension);
        }

        public override ContentInfo Use (Iori source) {
            if (Supports(source))
                return Detector.ContentSpecs.First();
            return null;
        }

        public override ContentInfo Use (Iori source, ContentInfo sink) {
            if (Supports(source))
                return SinkExtensions.Use(source, sink, s => Use(s));
            return null;
        }

        protected abstract ThingGraphContent OpenInternal(Iori source);
        public abstract void Flush (ThingGraphContent sink);
        public abstract void Close(ThingGraphContent sink);

        ThingGraphContent ISink<Iori,ThingGraphContent>.Use (Iori source) {
            return Open(source);
        }

        public virtual ThingGraphContent Open (Iori source) {
            var result = OpenInternal(source);
            if (source == null)
                result.Source = source;
            return result;
        }

        public virtual ThingGraphContent Use (Iori source, ThingGraphContent sink) {
            var result = Open(source);
            if (sink.Data == null)
                return result;
            else {
                throw new NotImplementedException("Merging not implemented");
            }
        }

        public virtual Iori Use (ThingGraphContent source) {
            Close(source);
            return source.Source as Iori;
        }

        public virtual Iori Use (ThingGraphContent source, Iori sink) {
            source.Source = sink;
            Close(source);
            return sink;

        }
    }
}