using Limaki.Drawing.Styles;
using NUnit.Framework;
using Limaki.Drawing;
using Limaki.Common;
using System.Linq.Expressions;
using System;
using Limaki.Common.Linqish;
using System.Reflection;
using Xwt.Drawing;
using System.Linq;

namespace Limaki.Tests.Drawing {

    public class StyleTest : DomainTest {
        
        [Test]
        public void TestCloning() {
            var styleSheets = Registry.Pooled<StyleSheets>();
            var style1 = styleSheets.DefaultStyleSheet.ItemStyle;
            var style2 = (IStyleGroup)style1.Clone();
            Assert.IsFalse(object.ReferenceEquals(style1.ParentStyle, style2.ParentStyle));
            Assert.AreEqual(style1.Font, style2.Font);
            Assert.AreEqual(style1.StrokeColor, style2.StrokeColor);
            Assert.AreEqual (style1.LineWidth, style2.LineWidth);
        }

        [Test]
        public void TestGroupCascading() {
            var styleSheets = Registry.Pooled<StyleSheets>();
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
        public void TestCascadingPaintData() {
            var styleSheets = Registry.Pooled<StyleSheets>();
            var styleSheet = (IStyleSheet) styleSheets.DefaultStyleSheet.Clone ();
            var edgePaint = true;
            styleSheet.EdgeStyle.PaintData = edgePaint;
            styleSheet.EdgeStyle.DefaultStyle.PaintData = edgePaint;
            styleSheet.EdgeStyle.SelectedStyle.PaintData = edgePaint;
            styleSheet.EdgeStyle.HoveredStyle.PaintData = edgePaint;
            TestPaintData (styleSheet.ItemStyle);
            TestPaintData(styleSheet.ItemStyle);
            TestPaintData(styleSheet.EdgeStyle);
            TestPaintData(styleSheet.EdgeStyle);
        }

        public void TestMemberCascade<T> (IStyleGroup stylegroup, Expression<Func<IStyle, T>> member) {
            var memberI = member.MemberInfo () as PropertyInfo;
            object oldValue = null;
            object newValue = null;
            if (memberI.PropertyType == typeof (bool)) {
                var value = (bool)memberI.GetValue (stylegroup, null);
                oldValue = value;
                newValue = !value;
            }

            if (memberI.PropertyType == typeof (Color)) {
                var value = (Color) memberI.GetValue (stylegroup, null);
                oldValue = value;
                newValue = value.WithIncreasedContrast(.05);
            }

            // set everything go parentStyle == stylegroup
            foreach (var style in stylegroup) {
                memberI.SetValue (style, oldValue,null);
            }
            memberI.SetValue (stylegroup, newValue,null);
            // now everything should have stylegroups value:
            foreach (var style in stylegroup) {
                var val =  memberI.GetValue (style,null);
                Assert.AreEqual (newValue, val, memberI.Name);
            }

            // set individual value:
            foreach (var style in stylegroup.Where(s=>s!=stylegroup)) {
                memberI.SetValue (style,oldValue,null);

                var val = memberI.GetValue (style, null);
                Assert.AreEqual (oldValue, val, memberI.Name);
            }
            // styleGroup should have old value:
            var baseVal = memberI.GetValue (stylegroup, null);
            Assert.AreEqual (newValue, baseVal, memberI.Name);
        }

        public void TestMemberCascade<T> (IStyleSheet styleSheet, Expression<Func<IStyle, T>> member) {
            TestMemberCascade (styleSheet.ItemStyle, s => s.PaintData);
            TestMemberCascade (styleSheet.EdgeStyle, s => s.PaintData);
        }

        [Test]
        public void TestCascading () {
            var styleSheets = Registry.Pooled<StyleSheets> ();
            var styleSheet = (IStyleSheet) styleSheets.DefaultStyleSheet.Clone ();
            TestMemberCascade (styleSheet, s => s.PaintData);
            TestMemberCascade (styleSheet, s => s.StrokeColor);
        }
    }
}