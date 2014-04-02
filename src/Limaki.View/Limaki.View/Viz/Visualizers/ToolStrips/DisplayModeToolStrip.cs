/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Iconerias;
using Limaki.View.Vidgets;
using Limaki.View.Viz.UI;
using Xwt.Backends;

namespace Limaki.View.Viz.Visualizers.ToolStrips {

    [BackendType(typeof(IDisplayModeToolStripBackend))]
    public class DisplayModeToolStrip : DisplayToolStrip<object, IDisplayModeToolStripBackend> {

        public ToolStripCommand SelectCommand { get; set; }
        public ToolStripCommand PanningCommand { get; set; }

        public ToolStripCommand ZoomInOutCommand { get; set; }
        public ToolStripCommand FitToWidthCommand { get; set; }
        public ToolStripCommand FitToHeigthCommand { get; set; }
        public ToolStripCommand FitToScreenCommand { get; set; }
        public ToolStripCommand OriginalSizeCommand { get; set; }


        public DisplayModeToolStrip () {
            Compose();
        }

        protected void DisplayAction (Action<IDisplay> act) {
            var display = CurrentDisplay as IDisplay;
            if (display != null)
                act(display);
        }

        protected void ZoomAction (Action<IZoomTarget> act) {
            var display = CurrentDisplay as IZoomTarget;
            if (display != null) {
                act (display);
                display.UpdateZoom ();
            }
        }

        protected virtual void Compose () {

            Action<IDisplay, bool> selectAction = (display, value) => display.SelectAction.Enabled = value;
            Action<IDisplay, bool> panningAction = (display, value) => display.MouseScrollAction.Enabled = value;

            var actionGroup = new List<Action<IDisplay, bool>>();
            actionGroup.Add(selectAction);
            actionGroup.Add(panningAction);
            Action<Action<IDisplay, bool>, bool> toogleAction = (ga, value) => {
                foreach (var a in actionGroup)
                    if (ga == a) DisplayAction(d => a(d, value)); else DisplayAction(d => a(d, !value));
            };

            SelectCommand = new ToolStripCommand {
                Action = s => toogleAction(selectAction, true),
                Image = Iconery.Select,
                Size = DefaultSize,
                ToolTipText = "Select"
            };

            PanningCommand = new ToolStripCommand {
                Action = s => toogleAction(panningAction, true),
                Image = Iconery.Panning,
                Size = DefaultSize,
                ToolTipText = "Move"
            };

            ZoomInOutCommand = new ToolStripCommand {
                Image = Iconery.Zoom,
                Size = DefaultSize,
                ToolTipText = "Zoom"
            };

            FitToWidthCommand = new ToolStripCommand {
                Action = s => ZoomAction(d => d.ZoomState = ZoomState.FitToWidth),
                Image = Iconery.FitToWidth,
                Size = DefaultSize,
                Text = "Fit to Width"
            };

            FitToHeigthCommand = new ToolStripCommand {
                Action = s => ZoomAction(d => d.ZoomState = ZoomState.FitToHeigth),
                Image = Iconery.FitToHeigth,
                Size = DefaultSize,
                Text = "Fit to Heigth",
            };

            FitToScreenCommand = new ToolStripCommand {
                Action = s => ZoomAction(d => d.ZoomState = ZoomState.FitToScreen),
                Image = Iconery.FitToScreen,
                Size = DefaultSize,
                Text = "Fit to Screen"
            };

            OriginalSizeCommand = new ToolStripCommand {
                Action = s => ZoomAction(d => d.ZoomState = ZoomState.Original),
                Image = Iconery.OriginalSize,
                Size = DefaultSize,
                Text = "Original size"
            };
        }


        public void ZoomInOut (MouseActionEventArgs e) {
            if (e == null)
                return;
            var zoomIn = e.Button == MouseActionButtons.Left;
            var display = this.CurrentDisplay as IDisplay;

            if (this.CurrentDisplay is IDisplayBackend)
                display = (this.CurrentDisplay as IDisplayBackend).Frontend;

            if (display != null) {
                if (display.ActiveVidget == null) {
                    var action = display.EventControler.GetAction<ZoomAction>();
                    if (zoomIn)
                        action.ZoomIn();
                    else
                        action.ZoomOut();
                    display.Viewport.UpdateZoom();
                    return;
                }
            }
            var zoomTarget = this.CurrentDisplay as IZoomTarget;
            if (zoomTarget != null) {
                zoomTarget.ZoomState = Drawing.ZoomState.Custom;
                if (zoomIn)
                    zoomTarget.ZoomFactor = zoomTarget.ZoomFactor * 1.1f;
                else
                    zoomTarget.ZoomFactor = zoomTarget.ZoomFactor / 1.1f;
                zoomTarget.UpdateZoom();
            }
        }

        public override void Detach (object sender) {

        }
    }

    public interface IDisplayModeToolStripBackend : IDisplayToolStripBackend { }
}