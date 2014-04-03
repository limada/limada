/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2010 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Drawing.Printing;
using Limaki.View;
using Limaki.View.Visuals;
using Limaki.View.Viz;

namespace Limada.Usecases {

    public class PrintManager {
        ImageExporter painter = null;

        public PrintDocument CreatePrintDocument(IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout) {
        
            this.painter = new ImageExporter(scene, layout);

            painter.Viewport.ClipOrigin = scene.Shape.Location;

            var doc = new PrintDocument();
            doc.PrintPage += new PrintPageEventHandler(doc_PrintPage);
            return doc;
        }

        void doc_PrintPage(object sender, PrintPageEventArgs e) {
            painter.Paint(e.Graphics, e.PageBounds);
            e.HasMorePages = false;
        }


    }
}