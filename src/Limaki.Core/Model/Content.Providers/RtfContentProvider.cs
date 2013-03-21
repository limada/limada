/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.IO;
using System.Collections.Generic;
using Limaki.Common.Collections;
using Limaki.Common;
using System.Text;
using Limaki.Model.Content;

namespace Limaki.Model.Content.Providers {

    public class RtfContentProvider : ContentProvider {
        ContentInfo[] _supportedContents =
            new ContentInfo[]{
				new ContentInfo(
			        "Rich Text Format",
                    ContentTypes.RTF,
                    "rtf",
                    "application/rtf",
                    CompressionType.bZip2,
				 	new Magic[]{
						new Magic(Encoding.ASCII.GetBytes(@"{\rtf"),0)
					}
			)
			
		};

        public override IEnumerable<ContentInfo> SupportedContents {
            get { return _supportedContents; }
        }

        public override bool Saveable { get { return true; } }
        public override bool Readable { get { return true; } }
    }
}
