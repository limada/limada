using Limaki.Presenter.Display;
using Limaki.Tests.Presenter.Display;
using Limaki.Presenter;
using Limaki.Common;

namespace Limaki.Tests.Presenter.Winform {
    public class WinformDisplayTestComposer<T> : IComposer<DisplayTest<T>>
        where T : class {
        public ITestDevice TestDevice { get; set; }
        public Get<IDisplay<T>> Factory { get; set; }
        public void Factor(DisplayTest<T> displayTest) {
            TestDevice = new WinformTestDevice<T>();
            Factory = () => Registry.Factory.Create<IDisplay<T>>();
        }

        public void Compose(DisplayTest<T> displayTest) {
            displayTest.TestDevice = this.TestDevice;
            displayTest.Factory = this.Factory;
        }



    }
}