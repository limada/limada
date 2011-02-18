/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 */

using System;
using System.Windows.Forms;
using Limada.Tests.ThingGraphs.SchemaGraph;
using Limada.View;
using Limaki.Common;
using Limaki.Data;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Presenter.Display;
using Limaki.Presenter.UI;
using Limaki.UseCases;
using Limaki.Presenter.Widgets;
using Limaki.Presenter.Winform.Display;
using Limaki.UseCases.Winform;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Presenter.Display;
using Limaki.Tests.Presenter.GDI;
using Limaki.Tests.Presenter.Winform;
using Limaki.Tests.Widget;
#if ! MONO
using Limaki.WCF.Data;
#endif
using Limaki.Widgets;
using Limada.Schemata;


namespace Limaki.Tests.UseCases {
    public class TestCases {
        public MessageEventHandler testMessage = null;

        public void CreateTestCases(UseCase useCase, UseCaseWinformComposer composer) {
            
            Get<BenchmarkOneTests> displayTest = () => {
                var test = new BenchmarkOneTests();
                var testinst = new WinformDisplayTestComposer<IGraphScene<IWidget, IEdgeWidget>>();

                testinst.Factory = () => new WinformWidgetDisplay().Display;
                testinst.Factor(test);
                testinst.Compose(test);

                test.WriteDetail += testMessage;
                        

                test.TestForm = this;
                test.Display = composer.SplitView.Display1.Display as IGraphSceneDisplay<IWidget, IEdgeWidget>;

                test.Setup();
                return test;
            };

            composer.MenuStrip.Items.AddRange(
            new ToolStripMenuItem[] {
                new ToolStripMenuItem("Test", null, new ToolStripMenuItem[] {
                                        new ToolStripMenuItem("Open Testcase...", null, (s, e) => {
                                            this.ExampleOpen (useCase);
                                        }),
            new ToolStripMenuItem("Selector", null, (s, e) => {
                var test = displayTest ();
                test.RunSelectorTest ();
                test.TearDown ();
            }),
            new ToolStripMenuItem("BenchmarkOne", null, (s, e) =>{
                var test = displayTest ();
                test.MoveAlongSceneBoundsTest();
                test.TearDown ();
            }),
            new ToolStripMenuItem("QuadTree", null, (s, e) =>{
                this.ShowQuadTree (useCase.GetCurrentDisplay ().Data);
            }),
            new ToolStripMenuItem("WCF", null, (s, e) =>{
                this.WCFServiceTest (useCase);
            }),
            new ToolStripMenuItem("SchemaFilter off", null, (s, e) =>{
                this.NoSchemaThingGraph (useCase);
            }),
            new ToolStripMenuItem("current problem", null, (s, e) =>{
                this.currentProblem (useCase);
            }),
        })
        });

            SetTests(useCase);
        }

        public void ExampleOpen(UseCase useCase) {

            OpenExampleData dialog = new OpenExampleData();
            if (dialog.ShowDialog() == DialogResult.OK) {
                ExampleData.ITypeChoose testData = dialog.ExampleData.Selected;
                testData.Data.Count = (int)dialog.numericUpDown1.Value;
                SetTestData(useCase.SplitView, testData.Data);
            }
            dialog.Dispose();

        }

        public void ShowQuadTree(Scene scene) {
            var form = new Form ();
            var display = new WinformWidgetDisplay ();
            display.Dock = DockStyle.Fill;
            form.Controls.Add (display);

            var quadTreeVisualizer = new QuadTreeVisualizer ();
            quadTreeVisualizer.widgetDisplay = display.Display as WidgetDisplay;
            quadTreeVisualizer.Data = (scene.SpatialIndex as QuadTreeIndex).GeoIndex;

            
            form.FormClosing += (s, e) => e=null;
            form.Show ();
            

        }
        public void SetTestData(Limaki.UseCases.Viewers.SplitView target, ISceneFactory factory) {
            Scene scene = new Scene();
            scene = factory.Scene;

            IGraph<IWidget, IEdgeWidget> data = null;
            if (factory is GenericBiGraphFactory<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>) {
                data = ((GenericBiGraphFactory<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>)factory).GraphPair;
            } else {
                data = factory.Graph;
            }

            scene.Graph = new GraphView<IWidget, IEdgeWidget>(data, new WidgetGraph());
            target.ChangeData(scene);
        }

        public void WCFServiceTest(UseCase sender) {
#if ! MONO
            DataBaseInfo info = new DataBaseInfo();
            info.Server = "http://localhost";
            info.Port = 8000;
            info.Path = "Limada";
            info.Name = "ThingGraphService";
            var handler = new SceneProvider();
            var provider = new WCFThingGraphClientProvider();

            handler.Provider = provider;
            handler.DataBound = sender.FileManager.DataBound;
            if (handler.Open(info)) {
                sender.DataPostProcess (provider.host.baseAddress.AbsoluteUri);
            }
#endif
        }

        public void InstrumentLayer(IRenderAction renderAction, WidgetDisplay display) {
            var layer = renderAction as ILayer<IGraphScene<IWidget, IEdgeWidget>>;
            if (layer != null) {
                layer.Data = () => display.Data;
                layer.Camera = () =>  display.Viewport.Camera;
            }
            var graphLayer = layer as GraphSceneLayer<IWidget, IEdgeWidget>;
            if (graphLayer !=null) {
                graphLayer.Layout = ()=> display.Layout;
                    
            }
            display.EventControler.Add(renderAction);
        }
        public void NoSchemaThingGraph(UseCase useCase) {
            var display = useCase.GetCurrentDisplay();
            var thingGraph = display.Data.Graph.ThingGraph();
            var schemaGraph = thingGraph as SchemaThingGraph;
            if(schemaGraph!=null) {
                schemaGraph.EdgeFilter = e=>true;
                schemaGraph.ItemFilter = e => true;
            }
        }

        public void SetTests(UseCase useCase) {
            bool showBounds = false;
            bool runThreadTest = false;
            bool showSandbox = false;
            bool showConvexHull = false;
            var display = useCase.GetCurrentDisplay ();
            if (showBounds) {
                InstrumentLayer(new WidgetBoundsLayer(), display);
            }

            if (showSandbox) {
                var sandbox = new RegionSandbox ();
                ((ILayer<Empty>)sandbox).Camera = ()=> display.Viewport.Camera;
                InstrumentLayer(sandbox, display);
            }

            if (showConvexHull) {
                InstrumentLayer (new ConvexHullLayer (), display);
            }

            if (runThreadTest) {
                SceneThreadTest threadTest = new SceneThreadTest(display.Data);
                threadTest.Run();
            }

        }

        public void currentProblem(UseCase sender) {
            try {
                //var maint = new WidgetThingGraphTest();
                //maint.ExpandAndSaveLinks(sender.GetCurrentDisplay().Data.Graph);

                //var test = new SchemaGraphPerformanceTest();
                //test.WriteDetail += testMessage;
                //test.WriteSummary += testMessage;
                //test.Setup();
                //test.ReadDescriptionTest();
                //test.TearDown();
                //test.WriteDetail -= testMessage;
                //test.WriteSummary -= testMessage;

                var test = new WinformWidgetDisplayTest<WidgetDisplayTest1>();
                test.WriteDetail += testMessage;

                test.Setup();

                var form = (test.Test.TestForm as Form);
                form.WindowState = FormWindowState.Normal;

                var button = new Button() { Text = "Test", Dock = DockStyle.Bottom };
                form.Controls.Add(button);

                button.Click += (s, e) => {
                    test.WriteSummary += testMessage;
                    test.Test.SelectorVersusMulitSelectTest();

                    test.WriteDetail -= testMessage;
                    test.WriteSummary -= testMessage;
                };
                test.TearDown();
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            } finally {

            }
        }
    }
}