/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Windows.Controls;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz.Visualizers.ToolStrips;
using System.Linq;
using Limaki.Common.Linqish;

namespace Limaki.View.WpfBackend {

    public class MarkerToolStripBackend : ToolStripBackend, IMarkerToolStripBackend {

        public MarkerToolStrip Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (MarkerToolStrip) frontend;
            Compose ();
        }

        protected override void Compose () {
            base.Compose ();
            MarkerCombo = new ComboBox { Width = 100 };
            MarkerCombo.Items.Clear ();
            this.AddItems (MarkerCombo);
            MarkerCombo.SelectionChanged += (s, e) => Frontend.ChangeMarkers (MarkerCombo.SelectionBoxItem.ToString ());
        }

        protected ComboBox MarkerCombo { get; set; }

        public void Attach (IGraphScene<IVisual, IVisualEdge> scene) {
            MarkerCombo.Items.Clear ();
            MarkerCombo.Text = string.Empty;
            bool makeVisible = scene != null && scene.Markers != null;
            if (makeVisible) {
                scene.Markers.MarkersAsStrings ().ForEach (m => MarkerCombo.Items.Add (m));
            }
            if (!makeVisible)
                this.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void Detach (IGraphScene<IVisual, IVisualEdge> oldScene) {
            MarkerCombo.Items.Clear ();
            MarkerCombo.Text = string.Empty;
            this.Visibility = System.Windows.Visibility.Collapsed;

        }
    }
}