using System;
using System.Collections.Generic;
using System.Text;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.UnitTest;
using NUnit.Framework;

namespace Limaki.Tests.View.Drawing.Shapes {
    public class ViewSerialisationTest:DomainTest {

        [Test]
        public void FontTest() {
            var ser = new DrawingPrimitivesSerializer ();
            var font = new Font ();
            font.FontFamily = "Tahoma";
            var elem = ser.Write (font);
            this.ReportDetail (elem.ToString());

            var newFont = ser.Read (elem);
            
            Assert.AreEqual (font.FontFamily, newFont.FontFamily);
            Assert.AreEqual(font.Size, newFont.Size);
            Assert.AreEqual(font.Style, newFont.Style);
        }
    }
}
