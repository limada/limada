/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.IO;
using System.Text.RegularExpressions;
using Limada.VisualThings;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.Model.Content;
using Limaki.Visuals;
using Xwt;

namespace Limaki.View.Ui.DragDrop {

    public class VisualsTextTransferDataHandler:ITransferDataHandler<IGraph<IVisual, IVisualEdge>, IVisual>, IVisualsTransferDataHandler {

        public virtual string[] Formats {
            get {
                return new string[] {
                                        TransferDataFormats.Rtf,
                                        TransferDataFormats.Html
                                    };
            }
        }

        public Type HandledType {
            get { return typeof(IVisual); }
        }

        public void SetData (TransferDataSource data, IGraph<IVisual, IVisualEdge> container, IVisual value) {

        }

        public IVisual GetData (TransferDataSource data, IGraph<IVisual, IVisualEdge> container) {
            var dataObject = data as TransferDataSource;
            IVisual result = null;

            object description = null;
            var textInfo = new Content<string>();
            textInfo.Compression = CompressionType.bZip2;
            var encoding = System.Text.Encoding.Default;

            if (dataObject != null) {
                // find out if this is a long string:

                if (dataObject.ContainsText(TextTransferDataFormat.Text)) {
                    string plaintext = dataObject.GetText(TextTransferDataFormat.Text);

                    // find lines
                    var rx = new Regex("\r\n|\n|\r|\n|\f");
                    var matches = rx.Matches(plaintext);

                    // extract first line
                    if (matches.Count > 0) {
                        description = plaintext.Substring(0, matches[0].Index);
                    } else {
                        // if there is only one line, make a plain string-thing
                        var visual = Registry.Pool.TryGetCreate<IVisualFactory>()
                            .CreateItem(plaintext);
                        return visual;
                    }
                }

                if (dataObject.ContainsText(TextTransferDataFormat.Rtf)) {

                    textInfo.Data = dataObject.GetText(TextTransferDataFormat.Rtf);
                    textInfo.ContentType = ContentTypes.RTF;

                } else if (dataObject.ContainsText(TextTransferDataFormat.Html)) {
                    string s = null;
                    var r = dataObject.GetData(TransferDataFormats.Html, -1);//dataObject.GetText(TextTransferDataFormat.Html);
                    if (r != null)
                        s = System.Text.Encoding.UTF8.GetString(r);
                    textInfo = HTMLPostProcess(s);
                    textInfo.ContentType = ContentTypes.HTML;
                    textInfo.Compression = CompressionType.bZip2;

                    if (false) {
                        var format = "HTML Format"; //"text/html";//
                        if (dataObject.GetDataPresent(format)) {
                            encoding = System.Text.Encoding.UTF8;
                            s = GetString(dataObject, format, encoding);
                            textInfo.Data = s;
                        }
                    }
                }
            }

            if (textInfo.Data != null && textInfo.Data != string.Empty) {

                if (textInfo.Description == null)
                    textInfo.Description = description;

                var content = new Content<Stream>(textInfo);
                content.Data = new MemoryStream();
                var writer = new StreamWriter(content.Data, encoding);
                writer.Write(textInfo.Data);
                writer.Flush();

                result = new VisualThingsContentViz().VisualOfContent(container, content);

                writer.Dispose();

            }
            return result;
        }

        string GetString (TransferDataSource data, string format, System.Text.Encoding encoding) {
            string s = null;
            var dataresult = data.GetData(format);
            var stream = dataresult as Stream;
            if (stream != null)
                using (var reader = new StreamReader(stream, encoding))
                    s = reader.ReadToEnd();
            if (dataresult is string)
                s = dataresult as string;
            return s;

        }

        Content<string> HTMLPostProcess (string text) {
            var result = new Content<string>();
            result.Data = text;
            result.Description = null;
            result.Source = null;
            try {
                int startIndex = -1;
                int endIndex = -1;
                string subText = Between(text, "StartHTML:", "\r\n", 0);
                if (subText != null) int.TryParse(subText, out startIndex);
                subText = Between(text, "EndHTML:", "\r\n", 0);
                if (subText != null)
                    int.TryParse(subText, out endIndex);
                if (startIndex != -1 && endIndex != -1) {
                    endIndex = Math.Min(text.Length, endIndex);
                    result.Source = Between(text, "SourceURL:", "\r\n", 0);
                    result.Data = text.Substring(startIndex, endIndex - startIndex);
                }
            } catch (Exception e) {
                throw e;
            }
            return result;
        }

        string Between (string text, string start, string end, int startIndex) {
            int posStart = text.IndexOf(start, startIndex);
            if (posStart == -1) return null;
            posStart += start.Length;
            int posEnd = text.IndexOf(end, posStart);
            if (posEnd == -1) return null;
            return text.Substring(posStart, posEnd - posStart);
        }
    }

    public enum TextTransferDataFormat {
        Text,
        Rtf,
        Html

    }

    public class TextTransferDataHandler : VisualsTransferDataHandler<string> {
        public override string[] Formats {
            get {
                return new string[] { TransferDataFormats.Text }
                    ;
            }
        }

        public override void SetData (TransferDataSource data, IGraph<IVisual, IVisualEdge> container, IVisual value) {
            data.SetData(TransferDataFormats.Text, value.Data.ToString());
            data.SetData(TransferDataFormats.UnicodeText, value.Data.ToString());
        }
    }
}