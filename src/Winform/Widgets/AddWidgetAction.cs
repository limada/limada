/*
 * Limaki 
 * Version 0.07
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
    /// Adds a widget (but not a linkWidget)
    /// </summary>
    public class AddWidgetAction : WidgetChanger {
        public AddWidgetAction(IWinControl control, ICamera camera):base(control, camera) {
            this.Priority = ActionPriorities.SelectionPriority - 20;
        }

        public override void OnMouseDown(MouseEventArgs e) {
            this.BaseMouseDown (e);
            Resolved = e.Button == MouseButtons.Left;
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
        protected override void OnMouseMoveResolved(MouseEventArgs e) {
            if (NewWidget == null) {
                NewWidget = new Widget<string> (newCounter+". Label");
                newCounter++;
                Scene.Commands.Add(new LayoutCommand<IWidget>(NewWidget, LayoutActionType.Invoke));
                Scene.Add(NewWidget);
                if (Scene.Focused != null) {
                    Scene.Commands.Add (new LayoutCommand<IWidget> (Scene.Focused, LayoutActionType.Perform));
                }
                this.MouseDownPos = e.Location;
                last = Scene.Focused;
                Scene.Focused = NewWidget;
                resizing = true;
 
            }
            base.OnMouseMoveResolved (e);
        }

        public override void OnMouseMove(MouseEventArgs e) {
            base.BaseMouseMove (e);
            //Resolved = Resolved && ( Widget != null ) && !(Widget is ILinkWidget);
            if (Resolved) {
                OnMouseMoveResolved (e);
            } else {
                base.OnMouseMoveNotResolved (e);
            }
        }

        public override void OnMouseUp(MouseEventArgs e) {
            if (NewWidget != null) {
                Size newSize = NewWidget.Size;
                if (newSize.Height<10 || newSize.Width < 10) {
                    Scene.Remove (NewWidget);
                    newCounter--;
                    Scene.Focused = null;
                    if (last != null) {
                        Scene.Commands.Add (new LayoutCommand<IWidget> (last, LayoutActionType.Perform));
                    }
                    Scene.Commands.Add(new Command<IWidget>(NewWidget));
                    control.CommandsExecute ();
                }
            }
            NewWidget = null;
            base.OnMouseUp(e);
        }
    }
   
}
