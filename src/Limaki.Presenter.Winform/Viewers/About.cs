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
using System.Diagnostics;
using System.Windows.Forms;

namespace Limaki.UseCases.Winform.Viewers {
    public partial class About : Form {
        public About() {
            InitializeComponent();
            this.linkLabel1.Links[0].LinkData = "http://limada.sourceforge.net/";
        }

        private void About_Click(object sender, EventArgs e) {
            this.Close ();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            // Determine which link was clicked within the LinkLabel.
            this.linkLabel1.Links[linkLabel1.Links.IndexOf(e.Link)].Visited = true;

            // Display the appropriate link based on the value of the 
            // LinkData property of the Link object.
            string target = e.Link.LinkData as string;

            // navigate to it.
            if (null != target) {
                Process.Start(target);
            }

        }
    }
}