using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.IOC;
using Limaki.UnitTest;
using NUnit.Framework;

namespace Limaki.Tests {
    [TestFixture]
    public class DomainTest:TestBase {
        [TestFixtureSetUp]
        public override void Setup() {
            if (Registry.ConcreteContext == null) {
                var loader = new Limaki.View.Winform.WinformContextRecourceLoader ();
                Registry.ConcreteContext = new ApplicationContext();
                loader.ApplyResources(Registry.ConcreteContext);
                var factory = new AppFactory<global::Limada.UseCases.AppResourceLoader>(loader);
                
            }
            base.Setup();
        }
    }
}