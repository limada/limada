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
using Limada.UseCases;
using Limaki.Usecases;
using Limaki.View.WpfBackends;
using Limaki.View.XwtBackend;
using System.Windows.Controls;
using Xwt.Backends;
using System.Linq;
using System.Windows;

namespace Limaki.View.WpfBackend {

    public class WpfUsecaseFactory : UsecaseFactory<ConceptUsecase> {
        public override void Compose(ConceptUsecase useCase) {
            var backendComposer = BackendComposer as IXwtConceptUseCaseComposer;
            AddToolbars (backendComposer.MainWindowBackend as Xwt.Window, useCase);

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

            toolBarTray.ToolBars.Add(useCase.ArrangerToolStrip.Backend as ToolBar);
            toolBarTray.ToolBars.Add(useCase.SplitViewToolStrip.Backend as ToolBar);
            toolBarTray.ToolBars.Add(useCase.MarkerToolStrip.Backend as ToolBar);
            toolBarTray.ToolBars.Add(useCase.LayoutToolStrip.Backend as ToolBar);
            toolBarTray.ToolBars.Add(useCase.DisplayDisplayToolStrip.Backend as ToolBar);

        }

    }
}