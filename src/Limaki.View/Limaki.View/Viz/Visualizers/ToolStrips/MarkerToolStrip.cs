/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2014 Lytico
 *
 * http://www.limada.org
 * 
 */


using Limaki.Common.Linqish;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Xwt.Backends;

namespace Limaki.View.Viz.Visualizers.ToolStrips {

    [BackendType (typeof (IMarkerToolStripBackend))]
    public class MarkerToolStrip : DisplayToolStrip<IGraphSceneDisplay<IVisual, IVisualEdge>> {

        public MarkerToolStrip () {
            Compose ();
        }

        protected virtual void Compose () {
            MarkerCombo = new ComboBox { Width = 100 };
            MarkerComboHost = new ToolStripItemHost { Child = MarkerCombo };
            this.AddItems (MarkerComboHost, new ToolStripSeparator());
            MarkerCombo.SelectionChanged += (s, e) => ChangeMarkers (MarkerCombo.SelectedItem);
        }

        protected ComboBox MarkerCombo { get; set; }
        protected ToolStripItemHost MarkerComboHost { get; set; }

        public override void Attach (object sender) {
            var display = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
            if (display != null) {
                this.CurrentDisplay = display;
                Attach (display.Data);
            }
        }

        protected void Attach (IGraphScene<IVisual, IVisualEdge> scene) {
            MarkerCombo.Items.Clear ();
            MarkerCombo.SelectedItem = null;
            bool makeVisible = scene != null && scene.Markers != null;
            if (makeVisible) {
                scene.Markers.MarkersAsStrings ().ForEach (m => MarkerCombo.Items.Add (m));
                if (scene.Markers.DefaultMarker != null)
                    MarkerCombo.SelectedItem = scene.Markers.DefaultMarker.ToString ();
            }
            if (!makeVisible)
                this.Visibility = Visibility.Collapsed;
        }

        public override void Detach (object sender) {
            this.CurrentDisplay = null;
        }

        protected void Detach (IGraphScene<IVisual, IVisualEdge> oldScene) {
            MarkerCombo.Items.Clear ();
            MarkerCombo.SelectedItem = null;
            this.Visibility = Visibility.Collapsed;

        }

        protected virtual void ChangeMarkers (object marker) {
            var display = CurrentDisplay;
            if (display != null) {
                var scene = display.Data;
                if (scene.Markers != null) {
                    SceneExtensions.ChangeMarkers (scene, scene.Selected.Elements, marker);
                }
                display.Perform ();
            }
        }


    }

    public interface IMarkerToolStripBackend : IDisplayToolStripBackend {
    }
}