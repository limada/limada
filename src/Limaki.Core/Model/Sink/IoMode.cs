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

namespace Limaki.Model.Content.IO {

    [Flags]
    public enum IoMode {

        None = 0,
        Read = 1 << 0,
        Write = 1 << 1,

        ReadWrite = Read | Write

    }
}