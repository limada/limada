using System.Windows.Forms;
using Limada.Usecases;
using Limaki.View.Swf;
using Limaki.Swf.Backends.UseCases;
using Limaki.Common.IOC;
using Limaki.Common;
using Xwt.WinformBackend;
using System;
using Limaki.Usecases;
using Limaki.View;

namespace Limaki.App {

    public class WinformAppFactory : AppFactory<global::Limada.Usecases.AppResourceLoader> {

        public WinformAppFactory(): base(new SwfContextResourceLoader()) {}

        public Form MainForm() {
            var result = new Limaki.View.Swf.Backends.VindowBackend ();
            
           
            CreateUseCase (result);
            
            return result;
        }

        public void CreateUseCase(IVindowBackend vindowBackend) {
            var mainform = vindowBackend as Form;

            mainform.Icon = Limaki.View.Properties.GdiIconery.LimadaLogo;
            mainform.ClientSize = new System.Drawing.Size(800, 600);

            var backendComposer = new SwfConceptUseCaseComposer();
            backendComposer.MainWindowBackend = vindowBackend;

            var factory = new UsecaseFactory<ConceptUsecase>();
            factory.Composer = new ConceptUsecaseComposer();
            factory.BackendComposer = backendComposer;
            
            var useCase = factory.Create();
            factory.Compose(useCase);

            CallPlugins(factory, useCase);
            
            useCase.Start();

            if (useCase.ApplicationQuitted) {
                Application.Exit();
                Environment.Exit(0);
            }
        }

        public void CallPlugins (UsecaseFactory<ConceptUsecase> factory, ConceptUsecase useCase) {
            var factories = Registry.Pooled<UsecaseFactories<ConceptUsecase>> ();
            foreach (var item in factories) {
                item.Composer = factory.Composer;
                item.BackendComposer = factory.BackendComposer;
                item.Compose (useCase);
            }
        }
    }
    
}