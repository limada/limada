/*
 * Limada
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System.Collections.Generic;
using System.Globalization;
using System.IO;
//using System.Xml;
using Limaki.Common.Collections;
using Id = System.Int64;
using System.Xml.Linq;
using System.Xml;
using System;

namespace Limaki.Drawing {
    public class DrawingPrimitivesSerializer {
        public virtual XAttribute Write<T>(T item) {
            return new XAttribute(this.GetType().Name, item.ToString());
        }
        public virtual XAttribute Write<T>(string name, T item) {
            return new XAttribute(name, item.ToString());
        }

        public virtual XElement Write(Font font) {
            XElement xmlthing = new XElement("Font");
            xmlthing.Add(Write("family", font.FontFamily));
            xmlthing.Add(Write("size",font.Size));
            xmlthing.Add(Write("style",font.Style));
            return xmlthing;
        }


        public double ReadDouble(XElement node, string attribute) {
            double result = default(int);
            string s = node.Attribute(attribute).Value;
            if (!string.IsNullOrEmpty(s)) {
                    double.TryParse(s, out result);
            }
            return result;
        }
        
        public string ReadString(XElement node, string attribute) {
            string result = node.Attribute(attribute).Value;
            return result;
        }

        public virtual Font Read(XElement font) {
            Font result = new Font();
            result.FontFamily = font.Attribute ("family").Value;
            result.Size = ReadDouble (font, "size");
            result.Style = (FontStyle)Enum.Parse(typeof(FontStyle), font.Attribute("style").Value);
            ;
            return result;
        }
    }
}
