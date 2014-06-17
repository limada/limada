/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limada.UseCases.Content;
using Limada.View.Vidgets;
using Limaki.Common;
using Limaki.View;
using Limaki.View.ContentViewers;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz.Visualizers;
using Limaki.View.Viz.Visualizers.ToolStrips;

namespace Limada.UseCases {

    public class ConceptUsecaseComposer : IComposer<ConceptUsecase> {

        public void Factor(ConceptUsecase useCase) {

            useCase.SplitView = new SplitView0();

            useCase.SheetManager = Registry.Factory.Create<ISheetManager>();
            useCase.VisualsDisplayHistory = new VisualsDisplayHistory ();

            useCase.GraphSceneUiManager = new ThingGraphUiManager {
                OpenFileDialog = new FileDialogMemento(),
                SaveFileDialog = new FileDialogMemento()
            };

            useCase.StreamContentUiManager = new StreamContentUiManager {
                OpenFileDialog = new FileDialogMemento(),
                SaveFileDialog = new FileDialogMemento()
            };

            useCase.FavoriteManager = new FavoriteManager();

            useCase.DisplayModeToolStrip = new DisplayModeToolStrip();
            useCase.ArrangerToolStrip = new ArrangerToolStrip();
            useCase.SplitViewToolStrip0 = new SplitViewToolStrip0();
            useCase.LayoutToolStrip0 = new LayoutToolStrip0();
            useCase.MarkerToolStrip = new MarkerToolStrip();

            useCase.FileDialogShow = this.FileDialogShow;

            Registry.Factory.Add<ContentViewerProvider, ContentVisualViewerProvider>();
        }

        public void Compose(ConceptUsecase useCase) {
            
            var splitView = useCase.SplitView;
            useCase.GetCurrentDisplay = () => splitView.CurrentDisplay;
            useCase.GetCurrentControl = () => splitView.CurrentWidget;

            useCase.SheetManager.SheetRegistered = sceneInfo => {
                useCase.VisualsDisplayHistory.Store(sceneInfo);
                //useCase.SplitViewToolStrip.Attach(splitView.CurrentDisplay);
            };
            splitView.VisualsDisplayHistory = useCase.VisualsDisplayHistory;
            splitView.SheetManager = useCase.SheetManager;
            
            splitView.FavoriteManager = useCase.FavoriteManager;
            useCase.FavoriteManager.SheetManager = useCase.SheetManager;

            useCase.SplitViewToolStrip0.SplitView = useCase.SplitView;
            useCase.SplitViewToolStrip0.SheetManager = useCase.SheetManager;

            splitView.CurrentWidgetChanged += c => useCase.DisplayModeToolStrip.Attach(c);
            splitView.CurrentWidgetChanged += c => useCase.LayoutToolStrip0.Attach(c);
            splitView.CurrentWidgetChanged += c => useCase.MarkerToolStrip.Attach(c);
            splitView.CurrentWidgetChanged += c => useCase.SplitViewToolStrip0.Attach(c);
            splitView.CurrentWidgetChanged += c => useCase.ArrangerToolStrip.Attach(c);
            
            useCase.DisplayStyleChanged += splitView.DoDisplayStyleChanged;

            splitView.Check();

            var fileManager = useCase.GraphSceneUiManager;
            fileManager.FileDialogShow = useCase.FileDialogShow;
            fileManager.MessageBoxShow = useCase.MessageBoxShow;

            fileManager.DataBound = scene => splitView.ChangeData(scene);
            fileManager.DataPostProcess = useCase.DataPostProcess;

            fileManager.Progress = useCase.Progress;
            fileManager.ApplicationQuit = useCase.ApplicationQuit;
            
            var streamManager = useCase.StreamContentUiManager;
            streamManager.FileDialogShow = useCase.FileDialogShow;
            streamManager.MessageBoxShow = useCase.MessageBoxShow;
            streamManager.Progress = useCase.Progress;
        }

        public DialogResult FileDialogShow (FileDialogMemento value, bool open) {
            FileDialogVidget fileDialog = null;
            if (open) {
                fileDialog = new OpenfileDialogVidget(value);
            } else
                fileDialog = new SavefileDialogVidget(value);
            var result = DialogResult.Cancel;
            if (fileDialog.Run())
                result = DialogResult.Ok;
            fileDialog.Dispose();

            return result;
        }
    }
}