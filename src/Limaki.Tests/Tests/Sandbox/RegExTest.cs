using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Limaki.Tests.Sandbox {
    public class RegExTest:DomainTest {
        [Test]
        public void Test () {
            string text = "First Line\r\nsecond line\n";
            Regex rx = new Regex ("\r\n|\n|\r|\n|\f");
            MatchCollection matches = rx.Matches(text);
            ReportDetail (matches.Count.ToString());
            if (matches.Count > 0) {
                string substr = text.Substring (0, matches[0].Index);
                ReportDetail(substr);
            }
        }
    }
}
