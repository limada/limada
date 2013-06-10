/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */


using Limaki.Model.Content;
using Limaki.Viewers.Vidgets;
using Limaki.View;
using System.IO;

namespace Limaki.Viewers.StreamViewers {

    public class TextContentViewerWithToolstrip : TextContentViewer {

        public override TextViewer TextViewer {
            get {
                return TextViewerWithToolstrip.TextViewer;
            }
        }
        public override IVidget Frontend {
            get { return TextViewerWithToolstrip; }
        }

        TextViewerWithToolstrip _textViewerWithToolstrip = null;
        TextViewerWithToolstrip TextViewerWithToolstrip {
            get {
                if (_textViewerWithToolstrip == null) {
                    _textViewerWithToolstrip = new TextViewerWithToolstrip ();
                    this.OnAttachBackend(_textViewerWithToolstrip.Backend);
                }
                return _textViewerWithToolstrip;
            }
        }

        public override IVidgetBackend Backend { get { return TextViewerWithToolstrip.Backend; } }

        public override void SetContent(Content<Stream> content) {
            base.SetContent(content);
            TextViewerWithToolstrip.Backend.ToolStripVisible = !this.ReadOnly;
        }
      
    }
}