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
 */

//#define TraceInvalidate

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.GDI.UI;
using Limaki.Drawing.UI;

namespace Limaki.Widgets.Paint {
    /// <summary>
    /// Decouples Scene from Control
    /// uses a layout to position or adjust the size of widgets
    /// executes the scene.CommandQueue and calculates the region to invalidate
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public class GDISceneControler<TData, TItem> : LayoutControler<TData, TItem>
        // TODO: after refactoring common methods: <Scene, IWidget>
        where TData : Scene
        where TItem : IWidget {
        public GDISceneControler(
            Func<TData> handler, IGDIControl control, IScrollTarget scrollTarget, ICamera camera, ILayout<TData, TItem> layout)
            :
                base(handler, control, scrollTarget, camera, layout) { }

        public override void Execute(ICommand<TItem> command) {
            if (command is LayoutCommand<TItem>) {
                LayoutCommand<TItem> layoutCommand = (LayoutCommand<TItem>) command;
                if (layoutCommand.Parameter == LayoutActionType.Justify) {
                    Layout.Justify (layoutCommand.Target);
                } else if (layoutCommand.Parameter == LayoutActionType.Perform) {
                    Layout.Perform (layoutCommand.Target);
                } else if (layoutCommand.Parameter == LayoutActionType.Invoke) {
                    Layout.Invoke (layoutCommand.Target);
                } else if (layoutCommand.Parameter == LayoutActionType.AddBounds) {
                    Layout.AddBounds(layoutCommand.Target);
                }
            } else if (command is LayoutCommand<TItem, IShape>) {
                LayoutCommand<TItem, IShape> layoutCommand = (LayoutCommand<TItem,IShape>)command;
                if (layoutCommand.Parameter == LayoutActionType.Justify) {
                    Layout.Justify(layoutCommand.Target, layoutCommand.Parameter2);
                } else if (layoutCommand.Parameter == LayoutActionType.Perform) {
                    Layout.Perform(layoutCommand.Target);
                } else if (layoutCommand.Parameter == LayoutActionType.Invoke) {
                    Layout.Invoke(layoutCommand.Target,layoutCommand.Parameter2);
                } else if (layoutCommand.Parameter == LayoutActionType.AddBounds) {
                    Layout.AddBounds(layoutCommand.Target);
                }
            } else if (command is DeleteCommand) {
                command.Execute();
            }
            else {
                RectangleI invalid = RectangleI.Empty;

                if (command.Target.Shape!=null) {
                    invalid = command.Target.Shape.BoundsRect; 
                }

                command.Execute();

                if (invalid != RectangleI.Empty)
                    Data.UpdateBounds(command.Target,invalid);
                else 
                    Data.AddBounds(command.Target);
            }
        }


        protected Region clipRegion = new Region();
        protected GraphicsPath clipPath = new GraphicsPath();

        protected int tolerance = 5;
        public override void Execute() {
            if (Data!=null && Data.Commands.Count != 0) {
                bool regionChanged = false;
                clipRegion.MakeInfinite();
                clipPath.Reset();
                clipPath.FillMode = FillMode.Winding;

                // get near:
                Matrice matrix = this.camera.Matrice.Clone();

                foreach (ICommand<TItem> command in Data.Commands) {
                    if (command != null && command.Target!=null) {
                        
                        PointI[] oldHull = null;
                        
                        if (command.Target.Shape != null) {
                            oldHull = command.Target.Shape.Hull(matrix, tolerance, true);
                        }

                        PointI[] oldDataHull = null;

                        if (command is StateChangeCommand) {
                            oldDataHull = Layout.GetDataHull(
                                command.Target, ((StateChangeCommand)command).Parameter.One,
                                matrix, tolerance, true);
                        } else {
                            oldDataHull = Layout.GetDataHull(command.Target, matrix, tolerance, true);
                        }

                        Execute(command);

                        PointI[] newHull = null;
                        if (command.Target.Shape != null) {
                            newHull = command.Target.Shape.Hull(matrix, tolerance, true);
                        }
                        PointI[] newDataHull = null;
                        if (command is StateChangeCommand) {
                            newDataHull = Layout.GetDataHull(
                                command.Target, ((StateChangeCommand)command).Parameter.Two,
                                matrix, tolerance, true);
                        } else {
                            newDataHull = Layout.GetDataHull(command.Target, matrix, tolerance, true);
                        }

                        if (oldHull != null)
                            clipPath.AddPolygon(GDIConverter.Convert(oldHull));
                        if (oldDataHull != null) 
                            clipPath.AddPolygon(GDIConverter.Convert(oldDataHull));
                        if (newHull != null) 
                            clipPath.AddPolygon(GDIConverter.Convert(newHull));
                        if (newDataHull != null) 
                            clipPath.AddPolygon(GDIConverter.Convert(newDataHull));

                        regionChanged = true;
                    }
                }

                if (regionChanged) {
                    var camera = new Camera (matrix);
                    var oldOffset = scrollTarget.Offset;
                    var oldSize = scrollTarget.ScrollMinSize;
                    var oldPosition = scrollTarget.ScrollPosition;
                    var matrixOffset = new PointI ((int)-matrix.OffsetX, (int)-matrix.OffsetY);

                    scrollTarget.UpdateScrollSize();
                    
                    var newOffset = scrollTarget.Offset;
                    var newSize = scrollTarget.ScrollMinSize;

                    if (newSize.Width < oldSize.Width &&
                        (oldPosition.X + oldOffset.X) == matrixOffset.X) {
                        var h = Math.Max(oldSize.Height, newSize.Height);
                        var rect = new RectangleI(
                            newSize.Width,
                            (int)matrix.OffsetY,
                            oldSize.Width - newSize.Width,
                            h);
                        clipPath.AddRectangle(GDIConverter.Convert(camera.ToSource(rect)));
                    }
                    
                    if (newSize.Height < oldSize.Height &&
                        (oldPosition.Y + oldOffset.Y) == matrixOffset.Y) {
                        var w = Math.Max(oldSize.Width, newSize.Width);
                        var rect = new RectangleI(
                            (int)matrix.OffsetX,
                            newSize.Height,
                            w,
                            oldSize.Height - newSize.Height);
                        clipPath.AddRectangle(GDIConverter.Convert(camera.ToSource(rect)));
                    }

                    if (newOffset.X != oldOffset.X || newOffset.Y != oldOffset.Y) {
                        control.Invalidate ();
                    } else {
                        // this is slow:
                        //control.Invalidate(Rectangle.Ceiling(clipPath.GetBounds()));

                        // this is fast, but has errors:
                        //control.Invalidate(clipPath);

                        // this is faster:
                        //clipRegion.Intersect (clipPath);

                        clipRegion.Intersect(clipPath);
                        ( (IGDIControl) control ).Invalidate (clipRegion);
                    }
                }
            }
        }

        public override void Done() {
            if (Data != null)
                Data.Commands.Clear();
        }

        public override void Invoke() {
            if (Layout != null && Data != null) {
                #region InitSpatialIndex

                SpatialIndex index = Data.SpatialIndex;
                index.BoundsDirty = true;

                #endregion

                Layout.Invoke();

                scrollTarget.UpdateScrollSize();

                if (Commons.Mono) {
                    //control.Invalidate ();
                }
            }
        }
        }
}