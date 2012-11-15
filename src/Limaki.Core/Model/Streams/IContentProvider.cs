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
using Limaki.Common;
using System.Text;


namespace Limaki.Model.Streams {
    public interface IContentProvider {

        bool Supports(string extension);
        bool Supports(long streamType);
        bool Supports (Stream stream);

        IEnumerable<StreamTypeInfo> SupportedStreamTypes { get; }

        StreamTypeInfo Info (string extension);
        StreamTypeInfo Info (long streamType);
        StreamTypeInfo Info (Stream stream);

        Content<Stream> Open(Stream stream);
        Content<Stream> Open(Uri uri);
        
        bool Readable { get; }
        bool Saveable { get; }

        void Save(Content<Stream> data, Uri uri);

    }

   
}
