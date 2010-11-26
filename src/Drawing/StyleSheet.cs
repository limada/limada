/*
 * Limaki 
 * Version 0.063
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
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Limaki.Drawing {
    public class StyleSheet:Style, IStyleSheet {

        public StyleSheet(string name) : base(name) { }
        public StyleSheet(string name, IStyle parentStyle) : base(name, parentStyle) { }

        private string DefaultStyleName = string.Empty;
        
        public override IStyle ParentStyle {
            get { return base.ParentStyle; }
            set {
                base.ParentStyle = value;
                if (!Styles.ContainsKey(value.Name)) {
                    Styles.Add(value.Name, value);
                    InitStyles();
                }
            }
        }
        
        private Dictionary<string, IStyle> Styles = new Dictionary<string, IStyle>();

        void InitStyles() {
            if (ParentStyle != null) {
                if (!Styles.ContainsKey(StyleNames.DefaultStyle)) {
                    Styles[StyleNames.DefaultStyle] = this.ParentStyle;
                }
                if (!Styles.ContainsKey(StyleNames.SelectedStyle)) {
                    IStyle style = new Style(StyleNames.SelectedStyle, this.ParentStyle);
                    style.Pen = (Pen)style.Pen.Clone();
                    style.Pen.Width = style.Pen.Width * 2;
                    Styles[style.Name] = style;
                }
                if (!Styles.ContainsKey(StyleNames.HoveredStyle)) {
                    IStyle style = new Style(StyleNames.HoveredStyle, this.ParentStyle);
                    style.FillColor = Color.FromArgb(100, this.ParentStyle.FillColor);
                    Styles[style.Name] = style;
                }
                if (!Styles.ContainsKey(StyleNames.LinkStyle)) {
                    IStyle style = new Style(StyleNames.LinkStyle, this.ParentStyle);
                    style.Font = new Font (
                        this.ParentStyle.Font.Name, 
                        this.ParentStyle.Font.SizeInPoints-2,
                        FontStyle.Italic,
                        GraphicsUnit.Point);
                    style.Pen = (Pen)style.Pen.Clone();
                    style.Pen.EndCap = LineCap.ArrowAnchor;
                    style.Pen.StartCap = LineCap.Round;
                    Styles[style.Name] = style;
                }
                if (!Styles.ContainsKey(StyleNames.LinkHoveredStyle)) {
                    IStyle linkStyle = Styles[StyleNames.LinkStyle];
                    IStyle style = new Style(StyleNames.LinkHoveredStyle, linkStyle);
                    style.Pen = (Pen)style.Pen.Clone();
                    style.PenColor = Color.FromArgb(150, linkStyle.PenColor);
                    style.Pen.Color = style.PenColor;
                    Styles[style.Name] = style;
                }
                if (!Styles.ContainsKey(StyleNames.LinkSelectedStyle)) {
                    IStyle linkStyle = Styles[StyleNames.LinkStyle];
                    IStyle style = new Style(StyleNames.LinkSelectedStyle, linkStyle);
                    style.Pen = (Pen)style.Pen.Clone();
                    style.Pen.Width = style.Pen.Width * 2;
                    Styles[style.Name] = style;
                }
            }
        }

        public IStyle this[string styleName] {
            get { return Styles[styleName]; }
            set { Styles[styleName] = value; }
        }

        public static IStyle SystemStyle {
            get {
                IStyle result = new Style("SystemStyle");
                result.Font = (Font)SystemFonts.DefaultFont.Clone();
                result.Pen = (Pen)SystemPens.ActiveCaption.Clone();// new Pen(result.PenColor);
                result.PenColor = result.Pen.Color;
                result.Pen.Alignment = PenAlignment.Inset;
                result.TextColor = SystemColors.WindowText;
                result.FillColor = SystemColors.Window;
                result.AutoSize = Shapes.ShapeUtils.GetTextDimension (result.Font, 
                    "ABCDEFGHIJKLMNOPQRESTUVWXY\n\n\n\n", 
                    new Size ()).ToSize();
                return result;
            }
        }

        #region IStyleSheet Member

        IStyle _defaultStyle = null;
        public IStyle DefaultStyle {
            get {
                if (_defaultStyle == null)
                    _defaultStyle = Styles[StyleNames.DefaultStyle];
                return _defaultStyle;

            }
            set {
                if (value != _defaultStyle && value != null) {
                    Styles[StyleNames.DefaultStyle] = value;
                    _defaultStyle = value;
                }
            }
        }

        IStyle _selectedStyle = null;
        public IStyle SelectedStyle {
            get {
                if (_selectedStyle == null)
                    _selectedStyle = Styles[StyleNames.SelectedStyle];
                return _selectedStyle;

            }
            set {
                if (value != _selectedStyle && value != null) {
                    Styles[StyleNames.SelectedStyle] = value;
                    _selectedStyle = value;
                }
            }
        }


        IStyle _hoveredStyle = null;
        public IStyle HoveredStyle {
            get {
                if (_hoveredStyle == null)
                    _hoveredStyle = Styles[StyleNames.HoveredStyle];
                return _hoveredStyle;

            }
            set {
                if (value != _hoveredStyle && value != null) {
                    Styles[StyleNames.HoveredStyle] = value;
                    _hoveredStyle = value;
                }
            }
        }

        IStyle _linkStyle = null;
        public IStyle LinkStyle {
            get {
                if (_linkStyle == null)
                    _linkStyle = Styles[StyleNames.LinkStyle];
                return _linkStyle;

            }
            set {
                if (value != _linkStyle && value != null) {
                    Styles[StyleNames.LinkStyle] = value;
                    _linkStyle = value;
                }
            }
        }

        IStyle _linkSelectedStyle = null;
        public IStyle LinkSelectedStyle {
            get {
                if (_linkSelectedStyle == null)
                    _linkSelectedStyle = Styles[StyleNames.LinkSelectedStyle];
                return _linkSelectedStyle;

            }
            set {
                if (value != _linkSelectedStyle && value != null) {
                    Styles[StyleNames.LinkSelectedStyle] = value;
                    _linkSelectedStyle = value;
                }
            }
        }

        IStyle _linkHoveredStyle = null;
        public IStyle LinkHoveredStyle {
            get {
                if (_linkHoveredStyle == null)
                    _linkHoveredStyle = Styles[StyleNames.LinkHoveredStyle];
                return _linkHoveredStyle;

            }
            set {
                if (value != _linkHoveredStyle && value != null) {
                    Styles[StyleNames.LinkHoveredStyle] = value;
                    _linkHoveredStyle = value;
                }
            }
        }

        #endregion
    }

    public class StyleNames {
        public const string DefaultStyle = "DefaultStyle";
        public const string SelectedStyle = "SelectedStyle";
        public const string HoveredStyle = "HoveredStyle";
        public const string LinkStyle = "LinkStyle";
        public const string LinkSelectedStyle = "LinkSelectedStyle";
        public const string LinkHoveredStyle = "LinkHoveredStyle";

    }
}