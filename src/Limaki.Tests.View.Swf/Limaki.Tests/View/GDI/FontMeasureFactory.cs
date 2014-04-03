using System.Drawing;
using Limaki.GdiBackend;
using NUnit.Framework;
using System.IO;
using Xwt.GdiBackend;

namespace Limaki.Tests.View.GDI {

    public class FontMeasureFactory : DomainTest {
        
        public void FontMeasures(string fontName) {
            var writer = new StringWriter();
            writer.WriteLine("public class FontMeasure{0}:FontMeasure {{", fontName);
            writer.WriteLine("\tpublic override void Make() {");
            var font = new Font(fontName, 10);

            CharacterRange[] characterRanges = { new CharacterRange(0, 0) };
            var stringFormat = GdiConverter.GetDefaultStringFormat().Clone() as StringFormat;
            stringFormat.SetMeasurableCharacterRanges(characterRanges);
            for (var i = 0x21; i < 0x17e; i++) {
                var c = char.ConvertFromUtf32(i);

                if (false) {
                    // something wrong here; gives always 0
                    var mcrSize = GdiUtils.DeviceContext.MeasureCharacterRanges(c, font, new Rectangle(0, 0, 1000, 1000), stringFormat);
                    ReportDetail("{0}\t{1}", c, mcrSize[0].GetBounds(GdiUtils.DeviceContext).Size.ToXwt());
                }
                var size = GdiUtils.GetTextDimension(font, c, new SizeF());
                ReportDetail("{0}\t{1}",c , size.Width);
                writer.WriteLine("Add({0},{1},{2}); // {3}", i, size.Width,size.Height,c);
                
            }
            ReportDetail(writer.ToString());
        }

        [Test]
        public void TestFont () {
            FontMeasures("Times New Roman");
        }


    }
}