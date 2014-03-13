using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limada.Usecases;
using Limaki.Common;
using Limada.Model;
using Limaki.Common.IOC;
using Limaki.Tests.UseCases;
using Limaki.Usecases;

namespace Limada.Tests {
    public class ResourceLoader : IContextResourceLoader {
        public void ApplyResources(IApplicationContext context) {
            var factories = context.Pool.TryGetCreate<UsecaseFactories<ConceptUsecase>>();
            factories.Add(new TestCaseFactory008());
        }
    }
}
