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
          

        }

        public void ReportStyleGroup(IStyleGroup stylegroup) {
            ReportDetail("{0}", stylegroup.Name);
            ReportDetail("\tParent\t{0}", stylegroup.ParentStyle.Name);
            ReportDetail("\tDefault\t{0}\t{1}", stylegroup.DefaultStyle.Name,stylegroup.DefaultStyle.ParentStyle.Name);
            ReportDetail("\tSelected\t{0}t{1}", stylegroup.SelectedStyle.Name, stylegroup.SelectedStyle.ParentStyle.Name);
            ReportDetail("\tHovered\t{0}t{1}", stylegroup.HoveredStyle.Name, stylegroup.HoveredStyle.ParentStyle.Name);
        }

        public void TestPaintData(IStyleGroup stylegroup) {
            ReportStyleGroup(stylegroup);
            var baseData = stylegroup.ParentStyle.PaintData;
            var toggle = stylegroup.PaintData;
            stylegroup.PaintData = !toggle;
            Assert.IsTrue(stylegroup.ParentStyle.PaintData == baseData);
            Assert.IsTrue(stylegroup.PaintData != toggle);
            Assert.IsTrue(stylegroup.DefaultStyle.PaintData != toggle);
            Assert.IsTrue(stylegroup.SelectedStyle.PaintData != toggle);
            Assert.IsTrue(stylegroup.HoveredStyle.PaintData != toggle);
        }

        [Test]
        public void TestCascading() {
            var styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
            
            TestPaintData(styleSheets.DefaultStyleSheet.ItemStyle);
            TestPaintData(styleSheets.DefaultStyleSheet.ItemStyle);
            TestPaintData(styleSheets.DefaultStyleSheet.EdgeStyle);
            TestPaintData(styleSheets.DefaultStyleSheet.EdgeStyle);
        }
    }
}