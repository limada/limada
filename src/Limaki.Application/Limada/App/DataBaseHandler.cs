/*
 * Limada 
 * Version 0.08
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System;
using Limada.Model;
using Limada.Schemata;
using Limada.View;
using Limaki.Data;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Drawing.UI;
using Limaki.Widgets;


namespace Limada.App {
    public class DataBaseHandler : IDataBaseHandler {
       
        WidgetThingGraph widgetThingGraph=null;

        Limada.Data.db4o.ThingGraph thingGraph = null;

        private IDataDisplay<Scene> _display = null;

        public IDataDisplay<Scene> Display {
            get { return _display; }
            set { _display = value; }
        }

        public bool useSchema = true;

        public void SetDisplayData(IDataDisplay<Scene> display) {
            IThingGraph schemaGraph = thingGraph;
            if (useSchema)
                schemaGraph = new SchemaThingGraph(this.thingGraph);

            this.widgetThingGraph = new WidgetThingGraph(new WidgetGraph(), schemaGraph);

            Scene scene = new Scene();

            GraphView<IWidget, IEdgeWidget> graphView =
                new GraphView<IWidget, IEdgeWidget>(widgetThingGraph,new WidgetGraph());

            scene.Graph = graphView;
            display.Data = scene;
        }

        public void Open( string FileName ) {
            Close();
            try {
                IGateway gateway = new Limaki.Data.db4o.Gateway();

                gateway.Open(DataBaseInfo.FromFileName(FileName));

                this.thingGraph = new Limada.Data.db4o.ThingGraph(gateway);

            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show("File load failed: " + ex.Message);

            }

            SchemaFacade.MakeMarkersUnique (thingGraph);

            SetDisplayData (this.Display);

            ShowRoots (Display);
        }



        public void ShowRoots(IDataDisplay<Scene> display) {
            var view = display.Data.Graph as GraphView<IWidget, IEdgeWidget>;

            var graph = new GraphPairFacade<IWidget, IEdgeWidget>()
                .Source<IThing, ILink>(view) as WidgetThingGraph;

            if (graph == null)
                return;

            IThingGraph source = graph.Two as IThingGraph;

            if (graph != null && view != null && source != null) {
                IThing topic = source.GetById (TopicSchema.Topics.Id);
                if (topic != null && (source.Edges(topic).Count > 0)) {
                    view.Add(graph.Get(topic));
                } else
                    foreach (IThing item in new GraphPairFacade<IThing, ILink>().FindRoots(source, null)) {
                        if (!source.IsMarker(item))
                            view.Add(graph.Get(item));
                    }
            }
            display.CommandsInvoke();

        }

        public void SaveCurrent() {
            if (widgetThingGraph != null) {
                widgetThingGraph.Mapper.ConvertOneTwo();
                //Limada.Data.db4o.ThingGraph thingGraph = widgetThingGraph.Two as Limada.Data.db4o.ThingGraph;
                if (this.thingGraph != null) {
                    this.thingGraph.Flush();
                }
            }
        }

        public void Save() {

            SaveCurrent();

            if (this.thingGraph != null) {
                IGraph<IWidget, IEdgeWidget> dataGraph = Display.Data.Graph;
                
                var facade = new GraphPairFacade<IWidget, IEdgeWidget>();
                var source = facade.Source(dataGraph);

                if (source != null) {
                    dataGraph = source.Two;
                }
                if (dataGraph != widgetThingGraph) {
                    if (source == null) {
                        Display.Data.Graph = widgetThingGraph;
                        widgetThingGraph.One = Display.Data.Graph;
                    } else {
                        widgetThingGraph.One = source.Two;
                        source.Two = widgetThingGraph;
                    }
                    SaveCurrent();

                }
            }
        }
        public void Close() {
            SaveCurrent();
            if (widgetThingGraph != null) {
                //Limada.Data.db4o.ThingGraph thingGraph = widgetThingGraph.Two as Limada.Data.db4o.ThingGraph;
                if (thingGraph != null) {
                    thingGraph.Close();
                }
                widgetThingGraph = null;
                thingGraph = null;
            }
        }
    }
}
