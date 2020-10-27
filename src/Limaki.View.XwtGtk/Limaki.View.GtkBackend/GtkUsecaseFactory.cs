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

using Limada.Usecases;
using Limaki.Usecases;
using System.Linq;
using Limaki.View.XwtBackend;
using Xwt.Backends;
using Limaki.Common.Linqish;
using System.Diagnostics;
using System;
using Limaki.Common;

namespace Limaki.View.GtkBackend {

    public class GtkUsecaseFactory : UsecaseFactory<ConceptUsecase> {

        public override void Compose (ConceptUsecase useCase) {
            
            var backendComposer = BackendComposer as IXwtBackendConceptUseCaseComposer;

            var xwtWindow = backendComposer.MainWindow.Backend as Xwt.Window;
            ComposeWindow (xwtWindow);
            (useCase.Toolbar.Backend as Vidgets.IToolbarPanelBackend).AddToWindow (backendComposer.MainWindow);
        }

        protected void ComposeWindow (Xwt.Window xwtWindow) {
            
            var windowBackend = xwtWindow.GetBackend () as Xwt.GtkBackend.WindowBackend;

            // set minimum size:
            var minSize = Registry.Pooled<Limaki.Drawing.IDrawingUtils> ().GetTextDimension ("File Edit Style", null);

#if XWT_GTKSHARP3
            windowBackend.SetMinSize (minSize);
#else            
            windowBackend.Window.SizeRequested += (s, e) => {
                var req = e.Requisition;
                var size = new Xwt.Size (e.Requisition.Width, e.Requisition.Height);
                Trace.WriteLine ($"{nameof (windowBackend)}.{nameof (windowBackend.Window.SizeRequested)}:\t{size}");
                req.Width = (int)minSize.Width;
                e.Requisition = req;
            };
#endif           
        }

        protected void AddToolbar (Xwt.Window xwtWindow, Vidgets.ToolbarPanel toolbar) {

            if (toolbar == null)
                return;

           

        }
    }
}