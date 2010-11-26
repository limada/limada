using Limada.Model;
using Limaki.Data;
using Limaki.Data.db4o;
using Limaki.Tests;
using ThingGraph=Limada.Data.db4o.ThingGraph;

namespace Limada.Tests.Data.db4o {
    /// <summary>
    /// Tests Limada.Data.db4o.ThingGraph
    /// </summary>
    public class ThingGraphTestBase : DomainTest {
        public string FileName;

        private Gateway _gateway = null;
        public Gateway Gateway {
            get {
                if (_gateway == null) {
                    _gateway = new Gateway();

                }
                return _gateway;
            }
            set { _gateway = value; }
        }

        protected IThingGraph _graph = null;
        public virtual IThingGraph Graph {
            get {
                if (!this.Gateway.IsOpen()) {
                    _graph = null;
                }
                if (_graph == null) {
                    Gateway.Open(DataBaseInfo.FromFileName(FileName));
                    _graph = new ThingGraph(Gateway);
                }
                return _graph;
            }
            set { this._graph = value; }
        }

        public override void Setup() {
            base.Setup();
            FileName = @"E:\testdata\txbProjekt\Limaki\graphtest.limo";
        }

        public override void TearDown() {
            Gateway.Close ();
            base.TearDown();
        }

        public virtual void Close() {
            Gateway.Close();
            Graph = null;
            Gateway = null;
        }
    }
}