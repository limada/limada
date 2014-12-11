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


using Limaki.Contents;
using System.IO;
using Limaki.View.Vidgets;

namespace Limaki.View.ContentViewers {
    public class TextContentViewerWithToolstrip0 : TextContentViewer {

        public override TextViewer TextViewer {
            get {
                return TextViewerWithToolstrip.TextViewer;
            }
        }
        public override IVidget Frontend {
            get { return TextViewerWithToolstrip; }
        }

        TextViewerWithToolstrip0 _textViewerWithToolstrip = null;
        TextViewerWithToolstrip0 TextViewerWithToolstrip {
            get {
                if (_textViewerWithToolstrip == null) {
                    _textViewerWithToolstrip = new TextViewerWithToolstrip0 ();
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