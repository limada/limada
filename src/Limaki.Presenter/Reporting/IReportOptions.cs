



using Limaki.Drawing;
using Xwt;

namespace Limaki.Reporting {
    public interface IReport {
        IReportOptions Options { get; }
    }

    public interface IReportOptions {
        RectangleD PageSize { get; set; }
        double MarginLeft { get; set; }
        double MarginRight { get; set; }
        double MarginTop { get; set; }
        double MarginBottom { get; set; }
        bool MarginMirroring { get; set; }
        int Columns { get; set; }
    }
}