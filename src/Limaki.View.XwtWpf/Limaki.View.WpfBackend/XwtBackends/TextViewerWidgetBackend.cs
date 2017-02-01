/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Limaki.Drawing;
using Limaki.View.Vidgets;
using Xwt;
using Xwt.Backends;
using XD = Xwt.Drawing;
using Xwt.WPFBackend;
using SWM = System.Windows.Media;
using SW = System.Windows;
using System.Linq;
using System.Text;

namespace Limaki.View.WpfBackend {

    public class TextViewerWidgetBackend : Xwt.WPFBackend.WidgetBackend, ITextViewerWidgetBackend, IZoomTarget {

        protected RichTextBox TextBox { get; set; }

        public TextViewerWidgetBackend () : this (new RichTextBox ()) { }

        protected TextViewerWidgetBackend (RichTextBox textBox) {
            this.Widget = textBox;
            this.TextBox = textBox;
            this.TextBox.TextChanged += (s, e) => {
                Modified = true;
            };
            ShowFrame = false;

            this.TextBox.SelectionChanged += (s, e) => {
                if (_selectionChanged != null)
                    _selectionChanged (this, e);
            };

            FlowDocument.PageWidthProperty.Bind (TextBox, s => s.ActualWidth, TextBox.Document, BindingMode.OneWay);
           // FlowDocument.PageHeightProperty.Bind (TextBox, s => s.ActualHeight, TextBox.Document, BindingMode.OneWay);

        }

        public bool ReadOnly {
            get { return TextBox.IsReadOnly; }
            set { TextBox.IsReadOnly = value; }
        }

        public bool Modified { get; set; }
        
        public bool EnableAutoDragDrop { get; set; }

        public Point AutoScrollOffset {
            get { return new Point (TextBox.VerticalOffset, TextBox.HorizontalOffset); }
            set { 
                //TextBox.VerticalOffset = value.X;
                //TextBox.HorizontalOffset = value.Y;
            }
        }

        public void Save (Stream stream, TextViewerTextType textType) {
            TextBox.Selection.Save (stream, textType.ToWpf ());
        }

        public void Load (Stream stream, TextViewerTextType textType) {
            var format = textType.ToWpf ();
            if (format == SW.DataFormats.UnicodeText) {
                var reader = new StreamReader (stream, textType == TextViewerTextType.UnicodePlainText ? Encoding.Unicode : Encoding.ASCII);
                var text = reader.ReadToEnd ();
                TextBox.Document.Blocks.Clear ();
                TextBox.Document.Blocks.Add (new Paragraph(new Run(text)));
            } else {
                new TextRange (TextBox.Document.ContentStart, TextBox.Document.ContentEnd)
                    .Load (stream, format);
            }

            Modified = false;
            TextBox.Selection.Select (TextBox.Document.ContentStart, TextBox.Document.ContentStart);
        }

        public void Clear () {
            TextBox.Document.Blocks.Clear ();
        }

        private bool _showFrame = true;
        public bool ShowFrame {
            get { return this._showFrame; }
            set {
                if (this._showFrame == value)
                    return;

                if (value) {
                    TextBox.ClearValue (Control.BorderBrushProperty);
                    TextBox.ClearValue (Control.BorderThicknessProperty);
                } else {
                    TextBox.BorderBrush = null;
                    TextBox.BorderThickness = new SW.Thickness ();
                }

                this._showFrame = value;
            }
        }

        public ZoomState ZoomState { get; set; }

        public double ZoomFactor { get; set; }
        public void UpdateZoom () { }

        protected override void Dispose (bool disposing) {
            base.Dispose (disposing);
        }

        public void SetAttribute (XD.TextAttribute a) {
            var range = new TextRange (TextBox.Selection.Start, TextBox.Selection.End);

            var visit = new TextAttributeVisitor {

                FontTextAttribute = attribute => {
                    if (!string.IsNullOrEmpty (attribute.Font.Family))
                        range.ApplyPropertyValue (FlowDocument.FontFamilyProperty, attribute.Font.Family);
                    if (attribute.Font.Size > 0)
                        range.ApplyPropertyValue (FlowDocument.FontSizeProperty, attribute.Font.Size);

                },

                FontDataAttribute = attribute => {
                    if (!string.IsNullOrEmpty (attribute.FontFamily))
                        range.ApplyPropertyValue (FlowDocument.FontFamilyProperty, attribute.FontFamily);
                    if (attribute.FontSize > 0)
                        range.ApplyPropertyValue (FlowDocument.FontSizeProperty, attribute.FontSize);

                },
                FontWeightTextAttribute = attribute =>
                    range.ApplyPropertyValue (FlowDocument.FontWeightProperty, attribute.Weight.ToWpfFontWeight()),

                FontStyleTextAttribute = attribute =>
                    range.ApplyPropertyValue (FlowDocument.FontStyleProperty, attribute.Style.ToWpfFontStyle()),

                StrikethroughTextAttribute = attribute =>
                    TextDecoration (range, SW.TextDecorations.Strikethrough, attribute.Strikethrough),

                UnderlineTextAttribute = attribute =>
                    TextDecoration (range, SW.TextDecorations.Underline, attribute.Underline),

                BackgroundTextAttribute = attribute => 
                    range.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SWM.SolidColorBrush(attribute.Color.ToWpfColor())),

                ColorTextAttribute = attribute =>
                     range.ApplyPropertyValue (FlowDocument.ForegroundProperty, new SWM.SolidColorBrush (attribute.Color.ToWpfColor ())),
            };

            visit.Visit (a);
        }

        public IEnumerable<XD.TextAttribute> GetAttributes () {

            var range = new TextRange (TextBox.Selection.Start, TextBox.Selection.End);
            var ff = range.GetPropertyValue (FlowDocument.FontFamilyProperty) as SW.Media.FontFamily;
            var size = (double)range.GetPropertyValue (FlowDocument.FontSizeProperty);
            if (ff != null)
                yield return new XD.FontDataAttribute { FontFamily = ff.FamilyNames.First ().Value, FontSize = size };

            var fw = (SW.FontWeight) range.GetPropertyValue (FlowDocument.FontWeightProperty);
            yield return new XD.FontWeightTextAttribute { Weight = fw.ToXwtFontWeight() };

            var fs = (SW.FontStyle) range.GetPropertyValue (FlowDocument.FontStyleProperty);
            yield return new XD.FontStyleTextAttribute { Style = fs.ToXwtFontStyle() };

            var decor = range.GetPropertyValue (Inline.TextDecorationsProperty) as SW.TextDecorationCollection;
            if (decor != null) {
                yield return new XD.StrikethroughTextAttribute { Strikethrough = decor.Any (d => d.Location == SW.TextDecorationLocation.Strikethrough) };
                yield return new XD.UnderlineTextAttribute { Underline = decor.Any (d => d.Location == SW.TextDecorationLocation.Underline) };

            }

            var brush = range.GetPropertyValue (FlowDocument.BackgroundProperty) as SWM.Brush;
            if (brush != null)
                yield return new XD.BackgroundTextAttribute { Color = brush.ToXwtColor () };

            brush = range.GetPropertyValue (FlowDocument.ForegroundProperty) as SWM.Brush;
            if (brush != null)
                yield return new XD.ColorTextAttribute { Color = brush.ToXwtColor () };

        }

        public void TextDecoration (TextRange range, SW.TextDecorationCollection decor, bool add) {
            var td = range.GetPropertyValue (Inline.TextDecorationsProperty) as SW.TextDecorationCollection;
            if (td == null)
                return;
            td = td.CloneCurrentValue ();
            foreach (var d in decor) {
                var r = td.FirstOrDefault (s => s.Location == d.Location);
                if (add) {
                    if (r == null)
                        td.Add (d.Clone ());
                } else {
                    if (r != null)
                        td.Remove (r);
                }
            }
            decor = td;

            range.ApplyPropertyValue (Inline.TextDecorationsProperty, decor);
        }

        private EventHandler _selectionChanged;
        public event EventHandler SelectionChanged {
            add { _selectionChanged += value; }
            remove { _selectionChanged -= value; }
        }

        
    }
}