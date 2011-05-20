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
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.IO;
using System.Collections.Generic;
using Limaki.Common;
using System.Text;


namespace Limaki.Model.Streams {
    public interface IStreamProvider {
        bool Supports(string extension);
        bool Supports(long streamType);

        IEnumerable<StreamTypeInfo> SupportedStreamTypes { get; }

        StreamInfo<Stream> Open(Stream stream);
        StreamInfo<Stream> Open(Uri uri);

        bool Saveable { get; }

        void Save(StreamInfo<Stream> data, Uri uri);

    }
}
