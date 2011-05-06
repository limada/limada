using System.Windows.Forms;
using Limaki.Presenter.Winform;
using Limaki.Tests.UseCases;
using Limaki.UseCases;
using Limaki.UseCases.Winform;
using Limaki.Common.IOC;

namespace Limaki.App {
    public class WinformAppFactory : AppFactory<Limada.UseCases.AppResourceLoader> {
        public WinformAppFactory(): base(new WinformContextRecourceLoader()) {}

        public Form MainForm() {
            var result = new Form ();

            CreateUseCase (result);
            
            return result;
        }

        
        public void CreateUseCase(Form mainform) {
            mainform.Icon = Limaki.Presenter.Properties.Resources.LimadaLogoA;
            mainform.ClientSize = new System.Drawing.Size(632, 406);

            var deviceInstrumenter = new UseCaseWinformComposer();
            deviceInstrumenter.Mainform = mainform;

            var factory = new UseCaseFactory<UseCase>();
            factory.Composer = new UseCaseComposer();
            factory.DeviceComposer = deviceInstrumenter;
            var useCase = factory.Create();

            factory.Instrument(useCase);

            var testCases = new TestCases();
            testCases.testMessage = (s, m) => {
                                        deviceInstrumenter.StatusLabel.Text = m;
                                        Application.DoEvents();
                                    };
            testCases.CreateTestCases(useCase, deviceInstrumenter);

            useCase.Start();
        }
    }
}