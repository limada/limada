/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 */

using Limada.Data;
using Limada.Model;
using Limada.Schemata;
using Limaki.Common;
using Limaki.Data;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Visuals;
using System;

namespace Limada.VisualThings {

    public class SceneProvider  {

        public IThingGraph ThingGraph { get; set; }

        public IGraphScene<IVisual, IVisualEdge> Scene { get; set; }

        public bool UseSchema = true;

        //done
        protected virtual IGraph<IVisual, IVisualEdge> CreateGraphView(IThingGraph thingGraph) {
            SchemaFacade.MakeMarkersUnique(thingGraph);

            var schemaGraph = thingGraph;
            if (UseSchema && !(thingGraph is SchemaThingGraph)) {
                schemaGraph = new SchemaThingGraph(thingGraph);
            }

            var visualThingGraph = new VisualThingGraph(new VisualGraph(), schemaGraph);

            var graphView = new GraphView<IVisual, IVisualEdge>(visualThingGraph, new VisualGraph());
            return graphView;
        }

        //done
        public virtual IGraphScene<IVisual, IVisualEdge> CreateScene (IThingGraph thingGraph) {
            var scene = new Scene();
            scene.Graph = CreateGraphView(thingGraph);
            return scene;

        }

        protected IThingGraphProvider _provider = null;
        public virtual IThingGraphProvider Provider { get { return _provider ?? (_provider = new MemoryThingGraphProvider()); } set { _provider = value; } }

        // done
        public virtual bool Open(Action openProvider) {

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

        public bool Open(IoInfo FileName) {
            return Open(() => {
                Provider.Open(FileName);
            });
        }

        //done
        public virtual void SaveCurrent() {
            if (Scene != null) {
                var visualThingGraph = this.Scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();
                if (visualThingGraph != null) {
                    visualThingGraph.Mapper.ConvertOneTwo();
                }
                Provider.SaveCurrent();
            }
        }

        //done
        public virtual GraphView<IThing, ILink> ThingViewOf (IGraphScene<IVisual, IVisualEdge> scene) {
            var graph = scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();

            if (graph == null)
                return null;

            // get a ThingGraphView with only the things that are in the view
            var thingView = new GraphView<IThing, ILink>(graph.Two as IThingGraph, new ThingGraph());
            foreach (var visual in scene.Elements) {
                var thing = graph.Get(visual);
                thingView.Add(thing);
            }
            return thingView;


        }

        public virtual void ExportTo (IGraphScene<IVisual, IVisualEdge> scene, IThingGraph target) {
            var thingView = ThingViewOf(scene);
            Provider.Export(thingView, target);
        }


        public virtual void ExportAsThingGraph(IGraphScene<IVisual, IVisualEdge> scene, IoInfo fileName) {
            var provider = Provider.Clone();
            provider.Open (fileName);
            ExportTo(scene, provider.Data);
            provider.Close ();
        }

 
        public Action<IGraphScene<IVisual, IVisualEdge>> DataBound { get; set; }


    }
}