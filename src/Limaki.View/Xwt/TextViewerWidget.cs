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
using Limaki.View.Vidgets;
using Xwt.Backends;
using Xwt.Drawing;
namespace Xwt {

    public interface ITextViewerWidgetBackend : IWidgetBackend, ITextViewerBackend { }

    [BackendType (typeof (ITextViewerWidgetBackend))]
    public class TextViewerWidget : Xwt.Widget, ITextViewerBackend {

        protected new ITextViewerWidgetBackend Backend {
            get { return base.Backend as ITextViewerWidgetBackend; }
        }

        public bool ReadOnly {
            get { return Backend.ReadOnly; } 
            set { Backend.ReadOnly = value; }
        }

        public bool Modified {
            get { return Backend.Modified; } 
            set { Backend.Modified = value; }
        }

        public Point AutoScrollOffset {
            get { return Backend.AutoScrollOffset; } 
            set { Backend.AutoScrollOffset = value; }
        }

        public void Save (Stream stream, TextViewerTextType textType) {
            Backend.Save (stream, textType);
        }

        public void Load (Stream stream, TextViewerTextType textType) {
            Backend.Load (stream, textType);
        }

        public void Clear () {
            Backend.Clear ();
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
    }


}