using Limada.View.Vidgets;
using Limaki.Common;
using Limaki.Usecases;
using Limaki.Usecases.Vidgets;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Xwt;
using System;

namespace Limada.UseCases {
	
    public interface IConceptUsecase {
        ISplitView SplitView { get;}
        VisualsDisplayHistory VisualsDisplayHistory { get;  }
        FavoriteManager FavoriteManager { get; }
        IGraphSceneUiManager GraphSceneUiManager { get;}
        ISheetManager SheetManager { get;  }
    }

	public interface IBackendConceptUseCaseComposer : IComposer<ConceptUsecase> {
		Vindow MainWindow { get; set; }
		Size WindowSize { get; set; }
		Action OnShow { get; set; }
	}
}