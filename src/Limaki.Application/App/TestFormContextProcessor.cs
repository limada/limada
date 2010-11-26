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

using Limada.Data;
using Limada.View;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Drawing;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Widget;
using Limaki.Widgets;
using Limaki.Tests.Display;
using Limaki.Tests.Graph.Wrappers;
using Limaki.Tests.Sandbox;
using System.Windows.Forms;
using System;
using System.IO;
using Limaki.Data;
using Limada.Tests.Data.db4o;
using Limada.Tests.View;
using Limada.Model;
using Limaki.Graphs.Extensions;
using Limaki.Winform.Displays;
using Limaki.Winform.Viewers;
using Limada.Tests.ThingGraphs.SchemaGraph;

namespace Limaki.App {
    public class TestFormContextProcessor : ContextProcessor<MainForm> {
        public override void ApplyProperties(IApplicationContext context, MainForm target) {

            var factory = new SceneFactory<GCJohnBostonGraphFactory>();
            factory.Count = 1;
            factory.SeperateLattice = true;
            SetTestData(target.SplitView, factory);
            SetTests(target.CurrentDisplay);

            testMessage += target.testMessage;
            testPaint += target.display_Paint;
            testMouseMove += target.display_MouseMove;
        }

        protected MessageEventHandler testMessage = null;
        protected PaintEventHandler testPaint = null;
        protected MouseEventHandler testMouseMove = null;

        public void ExampleOpen(MainForm sender) {

            OpenExampleData dialog = new OpenExampleData();
            if (dialog.ShowDialog(sender) == DialogResult.OK) {
                OpenExampleData.ITypeChoose testData = dialog.examples[dialog.comboBox1.SelectedIndex];
                testData.Data.Count = (int)dialog.numericUpDown1.Value;
                SetTestData(sender.SplitView, testData.Data);
            }
            dialog.Dispose();

        }

        public void SetTestData(SplitView target, ISceneFactory factory) {
            Scene scene = new Scene();
            scene = factory.Scene;

            IGraph<IWidget, IEdgeWidget> data = null;
            if (factory is GenericBiGraphFactory<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>) {
                data = ( (GenericBiGraphFactory<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>) factory ).GraphPair;
            } else {
                data = factory.Graph;
            }

            scene.Graph = new GraphView<IWidget, IEdgeWidget>(data, new WidgetGraph());
            target.ChangeData (scene);
        }

        public void ShowQuadTree(Scene scene) {
            QuadTreeVisualizer vis = new QuadTreeVisualizer();
            if (scene.SpatialIndex is QuadTreeIndex)
                vis.Data = ((QuadTreeIndex)scene.SpatialIndex).GeoIndex;
            vis.Show();
        }

        public void lineTextHull(WidgetDisplay display) {
            try {
                LineTextTest test = new LineTextTest(display);
                test.DoDetail = true;
                test.WriteSummary += this.testMessage;
                test.Setup();
                test.LineTextHover();

                test.TearDown();

            } finally {
            }

        }

        public void SelectorTest(WidgetDisplay display) {
            try {
                display.Paint -= this.testPaint;
                display.MouseMove -= this.testMouseMove;

                WidgetDisplayTest test = new BenchmarkOneTests(display);

                test.WriteDetail += this.testMessage;
                test.Setup();
                test.RunSelectorTest();
                test.TearDown();
            } finally {
                display.Paint += this.testPaint;
                display.MouseMove += this.testMouseMove;
            }
        }

        public void BenchmarkOneTest(WidgetDisplay display) {

            try {
                display.Paint -= this.testPaint;
                display.MouseMove -= this.testMouseMove;
                BenchmarkOneTests test = new BenchmarkOneTests(display);
                test.Scene = null;
                test.Setup();
                test.WriteDetail += this.testMessage;
                test.MoveAlongSceneBoundsTest();
                test.TearDown();
            } finally {
                display.Paint += this.testPaint;
                display.MouseMove += this.testMouseMove;
            }

        }

        public void SetTests(WidgetDisplay display) {
            # region tests

            bool showBounds = false;
            bool runThreadTest = false;
            bool showSandbox = false;

            if (showBounds) {
                WidgetBoundsLayer widgetBoundsLayer =
                    new WidgetBoundsLayer(display.Camera);
                widgetBoundsLayer.Data = display.Data;
                display.EventControler.Add(widgetBoundsLayer);
            }

            if (showSandbox) {
                RegionSandbox regionSandbox = new RegionSandbox(display.Camera);
                display.EventControler.Add(regionSandbox);
            }

            if (runThreadTest) {
                SceneThreadTest threadTest = new SceneThreadTest(display.Data);
                threadTest.Run();
            }

            #endregion
        }

        
        public void currentProblemToolStripMenuItem_Click(MainForm sender) {
            try {
                var maint = new WidgetThingGraphTest ();
                maint.ExpandAndSaveLinks (sender.CurrentDisplay.Data.Graph);

                var test = new SchemaGraphPerformanceTest();
                //test.WriteDetail += sender.testMessage;
                //test.WriteSummary += sender.testMessage;
                //test.Setup ();
                //test.ReadDescriptionTest();
                //test.TearDown ();
                //test.WriteDetail -= sender.testMessage;
                //test.WriteSummary -= sender.testMessage;

            } catch (Exception e) {
                MessageBox.Show (e.Message);
            }
            finally {
                
            }
        }

    }
}