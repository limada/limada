using Limaki.Tests.View.Display;
using Limaki.View.Visualizers;
using Limaki.View;
using Limaki.Common;

namespace Limaki.Tests.View.Winform {
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