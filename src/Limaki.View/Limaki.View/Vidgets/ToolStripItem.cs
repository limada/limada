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
using Xwt;
using Xwt.Backends;
using Xwt.Drawing;

namespace Limaki.View.Vidgets {

    [BackendType (typeof (IToolStripItemBackend))]
    public class ToolStripItem : Vidget, IToolStripCommand {

        public ToolStripItem () { }

        public ToolStripItem (IToolStripCommand command) {
            this.SetCommand (command);
        }

        private IToolStripItemBackend _backend = null;
        public new virtual IToolStripItemBackend Backend {
            get { return _backend ?? (_backend = BackendHost.Backend as IToolStripItemBackend); }
            set { _backend = value; }
        }

        public override void Dispose () { }

        protected Action<object> _action = null;
        public virtual Action<object> Action {
            get { return _action; }
            set {
                if (_action != value) {
                    _action = value;
                    SetBackendAction (value);
                }
            }
        }

        protected virtual void SetBackendAction (Action<object> value) {
            Backend.SetAction (value);
        }

        protected Image _image = null;
        public virtual Image Image {
            get { return _image; }
            set {
                if (_image != value) {
                    _image = value;
                    Backend.SetImage (value);
                }
            }
        }

        protected string _label = null;
        public virtual string Label {
            get { return _label; }
            set {
                if (_label != value) {
                    _label = value;
                    Backend.SetLabel (value);
                }
            }
        }

        protected string _toolTipText = null;
        public virtual string ToolTipText {
            get { return _toolTipText; }
            set {
                if (_toolTipText != value) {
                    _toolTipText = value;
                    Backend.SetToolTip (value);
                }
            }
        }

        public Size Size { get; set; }

        public void SetCommand (IToolStripCommand command) {
            VidgetUtils.SetCommand (this, command); 
        }
    }

    public interface IToolStripItemContainer {
        void InsertItem (int index, ToolStripItem item);
        void RemoveItem (ToolStripItem item);
    }

    public interface IToolStripItemBackend : IVidgetBackend {

        void SetImage (Image image);

        void SetLabel (string value);

        void SetToolTip (string value);

        void SetAction (Action<object> action);
    }

}