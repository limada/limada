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

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Limaki.View.Layout;
using Limaki.Viewers.ToolStripViewers;
using Alignment = Xwt.Alignment;
using Dimension = Limaki.Drawing.Dimension;
using Limaki.Viewers;

namespace Limaki.Swf.Backends.Viewers.ToolStrips {

    public partial class ArrangerToolStripBackend : ToolStrip, IToolStripViewerBackend {
        private ArrangerToolStrip _frontend = null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ArrangerToolStrip Frontend { get { return _frontend ?? (_frontend = new ArrangerToolStrip { Backend = this }); } }

        public ArrangerToolStripBackend () {
            InitializeComponent();
            Compose();
            Frontend.Backend = this;
        }

        protected virtual void Compose () {

            var horizontalButton = new ToolStripDropDownButtonEx { Command = Frontend.ArrangeLeftCommand };
            horizontalButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemEx { Command = Frontend.ArrangeCenterCommand, ToggleOnClick = horizontalButton },
                new ToolStripMenuItemEx { Command = Frontend.ArrangeRightCommand, ToggleOnClick = horizontalButton },
            });

            var verticalButton = new ToolStripDropDownButtonEx { Command = Frontend.ArrangeTopCommand };
            verticalButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemEx { Command = Frontend.ArrangeCenterVCommand, ToggleOnClick = verticalButton },
                new ToolStripMenuItemEx { Command = Frontend.ArrangeBottomCommand, ToggleOnClick = verticalButton },
            });

            var layoutButton = new ToolStripDropDownButtonEx { Command = Frontend.LogicalLayoutLeafCommand };
            layoutButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemEx { Command = Frontend.LogicalLayoutCommand, ToggleOnClick = layoutButton },
                new ToolStripMenuItemEx { Command = Frontend.ColumnsCommand, ToggleOnClick = layoutButton },
                new ToolStripMenuItemEx { Command = Frontend.OneColumnCommand, ToggleOnClick = layoutButton },
                new ToolStripMenuItemEx { Command = Frontend.FullLayoutCommand },
            });

            var dimensionButton = new ToolStripDropDownButtonEx { Command = Frontend.DimensionXCommand };
            dimensionButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemEx { Command = Frontend.DimensionYCommand, ToggleOnClick = dimensionButton },
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