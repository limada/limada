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
 * http://limada.sourceforge.net
 * 
 */


using System;
using System.Windows.Forms;

namespace Limaki.SWF.Viewers.ToolStripViewers {
    public class ToolStripShapeComboBox : ToolStripControlHost {
        public ToolStripShapeComboBox(): base(new ShapeComboBox()) {
            ShapeComboBoxControl.Margin = new Padding(0);
            ShapeComboBoxControl.FlatStyle = FlatStyle.Flat;
        }
        
        public ShapeComboBox ShapeComboBoxControl {
            get { return Control as ShapeComboBox; }
        }

        public override System.Drawing.Size Size {
            get {
                return base.Size;
            }
            set {
                if (ShapeComboBoxControl != null) {
                    if (ShapeComboBoxControl.ItemHeight!=value.Height-6)
                        ShapeComboBoxControl.ItemHeight = value.Height-6;
                    //ShapeComboBoxControl.DropDownWidth = value.Width - 15;
                }
                base.Size = value;
            }
        }
        // Subscribe and unsubscribe the control events you wish to expose.
        protected override void OnSubscribeControlEvents(Control c) {
            base.OnSubscribeControlEvents(c);
            ShapeComboBox control = (ShapeComboBox)c;
            control.SelectedIndexChanged += new EventHandler(OnSelectedIndexChanged);

        }

        protected override void OnUnsubscribeControlEvents(Control c) {
            base.OnUnsubscribeControlEvents(c);
            ShapeComboBox control = (ShapeComboBox)c;
            control.SelectedIndexChanged -= new EventHandler(OnSelectedIndexChanged);
        }


        public event EventHandler SelectedIndexChanged;

        private void OnSelectedIndexChanged(object sender, EventArgs e) {
            if (SelectedIndexChanged != null) {
                SelectedIndexChanged(this, e);
            }
        }

    }
}