using System.Text.RegularExpressions;
using NUnit.Framework;
using Limaki.Tests;
using System;

namespace Limaki.Playground.Proves {

    public class RegExProves : DomainTest {

        public void ShowResults (Regex rx, string text) {
            var matches = rx.Matches (text);
            ReportDetail (matches.Count.ToString ());
            if (matches.Count > 0) {
                var substr = text.Substring (matches[0].Index);
                ReportDetail (substr);
            }
            
        }

        [Test]
        public void ProveFirstLine() {
            var text = "First Line\r\nsecond line\n";
            var rx = new Regex ("\r\n|\n|\r|\n|\f");
            var matches = rx.Matches (text);
            ReportDetail (matches.Count.ToString ());
            if (matches.Count > 0) {
                var substr = text.Substring (0, matches[0].Index);
                ReportDetail (substr);
            }
            
        }

        [Test]
        public void RxProveEuropeanDateTime () {
            var text = "1.7.12 22:46; 22:46; 12:5 Pm; 1/7/12 22:46; 22.03.2006 1:16 aM; 31/2.06 22:33:22; 22.46.03; 1.3/1999 ";
            var rgx1 = @"^([1-9]|0[1-9]|[12][0-9]|3[01])[-\.]([1-9]|0[1-9]|1[012])[-\.]\d{4}$";
            var rgx2 = @"^([0]?[1-9]|[1|2][0-9]|[3][0|1])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$";
            var rgx3 = @"^(?=\d)(?:(?!(?:(?:0?[5-9]|1[0-4])(?:\.|-|\/)10(?:\.|-|\/)(?:1582))|(?:(?:0?[3-9]|1[0-3])(?:\.|-|\/)0?9(?:\.|-|\/)(?:1752)))(31(?!(?:\.|-|\/)(?:0?[2469]|11))|30(?!(?:\.|-|\/)0?2)|(?:29(?:(?!(?:\.|-|\/)0?2(?:\.|-|\/))|(?=\D0?2\D(?:(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:(?:\d\d)(?:[02468][048]|[13579][26])(?!\x20BC))|(?:00(?:42|3[0369]|2[147]|1[258]|09)\x20BC))))))|2[0-8]|1\d|0?[1-9])([-.\/])(1[012]|(?:0?[1-9]))\2((?=(?:00(?:4[0-5]|[0-3]?\d)\x20BC)|(?:\d{4}(?:$|(?=\x20\d)\x20)))\d{4}(?:\x20BC)?)(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])"
            +@"(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]\d|2[0-3])(?::[0-5]\d){1,2})?$";
            //text = "22:46";
            var rx = new Regex (rgx3);
            ShowResults (rx, text);
            foreach (var t in text.Split (';')) {
               
                ShowResults (rx, t.Trim());
            }
        }


        [Test]
        public void ProveEuropeanDateTime () {

            var text = "1.7.12 ,22:46; 1.JUni.12 22:46; 22:46; 12:5 Pm; 1/7/12 22:46; 22.03.2006 1:16 aM; 31/2.06 22:33:22; 22.46.03; 1.3/1999 ";


            foreach (var t in text.Split (';')) {
                var d = default (DateTime);
                if(DateTime.TryParse(t,out d))
                ReportDetail ("{0}", d);
                else
                    ReportDetail ("failed {0}", t);

            }
        }
    }
}
