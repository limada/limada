/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.View;
using Limaki.View.SwfBackend.VidgetBackends;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz.Visualizers.ToolStrips;

namespace Limaki.View.SwfBackend.Viz.ToolStrips {

    public partial class MarkerToolStripBackend : ToolStripBackend, IMarkerToolStripBackend {
        public MarkerToolStripBackend () {
            InitializeComponent();
        }

        public MarkerToolStrip Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (MarkerToolStrip)frontend;
        }

        public void Attach (IGraphScene<IVisual, IVisualEdge> scene) {
            markerCombo.Items.Clear();
            markerCombo.Text = string.Empty;
            bool makeVisible = scene != null && scene.Markers != null;
            if (makeVisible) {
                markerCombo.Items.AddRange(scene.Markers.MarkersAsStrings());
            }
            this.Visible = makeVisible;
        }

        public void Detach (IGraphScene<IVisual, IVisualEdge> oldScene) {
            markerCombo.Items.Clear();
            markerCombo.Text = string.Empty;
            this.Visible = false;
        }

        private void markerCombo_SelectedIndexChanged (object sender, EventArgs e) {
            Frontend.ChangeMarkers(markerCombo.SelectedItem.ToString());
        }


    }
}