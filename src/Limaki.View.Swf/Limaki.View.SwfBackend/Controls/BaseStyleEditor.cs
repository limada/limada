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
using System.Drawing;
using System.Windows.Forms;
using Limaki.Common;

namespace Limaki.View.SwfBackend.Controls {
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