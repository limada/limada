using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Context;
using Limaki.UnitTest;
using NUnit.Framework;

namespace Limaki.Tests {
    [TestFixture]
    public class DomainTest:TestBase {
        public override void Setup() {
            if (Registry.ConcreteContext == null) {
                var loader = new Limaki.Presenter.Winform.WinformContextRecourceLoader ();
                Registry.ConcreteContext = new ApplicationContext();
                loader.ApplyResources(Registry.ConcreteContext);
            }
            base.Setup();
        }
    }
}