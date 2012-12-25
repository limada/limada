using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.IOC;
using Limaki.UnitTest;
using Limaki.View;
using NUnit.Framework;
using Xwt.Blind.Backend;
using Xwt.Engine;


namespace Limaki.Tests {
    [TestFixture]
    public class DomainTest:TestBase {
        [TestFixtureSetUp]
        public override void Setup() {
            if (Registry.ConcreteContext == null) {
                new BlindEngine ().InitializeRegistry (WidgetRegistry.MainRegistry);
                BlindEngine.Registry.RegisterBackend(
                    typeof (Xwt.Drawing.SystemColors), typeof (SystemColorsBackend)
                );

                var loader = new ViewContextRecourceLoader();
                Registry.ConcreteContext = new ApplicationContext();
                loader.ApplyResources(Registry.ConcreteContext);

                new LimakiCoreContextRecourceLoader().ApplyResources(Registry.ConcreteContext);
                //var factory = new AppFactory<global::Limada.UseCases.AppResourceLoader>(loader as IBackendContextRecourceLoader);
                
            }
            base.Setup();
        }
    }
}