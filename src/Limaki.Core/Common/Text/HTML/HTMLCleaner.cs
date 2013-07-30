using System.Collections.Generic;
using System.Diagnostics;
using Limaki.Common.Text.HTML.Parser;
using System;

namespace Limaki.Common.Text.HTML {

    public class HtmlCleaner {
        public HtmlCleaner(string content) {
            this.Stuff = new Stuff(content);
        }

        public Stuff Stuff { get; private set; }
        
        TagParser _tagParser = null;
        public TagParser TagParser {
            get { return _tagParser ?? (_tagParser = new TagParser(Stuff)); }
        }
        StyleParser _styleParser = null;
        public StyleParser StyleParser{
            get { return _styleParser ?? (_styleParser = new StyleParser(Stuff)); }
        }

        public bool RemoveImages { get; set; }
        public bool RemoveSpan { get; set; }
        public bool RemoveFonts { get; set; }
        public bool RemoveStrong { get; set; }
        public bool RemoveCData { get; set; }
        public bool RemoveTable { get; set; }
        public bool RemoveStyle { get; set; }
        public bool RemoveComment { get; set; }
        public bool FistLineAsH1 { get; set; }
        /// <summary>
        /// not implemented!
        /// </summary>
        public bool RemoveBrBr { get; set; }

        protected virtual void Compose() {
            if (RemoveImages) {
                var doRemove = false;
                TagParser.DoElement += stuff => {
                                           if (!doRemove)
                                               doRemove = stuff.Status == Status.Name && stuff.Element.ToLower() == "img";
                                       };

                TagParser.DoTag += stuff => {
                                       if (doRemove) {
                                           TagParser.Remove(stuff.TagPosition, stuff.Position);
                                           doRemove = false;
                                       }
                                   };
                
            }
            if (RemoveSpan) {
                var doRemove = false;
               
                TagParser.DoElement += stuff => {
                                           var element = stuff.Element.ToLower();
                                           if (!doRemove)
                                               doRemove = 
                                                   stuff.Status == Status.Name && element== "span" ||
                                                   stuff.Status == Status.Endtag && element == "/span";
                                       };

                TagParser.DoTag += stuff => {
                                       if (doRemove) {
                                           TagParser.Remove(stuff.TagPosition, stuff.Position);
                                           doRemove = false;
                                       }
                                   };
            }
            if (RemoveFonts) {
                var doRemove = false;

                TagParser.DoElement += stuff => {
                                           var element = stuff.Element.ToLower();
                                           if (!doRemove)
                                               doRemove =
                                                   stuff.Status == Status.Name && element == "font" ||
                                                   stuff.Status == Status.Endtag && element == "/font";
                                       };

                TagParser.DoTag += stuff => {
                                       if (doRemove) {
                                           TagParser.Remove(stuff.TagPosition, stuff.Position);
                                           doRemove = false;
                                       }
                                   };
            }
            if (RemoveStrong) {
                var doRemove = false;

                TagParser.DoElement += stuff => {
                                           var element = stuff.Element.ToLower();
                                           if (!doRemove)
                                               doRemove =
                                                   stuff.Status == Status.Name && element == "strong" ||
                                                   stuff.Status == Status.Endtag && element == "/strong";
                                       };

                TagParser.DoTag += stuff => {
                                       if (doRemove) {
                                           TagParser.Remove(stuff.TagPosition, stuff.Position);
                                           doRemove = false;
                                       }
                                   };
            }

            if (RemoveCData) {
                var doRemove = false;

                TagParser.DoElement += stuff => {
                                           var element = stuff.Element;
                                           if (!doRemove)
                                               doRemove =
                                                   stuff.Status == Status.Commenttag && element.StartsWith("![CDATA");
                                       };

                TagParser.DoTag += stuff => {
                                       if (doRemove) {
                                           var start1 = stuff.TagPosition-2;
                                           var end1 = stuff.TagPosition + 11;
                                           var start2 = stuff.Position - 5;
                                           var end2 = stuff.Position+2;
                                           TagParser.Remove(start2, end2);
                                           TagParser.Remove(start1, end1);
                        
                                           doRemove = false;
                                       }
                                   };
            }

            if (RemoveTable) {
                var doRemove = false;

                TagParser.DoElement += stuff => {
                                           var element = stuff.Element.ToLower();
                                           if (!doRemove)
                                               doRemove =
                                                   (stuff.Status == Status.Name && (element == "table" || element == "td" || element == "tr"|| element == "th")) ||
                                                   (stuff.Status == Status.Endtag && (element == "/table" || element == "/td" || element == "/tr"|| element == "/th"));
                                       };

                TagParser.DoTag += stuff => {
                                       if (doRemove) {
                                           TagParser.Remove(stuff.TagPosition, stuff.Position);
                                           doRemove = false;
                                       }
                                   };
            }

            if (FistLineAsH1 && false) {
                throw new NotImplementedException ();
                var firstPara = false;
                bool removed = false;
                TagParser.DoElement += stuff => {
                    var element = stuff.Element.ToLower ();

                    if (!firstPara)
                        firstPara = stuff.Status == Status.Name && element == "p" ||
                                    stuff.Status == Status.Endtag && element == "/p";
                };

                TagParser.DoTag += stuff => {
                    if (firstPara && !removed) {
                       TagParser.Insert (stuff.TagPosition, "<H1/>");
                        removed = true;
                    }
                };
            }
            if (RemoveBrBr) {
                throw new NotImplementedException();
                var doRemove = false;
                var wasBr = false;
                TagParser.DoElement += stuff => {
                                           var element = stuff.Tag.ToLower();
                                           if (!wasBr)
                                               wasBr = stuff.Status == Status.Solotag && element == "br />";
                                       };

                TagParser.DoTag += stuff => {
                                       if (doRemove) {
                                           TagParser.Remove(stuff.TagPosition, stuff.Position);
                                           doRemove = false;
                                       }
                                   };
            }
            if (RemoveStyle) {
                var doRemove = false;
               
                TagParser.DoElement += stuff => {
                                           var element = stuff.Element.ToLower();
                                           if (!doRemove)
                                               doRemove = 
                                                   stuff.Status == Status.Name && element== "style" ||
                                                   stuff.Status == Status.Endtag && element == "/style";
                                       };

                TagParser.DoTag += stuff => {
                                       if (doRemove) {
                                           TagParser.Remove(stuff.TagPosition, stuff.Position);
                                           doRemove = false;
                                       }
                                   };
            }
            if (RemoveComment) {
                var doRemove = false;

                TagParser.DoElement += stuff => {
                    var element = stuff.Element.ToLower();
                    if (!doRemove)
                        doRemove = stuff.Status == Status.Commenttag;
                };

                TagParser.DoTag += stuff => {
                    if (doRemove) {
                        TagParser.Remove(stuff.TagPosition, stuff.Position);
                        doRemove = false;
                    }
                };
            }
        }
        public string Clean() {
            Compose();
            TagParser.Parse();
            return TagParser.Stuff.Text.ToString();
        }

        string tagText(Tag tag, Stuff stuff) {
            stuff.Origin = tag.Starts;
            stuff.Position = tag.Ends;

            return stuff.Element;
        }

    }
}