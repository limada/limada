/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2009-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Limaki.View.ContentViewers {

    public class ContentViewerProvider:IDisposable {

        protected ICollection<ContentViewer> _viewers = new List<ContentViewer>();

        public IEnumerable<ContentViewer> Viewers {
            get { return _viewers; }
        }

        public virtual ContentStreamViewer Supports(Int64 streamType) {
            return Viewers.OfType<ContentStreamViewer>().Where(v => v.Supports(streamType)).FirstOrDefault();
        }

        public virtual void Add (ContentViewer viewer) {
            this._viewers.Add(viewer);
        }

        public virtual void Dispose () {
            foreach (var viewer in this.Viewers) {
                viewer.Dispose ();
            }
        }

        public T Viewer<T> () where T : ContentViewer {
            return Viewers.OfType<T> ().FirstOrDefault ();
        }

        public virtual void Clear () {
            _viewers.Clear ();
        }
    }
}