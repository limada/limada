/*
 * Limaki 
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


using System.Collections.Generic;

namespace Limaki.Contents {

    public class ContentTypes {

        public static long Unknown = unchecked ((long)0x716D9CE0F33B086A);

        public static long Text = Limaki.Contents.IO.TextContentSpot.Text;
        public static long HTML = Limaki.Contents.IO.HtmlContentSpot.HTML;
        public static long Markdown = Limaki.Contents.IO.MarkdownContentSpot.Markdown;

        public static long ASCII = Limaki.Contents.IO.TextContentSpot.ASCII;
        public static long RTF = Limaki.Contents.IO.RtfContentSpot.RTF;

        public static long PNG = Limaki.Contents.IO.ImageContentSpot.PNG;
        public static long TIF = Limaki.Contents.IO.ImageContentSpot.TIF;
        public static long JPG = Limaki.Contents.IO.ImageContentSpot.JPG;
        
        public static long GIF = Limaki.Contents.IO.ImageContentSpot.GIF;
        public static long BMP = Limaki.Contents.IO.ImageContentSpot.BMP;
        public static long DIB = Limaki.Contents.IO.ImageContentSpot.DIB;

        public static long LimadaSheet = unchecked ((long)0x5a835878a618b44d);
        public static long Uri = unchecked ((long)0x273d44bf29878947);


        public static long EMF = unchecked ((long)0x04E1CDD3A8B6E72F);
        public static long WMF = unchecked ((long)0xD4BE7EA976152B1D);
        
        public static long Word97 = unchecked ((long)0xF4A5E7AF3232C887);
        public static long OLE = unchecked ((long)0x37606B1DFB0EB3DF);

    }


    //stypPic  = 0xC5409109CB0C4137;
    //stypJPG2  = 0xAB0B4E3B9C3A5FE2;
    //stypIco  = 0x86821E40ADD3F1B3;
    //stypCur  = 0x05D07C8AA5646706;


}