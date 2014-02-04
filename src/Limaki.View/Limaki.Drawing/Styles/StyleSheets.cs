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
using Limaki.Common;
using Xwt.Drawing;
using System.Globalization;

namespace Limaki.Drawing.Styles {

    public class StyleSheets : Dictionary<string, IStyleSheet> {

        ISystemFonts _systemfonts = null;
        protected ISystemFonts SystemFonts { get { return _systemfonts ?? (_systemfonts = Registry.Pool.TryGetCreate<ISystemFonts> ()); } }

        IDrawingUtils _drawingUtils = null;
        protected IDrawingUtils DrawingUtils { get { return _drawingUtils ?? (_drawingUtils = Registry.Pool.TryGetCreate<IDrawingUtils> ()); } }

        public IList<string> StyleSheetNames = new string[] { "Desktop", "TealSmoke", "WhiteGlass" };

        public virtual IStyleSheet DefaultStyleSheet { get { return this[StyleSheetNames[1]]; } }

        public virtual IStyleSheet PredefinedStyleSheets(string name) {
            IStyleSheet _styleSheet = null;
            var scale = Xwt.Desktop.PrimaryScreen.ScaleFactor;
            if (name == "Desktop") {
                var style = StyleSheet.CreateStyleWithSystemSettings();
                style.Name = name+"."+StyleNames.BaseStyle;
                //style.Pen.Color = style.PenColor;
                style.Font = SystemFonts.MessageBoxFont;

                _styleSheet = new StyleSheet(name, style);
                _styleSheet.ItemStyle.SelectedStyle.FillColor = SystemColors.ActiveCaption;
                
                _styleSheet.ItemStyle.SelectedStyle.TextColor = SystemColors.CaptionText;
                _styleSheet.ItemStyle.HoveredStyle.FillColor = SystemColors.Highlight;

            }
            if (name == "TealSmoke") {
                var style = StyleSheet.CreateStyleWithSystemSettings();
                style.Name = name + "." + StyleNames.BaseStyle;
                style.FillColor = DrawingExtensions.FromArgb(200, Colors.WhiteSmoke);
                style.PenColor = DrawingExtensions.FromArgb(200, Colors.Teal);
                //style.Pen.Color = style.PenColor;
                style.Font = style.Font.WithSize(10);
                _styleSheet = new StyleSheet(name, style);
                _styleSheet.ItemStyle.SelectedStyle.FillColor = Colors.Teal;
                _styleSheet.ItemStyle.SelectedStyle.TextColor = Colors.WhiteSmoke;
                _styleSheet.ItemStyle.HoveredStyle.FillColor = Colors.MintCream;

            }

            if (name == "WhiteGlass") {
                var style = StyleSheet.CreateStyleWithSystemSettings();
                style.Name = name + "." + StyleNames.BaseStyle;
                style.FillColor = DrawingExtensions.FromArgb(200, Colors.White);
                style.PenColor = DrawingExtensions.FromArgb(200, Colors.White);
                //style.Pen.Color = style.PenColor;
                style.Font = SystemFonts.MessageBoxFont;

                _styleSheet = new StyleSheet(name, style);

                _styleSheet.ItemStyle.SelectedStyle.PenColor = DrawingExtensions.FromArgb(50, 150, 150, 150);
                //_styleSheet.ItemStyle.SelectedStyle.Pen.Thickness = style.Pen.Thickness;

                var font = _styleSheet.BaseStyle.Font;
                //TODO: font.Style = FontStyle.Underline;
                _styleSheet.ItemStyle.SelectedStyle.Font = font;

                _styleSheet.ItemStyle.HoveredStyle.PenColor = DrawingExtensions.FromArgb(50, 150, 150, 150);
                _styleSheet.ItemStyle.HoveredStyle.FillColor = style.FillColor;

                _styleSheet.EdgeStyle.TextColor = DrawingExtensions.FromArgb(150, 100, 100, 100);
                _styleSheet.EdgeStyle.FillColor = Colors.White;
                _styleSheet.EdgeStyle.Font = style.Font.WithSize(style.Font.Size - 2.0d);

                var pen = (Pen)_styleSheet.EdgeStyle.Pen.Clone();
                pen.Thickness = 0.5d;
                _styleSheet.EdgeStyle.Pen = pen;
                _styleSheet.EdgeStyle.PenColor = Color.FromBytes (180, 180, 180,150);

                // this makes sense, cause CreateStyleGroup could make other colors:
                _styleSheet.EdgeStyle.HoveredStyle.FillColor = _styleSheet.EdgeStyle.FillColor;
                _styleSheet.EdgeStyle.HoveredStyle.PenColor = _styleSheet.EdgeStyle.PenColor;
                _styleSheet.EdgeStyle.HoveredStyle.TextColor = _styleSheet.EdgeStyle.TextColor;

                _styleSheet.EdgeStyle.SelectedStyle.TextColor = Color.FromBytes (50, 50, 50, 200);
                _styleSheet.EdgeStyle.SelectedStyle.PenColor = Color.FromBytes(180, 180, 180,50);
                _styleSheet.EdgeStyle.SelectedStyle.FillColor = _styleSheet.EdgeStyle.FillColor;

            }
            _styleSheet.BackColor = SystemColors.Window;
            return _styleSheet;
        }

        public virtual void Init() {
            foreach (var name in StyleSheetNames) {
                var sheet = PredefinedStyleSheets(name);
                if (sheet != null) {
                    if (!this.ContainsKey(sheet.Name))
                        this.Add(sheet.Name, sheet);
                }
            }
        }

        public new void Clear() {
            base.Clear();
            _systemfonts = null;
            _drawingUtils = null;
        }
    }
}