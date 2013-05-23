using Limaki.Common;
using Limada.View;
using Limada.Usecases;
using Limaki.View.Visualizers;
using Limaki.Viewers;
using Limaki.Visuals;

namespace Limaki.Usecases.Concept {

    public class ConceptUsecaseComposer : IComposer<ConceptUsecase> {

        public void Factor(ConceptUsecase useCase) {
            useCase.SheetManager = Registry.Factory.Create<ISheetManager>();
            useCase.SceneHistory = new SceneHistory ();
            useCase.FileManager = new FileManager ();
            useCase.FileManager.OpenFileDialog = new FileDialogMemento();
            useCase.FileManager.SaveFileDialog = new FileDialogMemento();
            useCase.ContentStreamUiManager = new ContentStreamUiManager();
            useCase.ContentStreamUiManager.OpenFileDialog = new FileDialogMemento();
            useCase.ContentStreamUiManager.SaveFileDialog = new FileDialogMemento();

            useCase.FavoriteManager = new FavoriteManager();
        }

        public void Compose(ConceptUsecase useCase) {
            
            var splitView = useCase.SplitView;
            useCase.GetCurrentDisplay = () => splitView.CurrentDisplay;
            useCase.GetCurrentControl = () => splitView.CurrentWidget;

            useCase.SheetManager.SheetRegistered = sceneInfo => {
                useCase.SceneHistory.Store(sceneInfo);
                //useCase.SplitViewToolStrip.Attach(splitView.CurrentDisplay);
            };
            splitView.SceneHistory = useCase.SceneHistory;
            splitView.SheetManager = useCase.SheetManager;
            
            splitView.FavoriteManager = useCase.FavoriteManager;
            useCase.FavoriteManager.SheetManager = useCase.SheetManager;

            useCase.SplitViewToolStrip.SplitView = useCase.SplitView;
            useCase.SplitViewToolStrip.SheetManager = useCase.SheetManager;

            splitView.CurrentWidgetChanged += c => useCase.DisplayToolStrip.Attach(c);
            splitView.CurrentWidgetChanged += c => useCase.LayoutToolController.Attach(c);
            splitView.CurrentWidgetChanged += c => useCase.MarkerToolStrip.Attach(c);
            splitView.CurrentWidgetChanged += c => useCase.SplitViewToolStrip.Attach(c);
            splitView.CurrentWidgetChanged += c => useCase.ArrangerToolStrip.Attach(c);
            
            useCase.DisplayStyleChanged += splitView.DoDisplayStyleChanged;

            var fileManager = useCase.FileManager;
            fileManager.FileDialogShow = useCase.FileDialogShow;
            fileManager.MessageBoxShow = useCase.MessageBoxShow;

            fileManager.DataBound = scene => splitView.ChangeData(scene);
            fileManager.DataPostProcess = useCase.DataPostProcess;

            fileManager.Progress = useCase.Progress;
            fileManager.ApplicationQuit = useCase.ApplicationQuit;
            
            splitView.Check ();
			
            var streamManager = useCase.ContentStreamUiManager;
            streamManager.FileDialogShow = useCase.FileDialogShow;
            streamManager.MessageBoxShow = useCase.MessageBoxShow;
            
        }
    }
}