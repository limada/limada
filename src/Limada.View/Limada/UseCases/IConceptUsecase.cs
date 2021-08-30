using Limada.View.Vidgets;
using Limaki.Common;
using Limaki.Usecases;
using Limaki.Usecases.Vidgets;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Xwt;
using System;

namespace Limada.Usecases {
	
    public interface IConceptUsecase {
        ISplitView SplitView { get;}
        VisualsDisplayHistory VisualsDisplayHistory { get;  }
        IFavoriteManager FavoriteManager { get; }
        IGraphSceneUiManager GraphSceneUiManager { get;}
        IVisualSceneStoreInteractor SceneManager { get; }
    }

	public interface IBackendConceptUseCaseComposer : IComposer<ConceptUsecase> {
		IVindow MainWindow { get; set; }
		Action OnShow { get; set; }
        Action FinalizeCompose { get; set; }
    }
}