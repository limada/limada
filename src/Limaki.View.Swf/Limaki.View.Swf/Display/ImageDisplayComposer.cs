using System.Drawing;
using Limaki.Drawing.Gdi;
using Limaki.View.Display;
using Limaki.View.UI;
using Point = Xwt.Point;
using Size = Xwt.Size;
using Xwt.Gdi.Backend;

namespace Limaki.View.Swf {

    public class ImageDisplayComposer: DisplayComposer<Image> {

        public override void Compose(Display<Image> display) {
            
            this.DataOrigin = () => { return Point.Zero; };
            this.DataSize = () => {
                var data = display.Data;
                if (data != null)
                    return data.Size.ToXwt ();
                else
                    return Size.Zero;
            };

            base.Compose(display);

            Compose(display, display.SelectionRenderer);
            Compose(display, display.MoveResizeRenderer);

            var selectAction = new SelectorAction();
            Compose(display, selectAction, true);
            selectAction.ShowGrips = false;
            selectAction.Enabled = false;

            display.SelectAction = selectAction;
            display.EventControler.Add(selectAction);

            display.SelectAction.ShowGrips = true;
            display.SelectAction.Enabled = false;
            display.MouseScrollAction.Enabled = true;
            
        }
    }
}