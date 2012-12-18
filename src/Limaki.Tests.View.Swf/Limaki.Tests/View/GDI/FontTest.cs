using System.Drawing;
using Limaki.Drawing.Gdi;
using NUnit.Framework;
using System.IO;

namespace Limaki.Tests.View.GDI {
    public class FontTest : DomainTest {
        
        public void FontMeasures(string fontName) {
            var writer = new StringWriter();
            writer.WriteLine("public class FontMeasure{0}:FontMeasure {{", fontName);
            writer.WriteLine("\tpublic override void Make() {");
            for (var i = 0x21; i < 0x17e; i++) {
                var c = char.ConvertFromUtf32(i);
                var font = new Font(fontName, 10);
                var size = GdiUtils.GetTextDimension(font, c, new SizeF());
                ReportDetail("{0}\t{1}", c, size.Width);
                writer.WriteLine("Add({0},{1},{2}); // {3}", i, size.Width,size.Height,c);
                
            }
            ReportDetail(writer.ToString());
        }
        [Test]
        public void TestFont () {
            FontMeasures("Arial");
        }


    }
}