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
using Limaki.Common.Text.RTF;

namespace Limaki.Contents.Text {

    public interface IAdobeRtfFilterConverter { 
        bool UseAdobeFilter { get; set; }
    }

    public class RtfHtmlConverter : HtmlConverterBase, IAdobeRtfFilterConverter {

        public override IEnumerable<Tuple<long, long>> SupportedTypes { get { yield return Tuple.Create (ContentTypes.RTF, ContentTypes.HTML); } }

        public override long ConversionType (long contentType) {
            if (contentType == ContentTypes.RTF)
                return contentType;
            return -1;
        }

        public bool UseAdobeFilter { get; set; } = false;

        public override string ToHtml (Stream source) {
            var stream = source;
            var doc = new HtmlDocument ();
            if (UseAdobeFilter) {
                var filter = new AdobeRTFFilter ();
                if (filter.IsAdobeRTF (source)) {
                    stream = filter.RemoveAdobeParagraphTags (source);
                }
            }
            var importer = new RtfImporter (stream, doc);
            importer.Import ();
            if (stream != source)
                stream.Dispose ();
            return doc.Body;
        }


    }

}
