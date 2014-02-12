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

using System.IO;
using Limaki.Model.Content;

namespace Limaki.Contents {

    public interface ICompressionWorker {
        bool Compressable ( CompressionType compression );
        Stream Compress ( Stream stream, CompressionType compression );
        Stream DeCompress ( Stream stream, CompressionType compression );
    }
}