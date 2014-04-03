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
using System.Drawing;
using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.GdiBackend;
using Limaki.Drawing.Styles;
using Xwt.GdiBackend;

namespace Limaki.SwfBackend.Controls {
    public partial class StyleEditor : BaseStyleEditor<IStyle> {
        public StyleEditor() {
            InitializeComponent();
        }


        public override IStyle SelectedObject {
            get {
                if (_selectedObject == null) {
                    _selectedObject = new Style( "none" );
                    SetEditorFromStyle(_selectedObject);
                }
                return _selectedObject;
            }
            set {
                _selectedObject = value;
                SetEditorFromStyle (_selectedObject);
            }
        }

        void SetEditorFromStyle(IStyle style) {
            disableChanges = true;
            try {
                if (style == null)
                    return;
                this.FillColor = style.FillColor.ToGdi();
                this.TextColor = style.TextColor.ToGdi();
                this.PenColor = style.PenColor.ToGdi();
                if ( style.Font != null ) {
                    var swfFont = style.Font.ToGdi();
                    if ( swfFont != null )
                        this.StyleFont = swfFont;
                }
                this.StyleAutoSize = style.AutoSize.ToGdi();
                this.PaintData = style.PaintData;
                if (style.Pen != null) {
                    this.PenThickness = style.Pen.Thickness;
                }
            } finally {
                disableChanges = false;
            }
        }


        private Color _fillColor = Color.Blue;
        public Color FillColor {
            get { return _fillColor; }
            set {
                _fillColor = value;
                SetColor (_fillColor, fillColorButton, fillTransparency);
                SelectedObject.FillColor = _fillColor.ToXwt(); 
            }
        }

        private Color _textColor = Color.Blue;
        public Color TextColor {
            get { return _textColor; }
            set {
                _textColor = value;
                SetColor(_textColor, textColorButton, textTransparency);
                SelectedObject.TextColor = GdiConverter.ToXwt(_textColor);
            }
        }
        private Color _penColor = Color.Blue;
        public Color PenColor {
            get { return _penColor; }
            set {
                _penColor = value;
                SetColor(_penColor, penColorButton, penTransparency);
                SelectedObject.PenColor = GdiConverter.ToXwt(_penColor);
            }
        }

        Font _font = null;
        public Font StyleFont {
            get { return _font; }
            set { 
                _font = value;
                if (value != null) {
                    fontButton.Font = _font;
                    var font =  _font.ToXwt();
                    SelectedObject.Font = font;
                }
                DoPropertyValueChanged();
            }
        }

        double _penThickness = 1;
        public double PenThickness {
            get {
                return _penThickness;
            }
            set {
                _penThickness = value;
                if (SelectedObject.Pen != null) {
                    SelectedObject.Pen.Thickness = value;
                } else {
                    var pen = new GdiPen ();
                    pen.Thickness = value;
                    pen.Color = GdiConverter.ToXwt (this.PenColor);
                    SelectedObject.Pen = pen;
                }
                this.penThicknessUpDown.Value = (decimal)value;
                DoPropertyValueChanged();
            }
        }

        private Size _autoSize = Size.Empty;
        public Size StyleAutoSize {
            get { return _autoSize; }
            set {
                _autoSize = value;

                SelectedObject.AutoSize = _autoSize.ToXwt ();

                this.heightAutoSizeUpDown.Value = (value.Height < 10000 ? value.Height : -1);
                this.widthAutoSizeUpDown.Value = (value.Width < 10000 ? value.Width : -1);

                DoPropertyValueChanged ();
            }
        }

        private bool _paintData;
        public bool PaintData {
            get { return _paintData; } 
            set { _paintData = value;
                SelectedObject.PaintData = value;
                this.paintDataCheckBox.Checked = value;
                DoPropertyValueChanged();
            }
        }

        private void color_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (button != null) {
                colorDialog.Color = button.BackColor;
                if (colorDialog.ShowDialog(this)==System.Windows.Forms.DialogResult.OK) {
                    if (button.Name.ToLower().StartsWith("fill")) {
                        this.FillColor =
                            Color.FromArgb(this.FillColor.A, colorDialog.Color);

                    }
                    if (button.Name.ToLower().StartsWith("text")) {
                        this.TextColor =
                            Color.FromArgb(this.TextColor.A, colorDialog.Color);

                    }
                    if (button.Name.ToLower().StartsWith("pen")) {
                        this.PenColor =
                            Color.FromArgb(this.PenColor.A, colorDialog.Color);

                    }
                }
            }
        }

        private void Transparency_ValueChanged(object sender, EventArgs e) {
            var upDown = sender as NumericUpDown;
            if (upDown != null) {
                if (upDown.Name.ToLower().StartsWith("fill")) {
                    this.FillColor =
                        Color.FromArgb((byte)upDown.Value, this.FillColor);

                }
                if (upDown.Name.ToLower().StartsWith("text")) {
                    this.TextColor =
                        Color.FromArgb((byte)upDown.Value, this.TextColor);

                } else if (upDown.Name.ToLower().StartsWith("pen")) {
                    this.PenColor =
                        Color.FromArgb((byte)upDown.Value, this.PenColor);

                }
            }
        }

        private void fontButton_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (StyleFont != null) {
                fontDialog.Font = new Font(StyleFont,StyleFont.Style);
                if (fontDialog.ShowDialog(this)==System.Windows.Forms.DialogResult.OK) {
                    this.StyleFont = fontDialog.Font;
                }
            }
        }

        private void penThicknessUpDown_Click(object sender, EventArgs e) {
            var upDown = sender as NumericUpDown;
            if (upDown != null) {
                this.PenThickness = (double)upDown.Value;
            }
        }

        private void AutoSizeUpDown_Click(object sender, EventArgs e) {
            var upDown = sender as NumericUpDown;
            if (upDown != null) {
                var value = (int) upDown.Value;
                if (value <=0 )
                    value = int.MaxValue;
                if (upDown.Name.ToLower().StartsWith("height")) {
                    this.StyleAutoSize = new Size(this.StyleAutoSize.Width, value);
                } else if (upDown.Name.ToLower().StartsWith("width")) {
                    this.StyleAutoSize = new Size(value, this.StyleAutoSize.Height);
                }
            }
        }

        private void paintDataCheckBox_CheckedChanged(object sender, EventArgs e) {
            var checkBox = sender as CheckBox;
            if (checkBox !=null) {
                this.PaintData = checkBox.Checked;
            }
        }



    }
}