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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Limaki.Drawing.GDI;
using Limaki.Common;

namespace Limaki.Winform.Viewers {
    public partial class BaseStyleEditor<T> : UserControl 
    where T:class {
        public BaseStyleEditor() {
            InitializeComponent();
        }

        public event EventHandler<EventArgs<T>> PropertyValueChanged;

        protected bool disableChanges = false;
        protected void DoPropertyValueChanged() {
            if (PropertyValueChanged != null && ! disableChanges) {
                PropertyValueChanged (this, new EventArgs<T>(SelectedObject));
            }
        }
        protected T _selectedObject = null;
        public virtual T SelectedObject {
            get {
                if (_selectedObject == null) {

                }
                return _selectedObject;
            }
            set {
                _selectedObject = value;
                SetEditorFromLayout(_selectedObject);
            }
        }

        protected void SetColor(Color color, Button colorButton, NumericUpDown colorTransparency) {
            colorButton.BackColor = color; //Color.FromArgb(255, color);
            colorTransparency.Value = color.A;
            DoPropertyValueChanged ();
        }
        protected virtual void SetEditorFromLayout(T selectedObject) {}
    }
}