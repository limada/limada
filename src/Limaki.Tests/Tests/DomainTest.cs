using Limaki.Common;
using Limaki.Context;
using Limaki.UnitTest;
using NUnit.Framework;

namespace Limaki.Tests {
    [TestFixture]
    public class DomainTest:TestBase {
        public override void Setup() {
            if (Registry.ConcreteContext == null) {
                var loader = new WinformContextRecourceLoader ();
                Registry.ConcreteContext = loader.CreateContext ();
                loader.ApplyResources(Registry.ConcreteContext);
            }
            base.Setup();
        }
    }
}