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
 * http://limada.sourceforge.net
 * 
 */


using System.Collections.Generic;
using Limaki.Common;
using Id = System.Int64;

namespace Limaki.Drawing {
    public class StyleSheet:Style, IStyleSheet {
        public Id Id { get; set; }
        public StyleSheet(string name) : base(name) {
            this.BackColor = KnownColors.FromKnownColor (KnownColor.Window);
        }
        public StyleSheet(string name, IStyle parentStyle) : base(name, parentStyle) {
            this.BackColor = KnownColors.FromKnownColor (KnownColor.Window);
        }

        public IStyleGroup ItemStyle { get; set; }
        public IStyleGroup EdgeStyle { get; set; }
        public virtual Color BackColor { get; set;}
        public override IStyle ParentStyle {
            get { return base.ParentStyle; }
            set {
                base.ParentStyle = value;
                if (value !=null && !styles.ContainsKey(value.Name)) {
                    styles.Add(value.Name, value);
                    InitStyles();
                }
            }
        }

        static ISystemFonts _systemfonts = null;
        protected static ISystemFonts SystemFonts {
            get {
                if (_systemfonts == null) {
                    _systemfonts = Registry.Factory.Create<ISystemFonts>();
                }
                return _systemfonts;
            }
        }

        static IDrawingUtils _drawingUtils = null;
        protected static IDrawingUtils DrawingUtils {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.Create<IDrawingUtils>();
                }
                return _drawingUtils;
            }
        }
        
        protected virtual Font CreateFont(string familiy, double size) {
            return DrawingUtils.CreateFont(familiy, size);
        }

        private IDictionary<string, IStyle> styles = new Dictionary<string, IStyle>();
        public ICollection<IStyle> Styles {
            get { return styles.Values; }

        }

        private IDictionary<string, IStyleGroup> _styleGroups = new Dictionary<string, IStyleGroup>();
        public ICollection<IStyleGroup> StyleGroups {
            get { return _styleGroups.Values; }
            
        }

        public IStyleGroup CreateStyleGroup(string name, IStyle parentStyle, bool forEdge) {
            var styleGroup = new StyleGroup(name,parentStyle);
            if (!forEdge) {
                var style = styleGroup.DefaultStyle;
                
                styles[style.Name] = style;

                var selectedStyle =new Style(name+".Selected", this.ParentStyle);
                selectedStyle.FillColor = Color.FromArgb(200, this.ParentStyle.FillColor);
                styleGroup.SelectedStyle = selectedStyle;
                styles[selectedStyle.Name] = selectedStyle;

                var hoveredStyle = new Style(name + ".Hovered", this.ParentStyle);
                styleGroup.HoveredStyle = hoveredStyle;
                hoveredStyle.FillColor = Color.FromArgb(200, this.ParentStyle.FillColor);
                styles[hoveredStyle.Name] = hoveredStyle;

            } else {
                var style = styleGroup.DefaultStyle;
                style.PaintData = false;

                style.Font = CreateFont(
                    this.ParentStyle.Font.FontFamily,
                    this.ParentStyle.Font.Size - 2);

                style.Font.Style = FontStyle.Italic;

                var pen = (Pen)style.Pen.Clone();
                var arrowWidth = (float)style.Pen.Thickness * 5.5f;
                var arrowHeigth = (float)style.Pen.Thickness * 1.5f;
                pen.CustomEndCap = DrawingUtils.GetCustomLineCap(arrowWidth, arrowHeigth);
                pen.StartCap = PenLineCap.Round;
                style.Pen = pen;

                styles[style.Name] = style;

                var selectedStyle = new Style(name + ".Selected", styleGroup.DefaultStyle);
                styleGroup.SelectedStyle = selectedStyle;

                pen = (Pen)selectedStyle.Pen.Clone();
                pen.Thickness = styleGroup.DefaultStyle.Pen.Thickness * 2;
                selectedStyle.Pen = pen;
                selectedStyle.PaintData = true;

                styles[selectedStyle.Name] = selectedStyle;

                var hoveredStyle = new Style(name + ".Hovered", styleGroup.DefaultStyle);
                styleGroup.HoveredStyle = hoveredStyle;

                pen = (Pen)hoveredStyle.Pen.Clone();
                pen.Color = Color.FromArgb(150, styleGroup.DefaultStyle.PenColor);
                hoveredStyle.PenColor = pen.Color;
                hoveredStyle.Pen = pen;
                styles[hoveredStyle.Name] = hoveredStyle;
                hoveredStyle.PaintData = true;
            }
            
            return styleGroup;
        }

        protected void InitStyles() {
            if (ParentStyle != null) {
                if (this.BaseStyle==null) {
                    var style = new Style(this.Name + "."+StyleNames.BaseStyle, this.ParentStyle);
                    styles[style.Name] = style;
                    this.BaseStyle = style;
                }

                if(this.ItemStyle==null) {
                    this.ItemStyle = CreateStyleGroup(this.Name + ".ItemStyle", this.ParentStyle, false);
                }
                if (this.EdgeStyle == null) {
                    this.EdgeStyle = CreateStyleGroup(this.Name + ".EdgeStyle", this.ParentStyle, true);
                }

                if (!styles.ContainsKey(StyleNames.ResizerToolStyle)) {
                    var style = new Style(StyleNames.ResizerToolStyle, this.ParentStyle);

                    var pen = (Pen)this.ParentStyle.Pen.Clone ();
                    pen.Color = this.ParentStyle.PenColor;
                    style.Pen = pen;

                    style.PenColor = pen.Color;
                    style.FillColor = this.ParentStyle.FillColor;
                    style.TextColor = this.ParentStyle.TextColor;
                    styles[style.Name] = style;
                }
            }
        }

        public IStyle this[string styleName] {
            get {
                IStyle result = null;
                styles.TryGetValue(styleName,out result);
                return result;
            }
            set { styles[styleName] = value; }
        }

        public static IStyle CreateStyleWithSystemSettings() {
            var result = new Style ("SystemStyle");
            result.Font = (Font) SystemFonts.DefaultFont.Clone ();
            result.Pen = DrawingUtils.CreatePen (KnownColors.FromKnownColor (KnownColor.ActiveCaption));

            result.PenColor = result.Pen.Color;
            
            result.TextColor = KnownColors.FromKnownColor(KnownColor.WindowText);
            result.FillColor = KnownColors.FromKnownColor(KnownColor.Window);
            
            result.AutoSize = DrawingUtils.GetTextDimension("ABCDEFGHIJKLMNOPQRESTUVWXY\n\n\n\n", result).ToSize();
            result.PaintData = true;
            return result;
        }

        static IStyleSheet _defaultStyleSheet = null;
        public static IStyleSheet CreateDefaultStyleSheet() {
            if (_defaultStyleSheet == null) {
                var style = StyleSheet.CreateStyleWithSystemSettings ();
                _defaultStyleSheet = new StyleSheet ("Default", style);
                _defaultStyleSheet.BackColor = KnownColors.FromKnownColor (KnownColor.Window);
            }
            return _defaultStyleSheet;
        }

        public IStyle BaseStyle { get; set; }


        #region IEnumerable<IStyle> Member

        public IEnumerator<IStyle> GetEnumerator() {
            return styles.Values.GetEnumerator();
        }

      
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        #endregion
    }
}