using System;
using System.Collections.Generic;
using System.Diagnostics;
using Limaki.Common.Text.HTML.Parser;
using Limaki.UnitTest;
using NUnit.Framework;
using Attribute = Limaki.Common.Text.HTML.Parser.Attribute;

namespace Limaki.Tests.Tests.Sandbox.HTML{
	[TestFixture]
	public class HtmlNextTest : TestBase{
		public TagParser TagParser;
		public StyleParser StyleParser;

		[Test]
		public void TestingReplace(){
			
			var watch = new Stopwatch();
			
			watch.Start();

			
			var data = new HtmlTestData();
			string source = data.HTML2;
			
			TagParser = new TagParser(source);
			StyleParser = new StyleParser(source);
			
			int deleteat = -1;
			Boolean styleit = false;

			
			string result = "";
			
			
			StyleParser.Element = stuff =>{
				TagParser.Stuff(StyleParser.Stuff());
			};

			
			TagParser.Element = stuff =>{
				if (stuff.Status.Equals(Status.Name)){
					if (stuff.Element().Equals("span")){
						deleteat = stuff.StartTag;
					} else if (stuff.Element().Equals("style")){
						styleit = true;
					}
				} else if (stuff.Status.Equals(Status.Endtag)){
					if (stuff.Element().Equals("/span")){
						deleteat = stuff.StartTag;
					}
				} else if (stuff.Status.Equals(Status.Value)){
					if (stuff.Element().Equals("Flieatext")){
						TagParser.Replace(stuff.StartAt, stuff.ActAt, "Inhalt");
					}
				}
			};
			
			TagParser.Tag = stuff =>{
				if (stuff.StartTag == deleteat){
					TagParser.Remove(stuff.StartTag, stuff.ActAt);
					deleteat = -1;
				}
				else if (styleit){
					StyleParser.Stuff(TagParser.Stuff());
					StyleParser.Parse();
					styleit = false;
				}
				else{

				}

			};
			
			TagParser.Text = stuff => { };

			TagParser.Parse();
			
			watch.Stop();
			

			ReportDetail(TagParser.Stuff().Text.ToString());

			ReportDetail(result);
			ReportDetail("Vergangene Zeit: " + watch.Elapsed);
		}

		[Test]
		public void TestingTags(){
			
			var watch = new Stopwatch();
			
			watch.Start();
			Boolean styleit = false;

			
			var data = new HtmlTestData();
			string source = data.HTML19;
			
			TagParser = new TagParser(source);
			StyleParser = new StyleParser(source);

			
			var tags = new List<Tag>();
			Tag tag = null;
			var styles = new List<Style>();
			Style style = null;
			Attribute att = null;

			
			string result = "";

			
			StyleParser.Element = stuff =>{
                if (stuff.Status.Equals(Status.Prename)){
            
                    style = new Style();
                    style.Starts = stuff.StartTag;
                }
                else if (stuff.Status.Equals(Status.Name)){
            
                    if(tag != null) {
                        style.AddTag(tag);
                    }
                    tag = new Tag(stuff.Element(),stuff.StartAt,stuff.ActAt);
                    tag.Starts = stuff.StartTag;
                }
                else if (stuff.Status.Equals(Status.None)){
            
                    if(style != null) {
                        if(tag != null) {
                            style.AddTag(tag);
                            tag = null;
                        }
                        style.Ends = stuff.ActAt;
                        styles.Add(style);
                        style = null;
                    }
                }
                else if (stuff.Status.Equals(Status.Attribute)){
            
                    if(att != null) {
                        tag.AddAttribute(att);
                    }
			        att = new Attribute(stuff.Element(),stuff.StartAt,stuff.ActAt);
			    }
                else if (stuff.Status.Equals(Status.Value)){
            
                    if(att != null) {
                        att.SetValue(stuff.Element(),stuff.StartAt,stuff.ActAt);
                        tag.AddAttribute(att);
                        att = null;
                    }
                }
                else if (stuff.Status.Equals(Status.Cite)){
                    
                }
                else if (stuff.Status.Equals(Status.Commenttag)){

                }
                else if (stuff.Status.Equals(Status.Endtag)){

                }
                else if (stuff.Status.Equals(Status.Solotag)){

                }
                else if (stuff.Status.Equals(Status.Text)){

                }
                else {
                    // this could be an error
                }





//				if (stuff.Status.Equals(Status.Prename)){
//					style = new Style();
//					style.Starts = stuff.StartTag;
//				}
//				else if(stuff.Status.Equals(Status.Text)){
//					if(stuff.ActAt > stuff.StartAt){
//						att = new Attribute(stuff.Element(), stuff.StartAt, stuff.ActAt);
//						tag.AddAttribute(att);
//					}
//					tag.Ends = stuff.ActAt;
//					style.AddTag(tag);
//					tag = null;
//				}
//				else if(stuff.Status.Equals(Status.Attribute)){
//					att = new Attribute(stuff.Element(), stuff.StartAt, stuff.ActAt);
//				}
//				else if(stuff.Status.Equals(Status.Value)){
//					att.SetValue(stuff.Element(), stuff.StartAt, stuff.ActAt);
//					tag.AddAttribute(att);
//					att = null;
//				}
//				else if (stuff.Status.Equals(Status.None)){
//					if(style != null) {
//                        style.AddTag(tag);
//                        style.Ends = stuff.ActAt;
//                        styles.Add(style);
//                        style = null;
//                    }
//				}
//                else if(stuff.Status.Equals(Status.Name)) {
//                    if(tag != null) {
//                        style.AddTag(tag);
//                    }
//                    tag = new Tag(stuff.Element(), stuff.StartAt, stuff.ActAt);
//                }
//				else{
                    //Whats that?
//				}
				TagParser.Stuff(stuff);
			};
			
			TagParser.Element = stuff =>{
				if (tag == null){
					tag = new Tag();
				}
				if (stuff.Status.Equals(Status.Name)){
					if (stuff.Element().Equals("style")){
						styleit = true;
					}
					tag.Me.Name = stuff.Element();
					tag.Me.Starts = stuff.StartAt;
					tag.Me.Ends = stuff.ActAt;
				} else if (stuff.Status.Equals(Status.Commenttag)){
					tag.Me.Name = stuff.Element();
					tag.Me.Starts = stuff.StartAt;
					tag.Me.Ends = stuff.ActAt;
				} else if (stuff.Status.Equals(Status.Endtag)){
					tag.Me.Name = stuff.Element();
					tag.Me.Starts = stuff.StartAt;
					tag.Me.Ends = stuff.ActAt;
				} else if (stuff.Status.Equals(Status.Solotag)){
					tag.Me.Name = stuff.Element();
					tag.Me.Starts = stuff.StartAt;
					tag.Me.Ends = stuff.ActAt;
				} else if (stuff.Status.Equals(Status.Attribute)){
					att = new Attribute(stuff.Element(), stuff.StartAt, stuff.ActAt);
				} else if (stuff.Status.Equals(Status.Value)){
					att.SetValue(stuff.Element(), stuff.StartAt, stuff.ActAt);
					tag.AddAttribute(att);
					att = null;
				}
			};
			
			TagParser.Tag = stuff =>{
				if (tag != null){
					tag.Starts = stuff.StartTag;
					tag.Ends = stuff.ActAt;
					tag.SetStatus(stuff.Status);
					tags.Add(tag);
					tag = null;
				}
				if (styleit){
					StyleParser.Stuff(TagParser.Stuff());
					StyleParser.Parse();
					styleit = false;
				}
			};
			
			TagParser.Text = stuff => { };

			TagParser.Parse();
			
			watch.Stop();
			

			ReportDetail(TagParser.Stuff().Text.ToString());

			ReportDetail(result);
			ReportDetail("Vergangene Zeit: " + watch.Elapsed);
		}

		[Test]
		public void TestingShorten(){
			
			var watch = new Stopwatch();
			
			watch.Start();

			
			var data = new HtmlTestData();
			string source = data.HTML2;
			
			TagParser = new TagParser(source);

			
			string tagname = "p";
			Boolean start = false;
			int count = 0;
			int countto = 250;
			var tagender = new TagEnder();

			
			string result = "";

			
			TagParser.Element = stuff =>{
			
				if (stuff.Status.Equals(Status.Name)){
					result = stuff.Element();
					if (result.Equals(tagname)){
						start = true;
					}
				}
			};
			
			TagParser.Tag = stuff => { tagender.Set(stuff.Tag(), stuff.Status); };
			
			TagParser.Text = stuff =>{
				if (start){
					count = count + stuff.ActAt - stuff.StartAt;
					if (count > countto){
						result = stuff.Text.ToString().Substring(0, stuff.ActAt);
						TagParser.Stop();
					}
				}
			};

			TagParser.Parse();
			
			watch.Stop();
			
			result = result + tagender.Get();
			ReportDetail(result);
			ReportDetail("Vergangene Zeit: " + watch.Elapsed);
		}

		[Test]
		public void TestingWebsiteUseCase(){
			
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
			Action<string> Replace = (nameToReplace) =>{
				string newNanme = null;
				if (knownstyles.TryGetValue(nameToReplace, out newNanme)){
				}
			};

			
			string result = "";

			
			TagParser.Element = stuff => { };
			
			TagParser.Tag = stuff => { };
			
			TagParser.Text = stuff => { };

			TagParser.Parse();
			
			watch.Stop();
			

			ReportDetail(TagParser.Stuff().Text.ToString());

			ReportDetail(result);
			ReportDetail("Vergangene Zeit: " + watch.Elapsed);
		}

		public void TestingPattern(){
			
			var watch = new Stopwatch();
			
			watch.Start();

			
			var data = new HtmlTestData();
			string source = data.HTML2;
			
			TagParser = new TagParser(source);

			


			
			string result = "";

			
			TagParser.Element = stuff => { };
			
			TagParser.Tag = stuff => { };
			
			TagParser.Text = stuff => { };

			TagParser.Parse();
			
			watch.Stop();
			

			ReportDetail(TagParser.Stuff().Text.ToString());

			ReportDetail(result);
			ReportDetail("Vergangene Zeit: " + watch.Elapsed);
		}
	}
}
