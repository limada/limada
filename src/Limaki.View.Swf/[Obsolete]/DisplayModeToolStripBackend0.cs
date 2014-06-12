/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */


using System.ComponentModel;
using System.Windows.Forms;
using Limaki.View;
using Limaki.View.SwfBackend.VidgetBackends;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;
using ToolStrip = System.Windows.Forms.ToolStrip;
using ToolStripItem = System.Windows.Forms.ToolStripItem;

namespace Limaki.View.SwfBackend.Viz.ToolStrips {



    public partial class DisplayModeToolStripBackend0 : ToolStripBackend, IDisplayModeToolStripBackend {
        public DisplayModeToolStripBackend0 () {
            InitializeComponent();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DisplayModeToolStrip0 Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (DisplayModeToolStrip0)frontend;
            Compose();
        }

        private void Compose () {

            var selectButton = new ToolStripDropDownButtonBackend0 { Command = Frontend.SelectCommand, DisplayStyle = ToolStripItemDisplayStyle.Image };
            selectButton.DropDownItems.AddRange(new ToolStripItem[] { 
                new ToolStripMenuItemBackend0 { Command = Frontend.PanningCommand,ToggleOnClick = selectButton, DisplayStyle = ToolStripItemDisplayStyle.Image},
            });
            var zoomButton = new ToolStripDropDownButtonBackend0 { Command = Frontend.ZoomInOutCommand, DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            zoomButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemBackend0 { Command = Frontend.FitToScreenCommand, DisplayStyle=ToolStripItemDisplayStyle.Text },
                new ToolStripMenuItemBackend0 { Command = Frontend.FitToWidthCommand, DisplayStyle=ToolStripItemDisplayStyle.Text},
                new ToolStripMenuItemBackend0 { Command = Frontend.FitToHeigthCommand, DisplayStyle=ToolStripItemDisplayStyle.Text},
                new ToolStripMenuItemBackend0 { Command = Frontend.OriginalSizeCommand, DisplayStyle=ToolStripItemDisplayStyle.Text},
            });

            zoomButton.MouseDown += (s, e) => Frontend.ZoomInOut(Converter.Convert(e));
            this.Items.AddRange(new ToolStripItem[] {
               selectButton,
               zoomButton,
            });
        }
    }
}