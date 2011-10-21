

using Limaki.Drawing;

namespace Limaki.Reporting {
    public interface IReport {
        IReportOptions Options { get; }
    }

    public interface IReportOptions {
        RectangleS PageSize { get; set; }
        float MarginLeft { get; set; }
        float MarginRight { get; set; }
        float MarginTop { get; set; }
        float MarginBottom { get; set; }
        bool MarginMirroring { get; set; }
        int Columns { get; set; }
    }
}