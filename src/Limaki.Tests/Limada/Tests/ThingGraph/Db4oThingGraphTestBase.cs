using Limada.Model;
using Limaki.Data;
using Limaki.Data.db4o;
using ThingGraph = Limada.Data.db4o.ThingGraph;

namespace Limada.Tests.Data.db4o {
    /// <summary>
    /// Tests Limada.Data.db4o.ThingGraph
    /// </summary>
    public class Db4oThingGraphTestBase  {
        public string FileName {
            get {
                return @"E:\testdata\txbProjekt\Limaki\graphtest.limo";
            }
        }

        private Gateway _gateway = null;
        public virtual Gateway Gateway {
            get {
                if (_gateway == null) {
                    _gateway = new Gateway();

                }
                return _gateway;
            }
            set { _gateway = value; }
        }


        public virtual IThingGraph CreateGraph() {
            IThingGraph _graph = null;
            if (!this.Gateway.IsOpen()) {
                _graph = null;
            }
            if (_graph == null) {
                Gateway.Open(DataBaseInfo.FromFileName(FileName));
                _graph = new ThingGraph(Gateway);
            }
            return _graph;
        }

        public virtual void Setup(Limada.Tests.ThingGraphs.ThingGraphTestBase sender) {
            sender.CreateGraph = this.CreateGraph;
            sender.Flush = this.Flush;
            sender.Close = this.Close;
        }
        
        public virtual void TearDown() {
            Gateway.Close();
        }

        public virtual void Close(Limada.Tests.ThingGraphs.ThingGraphTestBase sender) {
            Gateway.Close();
            Gateway = null;
            sender.Graph = null;
        }

        public virtual void Flush(IThingGraph graph) {
            var dbGraph = graph as ThingGraph;
            if (dbGraph != null) {
                dbGraph.Flush ();
            }

        }
    }

    public class ThingGraphTest:Limada.Tests.ThingGraphs.ThingGraphTest {
        public override void Setup() {
            Db4oThingGraphTestBase inner = new Db4oThingGraphTestBase ();
            inner.Setup (this);
            base.Setup ();
        }
    }

    public class StreamThingTest : Limada.Tests.ThingGraphs.StreamThingTest {
        public override void Setup() {
            Db4oThingGraphTestBase inner = new Db4oThingGraphTestBase();
            inner.Setup(this);
            base.Setup();
        }
    }

    public class ThingGraphDeleteItemsTest : Limada.Tests.ThingGraphs.ThingGraphDeleteItemsTest {
        public override void Setup() {
            Db4oThingGraphTestBase inner = new Db4oThingGraphTestBase();
            inner.Setup(this);
            base.Setup();
        }
    }

    public class StreamFacadeTest : Limada.Tests.ThingGraphs.StreamFacadeTest {
        public override void Setup() {
            Db4oThingGraphTestBase inner = new Db4oThingGraphTestBase();
            inner.Setup(this);
            base.Setup();
        }
    }

    public class SchemaGraphTest : Limada.Tests.ThingGraphs.SchemaGraph.SchemaGraphTest {
        public override void Setup() {
            Db4oThingGraphTestBase inner = new Db4oThingGraphTestBase();
            inner.Setup(this);
            base.Setup();
        }
    }
}