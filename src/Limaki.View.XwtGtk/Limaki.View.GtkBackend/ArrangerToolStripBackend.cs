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

using System.ComponentModel;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;

namespace Limaki.View.GtkBackend {

    public class ArrangerToolStripBackend : ToolStripBackend, IArrangerToolStripBackend {

        public ArrangerToolStripBackend () { }

        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public ArrangerToolStrip Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (ArrangerToolStrip) frontend;
            Compose ();
        }

        protected override void Compose () {

            base.Compose ();

            var horizontalButton = new ToolStripDropDownButton { Command = Frontend.ArrangeLeftCommand };
            horizontalButton.AddItems (
                new ToolStripButton { Command = Frontend.ArrangeCenterCommand, ToggleOnClick = horizontalButton },
                new ToolStripButton { Command = Frontend.ArrangeRightCommand, ToggleOnClick = horizontalButton }
                );

            var verticalButton = new ToolStripDropDownButton { Command = Frontend.ArrangeTopCommand };
            verticalButton.AddItems (
                new ToolStripButton { Command = Frontend.ArrangeCenterVCommand, ToggleOnClick = verticalButton },
                new ToolStripButton { Command = Frontend.ArrangeBottomCommand, ToggleOnClick = verticalButton }
                );

            var layoutButton = new ToolStripDropDownButton { Command = Frontend.LogicalLayoutLeafCommand };
            layoutButton.AddItems (
                new ToolStripButton { Command = Frontend.LogicalLayoutCommand, ToggleOnClick = layoutButton },
                new ToolStripButton { Command = Frontend.ColumnsCommand, ToggleOnClick = layoutButton },
                new ToolStripButton { Command = Frontend.OneColumnCommand, ToggleOnClick = layoutButton },
                new ToolStripButton { Command = Frontend.FullLayoutCommand }
                );

            var dimensionButton = new ToolStripDropDownButton { Command = Frontend.DimensionXCommand };
            dimensionButton.AddItems (
                new ToolStripButton { Command = Frontend.DimensionYCommand, ToggleOnClick = dimensionButton }
                );

            this.AddItems (
                layoutButton,
                horizontalButton,
                verticalButton,
                dimensionButton,
                new ToolStripButton { Command = Frontend.UndoCommand }
                );

        }
    }
}