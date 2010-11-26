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

using Limaki.Drawing;
using Limaki.Drawing.UI;
using Limaki.Winform.Displays;
using System.Windows.Forms;
using Limaki.Winform.Controls.ToolStrips;
using Limaki.Drawing.GDI;

namespace Limaki.Winform.Controls.ToolStrips {
    public class SceneToolsController:ToolsController<WidgetDisplay,Control> {
        SceneToolStrip toolStrip = null;
        public SceneToolsController(SceneToolStrip editor) {
            this.toolStrip = editor;

        }

        public override void Attach() {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay == null || toolStrip == null) {
                return;
            }
            toolStrip.Attach(
                currentDisplay.SelectAction.Enabled,
                currentDisplay.ScrollAction.Enabled,
                currentDisplay.AddEdgeAction.Enabled,
                currentDisplay.AddWidgetAction.Enabled
                );
        }

        public void SelectOrMove(bool select, bool move, bool connect, bool add) {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay == null) {
                return;
            }
            currentDisplay.SelectAction.Enabled = select;
            currentDisplay.ScrollAction.Enabled = move;

            currentDisplay.AddEdgeAction.Enabled = connect;
            currentDisplay.AddWidgetAction.Enabled = add;

            currentDisplay.WidgetChanger.Enabled = !currentDisplay.AddWidgetAction.Enabled;
            currentDisplay.EdgeWidgetChanger.Enabled = !currentDisplay.AddEdgeAction.Enabled;
        }

        public void ZoomState(ZoomState zoomState) {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay == null) {
                return;
            }
            // is some editor in action?? then do nothing
            if (currentDisplay.ActiveControl == null) {
                currentDisplay.ZoomState = zoomState;
                currentDisplay.UpdateZoom();
            }

        }
        
        public void ZoomInOut(bool zoomIn) {
            var currentDisplay = this.CurrentDisplay;
            var currentControl = this.CurrentControl;
            if (currentControl == currentDisplay && currentDisplay != null) {
                if (currentDisplay.ActiveControl == null) {
                    if (zoomIn)
                        currentDisplay.ZoomAction.ZoomIn();
                    else
                        currentDisplay.ZoomAction.ZoomOut();
                    currentDisplay.UpdateZoom();
                }
            } else if (currentControl is IZoomTarget) {
                IZoomTarget zoomTarget = currentControl as IZoomTarget;
                if (zoomIn)
                    zoomTarget.ZoomFactor = zoomTarget.ZoomFactor * 1.1f;
                else
                    zoomTarget.ZoomFactor = zoomTarget.ZoomFactor / 1.1f;
            }
        }

        public void Layout() {
            var currentDisplay = this.CurrentDisplay;
            currentDisplay.BackColor = GDIConverter.Convert(currentDisplay.DataLayout.StyleSheet.BackColor);
            currentDisplay.CommandsInvoke();
            currentDisplay.Invalidate();
        }
    }
}