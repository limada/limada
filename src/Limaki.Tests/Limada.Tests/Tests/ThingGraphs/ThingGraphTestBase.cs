using Limada.Model;
using Limaki.Tests;
using System;
using Limada.IO;
using Limaki.Data;
using Limaki.Common;
using Limaki;
using Limaki.Contents.IO;
using System.Linq;

namespace Limada.Tests.ThingGraphs {

    public class ThingGraphTestBase : DomainTest {

        protected ThingFactory Factory = new ThingFactory();
        protected string FileName { get; set; }
        protected Iori _iori = null;
        public virtual Iori Iori {
            get {
                if (_iori == null) {
                    if (FileName == null)
                        FileName = TestLocations.GraphtestFile;
                    _iori = Iori.FromFileName (FileName);
                }
                return _iori;
            }
        }

        public ThingGraphIo ThingGraphIo { get; set; }

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
                    _graphContent = CreateGraphContent();
                }
                return _graphContent;
            }
            set { this._graphContent = value; }
        }

        public virtual ThingGraphContent CreateGraphContent () {
            return CreateGraphContent (this.Iori);
        }

        public virtual ThingGraphContent CreateGraphContent (Iori iori) {
            if (ThingGraphIo != null) {
                var result = ThingGraphIo.Open (iori);
                ReportDetail ("*** file:\t" + iori.Name + ThingGraphIo.Extension ());
                return result;
            }
            return new ThingGraphContent { Data = new ThingGraph () };
        }

        
        public virtual void Close() {
            if (ThingGraphIo != null) {
                ThingGraphIo.Close (GraphContent);
                this.GraphContent = null;
            } 
        }

        
        public virtual void OnFlush(IThingGraph graph) {
            if (ThingGraphIo != null) {
                if (ThingGraphIo.IoMode== IoMode.Write)
                    ThingGraphIo.Flush(GraphContent);
            }
        }

        public override void TearDown() {
            Close ();
            base.TearDown();
        }
    }

    public static class ThingGraphIoExtensions {
        public static string Extension (this ThingGraphIo source) {
            if (source.Detector == null)
                return ".<null>";
            return "." + source.Detector.ContentSpecs.First ().Extension;
        }
    }
}