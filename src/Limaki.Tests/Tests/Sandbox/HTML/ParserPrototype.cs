using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Common;

namespace Limaki.Common.Text.HTML {

    public struct ParseInfo {
        public int i;
        public StringBuilder content;

    }

    public class ParserPrototype {

        public Action<ParseInfo, Tag> TagStarted;
        public Action<ParseInfo, Style> StyleStarted;

        protected void OnTagStarted(ParseInfo info, Tag tag) {
            if (TagStarted != null) {
                TagStarted(info, tag);
            }
        }


        public void Parse(string content) {

            var info = new ParseInfo();
            info.i = 0;
            info.content = new StringBuilder(content);
            while (info.i < info.content.Length) {
                var actualChar = info.content[info.i];

                // Tag starts
                if (actualChar.Equals('<')) {
                    ParseTag(info);
                }
                info.i++;
            }
        }



        public void ParseTag(ParseInfo info) {
            var tagStart = info.i;
            while (info.i < info.content.Length) {
                while (char.IsWhiteSpace(info.content[info.i]) && info.i < info.content.Length)
                    info.i++;

                // parse name
                var name = new StringBuilder();
                while (char.IsLetterOrDigit(info.content[info.i]) && info.i < info.content.Length) {
                    name.Append(info.content[info.i]);
                    info.i++;
                }

                var tag = new Tag(name.ToString()) { Start = tagStart };
                OnTagStarted(info, tag);
                info.i++;
            }
        }



        public void ParseTagValues(Tag tag, ParseInfo info) {

            while (info.content[info.i] != '/'
                && info.content[info.i] != '<' 
                && info.content[info.i] != '>' 
                && info.i < info.content.Length) {

                info.i++;

                while (char.IsWhiteSpace(info.content[info.i]) && info.i < info.content.Length)
                    info.i++;

                var valueStart = info.i;
                // parse attribute name
                var name = new StringBuilder();
                while (char.IsLetterOrDigit(info.content[info.i]) && info.i < info.content.Length) {
                    name.Append(info.content[info.i]);
                    info.i++;
                }
                while (char.IsWhiteSpace(info.content[info.i]) && info.i < info.content.Length)
                    info.i++;

                var value = new StringBuilder();
                if (info.content[info.i] == '=') {
                    while (char.IsWhiteSpace(info.content[info.i]) && info.i < info.content.Length)
                        info.i++;
                    
                    var quote = default(char);
                    if (info.content[info.i] == '\'' || info.content[info.i] == '"') {
                        quote = info.content[info.i];
                        info.i++;
                    }
                    while (!char.IsWhiteSpace(info.content[info.i])
                        && quote != info.content[info.i]
                        && info.i < info.content.Length) {
                        value.Append(info.content[info.i]);
                        info.i++;
                    }
                }
                tag.Attributes.Add (name.ToString ());
                tag.Values.Add (value.ToString ());
                
            }



        }
        public int ReadToEnd(Tag tag, ParseInfo info) {
            while (info.i < info.content.Length) {
                var actualChar = info.content[info.i];
                if (actualChar == '>')
                    return info.i;
                info.i++;
            }
            return info.i;
        }

        public void ParseStyleValues(Tag tag, ParseInfo info) {
            // parses until </style>
            while (info.i < info.content.Length) {
                var actualChar = info.content[info.i];
                var pos = info.i;

                // parse name
                //...

                // tag parsed until got name
                var name = "we got the name";

                var style = new Style(name);
                OnStyleStarted(info, style);

                info.i = pos;
            }
        }

        protected void OnStyleStarted(ParseInfo info, Style style) {
            if (StyleStarted != null) {
                StyleStarted(info, style);
            }
        }

        public void ParseStyleValues(Style style, ParseInfo info) {

        }
    }


    public class ParserPrototypeWorker {
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
        }
    }
}
