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
using Xwt;

namespace Limaki.View {

    public interface IToolkitAware {
        //ToolkitType ToolkitType0 { get; }
        Guid ToolkitType { get; }
    }
}