using Limaki.Common;
using Limaki.Common.IOC;

namespace Limaki.Usecases {

    public class UsercaseAppFactory<T, U>:AppFactory<T>
        where T : ContextResourceLoader
        where U : new () {

        public UsercaseAppFactory () { }
        public UsercaseAppFactory (IBackendContextResourceLoader backendContextResourceLoader)
            : base (backendContextResourceLoader) { }

        public void CallPlugins (UsecaseFactory<U> factory, U useCase) {
            var factories = Registry.Pooled<UsecaseFactories<U>> ();
            foreach (var item in factories) {
                item.Composer = factory.Composer;
                item.BackendComposer = factory.BackendComposer;
                item.Compose (useCase);
            }
        }
        }
}