using System;

namespace Limaki.Drawing.UI {
    public interface ISelectionPainter : IDisposable {
        bool ShowGrips { get; set; }
        int GripSize { get; set; }
        ICamera camera { get; set; }
        IControl control { get; set; }
        IShape Shape { get; set; }
        IStyle Style { get; set; }
        void InvalidateShapeOutline(IShape oldShape, IShape newShape);
        void RemoveSelection();
        void OnPaint(IPaintActionEventArgs e);
        void Clear();
    }
}