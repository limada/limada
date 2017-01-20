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
using Limaki.View.Viz.Visualizers.ToolStrips;
using Limaki.Common.Linqish;

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
                Trace.WriteLine ($"{nameof (ToolbarPanelBackend)}.{nameof (Widget.SizeAllocated)}:{e.Allocation}");
                Widget.Children.ForEach (c => {
                    //TODO
                });
            };
            Widget.SizeRequested += (s, e) => {
                Trace.WriteLine ($"{nameof (ToolbarPanelBackend)}.{nameof (Widget.SizeRequested)}:{new Xwt.Size (e.Requisition.Width, e.Requisition.Height)}");
                Widget.Children.ForEach (c => {
                    //TODO
                });
            };
        }

        Action<object, Gtk.SizeRequestedArgs> toolbarSizeRequested = (s, e) => {
            //TODO
            Trace.WriteLine ($"{nameof (toolbarSizeRequested)}:{new Xwt.Size (e.Requisition.Width, e.Requisition.Height)}");
        };

        public void InsertItem (int index, IToolStripBackend item) {
            
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

        public void RemoveItem (IToolStripBackend item) {
            Widget.Remove (item.ToGtk ());
        }

        public void SetVisibility (Visibility value) {
            if (value == Visibility.Visible && !Widget.Visible)
                Widget.Visible = true;
            else
                Widget.Visible = false;
        }
    }
}