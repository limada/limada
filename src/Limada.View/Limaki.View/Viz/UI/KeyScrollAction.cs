using Limaki.Common;
using Limaki.View.Vidgets;
using Xwt;
using System;

namespace Limaki.View.Viz.UI {
    /// <summary>
    /// Scrolls with cursor keys
    /// </summary>
    public class KeyScrollAction : KeyActionBase, ICheckable {
        public virtual Func<IViewport> Viewport { get; set; }
        public virtual Func<ICursorHandler> CursorGetter { get; set; }

        public override void OnKeyPressed(KeyActionEventArgs e) {
            ProcessKey(e);
        }

        protected virtual Rectangle ProcessKey(KeyActionEventArgs e) {
            base.OnKeyPressed(e);
            var viewport = Viewport();
            var x = 0d;
            var y = 0d;
            if (e.Key == Key.Down && e.Modifiers == ModifierKeys.None) {
                y = 1;
            }
            if (e.Key == Key.Up && e.Modifiers == ModifierKeys.None) {
                y = -1;
            }
            if (e.Key == Key.Left && e.Modifiers == ModifierKeys.None) {
                x = -1;
            }
            if (e.Key == Key.Right && e.Modifiers == ModifierKeys.None) {
                x = 1;
            }
            if (x != 0d || y != 0d) {
                var pos = viewport.ClipOrigin;
                var size = viewport.ClipSize;
                var bounds = new Rectangle(viewport.DataOrigin, new Size((int)viewport.DataSize.Width,(int)viewport.DataSize.Height));

                var result = Rectangle.FromLTRB(
                    x == -1 && pos.X == bounds.X ? -1 : 0,
                    y == -1 && pos.Y == bounds.Y ? -1 : 0,
                    x == 1 && pos.X + size.Width >= bounds.Width ? 1 : 0,
                    y == 1 && pos.Y + size.Height >= bounds.Height ? 1 : 0
                    );
                x = pos.X + x * (size.Width / 10);
                y = pos.Y + y * (size.Height / 10);

                if (x < bounds.Left)
                    x = bounds.Left;
                if (x > bounds.Right)
                    x = bounds.Right - size.Width;
                if (y < bounds.Top)
                    y = bounds.Top;
                if (y > bounds.Bottom)
                    y = bounds.Bottom - size.Height;

                pos = new Point(x, y);
                viewport.ClipOrigin = pos;
                viewport.Update();
                return result;
            } else {
                return new Rectangle();
            }
        }

        #region ICheckable Member

        public virtual bool Check() {
            if (this.Viewport == null) {
                throw new CheckFailedException(this.GetType(), typeof(IViewport));
            }
            return true;
        }

        #endregion

    }
}