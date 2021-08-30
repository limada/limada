/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2014 Lytico
 *
 * http://www.limada.org
 * 
 */


using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Drawing.Styles;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using System;
using Limaki.View;
using Limaki.View.Visuals;
using Limaki.View.Viz.Visuals;
using Xwt;

namespace Limaki.Tests.View.Visuals {

    public class EntityBenchmarkOneSceneFactory : EntitySampleSceneFactory<BenchmarkOneGraphFactory<IGraphEntity,IGraphEdge>> {

        public EntityBenchmarkOneSceneFactory () {
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
            public LongtermPerformanceSceneLayout(Func<IGraphScene<IVisual, IVisualEdge>> handler, IStyleSheet stylesheet)
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

            public override void Reset () {
                AlignOnReset = false;
                base.Reset();
            }
        }

        public virtual void Arrange (IGraphScene<IVisual, IVisualEdge> scene) {
            var location = startAt;

            var level2Width = Nodes[2].Size.Width + distance.Width + Nodes[3].Size.Width;
            var ident = startAt.X;
            Nodes[1].Location = new Point(
                ident + (level2Width - Nodes[1].Size.Width) / 2,
                startAt.Y);

            var level2Y = Nodes[1].Location.Y + Nodes[1].Size.Height + distance.Height;

            Nodes[2].Location = new Point(
                ident,
                level2Y);

            Nodes[3].Location = new Point(
                ident + level2Width - Nodes[3].Size.Width,
                level2Y);

            Nodes[4].Location = new Point(
                ident + (level2Width - Nodes[4].Size.Width) / 2,
                level2Y + Nodes[3].Size.Height + distance.Height);


            ident = ident + level2Width + distance.Width;
            level2Width = Nodes[6].Size.Width + distance.Width + Nodes[7].Size.Width;

            Nodes[5].Location = new Point(
                ident + (level2Width - Nodes[5].Size.Width) / 2,
                Nodes[1].Location.Y);

            level2Y = Nodes[5].Location.Y + Nodes[5].Size.Height + distance.Height;

            Nodes[6].Location = new Point(
                ident,
                level2Y);

            Nodes[7].Location = new Point(
                ident + level2Width - Nodes[7].Size.Width,
                level2Y);

            Nodes[8].Location = new Point(
                ident + (level2Width - Nodes[8].Size.Width) / 2,
                level2Y + Nodes[7].Size.Height + distance.Height);

            var vector = new Vector();
            vector.Start = Nodes[1].Shape[Anchor.RightMiddle];
            vector.End = Nodes[6].Shape[Anchor.LeftMiddle];
            ((VectorShape)Line1.Shape).Data = vector;
        }

        public virtual void PopulateScene (IGraphScene<IVisual, IVisualEdge> scene) {
            int oldCount = this.Count;
            this.Count = 1;
            Populate (scene.Graph);
            scene.ClearSpatialIndex ();
            this.Count = oldCount;

            var vector = new Vector();
            var visual = Registry.Pooled<IVisualFactory>().CreateItem("line");
            visual.Shape = new VectorShape(vector);
            scene.Add(visual);
            Line1 = visual;
        }


        protected override IGraphScene<IVisual, IVisualEdge> CreateScene () {
            var result = new Scene();
            result.Graph = this.Graph;
            var layout = new LongtermPerformanceSceneLayout(
                delegate() { return result; }, this.styleSheet);
            
            PopulateScene(result);
            EnsureShapes (layout);

            layout.Reset();
            Arrange(result);
            result.ClearSpatialIndex();
            return result;
        }

        public override IGraphScene<IVisual, IVisualEdge> NewScene () {
            var result = CreateScene ();
            var nextStart = new Point (startAt.X, result.Shape.BoundsRect.Bottom + distance.Height);
            for (int i = 0; i < Count; i++) {
                var example = new EntityBenchmarkOneSceneFactory ();
                example.startAt = nextStart;
                var scene = example.CreateScene ();
                foreach (var visual in scene.Elements)
                    result.Add (visual);
                nextStart = new Point (startAt.X, scene.Shape.BoundsRect.Bottom + distance.Height);
            }
            result.ClearSpatialIndex ();
            var shape = result.Shape;
            return result;
        }
    }
}