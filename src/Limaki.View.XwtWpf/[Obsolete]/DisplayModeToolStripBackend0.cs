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

using System.ComponentModel;
using SW = System.Windows;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;
using Limaki.View.XwtBackend;
using System;

namespace Limaki.View.WpfBackend {

    [Obsolete]
    public class DisplayModeToolStripBackend0 : ToolStripBackend, IDisplayModeToolStripBackend {

        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public DisplayModeToolStrip0 Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (DisplayModeToolStrip0) frontend;
            Compose ();
        }

        private void Compose () {

            var selectButton = new ToolStripDropDownButton0 { Command = Frontend.SelectCommand };
            selectButton.AddItems (
                new ToolStripButton0 { Command = Frontend.PanningCommand, ToggleOnClick = selectButton }
            );

            var zoomButton = new ToolStripDropDownButton0 { Command = Frontend.ZoomInOutCommand };
            zoomButton.AddItems (
                new ToolStripButton0 { Command = Frontend.FitToScreenCommand },
                new ToolStripButton0 { Command = Frontend.FitToWidthCommand },
                new ToolStripButton0 { Command = Frontend.FitToHeigthCommand },
                new ToolStripButton0 { Command = Frontend.OriginalSizeCommand }
            );

            zoomButton.PreviewMouseDown += (s, e) => {
                var args = WpfConverter.ToXwtButtonArgs (s as SW.FrameworkElement, e);
                Frontend.ZoomInOut (args.ToLmk());
            };

            this.AddItems (
               selectButton,
               zoomButton
            );
        }

    }
}