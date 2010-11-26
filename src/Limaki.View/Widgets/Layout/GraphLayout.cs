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


using System;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Drawing.UI;

namespace Limaki.Widgets.Layout {
    public class GraphLayout<TData, TItem> : WidgetLayout<TData, TItem>
        where TData : Scene
        where TItem : IWidget {



        public GraphLayout(Func<TData> handler, IStyleSheet stylesheet) : base(handler, stylesheet) { }

        public override void Invoke() {
            if (this.Data != null) {
                Arranger<Scene, IWidget, IEdgeWidget> arranger = 
                    new Arranger<Scene, IWidget, IEdgeWidget>(
                    Data, (Layout<Scene, IWidget>)(object)this);

                PointI startAt = (PointI)Distance;
                foreach (TItem root in new GraphPairFacade<IWidget,IEdgeWidget>().FindRoots(Data.Graph,Data.Focused)){
                    startAt = new PointI (Distance.Width, startAt.Y);
                    startAt = arranger.Arrange(root, startAt);
                    arranger.ClearRows ();
                }
                arranger.Commit();
            }
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
}

