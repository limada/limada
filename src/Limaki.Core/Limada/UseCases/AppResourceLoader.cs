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


using Limada.Data;
using Limada.Model;
using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Data;
using Limaki.Model.Content.IO;

namespace Limada.Usecases {

    public class AppResourceLoader : ContextRecourceLoader {
        private IContextRecourceLoader deviceContext = null;

        public AppResourceLoader(IContextRecourceLoader deviceContext) {
            this.deviceContext = deviceContext;
        }

        public override void ApplyResources(IApplicationContext context) {
            
            deviceContext.ApplyResources(context);

            var providers = context.Pool.TryGetCreate<DataProviders<IThingGraph>>();
            providers.Add(typeof(XMLThingGraphProvider));

            var thingGraphProvider = context.Pool.TryGetCreate<IoProvider<IThingGraph, IoInfo>>();
            //thingGraphProvider.Add(new XmlThingGraphIo());

        }
    }
}