using Limaki.Common;
using Limaki.UnitTest;
using NUnit.Framework;
using ApplicationContextRecourceLoader=Limaki.Context.ApplicationContextRecourceLoader;

namespace Limaki.Tests {
    [TestFixture]
    public class DomainTest:TestBase {
        public override void Setup() {
            if (Registry.ConcreteContext == null) {
                var loader = new ApplicationContextRecourceLoader ();
                Registry.ConcreteContext = loader.CreateContext ();
                loader.ApplyResources(Registry.ConcreteContext);

            }
            base.Setup();
        }
    }
}