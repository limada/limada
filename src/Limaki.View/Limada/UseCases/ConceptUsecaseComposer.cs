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
using Limaki.View.GraphScene;

namespace Limada.UseCases {

    public class ConceptUsecaseComposer : IComposer<ConceptUsecase> {

        IGraphSceneDisplayMesh<IVisual, IVisualEdge> _mesh = null;
        public IGraphSceneDisplayMesh<IVisual, IVisualEdge> Mesh { get { return _mesh ?? (_mesh = Registry.Pooled<IGraphSceneDisplayMesh<IVisual, IVisualEdge>> ()); } }

        public void Factor(ConceptUsecase useCase) {

            useCase.SplitView = new SplitView0();

            useCase.SheetManager = Registry.Factory.Create<ISheetManager>();
            useCase.VisualsDisplayHistory = new VisualsDisplayHistory ();

            useCase.GraphSceneUiManager = new ThingGraphUiManager {
                OpenFileDialog = new FileDialogMemento(),
                SaveFileDialog = new FileDialogMemento(),
                ThingGraphClosed = c => {
                    var h = Mesh.BackHandler<IThing, ILink> ();
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

            useCase.LayoutToolStrip = new LayoutToolStrip ();
            useCase.MarkerToolStrip = new MarkerToolStrip();

            useCase.FileDialogShow = this.FileDialogShow;

            Registry.Factory.Add<ContentViewerProvider, ContentVisualViewerProvider>();
        }

        public void Compose(ConceptUsecase useCase) {
            
            useCase.GetCurrentDisplay = () => useCase.SplitView.CurrentDisplay;
            useCase.GetCurrentVidget = () => useCase.SplitView.CurrentVidget;

            useCase.SheetManager.SheetRegistered = sceneInfo => {
                useCase.VisualsDisplayHistory.Store(sceneInfo);
                //useCase.SplitViewToolStrip.Attach(splitView.CurrentDisplay);
            };
            useCase.AskForVisualsDisplayHistorySaveChanges = true;
            useCase.SplitView.VisualsDisplayHistory = useCase.VisualsDisplayHistory;
            useCase.SplitView.SheetManager = useCase.SheetManager;
            
            useCase.SplitView.FavoriteManager = useCase.FavoriteManager;
            useCase.FavoriteManager.SheetManager = useCase.SheetManager;
            useCase.FavoriteManager.VisualsDisplayHistory = useCase.VisualsDisplayHistory;

            useCase.SplitViewToolStrip.SplitView = useCase.SplitView;
            useCase.SplitViewToolStrip.SheetManager = useCase.SheetManager;

            useCase.DisplayModeToolStrip.SplitView = useCase.SplitView;
            useCase.SplitView.CurrentVidgetChanged += c => useCase.DisplayModeToolStrip.Attach (c);
            useCase.SplitView.CurrentVidgetChanged += c => useCase.LayoutToolStrip.Attach (c);
            useCase.SplitView.CurrentVidgetChanged += c => useCase.MarkerToolStrip.Attach (c);
            useCase.SplitView.CurrentVidgetChanged += c => useCase.SplitViewToolStrip.Attach (c);
            useCase.SplitView.CurrentVidgetChanged += c => useCase.ArrangerToolStrip.Attach (c);

            useCase.DisplayStyleChanged += useCase.SplitView.DoDisplayStyleChanged;

            useCase.SplitView.Check();

            var fileManager = useCase.GraphSceneUiManager;
            fileManager.FileDialogShow = useCase.FileDialogShow;
            fileManager.MessageBoxShow = useCase.MessageBoxShow;

			fileManager.DataBound = () => {
				useCase.FavoriteManager.ResetHomeId();
				useCase.SplitView.ChangeData();
			};
            fileManager.DataPostProcess = useCase.DataPostProcess;

            fileManager.Progress = useCase.Progress;
            fileManager.ApplicationQuit = useCase.ApplicationQuit;
            
            var streamManager = useCase.StreamContentUiManager;
            streamManager.FileDialogShow = useCase.FileDialogShow;
            streamManager.MessageBoxShow = useCase.MessageBoxShow;
            streamManager.Progress = useCase.Progress;

            if (useCase.Toolbar == null) {
                useCase.Toolbar = new ToolbarPanel ();
                ComposeToolbar (useCase);
            }
        }

        public virtual void ComposeToolbar (ConceptUsecase useCase) {
            var toolbar = useCase.Toolbar;
            if (toolbar == null)
                return;

            toolbar.AddItems (
                useCase.SplitViewToolStrip,
                useCase.ArrangerToolStrip,
                useCase.DisplayModeToolStrip,
                useCase.MarkerToolStrip,
                useCase.LayoutToolStrip
            );
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