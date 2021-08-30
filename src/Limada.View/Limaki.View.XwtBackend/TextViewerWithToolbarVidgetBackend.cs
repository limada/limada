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

using Limaki.View.Vidgets;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.XwtBackend {

    public interface ITextViewerWithToolbarWidgetBackend : ITextViewerWithToolbar, IWidgetBackend { }

    [BackendType (typeof (ITextViewerWithToolbarWidgetBackend))]
    public class TextViewerWithToolbarWidget : Xwt.Widget, IScrollContainingWidget {
        protected new ITextViewerWithToolbarWidgetBackend Backend {
            get { return base.Backend as ITextViewerWithToolbarWidgetBackend; }
        }

        public void SetTextViewer (TextViewer viewer) { Backend.SetTextViewer (viewer); }
        public void SetToolbar (TextViewerToolbar toolbar) { Backend.SetToolbar (toolbar); }
    }

	public class TextViewerWithToolbarVidgetBackend : VidgetBackend<TextViewerWithToolbarWidget>, ITextViewerWithToolbarVidgetBackend {

        public bool ToolbarVisible { get { return true; } set { } }

        public new TextViewerWithToolbar Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (TextViewerWithToolbar) frontend;
            SetToolbar (this.Frontend.Toolbar);
            SetTextViewer (this.Frontend.TextViewer);
        }

        public void SetTextViewer (TextViewer viewer) { Widget.SetTextViewer (viewer); }

        public void SetToolbar (TextViewerToolbar toolbar) { Widget.SetToolbar (toolbar); }
    }
}