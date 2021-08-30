/*
 * Limada 
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
using Limada.Schemata;
using Limaki.Common;
using Limaki.Graphs;

namespace Limada.Schemata {

    public class SchemaFacade {
        
        public static void InitSchemata() {
            Schema schema = new CommonSchema();
            schema = new MetaSchema();
            schema = new TopicSchema();
            schema = new ViewMetaSchema();
            schema = new DigidocSchema();
        }

        public static void MakeMarkersUnique(IThingGraph thingGraph) {
            InitSchemata();
            foreach (var marker in Schemata.Schema.IdentityGraph) {
                thingGraph.UniqueThing(marker);
            }

        }

        public static bool DescriptionableThing(IThing thing) {
            if (thing == null)
                return false;
            
            return thing.GetType ().Equals (typeof (Thing))|| (thing is IStreamThing)||thing.Data==null;
        }

        private static GraphDepencencies<IThing, ILink> _dependencies = null;
        public static GraphDepencencies<IThing, ILink> Dependencies {
            get { return _dependencies ?? (_dependencies = Registry.Pooled<GraphDepencencies<IThing, ILink>>()); }
        }
    }
}