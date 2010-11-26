/*
 * Limaki 
 * Version 0.064
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


using System.Drawing;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Widgets;
using Limaki.Widgets.Layout;

namespace Limaki.Tests.Widget {
    public class BenchmarkOneData : SceneTestData {
        public BenchmarkOneData(): base() {
            Count = 0;
        }
        public override string Name {
            get { return "Benchmark 1"; }
        }
        public IWidget Line1;

        public Size distance = new Size(30, 30);
        public Size defaultSize = new Size(50, 20);
        public Point startAt = new Point(30, 30);
        public StyleSheet styleSheet = new StyleSheet("LongtermPerformanceStyle", StyleSheet.SystemStyle);

        public class LongtermPerformanceLayout:WidgetLayout<Scene,IWidget> {
            public LongtermPerformanceLayout(Handler<Scene> handler, IStyleSheet stylesheet):
                base(handler, stylesheet) { }

            public virtual Point nextWidgetLocation(IWidget widget, Size distance) {
                return widget.Location + widget.Size + distance;
            }
            protected override IShape LinkShape() {
                return new VectorShape();
            }
            protected override IShape WidgetShape() {
                return new RectangleShape();
            }
        }

        public virtual void Arrange(Scene scene) {
            Point location = startAt;

            int level2Width = Node2.Size.Width + distance.Width + Node3.Size.Width;
            int ident = startAt.X;
            Node1.Location = new Point(
                ident+(level2Width - Node1.Size.Width) / 2,
                startAt.Y);

            int level2Y = Node1.Location.Y+Node1.Size.Height + distance.Height;

            Node2.Location = new Point (
                ident, 
                level2Y);

            Node3.Location = new Point(
                ident + level2Width - Node3.Size.Width, 
                level2Y);

            Node4.Location = new Point(
                ident + (level2Width - Node4.Size.Width) / 2,
                level2Y + Node3.Size.Height + distance.Height);

            
            ident = ident + level2Width + distance.Width;
            level2Width = Node6.Size.Width + distance.Width + Node7.Size.Width;

            Node5.Location = new Point(
                ident + (level2Width - Node5.Size.Width) / 2,
                Node1.Location.Y);

            level2Y = Node5.Location.Y + Node5.Size.Height + distance.Height;

            Node6.Location = new Point(
                ident,
                level2Y);

            Node7.Location = new Point(
                ident + level2Width - Node7.Size.Width,
                level2Y);

            Node8.Location = new Point(
                ident + (level2Width - Node8.Size.Width) / 2,
                level2Y + Node7.Size.Height + distance.Height);

            Vector vector = new Vector ();
            vector.Start = Node1.Shape[Anchor.RightMiddle];
            vector.End = Node6.Shape[Anchor.LeftMiddle];
            ((VectorShape)Line1.Shape).Data= vector;

        }

        public override void Populate(Scene scene) {
            IWidget widget = new Widget<string>("first node");
            scene.Add(widget);
            Node1 = widget;


            widget = new Widget<string>("second node");
            scene.Add(widget);
            Node2 = widget;


            widget = new Widget<string>("third node");
            scene.Add(widget);
            Node3 = widget;

            widget = new Widget<string>("fourth node");
            scene.Add(widget);
            Node4 = widget;

            ILinkWidget linkWidget = new LinkWidget<string>("[first->second]");
            linkWidget.Root = Node1;
            linkWidget.Leaf = Node2;
            scene.Add(linkWidget);
            Link1 = linkWidget;

            linkWidget = new LinkWidget<string>("[third->[first->second]]");
            linkWidget.Root = Node3;
            linkWidget.Leaf = Link1;
            scene.Add(linkWidget);
            Link2 = linkWidget;


            linkWidget = new LinkWidget<string>("[fourth->[third->[first->second]]]");
            linkWidget.Root = Node4;
            linkWidget.Leaf = Link2;
            scene.Add(linkWidget);
            Link3 = linkWidget;

            // second lattice

            widget = new Widget<string>("fifth node");
            scene.Add(widget);
            Node5 = widget;

            widget = new Widget<string>("sixth node");
            scene.Add(widget);
            Node6 = widget;


            widget = new Widget<string>("seventh node");
            scene.Add(widget);
            Node7 = widget;

            widget = new Widget<string>("eigth node");
            scene.Add(widget);
            Node8 = widget;


            linkWidget = new LinkWidget<string>("[fifth->sixth]");
            linkWidget.Root = Node5;
            linkWidget.Leaf = Node6;
            scene.Add(linkWidget);
            Link4 = linkWidget;

            linkWidget = new LinkWidget<string>("[seventh->eigth]");
            linkWidget.Root = Node7;
            linkWidget.Leaf = Node8;
            scene.Add(linkWidget);
            Link5 = linkWidget;


            linkWidget = new LinkWidget<string>("[[fifth->sixth]->[seventh->eigth]]");
            linkWidget.Root = Link4;
            linkWidget.Leaf = Link5;
            scene.Add(linkWidget);
            Link6 = linkWidget;

            Vector vector = new Vector();
            widget = new Widget<string>("line");
            widget.Shape = new VectorShape(vector);
            scene.Add(widget);
            Line1 = widget;
        }


        protected Scene createScene() {
            Scene result = new Scene();
            result.Graph = this.Graph;
            LongtermPerformanceLayout layout = new LongtermPerformanceLayout(
                delegate() {return result;}, this.styleSheet);
            Populate(result);
            layout.Invoke ();
            Arrange(result);
            result.ClearSpatialIndex();
            return result;
        }

        public override Scene Scene {
            get {
                Scene result = createScene ();
                Point nextStart = new Point (startAt.X, result.Shape.BoundsRect.Bottom + distance.Height);
                for (int i = 0; i < Count; i++) {
                    BenchmarkOneData example = new BenchmarkOneData ();
                    example.startAt = nextStart;
                    Scene scene = example.createScene(); 
                    foreach (IWidget widget in scene.Elements)
                        result.Add (widget);
                    nextStart = new Point(startAt.X, scene.Shape.BoundsRect.Bottom + distance.Height);
                }
                result.ClearSpatialIndex ();
                IShape shape = result.Shape;
                return result;
            }
        }

    }
}
