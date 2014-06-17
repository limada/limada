/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.View.Vidgets;
using System;
using System.Windows;
using LVV = Limaki.View.Vidgets;
using SWC = System.Windows.Controls;

namespace Limaki.View.WpfBackend {

    public class ToolStripItemHostBackend : ToolStripItemBackend<SWC.StackPanel>, IToolStripItemHostBackend {

        public override void InitializeBackend (IVidget frontend, LVV.VidgetApplicationContext context) {

        }

        public override void Compose () {
            base.Compose ();
            
        }

        public void SetChild (Vidget value) {
            Control.Children.Clear ();
            var child = value.Backend.ToWpf ();
            Control.Children.Add (child);
            child.HorizontalAlignment = HorizontalAlignment.Stretch;
            child.VerticalAlignment = VerticalAlignment.Stretch;
            child.Style = ToolStripUtils.ToolbarItemStyle (child);
            var xwtBackend = value.Backend as Limaki.View.XwtBackend.IXwtBackend;
            if (xwtBackend != null) {
                if (xwtBackend.Widget.WidthRequest >= 0) {
                    Control.Width = xwtBackend.Widget.WidthRequest;
                }
                if (xwtBackend.Widget.HeightRequest >= 0) {
                    Control.Height = xwtBackend.Widget.HeightRequest;
                }
            }
            Control.UpdateLayout ();
        }

        public void SetImage (Xwt.Drawing.Image image) {
            
        }

        public void SetLabel (string value) {
            
        }
    }
}