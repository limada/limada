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
    }

    public class ContentSpec: IContentSpec {

        public ContentSpec (IEnumerable<ContentInfo> specs) {
            this.ContentSpecs = specs;
        }

        public virtual IEnumerable<ContentInfo> ContentSpecs { get; protected set; }

        protected bool? _streamHasMagics = null;
        public virtual bool SupportsMagics {
            get {
                if (!_streamHasMagics.HasValue) {
                    _streamHasMagics = ContentSpecs.Any(info => info.Magics != null && info.Magics.Count() > 0);
                }
                return _streamHasMagics.Value;
            }
        }

        public virtual ContentInfo Find (string extension) {
            extension = extension.ToLower().TrimStart('.');
            return ContentSpecs.Where(type => type.Extension == extension).FirstOrDefault();
        }

        public virtual ContentInfo Find (long contentType) {
            return ContentSpecs.Where(type => type.ContentType == contentType).FirstOrDefault();
        }

        public virtual bool Supports (string extension) {
            return Find(extension) != null;
        }

        public virtual bool Supports (long contentType) {
            return Find(contentType) != null;
        }
    }
}