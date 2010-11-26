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

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Limaki.Widgets;
using Limaki.Common;
using Limaki.Common.Collections;

namespace Limaki.Winform.Widgets {
    public class WidgetMultiSelector:SelectionShape {
        public WidgetMultiSelector(Handler<Scene> sceneHandler, IWinControl control, ITransformer transformer) : base(control, transformer) {
            ShowGrips = false;
            this.SceneHandler = sceneHandler;
        }

        ///<directed>True</directed>
        Handler<Scene> SceneHandler;
        public Scene Scene {
            get { return SceneHandler(); }
        }

        bool canStart = false;
        public override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            Resolved = false;
            canStart = TestSceneHit(e.Location); 
            Exclusive = false;
        }
        protected override void OnMouseMoveResolved(MouseEventArgs e) {
            base.OnMouseMoveResolved(e);
            Set<IWidget> oldSelected = new Set<IWidget> ();
            foreach (IWidget widget in Scene.Selected.Elements) {
                oldSelected.Add(widget);
            }
            if ((Form.ModifierKeys & Keys.Control) != Keys.Control) {
                Scene.Selected.Clear();
            }
            foreach(IWidget widget in Scene.ElementsIn(this.Shape.BoundsRect)) {
                bool isLinkKey = ( Form.ModifierKeys & Keys.Shift ) == Keys.Shift;
                bool isLink = widget is ILinkWidget;
                bool add = (isLinkKey && isLink) || (!isLinkKey && !isLink);
                if (add) {
                    Scene.Selected.Add (widget);
                    if (!oldSelected.Contains (widget)) {
                        Scene.Commands.Add (new Command<IWidget> (widget));
                    }
                    oldSelected.Remove (widget);
                }
            }
            foreach (IWidget widget in oldSelected) {
                Scene.Commands.Add(new Command<IWidget>(widget));
            }
            control.CommandsExecute ();
        }

        public override bool HitTest(Point p) {
            bool result = false;
            if (Shape == null) {
                return result;
            }
            result = HitBorder(p);
            return result;
        }

        bool TestSceneHit(Point p) {
            bool result = true;
            Point pt = transformer.ToSource(p);
            if (Scene.Hovered != null)
                result = !Scene.Hovered.Shape.IsHit(pt, this.HitSize);
            if (result && Scene.Focused != null)
                result = !Scene.Focused.Shape.IsHit(pt, this.HitSize);
            return result;
        }
        protected override void BaseMouseMove(MouseEventArgs e) {
            if (canStart&&!Exclusive) {
                canStart = TestSceneHit(e.Location);
            }
            if (canStart) {
                base.BaseMouseMove(e);
                Exclusive = Resolved;
            }
        }
        public override void OnMouseUp(MouseEventArgs e) {
            canStart = false;
            base.OnMouseUp(e);
            Exclusive = false;
            IShape oldShape = this.Shape;
            Shape = ShapeFactory.Shape(ShapeDataType, Point.Empty, Size.Empty);
            InvalidateShapeOutline(oldShape, this.Shape);

        }


        bool _exclusive = false;
        public override bool Exclusive {
            get {
                return _exclusive;
            }
            protected set {
                _exclusive = value;
            }
        }
    }
    

}
