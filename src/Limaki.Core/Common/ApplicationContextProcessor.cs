/*
 * Limaki 
 * Version 0.08
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


namespace Limaki.Common {
    
    /// <summary>
    /// instruments an object with properties token from an ApplicationContext
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ApplicationContextProcessor<T> {
        /// <summary>
        /// applies the properties found in the context to target
        /// </summary>
        /// <param name="context"></param>
        /// <param name="target"></param>
        public abstract void ApplyProperties(IApplicationContext context, T target);
    }
}