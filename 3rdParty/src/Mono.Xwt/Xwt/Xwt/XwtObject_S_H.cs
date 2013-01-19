// 
// XwtHandledObject.cs
//  
// Author:
//       Lytico (http://www.limada.org)
// 
// Copyright (c) 2011 Xamarin Inc
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Xwt.Backends;
using Xwt.Engine;

namespace Xwt {
    
    /// <summary>
    /// XwtObject with strong typed BackendHandler
    /// and reference to a WidgetRegistry per object
    /// to enable usage of mixing different Backend-Engines
    /// </summary>
    /// <typeparam name="S">self reference, needed for type in static methods</typeparam>
    /// <typeparam name="H">the handler type</typeparam>
    public abstract class XwtObject<S, H> : XwtObject, IXwtHandledObject, IFrontend
        where S : XwtObject
        where H : class,IBackendHandler {

        protected XwtObject (WidgetRegistry registry) {
            this.Registry = registry;
            this.handler = GetBackendHandler(registry);
        }

        public XwtObject (WidgetRegistry registry, object backend): base(backend) {
            this.Registry = registry;
            this.handler = GetBackendHandler(registry);
            if (backend == null)
                throw new ArgumentNullException("backend");

        }

        // TODO: rename to Handler or something else in a further step
        /// <summary>
        /// strong typed BackendHandler
        /// </summary>
        protected virtual H handler { get; set; }

        protected static H GetBackendHandler (WidgetRegistry registry) {
            return registry.CreateSharedBackend<H>(typeof(S));
        }

        protected override IBackendHandler BackendHandler {
            get {
                return handler;
            }
        }

        public WidgetRegistry Registry { get; protected set; }

        WidgetRegistry IFrontend.Registry {
            get { return this.Registry; }
        }
    }

    /// <summary>
    /// a marker interface for XwtObject_S_T
    /// </summary>
    public interface IXwtHandledObject { }
}