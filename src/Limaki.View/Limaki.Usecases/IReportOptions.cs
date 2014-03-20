using Xwt;

namespace Limaki.Usecases {

    public interface IReport {
        IReportOptions Options { get; }
    }

    public interface IReportOptions {
        Rectangle PageSize { get; set; }
        double MarginLeft { get; set; }
        double MarginRight { get; set; }
        double MarginTop { get; set; }
        double MarginBottom { get; set; }
        bool MarginMirroring { get; set; }
        int Columns { get; set; }
    }
}