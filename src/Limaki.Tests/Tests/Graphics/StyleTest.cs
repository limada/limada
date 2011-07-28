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
            Assert.IsFalse(object.ReferenceEquals(style1.ParentStyle, style2.ParentStyle));
            Assert.AreEqual(style1.Font, style2.Font);
            Assert.AreEqual(style1.Pen, style2.Pen);
        }
        [Test]
        public void TestGroupCascading() {
            var styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
            var stylegroup = (IStyleGroup)styleSheets.DefaultStyleSheet.ItemStyle.Clone();
            var newGroup = (styleSheets.DefaultStyleSheet as StyleSheet).CreateStyleGroup("test", null, true);
            // something is wrong with WhiteGlass; does not show CustonEndCaps with hover, seletcted!
            //TODO: test this!

        }
    }
}