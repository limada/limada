﻿/*
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
using System.Collections.Generic;
using System.Windows.Forms;
using Limaki.Drawing;

namespace Limaki.Winform.Controls.ToolStrips {
    public partial class SceneToolStrip : ToolStrip {
        public SceneToolStrip() {
            InitializeComponent();
        }

        SceneToolsController _controller = null;
        public SceneToolsController Controller {
            get {
                if (_controller == null) {
                    _controller = new SceneToolsController (this);
                }
                return _controller;
            }
            set {
                _controller = value;
            }
        }

        List<ToolStripButton> _toolsGroup = null;
        List<ToolStripButton> toolsGroup {
            get {
                if (_toolsGroup == null) {
                    _toolsGroup = new List<ToolStripButton>();
                    _toolsGroup.Add(selectButton);
                    _toolsGroup.Add(moveButton);
                    _toolsGroup.Add(connectorButton);
                    _toolsGroup.Add(AddWidgetButton);
                }
                return _toolsGroup;
            }
        }

        void activateToolInGroup(object sender) {
            bool activated = false;
            foreach (ToolStripButton button in toolsGroup) {
                if (sender == button) {
                    button.Checked = !button.Checked;
                    activated = button.Checked;
                } else {
                    button.Checked = false;
                }
            }
            if (!activated) {
                //moveButton.Checked = true;
            }
        }

        public void Attach(bool select, bool move, bool connect, bool add) {
            this.selectButton.Checked = select;
            this.moveButton.Checked = move;

            this.connectorButton.Checked = connect;
            this.AddWidgetButton.Checked = add;
        }

        private void SelectOrMove(object sender, EventArgs e) {
            // checkOnClick is false cause of mono bug so we have to toggle ourself
            activateToolInGroup(sender);

            Controller.SelectOrMove (
                this.selectButton.Checked,
                this.moveButton.Checked,
                this.connectorButton.Checked,
                this.AddWidgetButton.Checked
                );
        }

        private void ZoomState(object sender, EventArgs e) {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ZoomState zoomState = Drawing.ZoomState.Original;
            if (menuItem == this.zoomMenuFittoHeigth) {
                zoomState = Drawing.ZoomState.FitToHeigth;
            } else if (menuItem == this.zoomMenuFitToScreen) {
                zoomState = Drawing.ZoomState.FitToScreen;
            } else if (menuItem == this.zoomMenuFittoWidth) {
                zoomState = Drawing.ZoomState.FitToWidth;
            } else if (menuItem == this.zoomMenuOriginalSize) {
                zoomState = Drawing.ZoomState.Original;
            }
            Controller.ZoomState(zoomState);
            foreach (ToolStripMenuItem item in zoomButton.DropDownItems) {
                if (menuItem != item) {
                    item.Checked = false;
                }
            }
        }

        private void ZoomInOut(object sender, MouseEventArgs e) {
            if (!zoomButton.DropDownButtonPressed) {
                if (e.Button == MouseButtons.Left)
                    Controller.ZoomInOut (true);
                else if (e.Button == MouseButtons.Right)
                    Controller.ZoomInOut (false);
                
            }
        }
    

        private void LayoutButton_Click(object sender, EventArgs e) {
            Controller.Layout ();
        }


    }
}
