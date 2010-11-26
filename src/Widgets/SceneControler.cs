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
 */

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
    /// Decouples Scene from Control
    /// uses a layout to position or adjust the size of widgets
    /// executes the scene.CommandQueue and calculates the region to invalidate
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public class SceneControler<TData, TItem> : DataControler<TData, TItem>
        // TODO: after refactoring common methods: <Scene, IWidget>
        where TData : Scene
        where TItem : IWidget {
        public SceneControler(
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
                Rectangle invalid = Rectangle.Empty;
                if (command.Target.Shape!=null) {
                    invalid = command.Target.Shape.BoundsRect; 
                }
                command.Execute();
                if (invalid != Rectangle.Empty)
                    Data.UpdateBounds(command.Target,invalid);
                else
                    Data.AddBounds(command.Target);
            }
        }


        protected Region clipRegion = new Region();
        protected GraphicsPath clipPath = new GraphicsPath();

        int tolerance = 5;
        public override void Execute() {
            if (Data!=null && Data.Commands.Count != 0) {
                bool regionChanged = false;
                clipRegion.MakeInfinite();
                clipPath.Reset();
                clipPath.FillMode = FillMode.Winding;

                // get near:
                Matrice matrix = this.transformer.Matrice;
                Transformer transformer = new Transformer(matrix);


                foreach (ICommand<TItem> command in Data.Commands) {
                    if (command != null) {
                        Point[] oldHull = null;
                        if (command.Target.Shape != null) {
                            oldHull = command.Target.Shape.Hull(matrix, tolerance, true);
                        }

                        Execute(command);

                        Point[] newHull = command.Target.Shape.Hull(matrix, tolerance, true);
                        if (oldHull != null) {
                            clipPath.AddPolygon(oldHull);
                        }
                        if (newHull != null) {
                            clipPath.AddPolygon(newHull);
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
                //Data.ReCalculateBounds();
                scrollTarget.ScrollMinSize = transformer.FromSource(Data.Shape.BoundsRect).Size;
                if (Commons.Mono) {
                    control.Invalidate ();
                }
            }
        }
    }
}