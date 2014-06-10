using System.Windows.Forms;
using Limaki.View.Vidgets;
using Xwt.GdiBackend;
using ToolStripButton = System.Windows.Forms.ToolStripButton;
using SWF = System.Windows.Forms;
using LVV = Limaki.View.Vidgets;
using System.ComponentModel;
using Xwt.Backends;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class ToolStripButtonBackend : ToolStripButton, IToolStripButtonBackend {

        public ToolStripButtonBackend () {
            ImageScaling = ToolStripItemImageScaling.None;
        }

        public new Xwt.Size Size {
            get { return base.Size.ToXwt (); }
            set { base.Size = value.ToGdi (); }
        }

        #region IVidgetBackend Member

        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public LVV.ToolStrip Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (LVV.ToolStrip)frontend;
        }

        void IVidgetBackend.Update () { }

        void IVidgetBackend.Invalidate() {
            this.Invalidate();
        }

        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate (rect.ToGdi ());
        }

        void IVidgetBackend.SetFocus () {  }

        #endregion

        public void SetImage (Xwt.Drawing.Image image) {
            base.Image = image.ToGdi();
        }

        public void SetLabel (string value) {
            base.Text = value;
        }

        public void SetToolTip (string value) {
            base.ToolTipText = value;
        }
    }
}