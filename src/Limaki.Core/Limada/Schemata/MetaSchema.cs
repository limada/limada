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
 * http://limada.sourceforge.net
 * 
 */

using Limada.Model;
using Limada.Schemata;

namespace Limada.Schemata {
    /// <summary>
    /// MetaSchema is a schema for describing Marker-Schemas
    /// usage: new MetaSchema (graph, someMarkerHere);
    /// </summary>
    public class MetaSchema : Schema {
        /// <summary>
        /// show the Leaf of that someLink of aThing as a Description, 
        /// where someLink.Marker has a schemaLink with Marker == MetaSchema.DescriptionMarker
        /// where this schemaLink.Leaf is the Marker of someLink
        /// </summary>
        public static readonly IThing DescriptionMarker = Thing<string>("««description»»", 0x3B0549DCA766E138);

        public virtual IThing GetDescription(IThingGraph graph, IThing marker) {
            return GetTheLeaf(graph, marker, DescriptionMarker);
        }

        public virtual ILink SetDescription(IThingGraph graph, IThing marker, IThing value) {
            return SetTheLeaf(graph, marker, DescriptionMarker, value);
        }

        /// <summary>
        /// Be carefull! This description is for Markers!
        /// the link of this method is: 
        /// schemaLink.Root=this.Target where Target is a Marker
        /// schemaLink.Leaf:some other Marker 
        /// schemaLink.Marker=MetaSchema.DescriptionMarker
        /// </summary>
        public virtual IThing Description {
            get { return GetTheLeaf(DescriptionMarker); }
            set { SetTheLeaf(DescriptionMarker, value); }
        }

        /// <summary>
        /// Used for auto-generating Lattices out of Schemata
        /// Root means: link a new Thing() of a newThing as a root
        /// if a schemaLink.Marker has a link with a Marker == MetaSchema.Root
        /// usage in a Schema:
        /// linkOfMarker.Root = this.Target which is a Marker of a Schema
        /// linkOfMarker.Leaf = MetaSchema.Root ????
        /// linkOfMarker.Marker = someMarker ????
        /// usage in LatticeBuilder:
        /// newThing.Add( new Link(new Thing(), newThing, linkOfMarker.Marker)
        /// </summary>
        public static readonly IThing Root = Thing<string>("««as Root»»", 0xF73890EB29566698);

        /// <summary>
        /// link it as a leaf 
        /// link.root == Item
        /// link.leaf = markerItem of Item.field
        /// </summary>
        public static readonly IThing Leaf = Thing<string>("««as Leaf»»", 0x80800E3CB657AFD2);




    }
}