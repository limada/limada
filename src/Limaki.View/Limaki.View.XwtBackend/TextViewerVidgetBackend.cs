
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
using Limaki.View.Common;
using Limaki.View.Vidgets;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {

    public class TextViewerVidgetBackend : VidgetBackend<TextViewerWidget>, ITextViewerVidgetBackend {
        
        public new TextViewer Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (TextViewer) frontend;
        }

        public bool ReadOnly {
            get { return Widget.ReadOnly; }
            set { Widget.ReadOnly = value; }
        }

        public bool Modified {
            get { return Widget.Modified; }
            set { Widget.Modified = value; }
        }

        public Point AutoScrollOffset {
            get { return Widget.AutoScrollOffset; }
            set { Widget.AutoScrollOffset = value; }
        }

        public void Save (Stream stream, TextViewerTextType textType) {
            Widget.Save (stream, textType);
        }

        public void Load (Stream stream, TextViewerTextType textType) {
            Widget.Load (stream, textType);
        }

        public void Clear () { Widget.Clear (); }

        public void SetAttribute (TextAttribute attribute) {
            Widget.SetAttribute (attribute);
        }

        public IEnumerable<TextAttribute> GetAttributes () {
            return Widget.GetAttributes ();
        }

        public event EventHandler SelectionChanged {
            add { Widget.SelectionChanged += value; }
            remove { Widget.SelectionChanged -= value; }
        }

        public VidgetBorderStyle BorderStyle { get; set; }

        public ZoomState ZoomState { get; set; }

        public double ZoomFactor { get; set; }

        public void UpdateZoom () {
            
        }
    }
}