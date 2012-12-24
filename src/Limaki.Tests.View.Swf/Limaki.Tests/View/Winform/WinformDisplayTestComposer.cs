using Limaki.Tests.View.Display;
using Limaki.View.Visualizers;
using Limaki.View;
using Limaki.Common;
using System;

namespace Limaki.Tests.View.Winform {
    public class WinformDisplayTestComposer<T> : IComposer<DisplayTest<T>>
        where T : class {
        public ITestDevice TestDevice { get; set; }
        public Func<IDisplay<T>> Factory { get; set; }
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