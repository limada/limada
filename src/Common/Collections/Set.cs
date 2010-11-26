/*
 * Limaki 
 * Version 0.064
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
using System.Collections.Generic;
using System.Text;

namespace Limaki.Common.Collections {
    /// <summary>
    /// Set is a simple override of HashSet to resolve naming conflicts
    /// among .NET 2.0 and .Net 3.5.
    /// On .NET 2.0 a slightly changed version of monos implementation is used 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Set<T> : HashSet<T>, ICollection<T> { }
}
