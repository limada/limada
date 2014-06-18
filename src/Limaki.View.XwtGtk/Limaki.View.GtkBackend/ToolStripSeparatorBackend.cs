using Limaki.View.Vidgets;
using System;
using LVV = Limaki.View.Vidgets;

namespace Limaki.View.GtkBackend {

    public class ToolStripSeparatorBackend : ToolStripItemBackend<Gtk.SeparatorToolItem>, IToolStripSeparatorBackend {

        public LVV.ToolStripSeparator Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (LVV.ToolStripSeparator)frontend;
        }

        protected override void Compose () {
            base.Compose ();
        }
    }
}