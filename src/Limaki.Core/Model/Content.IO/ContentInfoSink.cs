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
using System.IO;
using System.Linq;
using Limaki.Model.Content;

namespace Limaki.Model.Content.IO {

    public class ContentInfoSink : ISink<Stream, ContentInfo> {

        protected ContentInfoSink (IEnumerable<ContentInfo> supportedContents) {
            this.SupportedContents = supportedContents;
        }

        public virtual IEnumerable<ContentInfo> SupportedContents { get; protected set; }

        #region ContentInfo handling

        protected bool? _supportedHasMagics = null;
        public virtual bool StreamHasMagics {
            get {
                if (!_supportedHasMagics.HasValue) {
                    _supportedHasMagics = SupportedContents.Any(info => info.Magics != null && info.Magics.Count() > 0);
                }
                return _supportedHasMagics.Value;
            }
        }

        public virtual ContentInfo Use (Stream stream) {
            if (!StreamHasMagics)
                return null;
            return SupportedContents
                .Where(info => info.Magics != null)
                .Where(info => info.Magics.Any(magic => HasMagic(stream, magic.Bytes, magic.Offset)))
                .FirstOrDefault();
        }

        public virtual ContentInfo Info (string extension) {
            extension = extension.ToLower().TrimStart('.');
            return SupportedContents.Where(type => type.Extension == extension).FirstOrDefault();
        }

        public virtual ContentInfo Info (long contentType) {
            return SupportedContents.Where(type => type.ContentType == contentType).FirstOrDefault();
        }

        public virtual bool Supports (string extension) {
            return Info(extension) != null;
        }

        public virtual bool Supports (Stream stream) {
            return Use(stream) != null;
        }

        public virtual bool Supports (long contentType) {
            return Info(contentType) != null;
        }

        protected virtual bool BuffersAreEqual (byte[] a, byte[] b) {
            if (a.Length != b.Length)
                return false;
            for (int i = 0; i < a.Length; i++) {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }

        protected virtual bool HasMagic (Stream stream, byte[] magic, int offset) {
            if (stream.Length <= magic.Length)
                return false;
            var buffer = new byte[magic.Length];
            var pos = stream.Position;
            stream.Position = offset;
            stream.Read(buffer, 0, buffer.Length);

            var result = BuffersAreEqual(magic, buffer);
            stream.Position = pos;
            return result;
        }

        #endregion

        public virtual ContentInfo Use (Stream source, ContentInfo sink) {
            return SinkExtensions.Use(source, sink, s => Use(s));
        }
    }
}