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
        public CompressionType Compression = CompressionType.None;
        public Id StreamType = ContentTypes.Unknown;
        public object Description=null;
        public object Source=null;
    }

    public class Content<T>:Content {
        public T Data;

        public Content() {}
        public Content(T data, CompressionType compression) {
            this.Data = data;
            this.Compression = compression;
        }

        public Content(T data, CompressionType compression, long streamType):this(data,compression) {
            this.StreamType = streamType;
        }

        public Content(Content source) {
            this.Description = source.Description;
            this.Source = source.Source;
            this.Compression = source.Compression;
            this.StreamType = source.StreamType;
            var sourceT = source as Content<T>;
            if (sourceT != null)
                this.Data = sourceT.Data;
        }
    }
}