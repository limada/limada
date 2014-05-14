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

            var horizontalButton = new ToolStripButton { Command = Frontend.ArrangeLeftCommand };

            this.AddItems (horizontalButton);
        }
    }
}