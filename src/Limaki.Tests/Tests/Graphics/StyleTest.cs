using NUnit.Framework;
using Limaki.Drawing;
using Limaki.Common;

namespace Limaki.Tests.Graphic {
    public class StyleTest : DomainTest {
        
        [Test]
        public void TestCloning() {
            var styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
            var style1 = styleSheets.DefaultStyleSheet.ItemStyle;
            var style2 = (IStyleGroup)style1.Clone();
            Assert.IsFalse(!object.ReferenceEquals(style1.ParentStyle, style2.ParentStyle));
           
        }

    }
}