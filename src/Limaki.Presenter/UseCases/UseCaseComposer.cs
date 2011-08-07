using Limada.Presenter;
using Limada.UseCases;
using Limaki.Common;
using Limaki.Presenter.Display;
using Limaki.UseCases.Viewers;
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
            useCase.GetCurrentControl = () => splitView.CurrentControl;

            useCase.SheetManager.SheetRegistered = i => useCase.SceneHistory.Store(i);
            splitView.SceneHistory = useCase.SceneHistory;
            splitView.SheetManager = useCase.SheetManager;
            
            splitView.FavoriteManager = useCase.FavoriteManager;
            useCase.FavoriteManager.SheetManager = useCase.SheetManager;

            splitView.CurrentControlChanged += (c) => useCase.DisplayToolController.Attach(c);
            splitView.CurrentControlChanged += (c) => useCase.LayoutToolController.Attach(c);
            splitView.CurrentControlChanged += (c) => useCase.MarkerToolController.Attach(c);

            useCase.DisplayStyleChanged += splitView.DoDisplayStyleChanged;

            var fileManager = useCase.FileManager;
            fileManager.FileDialogShow = useCase.FileDialogShow;
            fileManager.MessageBoxShow = useCase.MessageBoxShow;

            fileManager.DataBound = (scene) => splitView.ChangeData(scene);
            fileManager.DataPostProcess = useCase.DataPostProcess;

            fileManager.StateMessage = useCase.StateMessage;
            
            splitView.Check ();
			
            var streamManager = useCase.ContentProviderManager;
            streamManager.FileDialogShow = useCase.FileDialogShow;
            streamManager.MessageBoxShow = useCase.MessageBoxShow;
            streamManager.Import = useCase.ImportContent;
            streamManager.Export = useCase.ExtractContent;

            useCase.AlignTools = new AlignTools();
        }
    }
}