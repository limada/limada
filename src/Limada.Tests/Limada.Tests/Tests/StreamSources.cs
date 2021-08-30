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
using Xwt.Headless.Backend;

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
                var rnd = new Random();

                var result = new MemoryStream();
                var writer = new StreamWriter(result);
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
        /// produces a image with some text
        /// </summary>
        public Stream Image {
            get {

                if (Toolkit.CurrentEngine.Backend.GetType () == typeof (HeadlessEngine))
                    throw new PlatformNotSupportedException();

                var result = new MemoryStream();
                var size = new Size(1000, 3000);
                // Create image.
                var builder = new ImageBuilder(size.Width, size.Height);
                var graphics = builder.Context;

                var font = Font.FromName("MS Sans Serif 14");
                var stringPos = new Point(0, 0);
                var xInc = 10;
                var yInc = 3;

                graphics.SetColor(Colors.White);
                graphics.Rectangle(0, 0, size.Width, size.Height);
                graphics.SetColor(Colors.Black);
                var rnd = new Random();

                while ((stringPos.Y < size.Height) && (stringPos.X < size.Width)) {
                    var s = "this is " + rnd.Next(10000).ToString("#,##0");
                    var textLayout = new TextLayout(graphics);
                    textLayout.Font = font;
                    textLayout.Width =size.Width - stringPos.X;
                    textLayout.Trimming = TextTrimming.WordElipsis;
                    graphics.DrawTextLayout(textLayout,stringPos);
                    stringPos.Y += textLayout.GetSize().Height + yInc;
                    stringPos.X += xInc;
                }
                var image = builder.ToBitmap(ImageFormat.RGB24);

                image.Save(result,ImageFileType.Bmp);
                result.Flush();
                image.Dispose();
                graphics.Dispose();
                result.Position = 0;
                return result;
            }
        }


        /// <summary>
        /// http://www.gnupdf.org/Introduction_to_PDF
        /// </summary>
        /// <returns></returns>
        public string Pdf() {
            return @"%PDF-1.7

1 0 obj  % entry point
<<
  /Type /Catalog
  /Pages 2 0 R
>>
endobj

2 0 obj
<<
  /Type /Pages
  /MediaBox [ 0 0 200 200 ]
  /Count 1
  /Kids [ 3 0 R ]
>>
endobj

3 0 obj
<<
  /Type /Page
  /Parent 2 0 R
  /Resources <<
    /Font <<
      /F1 4 0 R 
    >>
  >>
  /Contents 5 0 R
>>
endobj

4 0 obj
<<
  /Type /Font
  /Subtype /Type1
  /BaseFont /Times-Roman
>>
endobj

5 0 obj  % page content
<<
  /Length 44
>>
stream
BT
70 50 TD
/F1 12 Tf
(Hello, world!) Tj
ET
endstream
endobj

xref
0 6
0000000000 65535 f 
0000000010 00000 n 
0000000079 00000 n 
0000000173 00000 n 
0000000301 00000 n 
0000000380 00000 n 
trailer
<<
  /Size 6
  /Root 1 0 R
>>
startxref
492
%%EOF
";
        }
    }
}