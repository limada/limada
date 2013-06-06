using System.Drawing;
using Limaki.Drawing.Gdi;
using Limaki.View.Visualizers;
using Limaki.View.UI;
using Point = Xwt.Point;
using Size = Xwt.Size;
using Xwt.Gdi.Backend;
using Xwt.Backends;

namespace Limaki.View.Swf.Visualizers {

    [BackendType(typeof(IImageDisplayBackend))]
    public class ImageDisplay:Display<Image> {}
    
    public interface IImageDisplayBackend : IDisplayBackend<Image> { }
    
    public class ImageDisplayFactory:DisplayFactory<Image> {
        public override Display<Image> Create () {
            return new ImageDisplay();
        }
    }

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