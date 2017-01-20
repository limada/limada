using SWF=System.Windows.Forms;
using LVV = Limaki.View.Vidgets;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public static class BackendExtensions {

        public static SWF.Control ToSwf (this IVidget item) {
            return item.Backend.ToSwf ();
        }

        public static SWF.Control ToSwf (this IVidgetBackend item) {
            var swfBackend = item as ISwfBackend;
            if (swfBackend != null)
                return swfBackend.Control;
            return item as SWF.Control;
        }

        public static SWF.ToolStripItem ToSwf (this LVV.IToolStripItemBackend backend) {
            var swfBackend = backend as ISwfToolStripItemBackend;
            if (swfBackend != null)
                return swfBackend.Control;
            return backend as SWF.ToolStripItem;
        }

        public static bool? IsChecked (this SWF.ToolStripButton control ) {
            if (control.CheckState == SWF.CheckState.Checked)
                return true;
            if (control.CheckState == SWF.CheckState.Unchecked)
                return false;
            return null;
        }

        public static void IsChecked (this SWF.ToolStripButton control, bool? value) {
            if (value.HasValue) {
                if (value.Value)
                    control.CheckState = SWF.CheckState.Checked;
                else
                    control.CheckState = SWF.CheckState.Unchecked;
            } else
                control.CheckState = SWF.CheckState.Indeterminate;
        }
    }
}