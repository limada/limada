using Limaki.Common;
using Limada.View;
using Limada.UseCases;
using Limaki.View.Visualizers;
using Limaki.Viewers;
using Limaki.Visuals;

namespace Limaki.UseCases {
    public class UseCaseComposer : IComposer<UseCase> {
        public void Factor(UseCase useCase) {
            useCase.SheetManager = Registry.Factory.Create<ISheetManager>();
            useCase.SceneHistory = new SceneHistory ();
            useCase.FileManager = new FileManager ();
            useCase.FileManager.OpenFileDialog = new FileDialogMemento();
            useCase.FileManager.SaveFileDialog = new FileDialogMemento();
            useCase.ContentProviderManager = new ContentProviderManager();
            useCase.ContentProviderManager.OpenFileDialog = new FileDialogMemento();
            useCase.ContentProviderManager.SaveFileDialog = new FileDialogMemento();

            useCase.FavoriteManager = new FavoriteManager();
        }

        public void Compose(UseCase useCase) {
            
            var splitView = useCase.SplitView;
            useCase.GetCurrentDisplay = () => splitView.CurrentDisplay;
            useCase.GetCurrentControl = () => splitView.CurrentWidget;

            useCase.SheetManager.SheetRegistered = sceneInfo => {
                useCase.SceneHistory.Store(sceneInfo);
                //useCase.SplitViewToolController.Attach(splitView.CurrentDisplay);
            };
            splitView.SceneHistory = useCase.SceneHistory;
            splitView.SheetManager = useCase.SheetManager;
            
            splitView.FavoriteManager = useCase.FavoriteManager;
            useCase.FavoriteManager.SheetManager = useCase.SheetManager;

            useCase.SplitViewToolController.SplitView = useCase.SplitView;
            useCase.SplitViewToolController.SheetManager = useCase.SheetManager;

            splitView.CurrentWidgetChanged += (c) => useCase.DisplayToolController.Attach(c);
            splitView.CurrentWidgetChanged += (c) => useCase.LayoutToolController.Attach(c);
            splitView.CurrentWidgetChanged += (c) => useCase.MarkerToolController.Attach(c);
            splitView.CurrentWidgetChanged += (c) => useCase.SplitViewToolController.Attach(c);
            splitView.CurrentWidgetChanged += (c) => useCase.ArrangerToolController.Attach(c);
            
            useCase.DisplayStyleChanged += splitView.DoDisplayStyleChanged;

            var fileManager = useCase.FileManager;
            fileManager.FileDialogShow = useCase.FileDialogShow;
            fileManager.MessageBoxShow = useCase.MessageBoxShow;

            fileManager.DataBound = (scene) => splitView.ChangeData(scene);
            fileManager.DataPostProcess = useCase.DataPostProcess;

            fileManager.Progress = useCase.Progress;
            fileManager.ApplicationQuit = useCase.ApplicationQuit;
            
            splitView.Check ();
			
            var streamManager = useCase.ContentProviderManager;
            streamManager.FileDialogShow = useCase.FileDialogShow;
            streamManager.MessageBoxShow = useCase.MessageBoxShow;
            streamManager.Import = useCase.ImportContent;
            streamManager.Export = useCase.ExtractContent;

            
        }
    }
}