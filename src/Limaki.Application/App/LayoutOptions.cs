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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Limaki;
using Limaki.Actions;
using Limaki.Widgets;
using Limaki.Widgets.Layout;

namespace Limaki.App {
    public partial class LayoutOptions : UserControl {
        public LayoutOptions() {
            InitializeComponent();
        }

        private ILayout _layout;
        private GraphLayout<Scene, IWidget> layout;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ILayout SceneLayout {
            get { return _layout; }
            set {
                layout = null;
                if (value is GraphLayout<Scene,IWidget>){
                    layout = (GraphLayout<Scene, IWidget>)value;
                    topBottomButton.Checked = layout.Orientation == Limaki.Widgets.Layout.Orientation.TopBottom;
                    leftRigthButton.Checked = layout.Orientation == Limaki.Widgets.Layout.Orientation.LeftRight;
                    centeredBox.Checked = layout.Centered;
                    BreathFirstButton.Checked = layout.Algo == Algo.BreathFirst;
                    DepthFirstButton.Checked = layout.Algo == Algo.DepthFirst;
                }
                _layout = value;

            }
        }

        private void leftRigthButton_CheckedChanged(object sender, EventArgs e) {
            if(layout != null) {
                if (leftRigthButton.Checked) {
                    layout.Orientation = Limaki.Widgets.Layout.Orientation.LeftRight;
                    topBottomButton.Checked = false;
                }
                if (topBottomButton.Checked) {
                    layout.Orientation = Limaki.Widgets.Layout.Orientation.TopBottom;
                    leftRigthButton.Checked = false;
                }
            }
        }

        private void centeredBox_CheckedChanged(object sender, EventArgs e) {
            if (layout != null) {
                layout.Centered = centeredBox.Checked;
            }
        }

        private void BreathFirstButton_CheckedChanged(object sender, EventArgs e) {
            if (layout != null) {
                if (BreathFirstButton.Checked) {
                    layout.Algo = Algo.BreathFirst;
                    DepthFirstButton.Checked = false;
                }
                if (DepthFirstButton.Checked) {
                    layout.Algo = Algo.DepthFirst;
                    BreathFirstButton.Checked = false;
                }
            }
        }
    }
}