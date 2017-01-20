/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2014 Lytico
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

    [BackendType (typeof (IDisplayModeToolStripBackend))]
    public class DisplayModeToolStrip : DisplayToolStrip<object> {

        public IToolStripCommand EditCommand { get; set; }
        public IToolStripCommand PanningCommand { get; set; }

        public IToolStripCommand ZoomInOutCommand { get; set; }
        public IToolStripCommand FitToWidthCommand { get; set; }
        public IToolStripCommand FitToHeigthCommand { get; set; }
        public IToolStripCommand FitToScreenCommand { get; set; }
        public IToolStripCommand OriginalSizeCommand { get; set; }


        public DisplayModeToolStrip () {
            Compose ();
        }

        protected void DisplayAction (Action<IDisplay> act) {
            var display = CurrentDisplay as IDisplay;
            if (display != null)
                act (display);
        }

        protected void ZoomAction (Action<IZoomTarget> act) {
            Func<object, bool> zoom = v => {
                var zoomTarget = v as IZoomTarget;
                if (zoomTarget == null)
                    return false;

                act (zoomTarget);
                zoomTarget.UpdateZoom ();
                return true;
            };
            Func<IDisplay, bool> zoomDisplay = v => {
                if (v == null)
                    return false;
                if (zoom (v))
                    return true;
                if (zoom (v.Backend))
                    return true;
                return zoom (v.ActiveVidget);
            };

            if (!zoom (CurrentDisplay) && !zoomDisplay (CurrentDisplay as IDisplay)) {
                var backend = CurrentDisplay as IDisplayBackend;
                if (backend != null)
                    zoomDisplay (backend.Frontend);
            }
        }

        // TODO: remove this dirty hack!:
        public Limaki.Usecases.Vidgets.ISplitView SplitView { get; set; }

        protected virtual void Compose () {

            Action<IDisplay, bool> selectAction = (display, value) => display.SelectAction.Enabled = value;
            Action<IDisplay, bool> panningAction = (display, value) => display.MouseScrollAction.Enabled = value;

            var actionGroup = new List<Action<IDisplay, bool>> ();
            actionGroup.Add (selectAction);
            actionGroup.Add (panningAction);
            Action<Action<IDisplay, bool>, bool> toogleAction = (ga, value) => {
                foreach (var a in actionGroup)
                    if (ga == a) DisplayAction (d => a (d, value)); else DisplayAction (d => a (d, !value));
            };

            EditCommand = new ToolStripCommand {
                Action = s => {
                    if (SplitView != null && SplitView.ContentVidget != null) {
                        SplitView.ContentVidget.SetFocus ();
                    }
                    // dirty hack:
                    if (SplitView != null && SplitView.ContentVidget is IMarkdownEdit) {
                        var md = (IMarkdownEdit)SplitView.ContentVidget;
                        if (md != null)
                            md.InEdit = !md.InEdit;
                        return;
                    }
                    // dirty hack end
                    toogleAction (selectAction, true);
                },
                Image = Iconery.Select,
                Size = DefaultSize,
                ToolTipText = "Edit"
            };

            PanningCommand = new ToolStripCommand {
                Action = s => toogleAction (panningAction, true),
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
                Action = s => ZoomAction (d => d.ZoomState = ZoomState.FitToWidth),
                Image = Iconery.FitToWidth,
                Size = DefaultSize,
                Label = "Fit to Width"
            };

            FitToHeigthCommand = new ToolStripCommand {
                Action = s => ZoomAction (d => d.ZoomState = ZoomState.FitToHeigth),
                Image = Iconery.FitToHeigth,
                Size = DefaultSize,
                Label = "Fit to Heigth",
            };

            FitToScreenCommand = new ToolStripCommand {
                Action = s => ZoomAction (d => d.ZoomState = ZoomState.FitToScreen),
                Image = Iconery.FitToScreen,
                Size = DefaultSize,
                Label = "Fit to Screen"
            };

            OriginalSizeCommand = new ToolStripCommand {
                Action = s => ZoomAction (d => d.ZoomState = ZoomState.Original),
                Image = Iconery.OriginalSize,
                Size = DefaultSize,
                Label = "Original size"
            };


            var selectButton = new ToolStripDropDownButton (EditCommand);
            selectButton.AddItems(
                new ToolStripButton (PanningCommand){ToggleOnClick = selectButton}
            );

            var zoomButton = new ToolStripDropDownButton (ZoomInOutCommand);
            zoomButton.AddItems (
                new ToolStripButton (FitToScreenCommand),
                new ToolStripButton (FitToWidthCommand),
                new ToolStripButton (FitToHeigthCommand),
                new ToolStripButton (OriginalSizeCommand)
            );

            this.AddItems (
               selectButton,
               zoomButton,
               new ToolStripSeparator ()
            );
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
                    var action = display.ActionDispatcher.GetAction<ZoomAction> ();
                    if (zoomIn)
                        action.ZoomIn ();
                    else
                        action.ZoomOut ();
                    display.Viewport.UpdateZoom ();
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
                zoomTarget.UpdateZoom ();
            }
        }

        public override void Detach (object sender) {

        }

        
    }

    public interface IDisplayModeToolStripBackend : IDisplayToolStripBackend { }
}