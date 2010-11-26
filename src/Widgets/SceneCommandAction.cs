/*
 * Limaki 
 * Version 0.063
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

#define useHull

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using System;

namespace Limaki.Widgets {
    /// <summary>
    /// Executes all Commands in Scene.Commands
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public class SceneCommandAction<TData, TItem> : CommandAction<TData, TItem>
        // TODO: after refactoring common methods: <Scene, IWidget>
        where TData : Scene
        where TItem : IWidget {
        public SceneCommandAction(
            Handler<TData> handler, IControl control, IScrollTarget scrollTarget, ITransformer transformer, ILayout<TData, TItem> layout)
            :
                base(handler, control, scrollTarget, transformer, layout) { }

        public override void Execute(ICommand<TItem> command) {
            if (command is LayoutCommand<TItem>) {
                LayoutCommand<TItem> layoutCommand = (LayoutCommand<TItem>)command;
                if (layoutCommand.Parameter == LayoutActionType.Justify) {
                    Layout.Justify(layoutCommand.Target);
                } else if (layoutCommand.Parameter == LayoutActionType.Perform) {
                    Layout.Perform(layoutCommand.Target);
                } else if (layoutCommand.Parameter == LayoutActionType.Invoke) {
                    Layout.Invoke(layoutCommand.Target);
                }
            } else {
                command.Execute();
            }
        }
#if useHull
        // is slower, and makes clutter:
        Point[] GetHull(VectorShape shape, Matrice matrix, int delta) {
            Vector vector = shape.Data;
            vector.Transform (matrix);
            return vector.PolygonHull (delta);
        }
#endif
        protected Region clipRegion = new Region();
        protected GraphicsPath clipPath = new GraphicsPath();

        int hullTolerance = 10;  // less than 10 doesnt work
        public override void Execute() {
            if (Data.Commands.Count != 0) {
                bool regionChanged = false;
                clipRegion.MakeInfinite();
                clipPath.Reset();
                clipPath.FillMode = FillMode.Winding;
                
                // get near:
                Matrice matrix = this.transformer.Matrice;
                Transformer transformer = new Transformer(matrix);


                foreach (ICommand<TItem> command in Data.Commands) {
                    if (command != null) {
                        Rectangle oldBounds = Rectangle.Empty;
#if useHull
                        Point[] oldHull = null;
                        bool isVector = command.Target.Shape is VectorShape;
#endif
                        if (command.Target.Shape != null) {
                            
#if useHull
                            if (isVector) {
                                oldHull = ((VectorShape)command.Target.Shape).PolygonHull(hullTolerance);
                                matrix.TransformPoints(oldHull);
                                //oldHull = GetHull(((VectorShape)command.Target.Shape), matrix, hullTolerance);
                            } else 
#endif
                                oldBounds = command.Target.Shape.BoundsRect;
                            Data.RemoveBounds(command.Target);
                        }

                        Execute(command);

                        
                        Data.AddBounds(command.Target);

#if useHull
                        if (isVector) {
                            Point[] newHull = ((VectorShape)command.Target.Shape).PolygonHull(hullTolerance);
                            matrix.TransformPoints(newHull);
                            //Point[] newHull = GetHull(((VectorShape)command.Target.Shape), matrix, hullTolerance);
                            if (oldHull != null) {
                                clipPath.AddPolygon(oldHull);
                            }
                            if (newHull != null) {
                                clipPath.AddPolygon(newHull);
                            }
                            
                        } else
#endif
 {
                            Rectangle newBounds = command.Target.Shape.BoundsRect;
                            if (oldBounds != newBounds) {
                                oldBounds = transformer.FromSource(oldBounds);
                                oldBounds.Inflate(5, 5);
                                clipPath.AddRectangle(oldBounds);
                            }
                            newBounds = transformer.FromSource(newBounds);
                            newBounds.Inflate(5, 5);
                            clipPath.AddRectangle(newBounds);
                        }


                        regionChanged = true;
                    }
                }

                if (regionChanged) {

                    // this is slow:
                    //control.Invalidate(Rectangle.Ceiling(clipPath.GetBounds()));

                    // this is fast, but has errors:
                    //control.Invalidate(clipPath);

                    // this is faster:
                    clipRegion.Intersect(clipPath);
                    control.Invalidate(clipRegion);

                    scrollTarget.ScrollMinSize = transformer.FromSource(Data.Shape.BoundsRect).Size;

                }
            }
        }

        public override void Done() {
            if (Data != null)
                Data.Commands.Clear();
        }
        public override void Invoke() {
            if (Layout != null && Data != null) {
                Layout.Invoke();
                Data.ReCalculateBounds();
                scrollTarget.ScrollMinSize = transformer.FromSource(Data.Shape.BoundsRect).Size;
            }
        }
    }
}