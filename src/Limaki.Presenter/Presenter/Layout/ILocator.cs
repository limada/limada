using Limaki.Drawing;

namespace Limaki.Presenter.Layout {
    public interface ILocator<TItem> {
        PointI GetLocation(TItem item);
        void SetLocation(TItem item, PointI location);

        SizeI GetSize(TItem item);
        void SetSize(TItem item, SizeI value);
    }
}