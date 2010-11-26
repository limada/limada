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
using Limaki.Drawing;
using Limaki.Drawing.UI;
using Limaki.Graphs;
using Limaki.Common;

namespace Limaki.Widgets.UI {
    /// <summary>
    /// Adds a widget (but not a linkWidget)
    /// </summary>
    public class AddWidgetAction : WidgetChanger {
        public AddWidgetAction(Func<Scene> handler, IControl control, ICamera camera, ILayout<Scene,IWidget> layout):
            base(handler, control, camera) {
            this.Priority = ActionPriorities.SelectionPriority - 20;
            this.Layout = layout;
        }

        public override void OnMouseDown(MouseActionEventArgs e) {
            this.BaseMouseDown (e);
            Resolved = e.Button == MouseActionButtons.Left;
        }

        private ILayout<Scene, IWidget> _layout = null;
        public virtual ILayout<Scene, IWidget> Layout {
            get { return _layout; }
            set { _layout = value; }
        }

        IWidget _newWidget = null;
        public virtual IWidget NewWidget {
            get { return _newWidget; }
            set { _newWidget = value; }
        }
        IWidget _last = null;
        public virtual IWidget last {
            get { return _last; }
            set { _last = value; }
        }

        private int newCounter = 1;
        protected override void OnMouseMoveResolved(MouseActionEventArgs e) {
            if (NewWidget == null) {

                NewWidget = Registry.Pool.TryGetCreate<IWidgetFactory>()
                            .CreateWidget(newCounter + ". Label");

                newCounter++;
                Scene.Add(NewWidget);
                // see: OnMouseUp Scene.Graph.OnGraphChanged (NewWidget, GraphChangeType.Add);
                Scene.Commands.Add(new LayoutCommand<IWidget>(NewWidget, LayoutActionType.Invoke));
                if (Scene.Focused != null) {
                    Scene.Commands.Add (new LayoutCommand<IWidget> (Scene.Focused, LayoutActionType.Perform));
                    Scene.Selected.Remove (Scene.Focused);
                }
                this.MouseDownPos = e.Location;
                last = Scene.Focused;
                Scene.Focused = NewWidget;
                resizing = true;
 
            }
            base.OnMouseMoveResolved (e);
        }

        public override void OnMouseMove(MouseActionEventArgs e) {
            base.BaseMouseMove (e);
            //Resolved = Resolved && ( Widget != null ) && !(Widget is ILinkWidget);
            if (Resolved) {
                OnMouseMoveResolved (e);
            } else {
                base.OnMouseMoveNotResolved (e);
            }
        }

        public override void OnMouseUp(MouseActionEventArgs e) {
            if (NewWidget != null) {
                SizeI newSize = NewWidget.Size;
                if (newSize.Height<10 || newSize.Width < 10) {
                    Scene.Remove (NewWidget);
                    newCounter--;
                    Scene.Focused = null;
                    if (last != null) {
                        Scene.Commands.Add (new LayoutCommand<IWidget> (last, LayoutActionType.Perform));
                    }
                    Scene.Commands.Add(new Command<IWidget>(NewWidget));
                    //control.CommandsExecute ();
                } else {
                    Scene.Graph.OnGraphChanged(NewWidget, GraphChangeType.Add);
                }
            }
            NewWidget = null;
            base.OnMouseUp(e);
        }
    }
}