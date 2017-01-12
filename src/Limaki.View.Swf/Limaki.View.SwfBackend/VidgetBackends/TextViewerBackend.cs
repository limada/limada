/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 */


using Limaki.Drawing;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Limaki.View;
using Limaki.View.Vidgets;
using Xwt.GdiBackend;
using Limaki.Common;
using System.Text;
using Limaki.Common.Text;
using XD = Xwt.Drawing;
using System.Collections.Generic;
using System;
using Xwt;

// this control uses ideas from RicherTextBox by ???

namespace Limaki.View.SwfBackend.VidgetBackends {

    public partial class TextViewerBackend : VidgetBackend<UserControl>, IZoomTarget, ITextViewerVidgetBackend {

        protected override void Compose () {
            base.Compose ();
            InitializeComponent ();
            Multiline = true;
            //innerTextBox.Enter += (sender, args) => { Control.OnEnter (args); };
            //innerTextBox.MouseUp += (sender, args) => { Control.OnMouseUp (args); };
            //innerTextBox.GotFocus += (sender, args) => { Control.OnGotFocus (args); };
            EnableAutoDragDrop = true;
        }

        TextBoxEditorControler _controller = null;
        [Browsable(false)]
        public TextBoxEditorControler Controller {
            get {
                if (_controller == null) {
                    _controller = new TextBoxEditorControler(this.innerTextBox);
                }
                return _controller;
            }
        }

        //[Browsable(true)]
        //TextBoxEditorToolStrip0 _toolStrip = null;
        //public TextBoxEditorToolStrip0 ToolStrip {
        //    get { return _toolStrip; }
        //    set {
        //        if (value != null) {
        //            value.Controller = this.Controller;
        //        } else {
        //            if (_toolStrip != null) {
        //                _toolStrip.Controller = null;
        //            }
        //        }
        //        _toolStrip = value;
        //    }
        //}

        [Browsable(true)]
        public virtual bool Multiline {
            get { return this.innerTextBox.Multiline; }
            set { this.innerTextBox.Multiline = value; }
        }

        [Browsable(true)]
        public virtual bool EnableAutoDragDrop {
            get { return this.innerTextBox.EnableAutoDragDrop; }
            set { this.innerTextBox.EnableAutoDragDrop = value; }
        }

        [Browsable(true)]
        public virtual bool ReadOnly {
            get { return this.innerTextBox.ReadOnly; }
            set { this.innerTextBox.ReadOnly = value; }
        }

        [Browsable(false)]
        public virtual bool Modified {
            get { return this.innerTextBox.Modified; }
            set { this.innerTextBox.Modified = value; }
        }

        public void Clear () {
            this.innerTextBox.Clear();
        }

        protected virtual void AfterLoadRTF () {
            //if (this._toolStrip != null) {
            //    _toolStrip.Reset();
            //}
        }

        public string Rtf {
            get { return this.innerTextBox.Rtf; }
            set {
                this.innerTextBox.Rtf = value;
                AfterLoadRTF();
            }
        }

        public void Load (Stream stream, TextViewerTextType textType) {
            //innerTextBox.Clear ();
            var color = innerTextBox.BackColor;
            if (textType == TextViewerTextType.RichText)
                innerTextBox.LoadFile (stream, RichTextBoxStreamType.RichText);
            else if (textType == TextViewerTextType.UnicodePlainText) {
                if (TextHelper.IsUnicode (stream.GetBuffer (30)))
                    using (var reader = new StreamReader (stream, Encoding.Unicode))
                        innerTextBox.Text = reader.ReadToEnd ();
                else
                    innerTextBox.LoadFile (stream, RichTextBoxStreamType.PlainText);
            }
            innerTextBox.BackColor = color;
            AfterLoadRTF();
        }

        public void Save (Stream stream, TextViewerTextType textType) {
            innerTextBox.SaveFile(stream, (RichTextBoxStreamType)textType);
        }

        public void SetAttribute (XD.TextAttribute a) {

            var rtfHelper = new Limaki.Common.Text.RTF.RTFHelper ();

            var visit = new TextAttributeVisitor {

                FontTextAttribute = attribute => {
                    if (!string.IsNullOrEmpty (attribute.Font.Family))
                        Controller.SetFontFamiliy (attribute.Font.Family);
                    if (attribute.Font.Size > 0)
                        Controller.SetFontSize ((int) attribute.Font.Size);

                },

                FontDataAttribute = attribute => {
                    if (!string.IsNullOrEmpty (attribute.FontFamily))
                        Controller.SetFontFamiliy (attribute.FontFamily);
                    if (attribute.FontSize > 0)
                        Controller.SetFontSize ((int) attribute.FontSize);

                },

                FontWeightTextAttribute = attribute => 
                    Controller.SetEditorSelectedRTF (
                        source => rtfHelper.SetAttributes (source, Convert (attribute.Weight))),

                FontStyleTextAttribute = attribute => 
                    Controller.SetEditorSelectedRTF (
                        source => rtfHelper.SetAttributes (source, Convert (attribute.Style))),

                StrikethroughTextAttribute = attribute => {
                    if (attribute.Strikethrough) {
                        Controller.SetEditorSelectedRTF (
                            source => rtfHelper.SetAttributes (source, Common.Text.RTF.FontStyle.Strikeout));
                    }

                },

                UnderlineTextAttribute = attribute => {
                    if (attribute.Underline) {
                        Controller.SetEditorSelectedRTF (
                            source => rtfHelper.SetAttributes (source, Common.Text.RTF.FontStyle.Underline));
                    }

                },
                BackgroundTextAttribute = attribute => { },
                ColorTextAttribute = attribute =>{}
            };

            visit.Visit (a);
        }

        
        private Common.Text.RTF.FontStyle Convert (XD.FontWeight fontWeight) {
            if (fontWeight == XD.FontWeight.Bold)
                return Common.Text.RTF.FontStyle.Bold;
            return Common.Text.RTF.FontStyle.Normal;
        }

        private Common.Text.RTF.FontStyle Convert (XD.FontStyle fontStyle) {
            if (fontStyle == XD.FontStyle.Italic || fontStyle == XD.FontStyle.Oblique)
                return Common.Text.RTF.FontStyle.Italic;
            return Common.Text.RTF.FontStyle.Normal;
        }

        public IEnumerable<XD.TextAttribute> GetAttributes () {

            var font = this.innerTextBox.SelectionFont;
            if (font != null) {

                yield return new XD.FontDataAttribute {
                    FontFamily = font.FontFamily.Name,
                    FontSize = font.SizeInPoints,
                };

                yield return new XD.FontWeightTextAttribute {
                    Weight = font.Bold ? XD.FontWeight.Bold : XD.FontWeight.Normal
                };

                yield return new XD.FontStyleTextAttribute {
                    Style = font.Italic ? XD.FontStyle.Italic : XD.FontStyle.Normal
                };

                if (font.Underline)
                    yield return new XD.UnderlineTextAttribute { Underline = true };
            }
        }

        public event EventHandler SelectionChanged {
            add { innerTextBox.SelectionChanged += value; }
            remove { innerTextBox.SelectionChanged -= value; }
        }

        #region IZoomTarget Member

        public ZoomState ZoomState {
            get { return ZoomState.Original; }
            set {
                if (value == ZoomState.Original) {
                    this.innerTextBox.ZoomFactor = 1.0f;
                }
            }
        }



        /// <summary>
        /// remark: works only with truetype-fonts, else next integer
        /// </summary>
        [Browsable(true)]
        public virtual double ZoomFactor {
            get { return this.innerTextBox.ZoomFactor; }
            set { this.innerTextBox.ZoomFactor = (float)value; }
        }

        public void UpdateZoom () {
            //base.Window.TextZoom = ZoomFactor;
        }

        #endregion

        private System.Windows.Forms.RichTextBox innerTextBox;
        private void InitializeComponent () {
            this.innerTextBox = new System.Windows.Forms.RichTextBox ();
            Control.SuspendLayout ();
            // 
            // innerTextBox
            // 
            this.innerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.innerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.innerTextBox.Font = new System.Drawing.Font ("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.innerTextBox.Location = new System.Drawing.Point (0, 0);
            this.innerTextBox.Name = "innerTextBox";
            this.innerTextBox.Size = new System.Drawing.Size (354, 227);
            this.innerTextBox.TabIndex = 0;
            this.innerTextBox.Text = "";
            // 
            // TextViewerBackend
            // 
            Control.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
            Control.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Control.Controls.Add (this.innerTextBox);
            Control.Name = "TextViewerBackend";
            Control.Size = new System.Drawing.Size (354, 227);
            Control.ResumeLayout (false);

        }
        
        public new TextViewer Frontend { get; protected set; }

        public override  void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (TextViewer)frontend;
        }

        VidgetBorderStyle ITextViewerVidgetBackend.BorderStyle {
            get { return (VidgetBorderStyle) Control.BorderStyle; }
            set { Control.BorderStyle = (BorderStyle)value; }
        }

        Xwt.Point ITextViewerBackend.AutoScrollOffset {
            get { return Control.AutoScrollOffset.ToXwt(); }
            set { Control.AutoScrollOffset = value.ToGdi(); }
        }


    }
}


