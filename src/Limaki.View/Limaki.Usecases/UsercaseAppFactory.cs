/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.View.XwtBackend;
using System.Linq;

namespace Limaki.Usecases {

    public abstract class UsercaseAppFactory<T, U> : AppFactory<T>
        where T : ContextResourceLoader
        where U : new () {

        public UsercaseAppFactory () { }

        public UsercaseAppFactory (IBackendContextResourceLoader backendContextResourceLoader)
            : base (backendContextResourceLoader) { }

        public virtual Xwt.ToolkitType ToolkitType { get; protected set; }

        public abstract void Run ();

        public void CallPlugins (UsecaseFactory<U> factory, U useCase) {

            var factories = Registry.Pooled<UsecaseFactories<U>> ();
            foreach (var item in factories) {
                item.Composer = factory.Composer;
                item.BackendComposer = factory.BackendComposer;
                item.Compose (useCase);
            }
        }

        public override bool TakeType (Type type) {

            if (type.GetInterfaces ().Any (t => t == typeof (IToolkitAware))) {
                if (type.GetConstructors ().Any (tc => tc.GetParameters ().Length == 0)) {
                    var loader = Activator.CreateInstance (type) as IToolkitAware;
                    return loader.ToolkitType == this.ToolkitType;
                }
            }
            return base.TakeType (type);
        }

        
    }
}