/*
 * Limada
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2016 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Collections.Generic;
using System.Xml.Linq;
using Limaki.Common;
using System.Linq;
using Limaki.Drawing.Styles;
using Xwt;
using Xwt.Drawing;
using System.Globalization;

//using System.Xml;

namespace Limaki.Drawing {

    public class DrawingXmlSerializer : XmlSerializerBase {

		public override XAttribute Write<T>(string attribute, T item)
		{

			if (typeof(T) == typeof(Point))
			{
				var point = (Point)(object)item;
				return Write(attribute, new Pair<string, string>(point.X.ToString(CultureInfo.InvariantCulture), point.Y.ToString(CultureInfo.InvariantCulture)));
			}

			if (typeof(T) == typeof(Size))
			{
				var size = (Size)(object)item;
				return Write(attribute, new Pair<string, string>(size.Width.ToString(CultureInfo.InvariantCulture), size.Height.ToString(CultureInfo.InvariantCulture)));
			}

			if (typeof(T) == typeof(Color))
			{
				var color = (Color)(object)item;
				return WriteColor(color, attribute);
			}

			return base.Write(attribute, item);
		}


        public override T Read<T> (XElement node, string attribute) {
            if (node == null)
                return default(T);
						
            if (typeof (T) == typeof (Size)) {
                return (T) (object) ReadSize (node, attribute);
            }

            if (typeof (T) == typeof (Point)) {
                var point = ReadTuple (node, attribute);
                var a = 0d;
                double.TryParse (point.One, NumberStyles.Number, CultureInfo.InvariantCulture, out a);
                var b = 0d;
                double.TryParse (point.Two, NumberStyles.Number, CultureInfo.InvariantCulture, out b);
                return (T) (object) new Point (a, b);
            }

            if (typeof (T) == typeof (Color)) {
                return (T) (object) ReadColor (node, attribute);
            }

            return base.Read<T> (node, attribute);
        }

        public virtual Size ReadSize (XElement node, string attribute) {
            var size = ReadTuple (node, attribute);
            var a = 0d;
            double.TryParse (size.One, NumberStyles.Number, CultureInfo.InvariantCulture, out a);
            var b = 0d;
            double.TryParse (size.Two, NumberStyles.Number, CultureInfo.InvariantCulture, out b);
            return new Size (a, b);
        }

        public virtual XAttribute WriteColor (Color color, string attribute) {
            return Write (attribute, (int)color.ToArgb ());
        }
        
        public virtual Color ReadColor (XElement node, string attribute) {
            var argb = (uint)Read<int> (node, attribute);
            return DrawingExtensions.FromArgb (argb);
        }
    }

    public class StyleXmlSerializer : DrawingXmlSerializer {

        static IDrawingUtils _drawingUtils = null;
        protected static IDrawingUtils DrawingUtils { get { return _drawingUtils ?? (_drawingUtils = Registry.Factory.Create<IDrawingUtils>()); } }

        public static class NodeNames {
            public const string StyleSheet = "stylesheet";
            public const string Styles = "styles";

            public const string Name = "name";
            public const string Parent = "parent";
            public const string FillColor = "fillcolor";
            public const string StrokeColor = "strokecolor";
            public const string LineWidth = "linewidth";
            public const string TextColor = "textcolor";
            public const string PaintData = "paintdata";
            public const string AutoSize = "autosize";
            public const string Font = "font";
            public const string FontFamily = "family";
            public const string FontSize = "size";
            public const string FontStyle = "style";
            public const string FontWeight = "weight";
            public const string FontStretch = "stretch";
        }

        public virtual XElement Write (Font font) {
            var node = new XElement (NodeNames.Font);
            node.Add (Write (NodeNames.FontFamily, font.Family ?? ""));
            node.Add (Write (NodeNames.FontSize, font.Size));
            node.Add (Write (NodeNames.FontStyle, font.Style));
            node.Add (Write (NodeNames.FontWeight, font.Weight));
            node.Add (Write (NodeNames.FontStretch, font.Stretch));
            return node;
        }

        protected  virtual Font CreateFont (string fam, double size, FontStyle style, FontWeight weigth, FontStretch stretch) {
            return Font.FromName (fam + " " +
                style.ToString () + " " +
                weigth.ToString () + " " +
                stretch.ToString () + " " +
                size.ToString (CultureInfo.InvariantCulture));
        }

        public virtual Font ReadBaseFont (XElement node) {
            var fam = node.Attribute (NodeNames.FontFamily).Value;
            var size = Read<double> (node, NodeNames.FontSize);
            var style = Read<FontStyle> (node, NodeNames.FontStyle);
            var weight = Read<FontWeight> (node, NodeNames.FontWeight);
            var stretch = Read<FontStretch> (node, NodeNames.FontStretch);
            var result = CreateFont (fam, size, style, weight, stretch);

            return result;
        }

        public virtual Font ReadFont(XElement node) {
            var font = ReadBaseFont (node);
            return font;
        }

        public virtual void ReadAndSetFont(XElement node, IStyle style) {
            if (style.ParentStyle == null || style.ParentStyle.Font==null) {
                style.Font =  ReadFont (node);
            } else {
                var font = ReadBaseFont (node);
                if (!style.ParentStyle.Font.Equals(font)) {
                    style.Font = font;
                } 
            }
        }

        public virtual XElement Write(IStyle style) {
            var result = new XElement(NodeNames.FontStyle);
            result.Add(Write(NodeNames.Name, style.Name));
            if (style.ParentStyle != null) {
                result.Add(Write(NodeNames.Parent, style.ParentStyle.Name));    
            }
            result.Add(Write(style.Font));
            result.Add(WriteColor(style.FillColor, NodeNames.FillColor));
            result.Add(WriteColor(style.StrokeColor, NodeNames.StrokeColor));
            result.Add (WriteDouble (style.LineWidth, NodeNames.LineWidth));
            result.Add(WriteColor(style.TextColor, NodeNames.TextColor));
            result.Add(Write(NodeNames.AutoSize,new Pair<double,double>(style.AutoSize.Width,style.AutoSize.Height)));
            result.Add(Write(NodeNames.PaintData, style.PaintData));
            return result;
        }

        public virtual IStyle ReadStyle(XElement node, IStyle parent) {
            var name = ReadString (node, NodeNames.Name);
            var parentStyle = ReadString(node, NodeNames.Parent);
            
            var result = new Style (name,parent);
            var font = node.Elements(NodeNames.Font).FirstOrDefault();
            if(font != null)
                ReadAndSetFont(font,result);

            result.FillColor = ReadColor (node, NodeNames.FillColor);
            result.StrokeColor = ReadColor(node, NodeNames.StrokeColor);
            result.LineWidth = ReadDouble (node, NodeNames.LineWidth);
            result.TextColor = ReadColor(node, NodeNames.TextColor);
            result.PaintData = ReadBool(node, NodeNames.PaintData);
            result.AutoSize = ReadSize (node, NodeNames.AutoSize);
            return result;
        }

        public virtual XElement Write(IStyleSheet styleSheet) {
            var result = new XElement (NodeNames.StyleSheet);
            result.Add(Write(NodeNames.Name, styleSheet.Name));
            if (styleSheet.ParentStyle != null) {
                result.Add(Write(NodeNames.Parent, styleSheet.ParentStyle.Name));
            }
            var styles = new XElement (NodeNames.Styles);
            foreach(var style in styleSheet.Styles) {
                styles.Add (Write(style));
            }
            result.Add (styles);
            return result;
        }

        public virtual IStyleSheet ReadStyleSheet(XElement node) {
            var name = ReadString(node, NodeNames.Name);
            
            var parent = node.Attribute(NodeNames.Parent);
            var styles = new Dictionary<string,IStyle> ();
            var styleList = new List<IStyle> ();
            var styleNodes = node.Element (NodeNames.Styles);
            IStyle parentStyle = null;
            foreach (var styleNode in styleNodes.Elements(NodeNames.FontStyle)) {
                var styleParent = styleNode.Attribute(NodeNames.Parent);
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
