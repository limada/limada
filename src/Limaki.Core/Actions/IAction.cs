/*
 * Limaki 
 * Version 0.08
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
using System.ComponentModel;

namespace Limaki.Actions {
    public interface IAction : IDisposable {
        [Browsable ( false )]
        bool Resolved { get; }

        [Browsable(false)]
        bool Exclusive { get; }

        bool Enabled { get; set; }

        int Priority { get;set;}
    }
}