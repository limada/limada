using System.Windows.Forms;
using Limaki.View.Swf;
using Limaki.Usecases.Concept;
using Limaki.Swf.Backends.UseCases;
using Limaki.Common.IOC;
using Limaki.Common;
using Xwt.WinformBackend;
using System;
using Limaki.Usecases;

namespace Limaki.App {

    public class WinformAppFactory : AppFactory<global::Limada.Usecases.AppResourceLoader> {

        public WinformAppFactory(): base(new SwfContextRecourceLoader()) {}

        public Form MainForm() {
            var result = new Form ();
            
           
            CreateUseCase (result);
            
            return result;
        }

        public void CreateUseCase(Form mainform) {

            mainform.Icon = Limaki.View.Properties.Iconery.LimadaLogo;
            mainform.ClientSize = new System.Drawing.Size(800, 600);

            var deviceComposer = new SwfUseCaseComposer();
            deviceComposer.Mainform = mainform;

            var factory = new UsecaseFactory<ConceptUsecase>();
            factory.Composer = new ConceptUsecaseComposer();
            factory.DeviceComposer = deviceComposer;
            
            var useCase = factory.Create();
            factory.Compose(useCase);

            CallPlugins(factory, useCase);
            
            useCase.Start();

            if (useCase.ApplicationQuitted) {
                Application.Exit();
                Environment.Exit(0);
            }
        }

        public void CallPlugins(UsecaseFactory<ConceptUsecase> factory, ConceptUsecase useCase) {
            var factories = Registry.Pool.TryGetCreate < UsecaseFactories<ConceptUsecase>>();
            foreach(var item in factories) {
                item.Composer = factory.Composer;
                item.DeviceComposer = factory.DeviceComposer;
                item.Compose(useCase);
            }
        }
    }
    
}