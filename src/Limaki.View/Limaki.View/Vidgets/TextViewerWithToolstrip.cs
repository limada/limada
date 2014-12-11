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

using Limaki.View.XwtBackend;
using Xwt.Backends;

namespace Limaki.View.Vidgets {

    public interface ITextViewerWithToolstrip {

        void SetTextViewer (TextViewer viewer);
        void SetToolStrip (TextViewerToolStrip toolstrip);
    }

    [BackendType (typeof (ITextViewerWithToolstripVidgetBackend))]
    public class TextViewerWithToolstrip : Vidget {

        public new virtual ITextViewerWithToolstripVidgetBackend Backend {
            get { return base.Backend as ITextViewerWithToolstripVidgetBackend; }
        }

        private TextViewer _textViewer = null;
        public TextViewer TextViewer {
            get {
                if (_textViewer == null) {
                    _textViewer = new TextViewer ();
                }
                return _textViewer;
            }
        }

        private TextViewerToolStrip _toolStrip = null;
        public TextViewerToolStrip ToolStrip {
            get {
                if (_toolStrip == null) {
                    _toolStrip = new TextViewerToolStrip ();
                    _toolStrip.Attach (TextViewer);
                }
                return _toolStrip;
            }
        }

        public override void Dispose () {
            
        }
    }
}