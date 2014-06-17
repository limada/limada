/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.View.Vidgets;

namespace Limaki.View.GtkBackend {

    public abstract class ToolStripItemBackend<T> : VidgetBackend<T> where T:ToolItem, new() {

        protected override void Compose () {
            base.Compose ();
            Widget.Click += this.OnButtonClicked;
        }

        public void SetLabel (string value) {
            Widget.Label = value;
        }

        public void SetToolTip (string value) {
            Widget.TooltipText = value;
        }

        private Action<object> _action;
        public void SetAction (Action<object> value) {
            _action = value;
        }

        protected virtual void OnButtonClicked (object sender, EventArgs e) {
            if (_action != null)
                _action (this);
        }

        protected bool _composed = false;
        public virtual void SetImage (Xwt.Drawing.Image image) {
            Widget.Image = image;
            if (!_composed) {
                Widget.Compose ();
                _composed = true;
            }
        }
    }
}