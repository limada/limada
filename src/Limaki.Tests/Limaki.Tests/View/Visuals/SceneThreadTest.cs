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
 * http://www.limada.org
 * 
 */


using Limaki.Drawing;
using System.Text;
using System.Threading;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;
using Xwt;

namespace Limaki.Tests.View.Visuals {

    public class SceneThreadTest {
       public class SceneTreadTester {
            public static int testCount = 0;
            public static object myLock = new StringBuilder();
            public IGraphScene<IVisual, IVisualEdge> scene = null;
            public void DoSomething() {
                for (int i = 0; i < 100; i++)
                    foreach (var visual in scene.Elements) {
                        visual.Shape.IsBorderHit(new Point(10, 10), 10);
                        Point location = visual.Shape.Location;
                        location.X++;
                        location.Y++;
                        visual.Shape.Location = location;
                        lock (myLock) testCount++;
                    }
            }
        }

       private IGraphScene<IVisual, IVisualEdge> _data = null;
       public IGraphScene<IVisual, IVisualEdge> Data {
            get {
                if (_data == null) {
                    _data = new BenchmarkOneSceneFactory().Scene;
                }
                return _data;
            }
            set { _data = value; }
        }

        public SceneThreadTest(IGraphScene<IVisual, IVisualEdge> data) {
            this.Data = data;
        }

        public void Run() {
            SceneTreadTester.testCount = 0;
            int threadCount = 100;
            Thread[] threads = new Thread[threadCount];

            Point p = new Point(0, 0);

            for (int i = 0; i < threads.Length; i++) {
                SceneTreadTester test = new SceneTreadTester();
                test.scene = this.Data;
                Thread t = new Thread(new ThreadStart(test.DoSomething));
                threads[i] = t;
            }
            for (int i = 0; i < threads.Length; i++) {
                threads[i].Start();
            }
            int x = SceneTreadTester.testCount;

        }
 
    }
}