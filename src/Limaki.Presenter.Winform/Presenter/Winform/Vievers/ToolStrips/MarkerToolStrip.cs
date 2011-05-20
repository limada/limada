/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System.Windows.Forms;
using Limaki.Visuals;
using Limaki.UseCases.Viewers.ToolStrips;
using Limaki.Presenter.Visuals;
using Limaki.Presenter.Visuals.UI;
using System;

namespace Limaki.UseCases.Winform.Viewers.ToolStrips {
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

        public void Attach(Scene scene) {
            markerCombo.Items.Clear();
            markerCombo.Text = string.Empty;
            bool makeVisible = scene != null && scene.Markers != null;
            if (makeVisible) {
                markerCombo.Items.AddRange(scene.Markers.MarkersAsStrings());
            }
            this.Visible = makeVisible;
        }

        private void markerCombo_SelectedIndexChanged(object sender, EventArgs e) {
            Controller.ChangeMarkers(markerCombo.SelectedItem.ToString());
        }
    }
}