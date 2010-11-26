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
 * 
 */


using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Tests.Widget;
using Limaki.Widgets;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using System;
using Limaki.Presenter.Widgets.Layout;

namespace Limaki.Tests.Graph.Model {
    public class BenchmarkOneSceneFactory : SceneFactory<BenchmarkOneGraphFactory> {
        public BenchmarkOneSceneFactory () {
            this.Count = 10;
        }

        public IWidget Line1;

        public SizeI distance = new SizeI(30, 30);
        public SizeI defaultSize = new SizeI(50, 20);
        public PointI startAt = new PointI(30, 30);
        public StyleSheet styleSheet = 
            new StyleSheet("LongtermPerformanceStyle", 
                StyleSheet.CreateStyleWithSystemSettings ()
                );

        public class LongtermPerformanceLayout : WidgetLayout<IWidget,IEdgeWidget> {
            public LongtermPerformanceLayout(Get<IGraphScene<IWidget, IEdgeWidget>> handler, IStyleSheet stylesheet)
                :
                    base(handler, stylesheet) { }

            public virtual PointI nextWidgetLocation(IWidget widget, SizeI distance) {
                return widget.Location + widget.Size + distance;
            }

            public override IShape CreateShape(IWidget item) {
                if (item is IEdgeWidget) {
                    return new VectorShape ();
                } else {
                    return new RectangleShape ();
                }
            }
        }

        public virtual void Arrange(Scene scene) {
            PointI location = startAt;

            int level2Width = Node[2].Size.Width + distance.Width + Node[3].Size.Width;
            int ident = startAt.X;
            Node[1].Location = new PointI(
                ident + (level2Width - Node[1].Size.Width) / 2,
                startAt.Y);

            int level2Y = Node[1].Location.Y + Node[1].Size.Height + distance.Height;

            Node[2].Location = new PointI(
                ident,
                level2Y);

            Node[3].Location = new PointI(
                ident + level2Width - Node[3].Size.Width,
                level2Y);

            Node[4].Location = new PointI(
                ident + (level2Width - Node[4].Size.Width) / 2,
                level2Y + Node[3].Size.Height + distance.Height);


            ident = ident + level2Width + distance.Width;
            level2Width = Node[6].Size.Width + distance.Width + Node[7].Size.Width;

            Node[5].Location = new PointI(
                ident + (level2Width - Node[5].Size.Width) / 2,
                Node[1].Location.Y);

            level2Y = Node[5].Location.Y + Node[5].Size.Height + distance.Height;

            Node[6].Location = new PointI(
                ident,
                level2Y);

            Node[7].Location = new PointI(
                ident + level2Width - Node[7].Size.Width,
                level2Y);

            Node[8].Location = new PointI(
                ident + (level2Width - Node[8].Size.Width) / 2,
                level2Y + Node[7].Size.Height + distance.Height);

            Vector vector = new Vector();
            vector.Start = Node[1].Shape[Anchor.RightMiddle];
            vector.End = Node[6].Shape[Anchor.LeftMiddle];
            ((VectorShape)Line1.Shape).Data = vector;

        }

        public override void Populate(Scene scene) {
            int oldCount = this.Count;
            this.Count = 1;
            base.Populate (scene);
            this.Count = oldCount;

            Vector vector = new Vector();
            IWidget widget = Registry.Pool.TryGetCreate<IWidgetFactory>().CreateItem("line");
            widget.Shape = new VectorShape(vector);
            scene.Add(widget);
            Line1 = widget;
        }


        public Scene createScene() {
            Scene result = new Scene();
            result.Graph = this.Graph;
            LongtermPerformanceLayout layout = new LongtermPerformanceLayout(
                delegate() { return result; }, this.styleSheet);
            Populate(result);
            layout.Invoke();
            Arrange(result);
            result.ClearSpatialIndex();
            return result;
        }

        public override Scene Scene {
            get {
                Scene result = createScene();
                PointI nextStart = new PointI(startAt.X, result.Shape.BoundsRect.Bottom + distance.Height);
                for (int i = 0; i < Count; i++) {
                    BenchmarkOneSceneFactory example = new BenchmarkOneSceneFactory();
                    example.startAt = nextStart;
                    Scene scene = example.createScene();
                    foreach (IWidget widget in scene.Elements)
                        result.Add(widget);
                    nextStart = new PointI(startAt.X, scene.Shape.BoundsRect.Bottom + distance.Height);
                }
                result.ClearSpatialIndex();
                IShape shape = result.Shape;
                return result;
            }
        }

    }
}