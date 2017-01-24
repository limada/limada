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
using Limaki.View.Viz.Mesh;
using Limada.Model;
using Limaki.View.GraphScene;
using Limaki.View.Viz.Visualizers.Toolbars;

namespace Limada.UseCases {

    public class ConceptUsecaseComposer : IComposer<ConceptUsecase> {

        IGraphSceneDisplayMesh<IVisual, IVisualEdge> _mesh = null;
        public IGraphSceneDisplayMesh<IVisual, IVisualEdge> Mesh { get { return _mesh ?? (_mesh = Registry.Pooled<IGraphSceneDisplayMesh<IVisual, IVisualEdge>> ()); } }

        public void Factor(ConceptUsecase useCase) {

            useCase.SplitView = new SplitView0();

            useCase.SceneManager = Registry.Factory.Create<ISceneManager> ();
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

            useCase.DisplayModeToolbar = new DisplayModeToolbar();
            useCase.ArrangerToolbar = new ArrangerToolbar();
            useCase.SplitViewToolbar = new SplitViewToolbar();

            useCase.LayoutToolbar = new LayoutToolbar ();
            useCase.MarkerToolbar = new MarkerToolbar();

            useCase.FileDialogShow = this.FileDialogShow;

            Registry.Factory.Add<ContentViewerProvider, ContentVisualViewerProvider>();
        }


        public void Compose(ConceptUsecase useCase) {
            
            useCase.GetCurrentDisplay = () => useCase.SplitView.CurrentDisplay;
            useCase.GetCurrentVidget = () => useCase.SplitView.CurrentVidget;

            useCase.SceneManager.SheetStore.SceneInfoRegistered = sceneInfo => {
                useCase.VisualsDisplayHistory.Store (sceneInfo);
                //useCase.SplitViewToolbar.Attach(splitView.CurrentDisplay);
            };
            useCase.AskForVisualsDisplayHistorySaveChanges = true;
            useCase.SplitView.VisualsDisplayHistory = useCase.VisualsDisplayHistory;
            useCase.SplitView.SceneManager = useCase.SceneManager;
            
            useCase.SplitView.FavoriteManager = useCase.FavoriteManager;
            useCase.FavoriteManager.SceneManager = useCase.SceneManager;
            useCase.FavoriteManager.VisualsDisplayHistory = useCase.VisualsDisplayHistory;

            useCase.SplitViewToolbar.SplitView = useCase.SplitView;
            useCase.SplitViewToolbar.SceneManager = useCase.SceneManager;

            useCase.DisplayModeToolbar.SplitView = useCase.SplitView;
            useCase.SplitView.CurrentVidgetChanged += c => useCase.DisplayModeToolbar.Attach (c);
            useCase.SplitView.CurrentVidgetChanged += c => useCase.LayoutToolbar.Attach (c);
            useCase.SplitView.CurrentVidgetChanged += c => useCase.MarkerToolbar.Attach (c);
            useCase.SplitView.CurrentVidgetChanged += c => useCase.SplitViewToolbar.Attach (c);
            useCase.SplitView.CurrentVidgetChanged += c => useCase.ArrangerToolbar.Attach (c);

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
                useCase.SplitViewToolbar,
                useCase.ArrangerToolbar,
                useCase.DisplayModeToolbar,
                useCase.MarkerToolbar,
                useCase.LayoutToolbar
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