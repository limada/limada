using Limada.Usecases;
using Limada.UseCases;
using Limaki.Common;
using Limaki.View.Vidgets;
using System;
using Limaki.View.Viz.Visualizers;
using Xwt;

namespace Limaki.View.XwtBackend {

    public interface IXwtBackendConceptUseCaseComposer : IBackendConceptUseCaseComposer {
		
        IVindowBackend MainWindowBackend { get; set; }
        Label StatusLabel { get; set; }

    }
}