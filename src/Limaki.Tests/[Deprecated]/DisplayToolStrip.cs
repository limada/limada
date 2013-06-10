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
 * http://www.limada.org
 * 
 */

using Limaki.Actions;
using Limaki.Drawing;
using Limaki.View;
using Limaki.View.UI;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;

namespace Limaki.Viewers.ToolStripViewers {

    public class DisplayToolStrip : ToolStripViewer0<IDisplay, IDisplayToolStripBackend> {

        public object Control { get; set; }
        public virtual bool ActionEnabled<T>(IDisplay display) where T : IAction {
            bool result = false;
            IAction action = display.EventControler.GetAction<T>();
            if (action != null) {
                result = action.Enabled;
            }
            return result;
        }
        
        public virtual bool ActionEnabled<T>(IDisplay display, bool enable) where T : IAction {
            IAction action = display.EventControler.GetAction<T>();
            if (action != null) {
                action.Enabled=enable;
                return action.Enabled;
            }
            return false;
        }

        public override void Attach(object sender) {
            this.Control = sender;

            var display = sender as IDisplay;
            var backend = sender as IDisplayBackend;
            if (backend != null) {
                display = backend.Frontend;
            }
            var tool = this.Backend;
            if (display == null || tool == null) {
                return;
            }
            this.CurrentDisplay = display;
            this.Control = display;
            tool.Attach(
                display.SelectAction.Enabled,
                display.MouseScrollAction.Enabled,
                ActionEnabled<AddEdgeAction>(display),
                ActionEnabled <GraphItemAddAction<IVisual, IVisualEdge>>(display)
                );
        }

        public override void Detach(object sender) {
            this.CurrentDisplay = null;
        }

        public void SelectOrMove(bool select, bool move, bool connect, bool add) {
            var display = this.CurrentDisplay;
            if (display == null) {
                return;
            }

            display.SelectAction.Enabled = select;
            display.MouseScrollAction.Enabled = move;

            connect = ActionEnabled<AddEdgeAction> (display, connect);
            add = ActionEnabled<GraphItemAddAction<IVisual, IVisualEdge>>(display, add);

            ActionEnabled<GraphEdgeChangeAction<IVisual, IVisualEdge>>(display, !connect);
            ActionEnabled<GraphItemMoveResizeAction<IVisual, IVisualEdge>>(display, !add);
            

        }

        public void ZoomState(ZoomState zoomState) {
            var display = this.CurrentDisplay;
            var currentControl = this.Control;
            if (display == null) {
                return;
            }

            // is some editor in action?? then do nothing
            if (display.ActiveControl == null) {
                if (currentControl == display && display != null) {
                    display.ZoomState = zoomState;
                    display.Viewport.UpdateZoom();
                } else if (currentControl is IZoomTarget) {
                    var zoomTarget = currentControl as IZoomTarget;
                    zoomTarget.ZoomState = zoomState;
                    zoomTarget.UpdateZoom();
                }
            }

        }
        
        public void ZoomInOut(bool zoomIn) {
            var display = this.CurrentDisplay;
            var currentControl = this.Control;
            if (currentControl == display && display != null) {
                if (display.ActiveControl == null) {
                    var action = display.EventControler.GetAction<ZoomAction> ();
                    if (zoomIn)
                        action.ZoomIn();
                    else
                        action.ZoomOut();
                    display.Viewport.UpdateZoom();
                }
            } else if (currentControl is IZoomTarget) {
                IZoomTarget zoomTarget = currentControl as IZoomTarget;
                zoomTarget.ZoomState = Drawing.ZoomState.Custom;
                if (zoomIn)
                    zoomTarget.ZoomFactor = zoomTarget.ZoomFactor * 1.1f;
                else
                    zoomTarget.ZoomFactor = zoomTarget.ZoomFactor / 1.1f;
                zoomTarget.UpdateZoom();
            }
        }

        public void Layout() {
            var display = this.CurrentDisplay;
            if (display != null) {
                display.BackColor = display.StyleSheet.BackColor;
                display.Invoke ();
                display.BackendRenderer.Render ();
            }
        }
    }
}