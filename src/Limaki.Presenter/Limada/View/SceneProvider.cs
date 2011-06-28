using System;
using Limada.Data;
using Limada.Model;
using Limada.Schemata;
using Limada.View;
using Limaki.Graphs;
using Limaki.Visuals;
using Limaki.Data;
using Limaki.Graphs.Extensions;
using Limaki.Common;
using System.Collections.Generic;
using Limaki.Common.Collections;
using System.Linq;
using Limaki.Drawing;

namespace Limada.View {
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
                var visualThingGraph = GraphPairExtension<IVisual, IVisualEdge>
                    .Source<IThing, ILink>(this.Scene.Graph);
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
            var graph =  GraphPairExtension<IVisual, IVisualEdge>.Source<IThing, ILink>(scene.Graph);

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
            if(visuals.Count()==0)
                visuals = scene.Graph.Where(v => !(v is IVisualEdge));
            if (visuals.Count() == 0)
                return;
            exporter.SaveAs(SortedThings(scene.Graph, visuals), fileName);
        }

        private IEnumerable<IThing> SortedThings(IGraph<IVisual, IVisualEdge> graph, IEnumerable<IVisual> items) {
            var result = items.OrderBy(v => v.Location, new Limaki.Drawing.Shapes.LeftRightTopBottomComparer());
            return result.Select(v => graph.ThingOf(v));
        }

        public Action<IGraphScene<IVisual, IVisualEdge>> BeforeOpen { get; set; }
        public Action<IGraphScene<IVisual, IVisualEdge>> DataBound { get; set; }
        public Action<IGraphScene<IVisual, IVisualEdge>> BeforeClose { get; set; }
        public Action<IGraphScene<IVisual, IVisualEdge>> AfterClose { get; set; }

    }
}