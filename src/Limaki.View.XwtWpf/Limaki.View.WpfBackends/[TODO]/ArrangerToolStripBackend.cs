using Limaki.Viewers.ToolStripViewers;
using System.ComponentModel;
using System.Windows.Controls;

namespace Limaki.View.WpfBackends {

    public class ArrangerToolStripBackend : ToolStripBackend, IArrangerToolStripBackend {
        public ArrangerToolStripBackend () {
           
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ArrangerToolStrip Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, Limaki.View.VidgetApplicationContext context) {
            this.Frontend = (ArrangerToolStrip)frontend;
            Compose();
        }

        protected override void Compose () {
            base.Compose();
            this.Items.Add
            (new Button { Width = 20, Height = 20, HorizontalAlignment = System.Windows.HorizontalAlignment.Left, Content = "!" });
        }

      
    }
}