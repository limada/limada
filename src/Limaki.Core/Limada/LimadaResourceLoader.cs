/*
 * Limada
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
using Limada.Model;
using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Data;
using Limaki.Contents.IO;
using Limaki.Graphs;

namespace Limada.Usecases {

    public class LimadaResourceLoader : ContextResourceLoader {

        protected IContextResourceLoader ResourceLoader { get; set; }

        public LimadaResourceLoader () { }

        public LimadaResourceLoader(IContextResourceLoader resourceLoader) {
            this.ResourceLoader = resourceLoader;
        }

		protected static bool Applied { get; set; } 

        public override void ApplyResources(IApplicationContext context) {

			if (Applied)
				return;

            if (ResourceLoader != null)
                ResourceLoader.ApplyResources (context);
            
            context.Factory.Add<IThingFactory, ThingFactory> ();
            context.Factory.Add<IGraphModelFactory<IThing, ILink>, ThingFactory> ();

            var thingGraphContentPool = context.Pooled<ThingGraphIoPool>();
            thingGraphContentPool.Add(new XmlThingGraphIo());
            thingGraphContentPool.Add(new IoriThingGraphIo());

			Applied = true;

        }
    }
}