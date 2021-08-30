/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

/*  this classes are adopted from: http://www.github.com/mono/xwt
    with the folowing license:
  
    Author: Lluis Sanchez <lluis@xamarin.com>
        
    Copyright (c) 2011-2012 Xamarin Inc
 
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:
 
    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.
 
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.
*/

using System;
using System.Collections.Generic;

namespace Limaki.View.Vidgets {
    
    public class VidgetBackendHost<T, B> : VidgetBackendHost where T : IVidget where B : IVidgetBackend {
        public new T Frontend {
            get { return (T)base.Frontend; }
            set { base.Frontend = value; }
        }

        public new B Backend {
            get { return (B)base.Backend; }
        }
    }

    public class VidgetBackendHost : IVidgetEventSink {

        public IVidget Frontend { get; internal set; }

        IVidgetBackend _backend;
        public IVidgetBackend Backend {
            get {
                LoadBackend ();
                return _backend;
            }
        }

        bool usingCustomBackend;
        public void SetCustomBackend (IVidgetBackend backend) {
            this._backend = backend;
            usingCustomBackend = true;
        }

        VidgetToolkit _engine;
        public VidgetToolkit ToolkitEngine {
            get { return _engine ?? (_engine = VidgetToolkit.CurrentEngine); }
            protected set { _engine = value; }
        }

        public VidgetToolkitEngineBackend EngineBackend {
            get { return ToolkitEngine.Backend; }
        }

        public bool BackendCreated {
            get { return _backend != null; }
        }

        protected virtual void OnBackendCreated () {
            if (_backend != null)
                _backend.InitializeEvents (this);
        }

        protected virtual IVidgetBackend OnCreateBackend () {
            return EngineBackend.CreateBackendForFrontend (Frontend.GetType ());
        }

        public void EnsureBackendLoaded () {
            if (_backend == null)
                LoadBackend ();
        }

        protected virtual void LoadBackend () {
            if (usingCustomBackend) {
                usingCustomBackend = false;
                _backend.InitializeBackend (Frontend, ToolkitEngine.Context);
                OnBackendCreated ();
            } else if (_backend == null) {
                _backend = OnCreateBackend ();
                if (_backend == null)
                    throw new InvalidOperationException ("No backend found for object: " + Frontend.GetType ());
                _backend.InitializeBackend (Frontend, _engine.Context);
                OnBackendCreated ();
            }
        }

        IDictionary<string, EventHandler> _events = new Dictionary<string, EventHandler> ();

        public void AddEvent (string name, EventHandler h) {
            _events [name] = h;
        }

        public void RemoveEvent (string name, EventHandler h) {
            _events.Remove (name);
        }

        public void OnEvent<T> (string name, T args) where T:EventArgs {
            EventHandler result = null;
            _events.TryGetValue (name, out result);
            result?.Invoke (Frontend, args);
        }
    }
}
