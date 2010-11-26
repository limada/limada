using Limada.Model;
using Limaki.Tests;
using System;

namespace Limada.Tests.ThingGraphs {
    public class ThingGraphTestBase : DomainTest {
        protected IThingGraph _graph = null;
        public virtual IThingGraph Graph {
            get {
                if (_graph == null) {
                    _graph = OnCreateGraph();
                }
                return _graph;
            }
            set { this._graph = value; }
        }

        public Func<IThingGraph> CreateGraph = null;
        public virtual IThingGraph OnCreateGraph() {
            if (CreateGraph != null) {
                return CreateGraph ();
            }
            return new ThingGraph ();
        }

        public Action<ThingGraphTestBase> Close = null;
        public virtual void OnClose() {
            if (Close != null) {
                Close(this);
            } 
        }

        public Action<IThingGraph> Flush = null;
        public virtual void OnFlush(IThingGraph graph) {
            if (Flush != null) {
                Flush(graph);
            }
        }

        public override void TearDown() {
            OnClose ();
            base.TearDown();
        }
    }
}