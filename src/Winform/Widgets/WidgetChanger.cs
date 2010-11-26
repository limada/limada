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
 * 
 */

using System.Drawing;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Widgets;

namespace Limaki.Winform.Widgets {

    /// <summary>
    /// Moves or resizes a Widget
    /// </summary>
    public class WidgetChanger : ShapeActionBase {
        ///<directed>True</directed>
        public WidgetChanger(IWinControl control, ITransformer transformer)
            : base(control, transformer) {
            initClip ();
        }

        ///<directed>True</directed>
        public Handler<Scene> SceneHandler;
        public virtual Scene Scene {
            get { return SceneHandler(); }
        }

        public virtual IWidget Widget {
            get {
                if (Scene == null) return null;
                return Scene.Selected;
            }
            set { }
        }

        // TODO: Shape is used to draw the grips; find another solution to 
        // get rid of ShapeSelectionBase-Inheritance
        public override IShape Shape {
            get {
                if (Widget != null)
                    return Widget.Shape;
                else
                    return null;
            }
            set {
                if ((Widget != null) && (value != null))
                    Widget.Shape = value;
            }
        }

        public override bool HitTest(Point p) {
            Point sp = transformer.ToSource(p);
            bool result = ((Widget != null) && (Widget.Shape.IsBorderHit(sp, HitSize)));
            Anchor anchor = Anchor.None;
            if (result && ShowGrips) {
                anchor = HitAnchor(p);
                if (!Resolved) {
                    hitAnchor = anchor;
                }
                if (!(Widget is ILinkWidget))
                    SelectorHelper.SetCursor(anchor, result, p, savedCursor);
            }

            return result;
        }


        public override void OnMouseDown(MouseEventArgs e) {
            Resolved = false;
            if (Widget != null && !(Widget is ILinkWidget)) {
                base.OnMouseDown(e);
            }
        }

        Rectangle clipRect = Rectangle.Empty;
        Shape<Rectangle> clipShape = new RectangleShape();
        IWidget _clipWidget = null;
        Command<IWidget> _clipCommand = null;

        protected void initClip() {
            if (_clipWidget == null) {
                _clipWidget = new Widget<Empty>(new Empty());
                _clipWidget.Shape = clipShape;
            }
            if (_clipCommand == null) {
                _clipCommand = new Command<IWidget>(_clipWidget);
            }            
        }

        // TODO: find a better solution for drawing the grips
        protected override void OnMouseMoveNotResolved(MouseEventArgs e) {
            if (HitTest(e.Location)) {
                ShowGrips = true;
                clipRect = Widget.Shape.BoundsRect;
                clipRect.Inflate(GripSize + 5, GripSize + 5);
                clipShape.Data = clipRect;
                Scene.Commands.Add(_clipCommand);
                control.CommandsExecute ();

            } else if (clipRect != Rectangle.Empty) {
                ShowGrips = false;
                clipShape.Data = clipRect;
                Scene.Commands.Add(_clipCommand);
                control.CommandsExecute();
                clipRect = Rectangle.Empty;
            }
        }

        protected virtual bool checkResizing() {
            return Resolved && resizing &&
            this.transformer.Matrice.Elements[0] > 0.01f &&
            this.transformer.Matrice.Elements[3] > 0.01f;
        }

        protected override void OnMouseMoveResolved(MouseEventArgs e) {
            if (!(Widget is ILinkWidget) && (moving || resizing)) {
                ShowGrips = true;

                Resolved = (Resolved) && (Widget != null);
                if (Resolved) {
                    // save previous shape
                    ICommand<IWidget> command = null;
                    if (moving) {
                        Rectangle delta = transformer.ToSource(
                            Rectangle.FromLTRB(e.Location.X, e.Location.Y,
                                               LastMousePos.X, LastMousePos.Y));

                        Point location = Point.Subtract(Widget.Location, delta.Size);

                        command = new MoveCommand (Widget, location);
                                      
                    } else if (checkResizing()) {
                        Rectangle rect = transformer.ToSource(
                            Rectangle.FromLTRB(MouseDownPos.X, MouseDownPos.Y,
                                               LastMousePos.X, LastMousePos.Y));

                        // do not normalize Links!!
                        if (!(Widget.Shape is ILinkShape)) {
                            rect = ShapeUtils.NormalizedRectangle(rect);
                        }
                        command = new ResizeCommand (Widget, rect);
                    }

                    Scene.Commands.Add(command);

                    foreach (IWidget widget in Scene.AffectedByChange(Widget)) {
                        Scene.Commands.Add(new LayoutCommand<IWidget>(widget, LayoutActionType.Justify));
                    }
                    control.CommandsExecute();
                    
                    // saving current mouse position to be used for next dragging
                    this.LastMousePos = e.Location;
                }
            } else {
                Resolved = false;
            }


        }

        public override void OnPaint(PaintActionEventArgs e) {
            if (Widget != null) {
                base.OnPaint(e);
            }
        }


    }
}
