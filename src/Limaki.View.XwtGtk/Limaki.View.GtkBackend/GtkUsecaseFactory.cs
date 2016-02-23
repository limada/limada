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
using System.Linq;
using Limaki.View.Viz.Visualizers.ToolStrips;
using Limaki.View.XwtBackend;
using Xwt.Backends;
using Limaki.Common.Linqish;
using System.Diagnostics;


namespace Limaki.View.GtkBackend {

    public class GtkUsecaseFactory : UsecaseFactory<ConceptUsecase> {

        public override void Compose (ConceptUsecase useCase) {
            var backendComposer = BackendComposer as IXwtConceptUseCaseComposer;
            AddToolbars (backendComposer.MainWindowBackend as Xwt.Window, useCase);

        }

        protected void AddToolbars (Xwt.Window xwtWindow, ConceptUsecase useCase) {

            var windowBackend = xwtWindow.GetBackend () as Xwt.GtkBackend.WindowBackend;
            var toolBox = new Gtk.HBox (false, 2);

            var tbs = new Gtk.Widget []{
                useCase.ArrangerToolStrip.Backend.ToGtk(),
                useCase.SplitViewToolStrip.Backend.ToGtk (),
                useCase.DisplayModeToolStrip.Backend.ToGtk (),
                useCase.MarkerToolStrip.Backend.ToGtk (),
                useCase.LayoutToolStrip.Backend.ToGtk (),
            };

            tbs.Cast<Gtk.Toolbar> ().ForEach (tb => {
                tb.ShowArrow = true;
                //tb.ResizeMode = Gtk.ResizeMode.Queue; //deprecated
 
                //tb.CheckResize ();
                //tb.ShowAll ();
                var w = 0;
                tb.Children.ForEach (c => {
                    var r = c.SizeRequest ();
                    w += r.Width;
                });

				// this depends on something strange; 
				// on cinnamon 8 is needed
				// otherwise 5
				tb.WidthRequest = w + 8; 
                tb.SizeRequested += (s, e) => {
                    
                };
                toolBox.PackStart (tb, false, false, 0);
                toolBox.SizeRequested += (s, e) => {
                    
                };
            });

            var mainBox = windowBackend.MainBox;
            mainBox.PackStart (toolBox, false, false, 0);
            
            ((Gtk.Box.BoxChild) mainBox[toolBox]).Position = 1;
            mainBox.ShowAll ();


        }

    }
}