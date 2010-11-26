/*
 * Limaki 
 * Version 0.07
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
using Limaki.Graphs;
using Limaki.Tests.Graph;
using Limaki.Tests.Graph.Model;

namespace Limaki.Tests.Widget {
    public class BinaryTree : SceneTestData {
        public override GenericGraphFactory<IGraphItem, IGraphEdge> Data {
            get {
                if (_data == null) {
                    _data = new BinaryTreeFactory();
                }
                return _data;
            }
        }

    }
}