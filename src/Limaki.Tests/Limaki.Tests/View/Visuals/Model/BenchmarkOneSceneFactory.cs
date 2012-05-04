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


using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Drawing.Styles;
using Limaki.Tests.Visuals;
using Limaki.Visuals;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using System;
using Limaki.View.Visuals.Layout;
using Xwt;

namespace Limaki.Tests.Graph.Model {
    public class BenchmarkOneSceneFactory : SceneFactory<BenchmarkOneGraphFactory> {
        public BenchmarkOneSceneFactory () {
            this.Count = 10;
        }

        public IVisual Line1;

        public Size distance = new Size(30, 30);
        public Size defaultSize = new Size(50, 20);
        public Point startAt = new Point(30, 30);
        public StyleSheet styleSheet = 
            new StyleSheet("LongtermPerformanceStyle", 
                StyleSheet.CreateStyleWithSystemSettings ()
                );

        public class LongtermPerformanceSceneLayout : VisualsSceneLayout<IVisual,IVisualEdge> {
            public LongtermPerformanceSceneLayout(Get<IGraphScene<IVisual, IVisualEdge>> handler, IStyleSheet stylesheet)
                :
                    base(handler, stylesheet) { }

            public virtual Point nextVisualLocation(IVisual visual, Size distance) {
                return visual.Location + visual.Size + distance;
            }

            public override IShape CreateShape(IVisual item) {
                if (item is IVisualEdge) {
                    return new VectorShape ();
                } else {
                    return new RectangleShape ();
                }
            }
        }

        public virtual void Arrange (IGraphScene<IVisual, IVisualEdge> scene) {
            var location = startAt;

            var level2Width = Node[2].Size.Width + distance.Width + Node[3].Size.Width;
            var ident = startAt.X;
            Node[1].Location = new Point(
                ident + (level2Width - Node[1].Size.Width) / 2,
                startAt.Y);

            var level2Y = Node[1].Location.Y + Node[1].Size.Height + distance.Height;

            Node[2].Location = new Point(
                ident,
                level2Y);

            Node[3].Location = new Point(
                ident + level2Width - Node[3].Size.Width,
                level2Y);

            Node[4].Location = new Point(
                ident + (level2Width - Node[4].Size.Width) / 2,
                level2Y + Node[3].Size.Height + distance.Height);


            ident = ident + level2Width + distance.Width;
            level2Width = Node[6].Size.Width + distance.Width + Node[7].Size.Width;

            Node[5].Location = new Point(
                ident + (level2Width - Node[5].Size.Width) / 2,
                Node[1].Location.Y);

            level2Y = Node[5].Location.Y + Node[5].Size.Height + distance.Height;

            Node[6].Location = new Point(
                ident,
                level2Y);

            Node[7].Location = new Point(
                ident + level2Width - Node[7].Size.Width,
                level2Y);

            Node[8].Location = new Point(
                ident + (level2Width - Node[8].Size.Width) / 2,
                level2Y + Node[7].Size.Height + distance.Height);

            var vector = new Vector();
            vector.Start = Node[1].Shape[Anchor.RightMiddle];
            vector.End = Node[6].Shape[Anchor.LeftMiddle];
            ((VectorShape)Line1.Shape).Data = vector;

        }

        public override void Populate (IGraphScene<IVisual, IVisualEdge> scene) {
            int oldCount = this.Count;
            this.Count = 1;
            base.Populate (scene);
            this.Count = oldCount;

            var vector = new Vector();
            var visual = Registry.Pool.TryGetCreate<IVisualFactory>().CreateItem("line");
            visual.Shape = new VectorShape(vector);
            scene.Add(visual);
            Line1 = visual;
        }


        public IGraphScene<IVisual, IVisualEdge> CreateScene () {
            var result = new Scene();
            result.Graph = this.Graph;
            var layout = new LongtermPerformanceSceneLayout(
                delegate() { return result; }, this.styleSheet);
            Populate(result);
            layout.Invoke();
            Arrange(result);
            result.ClearSpatialIndex();
            return result;
        }

        public override IGraphScene<IVisual, IVisualEdge> Scene {
            get {
                var result = CreateScene();
                var nextStart = new Point(startAt.X, result.Shape.BoundsRect.Bottom + distance.Height);
                for (int i = 0; i < Count; i++) {
                    var example = new BenchmarkOneSceneFactory();
                    example.startAt = nextStart;
                    var scene = example.CreateScene();
                    foreach (var visual in scene.Elements)
                        result.Add(visual);
                    nextStart = new Point(startAt.X, scene.Shape.BoundsRect.Bottom + distance.Height);
                }
                result.ClearSpatialIndex();
                var shape = result.Shape;
                return result;
            }
        }

    }
}