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
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View;
using Limaki.View.Vidgets;
using Xwt;
using Xwt.Gdi;
using DialogResult=Limaki.View.Vidgets.DialogResult;
using Limaki.Drawing.Gdi;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using Xwt.Gdi.Backend;

namespace Limaki.Swf.Backends {

    public partial class TextOkCancelBoxBackend : UserControl, ITextOkCancelBox,IVidgetBackend {

        public TextOkCancelBoxBackend() {
            InitializeComponent();
            ActiveControl = this.TextBox;
            Result = DialogResult.None;
        }


        public DialogResult Result { get; set; }

        private void buttonOk_Click(object sender, EventArgs e) {
            Text = TextBox.Text;
            DoFinish (DialogResult.Ok);
        }

        private void buttonCancel_Click(object sender, EventArgs e) {
            DoFinish(DialogResult.Cancel);
        }

        public string Title {
            get { return nameLabel.Text; }
            set { nameLabel.Text = value; }
        }


        public Action<string> OnOk { get; set; }
        public event EventHandler<TextOkCancelBoxEventArgs> Finish = null;
        
        void DoFinish(DialogResult result) {
            if (Finish != null) {
                Finish(this, new TextOkCancelBoxEventArgs(result, OnOk));
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
                Text = TextBox.Text;
                DoFinish(DialogResult.Ok);
            } else if (e.KeyCode == Keys.Escape) {
                DoFinish (DialogResult.Cancel);
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


        #endregion
    }
}