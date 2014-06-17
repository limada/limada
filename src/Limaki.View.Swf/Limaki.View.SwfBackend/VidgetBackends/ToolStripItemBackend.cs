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

using System.Windows.Forms;
using Limaki.View.Vidgets;
using Xwt.GdiBackend;
using System;
using SWF = System.Windows.Forms;
using LVV = Limaki.View.Vidgets;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public abstract class ToolStripItemBackend<T> : IVidgetBackend, ISwfToolStripItemBackend where T : System.Windows.Forms.ToolStripItem {

        public abstract void InitializeBackend (IVidget frontend, VidgetApplicationContext context);

        public ToolStripItemBackend () {
            Compose ();
        }

        protected virtual void Compose () {
            this.Control = Activator.CreateInstance<T>();
        }

        public T Control { get; protected set; }

        public Xwt.Size Size {
            get { return Control.Size.ToXwt (); }
            set { Control.Size = value.ToGdi (); }
        }

        public virtual void Invalidate (Xwt.Rectangle rect) {
            Control.Invalidate (rect.ToGdi ());
        }

        public virtual void SetFocus () { }

        public virtual void Update () { }

        public virtual void Invalidate () {
            Control.Invalidate ();
        }

        public virtual void Dispose () {
            Control.Dispose ();
        }

        public virtual void SetImage (Xwt.Drawing.Image image) {
            Control.BackgroundImage = image.ToGdi ();
        }

        public virtual void SetLabel (string value) {
            Control.Text = value;
        }

        public virtual void SetToolTip (string value) {
            Control.ToolTipText = value;
        }

        protected System.Action<object> _action = null;
        public virtual void SetAction (System.Action<object> value) {
            _action = value;
        }

        protected virtual void ClickAction (object sender, System.EventArgs e) {
            if (_action != null) {
                _action (this);
            }
        }

        SWF.ToolStripItem ISwfToolStripItemBackend.Control {
            get { return this.Control; }
        }
    }

    public interface ISwfToolStripItemBackend {
        System.Windows.Forms.ToolStripItem Control { get; }
    }
}