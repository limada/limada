using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Common;
using Limada.Model;
using Limaki.Common.IOC;
using Limaki.Data;
using Limada.Data;
using Limaki.Contents.IO;

namespace Limaki.db4o {

    public class Db4oResourceLoader : IContextRecourceLoader {

        public void ApplyResources(IApplicationContext context) {
            var thingGraphContentPool = context.Pool.TryGetCreate<ThingGraphIoPool>();
            thingGraphContentPool.Add(new Db4oThingGraphIo());

            var repairPool = context.Pool.TryGetCreate<ThingGraphRepairPool>();
            repairPool.Add(new Limada.Data.db4o.Db4oRepairer());
            
        }
    }
}
