/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 */


using System;
using System.Windows.Forms;
using Limaki.Common;

namespace Limaki.Winform.Controls {
    public partial class TextOkCancelBox : UserControl {
        public TextOkCancelBox() {
            InitializeComponent();
            ActiveControl = this.TextBox;
        }

        
        public DialogResult Result = DialogResult.None;
        private void buttonOk_Click(object sender, EventArgs e) {
            Text = TextBox.Text;
            DoFinish (DialogResult.OK);
        }

        private void buttonCancel_Click(object sender, EventArgs e) {
            DoFinish(DialogResult.Cancel);
        }

        public string Title {
            get { return nameLabel.Text; }
            set { nameLabel.Text = value; }
        }



        public event EventHandler<EventArgs<DialogResult>> Finish = null;
        void DoFinish(DialogResult result) {
            Result = result;
            if (Finish != null) {
                Finish (this, new EventArgs<DialogResult> (Result));
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
                Text = TextBox.Text;
                DoFinish(DialogResult.OK);
            } else if (e.KeyCode == Keys.Escape) {
                DoFinish (DialogResult.Cancel);
            }
        }
    }
}