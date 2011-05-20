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

namespace Limaki.Drawing {
    //TODO: StyleSheet should be changed to something like:
    //Style"Bundle" with default, selected, hovered etc.
    public class StyleSheet:Style, IStyleSheet {

        public StyleSheet(string name) : base(name) {
            this.BackColor = KnownColors.FromKnownColor (KnownColor.Window);
        }
        public StyleSheet(string name, IStyle parentStyle) : base(name, parentStyle) {
            this.BackColor = KnownColors.FromKnownColor (KnownColor.Window);
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
        protected static IDrawingUtils drawingUtils {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.Create<IDrawingUtils>();
                }
                return _drawingUtils;
            }
        }

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
        
        private Dictionary<string, IStyle> styles = new Dictionary<string, IStyle>();
        public ICollection<IStyle> Styles {
            get { return styles.Values; }
            
        }

        protected virtual Font CreateFont(string familiy, double size) {
            return drawingUtils.CreateFont (familiy, size);
        }

        void InitStyles() {
            if (ParentStyle != null) {
                if (!styles.ContainsKey(StyleNames.DefaultStyle)) {
                    styles[StyleNames.DefaultStyle] = 
                        new Style(StyleNames.DefaultStyle, this.ParentStyle);
                }
                if (!styles.ContainsKey(StyleNames.SelectedStyle)) {
                    IStyle style = 
                        new Style(StyleNames.SelectedStyle, this.ParentStyle);
                    var pen = (Pen)style.Pen.Clone();
                    pen.Thickness = pen.Thickness * 2;
                    style.Pen = pen;
                    styles[style.Name] = style;
                }

                if (!styles.ContainsKey(StyleNames.HoveredStyle)) {
                    IStyle style = 
                        new Style(StyleNames.HoveredStyle, this.ParentStyle);
                    style.FillColor = Color.FromArgb(200, this.ParentStyle.FillColor);
                    styles[style.Name] = style;
                }

                if (!styles.ContainsKey(StyleNames.EdgeStyle)) {
                    IStyle style = 
                        new Style(StyleNames.EdgeStyle, this.ParentStyle);
                    style.PaintData = false;

                    style.Font = CreateFont(
                        this.ParentStyle.Font.FontFamily, 
                        this.ParentStyle.Font.Size-2);

                    style.Font.Style = FontStyle.Italic;
                    
                    var pen = (Pen)style.Pen.Clone();

                    float arrowWidth = (float)style.Pen.Thickness * 5.5f;
                    float arrowHeigth = (float)style.Pen.Thickness * 1.5f;
                    pen.CustomEndCap = 
                        drawingUtils.GetCustomLineCap(arrowWidth, arrowHeigth);
                    
                    pen.StartCap = PenLineCap.Round;
                    
                    style.Pen = pen;

                    styles[style.Name] = style;
                    
                }

                if (!styles.ContainsKey(StyleNames.EdgeHoveredStyle)) {
                    IStyle edgeStyle = styles[StyleNames.EdgeStyle];
                    IStyle style = 
                        new Style(StyleNames.EdgeHoveredStyle, edgeStyle);

                    var pen = (Pen)style.Pen.Clone();
                    pen.Color = Color.FromArgb(150, edgeStyle.PenColor);
                    style.PenColor = pen.Color;
                    style.Pen = pen;
                    styles[style.Name] = style;
                    style.PaintData = true;
                }

                if (!styles.ContainsKey(StyleNames.EdgeSelectedStyle)) {
                    IStyle edgeStyle = styles[StyleNames.EdgeStyle];
                    IStyle style = 
                        new Style(StyleNames.EdgeSelectedStyle, edgeStyle);
                    
                    var pen = (Pen)style.Pen.Clone();
                    pen.Thickness = edgeStyle.Pen.Thickness * 2;
                    style.Pen = pen;
                    style.PaintData = true;

                    styles[style.Name] = style;
                }
                if (!styles.ContainsKey(StyleNames.ResizerToolStyle)) {
                    IStyle style = 
                        new Style(StyleNames.ResizerToolStyle, this.ParentStyle);

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
            IStyle result = new Style ("Style with System Settings");
            result.Font = (Font) SystemFonts.DefaultFont.Clone ();
            result.Pen =
                drawingUtils.CreatePen (KnownColors.FromKnownColor (KnownColor.ActiveCaption));

            result.PenColor = result.Pen.Color;
            
            result.TextColor = 
                KnownColors.FromKnownColor(KnownColor.WindowText);
            result.FillColor = 
                KnownColors.FromKnownColor(KnownColor.Window);
            
            result.AutoSize =
                drawingUtils.GetTextDimension(
                    "ABCDEFGHIJKLMNOPQRESTUVWXY\n\n\n\n", result).ToSize();
            result.PaintData = true;
            return result;
        }

        #region IStyleSheet Member

        public virtual Color BackColor { get; set; }

        static IStyleSheet _defaultStyleSheet = null;

        public static IStyleSheet CreateDefaultStyleSheet() {
            if (_defaultStyleSheet == null) {
                IStyle style = StyleSheet.CreateStyleWithSystemSettings ();
                _defaultStyleSheet = new StyleSheet ("Default", style);
                _defaultStyleSheet.BackColor = KnownColors.FromKnownColor (KnownColor.Window);
            }
            return _defaultStyleSheet;
        }

        IStyle _defaultStyle = null;
        public IStyle DefaultStyle {
            get {
                if (_defaultStyle == null)
                    _defaultStyle = styles[StyleNames.DefaultStyle];
                return _defaultStyle;

            }
            set {
                if (value != _defaultStyle && value != null) {
                    styles[StyleNames.DefaultStyle] = value;
                    _defaultStyle = value;
                }
            }
        }

        IStyle _selectedStyle = null;
        public IStyle SelectedStyle {
            get {
                if (_selectedStyle == null)
                    _selectedStyle = styles[StyleNames.SelectedStyle];
                return _selectedStyle;

            }
            set {
                if (value != _selectedStyle && value != null) {
                    styles[StyleNames.SelectedStyle] = value;
                    _selectedStyle = value;
                }
            }
        }


        IStyle _hoveredStyle = null;
        public IStyle HoveredStyle {
            get {
                if (_hoveredStyle == null)
                    _hoveredStyle = styles[StyleNames.HoveredStyle];
                return _hoveredStyle;

            }
            set {
                if (value != _hoveredStyle && value != null) {
                    styles[StyleNames.HoveredStyle] = value;
                    _hoveredStyle = value;
                }
            }
        }

        IStyle _edgeStyle = null;
        public IStyle EdgeStyle {
            get {
                if (_edgeStyle == null)
                    _edgeStyle = styles[StyleNames.EdgeStyle];
                return _edgeStyle;

            }
            set {
                if (value != _edgeStyle && value != null) {
                    styles[StyleNames.EdgeStyle] = value;
                    _edgeStyle = value;
                }
            }
        }

        IStyle _edgeSelectedStyle = null;
        public IStyle EdgeSelectedStyle {
            get {
                if (_edgeSelectedStyle == null)
                    _edgeSelectedStyle = styles[StyleNames.EdgeSelectedStyle];
                return _edgeSelectedStyle;

            }
            set {
                if (value != _edgeSelectedStyle && value != null) {
                    styles[StyleNames.EdgeSelectedStyle] = value;
                    _edgeSelectedStyle = value;
                }
            }
        }

        IStyle _edgeHoveredStyle = null;
        public IStyle EdgeHoveredStyle {
            get {
                if (_edgeHoveredStyle == null)
                    _edgeHoveredStyle = styles[StyleNames.EdgeHoveredStyle];
                return _edgeHoveredStyle;

            }
            set {
                if (value != _edgeHoveredStyle && value != null) {
                    styles[StyleNames.EdgeHoveredStyle] = value;
                    _edgeHoveredStyle = value;
                }
            }
        }

        IStyle _resizerToolStyle = null;
        public IStyle ResizerToolStyle {
            get {
                if (_resizerToolStyle == null)
                    _resizerToolStyle = styles[StyleNames.ResizerToolStyle];
                return _resizerToolStyle;

            }
            set {
                if (value != _resizerToolStyle && value != null) {
                    styles[StyleNames.ResizerToolStyle] = value;
                    _resizerToolStyle = value;
                }
            }
        }

        #endregion
    }

    public class StyleNames {
        public const string DefaultStyle = "DefaultStyle";
        public const string SelectedStyle = "SelectedStyle";
        public const string HoveredStyle = "HoveredStyle";
        public const string EdgeStyle = "EdgeStyle";
        public const string EdgeSelectedStyle = "EdgeSelectedStyle";
        public const string EdgeHoveredStyle = "EdgeHoveredStyle";
        public const string ResizerToolStyle = "ResizerToolStyle";

    }
}