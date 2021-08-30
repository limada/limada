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

using Xwt;
using Xwt.Backends;

namespace Limaki.View.Vidgets {

    public interface IVindow : IVidget {
        IVidget Content { get; set; }
        string Title { get; set; }
        Xwt.CursorType Cursor { get; set; }
        void Show();
    }

    public interface IVindowBackend : IVidgetBackend {
        void SetContent (IVidget value);
        string Title { get; set; }
        Xwt.CursorType Cursor { get; set; }
        new Xwt.Size Size { get; set; }
        void Show ();
    }

    [BackendType (typeof (IVindowBackend))]
    public class Vindow : Vidget, IVindow {

        public Vindow () : base () { }
        public Vindow (IVindowBackend customBackend) : this() {
            BackendHost.SetCustomBackend (customBackend);
        }

        public virtual new IVindowBackend Backend { get { return BackendHost.Backend as IVindowBackend; } }

        IVidget _content = null;
        public IVidget Content {
            get { return _content; }
            set { _content = value;
                Backend.SetContent (value);
            }
        }

        public override void Dispose() { }

        public new Xwt.Size Size { get { return Backend.Size; } set { Backend.Size = value; } }

        public string Title { get => Backend.Title; set => Backend.Title = value; }

        public CursorType Cursor { get => Backend.Cursor; set => Backend.Cursor = value; }

        public void Show () {
            Backend.Show();
        }
    }
}