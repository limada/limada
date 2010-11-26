
namespace Limaki.Drawing.UI {
    public interface ISelectionShapePainter:ISelectionPainter {
        IPainter Painter { get; set; }
        RenderType RenderType { get; set; }
    }
}