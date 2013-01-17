/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Xwt;
using Xwt.Drawing;
using Xwt.Engine;

namespace Limada.Test {
    /// <summary>
    /// Stream-Source with description
    /// </summary>
    public class StreamSource {
        public Stream Stream;
        public string Description;
        public StreamSource(Stream data, string description)
            : base() {
            this.Description = description;
            this.Stream = data;
        }
    }

    /// <summary>
    /// List of Source
    /// allSources == true: all Test-Streams, else Image
    /// writeSourceFile==true: writes Test-Streams 
    /// </summary>
    public class StreamSources :IEnumerable<StreamSource> {
        #region IEnumerable<Source> Member

        public IEnumerator<StreamSource> GetEnumerator() {
            yield return new StreamSource(Image, "Image");
            yield return new StreamSource(this.RandomHTML, "HTML random");
        }

        #endregion

        #region IEnumerable Member

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// HTML-File with formatted random numbers
        /// </summary>
        public Stream RandomHTML {
            get {
                Random rnd = new Random();

                MemoryStream result = new MemoryStream();
                StreamWriter writer = new StreamWriter(result);
                writer.Write("<HTML><BODY>");
                for (int i = 0; i <= 9999; i++) {
                    writer.Write("<p>" + rnd.Next(10000).ToString("#,##0") + "</p>");
                }
                writer.Write("</BODY></HTML>");
                writer.Flush();
                result.Position = 0;
                return result;
            }
        }

       
       /// <summary>
        /// produces a tif-image with some text
        /// </summary>
        public Stream Image {
            get {
                Stream result = new MemoryStream();
                var size = new Size(1000, 3000);
                // Create image.
                var builder = new ImageBuilder((int)size.Width, (int)size.Height,ImageFormat.RGB24);
                var graphics = builder.Context;

                var font = Font.FromName(WidgetRegistry.MainRegistry, "MS Sans Serif", 14);
                var stringPos = new Point(0, 0);
                var xInc = 10;
                var yInc = 3;

                graphics.SetColor(Colors.White);
                graphics.Rectangle(0, 0, size.Width, size.Height);
                graphics.SetColor(Colors.Black);
                var rnd = new Random();

                while ((stringPos.Y < size.Height) && (stringPos.X < size.Width)) {
                    string s = "this is " + rnd.Next(10000).ToString("#,##0");
                    var textLayout = new TextLayout(graphics);
                    textLayout.Font = font;
                    textLayout.Width =size.Width - stringPos.X;
                    textLayout.Trimming = TextTrimming.WordElipsis;
                    graphics.DrawTextLayout(textLayout,stringPos);
                    stringPos.Y += textLayout.GetSize().Height + yInc;
                    stringPos.X += xInc;
                }
                var image = builder.ToImage();

                image.Save(result, "image/tiff");
                result.Flush();
                image.Dispose();
                graphics.Dispose();
                result.Position = 0;
                return result;
            }
        }
    }
}