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

using Limaki.View.Vidgets;
using Xwt.GdiBackend;
using System;
using SWF = System.Windows.Forms;
using LVV = Limaki.View.Vidgets;

namespace Limaki.View.SwfBackend.VidgetBackends {

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// cannot be refactored to VidgetBackend as ToolbarItem is not a Control
    /// </remarks>
    public abstract class ToolStripItemBackend<T> : IVidgetBackend, ISwfToolStripItemBackend where T : SWF.ToolStripItem {

        public IVidget Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = Frontend;
        }

        IVidgetEventSink EventSink { get; set; }
        public void InitializeEvents (IVidgetEventSink eventSink) {
            EventSink = eventSink;
        }

        public ToolStripItemBackend () {
            Compose ();
        }

        public T Control { get; protected set; }

        protected virtual void Compose () {
            this.Control = Activator.CreateInstance<T>();
        }

        public Xwt.Size Size {
            get { return Control.Size.ToXwt (); }
            set { Control.Size = value.ToGdi (); }
        }

        public string ToolTipText {
            get { return Control.ToolTipText as string; }
            set { Control.ToolTipText = value; }
        }

        public virtual void QueueDraw (Xwt.Rectangle rect) {
            Control.Invalidate (rect.ToGdi ());
        }

        public virtual void SetFocus () { }

        public virtual void Update () { }

        public virtual void QueueDraw () {
            Control.Invalidate ();
        }

        public virtual void Dispose () {
            Control.Dispose ();
        }

        public bool IsEnabled {
            get { return Control.Enabled; }
            set { Control.Enabled = value; }
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