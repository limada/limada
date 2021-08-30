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


using System;
using System.Collections.Generic;
using Limaki.Common;
using Xwt.Drawing;
using System.Globalization;
using Xwt;

namespace Limaki.Drawing.Styles {

    public class StyleSheet:Style, IStyleSheet {
        
        public Int64 Id { get; set; }
        public StyleSheet(string name) : base(name) {
            this.BackColor = SystemColors.Window;
        }

        public StyleSheet(string name, IStyle parentStyle) : base(name, parentStyle) {
            this.BackColor = SystemColors.Window;
        }


        public IStyle BaseStyle { get; set; }

        public IStyleGroup ItemStyle { get; set; }
        public IStyleGroup EdgeStyle { get; set; }

        public virtual Color BackColor { get; set; }

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

        protected SystemFonts _systemfonts = null;
        protected SystemFonts SystemFonts { get { return _systemfonts ?? (_systemfonts = SystemFonts.Fonts); } }

        protected IDrawingUtils _drawingUtils = null;
        protected IDrawingUtils DrawingUtils { get { return _drawingUtils ?? (_drawingUtils = Registry.Pooled<IDrawingUtils>()); } }

        protected virtual Font CreateFont (string familiy, double size) {
            return Font.FromName(familiy + " " + size.ToString(CultureInfo.InvariantCulture));
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
            
            var defaultStyle = new Style(name + ".Default", styleGroup);
            styleGroup.DefaultStyle = defaultStyle;
            styles[defaultStyle.Name] = defaultStyle;

            var selectedStyle = new Style(name + ".Selected", styleGroup);
            styleGroup.SelectedStyle = selectedStyle;
            styles[selectedStyle.Name] = selectedStyle;

            var hoveredStyle = new Style(name + ".Hovered", styleGroup);
            styleGroup.HoveredStyle = hoveredStyle;
            styles[hoveredStyle.Name] = hoveredStyle;

            if (!forEdge) {
                selectedStyle.FillColor = DrawingExtensions.FromArgb(200, this.ParentStyle.FillColor);
                hoveredStyle.FillColor = DrawingExtensions.FromArgb(200, this.ParentStyle.FillColor);
            } else {
                defaultStyle.PaintData = false;

                defaultStyle.Font = CreateFont(
                    this.ParentStyle.Font.Family,
                    this.ParentStyle.Font.Size - 2).WithStyle(FontStyle.Italic);

                selectedStyle.LineWidth = styleGroup.DefaultStyle.LineWidth * 2;
                selectedStyle.PaintData = true;

                hoveredStyle.StrokeColor = DrawingExtensions.FromArgb(150, styleGroup.DefaultStyle.StrokeColor);
                hoveredStyle.PaintData = true;
            }
            
            return styleGroup;
        }

        protected void InitStyles() {
            if (ParentStyle != null) {
                if (this.BaseStyle == null) {
                    var style = new Style(this.Name + "." + StyleNames.BaseStyle, this.ParentStyle);
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

                    //var pen = (Pen)this.ParentStyle.Pen.Clone ();
                    //pen.Color = this.ParentStyle.PenColor;
                    //style.Pen = pen;

                    //style.PenColor = pen.Color;
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
            var systemfonts = SystemFonts.Fonts;
            var drawingUtils = Registry.Pooled<IDrawingUtils>();

            result.Font = systemfonts.DefaultFont;
            result.LineWidth = 1;
            result.StrokeColor = SystemColors.ActiveCaption;
            
            result.TextColor = SystemColors.WindowText;
            result.FillColor = SystemColors.Window;

            result.AutoSize = drawingUtils.GetTextDimension("ABCDEFGHIJKLMNOPQRESTUVWXY\n\n\n\n", result);
            result.PaintData = true;
            return result;
        }

        static IStyleSheet _defaultStyleSheet = null;
        public static IStyleSheet CreateDefaultStyleSheet() {
            if (_defaultStyleSheet == null) {
                var style = StyleSheet.CreateStyleWithSystemSettings ();
                _defaultStyleSheet = new StyleSheet ("Default", style);
                _defaultStyleSheet.BackColor = SystemColors.Window;
            }
            return _defaultStyleSheet;
        }

        public override bool Equals (object obj) {
            var other = obj as IStyleSheet;
            if (other == null)
                return false;

            return base.Equals (other)
                   && BaseStyle.Equals (other.BaseStyle)
                   && ItemStyle.Equals (other.ItemStyle)
                   && EdgeStyle.Equals (other.EdgeStyle)
                   && BackColor.Equals (other.BackColor);
        }

        public override object Clone () {
            return base.Clone ();
        }

        public override void CopyTo (IStyle other) {
            if (other is IStyleSheet && this != other) {
                CopyTo ((IStyleSheet) other);
                return;
            }
            base.CopyTo (other);
        }

        public virtual void CopyTo (IStyleSheet other) {
            base.CopyTo (other);
            other.BackColor = this.BackColor;

            other.ParentStyle = MakeCopy (this.ParentStyle, null);
            other.BaseStyle = MakeCopy (this.BaseStyle, other.ParentStyle);

            Func<IStyleGroup, IStyleGroup> makeCopy = (source) => {
                IStyleGroup sink = null;
                if (source != null) {
                    sink = new StyleGroup (source.Name, other.BaseStyle);
                    source.CopyTo (sink);
                }
                return sink;
            };

            other.ItemStyle = makeCopy (this.ItemStyle);
            other.EdgeStyle = makeCopy (this.EdgeStyle);
        }

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