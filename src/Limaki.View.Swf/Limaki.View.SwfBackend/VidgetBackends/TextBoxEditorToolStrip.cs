/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public partial class TextBoxEditorToolStrip : ToolStrip {
        public TextBoxEditorToolStrip() {
            InitializeComponent();
        }

        [Browsable(false)]
        TextBoxEditorControler _controller = null;
        [Browsable(false)]
        public TextBoxEditorControler Controller {
            get { return _controller; }
            set { 
                _controller = value;
                InitializeFontControl (this.fontComboBox);
                ApplyEvents ();
            }
        }

        void ApplyEvents() {
            this.boldButton.Click += (sender, args) => {
                Controller.SetFontStyle(System.Drawing.FontStyle.Bold);
            };
            this.italicButton.Click += (sender, args) => {
                Controller.SetFontStyle(System.Drawing.FontStyle.Italic);
            };
            this.underlineButton.Click += (sender, args) => {
                Controller.SetFontStyle(System.Drawing.FontStyle.Underline);
            };

            this.fontSizeComboBox.SelectedIndexChanged += (sender, args) => {
                if (fontSizeComboBox.SelectedItem != null) {
                    float newSize = Convert.ToSingle (this.fontSizeComboBox.SelectedItem.ToString ());
                    Controller.SetFontSize (newSize);
                }
            };

            this.fontComboBox.SelectedIndexChanged += (sender, args) => {
                if (fontComboBox.SelectedItem != null) {
                    string newFontFamiliy = this.fontComboBox.SelectedItem.ToString();
                    Controller.SetFontFamiliy(newFontFamiliy);
                }
            };

            Controller.SelectionChanged = this.SelectionChanged;
        }

        void InitializeFontControl(ToolStripComboBox fontComboBox) {
            fontComboBox.Items.Clear ();
            fontComboBox.SelectedItem = null;
            fontSizeComboBox.SelectedIndex = -1;
            fontComboBox.SelectedText = string.Empty;

            foreach (var fontName in Controller.FontsUsable()) {
                fontComboBox.Items.Add(fontName);
            }
        }

        public void Reset() {
            fontComboBox.SelectedItem = null;
            fontComboBox.SelectedIndex = -1;
            fontComboBox.SelectedText = string.Empty;

            fontSizeComboBox.SelectedIndex = -1;
            fontSizeComboBox.SelectedText = string.Empty;

            boldButton.Checked = false;
            italicButton.Checked = false;
            underlineButton.Checked = false;

        }

        private void SelectionChanged(RichTextBox document) {
            try {
                Controller.IsSelectionChanged = true;
                if (document.SelectionFont != null) {
                    boldButton.Checked = document.SelectionFont.Bold;
                    italicButton.Checked = document.SelectionFont.Italic;
                    underlineButton.Checked = document.SelectionFont.Underline;

                    switch (document.SelectionAlignment) {
                        case HorizontalAlignment.Left:
                            //tsbtnAlignLeft.Checked = true;
                            //tsbtnAlignCenter.Checked = false;
                            //tsbtnAlignRight.Checked = false;

                            break;

                        case HorizontalAlignment.Center:
                            //tsbtnAlignLeft.Checked = false;
                            //tsbtnAlignCenter.Checked = true;
                            //tsbtnAlignRight.Checked = false;

                            break;

                        case HorizontalAlignment.Right:
                            //tsbtnAlignLeft.Checked = false;
                            //tsbtnAlignCenter.Checked = false;
                            //tsbtnAlignRight.Checked = true;

                            break;
                    }

                    //tsbtnBullets.Checked = document.SelectionBullet;
                    //bulletsToolStripMenuItem.Checked = document.SelectionBullet;

                    fontComboBox.Text = document.SelectionFont.FontFamily.Name;
                    int size = (int) document.SelectionFont.Size;
                    fontSizeComboBox.SelectedItem = size.ToString ();
                }
            } finally {
                Controller.IsSelectionChanged = false;
            }
        } 
    }
}
