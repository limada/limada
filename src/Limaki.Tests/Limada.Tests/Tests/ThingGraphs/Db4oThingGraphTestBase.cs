using Limada.Model;
using Limaki.Data;
using Limada.IO;


namespace Limada.Tests.Data.db4o {

    public class ThingGraphTest:Limada.Tests.ThingGraphs.ThingGraphTest {

        public override void Setup() {
            this.ThingGraphIo = new Db4oThingGraphIo();
            base.Setup ();
        }
    }

    public class StreamThingTest : Limada.Tests.ThingGraphs.StreamThingTest {
        public override void Setup() {
            this.ThingGraphIo = new Db4oThingGraphIo();
            base.Setup();
        }
    }

    public class ThingGraphDeleteItemsTest : Limada.Tests.ThingGraphs.ThingGraphDeleteItemsTest {
        public override void Setup() {
            this.ThingGraphIo = new Db4oThingGraphIo();
            base.Setup();
        }
    }

    public class ThingContentFacadeTest : Limada.Tests.ThingGraphs.ThingContentFacadeTest {
        public override void Setup() {
            this.ThingGraphIo = new Db4oThingGraphIo();
            base.Setup();
        }
    }

    public class SchemaGraphTest : Limada.Tests.ThingGraphs.SchemaGraph.SchemaGraphTest {
        public override void Setup() {
            this.ThingGraphIo = new Db4oThingGraphIo();
            base.Setup();
        }
    }
}

