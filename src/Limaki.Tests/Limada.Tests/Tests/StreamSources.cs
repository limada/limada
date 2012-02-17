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
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

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
            yield return new StreamSource(Image, "Tiff B/W Image");
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
                SizeF size = new SizeF(1000, 3000);
                // Create image.
                Image image = new Bitmap((int)size.Width, (int)size.Height, PixelFormat.Format24bppRgb);

                // Create graphics object 
                Graphics graphics = Graphics.FromImage(image);
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                Font font = new Font("MS Sans Serif", 14f);
                PointF stringPos = new PointF(0, 0);
                float xInc = 10;
                float yInc = 3;

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;
                stringFormat.Trimming = StringTrimming.EllipsisWord;
                stringFormat.FormatFlags = StringFormatFlags.FitBlackBox;

                Brush brush = new SolidBrush(Color.White);
                graphics.FillRectangle(brush, new Rectangle(0, 0, (int)size.Width, (int)size.Height));
                brush.Dispose();

                brush = new SolidBrush(Color.Black);
                Random rnd = new Random();

                while ((stringPos.Y < size.Height) && (stringPos.X < size.Width)) {
                    string s = "this is " + rnd.Next(10000).ToString("#,##0");
                    int maxTextWidth = (int)(size.Width - stringPos.X);
                    SizeF sSize = graphics.MeasureString(s, font, maxTextWidth, stringFormat);
                    graphics.DrawString(s, font, brush, stringPos);
                    stringPos.Y += sSize.Height + yInc;
                    stringPos.X += xInc;
                }

                image.Save(result, ImageFormat.Tiff);
                result.Flush();
                image.Dispose();
                graphics.Dispose();
                result.Position = 0;
                return result;
            }
        }
    }
}