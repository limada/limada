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
 * 
 */

using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Visuals;

namespace Limaki.Presenter.Layout {
    /// <summary>
    /// searches for the nearest Anchors of Root and Link
    /// </summary>
    public class NearestAnchorRouter<TItem, TEdge> : RouterBase<TItem, TEdge>
        where TItem : IVisual
        where TEdge : TItem, IEdge<TItem> {


        # region Helper-Methods
        public enum Pos {
            left = 1,
            middle = 2,
            right = 4,
            top = 1,
            bottom = 4
        }

        public static Anchor PosToAnchor(Pos x, Pos y) {
            Anchor result = Anchor.None;
            if (x == Pos.left) {
                if (y == Pos.top)
                    result = Anchor.LeftTop;
                else if (y == Pos.middle)
                    result = Anchor.LeftMiddle;
                else if (y == Pos.bottom)
                    result = Anchor.LeftBottom;
            } else if (x == Pos.middle) {
                if (y == Pos.top)
                    result = Anchor.MiddleTop;
                else if (y == Pos.middle)
                    result = Anchor.Center;
                else if (y == Pos.bottom)
                    result = Anchor.MiddleBottom;
            } else if (x == Pos.right) {
                if (y == Pos.top)
                    result = Anchor.RightTop;
                else if (y == Pos.middle)
                    result = Anchor.RightMiddle;
                else if (y == Pos.bottom)
                    result = Anchor.RightBottom;
            }
            return result;
        }


        public static Pair<Anchor, Anchor> nearestAnchors(IVisual source, IVisual target) {
            var sourceShape = source.Shape;
            var tagetShape = target.Shape;
            if (sourceShape==null||tagetShape==null) {
                throw new ArgumentException ("shape must not be null");
            }
            return nearestAnchors (source.Shape, target.Shape, source is IVisualEdge, target is IVisualEdge);
        }

        public static Pair<Anchor, Anchor> nearestAnchors(IShape source, IShape target,bool sourceIsEdge, bool targetIsEdge) {
            Pos sourceX = Pos.middle;
            Pos targetX = Pos.middle;

            // get near; calls of IShape[AnchorType] are expensive cause  
            // call of System.Drawing.Rectangle.Location is expensive
            var sourceMostRightX = (sourceIsEdge?
                source[Anchor.Center].X:
                source[Anchor.MostRight].X);

            var sourceMostLeftX = (sourceIsEdge?
                source[Anchor.Center].X:
                source[Anchor.MostLeft].X);

            var targetMostRightX = (targetIsEdge?
                target[Anchor.Center].X :
                target[Anchor.MostRight].X);

            var targetMostLeftX = (targetIsEdge?
                target[Anchor.Center].X :
                target[Anchor.MostLeft].X);

            // calculate X
            if (sourceMostLeftX > targetMostRightX) {
                sourceX = Pos.left;
                targetX = Pos.right;
            } else if (sourceMostRightX > targetMostRightX) {
                sourceX = Pos.middle;
                targetX = Pos.middle;
            } else if (sourceMostRightX < targetMostLeftX) {
                sourceX = Pos.right;
                targetX = Pos.left;
            }

            var SourceY = Pos.middle;
            var TargetY = Pos.middle;

            var sourceMostTopY = (sourceIsEdge?
                source[Anchor.Center].Y:
                source[Anchor.MostTop].Y);
            var sourceMostBottomY = (sourceIsEdge ?
                source[Anchor.Center].Y :
                source[Anchor.MostBottom].Y);

            var targetMostBottomY =(targetIsEdge?
                target[Anchor.Center].Y : 
                target[Anchor.MostBottom].Y);

            var targetMostTopY = (targetIsEdge?
                target[Anchor.Center].Y : 
                target[Anchor.MostTop].Y);

            // calculate y
            if (sourceMostTopY > targetMostBottomY) {
                SourceY = Pos.top;
                TargetY = Pos.bottom;
            } else if (sourceMostBottomY < targetMostTopY) {
                SourceY = Pos.bottom;
                TargetY = Pos.top;
            }
            var one = PosToAnchor (sourceX, SourceY);
            var two = PosToAnchor (targetX, TargetY);
            return new Pair<Anchor, Anchor>(one, two);

        }

        #endregion

        public override void routeEdge(TEdge edge) {
            try {
                Pair<Anchor, Anchor> nearest = nearestAnchors (edge.Root, edge.Leaf);
                var e = edge as IVisualEdge;
                e.RootAnchor = nearest.One;
                e.LeafAnchor = nearest.Two;
            } catch (ArgumentException e) {
                ArgumentException ex = new ArgumentException ("Shape-Error with: "+edge.ToString(), e);
                Registry.Pool.TryGetCreate<IExceptionHandler>().Catch(ex,MessageType.OK);
            }
            base.routeEdge(edge);
            
        }
    }
}
