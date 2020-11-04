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
using Limaki.Common;
using Limaki.Data;
using Limaki.Contents.IO;

namespace Limada.IO {
    public class MemoryThingGraphIo : ThingGraphIo {

        public MemoryThingGraphIo (): base(new MemoryThingGraphSpot()) {
            this.IoMode = IoMode.None;
        }

        protected override ThingGraphContent OpenInternal (Iori source) {
            return new ThingGraphContent { Data = new ThingGraph(), Source = source, ContentType = MemoryThingGraphSpot.ContentType };
        }

        public override void Close (ThingGraphContent sink) { }

        public override void Flush (ThingGraphContent sink) { }
    }
}