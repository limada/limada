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

    public interface ITextViewerWithToolbar {

        void SetTextViewer (TextViewer viewer);
        void SetToolbar (TextViewerToolbar toolbar);
    }

	public interface ITextViewerWithToolbarVidgetBackend : ITextViewerWithToolbar, IVidgetBackend { }

    [BackendType (typeof (ITextViewerWithToolbarVidgetBackend))]
    public class TextViewerWithToolbar : Vidget {

        public new virtual ITextViewerWithToolbarVidgetBackend Backend {
            get { return base.Backend as ITextViewerWithToolbarVidgetBackend; }
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

        private TextViewerToolbar _toolbar = null;
        public TextViewerToolbar Toolbar {
            get {
                if (_toolbar == null) {
                    _toolbar = new TextViewerToolbar ();
                    _toolbar.Attach (TextViewer);
                }
                return _toolbar;
            }
        }

        public override void Dispose () {
            
        }
    }
}