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
using Limaki.View;
using System.Linq;

namespace Limaki.Usecases {

    public abstract class UsecaseAppFactory<T, U> : AppFactory<T>
        where T : ContextResourceLoader
        where U : new () {

        public UsecaseAppFactory () { }

        public UsecaseAppFactory (IBackendContextResourceLoader backendContextResourceLoader)
            : base (backendContextResourceLoader) {}

        protected override void Create (IBackendContextResourceLoader backendContextResourceLoader) {

            var tka = backendContextResourceLoader as IToolkitAware;
            if (tka != null)
                this.ToolkitType = tka.ToolkitType;

            base.Create (backendContextResourceLoader);
        }

        public virtual Xwt.ToolkitType XwtToolkitType { get; protected set; }

        public virtual Guid ToolkitType { get; protected set; }

        public abstract void Run ();

        public void CallPlugins (UsecaseFactory<U> factory, U useCase) {

            var factories = Registry.Pooled<UsecaseFactories<U>> ();
            foreach (var item in factories) {
                item.Composer = factory.Composer;
                item.BackendComposer = factory.BackendComposer;
                item.Compose (useCase);
            }
        }

        public virtual bool TakeToolkit (IToolkitAware loader) {
            return loader.ToolkitType == this.ToolkitType;
        }

        public override bool TakeType (Type type) {

            if (type.GetInterfaces ().Any (t => t == typeof (IToolkitAware))) {
                if (type.GetConstructors ().Any (tc => tc.GetParameters ().Length == 0)) {
                    var loader = Activator.CreateInstance (type) as IToolkitAware;
                    return TakeToolkit (loader);
                }
            }
            return base.TakeType (type);
        }
        
    }
}