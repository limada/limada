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
using System.Text;
using Limaki.Common;

namespace Limaki.Contents.Text {

    public class TextHtmlConverter : HtmlConverterBase {

        public override IEnumerable<Tuple<long, long>> SupportedTypes { get { yield return Tuple.Create (ContentTypes.Text, ContentTypes.HTML); } }

        public override long ConversionType (long contentType) {
            if (contentType == ContentTypes.Text)
                return contentType;
            return -1;
        }

        public override string ToHtml (Stream source) {
            using (var reader = new StreamReader (source, Encoding.Unicode, false, (int)source.Length, true)) {
                var pos = source.Position;
                var s = reader.ReadToEnd ();
                s = System.Net.WebUtility.HtmlEncode (s);
                source.Position = pos;
                return s;
            }
        }
    }
}