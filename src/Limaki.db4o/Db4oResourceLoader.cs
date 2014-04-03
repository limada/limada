/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limada.IO;
using Limaki.Common.IOC;

namespace Limaki.db4o {

    public class Db4oResourceLoader : IContextResourceLoader {

        public void ApplyResources(IApplicationContext context) {

            var thingGraphContentPool = context.Pooled<ThingGraphIoPool>();
            thingGraphContentPool.Add(new Db4oThingGraphIo());

            var repairPool = context.Pooled<ThingGraphRepairPool>();
            repairPool.Add(new Limada.IO.db4o.Db4oRepairer());
            
        }
    }
}
