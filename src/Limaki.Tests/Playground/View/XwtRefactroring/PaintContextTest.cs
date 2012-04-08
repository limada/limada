using NUnit.Framework;


using Xwt.Backends;
using Xwt.Engine;
using Xwt.Drawing;
using System;
using Limaki.Painting;


namespace Limaki.Tests.Sandbox {
    public class PaintContextTest : DomainTest {
        [Test]
        public void TestPureXwt() {
            WidgetRegistry.RegisterBackend(typeof(PaintContext), typeof(Limaki.GDI.Painting.PaintContextBackendHandler));
            WidgetRegistry.RegisterBackend(typeof(PaintCanvas), typeof(Limaki.Swf.Painting.PaintCanvasBackend));

            var control = new PaintCanvas();

        }
    }
}