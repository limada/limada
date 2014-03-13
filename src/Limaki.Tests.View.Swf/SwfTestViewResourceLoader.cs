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

using Limada.Usecases;
using Limaki.Common.IOC;
using Limaki.Tests.UseCases;
using Limaki.Usecases;

namespace Limada.Tests {

    public class SwfTestViewResourceLoader : IContextResourceLoader {

        public void ApplyResources(IApplicationContext context) {
            var factories = context.Pool.TryGetCreate<UsecaseFactories<ConceptUsecase>>();
            factories.Add(new SwfTestCaseFactory());
        }
    }
}
