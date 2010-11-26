/*
 * Limaki 
 * Version 0.071
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


using System;
using System.Drawing;
using System.Collections.Generic;
using Limaki.Graphs;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Widgets;
using Limaki.Actions;

namespace Limaki.Widgets.Layout {
    public class GraphLayout<TData, TItem> : WidgetLayout<TData, TItem>
        where TData : Scene
        where TItem : class, IWidget {

        protected Orientation _orientation = Orientation.TopBottom;
        public Orientation Orientation {
            get { return _orientation; }
            set { _orientation = value; }
        }
        protected bool _centered = true;
        public bool Centered {
            get { return _centered; }
            set { _centered = value; }
        }

        protected Algo _algo = Algo.BreathFirst;
        public Algo Algo {
            get { return _algo; }
            set { _algo = value; }
        }

        public bool showDebug = false;

        public Size Distance = new Size(30, 15);

        public GraphLayout(Handler<TData> handler, IStyleSheet stylesheet) : base(handler, stylesheet) { }

        protected virtual IEnumerable<TItem> findRoots() {
            if (Data != null) {
                List<Pair<TItem, int>> graphroots = new List<Pair<TItem, int>>();
                SceneWalker walker = new SceneWalker(Data.Graph);

                foreach (TItem item in Data.Graph) {
                    if (!walker.visited.Contains(item)) {
                        bool selected = false;
                        int count = 0;
                        Pair<TItem, int> root = new Pair<TItem, int>(item, int.MaxValue);
                        foreach (LevelItem<IWidget> levelItem in walker.DeepWalk(item,0)) {
                            //if ((levelItem.Link != null) //&& (levelItem.Link.Root == levelItem.Node)) 
                            if (!(levelItem.Node is IEdge<TItem>)) {
                                count = Data.Graph.EdgeCount(levelItem.Node);
                                if (root.Two > count) {
                                    root = new Pair<TItem, int>((TItem)levelItem.Node, count);
                                }
                            }
                            if (levelItem.Node == Data.Focused) {
                                selected = true;
                            }
                        }
                        if (selected) {
                            root = new Pair<TItem, int>((TItem)Data.Focused, 0);
                        }
                        graphroots.Add(root);
                    }
                }
                Comparison<Pair<TItem, int>> comparision = delegate(Pair<TItem, int> a, Pair<TItem, int> b) {
                    return -a.Two.CompareTo(b.Two);
                };
                graphroots.Sort(comparision);
                foreach (Pair<TItem, int> item in graphroots) {
                    yield return item.One;
                }
            }

        }

        public override Point Arrange(TItem start) {
            Point startAt = start.Location;
            Layout<Scene, IWidget> layout = (Layout<Scene, IWidget>)(object)this;
            Arranger<Scene, IWidget, IEdgeWidget> arranger = new Arranger<Scene, IWidget, IEdgeWidget>(
                Data, layout);

            
            arranger.AddDeepWalk(start);

            arranger.ArrangeRowsOfDeepWalk(ref startAt);

            foreach (IEdgeWidget edge in arranger.AffectedEdges) {
                Data.Commands.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Justify));
            }

            return startAt;
        }


        public override void Invoke() {
            if (this.Data != null) {
                Point startAt = (Point)Distance;
                foreach (TItem root in findRoots()) {
                    base.Invoke(root);
                    if (false) {

                        ILayout<Scene, IWidget> layout = (Layout<Scene, IWidget>)(object)this;
                        Arranger<Scene, IWidget, IEdgeWidget> arranger =
                                new Arranger<Scene, IWidget, IEdgeWidget>(Data, layout);

                        arranger.MoveTo (root, startAt);

                        arranger.AddDeepWalk(root);
                        arranger.ArrangeRowsOfDeepWalk (ref startAt);
                        foreach (IEdgeWidget edge in arranger.AffectedEdges) {
                            Data.Commands.Add (new LayoutCommand<IWidget> (edge, LayoutActionType.Justify));
                        }
                        
                    }
                    root.Location = startAt;
                    startAt = Arrange(root);
                }
                //InvokeEdges();
            }
        }

        public virtual void MoveTo(IWidget widget, Point location) {
            Rectangle invalid = widget.Shape.BoundsRect;
            widget.Location = location;
            Data.UpdateBounds(widget, invalid);
        }



    }

    public enum Orientation {
        LeftRight,
        TopBottom
        //,RightLeft,
        //BottomTop,
        //Center
    }

    public enum Order {
        Pre, Post
    }

    public enum Algo {
        DepthFirst, BreathFirst
    }

    public class SceneWalker : Walker<IWidget, IEdgeWidget> {
        public SceneWalker(IGraph<IWidget, IEdgeWidget> graph) : base(graph) { }
    }




}

