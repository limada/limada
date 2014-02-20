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


using System;
using Limaki.Graphs;
using Limaki.Tests.Visuals;
using Limaki.Visuals;
using Limada.Model;
using System.Collections;
using System.Collections.Generic;
using Limaki.Common.Collections;
using Limada.Schemata;

namespace Limada.Tests.View {
    public class SchemaViewTestData<T>:ThingSceneFactory0
    where T:Schema, new() {
        public override IThingGraph ThingGraph {
            get {
                
                if (_thingGraph==null) {
                    _thingGraph = new T().SchemaGraph;
                }
                return _thingGraph;
            }
            set {
                _thingGraph = value;
            }
        }
    }
}
