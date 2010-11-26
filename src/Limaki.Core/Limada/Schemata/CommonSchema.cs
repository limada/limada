/*
 * Limada
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

using Limada.Model;
using Limada.Schemata;

namespace Limada.Schemata {
    /// <summary>
    /// Handles Description on Things
    /// holds Definitions for EmptyMarker and EntryMarker
    /// Example:
    /// <code>
    /// CommonSchema schema = new CommonSchema(graph,thing);
    /// schena.Description = new Thing#string ("My Description");
    /// </code>
    /// or
    /// <code>
    /// CommonSchema(graph,thing).Description = new Thing#string ("My Description");
    /// </code>
    /// 
    /// as a result a link is added where:
    /// link.Root = thing;
    /// link.Leaf = thing#string:"My Description"
    /// link.Marker = CommonSchema.DescriptionMarker
    
    /// </summary>
    public class CommonSchema : Schema {
        /// <summary>
        /// empty marker if you don't want any marker
        /// </summary>
        public static readonly IThing EmptyMarker = Thing(0x528FE1B697E54910);

        public static string EmtpyMarkerString = "°";
        public static string ThingString = "·";
        public static string NullString =  ((char)(0x2260)).ToString(); // not equal
        public static string ProxyString = ((char)(0x221E)).ToString(); // infinite

        /// <summary>
        /// marker for general use if you don't have a special marker
        /// </summary>
        public static readonly IThing CommonMarker = Thing<string>("-»", 0x05C54DDB81FD4AB3);

        #region Description
        
        /// <summary>
        /// common description marker if there is no MetaSchema-Description
        /// </summary>
        public static readonly IThing DescriptionMarker = Thing<string>("Description", 0x4AD0E2AB77254D9E);

        /// <summary>
        /// source of the item (filename, uri)
        /// </summary>
        public static readonly IThing SourceMarker = Thing<string>("Source", 0x29D72C81FEF5C8A0);
        

        public virtual IThing Description {
            get { return GetTheLeaf(DescriptionMarker); }
            set { SetTheLeaf(DescriptionMarker, value); }
        }
        #endregion

        #region Data-Handling

        public virtual IThing GetDescription(IThingGraph graph, IThing thing) {
            return GetTheLeaf(graph, thing, DescriptionMarker);
        }

        public virtual ILink SetDescription(IThingGraph graph, IThing thing, IThing value) {
            return SetTheLeaf(graph, thing, DescriptionMarker, value);
        }

        public CommonSchema():base(){}
        public CommonSchema(IThingGraph graph,IThing thing):base(graph,thing) { }

        #endregion
    }
}