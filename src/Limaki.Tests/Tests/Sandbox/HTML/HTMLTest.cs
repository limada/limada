using System.Text.RegularExpressions;
using NUnit.Framework;
using Limaki.Common.Text.HTML;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Limaki.Tests.Sandbox {
    public class HtmlTest:DomainTest {
        [Test]
        public void TestParserPrototype() {
            var data = new HtmlTestData ();
            var source = data.HTML1;
            Parse (source);

        }

        public void Parse(string content) {

            var parser = new ParserPrototype();
            var tags = new List<Tag>();

            var styleStartPos = -1;
            parser.TagStarted = (info, tag) => {
                if (tag.Name == "style") {
                    //parser.ParseStyleValues (tag, info);
                    styleStartPos = tag.Start;
                }
                if (tag.Name == "/style") {
                    //parser.ParseStyleValues (tag, info);
                    // remove it, see below, starting from styleStartPos
                }
                if (tag.Name == "span" || tag.Name == "/span") {
                    tag.End = parser.ReadToEnd(tag, info);
                    if (tag.Name == "span") {
                        var len = tag.End - tag.Start + 1;
                        info.content.Remove(tag.Start, len);
                        info.i = tag.End - len;
                    }
                } else {
                    parser.ParseTagValues(tag, info);
                    tags.Add(tag);
                }
            };

            parser.Parse(content);
            foreach(var tag in tags) {
                ReportDetail (tag.Name);
            }
        }

        [Test]
        public void Test () {
            var data = new HtmlTestData ();
            var souce = data.HTML1;
            var tagger = new Tagger (souce);
            ReportDetail("*** Styles *** ");
            foreach (var style in tagger.Styles)
                ReportDetail (style.Name);
            ReportDetail("*** Tags *** ");
            foreach (var tag in tagger.Tags) {
                if (tag.Text != null) {
                    ReportDetail("--- text ---");
                    ReportDetail (tag.Text);
                    ReportDetail("--- /text ---");
                } else {
                    ReportDetail(tag.Name);    
                }
                if (tag.Attributes!=null && tag.Attributes.Count > 0) {
                    ReportDetail("\t--- attr ---");
                    foreach (var att in tag.Attributes)
                        ReportDetail ("\t"+att);
                    ReportDetail("\t--- /attr ---");
                }
            }

            ReportDetail("*** Body *** ");
            ReportDetail(tagger.Body);

            var ts = new TagStyler(data.HTML1);
            var css = ts.CSS;
            ReportDetail("*** CSS *** ");
            ReportDetail(css);

            ReportDetail("*** No Spans *** ");
            var nospans = CutSpans (souce, tagger.Tags);
            ReportDetail(nospans);

           

 
        }

        [Test]
        public void StyleSorterTest() {
            var list = new Style[] {
                                       new Style("third"),
                                       new Style("first"),
                                      new Style("z"),
                                       new Style("a very long stuff"),
                                        new Style("second"),

                                   };
            var sorter = new StyleSorter ();
            sorter.Styles = list.ToList ();
            sorter.sort ();
            foreach (var item in sorter.Styles)
                ReportDetail (item.Name);

        }
        [Test]
        public void CutCommentsTest() {
            var data = new HtmlTestData ();
            var s = CutComments (data.Commented);
            ReportDetail(s);
        }

        public string CutSpans(string source, IEnumerable<Tag> tags) {
            var result = new StringBuilder (source);
            var spans = tags
                .Where(t => t.Name == "span" || t.Name == "/span")
                .Select (t => new {t.Start, t.End})
                .OrderBy (s => s.Start);
            var cutted = 0;
            foreach(var span in spans) {
                var start = span.Start - cutted;
                var len = span.End - span.Start+1;
                result.Remove (start, len);
                cutted += len;
            }
            return result.ToString ();
        }

        public string CutComments(string source) {
            var sb = new StringBuilder(source);
            var last = default(char);
            var start = -1;
            for (int i = 0; i < sb.Length; i++) {
                var c = sb[i];
                if (last == '/' && c == '*')
                    start = i - 1;
                if (start != -1 && last == '*' && c == '/') {
                    var len = i - start + 1;
                    sb.Remove(start, len);
                    i -= len;
                    start = -1;
                }
                last = sb[i];
            }
            if (start != -1)
                sb.Remove(start, sb.Length - start);
            return sb.ToString();
        }
    }

    public class StyleSorter {

        public List<Style> Styles { get; set; }
        private List<Style> tempstyles = new List<Style>();
        public bool sorted { get; set; }
        /**Sortiert die Styles nach ihren Namen (Gross-/Kleinschreibung werden unterschieden)*/
        public void sort() {
            if (this.Styles.Count > 1) {
                Style takeit = this.Styles[0];
                int position = 0;
                for (int i = 1; i < this.Styles.Count; i++) {
                    string name = this.Styles[i].Name;
                    string smaller = getSmaller(this.Styles[i].Name, takeit.Name);
                    if (smaller == name) {
                        takeit = this.Styles[i];
                        position = i;
                    }
                }
                this.tempstyles.Add(takeit);
                this.Styles.RemoveAt(position);
                if (this.sorted == false) {
                    sort();
                }
            } else if (this.Styles.Count > 0) {
                this.tempstyles.Add(this.Styles[0]);
                this.Styles.RemoveAt(0);
                this.Styles = new List<Style>();
                this.Styles.AddRange(this.tempstyles);
                this.tempstyles = new List<Style>();
                this.sorted = true;
            }
        }
        /**Vergleich zweier Strings: Der größere wird zurückgegeben. Gross/Kleinschreibung wird unterschieden (wie in CSS)*/
        private string getSmaller(string first, string second) {
            if (first.Equals(second)) {
                /**Die Strings sind identisch*/
                return null;
            }
            int count = 0;
            if (first.Length > second.Length) {
                count = second.Length;
            } else {
                count = first.Length;
            }
            for (int i = 0; i < count; i++) {
                if (first[i] > second[i]) {
                    return second;
                } else if (first[i] < second[i]) {
                    return first;
                }
            }
            if (first.Length > second.Length) {
                return second;
            } else if (first.Length < second.Length) {
                return first;
            }
            return null;
        }
    }
}
