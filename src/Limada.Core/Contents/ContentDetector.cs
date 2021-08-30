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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Limaki.Contents;
using Limaki.Common;

namespace Limaki.Contents {

    /// <summary>
    /// gives back a ContentInfo describing the stream
    /// if the stream is supported by ContentSpecs
    /// wording: detector, spotter, finder
    /// </summary>
    public class ContentDetector : ContentSpec, IPipe<Stream, ContentInfo>  {

        protected ContentDetector (IEnumerable<ContentInfo> specs) : base(specs) { }

        #region ContentInfo handling

        public virtual ContentInfo Use (Stream stream) {
            if (!SupportsMagics)
                return null;
            return ContentSpecs					
				.FirstOrDefault(info => info.HasMagics && info.Magics.Any(magic => HasMagic(stream, magic.Bytes, magic.Offset)));
        }

        public virtual bool Supports (Stream stream) {
            return Use(stream) != null;
        }

        protected virtual bool HasMagic (Stream stream, byte[] magic, int offset) {
            if (stream.Length <= magic.Length)
                return false;
            var buffer = new byte[magic.Length];
            var pos = stream.Position;
            stream.Position = offset;
            stream.Read(buffer, 0, buffer.Length);

            var result = ByteUtils.BuffersAreEqual (magic, buffer);
            stream.Position = pos;
            return result;
        }

        #endregion

        public virtual ContentInfo Use (Stream source, ContentInfo sink) {
            return PipeExtensions.Use(source, sink, s => Use(s));
        }
    }
}