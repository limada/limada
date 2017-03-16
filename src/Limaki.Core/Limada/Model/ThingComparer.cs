using Limaki.Common;

namespace Limada.Model {
    
    public class ThingComparer : DataComparer<IThing> {
        
        protected override object GetData(IThing item) {
            if (item is INumberThing)
                return ((INumberThing) item).Number;
            return item.Data;
        }
        
    }
}