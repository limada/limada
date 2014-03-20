using Limaki.Common;

namespace Limaki.View.Visuals {
    public class VisualComparer : DataComparer<IVisual> {
        protected override object GetData(IVisual item) {
            return item.Data;
        }
    }
}