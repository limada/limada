/*
 * Limaki 
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


using Limaki.Drawing;
using System.Text;
using System.Threading;
using Limaki.Tests.Graph.Model;
using Limaki.Widgets;

namespace Limaki.Tests.Widget {
    public class SceneThreadTest {
       public class SceneTreadTester {
            public static int testCount = 0;
            public static object myLock = new StringBuilder();
            public Scene scene = null;
            public void DoSomething() {
                for (int i = 0; i < 100; i++)
                    foreach (IWidget widget in scene.Elements) {
                        widget.Shape.IsBorderHit(new PointI(10, 10), 10);
                        PointI location = widget.Shape.Location;
                        location.X++;
                        location.Y++;
                        widget.Shape.Location = location;
                        lock (myLock) testCount++;
                    }
            }
        }

        private Scene _data = null;
        public Scene Data {
            get {
                if (_data == null) {
                    _data = new BenchmarkOneSceneFactory().Scene;
                }
                return _data;
            }
            set { _data = value; }
        }

        public SceneThreadTest(Scene data) {
            this.Data = data;
        }

        public void Run() {
            SceneTreadTester.testCount = 0;
            int threadCount = 100;
            Thread[] threads = new Thread[threadCount];

            PointI p = new PointI(0, 0);

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