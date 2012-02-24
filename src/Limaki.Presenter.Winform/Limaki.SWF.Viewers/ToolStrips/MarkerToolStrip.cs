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

using System.Windows.Forms;
using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Viewers.ToolStripViewers;
using Limaki.Visuals;

namespace Limaki.SWF.Viewers.ToolStripViewers {
    public partial class MarkerToolStrip : ToolStrip, IMarkerTool {
        public MarkerToolStrip() {
            InitializeComponent();
        }

        MarkerToolController _controller = null;
        public MarkerToolController Controller {
            get {
                if (_controller == null) {
                    _controller = new MarkerToolController();
                    _controller.Tool = this;
                }
                return _controller;
            }
        }

        public void Attach(IGraphScene<IVisual, IVisualEdge> scene) {
            markerCombo.Items.Clear();
            markerCombo.Text = string.Empty;
            bool makeVisible = scene != null && scene.Markers != null;
            if (makeVisible) {
                markerCombo.Items.AddRange(scene.Markers.MarkersAsStrings());
            }
            this.Visible = makeVisible;
        }

        public void Detach(IGraphScene<IVisual, IVisualEdge> oldScene) {
            markerCombo.Items.Clear();
            markerCombo.Text = string.Empty;
            this.Visible = false;
        }

        private void markerCombo_SelectedIndexChanged(object sender, EventArgs e) {
            Controller.ChangeMarkers(markerCombo.SelectedItem.ToString());
        }

 

        

 
    }
}