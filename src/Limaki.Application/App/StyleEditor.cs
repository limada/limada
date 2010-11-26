using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Limaki.App {
    using Converter = Limaki.Drawing.GDI.GDIExtensions;
    
    public partial class StyleEditor : UserControl {
        public StyleEditor() {
            InitializeComponent();
        }

        public event EventHandler PropertyValueChanged;

        bool disableChanges = false;
        void DoPropertyValueChanged() {
            if (PropertyValueChanged != null && ! disableChanges) {
                PropertyValueChanged (this, new EventArgs());
            }
        }

        Limaki.Drawing.IStyle _selectedObject = null;
        public Limaki.Drawing.IStyle SelectedObject {
            get {
                if (_selectedObject == null) {
                    _selectedObject = new Limaki.Drawing.Style("none");
                    SetEditorFromStyle(_selectedObject);
                }
                return _selectedObject;
            }
            set {
                _selectedObject = value;
                SetEditorFromStyle (_selectedObject);
            }
        }

        void SetEditorFromStyle(Limaki.Drawing.IStyle style) {
            disableChanges = true;
            try {
                if (style == null)
                    return;
                this.FillColor = Converter.Native (style.FillColor);
                this.TextColor = Converter.Native (style.TextColor);
                this.PenColor = Converter.Native (style.PenColor);
                if (style.Font != null &&
                    ( style.Font is Limaki.Drawing.GDI.GDIFont )) {
                    this.StyleFont = ( (Limaki.Drawing.GDI.GDIFont) style.Font ).Native;
                }
                this.StyleAutoSize = Converter.Native (style.AutoSize);
                this.PaintData = style.PaintData;
                if (style.Pen != null) {
                    this.PenThickness = style.Pen.Thickness;
                }
            } finally {
                disableChanges = false;
            }
        }

        void SetColor (Color color, Button colorButton, NumericUpDown colorTransparency) {
            colorButton.BackColor = color; //Color.FromArgb(255, color);
            colorTransparency.Value = color.A;
            DoPropertyValueChanged ();
        }

        private Color _fillColor = Color.Blue;
        public Color FillColor {
            get { return _fillColor; }
            set {
                _fillColor = value;
                SetColor (_fillColor, fillColorButton, fillTransparency);
                SelectedObject.FillColor = Converter.Toolkit (_fillColor);
            }
        }

        private Color _textColor = Color.Blue;
        public Color TextColor {
            get { return _textColor; }
            set {
                _textColor = value;
                SetColor(_textColor, textColorButton, textTransparency);
                SelectedObject.TextColor = Converter.Toolkit(_textColor);
            }
        }
        private Color _penColor = Color.Blue;
        public Color PenColor {
            get { return _penColor; }
            set {
                _penColor = value;
                SetColor(_penColor, penColorButton, penTransparency);
                SelectedObject.PenColor = Converter.Toolkit(_penColor);
            }
        }

        Font _font = null;
        public Font StyleFont {
            get { return _font; }
            set { 
                _font = value;
                if (value != null) {
                    fontButton.Font = _font;
                    var font = new Limaki.Drawing.GDI.GDIFont ();
                    Limaki.Drawing.GDI.GDIUtils.SetFont (font, _font);
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
                    var pen = new Limaki.Drawing.GDI.GDIPen ();
                    pen.Thickness = value;
                    pen.Color = Converter.Toolkit (this.PenColor);
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

                SelectedObject.AutoSize = Converter.Toolkit (_autoSize);
                
                this.heightAutoSizeUpDown.Value = (value.Height<10000? value.Height: -1);
                this.widthAutoSizeUpDown.Value = (value.Width<10000? value.Width:-1);

                DoPropertyValueChanged();
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
                if (colorDialog.ShowDialog(this)==DialogResult.OK) {
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
                if (fontDialog.ShowDialog(this)==DialogResult.OK) {
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
