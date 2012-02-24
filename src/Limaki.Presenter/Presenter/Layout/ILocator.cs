using Limaki.Drawing;
using Xwt;

namespace Limaki.Presenter.Layout {
    public interface ILocator<TItem> {
        Point GetLocation(TItem item);
        void SetLocation(TItem item, Point location);

        Size GetSize(TItem item);
        void SetSize(TItem item, Size value);
    }
}