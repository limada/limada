using Limada.Model;
using Limaki.Tests;
using System;
using Limada.Data;
using Limaki.Data;
using Limaki.Common;
using Limaki;
using Limaki.Model.Content.IO;
using System.Linq;

namespace Limada.Tests.ThingGraphs {
    public static class ThingGraphIoExtensions {
        public static string Extension(this ThingGraphIo source) {
            return "."+source.Detector.ContentSpecs.First().Extension;
        }
    }
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

        public ThingGraphIo ThingGraphProvider { get; set; }

        IThingGraph _graph = null;
        public virtual IThingGraph Graph { get {
            if (_graph == null)
                return GraphContent.Data;
            else
                return _graph;
            }
            protected set { _graph = value; }
        }
        protected ThingGraphContent _graphContent = null;
        public virtual ThingGraphContent GraphContent {
            get {
                if (_graphContent == null) {
                    _graphContent = OnCreateGraph();
                }
                return _graphContent;
            }
            set { this._graphContent = value; }
        }

        public virtual ThingGraphContent OnCreateGraph () {
            return OnCreateGraph (this.FileName);
        }

        public virtual ThingGraphContent OnCreateGraph (string fileName) {
            if (ThingGraphProvider != null) {
                var info = Iori.FromFileName(fileName + ThingGraphProvider.Extension());
                var result = ThingGraphProvider.Open(info);
                ReportDetail("*** file:\t" + fileName + ThingGraphProvider.Extension());
                return result;
            }
            return new ThingGraphContent{ Data = new ThingGraph() };
        }

        
        public virtual void Close() {
            if (ThingGraphProvider != null) {
                ThingGraphProvider.Close (GraphContent);
                this.GraphContent = null;
            } 
        }

        
        public virtual void OnFlush(IThingGraph graph) {
            if (ThingGraphProvider != null) {
                if (ThingGraphProvider.IoMode== IoMode.Write)
                    ThingGraphProvider.Flush(GraphContent);
            }
        }

        public override void TearDown() {
            Close ();
            base.TearDown();
        }
    }
}