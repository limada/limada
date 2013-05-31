/*
 * Limada 
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
using Limaki.Data;
using Limaki.Model.Content.IO;

namespace Limada.Data {
    public class MemoryThingGraphIo : ThingGraphIo {

        public MemoryThingGraphIo (): base(new MemoryThingGraphInfo()) {
            this.IoMode = InOutMode.None;
        }

        protected override ThingGraphContent OpenInternal (IoInfo source) {
            return new ThingGraphContent { Data = new ThingGraph(), Source = source, ContentType = MemoryThingGraphInfo.MemoryThingGraphContentType };
        }

        public override void Close (ThingGraphContent sink) { }

        public override void Flush (ThingGraphContent sink) { }
    }
}