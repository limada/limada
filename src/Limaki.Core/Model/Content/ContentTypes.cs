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

namespace Limaki.Model.Content {
    
    public class ContentTypes {
        public static  long ASCII  = unchecked((long)0xB658B74F191B9645);
        public static  long Doc   = unchecked((long)0xF4A5E7AF3232C887);
        public static  long RTF    = unchecked((long)0x720F7A018B8FF1D5);
        public static  long HTML   = unchecked((long)0x97BC58EE45132F1E);
        public static  long OLE   = unchecked((long)0x37606B1DFB0EB3DF);
        public static  long TIF   = unchecked((long)0x4EB7076141A00A0D);
        public static  long JPG   = unchecked((long)0x296FB808C4559626);
        public static  long GIF = unchecked((long)0x66825CADB2730E3C);
        public static  long PNG = unchecked((long)0x8256F278CBBA0AE3);
        public static  long EMF = unchecked((long)0x04E1CDD3A8B6E72F);
        public static  long WMF = unchecked((long)0xD4BE7EA976152B1D);
        public static  long BMP = unchecked((long)0xF7D9A1343B792E0E);
        
        public static  long Unknown= unchecked((long)0x716D9CE0F33B086A);

        public static long LimadaSheet = unchecked((long)0x5a835878a618b44d);

        static IDictionary<long, string> _extensions = null;
        public static IDictionary<long,string> Extensions {
            get {
                if (_extensions == null) {
                    _extensions = new Dictionary<long, string> ();
                    _extensions.Add (ASCII, "txt");
                    _extensions.Add(Doc, "doc");
                    _extensions.Add(RTF, "rtf");
                    _extensions.Add(HTML, "html");
                    _extensions.Add(TIF, "tif");
                    _extensions.Add(JPG, "jpg");
                    _extensions.Add(GIF, "gif");
                    _extensions.Add(PNG, "png");
                    _extensions.Add(EMF, "emf");
                    _extensions.Add(WMF, "wmf");
                    _extensions.Add(BMP, "bmp");
                }
                return _extensions;
            }
        }

        public static string Extension(long streamType) {
            string result = null;
            if (Extensions.TryGetValue(streamType, out result))
                return "."+result;
            return ".unkown";
        }
    }

    
    //stypPic  = $C5409109CB0C4137;
    //stypJPG2  = $AB0B4E3B9C3A5FE2;
    //stypIco  = $86821E40ADD3F1B3;
    //stypCur  = $05D07C8AA5646706;
}