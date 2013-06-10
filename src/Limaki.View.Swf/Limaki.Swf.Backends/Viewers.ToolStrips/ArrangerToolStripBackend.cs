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
using Limaki.View.Layout;
using Limaki.Viewers.ToolStripViewers;
using Alignment = Xwt.Alignment;
using Dimension = Limaki.Drawing.Dimension;
using Limaki.Viewers;
using Limaki.View;

namespace Limaki.Swf.Backends.Viewers.ToolStrips {

    public partial class ArrangerToolStripBackend : ToolStripBackend, IArrangerToolStripBackend {

        public ArrangerToolStripBackend () {
            InitializeComponent();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ArrangerToolStrip Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, Limaki.View.VidgetApplicationContext context) {
            this.Frontend = (ArrangerToolStrip)frontend;
            Compose();
        }

        protected virtual void Compose () {

            var horizontalButton = new ToolStripDropDownButtonEx { Command = Frontend.ArrangeLeftCommand, DisplayStyle = ToolStripItemDisplayStyle.Image };
            horizontalButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemEx { Command = Frontend.ArrangeCenterCommand, ToggleOnClick = horizontalButton, DisplayStyle = ToolStripItemDisplayStyle.Image },
                new ToolStripMenuItemEx { Command = Frontend.ArrangeRightCommand, ToggleOnClick = horizontalButton, DisplayStyle = ToolStripItemDisplayStyle.Image },
            });

            var verticalButton = new ToolStripDropDownButtonEx { Command = Frontend.ArrangeTopCommand, DisplayStyle = ToolStripItemDisplayStyle.Image };
            verticalButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemEx { Command = Frontend.ArrangeCenterVCommand, ToggleOnClick = verticalButton, DisplayStyle = ToolStripItemDisplayStyle.Image },
                new ToolStripMenuItemEx { Command = Frontend.ArrangeBottomCommand, ToggleOnClick = verticalButton, DisplayStyle = ToolStripItemDisplayStyle.Image },
            });

            var layoutButton = new ToolStripDropDownButtonEx { Command = Frontend.LogicalLayoutLeafCommand, DisplayStyle = ToolStripItemDisplayStyle.Image };
            layoutButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemEx { Command = Frontend.LogicalLayoutCommand, ToggleOnClick = layoutButton, DisplayStyle = ToolStripItemDisplayStyle.Image },
                new ToolStripMenuItemEx { Command = Frontend.ColumnsCommand, ToggleOnClick = layoutButton, DisplayStyle = ToolStripItemDisplayStyle.Image },
                new ToolStripMenuItemEx { Command = Frontend.OneColumnCommand, ToggleOnClick = layoutButton , DisplayStyle = ToolStripItemDisplayStyle.Image},
                new ToolStripMenuItemEx { Command = Frontend.FullLayoutCommand , DisplayStyle = ToolStripItemDisplayStyle.Image},
            });

            var dimensionButton = new ToolStripDropDownButtonEx { Command = Frontend.DimensionXCommand, DisplayStyle = ToolStripItemDisplayStyle.Image };
            dimensionButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemEx { Command = Frontend.DimensionYCommand, ToggleOnClick = dimensionButton, DisplayStyle = ToolStripItemDisplayStyle.Image },
            });

            this.Items.AddRange(new ToolStripItem[] {
                layoutButton,
                horizontalButton,
                verticalButton,
                dimensionButton,
                new ToolStripButtonEx { Command = Frontend.UndoCommand},
            });
        }

    }
}