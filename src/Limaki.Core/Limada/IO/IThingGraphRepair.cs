using Limaki.Data;

namespace Limada.IO {
    public interface IThingGraphRepair {
        Iori Iori { get; }
    }
    public class ThingGraphRepair : IThingGraphRepair {
        public Iori Iori { get; set; }
    }
}