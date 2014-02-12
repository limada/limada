/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;

namespace Limaki.Contents.IO {

    [Flags]
    public enum IoMode {

        None = 0,
        Read = 1 << 0,
        Write = 1 << 1,
        Client = 1 << 2,
        Server = 1 << 3,

        ReadWrite = Read | Write,
        ClientServer = Client | Server

    }
}