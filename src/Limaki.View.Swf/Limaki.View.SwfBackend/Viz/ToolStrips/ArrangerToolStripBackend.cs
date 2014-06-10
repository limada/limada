/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
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

    public partial class ArrangerToolStripBackend : ToolStripBackend, IArrangerToolStripBackend {

        public ArrangerToolStripBackend () {
            InitializeComponent();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ArrangerToolStrip Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (ArrangerToolStrip)frontend;
            Compose();
        }

        protected virtual void Compose () {

            var horizontalButton = new ToolStripDropDownButtonBackend0 { Command = Frontend.ArrangeLeftCommand, DisplayStyle = ToolStripItemDisplayStyle.Image };
            horizontalButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemBackend0 { Command = Frontend.ArrangeCenterCommand, ToggleOnClick = horizontalButton, DisplayStyle = ToolStripItemDisplayStyle.Image },
                new ToolStripMenuItemBackend0 { Command = Frontend.ArrangeRightCommand, ToggleOnClick = horizontalButton, DisplayStyle = ToolStripItemDisplayStyle.Image },
            });

            var verticalButton = new ToolStripDropDownButtonBackend0 { Command = Frontend.ArrangeTopCommand, DisplayStyle = ToolStripItemDisplayStyle.Image };
            verticalButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemBackend0 { Command = Frontend.ArrangeCenterVCommand, ToggleOnClick = verticalButton, DisplayStyle = ToolStripItemDisplayStyle.Image },
                new ToolStripMenuItemBackend0 { Command = Frontend.ArrangeBottomCommand, ToggleOnClick = verticalButton, DisplayStyle = ToolStripItemDisplayStyle.Image },
            });

            var layoutButton = new ToolStripDropDownButtonBackend0 { Command = Frontend.LogicalLayoutLeafCommand, DisplayStyle = ToolStripItemDisplayStyle.Image };
            layoutButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemBackend0 { Command = Frontend.LogicalLayoutCommand, ToggleOnClick = layoutButton, DisplayStyle = ToolStripItemDisplayStyle.Image },
                new ToolStripMenuItemBackend0 { Command = Frontend.ColumnsCommand, ToggleOnClick = layoutButton, DisplayStyle = ToolStripItemDisplayStyle.Image },
                new ToolStripMenuItemBackend0 { Command = Frontend.OneColumnCommand, ToggleOnClick = layoutButton , DisplayStyle = ToolStripItemDisplayStyle.Image},
                new ToolStripMenuItemBackend0 { Command = Frontend.FullLayoutCommand , DisplayStyle = ToolStripItemDisplayStyle.Image},
            });

            var dimensionButton = new ToolStripDropDownButtonBackend0 { Command = Frontend.DimensionXCommand, DisplayStyle = ToolStripItemDisplayStyle.Image };
            dimensionButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemBackend0 { Command = Frontend.DimensionYCommand, ToggleOnClick = dimensionButton, DisplayStyle = ToolStripItemDisplayStyle.Image },
            });

            this.Items.AddRange(new ToolStripItem[] {
                layoutButton,
                horizontalButton,
                verticalButton,
                dimensionButton,
                new ToolStripButtonBackend0 { Command = Frontend.UndoCommand},
            });
        }

    }
}