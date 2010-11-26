using System.Drawing;
using Limaki.Winform;
using Limaki.Widgets;
namespace Limaki.Winform.Displays {
    public class ImageDisplay : DisplayBase<Image> {

		DisplayKit<Image> _kit = null;
        public override DisplayKit<Image>
            displayKit {
            get {
                if (_kit == null) {
                    _kit = new ImageKit();

                }
                return _kit;
            }
        }
    }
}
