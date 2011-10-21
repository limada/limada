using Limaki.Common;
namespace Limada.Model {
    public class ThingComparer : DataComparer<IThing> {
        protected override object GetData(IThing thing) {
            if (thing is INumberThing)
                return ((INumberThing) thing).Number;
            return thing.Data;
        }
        
    }
}