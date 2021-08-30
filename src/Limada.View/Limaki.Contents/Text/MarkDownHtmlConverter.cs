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

    public class MarkDownHtmlConverter : HtmlConverterBase {

        public override IEnumerable<Tuple<long, long>> SupportedTypes { get { yield return Tuple.Create (ContentTypes.Markdown, ContentTypes.HTML); } }

        public override long ConversionType (long contentType) {
            if (contentType == ContentTypes.Markdown)
                return contentType;
            return -1;
        }

        public override string ToHtml (Stream source) {
            string html = null;
            source.Position = 0;
            using (var reader = new StreamReader (source, Encoding.ASCII, false, (int)source.Length, true)) {
                html = CommonMark.CommonMarkConverter.Convert (reader.ReadToEnd ());
                source.Position = 0;
            }
            return html;
        }

    }

}
