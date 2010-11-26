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

using System.IO;
using Limada.Model;
using Limada.View;
using Limaki.Graphs;
using Limaki.Tests;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Graph.Wrappers;
using Limaki.Widgets;
using NUnit.Framework;
using Id = System.Int64;
using Limaki.Model;
using Limaki.Drawing;
using Limaki.Widgets.Layout;
using Limaki.Graphs.Extensions;

namespace Limada.Tests.Model {
    public class ThingSerialzerTest: DomainTest {
        
        [Test]
        public void ThingSerializerTest() {
            IThingGraph graph = new ThingGraph ();
            IThing thing = new Thing();
            graph.Add (thing);
            graph.Add (new Thing ());

            ThingSerializer serializer = new ThingSerializer ();
            serializer.Graph = graph;
            serializer.ThingCollection = graph;

            foreach(IThing t in graph) {
                Assert.IsTrue (serializer.ThingCollection.Contains (t));
            }

            Stream s = new MemoryStream ();
            serializer.Write (s);
            s.Position = 0;
            
            serializer = new ThingSerializer();
            serializer.Graph = graph;
            serializer.Read (s);
            foreach (IThing t in graph) {
                Assert.IsTrue(serializer.ThingCollection.Contains(t));
            }


        }


        WidgetThingGraph GetSourceGraph<TFactory>()
        where TFactory : GenericGraphFactory<IGraphItem, IGraphEdge>, new() {
            Mock<ProgrammingLanguageFactory> mock = new Mock<ProgrammingLanguageFactory>();
            mock.SceneControler.Invoke();
            mock.SceneFacade.ShowAllData();
            mock.Control.CommandsExecute();

            IGraph<IWidget, IEdgeWidget> widgetGraph = (mock.Scene.Graph as GraphView<IWidget, IEdgeWidget>).One;
            widgetGraph.ChangeData = null;
            widgetGraph.DataChanged = null;
            widgetGraph.GraphChanged = null;

            
            WidgetThingGraph sourceGraph = new WidgetThingGraph(widgetGraph, new ThingGraph());
            sourceGraph.Mapper.ConvertOneTwo();
            return sourceGraph;
        }

        public ILayout<Scene, IWidget> GetLayout() {
            return new GraphLayout<Scene, IWidget>(null, StyleSheet.CreateDefaultStyleSheet());
        }
        
        Stream SaveSheet(Scene scene, ILayout<Scene, IWidget> layout) {
            Sheet sheet = new Sheet(scene, layout);
            sheet.Layout.DataHandler = delegate() { return sheet.Scene; };

            Stream s = new MemoryStream();
            sheet.Save(s);
            s.Position = 0;
            return s;
        }

        [Test]
        public void SheetTest() {
            WidgetThingGraph sourceGraph = GetSourceGraph<ProgrammingLanguageFactory> ();

            Scene scene = new Scene();
            scene.Graph = sourceGraph;

            IThingGraph thingGraph = sourceGraph.Two as IThingGraph;
            
            ILayout<Scene, IWidget> layout = this.GetLayout();

            Stream s = SaveSheet (scene, layout);


            StreamReader reader = new StreamReader (s);
            ReportDetail (reader.ReadToEnd ());
            s.Position = 0;

            Sheet sheet = new Sheet(new Scene(), layout);
            sheet.Layout.DataHandler = delegate() { return sheet.Scene; };

            WidgetThingGraph targetGraph = new WidgetThingGraph(new WidgetGraph(), thingGraph);
            sheet.Scene.Graph = targetGraph;
            s.Position = 0;
            sheet.Read (s);

            foreach(IWidget target in targetGraph) {
                IThing thing = targetGraph.Get (target);
                IWidget source = sourceGraph.Get (thing);

                Assert.AreEqual (target.Location, source.Location);
                Assert.AreEqual(target.Size, source.Size);

            }

            foreach (IWidget source in sourceGraph) {
                IThing thing = sourceGraph.Get(source);
                IWidget target = targetGraph.Get(thing);

                Assert.AreEqual(target.Location, source.Location);
                Assert.AreEqual(target.Size, source.Size);

            }
        }


        void TestScene(Scene scene, ILayout<Scene, IWidget> layout, Stream s) {
            
            
            SheetManager sheetManager = new SheetManager();
            SceneTools.CleanScene(scene);

            using (Sheet sheet = new Sheet(scene, layout)) {
                s.Position = 0;
                sheet.Read(s);
            }
            
            IGraphPair<IWidget, IThing, IEdgeWidget, ILink> widgetThingGraph =
                new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink>(scene.Graph);

            foreach(IWidget widget in scene.Elements) {
                IThing thing = widgetThingGraph.Get (widget);
                Assert.IsNotNull (thing);
            }
        }

        [Test]
        public void SheetManagerTest() {
            Scene scene = new Scene();
            scene.Graph = GetSourceGraph<ProgrammingLanguageFactory>();
            if (!(scene.Graph is GraphView<IWidget,IEdgeWidget>)){
                scene.Graph = new GraphView<IWidget, IEdgeWidget> (new WidgetGraph (), scene.Graph);
            }
            ILayout<Scene, IWidget> layout = this.GetLayout();

            Stream s = SaveSheet(scene, layout);

            TestScene(scene, layout,s);

            TestScene(scene, layout, s);

            TestScene(scene, layout, s);

        }
    }
}
