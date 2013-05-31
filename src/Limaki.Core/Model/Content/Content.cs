/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using Id = System.Int64;

namespace Limaki.Model.Content {

    public class Content {
        public Content() {
            Compression = CompressionType.None;
            ContentType = ContentTypes.Unknown;
        }

        public CompressionType Compression { get; set; }
        public Id ContentType { get; set; }
        public object Description { get; set; }
        public object Source { get; set; }
    }

    public class Content<T>:Content {
        public T Data;

        public Content() {}
        public Content(T data, CompressionType compression) {
            this.Data = data;
            this.Compression = compression;
        }

        public Content(T data, CompressionType compression, long streamType):this(data,compression) {
            this.ContentType = streamType;
        }

        public Content(Content source) {
            this.Description = source.Description;
            this.Source = source.Source;
            this.Compression = source.Compression;
            this.ContentType = source.ContentType;
            var sourceT = source as Content<T>;
            if (sourceT != null)
                this.Data = sourceT.Data;
        }
    }
}