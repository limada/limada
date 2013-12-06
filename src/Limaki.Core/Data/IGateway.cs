/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Common.Collections;

namespace Limaki.Data {

    public interface IGateway:IDisposable {
        void Close();
        Iori Iori { get; set; }

        bool IsOpen();
        bool IsClosed();
        void Open( Iori iori );
    }
}
