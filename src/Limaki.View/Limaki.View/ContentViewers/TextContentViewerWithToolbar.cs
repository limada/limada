/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2014 Lytico
 *
 * http://www.limada.org
 * 
 */


using System.IO;
using Limaki.Contents;
using Limaki.View.Vidgets;

namespace Limaki.View.ContentViewers {

    public class TextContentViewerWithToolbar : TextContentViewer {

        public override TextViewer TextViewer {
            get {
                return TextViewerWithToolbar.TextViewer;
            }
        }
        public override IVidget Frontend {
            get { return TextViewerWithToolbar; }
        }

        public static new bool Available () {
            return VidgetToolkit.CurrentEngine.Backend.BackendRegistered<ITextViewerWithToolbarVidgetBackend> ();
        }

        TextViewerWithToolbar _textViewerWithToolbar = null;
        TextViewerWithToolbar TextViewerWithToolbar {
            get {
                if (_textViewerWithToolbar == null) {
                    _textViewerWithToolbar = new TextViewerWithToolbar ();
                }
                return _textViewerWithToolbar;
            }
        }

        public override IVidgetBackend Backend { get { return TextViewerWithToolbar.Backend; } }

        public override void SetContent (Content<Stream> content) {
            base.SetContent (content);
            if (this.ReadOnly)
                TextViewerWithToolbar.Toolbar.Visibility = Visibility.Hidden;
            else
                TextViewerWithToolbar.Toolbar.Visibility = Visibility.Visible;
        }

    }
}