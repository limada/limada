/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System.Drawing.Printing;
using Limaki.Winform.Displays;
using Limaki.Drawing;
using Limaki.Drawing.UI;
using Limaki.Widgets;
using Limaki.Drawing.GDI.UI;

namespace Limaki.Winform.Displays {
    public class PrintManager {
        GDIImageExporter painter = null;
        public PrintDocument CreatePrintDocument(WidgetDisplay display) {
            this.painter = 
                new GDIImageExporter(display.Data,display.DataLayout);

            PrintDocument doc = new PrintDocument ();
            doc.PrintPage += new PrintPageEventHandler(doc_PrintPage);
            return doc;
        }

        void doc_PrintPage(object sender, PrintPageEventArgs e) {
            painter.Paint (e.Graphics, e.PageBounds);
            e.HasMorePages = false;
        }
    }
}