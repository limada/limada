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

namespace Limaki.SwfBackend.VidgetBackends {

    public partial class TextOkCancelBoxBackend : UserControl, ITextOkCancelBoxBackend {

        public TextOkCancelBoxBackend() {
            InitializeComponent();
            ActiveControl = this.TextBox;
        }

        public string Title { get { return TitleLabel.Text; } set { TitleLabel.Text = value; } }
        public Action<DialogResult> Finish { get; set; }

        private void buttonOk_Click(object sender, EventArgs e) {
            OnFinish (DialogResult.Ok);
        }

        private void buttonCancel_Click(object sender, EventArgs e) {
            OnFinish(DialogResult.Cancel);
        }

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

        #region IVidgetBackend Member

        public TextOkCancelBox Frontend { get; set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (TextOkCancelBox)frontend;
        }

        Size IVidgetBackend.Size {
            get { return this.Size.ToXwt (); }
        }

        void IVidgetBackend.Update() {
            this.Update();
        }

        void IVidgetBackend.Invalidate() {
            this.Invalidate();
        }

        void IVidgetBackend.Invalidate(Rectangle rect) {
            this.Invalidate(rect.ToGdi ());
        }

        void IVidgetBackend.SetFocus () { this.Focus (); }

        #endregion
    }
}