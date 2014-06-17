using System.Windows.Forms;
using SWF=System.Windows.Forms;
using LVV = Limaki.View.Vidgets;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public static class BackendExtensions {

        public static Control ToSwf (this IVidget item) {
            var control = item.Backend as Control;
            var swfBackend = item.Backend as ISwfBackend;
            if (swfBackend != null)
                control = swfBackend.Control;
            return control;
        }

        public static SWF.ToolStripItem ToSwf (this LVV.IToolStripItemBackend backend) {
            var control = backend as SWF.ToolStripItem;
            var swfBackend = backend as ISwfToolStripItemBackend;
            if (swfBackend != null)
                control = swfBackend.Control;
            return control;
        }
    }
}