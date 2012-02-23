using Limaki.Common;

namespace Limaki.Visuals {
    public class VisualComparer : DataComparer<IVisual> {
        protected override object GetData(IVisual item) {
            return item.Data;
        }
    }
}