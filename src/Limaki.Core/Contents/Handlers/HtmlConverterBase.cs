/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2017 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Limaki.Common;
using Limaki.Common.Text.HTML;

namespace Limaki.Contents.Text {
    
    public interface IHtmlConverter : IContentConverter<Stream> {
        string ToHtml (Stream source);
        string WithHmtlHeaderTags (Stream s);
    }


    public abstract class HtmlConverterBase : ContentConverter<Stream>, IHtmlConverter {
        
        public abstract string ToHtml (Stream source);

        public abstract long ConversionType (long contentType);

        public virtual string WithHmtlHeaderTags (Stream s) => $"{HtmlHelper.HtmUtf8Begin}<body>{ToHtml (s)}</body></html>";

        public override Content<Stream> Use (Content<Stream> source, Content<Stream> sink) {
            if (ProveTypes (source, ConversionType(source.ContentType), sink, ContentTypes.HTML)) {
                var s = WithHmtlHeaderTags (source.Data);
                return new Content<Stream> (s.AsAsciiStream (), CompressionType.None, ContentTypes.HTML);
            }
            return null;
        }
    }

}
