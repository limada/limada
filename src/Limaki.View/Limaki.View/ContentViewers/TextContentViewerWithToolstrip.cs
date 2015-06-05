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
using Limaki.View.XwtBackend;

namespace Limaki.View.ContentViewers {

    public class TextContentViewerWithToolstrip : TextContentViewer {

        public override TextViewer TextViewer {
            get {
                return TextViewerWithToolstrip.TextViewer;
            }
        }
        public override IVidget Frontend {
            get { return TextViewerWithToolstrip; }
        }

        public static bool Available () {
            return VidgetToolkit.CurrentEngine.Backend.BackendRegistered<ITextViewerWithToolstripVidgetBackend>();
        }

        TextViewerWithToolstrip _textViewerWithToolstrip = null;
        TextViewerWithToolstrip TextViewerWithToolstrip {
            get {
                if (_textViewerWithToolstrip == null) {
                    _textViewerWithToolstrip = new TextViewerWithToolstrip ();
                }
                return _textViewerWithToolstrip;
            }
        }

        public override IVidgetBackend Backend { get { return TextViewerWithToolstrip.Backend; } }

        public override void SetContent (Content<Stream> content) {
            base.SetContent (content);
            if (this.ReadOnly)
                TextViewerWithToolstrip.ToolStrip.Visibility = Visibility.Hidden;
            else
                TextViewerWithToolstrip.ToolStrip.Visibility = Visibility.Visible;
        }
     
    }
}