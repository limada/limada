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
using System.Windows;
using System.Windows.Controls;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;
using System;

namespace Limaki.View.WpfBackend {

    [Obsolete]
    public class ArrangerToolStripBackend0 : ToolStripBackend, IArrangerToolStripBackend {

        public ArrangerToolStripBackend0 () { }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ArrangerToolStrip0 Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (ArrangerToolStrip0)frontend;
            Compose();
        }

        protected override void Compose () {
            
            base.Compose();

            var horizontalButton = new ToolStripDropDownButton0 {Command = Frontend.ArrangeLeftCommand};
            horizontalButton.AddItems (
                new ToolStripButton0 {Command = Frontend.ArrangeCenterCommand, ToggleOnClick = horizontalButton},
                new ToolStripButton0 {Command = Frontend.ArrangeRightCommand, ToggleOnClick = horizontalButton}
                );

            var verticalButton = new ToolStripDropDownButton0 {Command = Frontend.ArrangeTopCommand};
            verticalButton.AddItems (
                new ToolStripButton0 {Command = Frontend.ArrangeCenterVCommand, ToggleOnClick = verticalButton},
                new ToolStripButton0 {Command = Frontend.ArrangeBottomCommand, ToggleOnClick = verticalButton}
                );

            var layoutButton = new ToolStripDropDownButton0 {Command = Frontend.LogicalLayoutLeafCommand};
            layoutButton.AddItems (
                new ToolStripButton0 {Command = Frontend.LogicalLayoutCommand, ToggleOnClick = layoutButton},
                new ToolStripButton0 {Command = Frontend.ColumnsCommand, ToggleOnClick = layoutButton},
                new ToolStripButton0 {Command = Frontend.OneColumnCommand, ToggleOnClick = layoutButton},
                new ToolStripButton0 {Command = Frontend.FullLayoutCommand}
                );

            var dimensionButton = new ToolStripDropDownButton0 {Command = Frontend.DimensionXCommand};
            dimensionButton.AddItems (
                new ToolStripButton0 {Command = Frontend.DimensionYCommand, ToggleOnClick = dimensionButton}
                );

            this.AddItems (
                layoutButton,
                horizontalButton,
                verticalButton,
                dimensionButton,
                new ToolStripButton0 {Command = Frontend.UndoCommand}
                );
        }

    }
}