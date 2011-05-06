using System;
using Limada.Data;
using Limada.Model;
using Limada.Schemata;
using Limada.View;
using Limaki.Graphs;
using Limaki.Widgets;
using Limaki.Data;
using Limaki.Graphs.Extensions;
using Limaki.Common;
using System.Collections.Generic;
using Limaki.Common.Collections;
using System.Linq;

namespace Limada.View {
    public class SceneProvider : ISceneProvider {
        public IThingGraph ThingGraph = null;

        public Scene Scene { get; set; }

        public bool useSchema = true;

        protected virtual IGraph<IWidget, IEdgeWidget> CreateGraphView(IThingGraph thingGraph) {
            SchemaFacade.MakeMarkersUnique(thingGraph);

            IThingGraph schemaGraph = thingGraph;
            if (useSchema && !(thingGraph is SchemaThingGraph)) {
                schemaGraph = new SchemaThingGraph(this.ThingGraph);
            }

            var widgetThingGraph = new WidgetThingGraph(new WidgetGraph(), schemaGraph);

            GraphView<IWidget, IEdgeWidget> graphView =
                new GraphView<IWidget, IEdgeWidget>(widgetThingGraph, new WidgetGraph());
            return graphView;
        }

        public virtual Scene CreateScene(IThingGraph thingGraph) {
            Scene scene = new Scene();
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
                var widgetThingGraph =  GraphPairExtension<IWidget, IEdgeWidget>
                    .Source<IThing, ILink>(this.Scene.Graph);
                if (widgetThingGraph != null) {
                    widgetThingGraph.Mapper.ConvertOneTwo();
                }
                Provider.SaveCurrent();
            }
        }



        public virtual void Save() {

            SaveCurrent();

            //if (this.thingGraph != null) {
            //    IGraph<IWidget, IEdgeWidget> dataGraph = this.Scene.Graph;

            //    var facade = new GraphPairFacade<IWidget, IEdgeWidget>();
            //    var source = facade.Source(dataGraph);

            //    if (source != null) {
            //        dataGraph = source.Two;
            //    }
            //    if (dataGraph != widgetThingGraph) {
            //        if (source == null) {
            //            Display.Data.Graph = widgetThingGraph;
            //            widgetThingGraph.One = Display.Data.Graph;
            //        } else {
            //            widgetThingGraph.One = source.Two;
            //            source.Two = widgetThingGraph;
            //        }
            //        SaveCurrent();

            //    }
            //}
        }

        public virtual void Export(Scene scene, IThingGraph target) {
            var graph =  GraphPairExtension<IWidget, IEdgeWidget>
                .Source<IThing, ILink>(scene.Graph);

            if (graph != null) {
                // get a ThingGraphView with only the things that are in the view
                var thingView = new GraphView<IThing, ILink>(graph.Two as IThingGraph, new ThingGraph());
                foreach (var widget in scene.Elements) {
                    var thing = graph.Get(widget);
                    thingView.Add(thing);
                }

                Provider.Export(thingView, target);
            }
            
        }

        public virtual void ExportAs(Scene scene, DataBaseInfo fileName) {

            var provider = Provider.Clone();
            provider.Open (fileName);
            Export(scene, provider.Data);
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

        public Action<Scene> BeforeOpen { get; set; }
        public Action<Scene> DataBound { get; set; }
        public Action<Scene> BeforeClose { get; set; }
        public Action<Scene> AfterClose { get; set; }

    }
}