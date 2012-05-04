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
        /// <summary>
        /// marker for link to topics where the leaf is opened automatically if db opens
        /// </summary>
        public static readonly IThing AutoViewMarker = Thing<string>("AutoView", 0xCF5E308A1719F4FD);

        //TODO: Add whatever themesViewOnOpenConnectionLink may be
        /// <summary>
        /// ??? maybe the link which is shown automatically if an database is
        /// opened ??
        /// </summary>
        //public static readonly IThing TopicsViewOnOpenConnectionLink = Link(null, null, null, 0x7DF5714511B69DBD);

        /// <summary>
        /// all new created sheets are stored here
        /// </summary>
        public static readonly IThing Sheets = Thing<string>("Sheets", 0xF86E40042CAA2C3C);
        /// <summary>
        /// marker for link [sheets->someSheet, SheetMarker]
        /// </summary>
        public static readonly IThing SheetMarker = Thing<string>("Sheet", 0x9F108AEF87B496B2);
        /// <summary>
        /// link: [topics->sheets,topicMarker]
        /// </summary>
        public static readonly IThing TopicToSheetsLink = Link(Topics, Sheets, TopicMarker, 0xF2ADCF2637243C80);


//        					
//	0xC23AC9CD9C574A41	0x9138DBEF86C5A1B0	0x29BAD17B0DFC31FB	0x6E5754FF0BF12EA8	
//0x06DA57008E22674F	0xC4650C95F186E6EA	0x0EA32174FC1C7D95	0xA7755CE164D1FAB9	0xB25D88CEEDCFBB3C	
//0x3F70656F0B76C542	0x4F88368091E18063	0xEB69D1D93F973582	0xDA396B20FC688A50	0x23FA7B65FFAD0A9B
    }
}