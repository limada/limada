/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2015 Lytico
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

    public class MarkDownConverter : ContentConverter<Stream>, IContentConverter<Stream> {

        public override IEnumerable<Tuple<long, long>> SupportedTypes { get { yield return Tuple.Create (ContentTypes.Markdown, ContentTypes.HTML); } }

        public string MarkDownToHtml (Stream source) {
            string html = null;
            source.Position = 0;
            using (var reader = new StreamReader (source, Encoding.ASCII, false, (int)source.Length, true)) {
                html = CommonMark.CommonMarkConverter.Convert (reader.ReadToEnd ());
                source.Position = 0;
            }
            return html;
        }

        public string WithHmtlHeaderTags (Stream s) => $"{HtmlHelper.HtmUtf8Begin}<body>{MarkDownToHtml (s)}</body></html>";

        public override Content<Stream> Use (Content<Stream> source, Content<Stream> sink) {
            if (ProveTypes (source, ContentTypes.Markdown, sink, ContentTypes.HTML)) {
                var s = WithHmtlHeaderTags (source.Data);
                return new Content<Stream> (s.AsAsciiStream (), CompressionType.None, ContentTypes.HTML);
            }
            return null;
        }
    }

}
