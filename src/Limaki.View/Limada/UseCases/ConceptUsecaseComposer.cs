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

using Limada.UseCases.Contents;
using Limada.View.Vidgets;
using Limaki.Common;
using Limaki.View;
using Limaki.View.ContentViewers;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz.Visualizers;
using Limaki.View.Viz.Visualizers.ToolStrips;
using Limaki.View.Viz.Mesh;
using Limada.Model;

namespace Limada.UseCases {

    public class ConceptUsecaseComposer : IComposer<ConceptUsecase> {

        IGraphSceneMesh<IVisual, IVisualEdge> _mesh = null;
        public IGraphSceneMesh<IVisual, IVisualEdge> Mesh { get { return _mesh ?? (_mesh = Registry.Pooled<IGraphSceneMesh<IVisual, IVisualEdge>> ()); } }

        public void Factor(ConceptUsecase useCase) {

            useCase.SplitView = new SplitView0();

            useCase.SheetManager = Registry.Factory.Create<ISheetManager>();
            useCase.VisualsDisplayHistory = new VisualsDisplayHistory ();

            useCase.GraphSceneUiManager = new ThingGraphUiManager {
                OpenFileDialog = new FileDialogMemento(),
                SaveFileDialog = new FileDialogMemento(),
                ThingGraphClosed = c => {
                    var h = Mesh.BackHandler<IThing, ILink> () as GraphSceneMeshBackHandler<IVisual, IThing, IVisualEdge, ILink>;
                    if (h != null) {
                        h.UnregisterBackGraph (c.Data, true);
                    }
                }
            };

            useCase.StreamContentUiManager = new StreamContentUiManager {
                OpenFileDialog = new FileDialogMemento(),
                SaveFileDialog = new FileDialogMemento()
            };

            useCase.FavoriteManager = new FavoriteManager();

            useCase.DisplayModeToolStrip = new DisplayModeToolStrip();
            useCase.ArrangerToolStrip = new ArrangerToolStrip();
            useCase.SplitViewToolStrip = new SplitViewToolStrip();
            useCase.LayoutToolStrip0 = new LayoutToolStrip0();
            useCase.MarkerToolStrip = new MarkerToolStrip();

            useCase.FileDialogShow = this.FileDialogShow;

            Registry.Factory.Add<ContentViewerProvider, ContentVisualViewerProvider>();
        }

        public void Compose(ConceptUsecase useCase) {
            
            var splitView = useCase.SplitView;
            useCase.GetCurrentDisplay = () => splitView.CurrentDisplay;
            useCase.GetCurrentVidget = () => splitView.CurrentVidget;

            useCase.SheetManager.SheetRegistered = sceneInfo => {
                useCase.VisualsDisplayHistory.Store(sceneInfo);
                //useCase.SplitViewToolStrip.Attach(splitView.CurrentDisplay);
            };
            useCase.AskForVisualsDisplayHistorySaveChanges = true;
            splitView.VisualsDisplayHistory = useCase.VisualsDisplayHistory;
            splitView.SheetManager = useCase.SheetManager;
            
            splitView.FavoriteManager = useCase.FavoriteManager;
            useCase.FavoriteManager.SheetManager = useCase.SheetManager;
            useCase.FavoriteManager.VisualsDisplayHistory = useCase.VisualsDisplayHistory;

            useCase.SplitViewToolStrip.SplitView = useCase.SplitView;
            useCase.SplitViewToolStrip.SheetManager = useCase.SheetManager;

            useCase.DisplayModeToolStrip.SplitView = splitView;
            splitView.CurrentVidgetChanged += c => useCase.DisplayModeToolStrip.Attach (c);
            splitView.CurrentVidgetChanged += c => useCase.LayoutToolStrip0.Attach (c);
            splitView.CurrentVidgetChanged += c => useCase.MarkerToolStrip.Attach (c);
            splitView.CurrentVidgetChanged += c => useCase.SplitViewToolStrip.Attach (c);
            splitView.CurrentVidgetChanged += c => useCase.ArrangerToolStrip.Attach (c);

            useCase.DisplayStyleChanged += splitView.DoDisplayStyleChanged;

            splitView.Check();

            var fileManager = useCase.GraphSceneUiManager;
            fileManager.FileDialogShow = useCase.FileDialogShow;
            fileManager.MessageBoxShow = useCase.MessageBoxShow;

			fileManager.DataBound = scene => {
				useCase.FavoriteManager.ResetHomeId();
				splitView.ChangeData(scene);
			};
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