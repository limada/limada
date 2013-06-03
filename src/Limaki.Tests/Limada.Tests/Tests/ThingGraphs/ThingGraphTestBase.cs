using Limada.Model;
using Limaki.Tests;
using System;
using Limada.Data;
using Limaki.Data;
using Limaki.Common;
using Limaki;

namespace Limada.Tests.ThingGraphs {
    public class ThingGraphTestBase : DomainTest {
        protected ThingFactory Factory = new ThingFactory();

        protected virtual string _fileName { get; set; }
        public virtual string FileName {
            get {
                if (_fileName == null) {
                    return TestLocations.GraphtestFile;
                } else {
                    return _fileName;
                }
            }
        }

        public IThingGraphProvider ThingGraphProvider {get;set;}

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

        public virtual IThingGraph OnCreateGraph() {
            return OnCreateGraph (this.FileName);
        }

        public virtual IThingGraph OnCreateGraph(string fileName) {
            if (ThingGraphProvider != null) {
                var info = Iori.FromFileName(fileName + ThingGraphProvider.Extension);
                ThingGraphProvider.Open(info);
                ReportDetail("*** file:\t" + fileName + ThingGraphProvider.Extension);
                return ThingGraphProvider.Data;
            }
            return new ThingGraph ();
        }

        
        public virtual void Close() {
            if (ThingGraphProvider != null) {
                ThingGraphProvider.Close ();
                this.Graph = null;
            } 
        }

        
        public virtual void OnFlush(IThingGraph graph) {
            if (ThingGraphProvider != null) {
                if (ThingGraphProvider.Saveable)
                    ThingGraphProvider.Save ();
            }
        }

        public override void TearDown() {
            Close ();
            base.TearDown();
        }
    }
}