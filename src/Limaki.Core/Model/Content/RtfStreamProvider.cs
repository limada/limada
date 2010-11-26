/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.IO;
using System.Collections.Generic;
using Limaki.Common.Collections;
using Limaki.Common;
using System.Text;
using Limaki.Model.Streams;

namespace Limaki.Model.Content {

    public class RtfStreamProvider : StreamProvider {
        StreamTypeInfo[] _supportedStreamTypes =
            new StreamTypeInfo[]{
				new StreamTypeInfo(
			        "Rich Text Format",
                    StreamTypes.RTF,
                    "rtf",
                    "application/rtf",
                    CompressionType.bZip2,
				 	new Magic[]{
						new Magic(Encoding.ASCII.GetBytes(@"{\rtf"),0)
					}
			)
			
		};

        public override IEnumerable<StreamTypeInfo> SupportedStreamTypes {
            get { return _supportedStreamTypes; }
        }

        public override bool Saveable { get { return true; } }
    }
}
