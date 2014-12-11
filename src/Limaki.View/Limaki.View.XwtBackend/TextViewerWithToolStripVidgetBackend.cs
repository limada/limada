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

    public interface ITextViewerWithToolstripWidgetBackend : ITextViewerWithToolstrip, IWidgetBackend { }

    [BackendType (typeof (ITextViewerWithToolstripWidgetBackend))]
    public class TextViewerWithToolStripWidget : Xwt.Widget, IScrollContainingWidget {
        protected new ITextViewerWithToolstripWidgetBackend Backend {
            get { return base.Backend as ITextViewerWithToolstripWidgetBackend; }
        }

        public void SetTextViewer (TextViewer viewer) { Backend.SetTextViewer (viewer); }
        public void SetToolStrip (TextViewerToolStrip toolstrip) { Backend.SetToolStrip (toolstrip); }
    }

    public interface ITextViewerWithToolstripVidgetBackend : ITextViewerWithToolstrip, IVidgetBackend { }

    public class TextViewerWithToolStripVidgetBackend : VidgetBackend<TextViewerWithToolStripWidget>, ITextViewerWithToolstripVidgetBackend {

        public bool ToolStripVisible { get { return true; } set { } }

        protected TextViewerWithToolstrip Frontend { get; set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (TextViewerWithToolstrip) frontend;
            SetToolStrip (this.Frontend.ToolStrip);
            SetTextViewer (this.Frontend.TextViewer);
        }

        public void SetTextViewer (TextViewer viewer) { Widget.SetTextViewer (viewer); }

        public void SetToolStrip (TextViewerToolStrip toolstrip) { Widget.SetToolStrip (toolstrip); }
    }
}