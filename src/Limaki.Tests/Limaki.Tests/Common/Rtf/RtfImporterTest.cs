using Limaki.Common;
using Limaki.Contents.Text;
using Limaki.UnitTest;
using NUnit.Framework;

namespace Limaki.Tests.Common.Rtf {
    [TestFixture]
    public class RtfImporterTest : DomainTest {
        [Test]
        public void TestParse () {
            var doc = new HtmlDocument ();
            var importer = new RtfImporter (ByteUtils.AsAsciiStream (RtfTestData.WordGameUmlaut), doc);
            importer.Import ();
            ReportDetail (doc.Body);
        }
    }
}