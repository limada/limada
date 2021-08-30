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
using System.Collections.Generic;
using System.IO;
using Limaki.Drawing;
using Limaki.View.Common;
using Xwt;
using Xwt.Backends;
using Xwt.Drawing;

namespace Limaki.View.Vidgets {

    [BackendType(typeof(ITextViewerVidgetBackend))]
    public class TextViewer : Vidget, IZoomTarget {

        ITextViewerVidgetBackend _backend = null;
        public virtual new ITextViewerVidgetBackend Backend {
            get {
                if (_backend == null) {
                    _backend = BackendHost.Backend as ITextViewerVidgetBackend;
                }
                return _backend;
            }
            set { _backend = value; }
        }

        public void Save (Stream stream, TextViewerTextType textType) {
            Backend.Save (stream, textType);
        }

        public void Load (Stream stream, TextViewerTextType textType) {
            Backend.Load (stream, textType);
        }

        public void SetAttribute (TextAttribute attribute) {
            Backend.SetAttribute (attribute);
        }

        public IEnumerable<TextAttribute> GetAttributes () {
            return Backend.GetAttributes ();
        }

        public event EventHandler SelectionChanged {
            add { Backend.SelectionChanged += value; }
            remove { Backend.SelectionChanged -= value; }
        }

        public override void Dispose () {}

        public ZoomState ZoomState {
            get { return Backend.ZoomState; }
            set { Backend.ZoomState = value; }
        }

        public double ZoomFactor {
            get { return Backend.ZoomFactor; }
            set { Backend.ZoomFactor = value; }
        }

        public void UpdateZoom () { Backend.UpdateZoom (); }

        public bool ReadOnly {
            get { return Backend.ReadOnly; }
            set { Backend.ReadOnly = value; }
        }

        public bool Modified {
            get { return Backend.Modified; }
            set { Backend.Modified = value; }
        }

        public void Clear () { Backend.Clear (); }

        public Point AutoScrollOffset {
            get { return Backend.AutoScrollOffset; }
            set { Backend.AutoScrollOffset = value; }
        }
    }
}