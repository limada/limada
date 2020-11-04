using Limaki.Common;
using Limaki.Repository;

namespace Limaki.LinqData.Tests {

    public enum Orms {
        Ef6,
        NH,
        Linq2Db
    }

    public static class DbQuoreTestData {

        public static Orms Orm = Orms.Ef6;

        public static Iori Iori => MsSqlIori;

        public static Iori MsSqlIori =>
            new Iori {
                Provider = "MsSqlServer",
                Name = "LinqDbThingContextTests",
                Server = "file",
                Path = @"e:\temp", // must be an uncompressed path!!
            };

        public static Iori InMemIori =>
            new Iori {
                Provider = "InMemoryProvider",
            };

        public static void TearUp () {
            new DbQuoreResourceLoader ().ApplyResources (Registry.ConcreteContext);
            if (Orm == Orms.Linq2Db) {
                new LinqToDBLimadaResourceLoader ().ApplyResources (Registry.ConcreteContext);
            }
            // if (Orm == Orms.Ef6) {
            //     new Ef6ResourceLoader ().ApplyResources (Registry.ConcreteContext);
            //     new Ef6LimadaResourceLoader ().ApplyResources (Registry.ConcreteContext);
            // }
            //
            // if (Orm == Orms.NH) {
            //     new NhResourceLoader ().ApplyResources (Registry.ConcreteContext);
            //     new NhLimadaResourceLoader ().ApplyResources (Registry.ConcreteContext);
            // }
        }
    }

}