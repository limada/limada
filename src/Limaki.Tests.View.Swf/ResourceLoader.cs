using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Common;
using Limada.Model;
using Limaki.Common.IOC;
using Limaki.Usecases.Concept;
using Limaki.Tests.UseCases;
using Limaki.Usecases;

namespace Limada.Tests {
    public class ResourceLoader : IContextRecourceLoader {
        public void ApplyResources(IApplicationContext context) {
            var factories = context.Pool.TryGetCreate<UsecaseFactories<ConceptUsecase>>();
            factories.Add(new TestCaseFactory008());
        }
    }
}
