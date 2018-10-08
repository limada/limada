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

using Limada.Usecases.Contents;
using Limada.View.Vidgets;
using Limaki.Common;
using Limaki.View;
using Limaki.View.ContentViewers;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz.Visualizers;
using Limaki.View.Viz.Mapping;
using Limada.Model;
using Limaki.View.GraphScene;
using Limaki.View.Viz.Visualizers.Toolbars;
using Xwt;
using System.Linq;
using Limaki.View.Common;

namespace Limada.Usecases {

    public class ConceptUsecaseComposer : IComposer<ConceptUsecase> {

        IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> _organizer = null;
        public IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> Organizer { get { return _organizer ?? (_organizer = Registry.Pooled<IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge>> ()); } }

        public void Factor(ConceptUsecase useCase) {

            useCase.SplitView = new SplitView0();

            useCase.SceneManager = Registry.Factory.Create<IVisualSceneStoreInteractor> ();
            useCase.VisualsDisplayHistory = new VisualsDisplayHistory ();

            useCase.GraphSceneUiManager = new ThingGraphUiManager {
                OpenFileDialog = new FileDialogMemento(),
                SaveFileDialog = new FileDialogMemento(),
                ThingGraphClosed = c => {
                    var h = Organizer.MapInteractor<IThing, ILink> ();
                    if (h != null) {
                        h.UnregisterMappedGraph (c.Data, true);
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

            useCase.Toolbar = new ToolbarPanel ();
            useCase.Menu = new Menu ();

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

            ComposeMenu (useCase);

            ComposeToolbar (useCase);

            if (useCase.ExportAsImage == null) {
                useCase.ExportAsImage = () => {
                    useCase.ExportAsImageImpl (useCase);
                };
            }

            useCase.About.ApplicationName = useCase.UseCaseTitle;
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

        protected virtual void ComposeMenu (ConceptUsecase useCase) {
            
            var menu = useCase.Menu;
            menu.Font = SystemFonts.Fonts.MenuFont;
            var l = new Localizer ();

            var menuItems = new MenuItem [] {
            new MenuItem (l["File"], null, null, new MenuItem[] {
                new MenuItem(l["Open ..."], null, (s, e) =>  useCase.OpenFile() ),
                new MenuItem(l["Save"], null, (s, e) => useCase.SaveFile() ),
                new MenuItem(l["SaveAs ..."], null, (s, e) =>  useCase.SaveAsFile()),
                new MenuItem(l["Export"], null,null, new MenuItem[] {
                    new MenuItem(l["current view ..."], null, (s, e) => useCase.ExportCurrentView()),
                    new MenuItem(l["view as image ..."], null, (s, e) => useCase.ExportAsImage?.Invoke()),
                    new MenuItem(l["Content ..."], null, (s, e) => useCase.ExportContent() ),
                    new MenuItem(l["Report (pdf)..."], null, (s, e) => useCase.ExportThings()),
                }),
                new MenuItem(l["Import"], null, null,new MenuItem[] {
                    new MenuItem(l["Content ..."], null, (s, e) => useCase.ImportContent()),
                    new MenuItem(l["multi content ..."], null, (s, e) =>  useCase.ImportGraphCursor()),
                    new MenuItem(l["File from previous version ..."], null, (s, e) => useCase.ImportThingGraphRaw())
                }),
                //TODO: new MenuItem(l["Print ..."], null, (s, e) => { this.Print(useCase); }),
                //TODO: new MenuItem(l["PrintPreview ..."], null, (s, e) => { this.PrintPreview(useCase); }),
                new MenuItem(l["Exit"], null, (s, e) => Application.Exit()),
            }),

            new MenuItem (l["Edit"], null, null, new MenuItem[] {
                    
                new MenuItem(l["Copy"], null, (s, e) => useCase.GetCurrentDisplay()?.ActionDispatcher?.Copy()),
                new MenuItem(l["Paste"], null, (s, e) => useCase.GetCurrentDisplay()?.ActionDispatcher?.Paste()),

                new MenuItem (l["Style"], null, null, new MenuItem[] {
                    useCase.ShowLayoutEditor != null ? new MenuItem(l["Layout"], null, (s, e) => useCase.ShowLayoutEditor()) : null,
                    useCase.ShowLayoutEditor != null ? new MenuItem(l["StyleSheet"], null, (s, e) =>  useCase.ShowStyleEditor()) : null,
                }),

#if DEBUG
                new MenuItem (l["Merge"], null, (s, e) => useCase.MergeVisual ()),
                new MenuItem (l["Revert link"], null, (s, e) => useCase.RevertEdges ()),
#endif
                new MenuItem (l["Search"], null, (s, e) => useCase.Search() ),
                }),

            new MenuItem (l["Favorites"], null, null, new MenuItem[] {
                new MenuItem(l["Add to favorites"], null, (s, e) =>
                     useCase.FavoriteManager.AddToFavorites(useCase.GetCurrentDisplay().Data)),
                new MenuItem(l["View on open "], null, (s, e) =>
                    useCase.FavoriteManager.SetAutoView(useCase.GetCurrentDisplay().Data)),
            }),

            new MenuItem (l["Help"], null, null, new MenuItem[] {
                useCase.ShowAboutWindow != null ? new MenuItem (l["About"], null, (s, e) => useCase.ShowAboutWindow ()): null,
            }),

            };
            menu.AddItems (menuItems.Where (i => i != null).ToArray());

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