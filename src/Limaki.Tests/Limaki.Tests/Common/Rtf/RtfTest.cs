using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Limaki.Common;
using Limaki.Common.Text.RTF.Parser;
using Limaki.UnitTest;
using NUnit.Framework;

namespace Limaki.Tests.Common.Rtf {
    [TestFixture]
    public class RtfTest : TestBase {

        [Test]
        public void TestParse () {

            var rtf = new RTF (ByteUtils.AsAsciiStream (RtfTestData.WordGame));

            rtf.ClassCallback[TokenClass.Control] = r => {
                if (r.Major == Major.CharAttr) {
                    if (r.Minor == Minor.FontSize) {
                        Trace.Write (string.Format ("FontSize {0} ", r.Param));
                        return;
                    }
                }
            };

            var t = new StringBuilder ();
            rtf.ClassCallback[TokenClass.Text] = r => {
                t.Append (r.EncodedText);
            };

            rtf.Read ();

            Trace.WriteLine ("");
            Trace.WriteLine (t.ToString ());
        }
    }
}