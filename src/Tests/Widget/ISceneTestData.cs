/*
 * Limaki 
 * Version 0.063
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


using Limaki.Widgets;

namespace Limaki.Tests.Widget {
    public interface ISceneTestData {
        Scene Scene { get; }
        int Count { get; set; }
        string Name { get; }
        void Populate ( Scene scene );
    }
}
