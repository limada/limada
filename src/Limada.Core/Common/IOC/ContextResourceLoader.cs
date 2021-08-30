/*
 * Limaki 
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


namespace Limaki.Common.IOC {
    /// <summary>
    /// instruments an IApplicationContext
    /// used to load the resources (common objects and factories) in an application
    /// this class has to be implemented for each type of application
    /// </summary>
    public abstract class ContextResourceLoader : IContextResourceLoader {
        /// <summary>
        /// instruments the context
        /// </summary>
        /// <param name="context"></param>
        public abstract void ApplyResources(IApplicationContext context);

        public virtual IApplicationContext CreateContext() {
            var result = new ApplicationContext();
            return result;
        }
    }
}