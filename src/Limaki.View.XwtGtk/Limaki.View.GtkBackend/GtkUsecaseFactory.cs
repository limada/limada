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

using Limada.UseCases;
using Limaki.Usecases;
using Limaki.View.Viz.Visualizers.ToolStrips;
using Limaki.View.XwtBackend;
using Xwt.Backends;

namespace Limaki.View.GtkBackend {

    public class GtkUsecaseFactory : UsecaseFactory<ConceptUsecase> {

        public override void Compose (ConceptUsecase useCase) {
            var backendComposer = BackendComposer as IXwtConceptUseCaseComposer;
            AddToolbars (backendComposer.MainWindowBackend as Xwt.Window, useCase);

        }

        protected void AddToolbars (Xwt.Window xwtWindow, ConceptUsecase useCase) {

            var backend = xwtWindow.GetBackend () as Xwt.GtkBackend.WindowBackend;
            var toolBox = new Gtk.HBox (true, 2);

            var tb = useCase.ArrangerToolStrip.Backend as Gtk.Toolbar;
            toolBox.PackStart (tb, false, true, 0);

            tb = useCase.DisplayModeToolStrip.Backend as Gtk.Toolbar;
            toolBox.PackStart (tb, false, true, 0);

            var mainBox = backend.MainBox;
            mainBox.PackStart (toolBox, false, false, 0);
            
            ((Gtk.Box.BoxChild) mainBox[toolBox]).Position = 1;
            mainBox.ShowAll ();


        }

    }
}