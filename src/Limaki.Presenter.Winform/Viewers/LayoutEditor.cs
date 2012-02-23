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

using System;
using System.Drawing;
using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Visuals;
using Orientation = Limaki.Drawing.Orientation;
using Size = Xwt.Size;


namespace Limaki.UseCases.Winform.Viewers {
    public partial class LayoutEditor : BaseStyleEditor<IGraphLayout<IVisual, IVisualEdge>> {
        public LayoutEditor() {
            InitializeComponent();
        }

        protected override void SetEditorFromLayout(IGraphLayout<IVisual, IVisualEdge> layout) {
            disableChanges = true;
            this.Distance = layout.Distance;
            this.Orientation = layout.Orientation;
            this.Centered = layout.Centered;
            this.LayoutBackColor = GDIConverter.Convert(layout.StyleSheet.BackColor);
            disableChanges = false;
        }

        private Size _distance = new Size(30, 15);
        public Size Distance {
            get { return _distance; }
            set {
                _distance = value;
                heightDistanceDown.Value = (int)value.Height;
                widthDistanceUpDown.Value = (int)value.Width;
                this.SelectedObject.Distance = value;
                DoPropertyValueChanged ();
            }
        }

        private Size _border = new Size(30, 15);
        public Size Border {
            get { return _border; }
            set {
                _border = value;
                //heightDistanceDown.Value = value.Height;
                //widthDistanceUpDown.Value = value.Width;
                this.SelectedObject.Border = value;
                DoPropertyValueChanged();
            }
        }

        protected Orientation _orientation = Orientation.LeftRight;
        public Orientation Orientation {
            get { return _orientation; }
            set {
                _orientation = value;
                if (value==Orientation.LeftRight) {
                    leftRightButton.Checked = true;
                } else {
                    topBottomButton.Checked = true;
                }
                this.SelectedObject.Orientation = value;
                DoPropertyValueChanged();
            }
        }

        protected bool _centered = false;
        public bool Centered {
            get { return _centered; }
            set {
                _centered = value;
                centeredCheckBox.Checked = value;
                this.SelectedObject.Centered = value;
                DoPropertyValueChanged();
            }
        }

        private void distanceUpDown_Click(object sender, EventArgs e) {
            var upDown = sender as NumericUpDown;
            if (upDown != null) {
                var value = (int)upDown.Value;
                if (value <= 0)
                    value = 10;
                if (upDown.Name.ToLower().StartsWith("height")) {
                    this.Distance = new Size(this.Distance.Width, value);
                } else if (upDown.Name.ToLower().StartsWith("width")) {
                    this.Distance = new Size(value,this.Distance.Height);
                }
            }
        }

        private void orientationButton_Click(object sender, EventArgs e) {
            var button = sender as RadioButton;
            if (button != null) {
                if (button.Name.ToLower().StartsWith("left")) {
                    this.Orientation = Orientation.LeftRight;
                } else if (button.Name.ToLower().StartsWith("top")) {
                    this.Orientation = Orientation.TopBottom;
                }

            }
        }

        private void centeredCheckBox_CheckedChanged(object sender, EventArgs e) {
            var checkBox = sender as CheckBox;
            if (checkBox != null) {
                this.Centered = checkBox.Checked;
            }
        }
        private Color _backColor = Color.Blue;
        public Color LayoutBackColor {
            get { return _backColor; }
            set {
                _backColor = value;
                SetColor(_backColor, backColorButton, backColorTransparency);
                SelectedObject.StyleSheet.BackColor = GDIConverter.Convert(_backColor);
            }
        }

        private void color_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (button != null) {
                colorDialog.Color = button.BackColor;
                if (colorDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
                    if (button.Name.ToLower().StartsWith("back")) {
                        this.LayoutBackColor =
                            Color.FromArgb(this.LayoutBackColor.A, colorDialog.Color);

                    }

                }
            }
        }

        private void Transparency_ValueChanged(object sender, EventArgs e) {
            var upDown = sender as NumericUpDown;
            if (upDown != null) {
                if (upDown.Name.ToLower().StartsWith("back")) {
                    this.LayoutBackColor =
                        Color.FromArgb((byte)upDown.Value, this.LayoutBackColor);

                }
            }
        }
    }
}