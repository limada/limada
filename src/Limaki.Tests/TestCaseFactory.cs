/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 */

using System;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Data;
using Limaki.Drawing;
using Limaki.Graphs;
using Limada.View;
using Limada.VisualThings;
using Limaki.Model;
using Limaki.View.Display;
using Limaki.View.Rendering;
using Limaki.View.UI;
using Limaki.View.UI.GraphScene;
using Limaki.UseCases;
using Limaki.View.Winform.Display;
using Limaki.UseCases.Winform;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Presenter.Display;
using Limaki.Tests.Presenter.GDI;
using Limaki.Tests.Presenter.Winform;
using Limaki.Tests.Visuals;
#if WCF
using Limaki.WCF.Data;
#endif
using Limaki.Visuals;
using Limada.Schemata;
using Limaki.Tests.Presenter;
using Limaki.Viewers;


namespace Limaki.Tests.UseCases {
    public class TestCaseFactory : UseCaseFactory<UseCase> {
        public override void Compose(UseCase useCase) {
            var deviceComposer = DeviceComposer as WinformUseCaseComposer;
            
            this.testMessage = (s, m) => {
                deviceComposer.StatusLabel.Text = m;
                Application.DoEvents();
            };
            CreateTestCases(useCase, deviceComposer);
        }
    
    
        public MessageEventHandler testMessage = null;

        public void CreateTestCases(UseCase useCase, WinformUseCaseComposer composer) {
            
            Get<BenchmarkOneTests> displayTest = () => {
                var test = new BenchmarkOneTests();
                var testinst = new WinformDisplayTestComposer<IGraphScene<IVisual, IVisualEdge>>();

                testinst.Factory = () => new WinformVisualsDisplay().Display;
                testinst.Factor(test);
                testinst.Compose(test);

                test.WriteDetail += testMessage;
                        

                test.TestForm = this;
                test.Display = composer.SplitView.Display1.Display as IGraphSceneDisplay<IVisual, IVisualEdge>;

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
            new ToolStripMenuItem("Repair Database", null, (s, e) => {
                this.RepairDatabase(useCase);
            }), 
            new ToolStripMenuItem("current problem", null, (s, e) =>{
                this.currentProblem (useCase);
            }),
        })
        });

            SetTests(useCase);
        }

        public void ExampleOpen(UseCase useCase) {

            var dialog = new OpenExampleData();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                var testData = dialog.ExampleData.Selected;
                testData.Data.Count = (int)dialog.numericUpDown1.Value;
                SetTestData(useCase.SplitView, testData.Data);
            }
            dialog.Dispose();

        }

       
        public void SetTestData(SplitView0 target, ISceneFactory factory) {
            var scene = factory.Scene;

            IGraph<IVisual, IVisualEdge> data = null;
            if (factory is GenericBiGraphFactory<IVisual, IGraphItem, IVisualEdge, IGraphEdge>) {
                data = ((GenericBiGraphFactory<IVisual, IGraphItem, IVisualEdge, IGraphEdge>)factory).GraphPair;
            } else {
                data = factory.Graph;
            }

            scene.Graph = new GraphView<IVisual, IVisualEdge>(data, new VisualGraph());
            target.ChangeData(scene);
        }

        public void ShowQuadTree(IGraphScene<IVisual, IVisualEdge> scene) {
            var form = new Form();
            var display = new WinformVisualsDisplay();
            display.Dock = DockStyle.Fill;
            form.Controls.Add(display);

            var quadTreeVisualizer = new QuadTreeVisualizer();
            quadTreeVisualizer.VisualsDisplay = display.Display as GraphSceneDisplay<IVisual, IVisualEdge>;
            quadTreeVisualizer.Data = (scene.SpatialIndex as QuadTreeIndex).GeoIndex;


            form.FormClosing += (s, e) => e = null;
            form.Show();


        }
        public void WCFServiceTest(UseCase sender) {
#if WCF
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

        public void InstrumentLayer(IRenderAction renderAction, IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            var layer = renderAction as ILayer<IGraphScene<IVisual, IVisualEdge>>;
            if (layer != null) {
                layer.Data = () => display.Data;
                layer.Camera = () =>  display.Viewport.Camera;
            }
            var graphLayer = layer as GraphSceneLayer<IVisual, IVisualEdge>;
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
                InstrumentLayer(new VisualsBoundsLayer(), display);
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

        public void RepairDatabase(UseCase sender) {
            sender.FileManager.DefaultDialogValues(sender.FileManager.OpenFileDialog);
            if (sender.FileDialogShow(sender.FileManager.OpenFileDialog, true) == Viewers.DialogResult.OK) {
                sender.SaveChanges();
                sender.FileManager.Close();
                sender.FileManager.ShowEmptyThingGraph();
                var file = sender.FileManager.OpenFileDialog.FileName;
                var fileInfo = DataBaseInfo.FromFileName(file);
                if (fileInfo.Extension==".limo") {
                    var repairer = new global::Limada.Data.db4o.Db4oRepairer();
                    repairer.WriteDetail = (m)=> testMessage(repairer,m);
                    var newFile = DataBaseInfo.FromFileName(file);
                    newFile.Name = "repaired." + newFile.Name;
                    repairer.ReadAndSaveAs(fileInfo, newFile, true);
                    sender.FileManager.OpenFile(newFile);
                }
            }
        }

        public void currentProblem(UseCase sender) {
            try {
                //var maint = new VisualThingGraphTest();
                //maint.ExpandAndSaveLinks(sender.GetCurrentDisplay().Data.Graph);

                //var test = new SchemaGraphPerformanceTest();
                //test.WriteDetail += testMessage;
                //test.WriteSummary += testMessage;
                //test.Setup();
                //test.ReadDescriptionTest();
                //test.TearDown();
                //test.WriteDetail -= testMessage;
                //test.WriteSummary -= testMessage;

                //var test = new WinformVisualsDisplayTest<VisualsDisplayTest1>();
                //test.WriteDetail += testMessage;

                //test.Setup();

                //var form = (test.Test.TestForm as Form);
                //form.WindowState = FormWindowState.Normal;

                //var button = new Button() { Text = "Test", Dock = DockStyle.Bottom };
                //form.Controls.Add(button);

                //button.Click += (s, e) => {
                //    test.WriteSummary += testMessage;
                //    test.Test.SelectorVersusMulitSelectTest();

                //    test.WriteDetail -= testMessage;
                //    test.WriteSummary -= testMessage;
                //};
                //test.TearDown();

                //var exporter = new ThingsToPdfProvider();
                //var scene = sender.GetCurrentDisplay().Data;
                //var sceneProvider = new SceneProvider();
                //sceneProvider.ExportTo(scene, exporter, DataBaseInfo.FromFileName("testExport.pdf"));

                var test = new WebProxyTest();
                test.CircleFocusToHtml(sender.GetCurrentDisplay());

            } catch (Exception e) {
                MessageBox.Show(e.Message);
            } finally {

            }
        }
    }
}