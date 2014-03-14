using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Limaki.UnitTest;
using Limaki.Common.Collections;
using NUnit.Framework;

namespace Limaki.Tests.Common {

    public class HistoryTest : DomainTest {

        public void RunBack(History<string> history) {
            this.ReportDetail("** back");
            while (history.HasBack()) {
                this.ReportDetail(history.Back());
            }  
        }
        public void RunForward(History<string> history) {
            this.ReportDetail("** forward");
            while (history.HasForward()) {
                this.ReportDetail(history.Forward());
            }  
        }

        public void RunTest(History<string> history) {
            RunBack (history);
            RunForward (history);
            RunBack(history);
            RunForward(history);
            RunBack(history);
        }

        [Test]
        public void Test() {
            History<string> history = new History<string> ();

            history.Add ("one");
            RunTest(history);

            history.Add("two");
            history.Add("three");
            history.Add("four");
            RunTest(history);

        }
    }
}