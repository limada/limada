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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using Limaki.Common;
using Limaki.Viewers.ToolStripViewers;


namespace Limaki.Swf.Backends.Viewers.ToolStrips {

    public partial class DisplayToolStripBackend : ToolStripBackend, IDisplayToolStripBackend {
        public DisplayToolStripBackend() {
            InitializeComponent();
        }

        public override void InitializeBackend (Limaki.View.IVidget frontend, Limaki.View.VidgetApplicationContext context) {
            this.Strip = (DisplayToolStrip)frontend;
        }

        DisplayToolStrip _strip = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DisplayToolStrip Strip {
            get {
                if (_strip == null) {
                    _strip = new DisplayToolStrip ();
                    _strip.Backend = this;
                }
                return _strip;
            }
            set {
                _strip = value;
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
                    _toolsGroup.Add(AddVisualButton);
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
            this.AddVisualButton.Checked = add;
        }

        [TODO("implement detach")]
        public void Detach() {
            this.selectButton.CheckState = CheckState.Indeterminate;
            this.moveButton.CheckState = CheckState.Indeterminate;

            this.connectorButton.CheckState = CheckState.Indeterminate;
            this.AddVisualButton.CheckState = CheckState.Indeterminate;

        }
        
        private void SelectOrMove(object sender, EventArgs e) {
            // checkOnClick is false cause of mono bug so we have to toggle ourself
            activateToolInGroup(sender);

            Strip.SelectOrMove (
                this.selectButton.Checked,
                this.moveButton.Checked,
                this.connectorButton.Checked,
                this.AddVisualButton.Checked
                );
        }

        private void ZoomState(object sender, EventArgs e) {
            var menuItem = sender as ToolStripMenuItem;
            var zoomState = Drawing.ZoomState.Original;
            if (menuItem == this.zoomMenuFittoHeigth) {
                zoomState = Drawing.ZoomState.FitToHeigth;
            } else if (menuItem == this.zoomMenuFitToScreen) {
                zoomState = Drawing.ZoomState.FitToScreen;
            } else if (menuItem == this.zoomMenuFittoWidth) {
                zoomState = Drawing.ZoomState.FitToWidth;
            } else if (menuItem == this.zoomMenuOriginalSize) {
                zoomState = Drawing.ZoomState.Original;
            }
            Strip.ZoomState(zoomState);
            foreach (ToolStripMenuItem item in zoomButton.DropDownItems) {
                if (menuItem != item) {
                    item.Checked = false;
                }
            }
        }

        private void ZoomInOut(object sender, MouseEventArgs e) {
            if (!zoomButton.DropDownButtonPressed) {
                if (e.Button == MouseButtons.Left)
                    Strip.ZoomInOut (true);
                else if (e.Button == MouseButtons.Right)
                    Strip.ZoomInOut (false);
                
            }
        }
    

        private void LayoutButton_Click(object sender, EventArgs e) {
            Strip.Layout ();
        }

        

        
    }
}