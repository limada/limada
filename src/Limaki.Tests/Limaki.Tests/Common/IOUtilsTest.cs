using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Mono.Options;

namespace Limaki.Tests.Common {
    public class IOUtilsTest : DomainTest {
        [Test]
        public void NiceFileNameTest() {
            var fileName = "<y>:?";
            var nice = Uri.EscapeUriString(fileName);
            nice = Uri.UnescapeDataString(nice);
            ReportDetail(nice);
            var b = new StringBuilder(fileName);
            foreach (var s in Path.GetInvalidFileNameChars())
                b.Replace(s, '_');
            nice = b.ToString();
            ReportDetail(nice);
        }

        [Test]
        public void OptionsTest() {
            var args = new string[] { "-add=otherfile.limo", "myfile.limo" };
            var p = new OptionSet() { { "add=", a => ReportDetail("add: "+a) }, { "file=", a => ReportDetail("file: "+a) }};
            var options = p.Parse(args);
            options.ForEach(o => ReportDetail(o));
        }
    }
}