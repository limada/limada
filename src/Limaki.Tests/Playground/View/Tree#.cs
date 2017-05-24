/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2017 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Collections.Generic;
using System;

namespace Limaki.Common.Collections {

    /// <summary>
    /// a tree based on one? linkedlist
    /// </summary>
    public class Tree<T> {

        public LinkedList<T> List { get; } = new LinkedList<T> ();
    }
}

