using Limada.Model;
using Limaki.Tests;
using System;
using Limada.Data;
using Limaki.Data;
using Limaki.Common;
using Limaki;
using Limaki.Contents.IO;
using System.Linq;

namespace Limada.Tests.ThingGraphs {

    public class ThingGraphTestBase : DomainTest {

        protected ThingFactory Factory = new ThingFactory();

        protected virtual string _fileName { get; set; }
        public virtual string FileName { get { return _fileName ?? TestLocations.GraphtestFile; } }

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
            return CreateGraphContent (this.FileName);
        }

        public virtual ThingGraphContent CreateGraphContent (string fileName) {
            if (ThingGraphIo != null) {
                var info = Iori.FromFileName (fileName + ThingGraphIo.Extension ());
                var result = ThingGraphIo.Open (info);
                ReportDetail ("*** file:\t" + fileName + ThingGraphIo.Extension ());
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
            return "." + source.Detector.ContentSpecs.First ().Extension;
        }
    }
}