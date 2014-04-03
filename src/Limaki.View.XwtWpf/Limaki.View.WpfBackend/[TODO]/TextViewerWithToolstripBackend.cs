using System.Windows.Controls;
using Limaki.View.Vidgets;

namespace Limaki.View.WpfBackend {

    public class TextViewerWithToolstripBackend : TextViewerBackend, ITextViewerWithToolstripBackend {

        public bool ToolStripVisible { get; set; }


    }
}