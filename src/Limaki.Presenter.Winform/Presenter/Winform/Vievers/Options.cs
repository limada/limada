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
 * 
 */

using System;
using System.Windows.Forms;

namespace Limaki.UseCases.Winform.Viewers {
    public partial class Options : Form {
        public Options() {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e) {
            this.Close ();
        }

        private void applyButton_Click(object sender, EventArgs e) {

        }
    }
}