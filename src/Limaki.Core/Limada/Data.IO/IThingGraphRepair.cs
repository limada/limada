using Limaki.Data;

namespace Limada.Data {
    public interface IThingGraphRepair {
        IoInfo IoInfo { get; }
    }
    public class ThingGraphRepair : IThingGraphRepair {
        public IoInfo IoInfo { get; set; }
    }
}