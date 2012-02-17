using Limada.Model;
using Limaki.Data;
using Limada.Data;


namespace Limada.Tests.Data.db4o {
    public class ThingGraphTest:Limada.Tests.ThingGraphs.ThingGraphTest {
        public override void Setup() {
            this.ThingGraphProvider = new Db4oThingGraphProvider ();
            base.Setup ();
        }
    }

    public class StreamThingTest : Limada.Tests.ThingGraphs.StreamThingTest {
        public override void Setup() {
            this.ThingGraphProvider = new Db4oThingGraphProvider();
            base.Setup();
        }
    }

    public class ThingGraphDeleteItemsTest : Limada.Tests.ThingGraphs.ThingGraphDeleteItemsTest {
        public override void Setup() {
            this.ThingGraphProvider = new Db4oThingGraphProvider();
            base.Setup();
        }
    }

    public class StreamFacadeTest : Limada.Tests.ThingGraphs.StreamFacadeTest {
        public override void Setup() {
            this.ThingGraphProvider = new Db4oThingGraphProvider();
            base.Setup();
        }
    }

    public class SchemaGraphTest : Limada.Tests.ThingGraphs.SchemaGraph.SchemaGraphTest {
        public override void Setup() {
            this.ThingGraphProvider = new Db4oThingGraphProvider();
            base.Setup();
        }
    }
}

