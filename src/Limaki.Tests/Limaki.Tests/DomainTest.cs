using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.IOC;
using Limaki.UnitTest;
using Limaki.View;
using NUnit.Framework;
using Limaki.View.Html5;
using Limaki.View.Visualizers;
using System.IO;
using Limaki.View.HeadlessBackends;
using Limaki.Drawing;
using Limaki.View.UI;
using Limaki.View.Headless;


namespace Limaki.Tests {
    [TestFixture]
    public class DomainTest:TestBase {
        [TestFixtureSetUp]
        public override void Setup() {
            if (Registry.ConcreteContext == null) {
                Registry.ConcreteContext = new ApplicationContext();
                var context = Registry.ConcreteContext;
               
                RegisterHeadless (context);
                
            }
            if (!Registry.Factory.Contains<Limaki.View.Visualizers.IContextWriter>())
                Registry.Factory.Add<Limaki.View.Visualizers.IContextWriter, Html5ContextWriter>();
            
            base.Setup();
        }

        public void RegisterHeadless (IApplicationContext context) {
            new LimakiCoreContextRecourceLoader ().ApplyResources (context);
            var headless = new HeadlessContextRecourceLoader ();
            headless.ApplyHeadlessResources (context);
            //new Html5ContextRecourceLoader ().ApplyHtml5Resources (context);
            new ViewContextRecourceLoader ().ApplyResources (context);
            headless.RegisterBackends (context);
            headless.RegisterDragDropFormats (context);

        }

        IContextWriter _reportPainter = null;
        public virtual IContextWriter ReportPainter {
            get {
                return _reportPainter ?? (_reportPainter = Registry.Factory.Create<IContextWriter>());
            }
        }

        public virtual void WritePainter (string fileName) {
            using (var file = File.Create(fileName)) {
                ReportPainter.Write(file);
            }
        }
        public virtual void WritePainter () {
            WritePainter(this.GetType().Name + ".html");
        }
    }
}