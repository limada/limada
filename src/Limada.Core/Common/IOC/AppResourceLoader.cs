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



using System;

namespace Limaki.Common.IOC {

    [Obsolete]
    public class AppResourceLoader : ContextResourceLoader {
        
        private IContextResourceLoader deviceContext = null;

        public AppResourceLoader(IContextResourceLoader deviceContext) {
            this.deviceContext = deviceContext;
        }

        public override void ApplyResources(IApplicationContext context) {
            
            deviceContext.ApplyResources(context);

        }
    }
}