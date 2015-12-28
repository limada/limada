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
 */


using System;
using System.Windows.Forms;
using Limaki.View;
using Limaki.View.Vidgets;
using Xwt;
using Xwt.GdiBackend;
using DialogResult = Limaki.View.Vidgets.DialogResult;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using SWF = System.Windows.Forms;
using SD = System.Drawing;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public partial class TextOkCancelBoxBackend : VidgetBackend<UserControl>, ITextOkCancelBoxBackend {

        public new TextOkCancelBox Frontend { get; set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (TextOkCancelBox) frontend;
        }
        
        public TextBox TextBox;
        protected SWF.Label TitleLabel ;

        protected override void Compose () {

            base.Compose ();

            Control.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
            Control.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Control.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.Size = new Size (364, 24);

            this.TextBox = new TextBox () {
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                Location = new System.Drawing.Point (0, 0),
                Margin = new Padding (0),
                Size = new System.Drawing.Size (231, 19),
                TabIndex = 5,
            };

            TitleLabel = new System.Windows.Forms.Label {
                BackColor = System.Drawing.SystemColors.Window,
                Dock = System.Windows.Forms.DockStyle.Fill,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                ForeColor = System.Drawing.SystemColors.ControlLight,
                Location = new System.Drawing.Point (0, 0),
                Margin = new System.Windows.Forms.Padding (0),
                Size = new SD.Size (81, 24),
                TabIndex = 0,
                Text = "label1",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
            };

            var panel1 = new System.Windows.Forms.Panel {
                Dock = System.Windows.Forms.DockStyle.Left,
                Location = new System.Drawing.Point (0, 0),
                Size = new System.Drawing.Size (81, 24),
                TabIndex = 6,
            };


            var panel2 = new System.Windows.Forms.Panel {
                BackColor = System.Drawing.SystemColors.Window,
                Dock = System.Windows.Forms.DockStyle.Fill,
                Location = new System.Drawing.Point (81, 0),
                Size = new System.Drawing.Size (283, 24),
                TabIndex = 7,
            };

            var buttonOk = new System.Windows.Forms.Button {
                BackColor = System.Drawing.SystemColors.ButtonFace,
                Dock = System.Windows.Forms.DockStyle.Right,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                ForeColor = System.Drawing.SystemColors.ButtonFace,
                Image = global::Limaki.View.Properties.GdiIconery.OK,
                Location = new System.Drawing.Point (231, 0),
                Size = new System.Drawing.Size (26, 24),
                TabIndex = 3,
                UseVisualStyleBackColor = false,
            };

            var buttonCancel = new System.Windows.Forms.Button {
                BackColor = System.Drawing.SystemColors.ButtonFace,
                DialogResult = System.Windows.Forms.DialogResult.Cancel,
                Dock = System.Windows.Forms.DockStyle.Right,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                ForeColor = System.Drawing.SystemColors.ButtonFace,
                Image = Limaki.View.Properties.GdiIconery.Cancel,
                Location = new System.Drawing.Point (257, 0),
                Margin = new System.Windows.Forms.Padding (0),
                Size = new System.Drawing.Size (26, 24),
                TabIndex = 4,
                UseVisualStyleBackColor = false,
            };


            panel1.SuspendLayout ();
            panel2.SuspendLayout ();
            Control.SuspendLayout ();

            panel1.Controls.Add (TitleLabel);

            panel2.Controls.Add (this.TextBox);
            panel2.Controls.Add (buttonOk);
            panel2.Controls.Add (buttonCancel);

            Control.Controls.Add (panel2);
            Control.Controls.Add (panel1);
            
            panel1.ResumeLayout (false);
            panel2.ResumeLayout (false);
            panel2.PerformLayout ();
            Control.ResumeLayout (false);

            TextBox.KeyDown += (s, e) => this.TextBox_KeyDown (s, e);

            buttonOk.Click += (s,e) => OnFinish (DialogResult.Ok);
            

            buttonCancel.Click += (s,e)=> OnFinish (DialogResult.Cancel);

            Control.ActiveControl = this.TextBox;
        }

        public string Title { get { return TitleLabel.Text; } set { TitleLabel.Text = value; } }

        public Action<DialogResult> Finish { get; set; }


        void OnFinish (DialogResult result) {
            if (Finish != null) {
                Finish (result);
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
                OnFinish(DialogResult.Ok);
            } else if (e.KeyCode == Keys.Escape) {
                OnFinish (DialogResult.Cancel);
            }
        }

        public virtual string Text {
            get { return TextBox.Text; }
            set { TextBox.Text = value; }
        }
        
    }
}