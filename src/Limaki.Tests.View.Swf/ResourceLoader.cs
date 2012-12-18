using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Common;
using Limada.Model;
using Limaki.Common.IOC;
using Limaki.UseCases;
using Limaki.Tests.UseCases;

namespace Limada.Tests {
    public class ResourceLoader : IContextRecourceLoader {
        public void ApplyResources(IApplicationContext context) {
            var factories = context.Pool.TryGetCreate<UseCaseFactories<UseCase>>();
            factories.Add(new TestCaseFactory());
        }
    }
}
