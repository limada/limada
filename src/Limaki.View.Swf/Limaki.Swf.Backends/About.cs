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

namespace Limaki.Swf.Backends.Viewers {

    public partial class About : Form {
        public About() {
            InitializeComponent();
            this.linkLabel1.Links[0].LinkData = "http://www.limada.org/";
            this.label2.Click += About_Click;
            var version =  Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(4);
            this.label2.Text =
                "Version: "+version+"\r\n"
                +"\r\nCredits:\r\n"
                + "Storage: db4o object database http://www.db4o.com \r\n"
                + "Graphics abstraction layer: http://github.com/mono/xwt \r\n"
                + "Icons: http://fortawesome.github.com/Font-Awesome \r\n"
               
                ;
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