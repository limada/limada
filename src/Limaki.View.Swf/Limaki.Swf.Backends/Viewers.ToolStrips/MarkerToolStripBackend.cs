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
using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Viewers.ToolStripViewers;
using Limaki.Visuals;

namespace Limaki.Swf.Backends.Viewers.ToolStrips {

    public partial class MarkerToolStripBackend : ToolStrip, IMarkerToolStripBackend {
        public MarkerToolStripBackend () {
            InitializeComponent();
        }

        MarkerToolStrip _frontend = null;
        public MarkerToolStrip Frontend {
            get {
                if (_frontend == null) {
                    _frontend = new MarkerToolStrip();
                    _frontend.Backend = this;
                }
                return _frontend;
            }
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