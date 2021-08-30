using System;
using System.Collections.Generic;
using System.Diagnostics;
using Limaki.Common.Text.HTML.Parser;
using Limaki.UnitTest;
using NUnit.Framework;
using Attribute = Limaki.Common.Text.HTML.Parser.Attribute;

namespace Limaki.Tests.Common.HTML {

    [TestFixture]
    public class HtmlNextTest : TestBase {
        public TagParser TagParser;
        public StyleParser StyleParser;

        [Test]
        public void TestingReplace() {

            var watch = new Stopwatch();

            watch.Start();


            var data = new HtmlTestData();
            string source = data.HTML2;

            TagParser = new TagParser(source);
            StyleParser = new StyleParser(source);

            var deleteat = -1;
            var styleit = false;
            var result = "";


            StyleParser.DoElement = stuff => {
                TagParser.Stuff = StyleParser.Stuff;
            };


            TagParser.DoElement = stuff => {
                if (stuff.State.Equals(State.Name)) {
                    if (stuff.Element.Equals("span")) {
                        deleteat = stuff.TagPosition;
                    } else if (stuff.Element.Equals("style")) {
                        styleit = true;
                    }
                } else if (stuff.State.Equals(State.Endtag)) {
                    if (stuff.Element.Equals("/span")) {
                        deleteat = stuff.TagPosition;
                    }
                } else if (stuff.State.Equals(State.Value)) {
                    if (stuff.Element.Equals("Flieatext")) {
                        TagParser.Replace(stuff.Origin, stuff.Position, "Inhalt");
                    }
                }
            };

            TagParser.DoTag = stuff => {
                if (stuff.TagPosition == deleteat) {
                    TagParser.Remove(stuff.TagPosition, stuff.Position);
                    deleteat = -1;
                } else if (styleit) {
                    StyleParser.Stuff = TagParser.Stuff;
                    StyleParser.Parse();
                    styleit = false;
                } else {

                }

            };

            TagParser.DoText = stuff => { };

            TagParser.Parse();

            watch.Stop();


            ReportDetail(TagParser.Stuff.Text.ToString());

            ReportDetail(result);
            ReportDetail("Vergangene Zeit: " + watch.Elapsed);
        }


        [Test]
        public void TestingTags() {

            var watch = new Stopwatch();

            watch.Start();
            var styleit = false;


            var data = new HtmlTestData();
            var source = data.HTML1;

            TagParser = new TagParser(source);
            StyleParser = new StyleParser(source);


            var tags = new List<Tag>();
            Tag tag = null;
            var styles = new List<Style>();
            Style style = null;
            Attribute att = null;


            string result = "";


            StyleParser.DoElement = stuff => {
                if (stuff.State.Equals(State.Prename)) {
                    style = new Style();
                    style.Starts = stuff.TagPosition;
                } else if (stuff.State.Equals(State.Name)) {
                    if (tag != null) {
                        style.Add(tag);
                    }
                    tag = new Tag(stuff.Element, stuff.Origin, stuff.Position);
                    tag.Starts = stuff.TagPosition;
                } else if (stuff.State.Equals(State.None)) {

                    if (style != null) {
                        if (tag != null) {
                            style.Add(tag);
                            tag = null;
                        }
                        style.Ends = stuff.Position;
                        styles.Add(style);
                        style = null;
                    }
                } else if (stuff.State.Equals(State.Attribute)) {

                    if (att != null) {
                        tag.AddAttribute(att);
                    }
                    att = new Attribute(stuff.Element, stuff.Origin, stuff.Position);
                } else if (stuff.State.Equals(State.Value)) {

                    if (att != null) {
                        att.SetValue(stuff.Element, stuff.Origin, stuff.Position);
                        tag.AddAttribute(att);
                        att = null;
                    }
                } else if (stuff.State.Equals(State.Cite)) {

                } else if (stuff.State.Equals(State.Commenttag)) {

                } else if (stuff.State.Equals(State.Endtag)) {

                } else if (stuff.State.Equals(State.Solotag)) {

                } else if (stuff.State.Equals(State.Text)) {

                } else {
                    // this could be an error
                }

                if (stuff.State.Equals(State.Prename)) {
                    style = new Style();
                    style.Starts = stuff.TagPosition;
                } else if (stuff.State.Equals(State.Text)) {
                    if (stuff.Position > stuff.Origin) {
                        att = new Attribute(stuff.Element, stuff.Origin, stuff.Position);
                        tag.AddAttribute(att);
                    }
                    tag.Ends = stuff.Position;
                    style.Add(tag);
                    tag = null;
                } else if (stuff.State.Equals(State.Attribute)) {
                    att = new Attribute(stuff.Element, stuff.Origin, stuff.Position);
                } else if (stuff.State.Equals(State.Value)) {
                    att.SetValue(stuff.Element, stuff.Origin, stuff.Position);
                    tag.AddAttribute(att);
                    att = null;
                } else if (stuff.State.Equals(State.None)) {
                    if (style != null) {
                        style.Add(tag);
                        style.Ends = stuff.Position;
                        styles.Add(style);
                        style = null;
                    }
                } else if (stuff.State.Equals(State.Name)) {
                    if (tag != null) {
                        style.Add(tag);
                    }
                    tag = new Tag(stuff.Element, stuff.Origin, stuff.Position);
                } else {
                    //Whats that?
                }
                TagParser.Stuff = stuff;
            };

            TagParser.DoElement = stuff => {
                if (tag == null) {
                    tag = new Tag();
                }
                if (stuff.State.Equals(State.Name)) {
                    if (stuff.Element.Equals("style")) {
                        styleit = true;
                    }
                    tag.Me.Name = stuff.Element;
                    tag.Me.Starts = stuff.Origin;
                    tag.Me.Ends = stuff.Position;
                } else if (stuff.State.Equals(State.Commenttag)) {
                    tag.Me.Name = stuff.Element;
                    tag.Me.Starts = stuff.Origin;
                    tag.Me.Ends = stuff.Position;
                } else if (stuff.State.Equals(State.Endtag)) {
                    tag.Me.Name = stuff.Element;
                    tag.Me.Starts = stuff.Origin;
                    tag.Me.Ends = stuff.Position;
                } else if (stuff.State.Equals(State.Solotag)) {
                    tag.Me.Name = stuff.Element;
                    tag.Me.Starts = stuff.Origin;
                    tag.Me.Ends = stuff.Position;
                } else if (stuff.State.Equals(State.Attribute)) {
                    att = new Attribute(stuff.Element, stuff.Origin, stuff.Position);
                } else if (stuff.State.Equals(State.Value)) {
                    att.SetValue(stuff.Element, stuff.Origin, stuff.Position);
                    tag.AddAttribute(att);
                    att = null;
                }
            };

            TagParser.DoTag = stuff => {
                if (tag != null) {
                    tag.Starts = stuff.TagPosition;
                    tag.Ends = stuff.Position;
                    tag.SetStatus(stuff.State);
                    tags.Add(tag);
                    tag = null;
                }
                if (styleit) {
                    StyleParser.Stuff = TagParser.Stuff;
                    StyleParser.Parse();
                    styleit = false;
                }
            };

            TagParser.DoText = stuff => { };

            TagParser.Parse();

            watch.Stop();


            ReportDetail(TagParser.Stuff.Text.ToString());

            ReportDetail(result);
            ReportDetail("Vergangene Zeit: " + watch.Elapsed);
        }

        [Test]
        public void TestingShorten() {

            var watch = new Stopwatch();

            watch.Start();


            var data = new HtmlTestData();
            string source = data.HTML1;

            TagParser = new TagParser(source);


            string tagname = "p";
            var start = false;
            int count = 0;
            int countto = 250;
            var tagender = new TagEnder();

            string result = "";

            TagParser.DoElement = stuff => {
                if (stuff.State == State.Name && stuff.Element == tagname) {
                    start = true;
                }
            };

            TagParser.DoTag = stuff => { tagender.Set(stuff.Tag, stuff.State); };

            TagParser.DoText = stuff => {
                if (start) {
                    count += stuff.Position - stuff.Origin;
                    if (count > countto) {
                        result = stuff.Text.ToString().Substring(0, stuff.Position);
                        TagParser.Stop();
                    }
                }
            };

            TagParser.Parse();

            watch.Stop();

            result = result + tagender.CloseTags();
            ReportDetail(result);
            ReportDetail("Elapsed: " + watch.Elapsed);
        }

        [Test]
        public void MissingEndTagsTest() {

            var watch = new Stopwatch();

            watch.Start();


            var data = new HtmlTestData();
            string source = data.MissingEndTags1;

            TagParser = new TagParser(source);
           
            var tagender = new TagEnder();

            string result = "";
            var lastTagPosition = 0;

            TagParser.DoElement = stuff => {
                lastTagPosition = stuff.TagPosition;
            };

            TagParser.DoTag = stuff => {
                var tag = tagender.Name(stuff.Tag);
                if (!tagender.Set(tag, stuff.State)) {
                    var tagsend = tagender.CloseTag(tag);
                    TagParser.Insert(lastTagPosition, tagsend);
                }
            };

            TagParser.DoText = stuff => {};

            TagParser.Parse();

            watch.Stop();

            result = TagParser.Stuff.Text.ToString();
            ReportDetail(result);
            ReportDetail("Elapsed: " + watch.Elapsed);
        }
        
        [Test]
        public void TestRemoveImages() {

            var watch = new Stopwatch();

            watch.Start();


            var data = new HtmlTestData();
            string source = data.MissingEndTags1;

            TagParser = new TagParser(source);

            string result = "";

            var doRemove = false;
            TagParser.DoElement = stuff => {
                                      result = stuff.Element;
                                      if(!doRemove)
                doRemove = stuff.State == State.Name && stuff.Element == "img";
                                  };

            TagParser.DoTag = stuff => {
                result = stuff.Element;
                if (doRemove) {
                    TagParser.Remove(stuff.TagPosition, stuff.Position);
                    doRemove = false;
                }
                              };

            TagParser.DoText = stuff => {
                result = stuff.Element;
                               };

            TagParser.Parse();

            watch.Stop();


            ReportDetail(TagParser.Stuff.Text.ToString());

            ReportDetail(result);
            ReportDetail("Elapsed: " + watch.Elapsed);
        }

        [Test]
        public void TestingWebsiteUseCase() {

            var watch = new Stopwatch();

            watch.Start();


            var data = new HtmlTestData();
            string source = data.HTML2;

            TagParser = new TagParser(source);

            string defaultcss = "App_Themes/Design/WebSite.css";

            //Examples:
            //knownstyles.keys = styles of "default.css" 
            //knownstyles.values = styles to replace
            IDictionary<string, string> knownstyles = new Dictionary<string, string>();
            knownstyles.Add("quelle", "source");

            // 
            Action<string> Replace = (nameToReplace) => {
                string newNanme = null;
                if (knownstyles.TryGetValue(nameToReplace, out newNanme)) {
                }
            };


            string result = "";

            var x = 0;
            TagParser.DoElement = stuff => {
                                      x = 0;
                                  };

            TagParser.DoTag = stuff => { };

            TagParser.DoText = stuff => { };

            TagParser.Parse();

            watch.Stop();


            //ReportDetail(TagParser.Stuff.Text.ToString());
            ReportDetail(x.ToString());
            ReportDetail(result);
            ReportDetail("Elapsed: " + watch.Elapsed);
        }

        public void TestingPattern() {

            var watch = new Stopwatch();

            watch.Start();


            var data = new HtmlTestData();
            string source = data.HTML2;

            TagParser = new TagParser(source);

            string result = "";


            TagParser.DoElement = stuff => { };

            TagParser.DoTag = stuff => { };

            TagParser.DoText = stuff => { };

            TagParser.Parse();

            watch.Stop();


            ReportDetail(TagParser.Stuff.Text.ToString());

            ReportDetail(result);
            ReportDetail("Elapsed: " + watch.Elapsed);
        }
    }
}
