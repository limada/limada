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

using System;
using System.Diagnostics;
using Limaki.View.Vidgets;
using Limaki.Common.Linqish;
using Limaki.View.Common;
using Xwt.Backends;

namespace Limaki.View.GtkBackend {

    public class ToolbarPanelBackend : VidgetBackend<Gtk.HBox>, IToolbarPanelBackend {

        public VidgetApplicationContext ApplicationContext { get; set; }

        public new ToolbarPanel Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            ApplicationContext = context;
            this.Frontend = (ToolbarPanel)frontend;
        }

        protected override void Compose () {
            
            base.Compose ();

            Widget.Spacing = 2;

            Widget.SizeAllocated += (s, e) => {
                Trace.WriteLine ($"{nameof (ToolbarPanelBackend)}.{nameof (Widget.SizeAllocated)}:\t{e.Allocation}");
                Widget.Children.ForEach (c => {
                    //TODO
                });
            };
            Widget.SizeRequested += (s, e) => {
                Trace.WriteLine ($"{nameof (ToolbarPanelBackend)}.{nameof (Widget.SizeRequested)}:\t{new Xwt.Size (e.Requisition.Width, e.Requisition.Height)}");
                Widget.Children.ForEach (c => {
                    //TODO
                });
            };
        }

        Action<object, Gtk.SizeRequestedArgs> toolbarSizeRequested = (s, e) => {
            //TODO
            Trace.WriteLine ($"{nameof (toolbarSizeRequested)}:\t{new Xwt.Size (e.Requisition.Width, e.Requisition.Height)}");
        };

        public void InsertItem (int index, IToolbarBackend item) {
            
            var toolStrip = item.ToGtk() as Gtk.Toolbar;
            toolStrip.ShowArrow = true;
            //toolStrip.ResizeMode = Gtk.ResizeMode.Queue; //deprecated

            //toolStrip.CheckResize ();
            //toolStrip.ShowAll ();
            var w = 0;
            toolStrip.Children.ForEach (c => {
                var r = c.SizeRequest ();
                w += r.Width;
            });

            // this depends on something strange; 
            // on cinnamon 8 is needed
            // otherwise 5
            toolStrip.WidthRequest = w + 8;
            toolStrip.SizeRequested += (s, e) => {
                toolbarSizeRequested (s, e);
            };
            Widget.PackStart (toolStrip, false, false, 0);
        }

        public void RemoveItem (IToolbarBackend item) {
            Widget.Remove (item.ToGtk ());
        }

        public void SetVisibility (Visibility value) {
            if (value == Visibility.Visible && !Widget.Visible)
                Widget.Visible = true;
            else
                Widget.Visible = false;
        }

        public void AddToWindow (IVindow vindow) {
            var xwtWindow = vindow.Backend as Xwt.Window;
            var windowBackend = xwtWindow.GetBackend () as Xwt.GtkBackend.WindowBackend;
            var toolbarBackend = this.ToGtk ();
            var mainBox = windowBackend.MainBox;
            mainBox.PackStart (toolbarBackend, false, false, 0);

            ((Gtk.Box.BoxChild)mainBox[toolbarBackend]).Position = 1;
            mainBox.ShowAll ();
        }

    }
}