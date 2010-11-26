/*
 * Limada
 * Version 0.081
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
using Id = System.Int64;

namespace Limada.Schemata {
    /// <summary>
    /// TopicSchema defines a default root which should opend automatically
    /// when a database is opened
    /// whatever you want to see by opening a database should bee added to 
    /// graph.Add ( TopicSchema.Topics, thing, TopicSchema.TopicMarker);
    /// </summary>
    public class TopicSchema : Schema {
        /// <summary>
        /// root-Item for Topics
        /// </summary>
        public static readonly IThing Topics = Thing<string>("Topics", 0x2DFCD782BE4C69DD);

        /// <summary>
        /// marker-Item for theme
        /// </summary>
        public static readonly IThing TopicMarker = Thing<string>("Topic", 0xF426954398F4D754);

        public static readonly IThing AutoViewMarker = Thing<string>("AutoView", 0xCF5E308A1719F4FD);

        //TODO: Add whatever themesViewOnOpenConnectionLink may be
        /// <summary>
        /// ??? maybe the link which is shown automatically if an database is
        /// opened ??
        /// </summary>
        //public static readonly IThing TopicsViewOnOpenConnectionLink = Link(null, null, null, 0x7DF5714511B69DBD);

    }
}