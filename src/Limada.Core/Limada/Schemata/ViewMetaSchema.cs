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

namespace Limada.Schemata {

    /// <summary>
    /// GuiMetaSchema is a schema for describing the
    /// behavior of a schema in GUIs
    /// </summary>
    public class ViewMetaSchema : Schema {
        /// <summary>
        /// if metaLink.root == marker:
        /// hide all links where thing.rootLinks.marker==marker 
        /// if metaLink.leaf == marker:
        /// hide all links where thing.leafLinks.marker==marker 
        /// </summary>
        public static readonly IThing Hide = Thing<string>($"{MetaSchema.OpenMetaChar}hide all{MetaSchema.CloseMetaChar}", 0x0A154BF91CE930E9);

        /// <summary>
        /// if metaLink.root == marker:
        /// show only links where Item.rootLinks.marker==marker 
        /// if metaLink.leaf == marker:
        /// show only links where Item.leafLinks.marker==marker 
        /// </summary>
        public static readonly IThing ShowOnly = Thing<string>($"{MetaSchema.OpenMetaChar}show only{MetaSchema.CloseMetaChar}", 0xF777C0C38553C101);


    }


}
