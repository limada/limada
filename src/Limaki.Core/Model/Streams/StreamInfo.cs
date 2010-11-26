/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using Limaki.Model.Streams;
using Id = System.Int64;
using System;

namespace Limaki.Model.Streams {
    public class StreamInfo {
        public CompressionType Compression = CompressionType.None;
        public long StreamType = StreamTypes.Unknown;
        public object Description=null;
        public object Source=null;
    }

    public class StreamInfo<T>:StreamInfo {
        public T Data;

        public StreamInfo() {}
        public StreamInfo(T data, CompressionType compression) {
            this.Data = data;
            this.Compression = compression;
        }

        public StreamInfo(T data, CompressionType compression, long streamType):this(data,compression) {
            this.StreamType = streamType;
        }

        public StreamInfo(StreamInfo source) {
            this.Description = source.Description;
            this.Source = source.Source;
            this.Compression = source.Compression;
            this.StreamType = source.StreamType;
        }
    }
}