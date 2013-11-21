/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.IO;
using System.Text;
using Limaki.Common.Text;
using Limaki.Common.Text.HTML.Parser;
using System.Linq;

namespace Limaki.Model.Content.IO {

    public class HtmlContentDigger : ContentDigger {

        private static HtmlContentInfo _info = new HtmlContentInfo();

        public HtmlContentDigger ()
            : base() {
            this.DiggUse = Digg;
        }

        protected virtual Content<Stream> Digg (Content<Stream> source, Content<Stream> sink) {
            if (!_info.Supports(source.ContentType))
                return sink;
            var buffer = new byte[source.Data.Length];
            source.Data.Read(buffer, 0, buffer.Length);
            var s = (TextHelper.IsUnicode(buffer) ? Encoding.Unicode.GetString(buffer) : Encoding.ASCII.GetString(buffer));
            if (!Fragment2Html(s, sink)) {
                // TODO: find source
            }

            Digg(s, sink);
            return sink;
        }

        /// <summary>
        /// html dragdrop delivers fragments
        /// so extract the hmtl of the fragment
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sink"></param>
        protected virtual bool Fragment2Html (string source, Content<Stream> sink) {
            try {
                var startIndex = -1;
                var endIndex = -1;
                var subText = source.Between("StartHTML:", "\r\n", 0);
                if (subText == null)
                    return false;
                if (!int.TryParse(subText, out startIndex))
                    return false;
                subText = source.Between("EndHTML:", "\r\n", 0);
                if (subText == null)
                    return false;
                if (!int.TryParse(subText, out endIndex))
                    return false;
                if (startIndex != -1 && endIndex != -1) {
                    endIndex = Math.Min(source.Length, endIndex);
                    sink.Source = source.Between("SourceURL:", "\r\n", 0);
                    sink.Data = new MemoryStream(Encoding.Default.GetBytes(source.Substring(startIndex, endIndex - startIndex)));
                }
                return true;
            } catch (Exception e) {
                throw e;
            }
        }

        public class Element {
            public string Name { get; set; }
            public bool Parsing { get; set; }
            public bool Parsed { get; set; }
            public int Starts { get; set; }
            public int Ends { get; set; }
            public string Text { get; set; }
            string _endTag = null;
            public string EndTag { get { return _endTag ?? (_endTag = string.Format("</{0}>", this.Name)); } }
        }

        protected virtual void Digg (string source, Content<Stream> sink) {
            try {
                var parser = new TagParser(source);
                var firstText = new Element { Name = "body", Text = "" };
                var elements = new Element[] {
                                new Element { Name = "title", Text = "" },
                                new Element { Name = "h1", Text = "" },
                                new Element { Name = "h2", Text = "" },
                                new Element { Name = "h3", Text = "" },
                                new Element { Name = "h4", Text = "" },
                                firstText,
                            };

                parser.DoElement += stuff => {
                    var tag = stuff.Element.ToLower();
                    foreach (var element in elements) {
                        if (!element.Parsing && !element.Parsed && stuff.State == State.Name && tag == element.Name) {
                            element.Starts = stuff.TagPosition;
                            element.Parsing = true;
                        }
                    }

                };
                parser.DoTag += stuff => {
                    var tag = stuff.Tag.ToLower();
                    foreach (var element in elements) {
                        if (element.Parsing && !element.Parsed && stuff.State == State.Endtag && tag == element.EndTag) {
                            element.Ends = stuff.Position;
                            element.Parsing = false;
                            element.Parsed = true;
                        }
                    }
                    if (firstText.Parsing && (tag == "</br>" || tag == "<br>" || tag == "<br/>" || tag == "<br />" || tag == "</div>" || tag == "</p>")) {
                        firstText.Parsing = false;
                        firstText.Parsed = true;
                    }
                };
                parser.DoText += stuff => {
                    var text = stuff.Text.ToString(stuff.Origin, stuff.Position - stuff.Origin);
                    foreach (var element in elements) {
                        if (element.Parsing)
                            element.Text += text;
                    }
                };
                parser.Parse();

                foreach (var element in elements.Where(e => e.Parsed)) {
                    sink.Description = System.Net.WebUtility.HtmlDecode(element.Text.Replace("\r\n",""));
                    break;
                }
            } catch (Exception e) {
                throw e;
            }
        }
    }
}