using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Common;
using Limada.Model;
using Limaki.Common.IOC;
using Limaki.Data;
using Limada.IO;
using Limaki.Contents.IO;

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
