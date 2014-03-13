using Limada.Usecases;
using Limaki.Common.IOC;
using Limaki.Tests.UseCases;
using Limaki.Usecases;

namespace Limada.Tests {

    public class TestViewSwfResourceLoader : IContextResourceLoader {

        public void ApplyResources(IApplicationContext context) {
            var factories = context.Pool.TryGetCreate<UsecaseFactories<ConceptUsecase>>();
            factories.Add(new SwfTestCaseFactory());
        }
    }
}
