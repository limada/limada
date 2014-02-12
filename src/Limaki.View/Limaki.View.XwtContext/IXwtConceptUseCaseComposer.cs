using Limaki.Usecases.Concept;
using Limaki.View.Visualizers;
using System;
using Xwt;

namespace Limaki.View.XwtContext {

    public interface IXwtConceptUseCaseComposer : IComposer<ConceptUsecase> {
        Window MainWindow { get; set; }
        Menu Menu { get; set; }
        Label StatusLabel { get; set; }
        Size WindowSize { get; set; }
        Action OnShow { get; set; }
    }
}