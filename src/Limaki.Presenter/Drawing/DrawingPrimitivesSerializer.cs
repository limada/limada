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

using System.Collections.Generic;
using System.Globalization;
using System.IO;
//using System.Xml;
using Limaki.Common.Collections;
using Id = System.Int64;
using System.Xml.Linq;
using System.Xml;
using System;
using Limaki.Common;
using System.Linq;

namespace Limaki.Drawing {
    public class DrawingPrimitivesSerializer : SerializerBase {

        static IDrawingUtils _drawingUtils = null;
        protected static IDrawingUtils drawingUtils {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.Create<IDrawingUtils>();
                }
                return _drawingUtils;
            }
        }



        public virtual XElement Write(Font font) {
            XElement xmlthing = new XElement("font");
            xmlthing.Add(Write("family", font.FontFamily));
            xmlthing.Add(Write("size",font.Size));
            xmlthing.Add(Write("style",font.Style));
            return xmlthing;
        }

        public virtual Font ReadBaseFont(XElement node) {
            var fam = node.Attribute("family").Value;
            var size = ReadDouble(node, "size");
            var style = ReadEnum<FontStyle>(node.Attribute("style").Value);
            Font result = new Font(fam,size);
            result.Style = style;
            return result;
        }

        public virtual Font ReadFont(XElement node) {
            var font = ReadBaseFont (node);
            Font result = drawingUtils.CreateFont (font.FontFamily,font.Size);
            result.Style = font.Style;
            return result;
        }

        public virtual XAttribute Write(Color color, string attribute) {
            return WriteInt((int)color.ToArgb(),attribute,true);
        }

        public virtual Color ReadColor(XElement node, string attribute) {
            var argb = (uint)ReadInt (node, attribute, true);
            return Color.FromArgb(argb);
        }

        public virtual XElement Write(Pen pen) {
            XElement xmlthing = new XElement("pen");
            xmlthing.Add(Write(pen.Color,"color"));
            xmlthing.Add(Write("thickness", pen.Thickness));
            xmlthing.Add(Write("startcap", pen.StartCap));
            xmlthing.Add(Write("endcap", pen.EndCap));
            xmlthing.Add(Write("linejoin", pen.LineJoin));
            return xmlthing;
        }

        public virtual Pen ReadBasePen(XElement node) {
            Pen result = new Pen ();
            result.Color = ReadColor (node, "color");
            result.Thickness = ReadDouble(node, "thickness");
            result.StartCap = ReadEnum<PenLineCap>(node.Attribute("startcap").Value);
            result.EndCap = ReadEnum<PenLineCap>(node.Attribute("endcap").Value);
            result.LineJoin = ReadEnum<PenLineJoin>(node.Attribute("linejoin").Value);
            
            return result;
        }

        public virtual Pen ReadPen(XElement node) {
            var pen = ReadBasePen(node);
            Pen result = drawingUtils.CreatePen(pen.Color);
            result.Thickness = pen.Thickness;
            result.StartCap = pen.StartCap;
            result.EndCap = pen.EndCap;
            result.LineJoin = pen.LineJoin;
            return result;
        }




        public virtual void ReadAndSetFont(XElement node, IStyle style) {
            if (style.ParentStyle == null || style.ParentStyle.Font==null) {
                style.Font =  ReadFont (node);
            } else {
                var font = ReadBaseFont (node);
                if (!style.ParentStyle.Font.Equals(font)) {
                    var result = drawingUtils.CreateFont(font.FontFamily, font.Size);
                    result.Style = font.Style;
                    style.Font = result;
                } 
            }
        }

        public virtual void ReadAndSetPen(XElement node, IStyle style) {
            if (style.ParentStyle == null || style.ParentStyle.Pen == null) {
                style.Pen = ReadPen(node);
            } else {
                var pen = ReadBasePen(node);
                if (!style.ParentStyle.Pen.Equals(pen)) {
                    var result = drawingUtils.CreatePen(pen.Color);
                    result.Thickness = pen.Thickness;
                    result.StartCap = pen.StartCap;
                    result.EndCap = pen.EndCap;
                    result.LineJoin = pen.LineJoin;
                    style.Pen = result;
                }
            }
        }

        public virtual XElement Write(IStyle style) {
            var result = new XElement("style");
            result.Add(Write("name", style.Name));
            if (style.ParentStyle != null) {
                result.Add(Write("parent", style.ParentStyle.Name));    
            }
            result.Add(Write(style.Font));
            result.Add(Write(style.Pen));
            result.Add(Write(style.FillColor, "fillcolor"));
            result.Add(Write(style.PenColor, "pencolor"));
            result.Add(Write(style.TextColor, "textcolor"));
            result.Add(Write("autosize",new Pair<int,int>(style.AutoSize.Width,style.AutoSize.Height)));
            result.Add(Write("paintdata", style.PaintData));
            return result;
        }

        public virtual SizeI ReadSize(XElement node, string attribute) {
            var size = ReadTuple (node, attribute);
            var a = 0;
            int.TryParse (size.One, out a);
            var b = 0;
            int.TryParse(size.Two, out b);
            return new SizeI (a, b);
        }

        public virtual IStyle ReadStyle(XElement node, IStyle parent) {
            var name = ReadString (node, "name");
            var parentStyle = ReadString(node, "parent");
            
            IStyle result = new Style (name,parent);
            var font = node.Elements("font").FirstOrDefault();
            if(font != null)
                ReadAndSetFont(font,result);
            var pen = node.Elements("pen").FirstOrDefault();
            if (pen != null)
                ReadAndSetPen(pen, result);
            result.FillColor = ReadColor (node, "fillcolor");
            result.PenColor = ReadColor(node, "pencolor");
            result.TextColor = ReadColor(node, "textcolor");
            result.PaintData = ReadBool(node, "paintdata");
            result.AutoSize = ReadSize (node, "autosize");
            return result;
        }

        public virtual XElement Write(IStyleSheet styleSheet) {
            var result = new XElement("stylesheet");
            result.Add(Write("name", styleSheet.Name));
            if (styleSheet.ParentStyle != null) {
                result.Add(Write("parent", styleSheet.ParentStyle.Name));
            }
            var styles = new XElement("styles");
            foreach(var style in styleSheet.Styles) {
                styles.Add (Write(style));
            }
            result.Add (styles);
            return result;
        }
        public virtual IStyleSheet ReadStyleSheet(XElement node) {
            var name = ReadString(node, "name");
            
            var parent = node.Attribute("parent");
            var styles = new Dictionary<string,IStyle> ();
            var styleList = new List<IStyle> ();
            var styleNodes = node.Element ("styles");
            IStyle parentStyle = null;
            foreach (var styleNode in styleNodes.Elements("style")) {
                var styleParent = styleNode.Attribute("parent");
                parentStyle = null;
                if (styleParent != null) {
                    styles.TryGetValue (styleParent.Value, out parentStyle);
                }
                var style = ReadStyle(styleNode, parentStyle);
                styles.Add(style.Name, style);
                styleList.Add (style);
            }
            parentStyle = null;
            if (parent != null) {
                styles.TryGetValue (parent.Value, out parentStyle);
            }
            var result = new StyleSheet(name,parentStyle);
            foreach(var style in styleList) {
                if(!result.Styles.Contains(style)){
                    result[style.Name]=style;
                }
            }
            return result;
            
        }
    }
}
