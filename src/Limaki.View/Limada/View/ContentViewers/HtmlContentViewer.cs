/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limada.Model;
using Limaki.View.ContentViewers;
using Limaki.WebServers;

namespace Limada.View.ContentViewers {

    public class HtmlContentViewer : HtmlContentViewerBase {

        public virtual IThingGraph ThingGraph { get; set; }
        public virtual IThing ContentThing { get; set; }

        public override IWebResponse CreateResponse () {
            return new ThingWebResponse {
                IsStreamOwner = this.IsStreamOwner,
                Thing = this.ContentThing,
                ThingGraph = this.ThingGraph,
                UseProxy = this.UseProxy,
                BaseUri = this.WebServer.Uri,
            };
        }
    }
}