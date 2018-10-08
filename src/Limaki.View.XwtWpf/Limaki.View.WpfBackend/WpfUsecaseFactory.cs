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
using Limaki.View.WpfBackend;
using Limaki.View.XwtBackend;
using System.Windows.Controls;
using Xwt.Backends;
using System.Linq;
using System.Windows;
using Limada.Usecases;

namespace Limaki.View.WpfBackend {

    public class WpfUsecaseFactory : UsecaseFactory<ConceptUsecase> {
        
        public override void Compose(ConceptUsecase useCase) {
            var backendComposer = BackendComposer as IXwtBackendConceptUseCaseComposer;
            AddToolbars (backendComposer.MainWindow.Backend as Xwt.Window, useCase);

        }

        private void AddToolbars (Xwt.Window xwtWindow, ConceptUsecase useCase) {
            
            var backend = xwtWindow.GetBackend() as Xwt.WPFBackend.WindowBackend;
            var window = backend.Window;
            var grid = backend.Window.Content as Grid;

            var toolBarTray = new ToolBarTray();
            toolBarTray.Orientation = System.Windows.Controls.Orientation.Horizontal;
            Grid.SetColumn(toolBarTray, 0);
            Grid.SetRow(toolBarTray, 1);
            grid.Children.Add(toolBarTray);
            var toolRow = new RowDefinition() { Height = GridLength.Auto }; // Only take the toolRow requested space.
            grid.RowDefinitions.Insert(1, toolRow);

            var dockpanel = grid.Children.OfType<DockPanel>().FirstOrDefault();
            if (dockpanel != null) {
                Grid.SetColumn(dockpanel, 0);
                Grid.SetRow(dockpanel, 2);
            }

            if (useCase.Toolbar == null)
                return;

            var toolStrips = useCase.Toolbar.Items
                                 .Cast<Limaki.View.IVidget> ()
                                    .Select(t =>t.Backend.ToWpf() as ToolBar);
            
            foreach (var strip in toolStrips) { 
                toolBarTray.ToolBars.Add (strip);
            }

        }

        public class WpfToolbarPanelBackend : VidgetBackend<ToolBarTray> , Vidgets.IToolbarPanelBackend { 
            // TODO: 
        }

    }
}