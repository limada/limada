using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Context;
using Limaki.UnitTest;
using NUnit.Framework;

namespace Limaki.Tests {
    [TestFixture]
    public class DomainTest:TestBase {
        [TestFixtureSetUp]
        public override void Setup() {
            if (Registry.ConcreteContext == null) {
                var loader = new Limaki.Presenter.Winform.WinformContextRecourceLoader ();
                Registry.ConcreteContext = new ApplicationContext();
                loader.ApplyResources(Registry.ConcreteContext);
                var factory = new AppFactory<global::Limada.UseCases.AppResourceLoader>(loader);
                
            }
            base.Setup();
        }
    }
}