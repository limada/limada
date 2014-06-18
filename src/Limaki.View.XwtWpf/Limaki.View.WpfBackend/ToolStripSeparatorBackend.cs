using Limaki.View.Vidgets;
using SWC = System.Windows.Controls;
using LVV = Limaki.View.Vidgets;

namespace Limaki.View.WpfBackend {

    public class ToolStripSeparatorBackend : ToolStripItemBackend<SWC.Separator>, IToolStripSeparatorBackend {

        public LVV.ToolStripSeparator Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (LVV.ToolStripSeparator)frontend;
        }

        public override void Compose () {
            base.Compose ();
            this.Control.Style = ToolStripUtils.ToolbarItemStyle (this.Control);
        }

        public void SetImage (Xwt.Drawing.Image image) {
            
        }

        public void SetLabel (string value) {
            
        }
    }
}