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

using System;
using System.Drawing;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Widgets;
using Limaki.Widgets.Layout;

namespace Limaki.Winform.Widgets {

    /// <summary>
    /// Adds a link between two widgets
    /// </summary>
    public class AddLinkAction : LinkWidgetChanger {
        public AddLinkAction(Handler<Scene> sceneHandler, IWinControl control, ITransformer transformer)
            : base(sceneHandler, control, transformer) {
            this.Priority = ActionPriorities.SelectionPriority - 30;
        }


        private IWidget _current = null;
        public IWidget Current {
            get { return _current; }
            set { _current = value; }
        }

        ILinkWidget _link = null;
        public override ILinkWidget Link {
            get { return _link; }
            set { _link = value; }
        }
        //IWidget override HitTest(Point p) {
        //    IWidget result = null;
        //    Point sp = transformer.ToSource(p);

        //    result = Scene.Hit(sp, HitSize);

        //    return result;
        //}

        public override void OnMouseDown(MouseEventArgs e) {
            Link = null;
            if (Scene == null) return;
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left) {
                Resolved = (Scene.Focused != null) && (Scene.Focused != Current);
                Current = Scene.Focused;
                LastMousePos = e.Location;
                //Exclusive = true;
            }
        }

        protected virtual ILinkWidget NewWidget(Point p) {
            ILinkWidget result= new LinkWidget<string>(newCounter+". Link");
            newCounter++;
            result.Root = Current;
            return result;
        }

        private int newCounter = 1;

        protected override void OnMouseMoveResolved(MouseEventArgs e) {
            Point p = transformer.ToSource(e.Location);
            if (Link == null) {
                Link = NewWidget (p);
                
                Scene.Add(Link);
                Scene.Commands.Add(new LayoutCommand<IWidget>(Link, LayoutActionType.Invoke));
                Scene.Commands.Add(new LayoutCommand<IWidget>(Scene.Focused, LayoutActionType.Perform));

                Scene.Focused = Link;
                resizing = true;
                Anchor rootAnchor = Anchor.Center;
                if (Current is ILinkWidget) {
                    rootAnchor = Anchor.Center;
                } else {
                    VectorShape shape = new VectorShape ();
                    
                    shape.Location = p;
                    rootAnchor = NearestAnchorRouter.nearestAnchors(Current.Shape, shape,Current is ILinkWidget,true).One;
                }
                MouseDownPos = transformer.FromSource(Current.Shape[rootAnchor]);
            }

            ShowGrips = true;

            Rectangle rect = transformer.ToSource(
                        Rectangle.FromLTRB(MouseDownPos.X, MouseDownPos.Y,
                                           LastMousePos.X, LastMousePos.Y));

            Scene.Commands.Add(new ResizeCommand(Link, rect));

            control.CommandsExecute();
            
            // saving current mouse position to be used for next dragging
            this.LastMousePos = e.Location;
            
            IsTargetHit(this.LastMousePos);


        }

        public override void OnMouseMove(MouseEventArgs e) {
            Exclusive = false;
            if (Scene == null) return;
            if (Current == null) return;
            base.BaseMouseMove(e);
            Resolved = Resolved && (Scene.Focused != null);
            if (Resolved) {
                OnMouseMoveResolved(e);
            }
            if (e.Button != MouseButtons.Left) {
                EndAction();
                Resolved = false;
                return;
            }
            Dragging = !Resolved;
        }

        protected override void EndAction() {
            if (Link != null) {
                if ((Current != null) && (Scene.Focused != Current)) {
                    Scene.Focused = Current;
                    Scene.Commands.Add(new LayoutCommand<IWidget>(Scene.Focused, LayoutActionType.Perform));
                }
                if ((Target == null) && (Link.Leaf == null)) {
                    Scene.Remove(Link);
                    newCounter--;
                    Scene.Commands.Add(new Command<IWidget>(Link));
                    Link = null;
                    control.CommandsExecute();
                } else {
                    base.EndAction();
                }
            } else {
                base.EndAction();
            }
            Link = null;
            Current = null;            
        }
        public override void OnMouseUp(MouseEventArgs e) {
            Exclusive = Resolved;
            base.OnMouseUp (e);
        }
    }
}