using Limaki.View;

namespace Limaki.Tests.View.Display {
    public interface ITestDevice {
        object CreateForm ( IDisplay display );
        object FindForm ( IDisplay display );
        void DoEvents();
    }

}