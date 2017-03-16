using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.UnitTest;
using Limaki.View;
using Limaki.View.Viz.Visualizers;
using NUnit.Framework;
using Limaki.View.Html5;
using System.IO;
using Limaki.Drawing;
using Limaki.View.Headless;
using Xwt;
using Limada.Usecases;


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

            new Html5ContextResourceLoader ().RegisterXwtBackends (true);
            
            if (!Registry.Factory.Contains<IContextWriter>())
                Registry.Factory.Add<IContextWriter, Html5ContextWriter>();
            
            base.Setup();
        }

        public void RegisterHeadless (IApplicationContext context) {
            new LimakiCoreContextResourceLoader ().ApplyResources (context);
            new LimadaResourceLoader ().ApplyResources (context);
            var headless = new HeadlessContextResourceLoader ();
            headless.ApplyHeadlessResources (context);
            new ViewContextResourceLoader ().ApplyResources (context);
            headless.RegisterBackends (context);
            headless.RegisterDragDropFormats (context);
        }

        public void RegisterHtml5 (IApplicationContext context) { 
            var html5 = new Html5ContextResourceLoader ();
            html5.ApplyHtml5Resources (context);
            SystemFonts.Clear ();
            new ViewContextResourceLoader ().ApplyResources (context);
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