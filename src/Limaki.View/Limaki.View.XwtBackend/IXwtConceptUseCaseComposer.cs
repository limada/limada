using Limada.Usecases;
using Limada.UseCases;
using Limaki.Common;
using Limaki.View.Vidgets;
using System;
using Limaki.View.Viz.Visualizers;
using Xwt;

namespace Limaki.View.XwtBackend {

    public interface IXwtConceptUseCaseComposer : IComposer<ConceptUsecase> {
        IVindowBackend MainWindowBackend { get; set; }
        Menu Menu { get; set; }
        Label StatusLabel { get; set; }
        Size WindowSize { get; set; }
        Action OnShow { get; set; }
    }
}