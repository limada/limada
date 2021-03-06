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
 * 
 */

using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Usecases;

namespace Limaki.View.SwfBackend.Controls {

    public partial class AboutForm : Form {

        About _about = null;
        public virtual About About { get { return _about ?? (_about = Registry.Pooled<About> ()); } }

        public AboutForm() {
            InitializeComponent();
            this.linkLabel1.Links[0].LinkData = About.Link;
            this.label2.Click += About_Click;

            this.label2.Text = About.ToString ();
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