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

        IEnumerable<StreamTypeInfo> SupportedStreamTypes { get; }

        Content<Stream> Open(Stream stream);
        Content<Stream> Open(Uri uri);
        
        bool Readable { get; }
        bool Saveable { get; }

        void Save(Content<Stream> data, Uri uri);

    }

   
}
