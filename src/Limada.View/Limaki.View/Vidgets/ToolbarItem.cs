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

    [BackendType (typeof (IToolbarItemBackend))]
    public class ToolbarItem : Vidget, ICommandView {

        public ToolbarItem () { }

        public ToolbarItem (ICommandView command) {
            this.SetCommand (command);
        }

        private IToolbarItemBackend _backend = null;
        public new virtual IToolbarItemBackend Backend {
            get { return _backend ?? (_backend = BackendHost.Backend as IToolbarItemBackend); }
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
        public override string ToolTipText {
            get { return _toolTipText; }
            set {
                if (_toolTipText != value) {
                    _toolTipText = value;
                    Backend.ToolTipText = value;
                }
            }
        }

        public new Size Size { get; set; }
        public bool Enabled { get; set; }

        public void SetCommand (ICommandView command) {
            VidgetUtils.SetCommand (this, command); 
        }
    }

    public interface IToolbarItemContainer : IItemContainer<ToolbarItem> {}
        
    public class ToolbarItemCollection : ContainerItemCollection<ToolbarItem> {
        public ToolbarItemCollection (IItemContainer<ToolbarItem> parent) : base (parent) { }
    }

    public interface IToolbarItemBackend : IVidgetBackend {

        void SetImage (Image image);

        void SetLabel (string value);

        void SetAction (Action<object> action);

        bool IsEnabled { get; set; }
    }

}