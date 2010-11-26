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


using System;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Drawing.UI;

namespace Limaki.Widgets.UI {
    /// <summary>
    /// Moves or resizes a Widget
    /// </summary>
    public class WidgetChanger : SelectionBase {
        ///<directed>True</directed>
        public WidgetChanger(Func<Scene> handler, IControl control, ICamera camera)
            : base(control, camera) {
            initClip ();
            this.SceneHandler = handler;
        }

        ///<directed>True</directed>
        public Func<Scene> SceneHandler;
        public virtual Scene Scene {
            get { return SceneHandler(); }
        }

        public virtual IWidget Widget {
            get {
                Scene scene = SceneHandler ();
                if (scene == null) return null;
                return scene.Focused;
            }
            set { }
        }

        // TODO: Shape is used to draw the grips; find another solution to 
        // get rid of ShapeSelectionBase-Inheritance
        public override IShape Shape {
            get {
                IWidget widget = this.Widget;
                if (widget != null)
                    return widget.Shape;
                else
                    return null;
            }
            set {
                IWidget widget = this.Widget;
                if ((widget != null) && (value != null))
                    widget.Shape = value;
            }
        }

        public override bool HitTest(PointI p) {
            PointI sp = camera.ToSource(p);
            IWidget widget = this.Widget;
            bool result = ((widget != null) && (widget.Shape != null) && (widget.Shape.IsBorderHit(sp, HitSize)));
            Anchor anchor = Anchor.None;
            if (result && ShowGrips) {
                anchor = HitAnchor(p);
                if (!Resolved) {
                    hitAnchor = anchor;
                }
                if (!(widget is IEdgeWidget))
                    CursorHandler.SetCursor (this.control,anchor, result);
            }

            return result;
        }


        public override void OnMouseDown(MouseActionEventArgs e) {
            Resolved = false;
            IWidget widget = this.Widget;
            if (widget != null && !(widget is IEdgeWidget)) {
                base.OnMouseDown(e);
            }
        }

        RectangleI clipRect = RectangleI.Empty;
        Shape<RectangleI> clipShape = new RectangleShape();
        IWidget _clipWidget = null;
        Command<IWidget> _clipCommand = null;

        protected void initClip() {
            if (_clipWidget == null) {
                _clipWidget = new ToolWidget<Empty>(new Empty());
                _clipWidget.Shape = clipShape;
            }
            if (_clipCommand == null) {
                _clipCommand = new Command<IWidget>(_clipWidget);
            }            
        }

        // TODO: find a better solution for drawing the grips
        protected override void OnMouseMoveNotResolved(MouseActionEventArgs e) {
            if (HitTest(e.Location)) {
                ShowGrips = true;
                clipRect = this.Widget.Shape.BoundsRect;
                clipRect.Inflate (GripSize + 5, GripSize + 5);
                clipShape.Data = clipRect;
                Scene.Commands.Add (_clipCommand);
            } else if (clipRect != RectangleI.Empty) {
                ShowGrips = false;
                clipShape.Data = clipRect;
                Scene.Commands.Add(_clipCommand);
                clipRect = RectangleI.Empty;
                CursorHandler.RestoreCursor (this.control);
            }
        }

        protected virtual bool checkResizing() {
            return Resolved && resizing &&
                   this.camera.Matrice.Elements[0] > 0.01f &&
                   this.camera.Matrice.Elements[3] > 0.01f;
        }

        protected override void OnMouseMoveResolved(MouseActionEventArgs e) {
            IWidget widget = this.Widget;
            if (!(widget is IEdgeWidget) && (moving || resizing)) {
                ShowGrips = true;

                Resolved = (Resolved) && (widget != null);
                if (Resolved) {
                    ICommand<IWidget> command = null;
                    if (moving) {
                        RectangleI delta = camera.ToSource(
                            RectangleI.FromLTRB(e.Location.X, e.Location.Y,
                                                LastMousePos.X, LastMousePos.Y));

                        foreach(IWidget selected in Scene.Selected.Elements) {
                            if (!(selected is IEdgeWidget)) {
                                Scene.Commands.Add (new MoveByCommand (selected, delta.Size));
                                foreach (IWidget twig in Scene.Twig (selected)) {
                                    Scene.Commands.Add(new LayoutCommand<IWidget>(twig, LayoutActionType.Justify));
                                }
                            }
                        }

                    } else if (checkResizing()) {
                        RectangleI rect = camera.ToSource(
                            RectangleI.FromLTRB(MouseDownPos.X, MouseDownPos.Y,
                                                LastMousePos.X, LastMousePos.Y));

                        // do not normalize Links!!
                        if (!(widget.Shape is IEdgeShape)) {
                            rect = ShapeUtils.NormalizedRectangle(rect);
                        }
                        command = new ResizeCommand (widget, rect);
                        Scene.Commands.Add(command);

                        foreach (IWidget twig in Scene.Twig(widget)) {
                            Scene.Commands.Add(new LayoutCommand<IWidget>(twig, LayoutActionType.Justify));
                        }
                    }
                    
                    // saving current mouse position to be used for next dragging
                    this.LastMousePos = e.Location;
                }
            } else {
                Resolved = false;
            }


        }

        public override void OnPaint(IPaintActionEventArgs e) {
            if (Widget != null) {
                base.OnPaint(e);
            }
        }


    }
}