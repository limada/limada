using Limaki.View;
using System;

namespace Limaki.Tests.View.Display {
    [Obsolete]
    public interface ITestDevice {
        object CreateForm ( IDisplay display );
        object FindForm ( IDisplay display );
        void DoEvents();
    }

}