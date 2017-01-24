using Limaki.View.Vidgets;
using System;
using LVV = Limaki.View.Vidgets;

namespace Limaki.View.GtkBackend {

    public class ToolbarSeparatorBackend : ToolbarItemBackend<Gtk.SeparatorToolItem>, IToolbarSeparatorBackend {

        public new LVV.ToolbarSeparator Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (LVV.ToolbarSeparator)frontend;
        }

        protected override void Compose () {
            base.Compose ();
        }
    }
}