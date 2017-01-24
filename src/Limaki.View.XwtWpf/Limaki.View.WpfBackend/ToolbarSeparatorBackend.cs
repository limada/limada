using Limaki.View.Vidgets;
using SWC = System.Windows.Controls;
using LVV = Limaki.View.Vidgets;

namespace Limaki.View.WpfBackend {

    public class ToolbarSeparatorBackend : ToolbarItemBackend<SWC.Separator>, IToolbarSeparatorBackend {

        public new LVV.ToolbarSeparator Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (LVV.ToolbarSeparator)frontend;
        }

        public override void Compose () {
            base.Compose ();
            this.Control.Style = ToolbarUtils.ToolbarItemStyle (this.Control);
        }

        public void SetImage (Xwt.Drawing.Image image) {
            
        }

        public void SetLabel (string value) {
            
        }
    }
}