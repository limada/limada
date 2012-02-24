using System;
using Limada.Data;
using Limada.Model;
using Limada.Schemata;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Data;
using Limaki.Common;
using System.Collections.Generic;
using System.Linq;
using Limaki.Graphs.Extensions;
using Limaki.Reporting;
using Limaki.Visuals;

namespace Limaki.Limada.View {
    public class SceneProvider : ISceneProvider {
        public IThingGraph ThingGraph {get;set;}

        public IGraphScene<IVisual, IVisualEdge> Scene { get; set; }

        public bool UseSchema = true;

        protected virtual IGraph<IVisual, IVisualEdge> CreateGraphView(IThingGraph thingGraph) {
            SchemaFacade.MakeMarkersUnique(thingGraph);

            IThingGraph schemaGraph = thingGraph;
            if (UseSchema && !(thingGraph is SchemaThingGraph)) {
                schemaGraph = new SchemaThingGraph(this.ThingGraph);
            }

            var visualThingGraph = new VisualThingGraph(new VisualGraph(), schemaGraph);

            GraphView<IVisual, IVisualEdge> graphView =
                new GraphView<IVisual, IVisualEdge>(visualThingGraph, new VisualGraph());
            return graphView;
        }

        public virtual Scene CreateScene(IThingGraph thingGraph) {
            var scene = new Scene();
            scene.Graph = CreateGraphView(thingGraph);
            return scene;

        }

        protected IThingGraphProvider _provider = null;
        public virtual IThingGraphProvider Provider {
            get {
                if (_provider == null) {
                    _provider = new MemoryThingGraphProvider();
                }
                return _provider;
            }
            set { _provider = value; }
        }

        public virtual bool Open(Action openProvider) {
            //Close();
            if (BeforeOpen != null) {
                BeforeOpen(this.Scene);
            }
            try {
                openProvider();
                this.ThingGraph = Provider.Data;

                this.Scene = CreateScene(this.ThingGraph);

                if (DataBound != null) {
                    DataBound(this.Scene);
                }
            } catch (Exception ex) {
                Registry.Pool.TryGetCreate<IExceptionHandler>()
                    .Catch(new Exception("Open failed: " + ex.Message, ex),MessageType.OK);
                try {
                    Provider.Close();
                } catch{}

                return false;

            }
            return true;
        }

        public bool Open() {
            return Open(() => {
                Provider.Open (); 
            });
        }

        public bool Open(DataBaseInfo FileName) {
            return Open(() => {
                Provider.Open(FileName);
            });
        }

        //public bool SaveAs(DataBaseInfo FileName) {
        //    SaveCurrent ();
        //    try {
        //        Provider.SaveAs (FileName);
        //    } catch (Exception ex) {
        //        Registry.Pool.TryGetCreate<IExceptionHandler>()
        //            .Catch(new Exception("Save as failed: " + ex.Message, ex), MessageType.OK);

        //    }
        //    return true;
        //}

        public virtual void SaveCurrent() {
            if (Scene != null) {
                var visualThingGraph = this.Scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();
                if (visualThingGraph != null) {
                    visualThingGraph.Mapper.ConvertOneTwo();
                }
                Provider.SaveCurrent();
            }
        }



        public virtual void Save() {

            SaveCurrent();
        }

        public virtual void ExportTo(IGraphScene<IVisual, IVisualEdge> scene, IThingGraph target) {
            var graph = scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();

            if (graph != null) {
                // get a ThingGraphView with only the things that are in the view
                var thingView = new GraphView<IThing, ILink>(graph.Two as IThingGraph, new ThingGraph());
                foreach (var visual in scene.Elements) {
                    var thing = graph.Get(visual);
                    thingView.Add(thing);
                }

                Provider.Export(thingView, target);
            }
            
        }


        public virtual void ExportAsThingGraph(IGraphScene<IVisual, IVisualEdge> scene, DataBaseInfo fileName) {
            var provider = Provider.Clone();
            provider.Open (fileName);
            ExportTo(scene, provider.Data);
            provider.Close ();
        }


        public virtual void Close() {
            SaveCurrent();
            Provider.Close();
            if (AfterClose != null) {
                AfterClose(this.Scene);
            }
            this.ThingGraph = null;
        }

        public void ExportTo(IGraphScene<IVisual, IVisualEdge> scene, IDataProvider<IEnumerable<IThing>> exporter, DataBaseInfo fileName) {
            var visuals = scene.Selected.Elements;
            if (visuals.Count() == 0)
                visuals = scene.Graph.Where(v => !(v is IVisualEdge));
            if (visuals.Count() == 0)
                return;
            IEnumerable<IThing> things = null;
            if (visuals.Count() == 1) {
                var thing = scene.Graph.ThingOf(visuals.First());
                var schema = new DocumentSchema(scene.Graph.ThingGraph(), thing);
                if (schema.HasPages())
                    things = schema.OrderedPages();
                var report = exporter as IReport;
                if(report!=null) {
                    report.Options.MarginBottom = 0;
                    report.Options.MarginLeft = 0;
                    report.Options.MarginTop = 0;
                    report.Options.MarginRight = 0;
                }
            }
            if (things == null) {
                things = SortedThings(scene.Graph, visuals);
            }
            exporter.SaveAs(things, fileName);
        }

        private IEnumerable<IThing> SortedThings(IGraph<IVisual, IVisualEdge> graph, IEnumerable<IVisual> items) {
            var result = items.OrderBy(v => v.Location, new PointComparer{Delta=20});
            return result.Select(v => graph.ThingOf(v));
        }

        public Action<IGraphScene<IVisual, IVisualEdge>> BeforeOpen { get; set; }
        public Action<IGraphScene<IVisual, IVisualEdge>> DataBound { get; set; }
        public Action<IGraphScene<IVisual, IVisualEdge>> BeforeClose { get; set; }
        public Action<IGraphScene<IVisual, IVisualEdge>> AfterClose { get; set; }
        public Action<string,int,int> Progress { get; set; }

    }
}