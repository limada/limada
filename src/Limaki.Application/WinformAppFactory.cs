using System.Windows.Forms;
using Limaki.View.Swf;
using Limaki.UseCases;
using Limaki.Swf.Backends.UseCases;
using Limaki.Common.IOC;
using Limaki.Common;
using Xwt.WinformBackend;

namespace Limaki.App {

    public class WinformAppFactory : AppFactory<global::Limada.UseCases.AppResourceLoader> {

        public WinformAppFactory(): base(new SwfContextRecourceLoader()) {}

        public Form MainForm() {
            var result = new Form ();
            
           
            CreateUseCase (result);
            
            return result;
        }

        public void CreateUseCase(Form mainform) {

            mainform.Icon = Limaki.View.Properties.Resources.LimadaLogo;
            mainform.ClientSize = new System.Drawing.Size(800, 600);

            var deviceComposer = new SwfUseCaseComposer();
            deviceComposer.Mainform = mainform;

            var factory = new UseCaseFactory<UseCase>();
            factory.Composer = new UseCaseComposer();
            factory.DeviceComposer = deviceComposer;
            
            var useCase = factory.Create();
            factory.Compose(useCase);

            CallPlugins(factory, useCase);

            useCase.Start();
        }

        public void CallPlugins(UseCaseFactory<UseCase> factory, UseCase useCase) {
            var factories = Registry.Pool.TryGetCreate < UseCaseFactories<UseCase>>();
            foreach(var item in factories) {
                item.Composer = factory.Composer;
                item.DeviceComposer = factory.DeviceComposer;
                item.Compose(useCase);
            }
        }
    }
    
}