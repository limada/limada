using Limada.View.Vidgets;
using Limaki.Usecases;
using Limaki.Usecases.Vidgets;
using Limaki.View.Visuals;

namespace Limada.UseCases {
    public interface IConceptUsecase {
        ISplitView SplitView { get;}
        VisualsDisplayHistory VisualsDisplayHistory { get;  }
        FavoriteManager FavoriteManager { get; }
        IGraphSceneUiManager GraphSceneUiManager { get;}
        ISheetManager SheetManager { get;  }
    }
}