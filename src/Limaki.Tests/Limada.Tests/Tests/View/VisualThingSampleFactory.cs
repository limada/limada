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


using Limada.Model;
using Limada.View;
using Limada.VisualThings;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;

namespace Limada.Tests.View {

    public class VisualThingSampleFactory<T> : SampleGraphPairFactory<IVisual, IThing, IVisualEdge, ILink>
        where T : ISampleGraphFactory<IThing, ILink>, new() {
        public VisualThingSampleFactory() : 
            base(new T(), 
                 new VisualThingTransformer()) { }
        }
}
