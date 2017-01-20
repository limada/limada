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

namespace Limaki.Contents {

    public interface IContentSpec {
        IEnumerable<ContentInfo> ContentSpecs { get; }
        ContentInfo Find (string extension);
        ContentInfo FindMime (string mime);
        ContentInfo Find (long contentType);
        bool Supports (string extension);
        bool Supports (long contentType);
    }

    public class ContentSpec : IContentSpec {

        public ContentSpec (IEnumerable<ContentInfo> specs) {
            this.ContentSpecs = specs;
        }

        public virtual IEnumerable<ContentInfo> ContentSpecs { get; protected set; }

        protected bool? _streamHasMagics = null;
        public virtual bool SupportsMagics {
            get {
                if (!_streamHasMagics.HasValue) {
                    _streamHasMagics = ContentSpecs.Any (info => info.Magics != null && info.Magics.Count () > 0);
                }
                return _streamHasMagics.Value;
            }
        }

        public virtual ContentInfo Find (string extension) {
            return ContentSpecs.Find (extension);
        }

        public virtual ContentInfo FindMime (string mime) {
            return ContentSpecs.FindMime (mime);
        }

        public virtual ContentInfo Find (long contentType) {
            return ContentSpecs.Find (contentType);
        }

        public virtual bool Supports (string extension) {
            return ContentSpecs.Supports (extension);
        }

        public virtual bool Supports (long contentType) {
            return ContentSpecs.Supports (contentType);
        }
    }

    public static class ContenSpecExtensions {
        public static ContentInfo Find (this IEnumerable<ContentInfo> ContentSpecs, string extension) {
            extension = extension.ToLower ().TrimStart ('.');
            return ContentSpecs.Where (type => type.Extension == extension).FirstOrDefault ();
        }

        public static ContentInfo FindMime (this IEnumerable<ContentInfo> ContentSpecs, string mime) {
            mime = mime.ToLower ();
            return ContentSpecs.Where (type => type.MimeType == mime).FirstOrDefault ();
        }

        public static ContentInfo Find (this IEnumerable<ContentInfo> ContentSpecs, long contentType) {
            return ContentSpecs.Where (type => type.ContentType == contentType).FirstOrDefault ();
        }

        public static bool Supports (this IEnumerable<ContentInfo> ContentSpecs, string extension) {
            return Find (ContentSpecs, extension) != null;
        }

        public static bool Supports (this IEnumerable<ContentInfo> ContentSpecs, long contentType) {
            return Find (ContentSpecs, contentType) != null;
        }
    }
}