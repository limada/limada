using Limaki.Common;
using Limaki.Drawing;
using Limaki.View;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Xwt;

namespace Limaki.Tests.View.Display {

    public class MoreVisualsDisplayTest : VisualsDisplayTest {
        public void SelectorVersusMulitSelectTest () {
            Display.SelectAction.Enabled = true;

            var factory = Registry.Pooled<IVisualFactory> ();
            var w = factory.CreateItem ("SelectorVersusMulitSelectTest");
            Scene.Add (w);
            Display.Layout.Reset ();
            Display.Perform ();

            var p = w.Shape[Anchor.LeftBottom];
            p = Display.Viewport.Camera.FromSource (p);
            var e = new MouseActionEventArgs (MouseActionButtons.Left, ModifierKeys.None, 1, p.X + 5, p.Y + 1, 0);

            Display.ActionDispatcher.OnMouseDown (e);
            //Display.EventControler.OnMouseUp(e);
            DoEvents ();
            e = new MouseActionEventArgs (MouseActionButtons.None, ModifierKeys.None, 1, p.X + 5, p.Y + 2, 0);
            Display.ActionDispatcher.OnMouseMove (e);
            DoEvents ();

            e = new MouseActionEventArgs (MouseActionButtons.Left, ModifierKeys.None, 1, p.X + 5, p.Y + 3, 0);
            Display.ActionDispatcher.OnMouseDown (e);
            Display.ActionDispatcher.OnMouseMove (e);

            for (int i = 0; i < 100; i++) {
                e = new MouseActionEventArgs (MouseActionButtons.Left, ModifierKeys.None, 1, e.Location.X + 1, e.Location.Y + 1, 0);
                Display.ActionDispatcher.OnMouseMove (e);
            }


            //Display.EventControler.OnMouseUp(e);
        }
    }
}