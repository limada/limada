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

using System.Windows.Controls;
using Limaki.View.Vidgets;
using Limaki.View.XwtBackend;

namespace Limaki.View.WpfBackend {

    public class TextViewerWithToolStripWidgetBackend : Xwt.WPFBackend.WidgetBackend, ITextViewerWithToolstripWidgetBackend {

        protected DockPanel Box { get; set; }

        public TextViewerWithToolStripWidgetBackend () : this (new DockPanel ()) { }

        internal TextViewerWithToolStripWidgetBackend (DockPanel box) {
            this.Widget = box;
            this.Box = box;
            //box.Orientation = Orientation.Vertical;
            box.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            box.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
        }

        public void SetTextViewer (TextViewer viewer) {
            var backend = viewer.Backend.ToWpf () as RichTextBox;
            if (backend != null) {
                this.Box.Children.Add (new ScrollViewer () { Content = backend });
                this.Box.UpdateLayout ();
            }
        }

        public void SetToolStrip (TextViewerToolStrip toolstrip) {
            var backend = toolstrip.Backend.ToWpf () as ToolBar;
            if (backend != null) {
                var toolBarTray = new ToolBarTray ();
                toolBarTray.Orientation = System.Windows.Controls.Orientation.Horizontal;
                toolBarTray.ToolBars.Add (backend);
                DockPanel.SetDock (toolBarTray, Dock.Top);
                this.Box.Children.Add (toolBarTray);
                this.Box.UpdateLayout ();
            }
        }
    }
}